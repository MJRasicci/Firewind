namespace Firewind.UnitTests.Components.Mockup;

using Firewind.Components;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Reflection;

/// <summary>
/// Verifies JavaScript interop behavior for <see cref="FWCodeView"/>.
/// </summary>
public sealed class FWCodeViewInteropTests
{
    private static readonly PropertyInfo JsRuntimeProperty = typeof(FWCodeView)
        .GetProperty("JSRuntime", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("Expected private JSRuntime property on FWCodeView.");

    private static readonly MethodInfo CopySourceCodeMethod = typeof(FWCodeView)
        .GetMethod("CopySourceCodeAsync", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("Expected private CopySourceCodeAsync method on FWCodeView.");

    /// <summary>
    /// Ensures the clipboard helper module is imported when copy behavior is enabled on first render.
    /// </summary>
    [Fact]
    public async Task OnAfterRenderAsync_WhenShowCopyEnabled_ImportsClipboardModule()
    {
        await using var codeView = new TestCodeView();
        codeView.Configure(showCopy: true, sourceCode: string.Empty);

        var jsRuntime = new TestJsRuntime();
        JsRuntimeProperty.SetValue(codeView, jsRuntime);

        await codeView.InvokeAfterRenderAsync(firstRender: true);

        jsRuntime.ImportPath.Should().Be("./_content/Firewind/Components/Mockup/FWCodeView.razor.js");
    }

    /// <summary>
    /// Ensures first-render module import is skipped when copy behavior is disabled.
    /// </summary>
    [Fact]
    public async Task OnAfterRenderAsync_WhenShowCopyDisabled_DoesNotImportClipboardModule()
    {
        await using var codeView = new TestCodeView();
        codeView.Configure(showCopy: false, sourceCode: string.Empty);

        var jsRuntime = new TestJsRuntime();
        JsRuntimeProperty.SetValue(codeView, jsRuntime);

        await codeView.InvokeAfterRenderAsync(firstRender: true);

        jsRuntime.ImportPath.Should().BeNull();
    }

    /// <summary>
    /// Ensures clicking copy forwards the original source text to the clipboard helper.
    /// </summary>
    [Fact]
    public async Task CopySourceCodeAsync_WhenShowCopyEnabled_CopiesOriginalSourceText()
    {
        await using var codeView = new TestCodeView();
        codeView.Configure(showCopy: true, sourceCode: "public sealed class Demo { }");

        var jsRuntime = new TestJsRuntime();
        JsRuntimeProperty.SetValue(codeView, jsRuntime);

        await codeView.InvokeCopySourceCodeAsync();

        var copyInvocation = jsRuntime.Module.Invocations.Single(static invocation =>
            string.Equals(invocation.Identifier, "copyText", StringComparison.Ordinal));
        copyInvocation.Arguments.Should().HaveCount(1);
        copyInvocation.Arguments[0].Should().Be(codeView.SourceCode);
    }

    /// <summary>
    /// Ensures the clipboard helper module is disposed when the component is disposed.
    /// </summary>
    [Fact]
    public async Task DisposeAsync_WhenModuleIsInitialized_DisposesModule()
    {
        var codeView = new TestCodeView();
        codeView.Configure(showCopy: true, sourceCode: string.Empty);

        var jsRuntime = new TestJsRuntime();
        JsRuntimeProperty.SetValue(codeView, jsRuntime);

        await codeView.InvokeAfterRenderAsync(firstRender: true);
        await codeView.DisposeAsync();

        jsRuntime.Module.IsDisposed.Should().BeTrue();
    }

    private sealed class TestCodeView : FWCodeView
    {
        public void Configure(bool showCopy, string sourceCode)
        {
            this.ShowCopy = showCopy;
            this.SourceCode = sourceCode;
        }

        public Task InvokeAfterRenderAsync(bool firstRender) => base.OnAfterRenderAsync(firstRender);

        public async Task InvokeCopySourceCodeAsync()
        {
            var result = CopySourceCodeMethod.Invoke(this, [new MouseEventArgs()]);
            if (result is not Task task)
            {
                throw new InvalidOperationException("Expected CopySourceCodeAsync to return a Task.");
            }

            await task;
        }
    }

    private sealed class TestJsRuntime : IJSRuntime
    {
        public TestJsModule Module { get; } = new();

        public string? ImportPath { get; private set; }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            this.InvokeAsync<TValue>(identifier, CancellationToken.None, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            if (!string.Equals(identifier, "import", StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"Unexpected JS runtime invocation: {identifier}");
            }

            this.ImportPath = args?.Length > 0 ? args[0]?.ToString() : null;
            return new ValueTask<TValue>((TValue)(object)this.Module);
        }
    }

    private sealed class TestJsModule : IJSObjectReference
    {
        private readonly List<JsInvocation> invocations = [];

        public bool IsDisposed { get; private set; }

        public IReadOnlyList<JsInvocation> Invocations => this.invocations;

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            this.InvokeAsync<TValue>(identifier, CancellationToken.None, args);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            this.invocations.Add(new JsInvocation(identifier, args is null ? [] : [.. args]));
            return new ValueTask<TValue>(default(TValue)!);
        }

        public ValueTask DisposeAsync()
        {
            this.IsDisposed = true;
            return ValueTask.CompletedTask;
        }
    }

    private sealed record JsInvocation(string Identifier, IReadOnlyList<object?> Arguments);
}
