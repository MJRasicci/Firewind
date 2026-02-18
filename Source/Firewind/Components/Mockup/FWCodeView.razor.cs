namespace Firewind.Components;

using ColorCode;
using Firewind.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Net;
using System.Text;

/// <summary>
/// Renders syntax-highlighted source code within a mockup container.
/// </summary>
public partial class FWCodeView : FirewindComponentBase, IAsyncDisposable
{
    private const string ClipboardModulePath = "./_content/Firewind/Components/Mockup/FWCodeView.razor.js";
    private static readonly string[] LineSeparators = ["\r\n", "\n", "\r"];

    private readonly SourceCodeFormatter formatter = new();
    private IJSObjectReference? module;

    /// <summary>
    /// Gets the JavaScript runtime service used to load and call clipboard helpers.
    /// </summary>
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = null!;

    /// <summary>
    /// Gets or sets the source code text to render.
    /// </summary>
    [Parameter]
    public string SourceCode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ColorCode language identifier used for syntax highlighting.
    /// </summary>
    [Parameter]
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether a copy-to-clipboard button is rendered.
    /// </summary>
    [Parameter]
    public bool ShowCopy { get; set; } = true;

    /// <summary>
    /// Gets formatted markup for the configured source code and language.
    /// </summary>
    private MarkupString FormattedSource => this.ResolveLanguage() is ILanguage language
        ? this.formatter.GetMarkupString(this.SourceCode, language)
        : this.BuildPlainTextMarkup();

    /// <summary>
    /// Imports the clipboard helper module when copy support is enabled on first render.
    /// </summary>
    /// <param name="firstRender">Indicates whether this is the first render cycle.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender || !this.ShowCopy)
        {
            return;
        }

        this.module = await this.JSRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            ClipboardModulePath);
    }

    /// <summary>
    /// Copies the original source code text to the clipboard.
    /// </summary>
    /// <param name="_">The originating click event arguments.</param>
    /// <returns>A task that represents the asynchronous clipboard operation.</returns>
    private async Task CopySourceCodeAsync(MouseEventArgs _)
    {
        if (!this.ShowCopy)
        {
            return;
        }

        this.module ??= await this.JSRuntime.InvokeAsync<IJSObjectReference>(
            "import",
            ClipboardModulePath);

        await this.module.InvokeVoidAsync("copyText", this.SourceCode);
    }

    /// <summary>
    /// Resolves the configured language identifier to a ColorCode language descriptor.
    /// </summary>
    /// <returns>
    /// A resolved <see cref="ILanguage"/> instance, or <see langword="null"/> if the id cannot be resolved.
    /// </returns>
    private ILanguage? ResolveLanguage()
    {
        if (string.IsNullOrWhiteSpace(this.Language))
        {
            return Languages.CSharp;
        }

        return Languages.FindById(this.Language);
    }

    /// <summary>
    /// Builds HTML markup that renders escaped plain text with line numbers.
    /// </summary>
    /// <returns>A <see cref="MarkupString"/> containing escaped code lines.</returns>
    private MarkupString BuildPlainTextMarkup()
    {
        var lines = this.SourceCode.Split(LineSeparators, StringSplitOptions.None);
        var buffer = new StringBuilder();

        for (var i = 0; i < lines.Length; i++)
        {
            buffer.Append("<pre data-prefix=\"")
                  .Append(i + 1)
                  .Append("\"><code>")
                  .Append(WebUtility.HtmlEncode(lines[i]))
                  .Append("</code></pre>");
        }

        return new MarkupString(buffer.ToString());
    }

    /// <summary>
    /// Gets the root CSS classes for the code view component.
    /// </summary>
    protected override string CssClass => "fw-mockup-code relative border border-base-300 pb-0 flex flex-col overflow-hidden";

    /// <summary>
    /// Disposes the JavaScript module used by this component.
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
