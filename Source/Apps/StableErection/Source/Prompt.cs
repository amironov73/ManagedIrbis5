// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* Prompt.cs -- запрос
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using AM;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace StableErection;

/// <summary>
/// Запрос.
/// </summary>
[PublicAPI]
public sealed class Prompt
{
    #region Properties

    /// <summary>
    /// Заголовок.
    /// </summary>
    [JsonPropertyName ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Комментарий в произвольной форме.
    /// </summary>
    [JsonPropertyName ("comments")]
    public string? Comments { get; set; }

    /// <summary>
    /// Элементы.
    /// </summary>
    [JsonPropertyName ("items")]
    public PromptItem[] Items { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Prompt()
    {
        Items = Array.Empty<PromptItem>();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Загрузка промптов из JSON-файла.
    /// </summary>
    public static Prompt LoadFromFile
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var content = File.ReadAllBytes(fileName);
        var options = new JsonReaderOptions
        {
            CommentHandling = JsonCommentHandling.Skip
        };
        var reader = new Utf8JsonReader(content, options);

        return JsonSerializer.Deserialize<Prompt>(ref reader)!;
    }

    #endregion
}
