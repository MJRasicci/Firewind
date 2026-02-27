namespace Firewind.UnitTests.Components.Forms;

using Firewind.Components;
using FluentAssertions;

/// <summary>
/// Verifies schema generation behavior for <see cref="FWFormSchemaBuilder"/>.
/// </summary>
public sealed class FWFormSchemaBuilderTests
{
    [Fact]
    public void Build_WithScalarAndNestedProperties_ReturnsExpectedFields()
    {
        var fields = FWFormSchemaBuilder.Build(typeof(TestModel), maxDepth: 2);

        fields.Select(static field => field.Path).Should().BeEquivalentTo(
            "Name",
            "Age",
            "IsEnabled",
            "StartDate",
            "Status",
            "Address.City");

        fields.Single(static field => field.Path == "Status").Kind.Should().Be(FWFormInputKind.Select);
    }

    private sealed class TestModel
    {
        public string Name { get; set; } = string.Empty;

        public int Age { get; set; }

        public bool IsEnabled { get; set; }

        public DateOnly StartDate { get; set; }

        public TestStatus Status { get; set; }

        public TestAddress Address { get; set; } = new();
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
