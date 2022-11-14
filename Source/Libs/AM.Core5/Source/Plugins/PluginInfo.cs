// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PliginInfo.cs -- информация о плагине
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Plugins;

/// <summary>
/// Информация о плагине.
/// </summary>
public sealed class PluginInfo
{
    #region Properties

    /// <summary>
    /// Имя плагина (произвольное, уникальное).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Описание в произвольной форме.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Версия.
    /// </summary>
    public Version? Version { get; set; }

    #endregion
}
