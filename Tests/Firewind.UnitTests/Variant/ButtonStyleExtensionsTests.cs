namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies button style variant class mappings.
/// </summary>
public sealed class ButtonStyleExtensionsTests
{
    /// <summary>
    /// Validates style-to-class mappings for all button variants.
    /// </summary>
    /// <param name="style">The style value to resolve.</param>
    /// <param name="expectedClasses">The expected CSS class output.</param>
    [Theory]
    [InlineData(ButtonStyle.None, "")]
    [InlineData(ButtonStyle.Neutral, "fw-btn-neutral text-neutral-content")]
    [InlineData(ButtonStyle.Primary, "fw-btn-primary text-primary-content")]
    [InlineData(ButtonStyle.Secondary, "fw-btn-secondary text-secondary-content")]
    [InlineData(ButtonStyle.Accent, "fw-btn-accent text-accent-content")]
    [InlineData(ButtonStyle.Info, "fw-btn-info text-info-content")]
    [InlineData(ButtonStyle.Success, "fw-btn-success text-success-content")]
    [InlineData(ButtonStyle.Warning, "fw-btn-warning text-warning-content")]
    [InlineData(ButtonStyle.Error, "fw-btn-error text-error-content")]
    [InlineData(ButtonStyle.Ghost, "fw-btn-ghost")]
    [InlineData(ButtonStyle.Link, "fw-btn-link")]
    public void ClassNames_ReturnsExpectedMappings(ButtonStyle style, string expectedClasses)
    {
        var classes = style.ClassNames();

        classes.Should().Be(expectedClasses);
    }
}
