// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Instance.cs -- ссылка на конкретный экземпляр книги
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Ссылка на конкретный экземпляр книги.
/// </summary>
[PublicAPI]
public class Instance
{
    #region Properties

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    [JsonPropertyName ("db")]
    public string? Database { get; set; }

    /// <summary>
    /// Код книги в базе данных.
    /// </summary>
    [JsonPropertyName ("book")]
    public string? Book { get; set; }

    /// <summary>
    /// Инвентарный номер экземпляра.
    /// </summary>
    [JsonPropertyName ("number")]
    public string? Number { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Database}: {Book}: {Number}";

    #endregion
}
