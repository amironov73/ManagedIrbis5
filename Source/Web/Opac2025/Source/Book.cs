// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Book.cs -- информация о книге
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Информация о книге.
/// </summary>
[PublicAPI]
public sealed class Book
{
    #region Properties

    /// <summary>
    /// Идентификатор (шифр в базе).
    /// </summary>
    [JsonPropertyName ("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Библиографическое описание.
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Ссылка на изображение обложки (опционально).
    /// </summary>
    [JsonPropertyName ("cover")]
    public string? Cover { get; set; }

    /// <summary>
    /// Ссылки, сопровождающие книгу.
    /// </summary>
    [JsonPropertyName ("links")]
    public Link[]? Links { get; set; }
    /// <summary>
    /// Массив экземпляров.
    /// </summary>
    [JsonPropertyName ("exemplars")]
    public Exemplar[]? Exemplars { get; set; }

    /// <summary>
    /// Карточки.
    /// </summary>
    [JsonPropertyName ("cards")]
    public string[]? Cards { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Id}: {Description}";

    #endregion
}
