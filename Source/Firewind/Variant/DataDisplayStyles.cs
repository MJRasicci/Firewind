namespace Firewind.Variant;

/// <summary>
/// Represents placement options for toast containers.
/// </summary>
public enum ToastPlacement
{
    /// <summary>
    /// Positions toast at the top end corner.
    /// </summary>
    TopEnd,
    /// <summary>
    /// Positions toast at the top center.
    /// </summary>
    TopCenter,
    /// <summary>
    /// Positions toast at the top start corner.
    /// </summary>
    TopStart,
    /// <summary>
    /// Positions toast at the middle end side.
    /// </summary>
    MiddleEnd,
    /// <summary>
    /// Positions toast at the middle center.
    /// </summary>
    MiddleCenter,
    /// <summary>
    /// Positions toast at the middle start side.
    /// </summary>
    MiddleStart,
    /// <summary>
    /// Positions toast at the bottom end corner.
    /// </summary>
    BottomEnd,
    /// <summary>
    /// Positions toast at the bottom center.
    /// </summary>
    BottomCenter,
    /// <summary>
    /// Positions toast at the bottom start corner.
    /// </summary>
    BottomStart
}

/// <summary>
/// Represents directional options for timeline and steps components.
/// </summary>
public enum AxisDirection
{
    /// <summary>
    /// Uses vertical orientation.
    /// </summary>
    Vertical,
    /// <summary>
    /// Uses horizontal orientation.
    /// </summary>
    Horizontal
}

/// <summary>
/// Represents alignment modifiers for stack layout.
/// </summary>
public enum StackAlignment
{
    /// <summary>
    /// Uses default stack alignment.
    /// </summary>
    Default,
    /// <summary>
    /// Aligns stack items toward top.
    /// </summary>
    Top,
    /// <summary>
    /// Aligns stack items toward bottom.
    /// </summary>
    Bottom,
    /// <summary>
    /// Aligns stack items toward start edge.
    /// </summary>
    Start,
    /// <summary>
    /// Aligns stack items toward end edge.
    /// </summary>
    End
}

/// <summary>
/// Resolves class mappings for data-display components.
/// </summary>
public static class DataDisplayStyleExtensions
{
    /// <summary>
    /// Gets classes for toast placement.
    /// </summary>
    public static string ClassNames(this ToastPlacement placement) => placement switch
    {
        ToastPlacement.TopCenter => "fw-toast-top fw-toast-center",
        ToastPlacement.TopStart => "fw-toast-top fw-toast-start",
        ToastPlacement.MiddleEnd => "fw-toast-middle fw-toast-end",
        ToastPlacement.MiddleCenter => "fw-toast-middle fw-toast-center",
        ToastPlacement.MiddleStart => "fw-toast-middle fw-toast-start",
        ToastPlacement.BottomEnd => "fw-toast-bottom fw-toast-end",
        ToastPlacement.BottomCenter => "fw-toast-bottom fw-toast-center",
        ToastPlacement.BottomStart => "fw-toast-bottom fw-toast-start",
        _ => "fw-toast-top fw-toast-end"
    };

    /// <summary>
    /// Gets timeline direction classes.
    /// </summary>
    public static string TimelineClassNames(this AxisDirection direction) => direction switch
    {
        AxisDirection.Horizontal => "fw-timeline-horizontal",
        _ => "fw-timeline-vertical"
    };

    /// <summary>
    /// Gets steps direction classes.
    /// </summary>
    public static string StepsClassNames(this AxisDirection direction) => direction switch
    {
        AxisDirection.Horizontal => "fw-steps-horizontal",
        _ => "fw-steps-vertical"
    };

    /// <summary>
    /// Gets stack alignment classes.
    /// </summary>
    public static string ClassNames(this StackAlignment alignment) => alignment switch
    {
        StackAlignment.Top => "fw-stack-top",
        StackAlignment.Bottom => "fw-stack-bottom",
        StackAlignment.Start => "fw-stack-start",
        StackAlignment.End => "fw-stack-end",
        _ => string.Empty
    };

    /// <summary>
    /// Gets stats orientation classes.
    /// </summary>
    public static string StatsClassNames(this AxisDirection direction) => direction switch
    {
        AxisDirection.Vertical => "fw-stats-vertical",
        _ => "fw-stats-horizontal"
    };

    /// <summary>
    /// Gets step color classes.
    /// </summary>
    public static string StepClassNames(this ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-step-neutral",
        ThemeColor.Primary => "fw-step-primary",
        ThemeColor.Secondary => "fw-step-secondary",
        ThemeColor.Accent => "fw-step-accent",
        ThemeColor.Info => "fw-step-info",
        ThemeColor.Success => "fw-step-success",
        ThemeColor.Warning => "fw-step-warning",
        ThemeColor.Error => "fw-step-error",
        _ => string.Empty
    };
}
