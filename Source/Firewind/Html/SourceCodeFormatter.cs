namespace Firewind.Html;

using ColorCode;
using ColorCode.Styling;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Text;

/// <summary>
/// Formats source code into line-numbered HTML markup for display components.
/// </summary>
internal sealed class SourceCodeFormatter
{
    private readonly HtmlFormatter formatter;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceCodeFormatter"/> class.
    /// </summary>
    /// <param name="style">
    /// Optional ColorCode style dictionary. When not provided, <see cref="StyleDictionary.DefaultDark"/> is used.
    /// </param>
    public SourceCodeFormatter(StyleDictionary? style = null)
    {
        this.formatter = new HtmlFormatter(style ?? StyleDictionary.DefaultDark, languageParser: null);
    }

    /// <summary>
    /// Converts source code into line-numbered syntax-highlighted markup.
    /// </summary>
    /// <param name="sourceCode">The source code content to format.</param>
    /// <param name="language">The ColorCode language descriptor to use.</param>
    /// <returns>A <see cref="MarkupString"/> containing generated HTML.</returns>
    public MarkupString GetMarkupString(string sourceCode, ILanguage language)
    {
        ArgumentNullException.ThrowIfNull(sourceCode);
        ArgumentNullException.ThrowIfNull(language);

        var highlightedContent = GetHighlightedContent(sourceCode, language);
        var lineMarkup = SplitHighlightedLines(highlightedContent);
        var buffer = new StringBuilder();

        for (var i = 0; i < lineMarkup.Count; i++)
        {
            buffer.Append("<pre data-prefix=\"")
                  .Append(i + 1)
                  .Append("\"><code>")
                  .Append(lineMarkup[i])
                  .Append("</code></pre>");
        }

        return new MarkupString(buffer.ToString());
    }

    /// <summary>
    /// Formats source code as a single block and extracts content within the generated <c>pre</c> wrapper.
    /// </summary>
    /// <param name="sourceCode">The source code text to format.</param>
    /// <param name="language">The ColorCode language descriptor to use.</param>
    /// <returns>Syntax-highlighted HTML content without outer <c>pre</c> tags.</returns>
    private string GetHighlightedContent(string sourceCode, ILanguage language)
    {
        var html = this.formatter.GetHtmlString(sourceCode, language);

        if (TryExtractPreContent(html, out var content))
        {
            return content;
        }

        return WebUtility.HtmlEncode(sourceCode);
    }

    /// <summary>
    /// Extracts inner content from the first <c>pre</c> element in a formatted HTML block.
    /// </summary>
    /// <param name="html">The formatted HTML block.</param>
    /// <param name="content">When this method returns, contains extracted content if found.</param>
    /// <returns><see langword="true"/> when extraction succeeds; otherwise <see langword="false"/>.</returns>
    private static bool TryExtractPreContent(string html, out string content)
    {
        const string preOpenTag = "<pre";
        const string preCloseTag = "</pre>";

        var preStart = html.IndexOf(preOpenTag, StringComparison.OrdinalIgnoreCase);
        if (preStart < 0)
        {
            content = string.Empty;
            return false;
        }

        var preOpenEnd = html.IndexOf('>', preStart);
        if (preOpenEnd < 0)
        {
            content = string.Empty;
            return false;
        }

        var preEnd = html.LastIndexOf(preCloseTag, StringComparison.OrdinalIgnoreCase);
        if (preEnd <= preOpenEnd)
        {
            content = string.Empty;
            return false;
        }

        var contentStart = preOpenEnd + 1;
        var contentLength = preEnd - contentStart;
        content = html.Substring(contentStart, contentLength);
        return true;
    }

    /// <summary>
    /// Splits highlighted HTML into per-line fragments while preserving tag correctness per line.
    /// </summary>
    /// <param name="highlightedContent">The highlighted HTML content to split.</param>
    /// <returns>A list of line-level HTML fragments.</returns>
    private static List<string> SplitHighlightedLines(string highlightedContent)
    {
        var lines = new List<string>();
        var currentLine = new StringBuilder();
        var openTags = new List<OpenTag>();

        for (var i = 0; i < highlightedContent.Length;)
        {
            if (TryReadNewLine(highlightedContent, i, out var newlineLength))
            {
                AppendClosingTags(currentLine, openTags);
                lines.Add(currentLine.ToString());
                currentLine.Clear();
                AppendOpeningTags(currentLine, openTags);
                i += newlineLength;
                continue;
            }

            if (highlightedContent[i] == '<')
            {
                var tagEnd = highlightedContent.IndexOf('>', i);
                if (tagEnd < 0)
                {
                    currentLine.Append(highlightedContent.AsSpan(i));
                    break;
                }

                var tag = highlightedContent.Substring(i, tagEnd - i + 1);
                currentLine.Append(tag);
                UpdateOpenTags(openTags, tag);
                i = tagEnd + 1;
                continue;
            }

            currentLine.Append(highlightedContent[i]);
            i++;
        }

        AppendClosingTags(currentLine, openTags);
        lines.Add(currentLine.ToString());

        return lines;
    }

