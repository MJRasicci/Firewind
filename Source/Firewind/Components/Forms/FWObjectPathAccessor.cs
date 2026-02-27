namespace Firewind.Components;

using System.Globalization;
using System.Reflection;

/// <summary>
/// Reads and writes model values using dotted property paths.
/// </summary>
public static class FWObjectPathAccessor
{
    /// <summary>
    /// Gets a value from an object by property path.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <param name="path">The dotted property path.</param>
    /// <returns>The resolved value, if found; otherwise <see langword="null"/>.</returns>
    public static object? GetValue(object target, string path)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var current = target;

        foreach (var segment in segments)
        {
            if (current is null)
            {
                return null;
            }

            var property = GetProperty(current.GetType(), segment);
            current = property.GetValue(current);
        }

        return current;
    }

    /// <summary>
    /// Sets a value on an object by property path.
    /// </summary>
    /// <param name="target">The target object.</param>
    /// <param name="path">The dotted property path.</param>
    /// <param name="value">The incoming value.</param>
    public static void SetValue(object target, string path, object? value)
    {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        var segments = path.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var current = target;

        for (var i = 0; i < segments.Length - 1; i++)
        {
            var segment = segments[i];
            var property = GetProperty(current.GetType(), segment);
            var next = property.GetValue(current);

            if (next is null)
            {
                next = CreateDefaultInstance(property.PropertyType);
                property.SetValue(current, next);
            }

            current = next;
        }

        var finalProperty = GetProperty(current.GetType(), segments[^1]);
        var convertedValue = ConvertToType(value, finalProperty.PropertyType);
        finalProperty.SetValue(current, convertedValue);
    }

    private static PropertyInfo GetProperty(Type type, string propertyName)
    {
        return type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
            ?? throw new InvalidOperationException($"Property '{propertyName}' was not found on type '{type.FullName}'.");
    }

    private static object? ConvertToType(object? value, Type targetType)
    {
        var nonNullableType = Nullable.GetUnderlyingType(targetType) ?? targetType;

        if (value is null)
        {
            return targetType.IsValueType && Nullable.GetUnderlyingType(targetType) is null
                ? Activator.CreateInstance(targetType)
                : null;
        }

        if (nonNullableType.IsInstanceOfType(value))
        {
            return value;
        }

        if (value is string text)
        {
            if (nonNullableType == typeof(string))
            {
                return text;
            }

            if (nonNullableType.IsEnum)
            {
                return Enum.Parse(nonNullableType, text, ignoreCase: true);
            }

            if (nonNullableType == typeof(bool) && bool.TryParse(text, out var booleanResult))
            {
                return booleanResult;
            }

            if (nonNullableType == typeof(DateOnly) && DateOnly.TryParse(text, CultureInfo.InvariantCulture, out var dateOnlyResult))
            {
                return dateOnlyResult;
            }

            if (nonNullableType == typeof(DateTime) && DateTime.TryParse(text, CultureInfo.InvariantCulture, out var dateTimeResult))
            {
                return dateTimeResult;
            }
        }

        return Convert.ChangeType(value, nonNullableType, CultureInfo.InvariantCulture);
    }

    private static object CreateDefaultInstance(Type type)
    {
        var targetType = Nullable.GetUnderlyingType(type) ?? type;

        return Activator.CreateInstance(targetType)
            ?? throw new InvalidOperationException($"Unable to create an instance of type '{targetType.FullName}'.");
    }
}
