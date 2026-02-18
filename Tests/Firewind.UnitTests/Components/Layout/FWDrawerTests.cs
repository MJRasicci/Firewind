namespace Firewind.UnitTests.Components.Layout;

using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies class composition behavior for <see cref="FWDrawer"/>.
/// </summary>
public sealed class FWDrawerTests
{
    [Fact]
    public void OnParametersSet_WhenOpenEnd_ComposesExpectedCssClasses()
    {
        var drawer = new TestDrawer();
        drawer.Configure(open: true, end: true);

        drawer.ApplyParameters();

        drawer.ComponentAttributes["class"].Should().Be("fw-drawer fw-drawer-open fw-drawer-end");
    }

    private sealed class TestDrawer : FWDrawer
    {
        public void Configure(bool open, bool end)
        {
            this.Open = open;
            this.End = end;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
