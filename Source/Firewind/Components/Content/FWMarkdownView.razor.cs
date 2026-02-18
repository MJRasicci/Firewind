namespace Firewind.Components;

using Firewind.Base;
using Ganss.Xss;
using Markdig;
using Microsoft.AspNetCore.Components;

/// <summary>
/// Renders Markdown content as HTML suitable for typography-style presentation.
/// </summary>
public partial class FWMarkdownView : FirewindComponentBase
{
    private static readonly MarkdownPipeline BasicPipeline = new MarkdownPipelineBuilder()
        .DisableHtml()
        .Build();

    private static readonly MarkdownPipeline BasicPipelineAllowHtml = new MarkdownPipelineBuilder()
        .Build();

    private static readonly MarkdownPipeline AdvancedPipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .DisableHtml()
        .Build();

    private static readonly MarkdownPipeline AdvancedPipelineAllowHtml = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .Build();

    private static readonly HtmlSanitizer Sanitizer = new();

    /// <summary>
    /// Gets or sets the markdown source to render.
    /// </summary>
    [Parameter]
    public string Markdown { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether inline HTML from markdown should be preserved.
    /// </summary>
    [Parameter]
    public bool AllowHtml { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether rendered HTML should be sanitized.
    /// </summary>
    [Parameter]
    public bool Sanitize { get; set; } = true;

    /// <summary>
    /// Gets or sets the markdown feature profile used to render content.
    /// </summary>
    [Parameter]
    public MarkdownPipelinePreset PipelinePreset { get; set; } = MarkdownPipelinePreset.Advanced;

    /// <summary>
    /// Gets the rendered markdown markup.
    /// </summary>
    internal MarkupString Markup { get; private set; }

    /// <summary>
    /// Re-renders markdown when component parameters change.
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        var pipeline = this.ResolvePipeline();
        var html = Markdig.Markdown.ToHtml(this.Markdown ?? string.Empty, pipeline);

        if (this.Sanitize)
        {
            html = Sanitizer.Sanitize(html);
        }

        this.Markup = new MarkupString(html);
    }

    /// <summary>
    /// Gets the root CSS classes for markdown display.
    /// </summary>
    protected override string CssClass => "prose max-w-none";

    private MarkdownPipeline ResolvePipeline() => (this.PipelinePreset, this.AllowHtml) switch
    {
        (MarkdownPipelinePreset.Basic, true) => BasicPipelineAllowHtml,
        (MarkdownPipelinePreset.Basic, false) => BasicPipeline,
        (_, true) => AdvancedPipelineAllowHtml,
        _ => AdvancedPipeline,
    };
}

/// <summary>
/// Defines markdown feature profiles for <see cref="FWMarkdownView"/>.
/// </summary>
public enum MarkdownPipelinePreset
{
    /// <summary>
    /// Enables core markdown syntax only.
    /// </summary>
    Basic,

    /// <summary>
    /// Enables Markdig advanced extensions (tables, task lists, footnotes, and more).
    /// </summary>
    Advanced,
}
