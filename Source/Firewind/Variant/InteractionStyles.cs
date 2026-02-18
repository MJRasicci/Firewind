namespace Firewind.Variant;

/// <summary>
/// Represents size variants for dock components.
/// </summary>
public enum DockSize
{
    /// <summary>
    /// Uses default dock sizing.
    /// </summary>
    Default,
    /// <summary>
    /// Uses extra-small dock sizing.
    /// </summary>
    Tiny,
    /// <summary>
    /// Uses small dock sizing.
    /// </summary>
    Small,
    /// <summary>
    /// Uses medium dock sizing.
    /// </summary>
    Medium,
    /// <summary>
    /// Uses large dock sizing.
    /// </summary>
    Large,
    /// <summary>
    /// Uses extra-large dock sizing.
    /// </summary>
    ExtraLarge
}

/// <summary>
/// Resolves class names for interaction-oriented components.
/// </summary>
public static class InteractionStyleExtensions
{
    /// <summary>
    /// Gets dock size classes.
    /// </summary>
    /// <param name="size">The dock size variant.</param>
    /// <returns>A CSS class string for the selected size.</returns>
    public static string ClassNames(this DockSize size) => size switch
    {
        DockSize.Tiny => "fw-dock-xs",
        DockSize.Small => "fw-dock-sm",
        DockSize.Medium => "fw-dock-md",
        DockSize.Large => "fw-dock-lg",
        DockSize.ExtraLarge => "fw-dock-xl",
        _ => string.Empty
    };
}
