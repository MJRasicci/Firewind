namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies media style extension mappings.
/// </summary>
public sealed class MediaStyleExtensionsTests
{
    [Theory]
    [InlineData(MaskShape.Squircle, "fw-mask-squircle")]
    [InlineData(MaskShape.Circle, "fw-mask-circle")]
    [InlineData(MaskShape.Hexagon2, "fw-mask-hexagon-2")]
    [InlineData(MaskShape.Decagon, "fw-mask-decagon")]
    [InlineData(MaskShape.Pentagon, "fw-mask-pentagon")]
    [InlineData(MaskShape.Star, "fw-mask-star")]
    [InlineData(MaskShape.Star2, "fw-mask-star-2")]
    [InlineData(MaskShape.Triangle4, "fw-mask-triangle-4")]
    public void MaskShapeClassNames_ReturnsExpectedMappings(MaskShape shape, string expected)
    {
        shape.ClassNames().Should().Be(expected);
    }

    [Theory]
    [InlineData(MaskHalf.None, "")]
    [InlineData(MaskHalf.First, "fw-mask-half-1")]
    [InlineData(MaskHalf.Second, "fw-mask-half-2")]
    public void MaskHalfClassNames_ReturnsExpectedMappings(MaskHalf half, string expected)
    {
        half.ClassNames().Should().Be(expected);
    }
}
