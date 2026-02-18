namespace Firewind.Variant;

/// <summary>
/// Represents style modifiers for card components.
/// </summary>
public enum CardStyle
{
    /// <summary>
    /// Uses default card styling.
    /// </summary>
    Default,
    /// <summary>
    /// Applies bordered card style.
    /// </summary>
    Border,
    /// <summary>
    /// Applies dashed card style.
    /// </summary>
    Dash
}

/// <summary>
/// Resolves class names for card and avatar variants.
/// </summary>
public static class CardAvatarStyleExtensions
{
    /// <summary>
    /// Gets card style classes.
    /// </summary>
    /// <param name="style">The card style variant.</param>
    /// <returns>A CSS class string for the selected style.</returns>
    public static string ClassNames(this CardStyle style) => style switch
    {
        CardStyle.Border => "fw-card-border",
        CardStyle.Dash => "fw-card-dash",
        _ => string.Empty
    };

    /// <summary>
    /// Gets card size classes.
    /// </summary>
    /// <param name="size">The card size variant.</param>
    /// <returns>A CSS class string for the selected size.</returns>
    public static string CardClassNames(this ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-card-xs",
        ComponentSize.Small => "fw-card-sm",
        ComponentSize.Large => "fw-card-lg",
        ComponentSize.ExtraLarge => "fw-card-xl",
        _ => string.Empty
    };
}
