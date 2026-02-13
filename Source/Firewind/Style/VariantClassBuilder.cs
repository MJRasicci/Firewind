namespace Firewind.Style;

/// <summary>
/// Composes CSS classes from base classes and conditional variant rules.
/// </summary>
internal sealed class VariantClassBuilder
{
    private readonly CssClassList cssClasses;

    public VariantClassBuilder(string? baseClasses = null)
    {
        this.cssClasses = new(baseClasses ?? string.Empty);
    }

    public VariantClassBuilder Add(string? classes)
    {
        this.cssClasses.Add(classes ?? string.Empty);
        return this;
    }

    public VariantClassBuilder AddIf(bool condition, string? classes)
    {
        if (condition)
        {
            this.cssClasses.Add(classes ?? string.Empty);
        }

        return this;
    }

    public VariantClassBuilder AddVariant<TVariant>(TVariant variant, Func<TVariant, string?> resolver)
    {
        this.cssClasses.Add(resolver(variant) ?? string.Empty);
        return this;
    }

    public string Build() => this.cssClasses.ToString();
}
