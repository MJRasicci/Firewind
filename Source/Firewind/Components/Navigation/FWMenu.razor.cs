namespace Firewind.Components;

using Firewind.Base;
using Firewind.Data;
using Firewind.Variant;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Renders a menu that supports both static content and dynamic data-driven rendering.
/// </summary>
/// <typeparam name="TMenuItem">The type of source items used by dynamic menu rendering.</typeparam>
public partial class FWMenu<TMenuItem> : FirewindComponentBase, IDisposable
{
    private readonly Lock dataLock = new();
    private readonly CancellationTokenSource disposeTokenSource = new();
    private List<TMenuItem> items = [];
    private List<IDataTreeNode<TMenuItem>> treeRootNodes = [];
    private IDataSource<TMenuItem>? subscribedDataSource;
    private IDataTree<TMenuItem>? previousDataTree;
    private Func<IEnumerable<TMenuItem>, IDataTree<TMenuItem>>? previousDataTreeFactory;
    private bool shouldBindData = true;
    private bool IsTreeRenderingEnabled { get; set; }
    private bool isDisposed;

    /// <summary>
    /// Gets or sets the menu orientation variant.
    /// </summary>
    [Parameter]
    public MenuOrientation Orientation { get; set; }

    /// <summary>
    /// Gets or sets the menu size variant.
    /// </summary>
    [Parameter]
    public MenuSize Size { get; set; }

