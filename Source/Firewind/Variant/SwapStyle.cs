namespace Firewind.Variant;

/// <summary>
/// Defines animation styles for <c>FWSwap</c>.
/// </summary>
public enum SwapStyle
{
    /// <summary>
    /// Uses a fade transition.
    /// </summary>
    Fade,
    /// <summary>
    /// Uses a rotate transition.
    /// </summary>
    Rotate,
    /// <summary>
    /// Uses a flip transition.
    /// </summary>
    Flip
}

/// <summary>
/// Resolves CSS classes for <see cref="SwapStyle"/> values.
/// </summary>
public static class SwapStyleExtensions
{
    /// <summary>
    /// Gets the swap CSS class for a style value.
    /// </summary>
    /// <param name="style">The style value to resolve.</param>
    /// <returns>
    /// A style-specific CSS class, or an empty string when the default style is used.
    /// </returns>
    public static string ClassNames(this SwapStyle style) => style switch
    {
        SwapStyle.Rotate => "fw-swap-rotate",
        SwapStyle.Flip => "fw-swap-flip",
        _ => string.Empty
    };
}
