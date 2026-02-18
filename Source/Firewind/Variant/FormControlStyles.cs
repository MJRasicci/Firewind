namespace Firewind.Variant;

/// <summary>
/// Represents field styling options for text-like form controls.
/// </summary>
public enum FieldStyle
{
    /// <summary>
    /// Uses the default style for the control.
    /// </summary>
    Default,
    /// <summary>
    /// Applies the ghost visual style.
    /// </summary>
    Ghost
}

/// <summary>
/// Resolves CSS classes for form-control style, color, and size variants.
/// </summary>
public static class FormControlStyleExtensions
{
    /// <summary>
    /// Gets style classes for a text-like control token.
    /// </summary>
    /// <param name="style">The visual style variant.</param>
    /// <param name="componentToken">The daisyUI component token without a prefix (for example, <c>input</c>).</param>
    /// <returns>A CSS class string for the selected style.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="componentToken"/> is null, empty, or whitespace.</exception>
    public static string ClassNames(this FieldStyle style, string componentToken)
    {
        if (string.IsNullOrWhiteSpace(componentToken))
        {
            throw new ArgumentException("Component token cannot be null or whitespace.", nameof(componentToken));
        }

        return style switch
        {
            FieldStyle.Ghost => $"fw-{componentToken}-ghost",
            _ => string.Empty
        };
    }

    /// <summary>
    /// Gets color classes for the provided form-control token.
    /// </summary>
    /// <param name="color">The semantic color token.</param>
    /// <param name="componentToken">The daisyUI component token without a prefix.</param>
    /// <returns>A CSS class string for the selected color.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="componentToken"/> is null, empty, or whitespace.</exception>
    public static string FormControlClassNames(this ThemeColor color, string componentToken)
    {
        if (string.IsNullOrWhiteSpace(componentToken))
        {
            throw new ArgumentException("Component token cannot be null or whitespace.", nameof(componentToken));
        }

        var colorName = color switch
        {
            ThemeColor.Neutral => "neutral",
            ThemeColor.Primary => "primary",
            ThemeColor.Secondary => "secondary",
            ThemeColor.Accent => "accent",
            ThemeColor.Info => "info",
            ThemeColor.Success => "success",
            ThemeColor.Warning => "warning",
            ThemeColor.Error => "error",
            _ => string.Empty
        };

        return string.IsNullOrEmpty(colorName)
            ? string.Empty
            : $"fw-{componentToken}-{colorName}";
    }

    /// <summary>
    /// Gets size classes for the provided form-control token.
    /// </summary>
    /// <param name="size">The component size value.</param>
    /// <param name="componentToken">The daisyUI component token without a prefix.</param>
    /// <returns>A CSS class string for the selected size.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="componentToken"/> is null, empty, or whitespace.</exception>
    public static string FormControlClassNames(this ComponentSize size, string componentToken)
    {
        if (string.IsNullOrWhiteSpace(componentToken))
        {
            throw new ArgumentException("Component token cannot be null or whitespace.", nameof(componentToken));
        }

        return size switch
        {
            ComponentSize.Tiny => $"fw-{componentToken}-xs",
            ComponentSize.Small => $"fw-{componentToken}-sm",
            ComponentSize.Large => $"fw-{componentToken}-lg",
            ComponentSize.ExtraLarge => $"fw-{componentToken}-xl",
            ComponentSize.Responsive => $"fw-{componentToken}-xs sm:fw-{componentToken}-sm md:fw-{componentToken}-md lg:fw-{componentToken}-lg xl:fw-{componentToken}-xl",
            _ => string.Empty
        };
    }
}
