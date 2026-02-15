namespace Firewind.UnitTests.Data;

using Firewind.Data;
using FluentAssertions;

/// <summary>
/// Verifies observable and snapshot semantics for <see cref="InMemoryDataSource{TDataItem}"/>.
/// </summary>
public sealed class InMemoryDataSourceTests
{
    /// <summary>
    /// Ensures a null initial sequence is rejected.
    /// </summary>
    [Fact]
    public void Constructor_WhenInitialDataIsNull_ThrowsArgumentNullException()
    {
        var constructor = typeof(InMemoryDataSource<string>).GetConstructor([typeof(IEnumerable<string>)]);

        constructor.Should().NotBeNull();
        if (constructor is null)
        {
            throw new InvalidOperationException("Expected a constructor that accepts IEnumerable<string>.");
        }

        var act = () => constructor.Invoke([null]);

        act.Should().Throw<System.Reflection.TargetInvocationException>()
            .WithInnerException<ArgumentNullException>()
            .Which.ParamName.Should().Be("initialData");
    }

    /// <summary>
    /// Ensures added items appear in subsequent snapshots and change notifications are raised.
    /// </summary>
    [Fact]
    public async Task AddItemAsync_AddsDataAndRaisesDataChangedEvent()
    {
        var dataSource = new InMemoryDataSource<string>([]);
        var raised = 0;
        dataSource.DataChanged += (_, _) => raised++;

        await dataSource.AddItemAsync("new-item");
        var snapshot = (await dataSource.FetchDataAsync(CancellationToken.None)).ToArray();

        raised.Should().Be(1);
        snapshot.Should().Equal("new-item");
    }

    /// <summary>
    /// Ensures remove operations update stored data and raise notifications.
    /// </summary>
    [Fact]
    public async Task RemoveItemAsync_RemovesDataAndRaisesDataChangedEvent()
    {
        var dataSource = new InMemoryDataSource<string>(["alpha", "beta"]);
        var raised = 0;
        dataSource.DataChanged += (_, _) => raised++;

        await dataSource.RemoveItemAsync("alpha");
        var snapshot = (await dataSource.FetchDataAsync(CancellationToken.None)).ToArray();

        raised.Should().Be(1);
        snapshot.Should().Equal("beta");
    }

    /// <summary>
    /// Ensures fetch returns an isolated snapshot rather than exposing internal storage.
    /// </summary>
    [Fact]
    public async Task FetchDataAsync_ReturnsSnapshotIsolation()
    {
        var dataSource = new InMemoryDataSource<string>(["alpha"]);

        var firstSnapshot = (await dataSource.FetchDataAsync(CancellationToken.None)).ToList();
        firstSnapshot.Add("mutated-locally");

        var secondSnapshot = (await dataSource.FetchDataAsync(CancellationToken.None)).ToArray();

        secondSnapshot.Should().Equal("alpha");
    }

    /// <summary>
    /// Ensures cancellation is observed before data is returned.
    /// </summary>
    [Fact]
    public async Task FetchDataAsync_WhenCancellationRequested_ThrowsOperationCanceledException()
    {
        var dataSource = new InMemoryDataSource<string>(["alpha"]);
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        var act = async () => await dataSource.FetchDataAsync(cancellationTokenSource.Token);

        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
