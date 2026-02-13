namespace Firewind.Style;

/// <summary>
/// Resolves CSS classes derived from <see cref="ThemeColor"/> values.
/// </summary>
public static class ThemeStyleExtensions
{
    /// <summary>
    /// Gets background and foreground classes for a theme color.
    /// </summary>
    /// <param name="color">The theme color token.</param>
    /// <returns>
    /// A CSS class string suitable for background usage.
    /// </returns>
    public static string BackgroundClasses(this ThemeColor color) => color switch
    {
        ThemeColor.Base300 => "bg-base-300",
        ThemeColor.Base200 => "bg-base-200",
        ThemeColor.Base100 => "bg-base-100",
        ThemeColor.Neutral => "bg-neutral text-neutral-content",
        ThemeColor.Primary => "bg-primary text-primary-content",
        ThemeColor.Secondary => "bg-secondary text-secondary-content",
        ThemeColor.Accent => "bg-accent text-accent-content",
        ThemeColor.Info => "bg-info text-info-content",
        ThemeColor.Success => "bg-success text-success-content",
        ThemeColor.Warning => "bg-warning text-warning-content",
        ThemeColor.Error => "bg-error text-error-content",
        _ => string.Empty
    };

    /// <summary>
    /// Gets glow or shadow classes for a theme color.
    /// </summary>
    /// <param name="color">The theme color token.</param>
    /// <returns>
    /// A CSS class string suitable for glow/shadow styling.
    /// </returns>
    public static string GlowClasses(this ThemeColor color) => color switch
    {
        ThemeColor.Base300 => "shadow shadow-base-300/40",
        ThemeColor.Base200 => "shadow shadow-base-200/40",
        ThemeColor.Base100 => "shadow shadow-base-100/40",
        ThemeColor.Neutral => "shadow shadow-neutral/40",
        ThemeColor.Primary => "shadow shadow-primary/40",
        ThemeColor.Secondary => "shadow shadow-secondary/40",
        ThemeColor.Accent => "shadow shadow-accent/40",
        ThemeColor.Info => "shadow shadow-info/40",
        ThemeColor.Success => "shadow shadow-success/40",
        ThemeColor.Warning => "shadow shadow-warning/40",
        ThemeColor.Error => "shadow shadow-error/40",
        _ => string.Empty
    };
}
