namespace Firewind.UnitTests.Components.Mockup;

using Firewind.Components;
using FluentAssertions;
using Microsoft.AspNetCore.Components;
using System.Reflection;

/// <summary>
/// Verifies formatted code output behavior for <see cref="FWCodeView"/>.
/// </summary>
public sealed class FWCodeViewTests
{
    /// <summary>
    /// Ensures highlighted output does not include a synthetic empty first line.
    /// </summary>
    [Fact]
    public void FormattedSource_WhenSourceDoesNotStartWithNewLine_DoesNotRenderLeadingEmptyLine()
    {
        var codeView = new TestCodeView();
        codeView.Configure("public sealed class Demo { }", "csharp");

        var lines = TestCodeView.ExtractRenderedCodeLines(codeView.GetFormattedMarkup());

        lines.Should().NotBeEmpty();
        lines[0].Should().NotBeEmpty();
    }

    /// <summary>
    /// Ensures caller-provided leading blank lines are preserved.
    /// </summary>
    [Fact]
    public void FormattedSource_WhenSourceStartsWithNewLine_PreservesSingleLeadingBlankLine()
    {
        var codeView = new TestCodeView();
        codeView.Configure("\npublic sealed class Demo { }", "csharp");

        var lines = TestCodeView.ExtractRenderedCodeLines(codeView.GetFormattedMarkup());

        lines.Should().HaveCountGreaterThan(1);
        lines[0].Should().BeEmpty();
        lines[1].Should().NotBeEmpty();
    }

    private sealed class TestCodeView : FWCodeView
    {
        private const string CodeOpenTag = "\"><code>";
        private const string CodeCloseTag = "</code></pre>";
        private const string PreOpenTag = "<pre data-prefix=\"";

        private static readonly PropertyInfo FormattedSourceProperty = typeof(FWCodeView)
            .GetProperty("FormattedSource", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new InvalidOperationException("Expected private FormattedSource property.");

        public void Configure(string sourceCode, string language)
        {
            this.SourceCode = sourceCode;
            this.Language = language;
        }

        public string GetFormattedMarkup()
        {
            var value = FormattedSourceProperty.GetValue(this);
            if (value is not MarkupString markup)
            {
                throw new InvalidOperationException("Expected FormattedSource to return a MarkupString.");
            }

            return markup.Value;
        }

        public static List<string> ExtractRenderedCodeLines(string markup)
        {
            var lines = new List<string>();
            var searchIndex = 0;

            while (searchIndex < markup.Length)
            {
                var preStart = markup.IndexOf(PreOpenTag, searchIndex, StringComparison.Ordinal);
                if (preStart < 0)
                {
                    break;
                }

                var codeStart = markup.IndexOf(CodeOpenTag, preStart, StringComparison.Ordinal);
                if (codeStart < 0)
                {
                    break;
                }

                codeStart += CodeOpenTag.Length;
                var codeEnd = markup.IndexOf(CodeCloseTag, codeStart, StringComparison.Ordinal);
                if (codeEnd < 0)
                {
                    break;
                }

                lines.Add(markup[codeStart..codeEnd]);
                searchIndex = codeEnd + CodeCloseTag.Length;
            }

            return lines;
        }
    }
}
