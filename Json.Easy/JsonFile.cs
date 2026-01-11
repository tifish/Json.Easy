using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Json.Easy;

public class JsonFile
{
    public string FilePath { get; set; }

    public JsonFile(string filePath)
    {
        FilePath = filePath;
    }

    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        WriteIndented = true,
        IgnoreReadOnlyProperties = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
#if NET10_0_OR_GREATER
        NewLine = "\n",
#endif
    };

    public async Task<T?> Load<T>()
        where T : class
    {
        if (!File.Exists(FilePath))
            return default;

        await using var fileStream = File.OpenRead(FilePath);
        return await JsonSerializer.DeserializeAsync<T>(fileStream, JsonSerializerOptions);
    }

    public async Task<JsonNode?> Load(
        JsonNodeOptions? options = null,
        JsonDocumentOptions documentOptions = default,
        CancellationToken cancellationToken = default
    )
    {
        if (!File.Exists(FilePath))
            return null;

#if NET8_0_OR_GREATER
        await using var fileStream = File.OpenRead(FilePath);
        return await JsonNode.ParseAsync(fileStream, options, documentOptions, cancellationToken);
#else
        return JsonNode.Parse(File.ReadAllText(FilePath), options, documentOptions);
#endif
    }

    public async Task Save<T>(T obj)
        where T : class
    {
        await using var fileStream = File.Create(FilePath);
        await JsonSerializer.SerializeAsync(fileStream, obj, JsonSerializerOptions);
    }

    public static T? FromJson<T>(string json)
        where T : class
    {
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }

    public static JsonNode? ParseJson(string json)
    {
        return JsonNode.Parse(json);
    }

    public static string ToJson<T>(T obj)
        where T : class
    {
        return JsonSerializer.Serialize(obj, JsonSerializerOptions);
    }

    public static string ToJson(JsonNode node)
    {
        return node.ToJsonString(JsonSerializerOptions);
    }
}
