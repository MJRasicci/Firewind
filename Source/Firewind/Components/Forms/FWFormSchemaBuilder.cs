namespace Firewind.Components;

using System.Globalization;
using System.Reflection;

/// <summary>
/// Generates field descriptors from model metadata.
/// </summary>
public static class FWFormSchemaBuilder
{
    /// <summary>
    /// Builds field definitions for a model type.
    /// </summary>
    /// <param name="modelType">The model type.</param>
    /// <param name="maxDepth">The maximum recursion depth for nested object traversal.</param>
    /// <returns>A list of generated field definitions.</returns>
    public static IReadOnlyList<FWFormFieldDefinition> Build(Type modelType, int maxDepth = 2)
    {
        ArgumentNullException.ThrowIfNull(modelType);

        if (maxDepth < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxDepth), maxDepth, "Max depth must be greater than zero.");
        }

        var fields = new List<FWFormFieldDefinition>();
        AddFields(modelType, prefix: string.Empty, depth: 1, maxDepth, fields);

        return fields
            .OrderBy(static field => field.Order)
            .ThenBy(static field => field.Path, StringComparer.Ordinal)
            .ToArray();
    }

    private static void AddFields(Type type, string prefix, int depth, int maxDepth, ICollection<FWFormFieldDefinition> fields)
    {
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!property.CanRead || !property.CanWrite || property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            var propertyPath = string.IsNullOrWhiteSpace(prefix) ? property.Name : $"{prefix}.{property.Name}";
            var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

            if (TryGetInputKind(propertyType, out var kind))
            {
                fields.Add(new FWFormFieldDefinition
                {
                    Path = propertyPath,
                    Label = ToLabel(property.Name),
                    Kind = kind,
                    Options = kind == FWFormInputKind.Select && propertyType.IsEnum
                        ? BuildEnumOptions(propertyType)
                        : []
                });

                continue;
            }

            if (depth < maxDepth && IsComplexObject(propertyType))
            {
                AddFields(propertyType, propertyPath, depth + 1, maxDepth, fields);
            }
        }
    }

    private static bool TryGetInputKind(Type propertyType, out FWFormInputKind kind)
    {
        if (propertyType.IsEnum)
        {
            kind = FWFormInputKind.Select;
            return true;
        }

        if (propertyType == typeof(string))
        {
            kind = FWFormInputKind.Text;
            return true;
        }

        if (propertyType == typeof(bool))
        {
            kind = FWFormInputKind.Checkbox;
            return true;
        }

        if (propertyType == typeof(DateOnly) || propertyType == typeof(DateTime))
        {
            kind = FWFormInputKind.Date;
            return true;
        }

        if (propertyType == typeof(byte)
            || propertyType == typeof(sbyte)
            || propertyType == typeof(short)
            || propertyType == typeof(ushort)
            || propertyType == typeof(int)
            || propertyType == typeof(uint)
            || propertyType == typeof(long)
            || propertyType == typeof(ulong)
            || propertyType == typeof(float)
            || propertyType == typeof(double)
            || propertyType == typeof(decimal))
        {
            kind = FWFormInputKind.Number;
            return true;
        }

        kind = default;
        return false;
    }

    private static bool IsComplexObject(Type propertyType)
    {
        return propertyType.IsClass && propertyType != typeof(string);
    }

    private static FWFormSelectOption[] BuildEnumOptions(Type enumType)
    {
        return Enum.GetNames(enumType)
            .Select(static name => new FWFormSelectOption(name, ToLabel(name)))
            .ToArray();
    }

    private static string ToLabel(string value)
    {
        return string.Create(CultureInfo.InvariantCulture, $"{value[0]}{string.Concat(value.Skip(1).Select(ch => char.IsUpper(ch) ? $" {ch}" : ch.ToString()))}");
    }
}
