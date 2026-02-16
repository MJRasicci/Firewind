namespace Firewind.UnitTests.Components.Navigation;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;

/// <summary>
/// Verifies parameter-driven behavior for <see cref="FWMenuItem"/>.
/// </summary>
public sealed class FWMenuItemTests
{
    /// <summary>
    /// Ensures disabled anchor triggers are made inert and no href is rendered.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenAnchorIsDisabled_RemovesHrefAndAppliesAriaDisabled()
    {
        var item = new TestMenuItem();
        item.Configure(
            asElement: "a",
            href: "/dashboard",
            behavior: MenuItemBehavior.Disabled,
            collapsible: false,
            open: false,
            actionAttributes: new Dictionary<string, object>());

        item.ApplyParameters();

        item.ComputedActionAttributes.Should().ContainKey("aria-disabled").WhoseValue.Should().Be("true");
        item.ComputedActionAttributes.Should().ContainKey("tabindex").WhoseValue.Should().Be("-1");
        item.ComputedActionAttributes.Should().NotContainKey("href");
    }

    /// <summary>
    /// Ensures enabled anchor triggers keep href and remain accessible.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenAnchorIsEnabled_AppliesHrefAndRemovesAriaDisabled()
    {
        var item = new TestMenuItem();
        item.Configure(
            asElement: "a",
            href: "/dashboard",
            behavior: MenuItemBehavior.None,
            collapsible: false,
            open: false,
            actionAttributes: new Dictionary<string, object>());

        item.ApplyParameters();

        item.ComputedActionAttributes.Should().ContainKey("href").WhoseValue.Should().Be("/dashboard");
        item.ComputedActionAttributes.Should().NotContainKey("aria-disabled");
        item.ComputedActionAttributes.Should().NotContainKey("tabindex");
    }

    /// <summary>
    /// Ensures button triggers emit default type and disabled attributes.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenButtonIsDisabled_AppliesTypeAndDisabledAttribute()
    {
        var item = new TestMenuItem();
        item.Configure(
            asElement: "button",
            href: null,
            behavior: MenuItemBehavior.Disabled,
            collapsible: false,
            open: false,
            actionAttributes: new Dictionary<string, object>());

        item.ApplyParameters();

        item.ComputedActionAttributes.Should().ContainKey("type").WhoseValue.Should().Be("button");
        item.ComputedActionAttributes.Should().ContainKey("disabled").WhoseValue.Should().Be(true);
        item.ComputedActionAttributes.Should().NotContainKey("aria-disabled");
    }

    /// <summary>
    /// Ensures collapsible mode emits open details attributes when requested.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenCollapsibleOpen_EmitsOpenDetailsAttribute()
    {
        var item = new TestMenuItem();
        item.Configure(
            asElement: "a",
            href: null,
            behavior: MenuItemBehavior.None,
            collapsible: true,
            open: true,
            actionAttributes: new Dictionary<string, object>());

        item.ApplyParameters();

        item.ComputedDetailsAttributes.Should().ContainKey("open").WhoseValue.Should().Be("open");
    }

    /// <summary>
    /// Ensures submenu classes include dropdown and show markers for collapsible shown groups.
    /// </summary>
    [Fact]
    public void SubMenuCssClass_WhenCollapsibleAndDropdownShow_ContainsExpectedClasses()
    {
        var item = new TestMenuItem();
        item.Configure(
            asElement: "a",
            href: null,
            behavior: MenuItemBehavior.DropdownShow,
            collapsible: true,
            open: false,
            actionAttributes: new Dictionary<string, object>());

        item.ApplyParameters();

        item.SubMenuClass.Should().Contain("fw-menu-dropdown");
        item.SubMenuClass.Should().Contain("fw-menu-dropdown-show");
    }

    /// <summary>
    /// Ensures collapsible items prevent native details toggle behavior.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenCollapsible_PreventsDefaultClickBehavior()
    {
        var item = new TestMenuItem();
        item.Configure(
            asElement: "button",
            href: null,
            behavior: MenuItemBehavior.None,
            collapsible: true,
            open: true,
            actionAttributes: new Dictionary<string, object>());

        item.ApplyParameters();

        item.PreventDefault.Should().BeTrue();
    }

    /// <summary>
    /// Ensures collapsible items preserve internal toggle state when the open parameter does not change.
    /// </summary>
    [Fact]
    public async Task HandleClickAsync_WhenOpenParameterIsUnchanged_PreservesInternalToggleState()
    {
        var item = new TestMenuItem();
        item.Configure(
            asElement: "button",
            href: null,
            behavior: MenuItemBehavior.None,
            collapsible: true,
            open: true,
            actionAttributes: new Dictionary<string, object>());

        item.ApplyParameters();
        item.ComputedDetailsAttributes.Should().ContainKey("open").WhoseValue.Should().Be("open");

        await item.InvokeClickAsync();
        item.ComputedDetailsAttributes.Should().NotContainKey("open");

        item.ApplyParameters();
        item.ComputedDetailsAttributes.Should().NotContainKey("open");
    }

    /// <summary>
    /// Ensures explicit open parameter changes reapply controlled state.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenOpenParameterChanges_ReappliesControlledState()
    {
        var item = new TestMenuItem();
        item.Configure(
            asElement: "button",
            href: null,
            behavior: MenuItemBehavior.None,
            collapsible: true,
            open: true,
            actionAttributes: new Dictionary<string, object>());

        item.ApplyParameters();
        item.ComputedDetailsAttributes.Should().ContainKey("open").WhoseValue.Should().Be("open");

        item.SetOpenParameter(false);
        item.ApplyParameters();
        item.ComputedDetailsAttributes.Should().NotContainKey("open");

        item.SetOpenParameter(true);
        item.ApplyParameters();
        item.ComputedDetailsAttributes.Should().ContainKey("open").WhoseValue.Should().Be("open");
    }

    private sealed class TestMenuItem : FWMenuItem
    {
        public IReadOnlyDictionary<string, object> ComputedActionAttributes => this.ActionElementAttributes;

        public IReadOnlyDictionary<string, object> ComputedDetailsAttributes => this.DetailsElementAttributes;

        public string SubMenuClass => this.SubMenuCssClass;

        public bool PreventDefault => this.PreventDefaultOnClick;

        public void Configure(
            string asElement,
            string? href,
            MenuItemBehavior behavior,
            bool collapsible,
            bool open,
            IReadOnlyDictionary<string, object> actionAttributes,
            bool hasClickHandler = false)
        {
            this.As = asElement;
            this.Href = href;
            this.Behavior = behavior;
            this.Collapsible = collapsible;
            this.Open = open;
            this.ActionAttributes = actionAttributes;
            this.OnClick = hasClickHandler
                ? EventCallback.Factory.Create<MouseEventArgs>(this, static _ => Task.CompletedTask)
                : default;
        }

        public void ApplyParameters() => base.OnParametersSet();

        public void SetOpenParameter(bool open) => this.Open = open;

        public async Task InvokeClickAsync()
        {
            var handleClickMethod = typeof(FWMenuItem).GetMethod(
                name: "HandleClickAsync",
                bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic);

            handleClickMethod.Should().NotBeNull();
            var task = handleClickMethod!.Invoke(this, [new MouseEventArgs()]) as Task;
            task.Should().NotBeNull();
            await task!;
        }
    }
}
