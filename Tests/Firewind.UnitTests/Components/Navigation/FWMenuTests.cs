namespace Firewind.UnitTests.Components.Navigation;

using Firewind.Components;
using Firewind.Data;
using Firewind.Variant;
using FluentAssertions;
using Microsoft.AspNetCore.Components;

/// <summary>
/// Verifies parameter-driven attribute behavior for <see cref="FWMenu{TMenuItem}"/>.
/// </summary>
public sealed class FWMenuTests
{
    /// <summary>
    /// Ensures menu classes include orientation, size, and themed surface modifiers.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenVariantsAreConfigured_ComposesExpectedClassNames()
    {
        using var menu = new TestMenu();
        menu.Configure(
            orientation: MenuOrientation.Horizontal,
            size: MenuSize.Small,
            color: ThemeColor.Primary,
            glow: true,
            additionalAttributes: new Dictionary<string, object>());

        menu.ApplyParametersSync();

        menu.ComponentAttributes.Should().ContainKey("class");
        var classNames = menu.ComponentAttributes["class"]?.ToString();
        classNames.Should().Contain("fw-menu");
        classNames.Should().Contain("fw-menu-horizontal");
        classNames.Should().Contain("fw-menu-sm");
        classNames.Should().Contain("bg-primary");
        classNames.Should().Contain("text-primary-content");
        classNames.Should().Contain("shadow-primary/40");
    }

    /// <summary>
    /// Ensures a no-wrap style declaration is appended when not already present.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenFlexWrapNotProvided_AppendsNoWrapStyle()
    {
        using var menu = new TestMenu();
        menu.Configure(
            orientation: MenuOrientation.Vertical,
            size: MenuSize.Normal,
            color: ThemeColor.None,
            glow: false,
            additionalAttributes: new Dictionary<string, object>
            {
                ["style"] = "padding: 0.5rem;"
            });

        menu.ApplyParametersSync();

        menu.ComponentAttributes.Should().ContainKey("style");
        menu.ComponentAttributes["style"]?.ToString().Should().Contain("flex-wrap: nowrap;");
    }

    /// <summary>
    /// Ensures static menu usage does not require a data source.
    /// </summary>
    [Fact]
    public async Task OnParametersSetAsync_WhenNoDynamicDataProvided_KeepsStaticRenderingMode()
    {
        using var menu = new TestMenu();
        menu.Configure(
            orientation: MenuOrientation.Vertical,
            size: MenuSize.Normal,
            color: ThemeColor.None,
            glow: false,
            additionalAttributes: new Dictionary<string, object>());

        await menu.ApplyParametersAsync();

        menu.IsTreeMode.Should().BeFalse();
        menu.FlatItems.Should().BeEmpty();
        menu.RootNodes.Should().BeEmpty();
    }

    /// <summary>
    /// Ensures dynamic tree mode is used when a data source and factory are provided.
    /// </summary>
    [Fact]
    public async Task OnParametersSetAsync_WhenDataTreeFactoryProvided_UsesTreeRendering()
    {
        using var menu = new TestMenu();
        menu.Configure(
            orientation: MenuOrientation.Vertical,
            size: MenuSize.Small,
            color: ThemeColor.Base200,
            glow: false,
            additionalAttributes: new Dictionary<string, object>());

        menu.SetDynamicDataSource(new InMemoryDataSource<string>(["Workspace", "Dashboard"]));
        menu.SetDataTreeFactory(static items => DataTree
            .FromFlat<string, string>(
                static item => item,
                static item => item == "Workspace" ? null : "Workspace")
            .WithNodeKind(static item => item == "Workspace" ? DataTreeNodeKind.Header : DataTreeNodeKind.Item)
            .Build(items));

        await menu.ApplyParametersAsync();

        menu.IsTreeMode.Should().BeTrue();
        menu.RootNodes.Should().ContainSingle();
        menu.RootNodes[0].Kind.Should().Be(DataTreeNodeKind.Header);
        menu.RootNodes[0].Children.Should().ContainSingle();
    }

    /// <summary>
    /// Ensures explicit data trees are rendered without querying a data source.
    /// </summary>
    [Fact]
    public async Task OnParametersSetAsync_WhenExplicitDataTreeProvided_UsesTreeNodes()
    {
        using var menu = new TestMenu();
        menu.Configure(
            orientation: MenuOrientation.Vertical,
            size: MenuSize.Small,
            color: ThemeColor.None,
            glow: false,
            additionalAttributes: new Dictionary<string, object>());

        menu.SetDataTree(DataTree
            .FromFlat<string, string>(
                static item => item,
                static item => item == "Workspace" ? null : "Workspace")
            .Build(["Workspace", "Dashboard"]));

        await menu.ApplyParametersAsync();

        menu.IsTreeMode.Should().BeTrue();
        menu.RootNodes.Should().ContainSingle();
        menu.RootNodes[0].Key.Should().Be("Workspace");
    }

