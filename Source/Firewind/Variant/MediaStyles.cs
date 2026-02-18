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
    /// Applies alternate hexagon mask shape.
    /// </summary>
    Hexagon2,
    /// <summary>
    /// Applies decagon mask shape.
    /// </summary>
    Decagon,
    /// <summary>
    /// Applies pentagon mask shape.
    /// </summary>
    Pentagon,
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
    /// Applies alternate star mask shape.
    /// </summary>
    Star2,
    /// <summary>
    /// Applies triangle mask shape.
    /// </summary>
    Triangle,
    /// <summary>
    /// Applies second triangle mask shape.
    /// </summary>
    Triangle2,
    /// <summary>
    /// Applies third triangle mask shape.
    /// </summary>
    Triangle3,
    /// <summary>
    /// Applies fourth triangle mask shape.
    /// </summary>
    Triangle4
}

/// <summary>
/// Represents optional half-mask modifiers.
/// </summary>
public enum MaskHalf
{
    /// <summary>
    /// Applies no half-mask modifier.
    /// </summary>
    None,
    /// <summary>
    /// Applies the first half-mask modifier.
    /// </summary>
    First,
    /// <summary>
    /// Applies the second half-mask modifier.
    /// </summary>
    Second
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
        MaskShape.Hexagon2 => "fw-mask-hexagon-2",
        MaskShape.Decagon => "fw-mask-decagon",
        MaskShape.Pentagon => "fw-mask-pentagon",
        MaskShape.Diamond => "fw-mask-diamond",
        MaskShape.Square => "fw-mask-square",
        MaskShape.Circle => "fw-mask-circle",
        MaskShape.Star => "fw-mask-star",
        MaskShape.Star2 => "fw-mask-star-2",
        MaskShape.Triangle => "fw-mask-triangle",
        MaskShape.Triangle2 => "fw-mask-triangle-2",
        MaskShape.Triangle3 => "fw-mask-triangle-3",
        MaskShape.Triangle4 => "fw-mask-triangle-4",
        _ => "fw-mask-squircle"
    };

    /// <summary>
    /// Gets mask half-modifier classes.
    /// </summary>
    /// <param name="half">The half-mask modifier.</param>
    /// <returns>A CSS class string for the selected half modifier.</returns>
    public static string ClassNames(this MaskHalf half) => half switch
    {
        MaskHalf.First => "fw-mask-half-1",
        MaskHalf.Second => "fw-mask-half-2",
        _ => string.Empty
    };
}
