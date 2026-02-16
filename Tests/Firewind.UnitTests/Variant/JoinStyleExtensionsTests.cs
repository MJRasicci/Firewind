namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies join orientation variant class mappings.
/// </summary>
public sealed class JoinStyleExtensionsTests
{
    /// <summary>
    /// Validates orientation-to-class mappings for joined layouts.
    /// </summary>
    /// <param name="orientation">The orientation value to resolve.</param>
    /// <param name="expectedClasses">The expected CSS class output.</param>
    [Theory]
    [InlineData(JoinOrientation.Horizontal, "fw-join-horizontal")]
    [InlineData(JoinOrientation.Vertical, "fw-join-vertical")]
    public void ClassNames_ReturnsExpectedMappings(JoinOrientation orientation, string expectedClasses)
    {
        var classes = orientation.ClassNames();

        classes.Should().Be(expectedClasses);
    }
}
