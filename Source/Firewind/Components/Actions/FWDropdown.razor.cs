namespace Firewind.Components;

using Firewind.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

/// <summary>
/// Provides JavaScript interop behavior for the <c>FWDropdown</c> component.
/// </summary>
public partial class FWDropdown : FirewindComponentBase, IAsyncDisposable
{
    /// <summary>
    /// Gets the JavaScript runtime service used to load and call dropdown helpers.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    private IJSObjectReference? module;
    private string? renderedElementId;
    private bool? renderedCloseOnOutsideClick;

    /// <summary>
    /// Synchronizes outside-click close behavior with the JavaScript module after each render.
    /// </summary>
    /// <param name="firstRender">Indicates whether this is the first render cycle.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            this.module = await this.JSRuntime.InvokeAsync<IJSObjectReference>(
                "import",
                "./_content/Firewind/Components/Actions/FWDropdown.razor.js");
        }

        if (this.module is null)
        {
            return;
        }

        var elementIdChanged = !string.Equals(this.renderedElementId, this.Id, StringComparison.Ordinal);
        var outsideClickOptionChanged = this.renderedCloseOnOutsideClick != this.CloseOnOutsideClick;

        if (!elementIdChanged && !outsideClickOptionChanged)
        {
            return;
        }

        if (elementIdChanged && !string.IsNullOrWhiteSpace(this.renderedElementId))
        {
            await this.module.InvokeVoidAsync("removeOutsideClickClose", this.renderedElementId);
        }

        this.renderedElementId = this.Id;
        this.renderedCloseOnOutsideClick = this.CloseOnOutsideClick;

        await this.module.InvokeVoidAsync("configureOutsideClickClose", this.Id, this.CloseOnOutsideClick);
    }

    /// <summary>
    /// Disposes the JavaScript module used by the dropdown component.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (this.module is not null)
            {
                if (!string.IsNullOrWhiteSpace(this.renderedElementId))
                {
                    await this.module.InvokeVoidAsync("removeOutsideClickClose", this.renderedElementId);
                }

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
