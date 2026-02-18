namespace Firewind.UnitTests.Components.Layout;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition behavior for <see cref="FWDock"/>.
/// </summary>
public sealed class FWDockTests
{
    [Fact]
    public void OnParametersSet_WithLargeSize_ComposesExpectedCssClasses()
    {
        var dock = new TestDock();
        dock.Configure(DockSize.Large);

        dock.ApplyParameters();

        dock.ComponentAttributes["class"].Should().Be("fw-dock fw-dock-lg");
    }

    private sealed class TestDock : FWDock
    {
        public void Configure(DockSize size) => this.Size = size;

        public void ApplyParameters() => base.OnParametersSet();
    }
}
