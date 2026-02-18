namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies feedback-related variant class mappings.
/// </summary>
public sealed class FeedbackStyleExtensionsTests
{
    [Theory]
    [InlineData(AlertStyle.Default, "")]
    [InlineData(AlertStyle.Outline, "fw-alert-outline")]
    [InlineData(AlertStyle.Dash, "fw-alert-dash")]
    [InlineData(AlertStyle.Soft, "fw-alert-soft")]
    public void AlertStyleClassNames_ReturnsExpectedMappings(AlertStyle style, string expected)
    {
        var classes = style.ClassNames();

        classes.Should().Be(expected);
    }

    [Theory]
    [InlineData(ThemeColor.None, "")]
    [InlineData(ThemeColor.Info, "fw-alert-info")]
    [InlineData(ThemeColor.Success, "fw-alert-success")]
    [InlineData(ThemeColor.Warning, "fw-alert-warning")]
    [InlineData(ThemeColor.Error, "fw-alert-error")]
    public void AlertClassNames_ReturnsExpectedMappings(ThemeColor color, string expected)
    {
        var classes = color.AlertClassNames();

        classes.Should().Be(expected);
    }

    [Theory]
    [InlineData(LoadingStyle.Spinner, "fw-loading-spinner")]
    [InlineData(LoadingStyle.Dots, "fw-loading-dots")]
    [InlineData(LoadingStyle.Ring, "fw-loading-ring")]
    [InlineData(LoadingStyle.Ball, "fw-loading-ball")]
    [InlineData(LoadingStyle.Bars, "fw-loading-bars")]
    [InlineData(LoadingStyle.Infinity, "fw-loading-infinity")]
    public void LoadingStyleClassNames_ReturnsExpectedMappings(LoadingStyle style, string expected)
    {
        var classes = style.ClassNames();

        classes.Should().Be(expected);
    }
}
