// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* CivitMetadata.cs -- метаданные
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Метаданные.
/// </summary>
[PublicAPI]
public sealed class CivitMetadata
{
    #region Properties

    /// <summary>
    /// Общее количество доступных элементов по запросу.
    /// </summary>
    [JsonProperty ("totalItems")]
    public int TotalItems { get; set; }

    /// <summary>
    /// Номер текущей страницы.
    /// </summary>
    [JsonProperty ("currentPage")]
    public int CurrentPage { get; set; }

    /// <summary>
    /// Размер страницы.
    /// </summary>
    [JsonProperty ("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    /// Общее количество доступных страниц.
    /// </summary>
    [JsonProperty ("totalPages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Ссылка на следующую страницу.
    /// </summary>
    [JsonProperty ("nextPage")]
    public string? NextPage { get; set; }

    /// <summary>
    /// Ссылка на предыдущую страницу.
    /// </summary>
    [JsonProperty ("prevPage")]
    public string? PreviousPage { get; set; }

    #endregion
}
