namespace Firewind.Style;

public enum ButtonStyle
{
    None,
    Neutral,
    Primary,
    Secondary,
    Accent,
    Info,
    Success,
    Warning,
    Error,
    Ghost,
    Link
}

public enum ButtonShape
{
    Normal,
    Wide,
    Block,
    Square,
    Circle,
}

public static class ButtonStyleExtensions
{
    public static string ClassNames(this ButtonStyle style) => style switch
    {
        _ => ""
    };
}
