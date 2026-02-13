namespace Firewind.Data;

/// <summary>
/// Provides an in-memory implementation of <see cref="IDataSource{TDataItem}"/> that can be used for temporary storage and manipulation of a collection of data items.
/// </summary>
/// <typeparam name="TDataItem">The type of data items stored in the data source.</typeparam>
/// <remarks>
/// This class is particularly useful in scenarios where persistent storage is not necessary, or when you want to work with a volatile copy of the data.
/// Access to the backing collection is synchronized so reads and writes are thread-safe.
/// </remarks>
public class InMemoryDataSource<TDataItem> : IDataSource<TDataItem>
{
    private readonly Lock dataLock = new();
    private readonly List<TDataItem> data;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryDataSource{TDataItem}"/> class with an initial set of data.
    /// </summary>
    /// <param name="initialData">An <see cref="IEnumerable{T}"/> containing the initial set of data items to store in the data source.</param>
    public InMemoryDataSource(IEnumerable<TDataItem> initialData)
    {
        // The '?? throw' construct ensures that a null reference cannot be passed as initial data, preventing potential null reference exceptions later on.
        this.data = [.. initialData ?? throw new ArgumentNullException(nameof(initialData))];
    }

    /// <summary>
    /// Occurs when the data in the source has changed.
    /// </summary>
    public event EventHandler? DataChanged;

    /// <inheritdoc />
    public Task AddItemAsync(TDataItem item)
    {
        // Locking ensures thread-safe modification of the in-memory data collection.
        lock (this.dataLock)
        {
            this.data.Add(item);
        }

        // The DataChanged event is raised on every addition to the collection to notify subscribers of the change.
        DataChanged?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task RemoveItemAsync(TDataItem item)
    {
        // Locking ensures thread-safe modification of the in-memory data collection.
        lock (this.dataLock)
        {
            this.data.Remove(item);
        }

        // The DataChanged event is raised on every removal from the collection to notify subscribers of the change.
        DataChanged?.Invoke(this, EventArgs.Empty);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<IEnumerable<TDataItem>> FetchDataAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        List<TDataItem> snapshot;

        // Locking ensures thread-safe read operation of the in-memory data collection.
        lock (this.dataLock)
        {
            snapshot = [.. this.data];
        }

        // A snapshot of the data is returned to prevent external modification of the internal data collection.
        return Task.FromResult<IEnumerable<TDataItem>>(snapshot);
    }
}
