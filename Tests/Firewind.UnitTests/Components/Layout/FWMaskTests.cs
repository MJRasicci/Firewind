namespace Firewind.UnitTests.Components.Layout;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition behavior for <see cref="FWMask"/>.
/// </summary>
public sealed class FWMaskTests
{
    [Fact]
    public void OnParametersSet_WithShapeAndHalf_ComposesExpectedCssClasses()
    {
        var mask = new TestMask();
        mask.Configure(MaskShape.Decagon, MaskHalf.First);

        mask.ApplyParameters();

        mask.ComponentAttributes["class"].Should().Be("fw-mask fw-mask-decagon fw-mask-half-1");
    }

    private sealed class TestMask : FWMask
    {
        public void Configure(MaskShape shape, MaskHalf half)
        {
            this.Shape = shape;
            this.Half = half;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
