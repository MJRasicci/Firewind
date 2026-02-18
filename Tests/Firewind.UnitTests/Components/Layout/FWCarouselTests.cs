namespace Firewind.UnitTests.Components.Layout;

using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies class composition and validation for <see cref="FWCarousel"/>.
/// </summary>
public sealed class FWCarouselTests
{
    [Fact]
    public void OnParametersSet_WithVerticalCenter_ComposesExpectedCssClasses()
    {
        var carousel = new TestCarousel();
        carousel.Configure(vertical: true, center: true, end: false);

        carousel.ApplyParameters();

        carousel.ComponentAttributes["class"].Should().Be("fw-carousel fw-carousel-vertical fw-carousel-center");
    }

    [Fact]
    public void OnParametersSet_WithEndAlignment_ComposesExpectedCssClasses()
    {
        var carousel = new TestCarousel();
        carousel.Configure(vertical: false, center: false, end: true);

        carousel.ApplyParameters();

        carousel.ComponentAttributes["class"].Should().Be("fw-carousel fw-carousel-end");
    }

    [Fact]
    public void OnParametersSet_WhenCenterAndEnd_ThrowsInvalidOperationException()
    {
        var carousel = new TestCarousel();
        carousel.Configure(vertical: false, center: true, end: true);

        var action = carousel.ApplyParameters;

        action.Should().Throw<InvalidOperationException>()
            .WithMessage("*cannot be both center and end*");
    }

    private sealed class TestCarousel : FWCarousel
    {
        public void Configure(bool vertical, bool center, bool end)
        {
            this.Vertical = vertical;
            this.Center = center;
            this.End = end;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
