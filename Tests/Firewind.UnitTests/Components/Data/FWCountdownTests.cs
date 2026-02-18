namespace Firewind.UnitTests.Components.Data;

using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies parameter validation for <see cref="FWCountdown"/>.
/// </summary>
public sealed class FWCountdownTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(1000)]
    public void OnParametersSet_WhenValueOutOfRange_ThrowsArgumentOutOfRangeException(int value)
    {
        var countdown = new TestCountdown();
        countdown.Configure(value);

        var action = countdown.ApplyParameters;

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*between 0 and 999*");
    }

    [Fact]
    public void OnParametersSet_WhenValueInRange_ComposesExpectedCssClass()
    {
        var countdown = new TestCountdown();
        countdown.Configure(999);

        countdown.ApplyParameters();

        countdown.ComponentAttributes["class"].Should().Be("fw-countdown");
    }

    private sealed class TestCountdown : FWCountdown
    {
        public void Configure(int value) => this.Value = value;

        public void ApplyParameters() => base.OnParametersSet();
    }
}
