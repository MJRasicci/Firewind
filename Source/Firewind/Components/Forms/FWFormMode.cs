namespace Firewind.Components;

/// <summary>
/// Defines how <see cref="FWForm{TModel}"/> renders fields.
/// </summary>
public enum FWFormMode
{
    /// <summary>
    /// Automatically chooses dynamic mode when fields are provided or auto generation is enabled.
    /// </summary>
    Auto,

    /// <summary>
    /// Renders fields from <see cref="FWForm{TModel}.Fields"/> or generated model metadata.
    /// </summary>
    Dynamic,

    /// <summary>
    /// Renders explicit child content without generated fields.
    /// </summary>
    Static
}
