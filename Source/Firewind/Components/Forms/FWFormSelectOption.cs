namespace Firewind.Components;

/// <summary>
/// Represents a select-option descriptor for a generated form field.
/// </summary>
/// <param name="Value">The option value.</param>
/// <param name="Label">The display label.</param>
public readonly record struct FWFormSelectOption(string Value, string Label);
