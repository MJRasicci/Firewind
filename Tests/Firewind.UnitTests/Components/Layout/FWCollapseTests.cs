namespace Firewind.UnitTests.Components.Layout;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition behavior for <see cref="FWCollapse"/>.
/// </summary>
public sealed class FWCollapseTests
{
    [Fact]
    public void OnParametersSet_WithOpenArrow_ComposesExpectedCssClasses()
    {
        var collapse = new TestCollapse();
        collapse.Configure(CollapseStyle.Arrow, open: true, closed: false);
        collapse.ApplyParameters();

        collapse.ComponentAttributes["class"].Should().Be("fw-collapse fw-collapse-arrow fw-collapse-open");
    }

    [Fact]
    public void OnParametersSet_WithClosedPlus_ComposesExpectedCssClasses()
    {
        var collapse = new TestCollapse();
        collapse.Configure(CollapseStyle.Plus, open: false, closed: true);
        collapse.ApplyParameters();

        collapse.ComponentAttributes["class"].Should().Be("fw-collapse fw-collapse-plus fw-collapse-close");
    }

    private sealed class TestCollapse : FWCollapse
    {
        public void Configure(CollapseStyle style, bool open, bool closed)
        {
            this.Style = style;
            this.Open = open;
            this.Closed = closed;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
