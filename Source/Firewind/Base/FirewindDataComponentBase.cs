namespace Firewind.Base;

using Firewind.Data;
using Microsoft.AspNetCore.Components;

/// <summary>
/// Serves as the base class for Firewind components that display data. It provides a framework for
/// data-binding and rendering of individual data items, encapsulating thread safety and change notification.
/// This abstract class implements <see cref="IDataComponent{TDataItem}"/>, enforcing a contract for data fetching
/// and rendering logic.
/// </summary>
/// <typeparam name="TDataItem">The type of data that the component is responsible for displaying.</typeparam>
public abstract class FirewindDataComponentBase<TDataItem> : FirewindComponentBase, IDataComponent<TDataItem>
{
    private IDataSource<TDataItem> dataSource = null!;
    private List<TDataItem> items = [];
    private readonly object itemsLock = new();

    /// <summary>
    /// Responds to data source changes by re-binding data asynchronously.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Arguments of the event.</param>
    private void OnDataChanged(object? sender, EventArgs e) => BindDataAsync(CancellationToken.None).ConfigureAwait(false);

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
                return this.items.ToList();
            }
        }
        set
        {
            lock (this.itemsLock)
            {
                this.items = new List<TDataItem>(value);
            }
        }
    }

    /// <summary>
    /// Binds data to the component asynchronously, ensuring that the data is up-to-date and ready for rendering.
    /// This method must be implemented to handle fetching data from <see cref="DataSource"/> and updating
    /// the component state.
    /// </summary>
    /// <param name="cancellationToken">A token that can be used to signal cancellation of the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown if <see cref="DataSource"/> is null when the method is invoked.</exception>
    public async Task BindDataAsync(CancellationToken cancellationToken)
    {
        if (this.dataSource == null)
        {
            throw new InvalidOperationException("DataSource must not be null.");
        }

        var data = await this.dataSource.FetchDataAsync(cancellationToken).ConfigureAwait(false);
        this.Items = data; // Thread-safe assignment
        StateHasChanged();
    }

    // BL0007: Component parameters should be auto properties
    // Justification: Custom setter required so that when the data source changes we unsubscribe to events from the old data source (in case it hasn't been disposed)
#pragma warning disable BL0007
    /// <summary>
    /// Gets or sets the data source object responsible for providing data to the component.
    /// This object must implement the <see cref="IDataSource{TDataItem}"/> interface.
    /// </summary>
    /// <remarks>
    /// When the data source is set, the component subscribes to the <see cref="IDataSource{TDataItem}.DataChanged"/>
    /// event to receive notifications about data updates. These notifications trigger data re-binding.
    /// </remarks>
    [Parameter]
    public IDataSource<TDataItem> DataSource
    {
        get => this.dataSource;
        set
        {
            if (this.dataSource == value)
            {
                return;
            }

            if (this.dataSource is not null)
            {
                this.dataSource.DataChanged -= OnDataChanged;
            }

            this.dataSource = value ?? throw new ArgumentNullException(nameof(value), "DataSource cannot be null.");
            this.dataSource.DataChanged += OnDataChanged;
        }
    }
#pragma warning restore BL0007

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
}
