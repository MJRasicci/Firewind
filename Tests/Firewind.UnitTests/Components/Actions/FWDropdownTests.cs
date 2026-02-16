namespace Firewind.UnitTests.Components.Actions;

using Firewind.Components;
using FluentAssertions;
using Microsoft.JSInterop;
using System.Reflection;

/// <summary>
/// Verifies JavaScript interop behavior for <see cref="FWDropdown"/>.
/// </summary>
public sealed class FWDropdownTests
{
    private static readonly PropertyInfo JsRuntimeProperty = typeof(FWDropdown)
        .GetProperty("JSRuntime", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("Expected private JSRuntime property on FWDropdown.");

    /// <summary>
    /// Ensures outside-click close is configured as enabled by default.
    /// </summary>
    [Fact]
    public async Task OnAfterRenderAsync_WhenFirstRender_EnablesOutsideClickCloseByDefault()
    {
        await using var dropdown = new TestDropdown();
        var jsRuntime = new TestJsRuntime();
        JsRuntimeProperty.SetValue(dropdown, jsRuntime);

        await dropdown.InvokeAfterRenderAsync(firstRender: true);

        jsRuntime.ImportPath.Should().Be("./_content/Firewind/Components/Actions/FWDropdown.razor.js");
        var configureInvocation = jsRuntime.Module.Invocations.Single(static invocation =>
            string.Equals(invocation.Identifier, "configureOutsideClickClose", StringComparison.Ordinal));
        configureInvocation.Arguments.Should().HaveCount(2);
        configureInvocation.Arguments[0]?.ToString().Should().Be(dropdown.Id);
        configureInvocation.Arguments[1].Should().Be(true);
    }

    /// <summary>
    /// Ensures outside-click close can be disabled by component parameters.
    /// </summary>
    [Fact]
    public async Task OnAfterRenderAsync_WhenCloseOnOutsideClickDisabled_ConfiguresDisabledState()
    {
        await using var dropdown = new TestDropdown();
        dropdown.ConfigureCloseOnOutsideClick(false);

        var jsRuntime = new TestJsRuntime();
        JsRuntimeProperty.SetValue(dropdown, jsRuntime);

        await dropdown.InvokeAfterRenderAsync(firstRender: true);

        var configureInvocation = jsRuntime.Module.Invocations.Single(static invocation =>
            string.Equals(invocation.Identifier, "configureOutsideClickClose", StringComparison.Ordinal));
        configureInvocation.Arguments.Should().HaveCount(2);
        configureInvocation.Arguments[0]?.ToString().Should().Be(dropdown.Id);
        configureInvocation.Arguments[1].Should().Be(false);
    }

    /// <summary>
    /// Ensures dispose removes registered outside-click handlers and releases module resources.
    /// </summary>
    [Fact]
    public async Task DisposeAsync_WhenModuleIsInitialized_RemovesOutsideClickCloseHandlerAndDisposesModule()
    {
        await using var dropdown = new TestDropdown();
        var jsRuntime = new TestJsRuntime();
        JsRuntimeProperty.SetValue(dropdown, jsRuntime);

        await dropdown.InvokeAfterRenderAsync(firstRender: true);
        await dropdown.DisposeAsync();

        jsRuntime.Module.Invocations.Should().Contain(static invocation =>
            string.Equals(invocation.Identifier, "removeOutsideClickClose", StringComparison.Ordinal));
        jsRuntime.Module.IsDisposed.Should().BeTrue();
    }

    private sealed class TestDropdown : FWDropdown
    {
        public void ConfigureCloseOnOutsideClick(bool closeOnOutsideClick)
        {
            this.CloseOnOutsideClick = closeOnOutsideClick;
        }

        public Task InvokeAfterRenderAsync(bool firstRender) => base.OnAfterRenderAsync(firstRender);
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
