using Microsoft.JSInterop;

namespace Firewind.JSInterop;

/// <summary>
/// Provides a sample JavaScript interop wrapper that loads a module on first use.
/// </summary>
/// <remarks>
/// Register this service in dependency injection and inject it into Blazor components
/// that need to call <c>exampleJsInterop.js</c>.
/// </remarks>
public class ExampleJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Firewind/exampleJsInterop.js").AsTask());

    /// <summary>
    /// Displays a browser prompt dialog and returns the user-entered value.
    /// </summary>
    /// <param name="message">The prompt message displayed to the user.</param>
    /// <returns>The value entered by the user.</returns>
    public async ValueTask<string> Prompt(string message)
    {
        var module = await moduleTask.Value;
        return await module.InvokeAsync<string>("showPrompt", message);
    }

    /// <summary>
    /// Disposes the loaded JavaScript module when it has been created.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }

        GC.SuppressFinalize(this);
    }
}
