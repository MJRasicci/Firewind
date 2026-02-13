namespace Firewind.Style;

public enum SwapStyle
{
    Fade,
    Rotate,
    Flip
}

public static class SwapStyleExtensions
{
    public static string ClassNames(this SwapStyle style) => style switch
    {
        SwapStyle.Rotate => "fw-swap-rotate",
        SwapStyle.Flip => "fw-swap-flip",
        _ => string.Empty
    };
}
