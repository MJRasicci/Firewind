namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies form control style extension class mappings.
/// </summary>
public sealed class FormControlStyleExtensionsTests
{
    [Theory]
    [InlineData(FieldStyle.Default, "input", "")]
    [InlineData(FieldStyle.Ghost, "input", "fw-input-ghost")]
    [InlineData(FieldStyle.Ghost, "select", "fw-select-ghost")]
    [InlineData(FieldStyle.Ghost, "textarea", "fw-textarea-ghost")]
    [InlineData(FieldStyle.Ghost, "file-input", "fw-file-input-ghost")]
    [InlineData(FieldStyle.Ghost, "badge", "fw-badge-ghost")]
    [InlineData(FieldStyle.Ghost, "InPuT", "fw-input-ghost")]
    public void FieldStyleClassNames_ReturnsExpectedClasses(FieldStyle style, string token, string expected)
    {
        var classes = style.ClassNames(token);

        classes.Should().Be(expected);
    }

    [Theory]
    [InlineData(ThemeColor.None, "input", "")]
    [InlineData(ThemeColor.Primary, "input", "fw-input-primary")]
    [InlineData(ThemeColor.Warning, "textarea", "fw-textarea-warning")]
    [InlineData(ThemeColor.Success, "badge", "fw-badge-success")]
    [InlineData(ThemeColor.Neutral, "progress", "fw-progress-neutral")]
    [InlineData(ThemeColor.Error, "toggle", "fw-toggle-error")]
    [InlineData(ThemeColor.Accent, "file-input", "fw-file-input-accent")]
    public void ThemeColorFormControlClassNames_ReturnsExpectedClasses(ThemeColor color, string token, string expected)
    {
        var classes = color.FormControlClassNames(token);

        classes.Should().Be(expected);
    }

    [Theory]
    [InlineData(ComponentSize.Normal, "checkbox", "")]
    [InlineData(ComponentSize.Tiny, "checkbox", "fw-checkbox-xs")]
    [InlineData(ComponentSize.Small, "loading", "fw-loading-sm")]
    [InlineData(ComponentSize.Large, "table", "fw-table-lg")]
    [InlineData(ComponentSize.ExtraLarge, "status", "fw-status-xl")]
    [InlineData(ComponentSize.Responsive, "rating", "fw-rating-xs sm:fw-rating-sm md:fw-rating-md lg:fw-rating-lg xl:fw-rating-xl")]
    public void FormControlSizeClassNames_ReturnsExpectedClasses(ComponentSize size, string token, string expected)
    {
        var classes = size.FormControlClassNames(token);

        classes.Should().Be(expected);
    }

    [Fact]
    public void FormControlClassNames_ThrowsForWhitespaceToken()
    {
        var action = () => ThemeColor.Primary.FormControlClassNames(" ");

        action.Should().Throw<ArgumentException>()
            .WithMessage("*Component token cannot be null or whitespace.*");
    }

    [Fact]
    public void FormControlClassNames_ThrowsForUnsupportedColorToken()
    {
        var action = () => ThemeColor.Primary.FormControlClassNames("table");

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Unsupported form control component token.*");
    }

    [Fact]
    public void FieldStyleClassNames_ThrowsForUnsupportedStyleToken()
    {
        var action = () => FieldStyle.Ghost.ClassNames("toggle");

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Unsupported form control component token.*");
    }

    [Fact]
    public void FormControlSizeClassNames_ThrowsForUnsupportedSizeToken()
    {
        var action = () => ComponentSize.Small.FormControlClassNames("hero");

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*Unsupported form control component token.*");
    }
}
