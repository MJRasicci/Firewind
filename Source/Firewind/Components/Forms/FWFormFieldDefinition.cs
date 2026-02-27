namespace Firewind.Components;

/// <summary>
/// Describes a field rendered by <see cref="FWForm{TModel}"/>.
/// </summary>
public sealed class FWFormFieldDefinition
{
    /// <summary>
    /// Gets or sets the model property path for binding (for example, <c>Address.City</c>).
    /// </summary>
    public required string Path { get; set; }

    /// <summary>
    /// Gets or sets the field label.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets the input control kind.
    /// </summary>
    public FWFormInputKind Kind { get; set; } = FWFormInputKind.Text;

    /// <summary>
    /// Gets or sets a placeholder value used by text-like controls.
    /// </summary>
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the field order value.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets select-list options when <see cref="Kind"/> is <see cref="FWFormInputKind.Select"/>.
    /// </summary>
    public IReadOnlyList<FWFormSelectOption> Options { get; set; } = [];
}
