namespace Firewind.UnitTests.Components.Forms;

using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies dotted-path value operations for <see cref="FWObjectPathAccessor"/>.
/// </summary>
public sealed class FWObjectPathAccessorTests
{
    [Fact]
    public void SetValue_WithNestedPath_UpdatesNestedObject()
    {
        var model = new TestModel();

        FWObjectPathAccessor.SetValue(model, "Address.City", "Austin");

        model.Address.City.Should().Be("Austin");
    }

    [Fact]
    public void SetValue_WithEnumText_ParsesEnum()
    {
        var model = new TestModel();

        FWObjectPathAccessor.SetValue(model, "Status", "Active");

        model.Status.Should().Be(TestStatus.Active);
    }

    [Fact]
    public void GetValue_WithNestedPath_ReturnsExpectedValue()
    {
        var model = new TestModel { Address = new TestAddress { City = "Boston" } };

        var value = FWObjectPathAccessor.GetValue(model, "Address.City");

        value.Should().Be("Boston");
    }

    private sealed class TestModel
    {
        public TestAddress Address { get; set; } = new();

        public TestStatus Status { get; set; }
    }

    private sealed class TestAddress
    {
        public string City { get; set; } = string.Empty;
    }

    private enum TestStatus
    {
        Draft,
        Active
    }
}
