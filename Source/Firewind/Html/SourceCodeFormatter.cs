namespace Firewind.Html;

using ColorCode;
using ColorCode.Common;
using ColorCode.Parsing;
using ColorCode.Styling;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Text;

internal class SourceCodeFormatter : CodeColorizerBase
{
    private TextWriter writer = null!;
    private int lineNumber = 1;
    private static readonly string[] separator = ["\r\n", "\r", "\n"];

    public SourceCodeFormatter(StyleDictionary? style = null, ILanguageParser? languageParser = null)
        : base(style, languageParser)
    { }

    public MarkupString GetMarkupString(string sourceCode, ILanguage language)
    {
        this.lineNumber = 1;
        var buffer = new StringBuilder();

        using (this.writer = new StringWriter(buffer))
        {
            this.languageParser.Parse(sourceCode, language, Write);
            this.writer.Flush();
        }

        return new MarkupString(buffer.ToString());
    }

    protected override void Write(string parsedSourceCode, IList<Scope> scopes)
    {
        var styleInsertions = new List<TextInsertion>();

        foreach (var scope in scopes)
        {
            GetStyleInsertionsForCapturedStyle(scope, styleInsertions);
        }

        styleInsertions.SortStable((x, y) => x.Index.CompareTo(y.Index));

        var offset = 0;

        foreach (var styleInsertion in styleInsertions)
        {
            WriteHtmlEncodedText(parsedSourceCode.Substring(offset, styleInsertion.Index - offset));

            if (string.IsNullOrEmpty(styleInsertion.Text))
                BuildSpanForCapturedStyle(styleInsertion.Scope);
            else
                this.writer.Write(styleInsertion.Text);

            offset = styleInsertion.Index;
        }

        WriteHtmlEncodedText(parsedSourceCode.Substring(offset));
    }

    private void WriteHtmlEncodedText(string text)
    {
        var encodedText = WebUtility.HtmlEncode(text);
        var lines = encodedText.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < lines.Length; i++)
        {
            if (i > 0)
            {
                this.writer.Write("</code></pre>"); // Close previous tags
                lineNumber++;
            }

            this.writer.Write($"<pre data-prefix=\"{lineNumber}\"><code>");
            this.writer.Write(lines[i]);
            if (i == lines.Length - 1)
            {
                this.writer.Write("</code></pre>"); // Close tags at the end
            }
        }
    }

    private void BuildSpanForCapturedStyle(Scope scope)
    {
        var cssClassName = "";

        if (this.Styles.Contains(scope.Name))
        {
            var style = this.Styles[scope.Name];

            cssClassName = style.ReferenceName;
        }

        this.writer.Write($"<span class=\"{cssClassName}\">");
    }

    private static void GetStyleInsertionsForCapturedStyle(Scope scope, ICollection<TextInsertion> styleInsertions)
    {
        styleInsertions.Add(new TextInsertion
        {
            Index = scope.Index,
            Scope = scope
        });

        foreach (var childScope in scope.Children)
        {
            GetStyleInsertionsForCapturedStyle(childScope, styleInsertions);
        }

        styleInsertions.Add(new TextInsertion
        {
            Index = scope.Index + scope.Length,
            Text = "</span>"
        });
    }
}
