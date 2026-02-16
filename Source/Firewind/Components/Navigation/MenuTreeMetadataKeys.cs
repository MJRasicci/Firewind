namespace Firewind.Components;

/// <summary>
/// Defines metadata keys recognized by <see cref="FWMenu{TMenuItem}"/> default tree-node rendering.
/// </summary>
public static class MenuTreeMetadataKeys
{
    /// <summary>
    /// Metadata key for the rendered label text.
    /// </summary>
    public const string Label = "label";

    /// <summary>
    /// Metadata key for the rendered href value.
    /// </summary>
    public const string Href = "href";

    /// <summary>
    /// Metadata key for the action element tag name (for example, <c>a</c> or <c>button</c>).
    /// </summary>
    public const string ActionElement = "as";

    /// <summary>
    /// Metadata key for <see cref="global::Firewind.Variant.MenuItemBehavior"/> flags.
    /// </summary>
    public const string Behavior = "behavior";
}
