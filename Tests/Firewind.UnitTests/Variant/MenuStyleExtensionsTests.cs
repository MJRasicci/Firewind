namespace Firewind.UnitTests.Variant;

using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies menu-specific variant class mappings.
/// </summary>
public sealed class MenuStyleExtensionsTests
{
    /// <summary>
    /// Validates action modifier classes for representative behavior combinations.
    /// </summary>
    /// <param name="behavior">The behavior flags to resolve.</param>
    /// <param name="expectedClasses">The expected action class output.</param>
    [Theory]
    [InlineData(MenuItemBehavior.None, "")]
    [InlineData(MenuItemBehavior.Active, "fw-menu-active")]
    [InlineData(MenuItemBehavior.Focus, "fw-menu-focus")]
    [InlineData(MenuItemBehavior.Disabled | MenuItemBehavior.DropdownToggle, "fw-menu-disabled fw-menu-dropdown-toggle")]
    public void ActionClassNames_ReturnsExpectedMappings(MenuItemBehavior behavior, string expectedClasses)
    {
        var classes = behavior.ActionClassNames();

        classes.Should().Be(expectedClasses);
    }

    /// <summary>
    /// Validates dropdown visibility classes for menu behaviors.
    /// </summary>
    /// <param name="behavior">The behavior flags to resolve.</param>
    /// <param name="expectedClasses">The expected dropdown class output.</param>
    [Theory]
    [InlineData(MenuItemBehavior.None, "")]
    [InlineData(MenuItemBehavior.DropdownShow, "fw-menu-dropdown-show")]
    [InlineData(MenuItemBehavior.Active | MenuItemBehavior.DropdownShow, "fw-menu-dropdown-show")]
    public void DropdownClassNames_ReturnsExpectedMappings(MenuItemBehavior behavior, string expectedClasses)
    {
        var classes = behavior.DropdownClassNames();

        classes.Should().Be(expectedClasses);
    }

    /// <summary>
    /// Validates responsive label visibility class mappings.
    /// </summary>
    /// <param name="visibility">The visibility option to resolve.</param>
    /// <param name="expectedClasses">The expected label class output.</param>
    [Theory]
    [InlineData(MenuContentVisibility.Always, "")]
    [InlineData(MenuContentVisibility.Hidden, "hidden")]
    [InlineData(MenuContentVisibility.VisibleFromLarge, "hidden lg:inline")]
    [InlineData(MenuContentVisibility.HiddenFromMedium, "md:hidden")]
    public void LabelClassNames_ReturnsExpectedMappings(MenuContentVisibility visibility, string expectedClasses)
    {
        var classes = visibility.LabelClassNames();

        classes.Should().Be(expectedClasses);
    }

    /// <summary>
    /// Validates responsive icon visibility class mappings.
    /// </summary>
    /// <param name="visibility">The visibility option to resolve.</param>
    /// <param name="expectedClasses">The expected icon class output.</param>
    [Theory]
    [InlineData(MenuContentVisibility.Always, "")]
    [InlineData(MenuContentVisibility.Hidden, "hidden")]
    [InlineData(MenuContentVisibility.VisibleFromMedium, "hidden md:inline-flex")]
    [InlineData(MenuContentVisibility.HiddenFromExtraLarge, "xl:hidden")]
    public void IconClassNames_ReturnsExpectedMappings(MenuContentVisibility visibility, string expectedClasses)
    {
        var classes = visibility.IconClassNames();

        classes.Should().Be(expectedClasses);
    }
}
