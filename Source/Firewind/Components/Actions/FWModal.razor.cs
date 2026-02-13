namespace Firewind.Components;

using Firewind.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

/// <summary>
/// Provides JavaScript interop behavior for the <c>FWModal</c> component.
/// </summary>
public partial class FWModal : FirewindComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Gets the JavaScript runtime service used to load and call modal helpers.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    private IJSObjectReference? module;
    private bool? renderedOpenState;

    /// <summary>
    /// Synchronizes the dialog open state with the JavaScript module after each render.
    /// </summary>
    /// <param name="firstRender">Indicates whether this is the first render cycle.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
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

    /// <summary>
    /// Disposes the JavaScript module used by the modal component.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (this.module is not null)
            {
                await this.module.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Ignore disconnects during teardown.
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }
}
