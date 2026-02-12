namespace Firewind.Showcase.Data;

using Firewind.Data;
using Microsoft.AspNetCore.Components;
using System.Reflection;

public record struct ComponentDemo(Type ComponentType, string Title, string Article);

public sealed class DemoProvider : InMemoryDataSource<ComponentDemo>
{
    public DemoProvider() : base([])
    {
        var assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Entry assembly is not available.");

        var demos = assembly.GetTypes()
                            .Where(type => (type.Namespace?.StartsWith("Firewind.Showcase.Components.Demos") ?? false)
                                        && !type.IsAbstract
                                        && type.IsAssignableTo(typeof(IComponent))
                                        && type.IsPublic)
                            .OrderBy(type => type.FullName);

        foreach (var demo in demos)
        {
            var title = demo.Name.EndsWith("Demo") ? demo.Name[..^4] : demo.Name;
            AddItemAsync(new(demo, title, $"Documentation for the {demo.Name}.")).GetAwaiter().GetResult();
        }
    }
}
