namespace Firewind.Html;

using ColorCode;
using ColorCode.Styling;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Text;

internal sealed class SourceCodeFormatter
{
    private static readonly string[] LineSeparators = ["\r\n", "\n", "\r"];
    private readonly HtmlFormatter formatter;

    public SourceCodeFormatter(StyleDictionary? style = null)
        => this.formatter = new HtmlFormatter(style ?? StyleDictionary.DefaultDark, languageParser: null);

    public MarkupString GetMarkupString(string sourceCode, ILanguage language)
    {
        ArgumentNullException.ThrowIfNull(sourceCode);
        ArgumentNullException.ThrowIfNull(language);

        var buffer = new StringBuilder();
        var lines = sourceCode.Split(LineSeparators, StringSplitOptions.None);

        for (var i = 0; i < lines.Length; i++)
        {
            buffer.Append("<pre data-prefix=\"")
                  .Append(i + 1)
                  .Append("\"><code>")
                  .Append(this.GetLineMarkup(lines[i], language))
                  .Append("</code></pre>");
        }

        return new MarkupString(buffer.ToString());
    }

    private string GetLineMarkup(string line, ILanguage language)
    {
        if (string.IsNullOrEmpty(line))
        {
            return string.Empty;
        }

        const string preOpenTag = "<pre>";
        const string preCloseTag = "</pre>";

        var html = this.formatter.GetHtmlString(line, language);
        var preStart = html.IndexOf(preOpenTag, StringComparison.OrdinalIgnoreCase);
        var preEnd = html.IndexOf(preCloseTag, StringComparison.OrdinalIgnoreCase);
        if (preStart < 0 || preEnd <= preStart)
        {
            return WebUtility.HtmlEncode(line);
        }

        var contentStart = preStart + preOpenTag.Length;
        var contentLength = preEnd - contentStart;
        return html.Substring(contentStart, contentLength).Trim('\r', '\n');
    }
}
