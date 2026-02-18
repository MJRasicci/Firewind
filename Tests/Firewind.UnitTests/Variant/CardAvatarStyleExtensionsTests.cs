namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies card and avatar-related style mappings.
/// </summary>
public sealed class CardAvatarStyleExtensionsTests
{
    [Theory]
    [InlineData(CardStyle.Default, "")]
    [InlineData(CardStyle.Border, "fw-card-border")]
    [InlineData(CardStyle.Dash, "fw-card-dash")]
    public void CardStyleClassNames_ReturnsExpectedMappings(CardStyle style, string expected)
    {
        var classes = style.ClassNames();

        classes.Should().Be(expected);
    }

    [Theory]
    [InlineData(ComponentSize.Normal, "")]
    [InlineData(ComponentSize.Tiny, "fw-card-xs")]
    [InlineData(ComponentSize.Small, "fw-card-sm")]
    [InlineData(ComponentSize.Large, "fw-card-lg")]
    [InlineData(ComponentSize.ExtraLarge, "fw-card-xl")]
    public void CardClassNames_ReturnsExpectedMappings(ComponentSize size, string expected)
    {
        var classes = size.CardClassNames();

        classes.Should().Be(expected);
    }
}
