namespace Firewind.UnitTests.Components.Data;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition defaults for <see cref="FWSteps"/>.
/// </summary>
public sealed class FWStepsTests
{
    [Fact]
    public void OnParametersSet_WithDefaultDirection_ComposesHorizontalClass()
    {
        var steps = new TestSteps();

        steps.ApplyParameters();

        steps.ComponentAttributes["class"].Should().Be("fw-steps fw-steps-horizontal");
    }

    [Fact]
    public void OnParametersSet_WithVerticalDirection_ComposesVerticalClass()
    {
        var steps = new TestSteps();
        steps.Configure(AxisDirection.Vertical);

        steps.ApplyParameters();

        steps.ComponentAttributes["class"].Should().Be("fw-steps fw-steps-vertical");
    }

    private sealed class TestSteps : FWSteps
    {
        public void Configure(AxisDirection direction) => this.Direction = direction;

        public void ApplyParameters() => base.OnParametersSet();
    }
}