    /// <summary>
    /// Ensures template resolution prioritizes selector, then level, then kind, then fallback.
    /// </summary>
    [Fact]
    public void ResolveNodeTemplate_UsesDefinedPrecedenceOrder()
    {
        using var menu = new TestMenu();
        menu.Configure(
            orientation: MenuOrientation.Vertical,
            size: MenuSize.Normal,
            color: ThemeColor.None,
            glow: false,
            additionalAttributes: new Dictionary<string, object>());

        RenderFragment<IDataTreeNode<string>> selectorTemplate = static _ => static _ => { };
        RenderFragment<IDataTreeNode<string>> levelTemplate = static _ => static _ => { };
        RenderFragment<IDataTreeNode<string>> headerTemplate = static _ => static _ => { };
        RenderFragment<IDataTreeNode<string>> nodeTemplate = static _ => static _ => { };

        menu.SetTreeTemplateSelector(static _ => null);
        menu.SetTreeLevelTemplates(new Dictionary<int, RenderFragment<IDataTreeNode<string>>>
        {
            [1] = levelTemplate
        });
        menu.SetTreeHeaderTemplate(headerTemplate);
        menu.SetTreeNodeTemplate(nodeTemplate);

        var headerNode = TestTreeNode.Create(
            key: "workspace",
            level: 1,
            kind: DataTreeNodeKind.Header,
            children: []);

        menu.ResolveTemplateFor(headerNode).Should().BeSameAs(levelTemplate);

        menu.SetTreeLevelTemplates(null);
        menu.ResolveTemplateFor(headerNode).Should().BeSameAs(headerTemplate);

        menu.SetTreeTemplateSelector(_ => selectorTemplate);
        menu.ResolveTemplateFor(headerNode).Should().BeSameAs(selectorTemplate);
    }

    /// <summary>
    /// Ensures default rendering metadata is read from known menu metadata keys.
    /// </summary>
    [Fact]
    public void ResolveNodeDefaults_WhenMetadataIsPresent_UsesMetadataValues()
    {
        using var menu = new TestMenu();
        menu.Configure(
            orientation: MenuOrientation.Vertical,
            size: MenuSize.Normal,
            color: ThemeColor.None,
            glow: false,
            additionalAttributes: new Dictionary<string, object>());

        var node = TestTreeNode.Create(
            key: "dashboard",
            level: 0,
            kind: DataTreeNodeKind.Item,
            children: [],
            metadata: new Dictionary<string, object?>
            {
                [MenuTreeMetadataKeys.Label] = "Dashboard",
                [MenuTreeMetadataKeys.Href] = "/dashboard",
                [MenuTreeMetadataKeys.ActionElement] = "button",
                [MenuTreeMetadataKeys.Behavior] = MenuItemBehavior.Active
            });

        menu.ResolveLabelFor(node).Should().Be("Dashboard");
        menu.ResolveHrefFor(node).Should().Be("/dashboard");
        menu.ResolveActionElementFor(node).Should().Be("button");
        menu.ResolveBehaviorFor(node).Should().Be(MenuItemBehavior.Active);
    }

    /// <summary>
    /// Ensures non-collapsible branch nodes are rendered as non-interactive title rows.
    /// </summary>
    [Fact]
    public void ShouldRenderNodeAsTitle_WhenNodeIsNonCollapsibleBranch_ReturnsTrue()
    {
        using var menu = new TestMenu();
        menu.Configure(
            orientation: MenuOrientation.Vertical,
            size: MenuSize.Normal,
            color: ThemeColor.None,
            glow: false,
            additionalAttributes: new Dictionary<string, object>());

        var childNode = TestTreeNode.Create(
            key: "child",
            level: 1,
            kind: DataTreeNodeKind.Item,
            children: []);
        var branchNode = TestTreeNode.Create(
            key: "branch",
            level: 0,
            kind: DataTreeNodeKind.Item,
            children: [childNode],
            isCollapsible: false);

        menu.ShouldRenderAsTitle(branchNode).Should().BeTrue();
    }

