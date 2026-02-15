namespace Firewind.Showcase.Data;

using Firewind.Data;
using Microsoft.AspNetCore.Components;
using System.Reflection;

internal record struct ComponentDemo(Type ComponentType, string Title, string Article);

internal sealed class DemoProvider : InMemoryDataSource<ComponentDemo>
{
    public DemoProvider() : base(BuildDemos())
    {
    }

    private static IReadOnlyList<ComponentDemo> BuildDemos()
    {
        var assembly = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Entry assembly is not available.");

        return [.. assembly.GetTypes()
                           .Where(type => (type.Namespace?.StartsWith("Firewind.Showcase.Components.Demos", StringComparison.Ordinal) ?? false)
                                       && !type.IsAbstract
                                       && type.IsAssignableTo(typeof(IComponent))
                                       && type.IsPublic)
                           .OrderBy(type => type.FullName)
                           .Select(static demo => new ComponentDemo(
                               demo,
                               demo.Name.EndsWith("Demo", StringComparison.Ordinal) ? demo.Name[..^4] : demo.Name,
                               $"Documentation for the {demo.Name}."))];
    }
}
