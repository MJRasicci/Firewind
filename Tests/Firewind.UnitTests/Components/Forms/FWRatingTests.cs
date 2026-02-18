namespace Firewind.UnitTests.Components.Forms;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition behavior for <see cref="FWRating"/>.
/// </summary>
public sealed class FWRatingTests
{
    [Fact]
    public void OnParametersSet_WithHalfLarge_ComposesExpectedCssClasses()
    {
        var rating = new TestRating();
        rating.Configure(ComponentSize.Large, half: true, hidden: false);

        rating.ApplyParameters();

        rating.ComponentAttributes["class"].Should().Be("fw-rating fw-rating-lg fw-rating-half");
    }

    [Fact]
    public void OnParametersSet_WithHiddenModifier_ComposesExpectedCssClasses()
    {
        var rating = new TestRating();
        rating.Configure(ComponentSize.Small, half: false, hidden: true);

        rating.ApplyParameters();

        rating.ComponentAttributes["class"].Should().Be("fw-rating fw-rating-sm fw-rating-hidden");
    }

    private sealed class TestRating : FWRating
    {
        public void Configure(ComponentSize size, bool half, bool hidden)
        {
            this.Size = size;
            this.Half = half;
            this.Hidden = hidden;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
