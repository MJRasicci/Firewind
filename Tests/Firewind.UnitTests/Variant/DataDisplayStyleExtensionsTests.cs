namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies data-display style extension mappings.
/// </summary>
public sealed class DataDisplayStyleExtensionsTests
{
    [Theory]
    [InlineData(ToastPlacement.TopEnd, "fw-toast-top fw-toast-end")]
    [InlineData(ToastPlacement.BottomCenter, "fw-toast-bottom fw-toast-center")]
    [InlineData(ToastPlacement.MiddleStart, "fw-toast-middle fw-toast-start")]
    public void ToastClassNames_ReturnsExpectedMappings(ToastPlacement placement, string expected)
    {
        placement.ClassNames().Should().Be(expected);
    }

    [Theory]
    [InlineData(AxisDirection.Vertical, "fw-timeline-vertical")]
    [InlineData(AxisDirection.Horizontal, "fw-timeline-horizontal")]
    public void TimelineClassNames_ReturnsExpectedMappings(AxisDirection direction, string expected)
    {
        direction.TimelineClassNames().Should().Be(expected);
    }

    [Theory]
    [InlineData(StackAlignment.Default, "")]
    [InlineData(StackAlignment.Top, "fw-stack-top")]
    [InlineData(StackAlignment.Bottom, "fw-stack-bottom")]
    public void StackClassNames_ReturnsExpectedMappings(StackAlignment alignment, string expected)
    {
        alignment.ClassNames().Should().Be(expected);
    }

    [Theory]
    [InlineData(ThemeColor.None, "")]
    [InlineData(ThemeColor.Primary, "fw-step-primary")]
    [InlineData(ThemeColor.Warning, "fw-step-warning")]
    public void StepClassNames_ReturnsExpectedMappings(ThemeColor color, string expected)
    {
        color.StepClassNames().Should().Be(expected);
    }
}
