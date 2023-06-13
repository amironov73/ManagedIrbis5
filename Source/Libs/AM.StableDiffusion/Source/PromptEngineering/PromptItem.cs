// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PromptItem.cs -- элемент промпта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.StableDiffusion.PromptEngineering;

/// <summary>
/// Элемент промпта.
/// </summary>
[PublicAPI]
public sealed class PromptItem
{
    #region Properties

    /// <summary>
    /// Нестандартный префикс (стандартный - пробел).
    /// </summary>
    [JsonPropertyName ("prefix")]
    public string? Prefix { get; set; }

    /// <summary>
    /// Нестандартный разделитель (стандартный - запятая).
    /// </summary>
    public string? Separator { get; set; }

    /// <summary>
    /// Логика сборки элемента запроса из представленных вариантов.
    /// </summary>
    [JsonPropertyName ("logic")]
    public PromptLogic Logic { get; set; }

    /// <summary>
    /// Заголовок.
    /// </summary>
    [JsonPropertyName ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Текущее значение элемента.
    /// </summary>
    [JsonPropertyName ("value")]
    public string? Value { get; set; }

    /// <summary>
    /// Предлагаемые варианты.
    /// </summary>
    [JsonPropertyName ("variants")]
    public PromptVariant[]? Variants { get; set; }

    /// <summary>
    /// Включенный файл.
    /// </summary>
    [JsonPropertyName ("include")]
    public string? Include { get; set; }

    /// <summary>
    /// Вложенные элементы.
    /// </summary>
    [JsonIgnore]
    public PromptItem[]? SubItems { get; set; }

    #endregion
}