    /// <summary>
    /// Appends opening tags for all currently open HTML scopes.
    /// </summary>
    /// <param name="builder">The destination builder.</param>
    /// <param name="openTags">The active open tag stack.</param>
    private static void AppendOpeningTags(StringBuilder builder, IReadOnlyList<OpenTag> openTags)
    {
        for (var i = 0; i < openTags.Count; i++)
        {
            builder.Append(openTags[i].OpeningTag);
        }
    }

    /// <summary>
    /// Appends closing tags for all currently open HTML scopes in reverse order.
    /// </summary>
    /// <param name="builder">The destination builder.</param>
    /// <param name="openTags">The active open tag stack.</param>
    private static void AppendClosingTags(StringBuilder builder, IReadOnlyList<OpenTag> openTags)
    {
        for (var i = openTags.Count - 1; i >= 0; i--)
        {
            builder.Append("</")
                   .Append(openTags[i].Name)
                   .Append('>');
        }
    }

    /// <summary>
    /// Updates the list of open tags based on a parsed HTML tag token.
    /// </summary>
    /// <param name="openTags">The active open tag list.</param>
    /// <param name="tag">The raw HTML tag token.</param>
    private static void UpdateOpenTags(List<OpenTag> openTags, string tag)
    {
        if (tag.StartsWith("<!--", StringComparison.Ordinal) ||
            tag.StartsWith("<!", StringComparison.Ordinal) ||
            tag.StartsWith("<?", StringComparison.Ordinal))
        {
            return;
        }

        if (tag.StartsWith("</", StringComparison.Ordinal))
        {
            var closingTagName = GetTagName(tag, startIndex: 2);
            if (string.IsNullOrEmpty(closingTagName))
            {
                return;
            }

            for (var i = openTags.Count - 1; i >= 0; i--)
            {
                if (string.Equals(openTags[i].Name, closingTagName, StringComparison.OrdinalIgnoreCase))
                {
                    openTags.RemoveAt(i);
                    break;
                }
            }

            return;
        }

        if (tag.EndsWith("/>", StringComparison.Ordinal))
        {
            return;
        }

        var openingTagName = GetTagName(tag, startIndex: 1);
        if (string.IsNullOrEmpty(openingTagName))
        {
            return;
        }

        openTags.Add(new OpenTag(openingTagName, tag));
    }

    /// <summary>
    /// Extracts the tag name from an opening or closing HTML tag.
    /// </summary>
    /// <param name="tag">The raw HTML tag token.</param>
    /// <param name="startIndex">The index where tag name scanning should begin.</param>
    /// <returns>The parsed tag name, or an empty string when parsing fails.</returns>
    private static string GetTagName(string tag, int startIndex)
    {
        var i = startIndex;
        while (i < tag.Length && char.IsWhiteSpace(tag[i]))
        {
            i++;
        }

        var nameStart = i;
        while (i < tag.Length && !char.IsWhiteSpace(tag[i]) && tag[i] != '>' && tag[i] != '/')
        {
            i++;
        }

        return i > nameStart
            ? tag[nameStart..i]
            : string.Empty;
    }

    /// <summary>
    /// Determines whether a newline starts at the specified index and returns its character length.
    /// </summary>
    /// <param name="value">The text to inspect.</param>
    /// <param name="index">The index to inspect.</param>
    /// <param name="newlineLength">When this method returns, contains newline length when found.</param>
    /// <returns><see langword="true"/> when a newline is present; otherwise <see langword="false"/>.</returns>
    private static bool TryReadNewLine(string value, int index, out int newlineLength)
    {
        if (value[index] == '\r')
        {
            newlineLength = index + 1 < value.Length && value[index + 1] == '\n' ? 2 : 1;
            return true;
        }

        if (value[index] == '\n')
        {
            newlineLength = 1;
            return true;
        }

        newlineLength = 0;
        return false;
    }

    /// <summary>
    /// Represents a parsed opening tag that remains active at the current read position.
    /// </summary>
    /// <param name="Name">The tag name.</param>
    /// <param name="OpeningTag">The original opening tag text, including attributes.</param>
    private sealed record OpenTag(string Name, string OpeningTag);
}
