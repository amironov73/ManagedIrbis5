// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ModelVersion.cs -- информация о версии модели
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Информация о версии модели.
/// </summary>
public sealed class ModelVersion
{
    #region Properties

    /// <summary>
    /// Идентификатор версии.
    /// </summary>
    [JsonProperty ("id")]
    public int VersionId { get; set; }

    /// <summary>
    /// Идентификатор модели.
    /// </summary>
    [JsonProperty ("modelId")]
    public int ModelId { get; set; }

    /// <summary>
    /// Номер версии.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    [JsonProperty ("createdAt")]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Дата обновления.
    /// </summary>
    [JsonProperty ("updatedAt")]
    public DateTime? Updated { get; set; }

    /// <summary>
    /// Слова для активации.
    /// </summary>
    [JsonProperty ("trainedWord")]
    public string[]? Words { get; set; }

    /// <summary>
    /// Базовая модель.
    /// </summary>
    [JsonProperty ("baseModel")]
    public string? BaseModel { get; set; }

    /// <summary>
    /// Статистика по версии.
    /// </summary>
    [JsonProperty ("stats")]
    public ModelStats? Stats { get; set; }

    /// <summary>
    /// Файлы модели.
    /// </summary>
    [JsonProperty ("files")]
    public FileInfo[]? Files { get; set; }

    /// <summary>
    /// Прилагаемые изображения.
    /// </summary>
    [JsonProperty ("images")]
    public ImageInfo[]? Images { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Name.ToVisibleString();

    #endregion
}
