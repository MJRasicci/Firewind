namespace Firewind.UnitTests.Components.Layout;

using System.Reflection;
using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies indicator placement class resolution for <see cref="FWIndicator"/>.
/// </summary>
public sealed class FWIndicatorTests
{
    [Fact]
    public void IndicatorClasses_WithDefaults_UsesEndAndTop()
    {
        var indicator = new TestIndicator();

        indicator.GetIndicatorClasses().Should().Be("fw-indicator-end fw-indicator-top");
    }

    [Fact]
    public void IndicatorClasses_WithCenterAndMiddle_UsesCenterAndMiddle()
    {
        var indicator = new TestIndicator();
        indicator.Configure(ElementPosition.None, ElementPosition.None);

        indicator.GetIndicatorClasses().Should().Be("fw-indicator-center fw-indicator-middle");
    }

    [Fact]
    public void IndicatorClasses_WithLeftAndBottom_UsesStartAndBottom()
    {
        var indicator = new TestIndicator();
        indicator.Configure(ElementPosition.Left, ElementPosition.Bottom);

        indicator.GetIndicatorClasses().Should().Be("fw-indicator-start fw-indicator-bottom");
    }

    private sealed class TestIndicator : FWIndicator
    {
        private static readonly PropertyInfo IndicatorClassesProperty = typeof(FWIndicator)
            .GetProperty("IndicatorClasses", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Unable to locate FWIndicator.IndicatorClasses.");

        public void Configure(ElementPosition horizontalPosition, ElementPosition verticalPosition)
        {
            this.HorizontalPosition = horizontalPosition;
            this.VerticalPosition = verticalPosition;
        }

        public string GetIndicatorClasses() => (string)(IndicatorClassesProperty.GetValue(this)
            ?? throw new InvalidOperationException("Indicator classes must not be null."));
    }
}
