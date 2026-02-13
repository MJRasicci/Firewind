namespace Firewind.Base;

/// <summary>
/// Defines the basic functionality of a Firewind component.
/// </summary>
public interface IFirewindComponent
{
    /// <summary>
    /// Gets the unique identifier for the component instance, used for referencing the rendered HTML element.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> representing the unique identifier of the component.
    /// </value>
    public string Id { get; }

    /// <summary>
    /// Gets additional attributes that will be rendered with the component's root HTML element.
    /// These are attributes not explicitly defined by the component's properties.
    /// </summary>
    /// <value>
    /// A <see cref="Dictionary{TKey, TValue}"/> containing the attributes as key-value pairs,
    /// with the key being the attribute name and the value being the attribute value.
    /// </value>
    public Dictionary<string, object> ComponentAttributes { get; }
}
