namespace Firewind.Base;

using Firewind.Style;
using Microsoft.AspNetCore.Components;

/// <summary>
/// Serves as the base class for all Firewind components, encapsulating common properties and functionality.
/// This abstract class implements <see cref="IFirewindComponent"/> and ensures consistent handling
/// of unmatched attributes and component identification across derived components.
/// </summary>
public abstract class FirewindComponentBase : ComponentBase, IFirewindComponent
{

    /// <summary>
    /// Gets or sets additional attributes captured from the component invocation.
    /// </summary>
    /// <remarks>
    /// This property captures all unmatched attributes. If an 'id' attribute is provided, it will be used
    /// as the component's HTML 'id' attribute; otherwise, a unique identifier will be generated.
    /// </remarks>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; } = [];

    /// <summary>
    /// Gets or sets additional attributes that will be rendered with the component's HTML element
    /// and are not explicitly captured by other component parameters.
    /// </summary>
    public Dictionary<string, object> ComponentAttributes { get; private set; } = [];

    /// <summary>
    /// Gets the component's unique identifier, which is used as the HTML 'id' attribute for the rendered element.
    /// </summary>
    public string Id { get; private set; } = $"fw-{Guid.NewGuid().ToString("N")[..8]}";

    /// <summary>
    /// Ensures that a unique 'id' attribute is present in <see cref="ComponentAttributes"/> 
    /// if it has not been explicitly supplied.
    /// </summary>
    protected override void OnParametersSet()
    {
        if (this.AdditionalAttributes.TryGetValue("id", out var attr1) && attr1 is string id)
        {
            this.Id = id;
        }

        this.CssClasses = new(this.CssClass);

        if (this.AdditionalAttributes.TryGetValue("class", out var attr2) && attr2 is string classNames)
        {
            this.CssClasses.Add(classNames);
        }

        this.ComponentAttributes = new(this.AdditionalAttributes)
        {
            ["id"] = this.Id,
            ["class"] = this.CssClasses.ToString()
        };
    }

    /// <summary>
    /// Gets base CSS classes for the component before unmatched class attributes are merged.
    /// </summary>
    protected virtual string CssClass => string.Empty;

    /// <summary>
    /// Gets or sets the internal CSS class list used to build the final rendered <c>class</c> attribute.
    /// </summary>
    internal CssClassList CssClasses { get; set; } = [];
}
