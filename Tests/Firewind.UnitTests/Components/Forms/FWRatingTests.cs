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
        rating.Configure(ComponentSize.Large, half: true);

        rating.ApplyParameters();

        rating.ComponentAttributes["class"].Should().Be("fw-rating fw-rating-lg fw-rating-half");
    }

    private sealed class TestRating : FWRating
    {
        public void Configure(ComponentSize size, bool half)
        {
            this.Size = size;
            this.Half = half;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
