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
        // 先写入临时文件再原子替换，避免序列化过程中进程退出（被杀、崩溃、关机）
        // 在目标路径留下一个被截断的、无法解析的文件。
        var tempFilePath = $"{FilePath}.{Guid.NewGuid():N}.tmp";

        try
        {
            await using (var fileStream = File.Create(tempFilePath))
            {
                await JsonSerializer.SerializeAsync(fileStream, obj, JsonSerializerOptions);
                // Flush(true) 会把数据刷到磁盘而不只是操作系统缓存，断电时也不会得到半个文件。
                fileStream.Flush(true);
            }

            File.Move(tempFilePath, FilePath, true);
        }
        catch
        {
            try
            {
                File.Delete(tempFilePath);
            }
            catch
            {
                // 清理临时文件失败不应该掩盖原本的异常。
            }

            throw;
        }
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
