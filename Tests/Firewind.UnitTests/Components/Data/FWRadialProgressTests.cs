namespace Firewind.UnitTests.Components.Data;

using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies parameter behavior for <see cref="FWRadialProgress"/>.
/// </summary>
public sealed class FWRadialProgressTests
{
    [Fact]
    public void OnParametersSet_WhenDisplayTextNotProvided_DefaultsToPercentageText()
    {
        var radial = new TestRadialProgress();
        radial.Configure(value: 64, displayText: null);

        radial.ApplyParameters();

        radial.DisplayText.Should().Be("64%");
    }

    private sealed class TestRadialProgress : FWRadialProgress
    {
        public new string? DisplayText => base.DisplayText;

        public void Configure(int value, string? displayText)
        {
            this.Value = value;
            base.DisplayText = displayText;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
