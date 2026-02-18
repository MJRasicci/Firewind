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
    [InlineData(MaskShape.Star, "fw-mask-star")]
    public void MaskShapeClassNames_ReturnsExpectedMappings(MaskShape shape, string expected)
    {
        shape.ClassNames().Should().Be(expected);
    }
}
