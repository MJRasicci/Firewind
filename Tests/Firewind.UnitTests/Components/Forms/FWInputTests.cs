namespace Firewind.UnitTests.Components.Forms;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition behavior for <see cref="FWInput"/>.
/// </summary>
public sealed class FWInputTests
{
    [Fact]
    public void OnParametersSet_WithVariants_ComposesExpectedCssClasses()
    {
        var input = new TestInput();
        input.Configure(FieldStyle.Ghost, ThemeColor.Primary, ComponentSize.Small);
        input.ApplyParameters();

        input.ComponentAttributes["class"].Should().Be("fw-input fw-input-ghost fw-input-primary fw-input-sm");
    }

    private sealed class TestInput : FWInput
    {
        public void Configure(FieldStyle style, ThemeColor color, ComponentSize size)
        {
            this.Style = style;
            this.Color = color;
            this.Size = size;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
