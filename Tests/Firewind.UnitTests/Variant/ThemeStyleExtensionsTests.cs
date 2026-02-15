namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies theme color variant class mappings.
/// </summary>
public sealed class ThemeStyleExtensionsTests
{
    /// <summary>
    /// Validates that background classes are resolved for representative theme colors.
    /// </summary>
    /// <param name="color">The theme color to resolve.</param>
    /// <param name="expectedClasses">The expected CSS class output.</param>
    [Theory]
    [InlineData(ThemeColor.None, "")]
    [InlineData(ThemeColor.Primary, "bg-primary text-primary-content")]
    [InlineData(ThemeColor.Base200, "bg-base-200")]
    [InlineData(ThemeColor.Error, "bg-error text-error-content")]
    public void BackgroundClasses_ReturnsExpectedMappings(ThemeColor color, string expectedClasses)
    {
        var classes = color.BackgroundClasses();

        classes.Should().Be(expectedClasses);
    }

    /// <summary>
    /// Validates that glow classes are resolved for representative theme colors.
    /// </summary>
    /// <param name="color">The theme color to resolve.</param>
    /// <param name="expectedClasses">The expected CSS class output.</param>
    [Theory]
    [InlineData(ThemeColor.None, "")]
    [InlineData(ThemeColor.Secondary, "shadow shadow-secondary/40")]
    [InlineData(ThemeColor.Base300, "shadow shadow-base-300/40")]
    [InlineData(ThemeColor.Warning, "shadow shadow-warning/40")]
    public void GlowClasses_ReturnsExpectedMappings(ThemeColor color, string expectedClasses)
    {
        var classes = color.GlowClasses();

        classes.Should().Be(expectedClasses);
    }
}
