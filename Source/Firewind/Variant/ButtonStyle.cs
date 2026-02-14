namespace Firewind.Variant;

/// <summary>
/// Defines visual variants for <c>FWButton</c>.
/// </summary>
public enum ButtonStyle
{
    /// <summary>
    /// Uses only base button classes with no semantic style modifier.
    /// </summary>
    None,
    /// <summary>
    /// Applies the neutral button style.
    /// </summary>
    Neutral,
    /// <summary>
    /// Applies the primary button style.
    /// </summary>
    Primary,
    /// <summary>
    /// Applies the secondary button style.
    /// </summary>
    Secondary,
    /// <summary>
    /// Applies the accent button style.
    /// </summary>
    Accent,
    /// <summary>
    /// Applies the informational button style.
    /// </summary>
    Info,
    /// <summary>
    /// Applies the success button style.
    /// </summary>
    Success,
    /// <summary>
    /// Applies the warning button style.
    /// </summary>
    Warning,
    /// <summary>
    /// Applies the error button style.
    /// </summary>
    Error,
    /// <summary>
    /// Applies a ghost button style.
    /// </summary>
    Ghost,
    /// <summary>
    /// Applies a link-style button.
    /// </summary>
    Link
}

/// <summary>
/// Defines shape variants for <c>FWButton</c>.
/// </summary>
public enum ButtonShape
{
    /// <summary>
    /// Uses the default rectangular button shape.
    /// </summary>
    Normal,
    /// <summary>
    /// Applies a wider horizontal button shape.
    /// </summary>
    Wide,
    /// <summary>
    /// Expands the button to fill available width.
    /// </summary>
    Block,
    /// <summary>
    /// Applies a square button shape.
    /// </summary>
    Square,
    /// <summary>
    /// Applies a circular button shape.
    /// </summary>
    Circle,
}

/// <summary>
/// Resolves CSS classes for <see cref="ButtonStyle"/> values.
/// </summary>
public static class ButtonStyleExtensions
{
    /// <summary>
    /// Gets the CSS class string for a style variant.
    /// </summary>
    /// <param name="style">The style variant to resolve.</param>
    /// <returns>
    /// A space-delimited CSS class string, or an empty string when <paramref name="style"/> is <see cref="ButtonStyle.None"/>.
    /// </returns>
    public static string ClassNames(this ButtonStyle style) => style switch
    {
        ButtonStyle.Neutral => "fw-btn-neutral",
        ButtonStyle.Primary => "fw-btn-primary",
        ButtonStyle.Secondary => "fw-btn-secondary",
        ButtonStyle.Accent => "fw-btn-accent",
        ButtonStyle.Info => "fw-btn-info",
        ButtonStyle.Success => "fw-btn-success",
        ButtonStyle.Warning => "fw-btn-warning",
        ButtonStyle.Error => "fw-btn-error",
        ButtonStyle.Ghost => "fw-btn-ghost",
        ButtonStyle.Link => "fw-btn-link",
        _ => string.Empty
    };
}

/// <summary>
/// Resolves CSS classes for <see cref="ButtonShape"/> values.
/// </summary>
public static class ButtonShapeExtensions
{
    /// <summary>
    /// Gets the CSS class string for a shape variant.
    /// </summary>
    /// <param name="shape">The shape variant to resolve.</param>
    /// <returns>
    /// A CSS class string, or an empty string when <paramref name="shape"/> is <see cref="ButtonShape.Normal"/>.
    /// </returns>
    public static string ClassNames(this ButtonShape shape) => shape switch
    {
        ButtonShape.Wide => "fw-btn-wide",
        ButtonShape.Block => "fw-btn-block",
        ButtonShape.Square => "fw-btn-square",
        ButtonShape.Circle => "fw-btn-circle",
        _ => string.Empty
    };
}

/// <summary>
/// Resolves button-size CSS classes from <see cref="ComponentSize"/> values.
/// </summary>
public static class ButtonSizeExtensions
{
    /// <summary>
    /// Gets size-specific button classes.
    /// </summary>
    /// <param name="size">The size value to map.</param>
    /// <returns>
    /// A CSS class string representing the selected button size.
    /// </returns>
    public static string ButtonClassNames(this ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-btn-xs",
        ComponentSize.Small => "fw-btn-sm",
        ComponentSize.Large => "fw-btn-lg",
        ComponentSize.ExtraLarge => "fw-btn-xl",
        ComponentSize.Responsive => "fw-btn-xs sm:fw-btn-sm md:fw-btn-md lg:fw-btn-lg xl:fw-btn-xl",
        _ => string.Empty
    };
}
