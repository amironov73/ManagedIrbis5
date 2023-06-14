// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* FileInfo.cs -- информация о файле модели
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Информация о файле модели.
/// </summary>
public sealed class FileInfo
{
    #region Properties

    /// <summary>
    /// Идентификатор.
    /// </summary>
    [JsonProperty ("id")]
    public int Id { get; set; }

    /// <summary>
    /// Имя файла.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Размер в килобайтах.
    /// </summary>
    [JsonProperty ("sizeKB")]
    public float SizeKb { get; set; }

    /// <summary>
    /// Тип файла, например, <code>"Model"</code>.
    /// </summary>
    [JsonProperty ("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Ссылка для скачивания.
    /// </summary>
    [JsonProperty ("downloadUrl")]
    public string? DownloadUrl { get; set; }

    /// <summary>
    /// Основной файл модели?
    /// </summary>
    [JsonProperty ("primary")]
    public bool Primary { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Name.ToVisibleString();

    #endregion
}
