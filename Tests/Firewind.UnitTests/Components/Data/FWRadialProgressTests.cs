namespace Firewind.UnitTests.Components.Data;

using System.Reflection;
using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies parameter behavior for <see cref="FWRadialProgress"/>.
/// </summary>
public sealed class FWRadialProgressTests
{
    [Fact]
    public void OnParametersSet_WhenDisplayTextNotProvided_DoesNotMutateParameter()
    {
        var radial = new TestRadialProgress();
        radial.Configure(value: 64, displayText: null);

        radial.ApplyParameters();

        radial.DisplayText.Should().BeNull();
    }

    [Fact]
    public void OnParametersSet_WhenDisplayTextNotProvided_ResolvedDisplayTextUsesCurrentValue()
    {
        var radial = new TestRadialProgress();
        radial.Configure(value: 64, displayText: null);
        radial.ApplyParameters();

        radial.GetResolvedDisplayText().Should().Be("64%");

        radial.Configure(value: 32, displayText: null);
        radial.ApplyParameters();

        radial.GetResolvedDisplayText().Should().Be("32%");
    }

    [Fact]
    public void OnParametersSet_WhenValueOutOfRange_ThrowsArgumentOutOfRangeException()
    {
        var radial = new TestRadialProgress();
        radial.Configure(value: 101, displayText: null);

        var action = radial.ApplyParameters;

        action.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*between 0 and 100*");
    }

    [Fact]
    public void OnParametersSet_WhenDisplayTextProvided_UsesProvidedText()
    {
        var radial = new TestRadialProgress();
        radial.Configure(value: 42, displayText: "Loaded");

        radial.ApplyParameters();

        radial.GetResolvedDisplayText().Should().Be("Loaded");
    }

    private sealed class TestRadialProgress : FWRadialProgress
    {
        private static readonly PropertyInfo ResolvedDisplayTextProperty = typeof(FWRadialProgress)
            .GetProperty("ResolvedDisplayText", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Unable to locate FWRadialProgress.ResolvedDisplayText.");

        public new string? DisplayText => base.DisplayText;

        public void Configure(int value, string? displayText)
        {
            this.Value = value;
            base.DisplayText = displayText;
        }

        public void ApplyParameters() => base.OnParametersSet();

        public string GetResolvedDisplayText() => (string)(ResolvedDisplayTextProperty.GetValue(this)
            ?? throw new InvalidOperationException("Resolved display text must not be null."));
    }
}
