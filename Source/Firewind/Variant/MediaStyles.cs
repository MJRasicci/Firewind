namespace Firewind.Variant;

/// <summary>
/// Represents supported mask shape variants.
/// </summary>
public enum MaskShape
{
    /// <summary>
    /// Applies squircle mask shape.
    /// </summary>
    Squircle,
    /// <summary>
    /// Applies heart mask shape.
    /// </summary>
    Heart,
    /// <summary>
    /// Applies hexagon mask shape.
    /// </summary>
    Hexagon,
    /// <summary>
    /// Applies diamond mask shape.
    /// </summary>
    Diamond,
    /// <summary>
    /// Applies square mask shape.
    /// </summary>
    Square,
    /// <summary>
    /// Applies circle mask shape.
    /// </summary>
    Circle,
    /// <summary>
    /// Applies star mask shape.
    /// </summary>
    Star,
    /// <summary>
    /// Applies triangle mask shape.
    /// </summary>
    Triangle
}

/// <summary>
/// Resolves class mappings for media and decorative component styles.
/// </summary>
public static class MediaStyleExtensions
{
    /// <summary>
    /// Gets mask shape classes.
    /// </summary>
    /// <param name="shape">The mask shape variant.</param>
    /// <returns>A CSS class string for the selected shape.</returns>
    public static string ClassNames(this MaskShape shape) => shape switch
    {
        MaskShape.Heart => "fw-mask-heart",
        MaskShape.Hexagon => "fw-mask-hexagon",
        MaskShape.Diamond => "fw-mask-diamond",
        MaskShape.Square => "fw-mask-square",
        MaskShape.Circle => "fw-mask-circle",
        MaskShape.Star => "fw-mask-star",
        MaskShape.Triangle => "fw-mask-triangle",
        _ => "fw-mask-squircle"
    };
}
