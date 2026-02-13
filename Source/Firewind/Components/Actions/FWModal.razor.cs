namespace Firewind.Components;

using Firewind.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

public partial class FWModal : FirewindComponentBase, IAsyncDisposable
{
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    private IJSObjectReference? module;
    private bool? renderedOpenState;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            this.module = await this.JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./_content/Firewind/Components/Actions/FWModal.razor.js");
        }

        if (this.module is null || this.renderedOpenState == this.Open)
        {
            return;
        }

        this.renderedOpenState = this.Open;

        var methodName = this.Open ? "showDialog" : "closeDialog";
        await this.module.InvokeVoidAsync(methodName, this.Id);
    }

    public async ValueTask DisposeAsync()
    {
        if (this.module is null)
        {
            return;
        }

        try
        {
            await this.module.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
            // Ignore disconnects during teardown.
        }
    }
}
