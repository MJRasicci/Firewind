namespace Firewind.UnitTests.Components.Actions;

using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies outside-click close configuration behavior for <see cref="FWModal"/>.
/// </summary>
public sealed class FWModalTests
{
    /// <summary>
    /// Ensures outside-click close is enabled by default.
    /// </summary>
    [Fact]
    public async Task CloseOnOutsideClick_DefaultsToTrue()
    {
        await using var modal = new TestModal();

        modal.CloseOnOutsideClick.Should().BeTrue();
        modal.ShouldRenderBackdrop().Should().BeTrue();
    }

    /// <summary>
    /// Ensures backdrop rendering remains enabled when outside-click close is enabled.
    /// </summary>
    [Fact]
    public async Task ShouldRenderBackdrop_WhenCloseOnOutsideClickEnabled_ReturnsTrue()
    {
        await using var modal = new TestModal();
        modal.ConfigureCloseOnOutsideClick(true);

        modal.ShouldRenderBackdrop().Should().BeTrue();
    }

    /// <summary>
    /// Ensures backdrop rendering can be disabled when outside-click close is disabled.
    /// </summary>
    [Fact]
    public async Task ShouldRenderBackdrop_WhenCloseOnOutsideClickDisabled_ReturnsFalse()
    {
        await using var modal = new TestModal();
        modal.ConfigureCloseOnOutsideClick(false);

        modal.ShouldRenderBackdrop().Should().BeFalse();
    }

    private sealed class TestModal : FWModal
    {
        public void ConfigureCloseOnOutsideClick(bool closeOnOutsideClick)
        {
            this.CloseOnOutsideClick = closeOnOutsideClick;
        }

        public bool ShouldRenderBackdrop() => this.ShouldRenderBackdropClose;
    }
}
