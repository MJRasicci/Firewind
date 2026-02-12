namespace Firewind.Base;

using Firewind.Data;
using Microsoft.AspNetCore.Components;

/// <summary>
/// Represents components that are responsible for rendering data dynamically.
/// This interface prescribes methods and properties for managing data binding and presentation.
/// </summary>
/// <typeparam name="TDataItem">The type of the data item that the component is responsible for rendering.</typeparam>
public interface IDataComponent<TDataItem> : IFirewindComponent
{
    /// <summary>
    /// Asynchronously binds data to the component, allowing for dynamic data fetching and rendering.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that the task will observe for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation of data binding.</returns>
    Task BindDataAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets or sets the source of data that the component consumes.
    /// </summary>
    /// <value>
    /// An <see cref="IDataSource{TDataItem}"/> that provides the data to be rendered by the component.
    /// </value>
    IDataSource<TDataItem> DataSource { get; set; }

    /// <summary>
    /// Gets or sets the template used for rendering each data item within the component.
    /// </summary>
    /// <value>
    /// A <see cref="RenderFragment{TDataItem}"/> that generates the markup for a single data item.
    /// The template can be defined inline or as a separate Razor component.
    /// </value>
    RenderFragment<TDataItem>? ItemTemplate { get; set; }
}
