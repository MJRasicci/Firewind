namespace Firewind.UnitTests.Components.Data;

using Firewind.Components;
using Firewind.Variant;
using FluentAssertions;

/// <summary>
/// Verifies class composition behavior for <see cref="FWTable"/>.
/// </summary>
public sealed class FWTableTests
{
    [Fact]
    public void OnParametersSet_WithFlagsAndSize_ComposesExpectedCssClasses()
    {
        var table = new TestTable();
        table.Configure(ComponentSize.Small, zebra: true, pinRows: true, pinCols: true);
        table.ApplyParameters();

        table.ComponentAttributes["class"].Should().Be("fw-table fw-table-sm fw-table-zebra fw-table-pin-rows fw-table-pin-cols");
    }

    private sealed class TestTable : FWTable
    {
        public void Configure(ComponentSize size, bool zebra, bool pinRows, bool pinCols)
        {
            this.Size = size;
            this.Zebra = zebra;
            this.PinRows = pinRows;
            this.PinCols = pinCols;
        }

        public void ApplyParameters() => base.OnParametersSet();
    }
}
