namespace Firewind.UnitTests.Components.Data;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition defaults for <see cref="FWStats"/>.
/// </summary>
public sealed class FWStatsTests
{
    [Fact]
    public void OnParametersSet_WithDefaultDirection_ComposesHorizontalClass()
    {
        var stats = new TestStats();

        stats.ApplyParameters();

        stats.ComponentAttributes["class"].Should().Be("fw-stats fw-stats-horizontal");
    }

    [Fact]
    public void OnParametersSet_WithVerticalDirection_ComposesVerticalClass()
    {
        var stats = new TestStats();
        stats.Configure(AxisDirection.Vertical);

        stats.ApplyParameters();

        stats.ComponentAttributes["class"].Should().Be("fw-stats fw-stats-vertical");
    }

    private sealed class TestStats : FWStats
    {
        public void Configure(AxisDirection direction) => this.Direction = direction;

        public void ApplyParameters() => base.OnParametersSet();
    }
}
