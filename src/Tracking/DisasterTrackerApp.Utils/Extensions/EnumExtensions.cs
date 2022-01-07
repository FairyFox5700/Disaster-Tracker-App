using System.ComponentModel;
using System.Reflection;

namespace DisasterTrackerApp.Utils.Extensions;

internal static class EnumCacheExtensions
{
    private static TValue? GetValueFromAttribute<TAttribute, TValue>(MemberInfo type, Func<TAttribute, TValue> valueSelector)
    {
        if (type?.GetCustomAttributes(typeof(TAttribute), false).FirstOrDefault() is TAttribute attr)
        {
            return valueSelector(attr);
        }

        return default;
    }

    internal static TValue? GetValueFromEnumAttribute<TAttribute, TValue, T>(this T enumValue, Func<TAttribute, TValue> valueSelector)
        where T : Enum
    {
        var enumInfo = enumValue.GetType().GetField(enumValue.ToString());
        return GetValueFromAttribute(enumInfo, valueSelector);
    }
}

internal static class EnumDescriptionCache<T>
    where T : Enum
{
    private static readonly Dictionary<T, string?> Cache = new Dictionary<T, string?>();

    static EnumDescriptionCache()
    {
        foreach (T enumValue in Enum.GetValues(typeof(T)))
        {
            Cache.TryAdd(enumValue, enumValue.GetValueFromEnumAttribute<DescriptionAttribute, string, Enum>(e => e.Description));
        }
    }

    internal static string? GetDescription(T value)
    {
        Cache.TryGetValue(value, out string? description);
        return description;
    }
}

public static class EnumExtension
{
    public static string? GetDescription<T>(this T enumValue)
        where T : Enum
    {
        return EnumDescriptionCache<T>.GetDescription(enumValue);
    }
}