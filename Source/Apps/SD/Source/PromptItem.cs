// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

#region Using directives

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace SDHelper;

public sealed class PromptItem
{
    /// <summary>
    /// Заголовок.
    /// </summary>
    [JsonPropertyName ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Текущее значение.
    /// </summary>
    [JsonPropertyName ("value")]
    public string? Value { get; set; }

    /// <summary>
    /// Подсказки.
    /// </summary>
    [JsonPropertyName ("suggestions")]
    public string[]? Suggestions { get; set; }

    public static PromptItem[] LoadFromFile
        (
            string fileName
        )
    {
        var content = File.ReadAllBytes(fileName);
        var options = new JsonReaderOptions
        {
            CommentHandling = JsonCommentHandling.Skip
        };
        var reader = new Utf8JsonReader(content, options);

        return JsonSerializer.Deserialize<PromptItem[]>(ref reader)!;
    }

}
