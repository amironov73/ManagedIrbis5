// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LinkData.cs -- информация о сокращенной ссылке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;

using JetBrains.Annotations;

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace AM.Web.Shortening;

/// <summary>
/// Информация о сокращенной ссылке.
/// </summary>
[Table ("links")]
[UsedImplicitly]
public sealed class LinkData
{
    #region Properties

    /// <summary>
    /// Идентификатор.
    /// </summary>
    [UsedImplicitly]
    [JsonPropertyName ("id")]
    [Identity, PrimaryKey, Column]
    public int Id { get; set; }

    /// <summary>
    /// Момент создания сокращенной ссылки.
    /// </summary>
    [Column]
    [UsedImplicitly]
    [JsonPropertyName ("moment")]
    public DateTime Moment { get; set; }

    /// <summary>
    /// Полный вариант ссылки (с протоколом, хостом
    /// и всем прочим).
    /// </summary>
    [Column]
    [UsedImplicitly]
    [JsonPropertyName ("full")]
    public string? FullLink { get; set; }

    /// <summary>
    /// Краткий вариант ссылки (без протокола и хоста).
    /// </summary>
    [Column]
    [UsedImplicitly]
    [JsonPropertyName ("short")]
    public string? ShortLink { get; set; }

    /// <summary>
    /// Описание в произвольной форме (опционально).
    /// </summary>
    [UsedImplicitly]
    [Column, Nullable]
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Счетчик загрузок данной ссылки.
    /// </summary>
    [Column]
    [UsedImplicitly]
    [JsonPropertyName ("counter")]
    public int Counter { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() =>
        System.Text.Json.JsonSerializer.Serialize (this);

    #endregion
}
