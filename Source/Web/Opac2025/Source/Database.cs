// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Database.cs -- информация о базе данных ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Информация о базе данных ИРБИС64.
/// </summary>
[PublicAPI]
public sealed class Database
{
    #region Properties

    /// <summary>
    /// Имя базы данных.
    /// </summary>
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Описание в произвольной форме.
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}: {Description}";

    #endregion
}