    /// <summary>
    /// Ensures collapsible branch nodes continue to render as interactive menu items.
    /// </summary>
    [Fact]
    public void ShouldRenderNodeAsTitle_WhenNodeIsCollapsibleBranch_ReturnsFalse()
    {
        using var menu = new TestMenu();
        menu.Configure(
            orientation: MenuOrientation.Vertical,
            size: MenuSize.Normal,
            color: ThemeColor.None,
            glow: false,
            additionalAttributes: new Dictionary<string, object>());

        var childNode = TestTreeNode.Create(
            key: "child",
            level: 1,
            kind: DataTreeNodeKind.Item,
            children: []);
        var branchNode = TestTreeNode.Create(
            key: "branch",
            level: 0,
            kind: DataTreeNodeKind.Item,
            children: [childNode],
            isCollapsible: true);

        menu.ShouldRenderAsTitle(branchNode).Should().BeFalse();
    }

    private sealed class TestMenu : FWMenu<string>
    {
        public void Configure(
            MenuOrientation orientation,
            MenuSize size,
            ThemeColor color,
            bool glow,
            IReadOnlyDictionary<string, object> additionalAttributes)
        {
            this.Orientation = orientation;
            this.Size = size;
            this.Color = color;
            this.Glow = glow;
            this.AdditionalAttributes = additionalAttributes;
        }

        public IReadOnlyList<string> FlatItems => this.Items;

        public IReadOnlyList<IDataTreeNode<string>> RootNodes => this.TreeRootNodes;

        public bool IsTreeMode => this.UseTreeRendering;

        public RenderFragment<IDataTreeNode<string>>? ResolveTemplateFor(IDataTreeNode<string> node) =>
            this.ResolveNodeTemplate(node);

        public bool ShouldRenderAsTitle(IDataTreeNode<string> node) => this.ShouldRenderNodeAsTitle(node);

        public string ResolveLabelFor(IDataTreeNode<string> node) => this.ResolveNodeLabel(node);

        public string? ResolveHrefFor(IDataTreeNode<string> node) => this.ResolveNodeHref(node);

        public string ResolveActionElementFor(IDataTreeNode<string> node) => this.ResolveNodeActionElement(node);

        public MenuItemBehavior ResolveBehaviorFor(IDataTreeNode<string> node) => this.ResolveNodeBehavior(node);

        public void ApplyParametersSync() => base.OnParametersSet();

        public void SetDataTree(IDataTree<string>? dataTree) => this.DataTree = dataTree;

        public void SetDataTreeFactory(Func<IEnumerable<string>, IDataTree<string>>? dataTreeFactory) =>
            this.DataTreeFactory = dataTreeFactory;

        public void SetDynamicDataSource(IDataSource<string>? dataSource) => this.DataSource = dataSource;

        public void SetTreeTemplateSelector(
            Func<IDataTreeNode<string>, RenderFragment<IDataTreeNode<string>>?>? treeTemplateSelector) =>
            this.TreeTemplateSelector = treeTemplateSelector;

        public void SetTreeLevelTemplates(
            IReadOnlyDictionary<int, RenderFragment<IDataTreeNode<string>>>? treeLevelTemplates) =>
            this.TreeLevelTemplates = treeLevelTemplates;

        public void SetTreeHeaderTemplate(RenderFragment<IDataTreeNode<string>>? treeHeaderTemplate) =>
            this.TreeHeaderTemplate = treeHeaderTemplate;

        public void SetTreeNodeTemplate(RenderFragment<IDataTreeNode<string>>? treeNodeTemplate) =>
            this.TreeNodeTemplate = treeNodeTemplate;

        public async Task ApplyParametersAsync()
        {
            base.OnParametersSet();
            await base.OnParametersSetAsync();
        }
    }

    private sealed class TestTreeNode : IDataTreeNode<string>
    {
        public string Key { get; init; } = string.Empty;

        public string? DataItem { get; init; }

        public int Level { get; init; }

        public DataTreeNodeKind Kind { get; init; }

        public IReadOnlyList<IDataTreeNode<string>> Children { get; init; } = [];

        public bool IsCollapsible { get; init; }

        public bool IsExpanded { get; init; }

        public IReadOnlyDictionary<string, object?> Metadata { get; init; } = new Dictionary<string, object?>();

        public static TestTreeNode Create(
            string key,
            int level,
            DataTreeNodeKind kind,
            IReadOnlyList<IDataTreeNode<string>> children,
            IReadOnlyDictionary<string, object?>? metadata = null,
            bool? isCollapsible = null) => new()
            {
                Key = key,
                DataItem = key,
                Level = level,
                Kind = kind,
                Children = children,
                IsCollapsible = isCollapsible ?? (children.Count > 0),
                IsExpanded = false,
                Metadata = metadata ?? new Dictionary<string, object?>()
            };
    }
}
