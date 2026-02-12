namespace Firewind.Data;

/// <summary>
/// Interface for services which acquire data.
/// </summary>
/// <typeparam name="TDataItem">The type of data acquired by the data source.</typeparam>
public interface IDataSource<TDataItem>
{
    /// <summary>
    /// Fetches data asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A collection of <typeparamref name="TDataItem" />.</returns>
    Task<IEnumerable<TDataItem>> FetchDataAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Adds an item to the data source.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public Task AddItemAsync(TDataItem item);

    /// <summary>
    /// Removes an item from the data source.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    public Task RemoveItemAsync(TDataItem item);

    /// <summary>
    /// Event triggered when data changes.
    /// </summary>
    event EventHandler DataChanged;
}
