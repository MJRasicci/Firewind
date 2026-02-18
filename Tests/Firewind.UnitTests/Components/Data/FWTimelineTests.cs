namespace Firewind.UnitTests.Components.Data;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition behavior for <see cref="FWTimeline"/>.
/// </summary>
public sealed class FWTimelineTests
{
    [Fact]
    public void OnParametersSet_WithBoxAndCompact_ComposesExpectedCssClasses()
    {
        var timeline = new TestTimeline();
        timeline.Configure(AxisDirection.Horizontal, snapIcon: true, box: true, compact: true);

        timeline.ApplyParameters();

        timeline.ComponentAttributes["class"].Should().Be("fw-timeline fw-timeline-horizontal fw-timeline-snap-icon fw-timeline-box fw-timeline-compact");
    }

    private sealed class TestTimeline : FWTimeline
    {
        public void Configure(AxisDirection direction, bool snapIcon, bool box, bool compact)
        {
            this.Direction = direction;
            this.SnapIcon = snapIcon;
            this.Box = box;
            this.Compact = compact;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
