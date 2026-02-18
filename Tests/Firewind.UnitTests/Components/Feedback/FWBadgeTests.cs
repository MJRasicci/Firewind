namespace Firewind.UnitTests.Components.Feedback;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition behavior for <see cref="FWBadge"/>.
/// </summary>
public sealed class FWBadgeTests
{
    [Fact]
    public void OnParametersSet_WithAllStyleFlags_ComposesExpectedCssClasses()
    {
        var badge = new TestBadge();
        badge.Configure(
            outline: true,
            dash: true,
            soft: true,
            color: ThemeColor.Success,
            size: ComponentSize.Large);
        badge.ApplyParameters();

        badge.ComponentAttributes["class"].Should().Be("fw-badge fw-badge-outline fw-badge-dash fw-badge-soft fw-badge-success fw-badge-lg");
    }

    private sealed class TestBadge : FWBadge
    {
        public void Configure(bool outline, bool dash, bool soft, ThemeColor color, ComponentSize size)
        {
            this.Outline = outline;
            this.Dash = dash;
            this.Soft = soft;
            this.Color = color;
            this.Size = size;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
