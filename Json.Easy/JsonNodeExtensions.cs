using System.Text.Json.Nodes;

namespace Json.Easy;

public static class JsonNodeExtensions
{
    public static T? Get<T>(this JsonNode node, string propertyName, T defaultValue)
    {
        if (node[propertyName] is not JsonValue value)
            return defaultValue;

        return value.GetValue<T>();
    }

    public static void Set<T>(this JsonNode node, string propertyName, T value)
    {
        node[propertyName] = JsonValue.Create(value);
    }

    public static bool TrySet<T>(this JsonNode node, string propertyName, T value)
        where T : IEquatable<T>
    {
        if (node[propertyName] is JsonValue oldValue && oldValue.GetValue<T>().Equals(value))
            return false;

        node.Set(propertyName, value);
        return true;
    }
}
