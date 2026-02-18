namespace Firewind.Variant;

/// <summary>
/// Represents horizontal alignment options for layout sections.
/// </summary>
public enum HorizontalAlignment
{
    /// <summary>
    /// Aligns content to the start edge.
    /// </summary>
    Start,
    /// <summary>
    /// Aligns content to the center.
    /// </summary>
    Center,
    /// <summary>
    /// Aligns content to the end edge.
    /// </summary>
    End
}

/// <summary>
/// Represents visual style variants for tabs containers.
/// </summary>
public enum TabsStyle
{
    /// <summary>
    /// Uses the default tabs style.
    /// </summary>
    Default,
    /// <summary>
    /// Applies boxed tabs styling.
    /// </summary>
    Box,
    /// <summary>
    /// Applies bordered tabs styling.
    /// </summary>
    Border,
    /// <summary>
    /// Applies lifted tabs styling.
    /// </summary>
    Lift
}

/// <summary>
/// Represents placement options for tabs.
/// </summary>
public enum TabsPlacement
{
    /// <summary>
    /// Places tabs at the top.
    /// </summary>
    Top,
    /// <summary>
    /// Places tabs at the bottom.
    /// </summary>
    Bottom
}

/// <summary>
/// Represents trigger style variants for collapse components.
/// </summary>
public enum CollapseStyle
{
    /// <summary>
    /// Uses default collapse styling.
    /// </summary>
    Default,
    /// <summary>
    /// Uses arrow indicator styling.
    /// </summary>
    Arrow,
    /// <summary>
    /// Uses plus indicator styling.
    /// </summary>
    Plus
}

/// <summary>
/// Resolves class names for navigation and composite layout variants.
/// </summary>
public static class NavigationCompositeStyleExtensions
{
    /// <summary>
    /// Gets navbar section classes for the provided alignment.
    /// </summary>
    /// <param name="alignment">The alignment value.</param>
    /// <returns>A CSS class string for the selected alignment.</returns>
    public static string NavbarClassNames(this HorizontalAlignment alignment) => alignment switch
    {
        HorizontalAlignment.Center => "fw-navbar-center",
        HorizontalAlignment.End => "fw-navbar-end",
        _ => "fw-navbar-start"
    };

    /// <summary>
    /// Gets footer direction classes.
    /// </summary>
    /// <param name="orientation">The footer orientation.</param>
    /// <returns>A CSS class string for the selected orientation.</returns>
    public static string FooterClassNames(this JoinOrientation orientation) => orientation switch
    {
        JoinOrientation.Horizontal => "fw-footer-horizontal",
        _ => "fw-footer-vertical"
    };

    /// <summary>
    /// Gets tabs style classes.
    /// </summary>
    /// <param name="style">The tabs style variant.</param>
    /// <returns>A CSS class string for the selected style.</returns>
    public static string ClassNames(this TabsStyle style) => style switch
    {
        TabsStyle.Box => "fw-tabs-box",
        TabsStyle.Border => "fw-tabs-border",
        TabsStyle.Lift => "fw-tabs-lift",
        _ => string.Empty
    };

    /// <summary>
    /// Gets tabs placement classes.
    /// </summary>
    /// <param name="placement">The tabs placement value.</param>
    /// <returns>A CSS class string for the selected placement.</returns>
    public static string ClassNames(this TabsPlacement placement) => placement switch
    {
        TabsPlacement.Bottom => "fw-tabs-bottom",
        _ => "fw-tabs-top"
    };

    /// <summary>
    /// Gets collapse style classes.
    /// </summary>
    /// <param name="style">The collapse style variant.</param>
    /// <returns>A CSS class string for the selected style.</returns>
    public static string ClassNames(this CollapseStyle style) => style switch
    {
        CollapseStyle.Arrow => "fw-collapse-arrow",
        CollapseStyle.Plus => "fw-collapse-plus",
        _ => string.Empty
    };
}
