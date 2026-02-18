namespace Firewind.Variant;

/// <summary>
/// Represents visual style modifiers for alert components.
/// </summary>
public enum AlertStyle
{
    /// <summary>
    /// Uses default alert styling.
    /// </summary>
    Default,
    /// <summary>
    /// Applies an outlined alert style.
    /// </summary>
    Outline,
    /// <summary>
    /// Applies a dashed alert style.
    /// </summary>
    Dash,
    /// <summary>
    /// Applies a soft alert style.
    /// </summary>
    Soft
}

/// <summary>
/// Represents orientation variants for alert layout.
/// </summary>
public enum AlertDirection
{
    /// <summary>
    /// Uses alert default flow direction.
    /// </summary>
    Default,
    /// <summary>
    /// Stacks alert content vertically.
    /// </summary>
    Vertical,
    /// <summary>
    /// Places alert content horizontally.
    /// </summary>
    Horizontal
}

/// <summary>
/// Represents loading animation variants.
/// </summary>
public enum LoadingStyle
{
    /// <summary>
    /// Uses spinner animation.
    /// </summary>
    Spinner,
    /// <summary>
    /// Uses dots animation.
    /// </summary>
    Dots,
    /// <summary>
    /// Uses ring animation.
    /// </summary>
    Ring,
    /// <summary>
    /// Uses ball animation.
    /// </summary>
    Ball,
    /// <summary>
    /// Uses bars animation.
    /// </summary>
    Bars,
    /// <summary>
    /// Uses infinity animation.
    /// </summary>
    Infinity
}

/// <summary>
/// Resolves class names for feedback and status component variants.
/// </summary>
public static class FeedbackStyleExtensions
{
    /// <summary>
    /// Gets alert style classes.
    /// </summary>
    /// <param name="style">The alert style variant.</param>
    /// <returns>A CSS class string for the selected style.</returns>
    public static string ClassNames(this AlertStyle style) => style switch
    {
        AlertStyle.Outline => "fw-alert-outline",
        AlertStyle.Dash => "fw-alert-dash",
        AlertStyle.Soft => "fw-alert-soft",
        _ => string.Empty
    };

    /// <summary>
    /// Gets alert color classes.
    /// </summary>
    /// <param name="color">The semantic color token.</param>
    /// <returns>A CSS class string for the selected color.</returns>
    public static string AlertClassNames(this ThemeColor color) => color switch
    {
        ThemeColor.Info => "fw-alert-info",
        ThemeColor.Success => "fw-alert-success",
        ThemeColor.Warning => "fw-alert-warning",
        ThemeColor.Error => "fw-alert-error",
        _ => string.Empty
    };

    /// <summary>
    /// Gets alert direction classes.
    /// </summary>
    /// <param name="direction">The alert direction variant.</param>
    /// <returns>A CSS class string for the selected direction.</returns>
    public static string ClassNames(this AlertDirection direction) => direction switch
    {
        AlertDirection.Vertical => "fw-alert-vertical",
        AlertDirection.Horizontal => "fw-alert-horizontal",
        _ => string.Empty
    };

    /// <summary>
    /// Gets loading style classes.
    /// </summary>
    /// <param name="style">The loading style variant.</param>
    /// <returns>A CSS class string for the selected animation style.</returns>
    public static string ClassNames(this LoadingStyle style) => style switch
    {
        LoadingStyle.Dots => "fw-loading-dots",
        LoadingStyle.Ring => "fw-loading-ring",
        LoadingStyle.Ball => "fw-loading-ball",
        LoadingStyle.Bars => "fw-loading-bars",
        LoadingStyle.Infinity => "fw-loading-infinity",
        _ => "fw-loading-spinner"
    };
}
