namespace Firewind.Style;

/// <summary>
/// Composes CSS classes from base classes and conditional variant rules.
/// </summary>
internal sealed class VariantClassBuilder
{
    private readonly CssClassList cssClasses;

    /// <summary>
    /// Initializes a new instance of the <see cref="VariantClassBuilder"/> class.
    /// </summary>
    /// <param name="baseClasses">Optional base classes to seed the builder with.</param>
    public VariantClassBuilder(string? baseClasses = null)
    {
        this.cssClasses = new(baseClasses ?? string.Empty);
    }

    /// <summary>
    /// Adds classes to the builder.
    /// </summary>
    /// <param name="classes">One or more CSS classes to add.</param>
    /// <returns>The current builder instance.</returns>
    public VariantClassBuilder Add(string? classes)
    {
        this.cssClasses.Add(classes ?? string.Empty);
        return this;
    }

    /// <summary>
    /// Adds classes only when the specified condition is <see langword="true"/>.
    /// </summary>
    /// <param name="condition">Determines whether classes are added.</param>
    /// <param name="classes">Classes to add when <paramref name="condition"/> is true.</param>
    /// <returns>The current builder instance.</returns>
    public VariantClassBuilder AddIf(bool condition, string? classes)
    {
        if (condition)
        {
            this.cssClasses.Add(classes ?? string.Empty);
        }

        return this;
    }

    /// <summary>
    /// Resolves and adds classes for a variant value.
    /// </summary>
    /// <typeparam name="TVariant">The variant type.</typeparam>
    /// <param name="variant">The variant value.</param>
    /// <param name="resolver">A function that maps the variant to a class string.</param>
    /// <returns>The current builder instance.</returns>
    public VariantClassBuilder AddVariant<TVariant>(TVariant variant, Func<TVariant, string?> resolver)
    {
        this.cssClasses.Add(resolver(variant) ?? string.Empty);
        return this;
    }

    /// <summary>
    /// Builds the final class string.
    /// </summary>
    /// <returns>A space-delimited CSS class string.</returns>
    public string Build() => this.cssClasses.ToString();
}
