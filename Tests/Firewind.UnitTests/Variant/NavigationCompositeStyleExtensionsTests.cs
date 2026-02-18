namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies navigation and composite style class mappings.
/// </summary>
public sealed class NavigationCompositeStyleExtensionsTests
{
    [Theory]
    [InlineData(HorizontalAlignment.Start, "fw-navbar-start")]
    [InlineData(HorizontalAlignment.Center, "fw-navbar-center")]
    [InlineData(HorizontalAlignment.End, "fw-navbar-end")]
    public void NavbarClassNames_ReturnsExpectedMappings(HorizontalAlignment alignment, string expected)
    {
        var classes = alignment.NavbarClassNames();

        classes.Should().Be(expected);
    }

    [Theory]
    [InlineData(TabsStyle.Default, "")]
    [InlineData(TabsStyle.Box, "fw-tabs-box")]
    [InlineData(TabsStyle.Border, "fw-tabs-border")]
    [InlineData(TabsStyle.Lift, "fw-tabs-lift")]
    public void TabsStyleClassNames_ReturnsExpectedMappings(TabsStyle style, string expected)
    {
        var classes = style.ClassNames();

        classes.Should().Be(expected);
    }

    [Theory]
    [InlineData(CollapseStyle.Default, "")]
    [InlineData(CollapseStyle.Arrow, "fw-collapse-arrow")]
    [InlineData(CollapseStyle.Plus, "fw-collapse-plus")]
    public void CollapseStyleClassNames_ReturnsExpectedMappings(CollapseStyle style, string expected)
    {
        var classes = style.ClassNames();

        classes.Should().Be(expected);
    }
}
