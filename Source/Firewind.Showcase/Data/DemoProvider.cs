namespace Firewind.Showcase.Data;

using Firewind.Data;
using Microsoft.AspNetCore.Components;
using System.Reflection;

internal readonly record struct ComponentDemo(
    string Key,
    Type ComponentType,
    string Title,
    string Category,
    string Article);

internal sealed class DemoProvider : InMemoryDataSource<ComponentDemo>
{
    private const string DemoNamespacePrefix = "Firewind.Showcase.Components.Demos.";

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
                               demo.FullName ?? demo.Name,
                               demo,
                               demo.Name.EndsWith("Demo", StringComparison.Ordinal) ? demo.Name[..^4] : demo.Name,
                               ResolveCategory(demo),
                               $"Documentation for the {demo.Name}."))];
    }

    private static string ResolveCategory(Type demoType)
    {
        var namespaceName = demoType.Namespace ?? string.Empty;
        if (!namespaceName.StartsWith(DemoNamespacePrefix, StringComparison.Ordinal))
        {
            return "General";
        }

        var categoryNamespace = namespaceName[DemoNamespacePrefix.Length..];
        if (string.IsNullOrWhiteSpace(categoryNamespace))
        {
            return "General";
        }

        var separatorIndex = categoryNamespace.IndexOf('.', StringComparison.Ordinal);
        return separatorIndex < 0
            ? categoryNamespace
            : categoryNamespace[..separatorIndex];
    }
}
