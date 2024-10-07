// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Link.cs -- информация о ссылке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Информация о ссылке (например, на полный текст книги).
/// </summary>
[PublicAPI]
public sealed class Link
{
    #region Properties

    /// <summary>
    /// Ссылка на объект.
    /// </summary>
    [JsonPropertyName ("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Описание в произвольной форме, например,
    /// "Экземпляр в ЭБС".
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Description}: {Url}";

    #endregion
}
