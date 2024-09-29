// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* Exemplar.cs -- информация об экземпляре
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Информация об экземпляре.
/// </summary>
[PublicAPI]
public class Exemplar
{
    #region Properties

    /// <summary>
    /// Номер экземпляра.
    /// </summary>
    [JsonPropertyName ("number")]
    public string? Number { get; set; }

    /// <summary>
    /// Статус экземпляра.
    /// </summary>
    [JsonPropertyName ("status")]
    public string? Status { get; set; }

    #endregion
}
