// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PluginDescription.cs -- описание плагина
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.Json.Serialization;

#endregion

#nullable enable

namespace AM.Plugins;

/// <summary>
/// Описание плагина.
/// </summary>
public sealed class PluginDescription
{
    #region Properties

    /// <summary>
    /// Имя плагина, произвольное, уникальное.
    /// </summary>
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Описание плагина в произвольной форме.
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Версия плагина.
    /// </summary>
    [JsonPropertyName ("version")]
    public Version? Version { get; set; }

    /// <summary>
    /// Имя динамической библиотеки, содержащей плагин.
    /// </summary>
    public string? Dll { get; set; }

    /// <summary>
    /// Пропустить плагин при загрузке.
    /// </summary>
    [JsonPropertyName ("skip")]
    public bool Inactive { get; set; }

    #endregion
}
