namespace Firewind.Components.Internal;

using Firewind.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

/// <summary>
/// Represents a dynamic HTML element in the Firewind component system.
/// This component allows for rendering of arbitrary HTML tags with content,
/// leveraging the power of Blazor's RenderTreeBuilder.
/// </summary>
public sealed class DynamicElement : FirewindComponentBase
{
    /// <summary>
    /// Gets or sets the HTML tag name to be used for the component rendering.
    /// </summary>
    /// <value>
    /// The tag name as a <see cref="string"/>. The default is <c>div</c>.
    /// </value>
    [Parameter]
    public string Tag { get; set; } = "div";

    /// <summary>
    /// Gets or sets the content to be rendered inside the dynamic element.
    /// </summary>
    /// <value>
    /// A <see cref="RenderFragment"/> representing the child content of the element.
    /// </value>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the rendered element is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the default click action should be prevented.
    /// </summary>
    [Parameter]
    public bool PreventDefaultOnClick { get; set; }

    /// <summary>
    /// Builds the render tree for the <see cref="DynamicElement"/> component.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> used to build the component's render tree.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="builder"/> argument is null.
    /// </exception>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        // Ensure the builder instance is not null to avoid runtime errors.
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));

        // Sequence identifier for the rendering operations, ensuring the diffing algorithm operates efficiently.
        var seq = 0;

        var tag = string.IsNullOrWhiteSpace(this.Tag) ? "div" : this.Tag;

        // Open the element with the tag specified in the Tag property.
        builder.OpenElement(seq++, tag);

        // Add any additional attributes that have been provided to the component.
        builder.AddMultipleAttributes(seq++, this.ComponentAttributes);

        // Add click support when an event handler is supplied.
        if (this.OnClick.HasDelegate)
        {
            builder.AddAttribute(seq++, "onclick", this.OnClick);

            if (this.PreventDefaultOnClick)
            {
                builder.AddEventPreventDefaultAttribute(seq++, "onclick", true);
            }
        }

        // If ChildContent is not null, add it to the render tree inside the element.
        if (this.ChildContent is not null)
        {
            builder.AddContent(seq++, this.ChildContent);
        }

        // Close the element to complete the rendering operation.
        builder.CloseElement();
    }
}