    /// <summary>
    /// Gets or sets optional free-form menu content rendered before dynamic content.
    /// </summary>
    /// <remarks>
    /// This enables static menu composition even when no data source is supplied.
    /// </remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the optional theme color used for the menu surface.
    /// </summary>
    [Parameter]
    public ThemeColor Color { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether glow classes are applied based on <see cref="Color"/>.
    /// </summary>
    [Parameter]
    public bool Glow { get; set; }

    /// <summary>
    /// Gets or sets the dynamic source used when rendering flat or tree-based data.
    /// </summary>
    [Parameter]
    public IDataSource<TMenuItem>? DataSource { get; set; }

    /// <summary>
    /// Gets or sets an explicit tree for dynamic menu rendering.
    /// </summary>
    /// <remarks>
    /// When provided, this tree is rendered directly and <see cref="DataSource"/> is not queried.
    /// </remarks>
    [Parameter]
    public IDataTree<TMenuItem>? DataTree { get; set; }

    /// <summary>
    /// Gets or sets a factory used to project fetched data into a tree representation.
    /// </summary>
    /// <remarks>
    /// This is used only when <see cref="DataTree"/> is <see langword="null"/> and
    /// <see cref="DataSource"/> is provided.
    /// </remarks>
    [Parameter]
    public Func<IEnumerable<TMenuItem>, IDataTree<TMenuItem>>? DataTreeFactory { get; set; }

    /// <summary>
    /// Gets or sets the template used for flat data item rendering.
    /// </summary>
    [Parameter]
    public RenderFragment<TMenuItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Gets or sets a generic node template used when no more specific tree template applies.
    /// </summary>
    [Parameter]
    public RenderFragment<IDataTreeNode<TMenuItem>>? TreeNodeTemplate { get; set; }

    /// <summary>
    /// Gets or sets the template used for <see cref="DataTreeNodeKind.Header"/> nodes.
    /// </summary>
    [Parameter]
    public RenderFragment<IDataTreeNode<TMenuItem>>? TreeHeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets the template used for non-header nodes that have children.
    /// </summary>
    [Parameter]
    public RenderFragment<IDataTreeNode<TMenuItem>>? TreeBranchTemplate { get; set; }

    /// <summary>
    /// Gets or sets the template used for non-header terminal nodes.
    /// </summary>
    [Parameter]
    public RenderFragment<IDataTreeNode<TMenuItem>>? TreeLeafTemplate { get; set; }

    /// <summary>
    /// Gets or sets templates keyed by zero-based tree level.
    /// </summary>
    /// <remarks>
    /// Level templates are evaluated after <see cref="TreeTemplateSelector"/> and before
    /// node-kind templates.
    /// </remarks>
    [Parameter]
    public IReadOnlyDictionary<int, RenderFragment<IDataTreeNode<TMenuItem>>>? TreeLevelTemplates { get; set; }

    /// <summary>
    /// Gets or sets a callback that can return a per-node template override.
    /// </summary>
    [Parameter]
    public Func<IDataTreeNode<TMenuItem>, RenderFragment<IDataTreeNode<TMenuItem>>?>? TreeTemplateSelector { get; set; }

    /// <summary>
    /// Gets the items used by flat dynamic rendering.
    /// </summary>
    protected IReadOnlyList<TMenuItem> Items
    {
        get
        {
            lock (this.dataLock)
            {
                return [.. this.items];
            }
        }
    }

    /// <summary>
    /// Gets the root nodes used by tree dynamic rendering.
    /// </summary>
    protected IReadOnlyList<IDataTreeNode<TMenuItem>> TreeRootNodes
    {
        get
        {
            lock (this.dataLock)
            {
                return [.. this.treeRootNodes];
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether dynamic rendering currently uses tree mode.
    /// </summary>
    protected bool UseTreeRendering
    {
        get
        {
            lock (this.dataLock)
            {
                return this.IsTreeRenderingEnabled;
            }
        }
    }

    /// <summary>
    /// Gets the root CSS class for the menu component.
    /// </summary>
    protected override string CssClass => new VariantClassBuilder("fw-menu")
        .AddVariant(this.Orientation, static orientation => orientation.ClassNames())
        .AddVariant(this.Size, static size => size.ClassNames())
        .AddVariant(this.Color, static color => color.BackgroundClasses())
        .AddIf(this.Glow, this.Color.GlowClasses())
        .Build();

    /// <summary>
    /// Ensures menu classes and dynamic bindings are synchronized with the latest parameters.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        EnsureNoWrapStyle();
        UpdateDataSourceSubscription();
        UpdateTreeInputs();
    }

    /// <summary>
    /// Refreshes dynamic data when relevant menu parameters have changed.
    /// </summary>
    /// <returns>A task representing asynchronous parameter processing.</returns>
    protected override async Task OnParametersSetAsync()
    {
        if (this.shouldBindData)
        {
            this.shouldBindData = false;
            await BindDataAsync(this.disposeTokenSource.Token);
        }

        await base.OnParametersSetAsync();
    }

    /// <summary>
    /// Binds dynamic menu data from <see cref="DataTree"/>, <see cref="DataTreeFactory"/>, or <see cref="DataSource"/>.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
    /// <returns>A task that represents the asynchronous bind operation.</returns>
    public async Task BindDataAsync(CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(this.isDisposed, this);

        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(
            cancellationToken,
            this.disposeTokenSource.Token);

        if (this.DataTree is not null)
        {
            SetTreeNodes(this.DataTree.RootNodes);
            return;
        }

        if (this.DataSource is null)
        {
            SetFlatItems([], useTreeRendering: false);
            return;
        }

        var sourceItems = await this.DataSource.FetchDataAsync(linkedTokenSource.Token);
        var itemSnapshot = sourceItems as IReadOnlyList<TMenuItem> ?? [.. sourceItems];

        if (this.DataTreeFactory is not null)
        {
            var dataTree = this.DataTreeFactory(itemSnapshot) ??
                throw new InvalidOperationException("DataTreeFactory returned null.");
            SetTreeNodes(dataTree.RootNodes);
        }
        else
        {
            SetFlatItems(itemSnapshot, useTreeRendering: false);
        }
    }

    /// <summary>
    /// Releases resources used by the component and detaches data source subscriptions.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Resolves the template used for rendering a specific tree node.
    /// </summary>
    /// <param name="node">The node being rendered.</param>
    /// <returns>The chosen template, or <see langword="null"/> when default rendering should be used.</returns>
    protected RenderFragment<IDataTreeNode<TMenuItem>>? ResolveNodeTemplate(IDataTreeNode<TMenuItem> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        var selectorTemplate = this.TreeTemplateSelector?.Invoke(node);
        if (selectorTemplate is not null)
        {
            return selectorTemplate;
        }

        if (this.TreeLevelTemplates is not null &&
            this.TreeLevelTemplates.TryGetValue(node.Level, out var levelTemplate))
        {
            return levelTemplate;
        }

        if (node.Kind == DataTreeNodeKind.Header && this.TreeHeaderTemplate is not null)
        {
            return this.TreeHeaderTemplate;
        }

        if (node.Children.Count > 0 && this.TreeBranchTemplate is not null)
        {
            return this.TreeBranchTemplate;
        }

        if (node.Children.Count == 0 && this.TreeLeafTemplate is not null)
        {
            return this.TreeLeafTemplate;
        }

        return this.TreeNodeTemplate;
    }

    /// <summary>
    /// Determines whether a node should render as a non-interactive title row.
    /// </summary>
    /// <param name="node">The node being rendered.</param>
    /// <returns>
    /// <see langword="true"/> for header nodes and non-collapsible branch nodes; otherwise <see langword="false"/>.
    /// </returns>
    protected bool ShouldRenderNodeAsTitle(IDataTreeNode<TMenuItem> node)
    {
        ArgumentNullException.ThrowIfNull(node);
        return node.Kind == DataTreeNodeKind.Header || (node.Children.Count > 0 && !node.IsCollapsible);
    }

    /// <summary>
    /// Resolves the display label for a tree node when default rendering is used.
    /// </summary>
    /// <param name="node">The node being rendered.</param>
    /// <returns>A display label for the node.</returns>
    protected string ResolveNodeLabel(IDataTreeNode<TMenuItem> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (TryGetMetadataValue(node, MenuTreeMetadataKeys.Label, out string? label) &&
            !string.IsNullOrWhiteSpace(label))
        {
            return label;
        }

        return node.DataItem?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// Resolves the menu action element tag for a tree node when default rendering is used.
    /// </summary>
    /// <param name="node">The node being rendered.</param>
    /// <returns>The action element tag name.</returns>
    protected string ResolveNodeActionElement(IDataTreeNode<TMenuItem> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        if (TryGetMetadataValue(node, MenuTreeMetadataKeys.ActionElement, out string? actionElement) &&
            !string.IsNullOrWhiteSpace(actionElement))
        {
            return actionElement;
        }

        return "a";
    }

    /// <summary>
    /// Resolves the node href value for default rendering.
    /// </summary>
    /// <param name="node">The node being rendered.</param>
    /// <returns>The href value, or <see langword="null"/>.</returns>
    protected string? ResolveNodeHref(IDataTreeNode<TMenuItem> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        return TryGetMetadataValue(node, MenuTreeMetadataKeys.Href, out string? href)
            ? href
            : null;
    }

    /// <summary>
    /// Resolves menu behavior flags for default node rendering.
    /// </summary>
    /// <param name="node">The node being rendered.</param>
    /// <returns>Menu behavior flags for the node.</returns>
    protected MenuItemBehavior ResolveNodeBehavior(IDataTreeNode<TMenuItem> node)
    {
        ArgumentNullException.ThrowIfNull(node);

        return TryGetMetadataValue(node, MenuTreeMetadataKeys.Behavior, out MenuItemBehavior behavior)
            ? behavior
            : MenuItemBehavior.None;
    }

    /// <summary>
    /// Releases managed resources when <paramref name="disposing"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="disposing">
    /// <see langword="true"/> to dispose managed resources; otherwise <see langword="false"/>.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.isDisposed)
        {
            return;
        }

        this.isDisposed = true;
        if (!disposing)
        {
            return;
        }

        this.disposeTokenSource.Cancel();
        this.subscribedDataSource?.DataChanged -= this.OnDataChanged;
        this.disposeTokenSource.Dispose();
    }

    /// <summary>
    /// Responds to data source change notifications by refreshing dynamic menu data.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">Event arguments.</param>
    private void OnDataChanged(object? sender, EventArgs e)
    {
        if (this.isDisposed)
        {
            return;
        }

        this.shouldBindData = true;
        _ = HandleDataChangedAsync();
    }

    /// <summary>
    /// Rebinds data after a <see cref="IDataSource{TMenuItem}.DataChanged"/> event.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [SuppressMessage(
        "Design",
        "CA1031:Do not catch general exception types",
        Justification = "Exceptions are intentionally dispatched to Blazor via DispatchExceptionAsync.")]
    private async Task HandleDataChangedAsync()
    {
        try
        {
            await InvokeAsync(async () =>
            {
                await BindDataAsync(this.disposeTokenSource.Token);
                StateHasChanged();
            });
        }
        catch (OperationCanceledException) when (this.disposeTokenSource.IsCancellationRequested)
        {
            // The component is disposing. Ignore cancellation from teardown.
        }
        catch (Exception ex)
        {
            await DispatchExceptionAsync(ex);
        }
    }

    /// <summary>
    /// Synchronizes data source event subscriptions.
    /// </summary>
    private void UpdateDataSourceSubscription()
    {
        if (ReferenceEquals(this.subscribedDataSource, this.DataSource))
        {
            return;
        }

        this.subscribedDataSource?.DataChanged -= this.OnDataChanged;
        this.subscribedDataSource = this.DataSource;
        this.subscribedDataSource?.DataChanged += this.OnDataChanged;
        this.shouldBindData = true;
    }

    /// <summary>
    /// Marks dynamic bindings dirty when tree inputs have changed.
    /// </summary>
    private void UpdateTreeInputs()
    {
        if (!ReferenceEquals(this.previousDataTree, this.DataTree))
        {
            this.previousDataTree = this.DataTree;
            this.shouldBindData = true;
        }

        if (!ReferenceEquals(this.previousDataTreeFactory, this.DataTreeFactory))
        {
            this.previousDataTreeFactory = this.DataTreeFactory;
            this.shouldBindData = true;
        }

        if (this.DataTree is not null)
        {
            this.shouldBindData = true;
        }
    }

    /// <summary>
    /// Ensures orientation classes are not counteracted by DaisyUI's default menu wrapping behavior.
    /// </summary>
    private void EnsureNoWrapStyle()
    {
        if (this.MutableComponentAttributes.TryGetValue("style", out var styleAttribute) &&
            styleAttribute is string styleValue)
        {
            if (styleValue.Contains("flex-wrap", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var normalizedStyle = styleValue.Trim().TrimEnd(';');
            this.MutableComponentAttributes["style"] = $"{normalizedStyle}; flex-wrap: nowrap;";
            return;
        }

        this.MutableComponentAttributes["style"] = "flex-wrap: nowrap;";
    }

    /// <summary>
    /// Replaces dynamic state with flat item data.
    /// </summary>
    /// <param name="newItems">The items to store.</param>
    /// <param name="useTreeRendering"><see langword="true"/> to keep tree mode active.</param>
    private void SetFlatItems(IEnumerable<TMenuItem> newItems, bool useTreeRendering)
    {
        lock (this.dataLock)
        {
            this.items = [.. newItems];
            this.treeRootNodes = [];
            this.IsTreeRenderingEnabled = useTreeRendering;
        }
    }

    /// <summary>
    /// Replaces dynamic state with tree root nodes.
    /// </summary>
    /// <param name="newRootNodes">The root nodes to store.</param>
    private void SetTreeNodes(IEnumerable<IDataTreeNode<TMenuItem>> newRootNodes)
    {
        lock (this.dataLock)
        {
            this.items = [];
            this.treeRootNodes = [.. newRootNodes];
            this.IsTreeRenderingEnabled = true;
        }
    }

    /// <summary>
    /// Reads a metadata value from a node with type-safe casting.
    /// </summary>
    /// <typeparam name="TValue">The expected metadata value type.</typeparam>
    /// <param name="node">The node containing metadata.</param>
    /// <param name="metadataKey">The metadata key to read.</param>
    /// <param name="value">When this method returns <see langword="true"/>, contains the typed value.</param>
    /// <returns><see langword="true"/> when a typed metadata value was found; otherwise <see langword="false"/>.</returns>
    private static bool TryGetMetadataValue<TValue>(
        IDataTreeNode<TMenuItem> node,
        string metadataKey,
        [NotNullWhen(true)] out TValue? value)
    {
        if (node.Metadata.TryGetValue(metadataKey, out var rawValue) && rawValue is TValue typedValue)
        {
            value = typedValue;
            return true;
        }

        value = default;
        return false;
    }
}
