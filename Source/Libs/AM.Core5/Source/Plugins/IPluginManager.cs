// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IPluginLoader.cs -- интерфейс загрузчика плагинов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using Autofac;

#endregion

#nullable enable

namespace AM.Plugins;

/// <summary>
/// Интерфейс загрузчика плагинов.
/// </summary>
public interface IPluginManager
{
    /// <summary>
    /// Загруженные плагины.
    /// </summary>
    public IReadOnlyDictionary<string, PluginDescription> Plugins { get; }

    /// <summary>
    /// Контейнер для разрешения зависимостей.
    /// </summary>
    public IContainer Container { get; }

    /// <summary>
    /// Загрузка плагинов.
    /// </summary>
    void LoadPlugins();
}
