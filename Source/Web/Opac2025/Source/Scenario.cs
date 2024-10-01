// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Scenario.cs -- сценарий поиска по словарю
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Сценарий поиска по словарю в базе данных ИРБИС64.
/// </summary>
[PublicAPI]
public sealed class Scenario
{
    #region Properties

    /// <summary>
    /// Префикс.
    /// </summary>
    [JsonPropertyName ("prefix")]
    public string? Prefix { get; set; }

    /// <summary>
    /// Описание в произвольной форме.
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Prefix}: {Description}";

    #endregion
}
