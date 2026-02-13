namespace Firewind.Style;

/// <summary>
/// Represents placement options for dropdown trigger alignment.
/// </summary>
public enum DropdownAlignment
{
    /// <summary>
    /// Aligns dropdown content to the logical start edge.
    /// </summary>
    Start,
    /// <summary>
    /// Centers dropdown content relative to the trigger.
    /// </summary>
    Center,
    /// <summary>
    /// Aligns dropdown content to the logical end edge.
    /// </summary>
    End
}

/// <summary>
/// Represents direction options for dropdown content.
/// </summary>
public enum DropdownDirection
{
    /// <summary>
    /// Opens below the trigger.
    /// </summary>
    Bottom,
    /// <summary>
    /// Opens above the trigger.
    /// </summary>
    Top,
    /// <summary>
    /// Opens to the left of the trigger.
    /// </summary>
    Left,
    /// <summary>
    /// Opens to the right of the trigger.
    /// </summary>
    Right
}

/// <summary>
/// Resolves CSS classes for dropdown variants.
/// </summary>
public static class DropdownStyleExtensions
{
    /// <summary>
    /// Gets CSS classes for dropdown alignment.
    /// </summary>
    /// <param name="alignment">The alignment value to resolve.</param>
    /// <returns>A CSS class string for the selected alignment.</returns>
    public static string ClassNames(this DropdownAlignment alignment) => alignment switch
    {
        DropdownAlignment.Center => "fw-dropdown-center",
        DropdownAlignment.End => "fw-dropdown-end",
        _ => "fw-dropdown-start"
    };

    /// <summary>
    /// Gets CSS classes for dropdown direction.
    /// </summary>
    /// <param name="direction">The direction value to resolve.</param>
    /// <returns>A CSS class string for the selected direction.</returns>
    public static string ClassNames(this DropdownDirection direction) => direction switch
    {
        DropdownDirection.Top => "fw-dropdown-top",
        DropdownDirection.Left => "fw-dropdown-left",
        DropdownDirection.Right => "fw-dropdown-right",
        _ => "fw-dropdown-bottom"
    };
}

/// <summary>
/// Represents orientation options for menu layouts.
/// </summary>
public enum MenuOrientation
{
    /// <summary>
    /// Renders items in a vertical stack.
    /// </summary>
    Vertical,
    /// <summary>
    /// Renders items in a horizontal row.
    /// </summary>
    Horizontal
}

/// <summary>
/// Represents size options for menus.
/// </summary>
public enum MenuSize
{
    /// <summary>
    /// Uses default menu sizing.
    /// </summary>
    Normal,
    /// <summary>
    /// Uses extra-small menu sizing.
    /// </summary>
    Tiny,
    /// <summary>
    /// Uses small menu sizing.
    /// </summary>
    Small,
    /// <summary>
    /// Uses medium menu sizing.
    /// </summary>
    Medium,
    /// <summary>
    /// Uses large menu sizing.
    /// </summary>
    Large,
    /// <summary>
    /// Uses extra-large menu sizing.
    /// </summary>
    ExtraLarge
}

/// <summary>
/// Resolves CSS classes for menu variants.
/// </summary>
public static class MenuStyleExtensions
{
    /// <summary>
    /// Gets CSS classes for menu orientation.
    /// </summary>
    /// <param name="orientation">The orientation value to resolve.</param>
    /// <returns>A CSS class string for the selected orientation.</returns>
    public static string ClassNames(this MenuOrientation orientation) => orientation switch
    {
        MenuOrientation.Horizontal => "fw-menu-horizontal",
        _ => "fw-menu-vertical"
    };

    /// <summary>
    /// Gets CSS classes for menu size.
    /// </summary>
    /// <param name="size">The size value to resolve.</param>
    /// <returns>A CSS class string for the selected size.</returns>
    public static string ClassNames(this MenuSize size) => size switch
    {
        MenuSize.Tiny => "fw-menu-xs",
        MenuSize.Small => "fw-menu-sm",
        MenuSize.Medium => "fw-menu-md",
        MenuSize.Large => "fw-menu-lg",
        MenuSize.ExtraLarge => "fw-menu-xl",
        _ => string.Empty
    };
}

/// <summary>
/// Represents orientation options for <c>FWJoin</c>.
/// </summary>
public enum JoinOrientation
{
    /// <summary>
    /// Renders joined elements in a horizontal row.
    /// </summary>
    Horizontal,
    /// <summary>
    /// Renders joined elements in a vertical column.
    /// </summary>
    Vertical
}

