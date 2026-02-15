namespace Firewind.UnitTests.Components.Actions;

using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies parameter-driven attribute behavior for <see cref="FWButton"/>.
/// </summary>
public sealed class FWButtonTests
{
    /// <summary>
    /// Ensures non-button render targets become keyboard-inert when disabled.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenNonButtonTargetIsDisabled_AppliesAriaDisabledAndTabIndexMinusOne()
    {
        var button = new TestButton();
        button.Configure("a", true, new Dictionary<string, object>());

        button.ApplyParameters();

        button.ComponentAttributes.Should().ContainKey("aria-disabled").WhoseValue.Should().Be("true");
        button.ComponentAttributes.Should().ContainKey("tabindex").WhoseValue.Should().Be("-1");
    }

    /// <summary>
    /// Ensures framework-added keyboard suppression is removed when re-enabled.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenNonButtonTargetIsReEnabled_RemovesFrameworkTabIndex()
    {
        var button = new TestButton();
        button.Configure("a", true, new Dictionary<string, object>());
        button.ApplyParameters();

        button.SetDisabled(false);
        button.ApplyParameters();

        button.ComponentAttributes.Should().NotContainKey("aria-disabled");
        button.ComponentAttributes.Should().NotContainKey("tabindex");
    }

    /// <summary>
    /// Ensures explicitly supplied tab index values are preserved across disabled-state transitions.
    /// </summary>
    [Fact]
    public void OnParametersSet_WhenCustomTabIndexProvided_PreservesCustomTabIndex()
    {
        var button = new TestButton();
        button.Configure(
            "a",
            true,
            new Dictionary<string, object>
            {
                ["tabindex"] = "2"
            });
        button.ApplyParameters();
        button.ComponentAttributes.Should().ContainKey("tabindex").WhoseValue.Should().Be("2");

        button.SetDisabled(false);
        button.ApplyParameters();
        button.ComponentAttributes.Should().ContainKey("tabindex").WhoseValue.Should().Be("2");
    }

    private sealed class TestButton : FWButton
    {
        public void Configure(string asElement, bool disabled, IReadOnlyDictionary<string, object> additionalAttributes)
        {
            this.As = asElement;
            this.Disabled = disabled;
            this.AdditionalAttributes = additionalAttributes;
        }

        public void SetDisabled(bool disabled) => this.Disabled = disabled;

        public void ApplyParameters() => base.OnParametersSet();
    }
}
