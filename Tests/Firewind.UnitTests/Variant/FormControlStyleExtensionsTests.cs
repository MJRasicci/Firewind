namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies form control style extension class mappings.
/// </summary>
public sealed class FormControlStyleExtensionsTests
{
    [Fact]
    public void FieldStyleClassNames_ReturnsGhostClass()
    {
        var classes = FieldStyle.Ghost.ClassNames("input");

        classes.Should().Be("fw-input-ghost");
    }

    [Theory]
    [InlineData(ThemeColor.None, "")]
    [InlineData(ThemeColor.Primary, "fw-input-primary")]
    [InlineData(ThemeColor.Warning, "fw-input-warning")]
    public void ThemeColorFormControlClassNames_ReturnsExpectedClasses(ThemeColor color, string expected)
    {
        var classes = color.FormControlClassNames("input");

        classes.Should().Be(expected);
    }

    [Theory]
    [InlineData(ComponentSize.Normal, "")]
    [InlineData(ComponentSize.Tiny, "fw-checkbox-xs")]
    [InlineData(ComponentSize.Small, "fw-checkbox-sm")]
    [InlineData(ComponentSize.Large, "fw-checkbox-lg")]
    [InlineData(ComponentSize.ExtraLarge, "fw-checkbox-xl")]
    public void FormControlSizeClassNames_ReturnsExpectedClasses(ComponentSize size, string expected)
    {
        var classes = size.FormControlClassNames("checkbox");

        classes.Should().Be(expected);
    }

    [Fact]
    public void FormControlSizeClassNames_ReturnsResponsiveClasses()
    {
        var classes = ComponentSize.Responsive.FormControlClassNames("input");

        classes.Should().Be("fw-input-xs sm:fw-input-sm md:fw-input-md lg:fw-input-lg xl:fw-input-xl");
    }

    [Fact]
    public void FormControlClassNames_ThrowsForInvalidToken()
    {
        var action = () => ThemeColor.Primary.FormControlClassNames(" ");

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Component token cannot be null or whitespace.*");
    }
}
