namespace Firewind.UnitTests.Components.Content;

using Firewind.Components;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using System.Reflection;

/// <summary>
/// Verifies markdown rendering behavior for <see cref="FWMarkdownView"/>.
/// </summary>
public sealed class FWMarkdownViewTests
{
    private static readonly PropertyInfo MarkupProperty = typeof(FWMarkdownView)
        .GetProperty("Markup", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("Expected non-public Markup property on FWMarkdownView.");
    /// <summary>
    /// Ensures markdown syntax is rendered as HTML.
    /// </summary>
    [Fact]
    public void Markup_WhenMarkdownProvided_RendersExpectedHtml()
    {
        var view = new TestMarkdownView();
        view.Configure("**hello**");

        var html = view.RenderMarkup();

        html.Should().Contain("<strong>hello</strong>");
    }

    /// <summary>
    /// Ensures inline HTML is encoded when <see cref="FWMarkdownView.AllowHtml"/> is disabled.
    /// </summary>
    [Fact]
    public void Markup_WhenAllowHtmlIsFalse_EncodesInlineHtml()
    {
        var view = new TestMarkdownView();
        view.Configure("<span>safe</span>", allowHtml: false);

        var html = view.RenderMarkup();

        html.Should().Contain("&lt;span&gt;");
    }

    /// <summary>
    /// Ensures unsafe script tags are removed when sanitization is enabled.
    /// </summary>
    [Fact]
    public void Markup_WhenSanitizeEnabled_RemovesScriptTags()
    {
        var view = new TestMarkdownView();
        view.Configure("<script>alert('x')</script><p>ok</p>", allowHtml: true, sanitize: true);

        var html = view.RenderMarkup();

        html.IndexOf("<script", StringComparison.OrdinalIgnoreCase).Should().Be(-1);
        html.Should().Contain("<p>ok</p>");
    }

    /// <summary>
    /// Ensures script tags remain when caller explicitly opts out of sanitization.
    /// </summary>
    [Fact]
    public void Markup_WhenSanitizeDisabled_KeepsRawHtml()
    {
        var view = new TestMarkdownView();
        view.Configure("<script>alert('x')</script>", allowHtml: true, sanitize: false);

        var html = view.RenderMarkup();

        html.Should().Contain("<script>alert('x')</script>");
    }

    private sealed class TestMarkdownView : FWMarkdownView
    {
        public void Configure(string markdown, bool allowHtml = false, bool sanitize = true, MarkdownPipelinePreset preset = MarkdownPipelinePreset.Advanced)
        {
            this.Markdown = markdown;
            this.AllowHtml = allowHtml;
            this.Sanitize = sanitize;
            this.PipelinePreset = preset;
        }

        public string RenderMarkup()
        {
            this.TriggerParameterSet();

            var value = MarkupProperty.GetValue(this);
            if (value is not MarkupString markup)
            {
                throw new InvalidOperationException("Expected Markup to be a MarkupString.");
            }

            return markup.Value;
        }

        private void TriggerParameterSet() => base.OnParametersSet();
    }
}
