namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies interaction style extension mappings.
/// </summary>
public sealed class InteractionStyleExtensionsTests
{
    [Theory]
    [InlineData(DockSize.Default, "")]
    [InlineData(DockSize.Tiny, "fw-dock-xs")]
    [InlineData(DockSize.Small, "fw-dock-sm")]
    [InlineData(DockSize.Medium, "fw-dock-md")]
    [InlineData(DockSize.Large, "fw-dock-lg")]
    [InlineData(DockSize.ExtraLarge, "fw-dock-xl")]
    public void DockSizeClassNames_ReturnsExpectedMappings(DockSize size, string expected)
    {
        size.ClassNames().Should().Be(expected);
    }
}
