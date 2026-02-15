namespace Firewind.Base;

using Firewind.Data;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Serves as the base class for Firewind components that display data. It provides a framework for
/// data-binding and rendering of individual data items, encapsulating thread safety and change notification.
/// This abstract class implements <see cref="IDataComponent{TDataItem}"/>, enforcing a contract for data fetching
/// and rendering logic.
/// </summary>
/// <typeparam name="TDataItem">The type of data that the component is responsible for displaying.</typeparam>
public abstract class FirewindDataComponentBase<TDataItem> : FirewindComponentBase, IDataComponent<TDataItem>, IDisposable
{
    private List<TDataItem> items = [];
    private readonly Lock itemsLock = new();
    private readonly CancellationTokenSource disposeTokenSource = new();
    private bool isDataSourceDirty;
    private bool isDisposed;

    /// <summary>
    /// Responds to data source changes by re-binding data asynchronously.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Arguments of the event.</param>
    private void OnDataChanged(object? sender, EventArgs e)
    {
        if (this.isDisposed)
        {
            return;
        }

        _ = HandleDataChangedAsync();
    }

    /// <summary>
    /// Rebinds data for a <see cref="IDataSource{TDataItem}.DataChanged"/> notification and dispatches errors
    /// to Blazor's normal component exception flow.
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
            await InvokeAsync(() => BindDataAsync(this.disposeTokenSource.Token));
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
    /// Renders an individual data item using the defined item template.
    /// If no item template is specified, the default template is used.
    /// </summary>
    /// <param name="item">The data item to render.</param>
    /// <returns>A <see cref="RenderFragment"/> representing the rendered view of the item.</returns>
    protected RenderFragment RenderItem(TDataItem item) => (this.ItemTemplate ?? this.DefaultItemTemplate)(item);

    /// <summary>
    /// Gets the default item template to be used when no custom <see cref="ItemTemplate"/> is set.
    /// Derived classes must override this to define the default rendering of data items.
    /// </summary>
    protected abstract RenderFragment<TDataItem> DefaultItemTemplate { get; }

    /// <summary>
    /// Provides thread-safe access to the collection of data items.
    /// </summary>
    protected IEnumerable<TDataItem> Items
    {
        get
        {
            lock (this.itemsLock)
            {
                return [.. this.items];
            }
        }
        set
        {
            lock (this.itemsLock)
            {
                this.items = [.. value];
            }
        }
    }

    /// <summary>
    /// Binds data to the component asynchronously, ensuring that the data is up-to-date and ready for rendering.
    /// The method fetches data from <see cref="DataSource"/>, updates <see cref="Items"/>, and triggers rerendering.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to signal cancellation of the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="DataSource"/> is null when the method is invoked.</exception>
    public async Task BindDataAsync(CancellationToken cancellationToken)
    {
        ObjectDisposedException.ThrowIf(this.isDisposed, this);

        if (this.DataSource == null)
        {
            throw new InvalidOperationException("DataSource must not be null.");
        }

        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this.disposeTokenSource.Token);
        var data = await this.DataSource.FetchDataAsync(linkedTokenSource.Token);
        this.Items = data; // Thread-safe assignment
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Gets or sets the data source object responsible for providing data to the component.
    /// This object must implement the <see cref="IDataSource{TDataItem}"/> interface.
    /// </summary>
    /// <remarks>
    /// When the data source is set, the component subscribes to the <see cref="IDataSource{TDataItem}.DataChanged"/>
    /// event to receive notifications about data updates. These notifications trigger data re-binding.
    /// </remarks>
    [Parameter]
    [EditorRequired]
    public IDataSource<TDataItem> DataSource
    {
        get;
        set
        {
            if (field == value)
            {
                return;
            }

            field?.DataChanged -= OnDataChanged;

            field = value ?? throw new ArgumentNullException(nameof(value), "DataSource cannot be null.");
            field.DataChanged += OnDataChanged;
            this.isDataSourceDirty = true;
        }
    } = null!;

    /// <summary>
    /// Gets or sets the template used for rendering each data item within the component.
    /// This can be specified to customize the presentation of each item.
    /// </summary>
    /// <remarks>
    /// If <see cref="ItemTemplate"/> is not provided, <see cref="DefaultItemTemplate"/> will be used
    /// as the rendering mechanism for each data item.
    /// </remarks>
    [Parameter]
    public RenderFragment<TDataItem>? ItemTemplate { get; set; }

    /// <summary>
    /// Rebinds data after parameters are set when the data source has changed.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override async Task OnParametersSetAsync()
    {
        if (this.DataSource is not null && this.isDataSourceDirty)
        {
            this.isDataSourceDirty = false;
            await BindDataAsync(this.disposeTokenSource.Token);
        }

        await base.OnParametersSetAsync();
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
        this.DataSource?.DataChanged -= OnDataChanged;
        this.disposeTokenSource.Dispose();
    }
}
