namespace Firewind.Components;

using Firewind.Base;
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

[SupportedOSPlatform("browser")]
public partial class FWModal : FirewindComponentBase
{
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSHost.ImportAsync(nameof(FWModal), "/_content/Firewind/Components/Actions/FWModal.razor.js");
        }
        else
        {
            if (this.Open)
                Show(this.Id);
            else
                Hide(this.Id);
        }
    }

    [JSImport("showDialog", nameof(FWModal))]
    internal static partial void Show(string element);

    [JSImport("closeDialog", nameof(FWModal))]
    internal static partial void Hide(string element);
}
