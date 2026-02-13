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

public static class ButtonShapeExtensions
{
    public static string ClassNames(this ButtonShape shape) => shape switch
    {
        ButtonShape.Wide => "fw-btn-wide",
        ButtonShape.Block => "fw-btn-block",
        ButtonShape.Square => "fw-btn-square",
        ButtonShape.Circle => "fw-btn-circle",
        _ => string.Empty
    };
}

public static class ButtonSizeExtensions
{
    public static string ButtonClassNames(this ComponentSize size) => size switch
    {
        ComponentSize.Tiny => "fw-btn-xs",
        ComponentSize.Small => "fw-btn-sm",
        ComponentSize.Large => "fw-btn-lg",
        ComponentSize.Responsive => "fw-btn-xs sm:fw-btn-sm md:fw-btn-md lg:fw-btn-lg",
        _ => string.Empty
    };
}
