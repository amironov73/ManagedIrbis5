// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Reader.cs -- информация о читателе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Информация о читателе.
/// </summary>
[PublicAPI]
public sealed class Reader
{
    #region Properties

    /// <summary>
    /// Номер читательского билета.
    /// </summary>
    [JsonPropertyName ("ticket")]
    public string? Ticket { get; set; }

    /// <summary>
    /// ФИО читателя.
    /// </summary>
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Адрес почты для связи.
    /// </summary>
    [JsonPropertyName ("mail")]
    public string? Mail { get; set; }

    #endregion
}