/// <summary>
/// Resolves CSS classes for join orientation variants.
/// </summary>
public static class JoinStyleExtensions
{
    /// <summary>
    /// Gets CSS classes for join orientation.
    /// </summary>
    /// <param name="orientation">The orientation value to resolve.</param>
    /// <returns>A CSS class string for the selected orientation.</returns>
    public static string ClassNames(this JoinOrientation orientation) => orientation switch
    {
        JoinOrientation.Vertical => "fw-join-vertical",
        _ => "fw-join-horizontal"
    };
}

/// <summary>
/// Represents orientation options for <c>FWDivider</c>.
/// </summary>
public enum DividerOrientation
{
    /// <summary>
    /// Uses the default horizontal divider rendering.
    /// </summary>
    Vertical,
    /// <summary>
    /// Uses the vertical divider rendering.
    /// </summary>
    Horizontal
}

/// <summary>
/// Resolves CSS classes for divider variants.
/// </summary>
public static class DividerStyleExtensions
{
    /// <summary>
    /// Gets CSS classes for divider orientation.
    /// </summary>
    /// <param name="orientation">The orientation value to resolve.</param>
    /// <returns>A CSS class string for the selected orientation.</returns>
    public static string ClassNames(this DividerOrientation orientation) => orientation switch
    {
        DividerOrientation.Horizontal => "fw-divider-horizontal",
        _ => "fw-divider-vertical"
    };

    /// <summary>
    /// Gets CSS classes for divider line alignment.
    /// </summary>
    /// <param name="position">The line alignment position.</param>
    /// <returns>A CSS class string for the selected position.</returns>
    public static string DividerClassNames(this ElementPosition position) => position switch
    {
        ElementPosition.Start => "fw-divider-start",
        ElementPosition.End => "fw-divider-end",
        _ => string.Empty
    };

    /// <summary>
    /// Gets CSS classes for divider color.
    /// </summary>
    /// <param name="color">The theme color value to resolve.</param>
    /// <returns>A CSS class string for the selected divider color.</returns>
    public static string DividerClassNames(this ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-divider-neutral",
        ThemeColor.Primary => "fw-divider-primary",
        ThemeColor.Secondary => "fw-divider-secondary",
        ThemeColor.Accent => "fw-divider-accent",
        ThemeColor.Info => "fw-divider-info",
        ThemeColor.Success => "fw-divider-success",
        ThemeColor.Warning => "fw-divider-warning",
        ThemeColor.Error => "fw-divider-error",
        _ => string.Empty
    };
}

/// <summary>
/// Resolves CSS classes for link variants.
/// </summary>
public static class LinkStyleExtensions
{
    /// <summary>
    /// Gets CSS classes for link color.
    /// </summary>
    /// <param name="color">The theme color value to resolve.</param>
    /// <returns>A CSS class string for the selected link color.</returns>
    public static string LinkClassNames(this ThemeColor color) => color switch
    {
        ThemeColor.Neutral => "fw-link-neutral",
        ThemeColor.Primary => "fw-link-primary",
        ThemeColor.Secondary => "fw-link-secondary",
        ThemeColor.Accent => "fw-link-accent",
        ThemeColor.Info => "fw-link-info",
        ThemeColor.Success => "fw-link-success",
        ThemeColor.Warning => "fw-link-warning",
        ThemeColor.Error => "fw-link-error",
        _ => string.Empty
    };
}

/// <summary>
/// Represents placement options for the modal container.
/// </summary>
public enum ModalPlacement
{
    /// <summary>
    /// Uses the default centered modal placement.
    /// </summary>
    Default,
    /// <summary>
    /// Places the modal at the top edge.
    /// </summary>
    Top,
    /// <summary>
    /// Places the modal in the middle.
    /// </summary>
    Middle,
    /// <summary>
    /// Places the modal at the bottom edge.
    /// </summary>
    Bottom,
    /// <summary>
    /// Places the modal at the start edge.
    /// </summary>
    Start,
    /// <summary>
    /// Places the modal at the end edge.
    /// </summary>
    End
}

/// <summary>
/// Resolves CSS classes for modal placement variants.
/// </summary>
public static class ModalStyleExtensions
{
    /// <summary>
    /// Gets CSS classes for modal placement.
    /// </summary>
    /// <param name="placement">The placement value to resolve.</param>
    /// <returns>A CSS class string for the selected placement.</returns>
    public static string ClassNames(this ModalPlacement placement) => placement switch
    {
        ModalPlacement.Top => "fw-modal-top",
        ModalPlacement.Middle => "fw-modal-middle",
        ModalPlacement.Bottom => "fw-modal-bottom",
        ModalPlacement.Start => "fw-modal-start",
        ModalPlacement.End => "fw-modal-end",
        _ => string.Empty
    };
}
