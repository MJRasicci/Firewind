namespace Firewind.Components;

/// <summary>
/// Provides model-binding operations for form fields rendered inside <see cref="FWForm{TModel}"/>.
/// </summary>
/// <typeparam name="TModel">The model type.</typeparam>
public sealed class FWFormContext<TModel>
{
    private readonly Func<string, object?> valueAccessor;
    private readonly Func<string, object?, Task> valueMutator;

    /// <summary>
    /// Initializes a new instance of the <see cref="FWFormContext{TModel}"/> class.
    /// </summary>
    /// <param name="model">The bound model instance.</param>
    /// <param name="valueAccessor">A delegate that gets field values by path.</param>
    /// <param name="valueMutator">A delegate that sets field values by path.</param>
    public FWFormContext(TModel model, Func<string, object?> valueAccessor, Func<string, object?, Task> valueMutator)
    {
        this.Model = model;
        this.valueAccessor = valueAccessor;
        this.valueMutator = valueMutator;
    }

    /// <summary>
    /// Gets the bound model instance.
    /// </summary>
    public TModel Model { get; }

    /// <summary>
    /// Gets a model value for the supplied property path.
    /// </summary>
    /// <param name="path">The property path.</param>
    /// <returns>The resolved value, if available; otherwise <see langword="null"/>.</returns>
    public object? GetValue(string path) => this.valueAccessor(path);

    /// <summary>
    /// Sets a model value for the supplied property path.
    /// </summary>
    /// <param name="path">The property path.</param>
    /// <param name="value">The incoming value.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task SetValueAsync(string path, object? value) => this.valueMutator(path, value);
}
