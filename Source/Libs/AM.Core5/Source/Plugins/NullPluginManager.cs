// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NullPluginLoader.cs -- пустой загрузчик плагинов (заглушка)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using Autofac;

#endregion

#nullable enable

namespace AM.Plugins;

/// <summary>
/// Пустой загрузчик плагинов (заглушка). Не загружает никаких плагинов вообще.
/// </summary>
public sealed class NullPluginLoader
    : IPluginManager
{
    #region Properties

    /// <inheritdoc cref="IPluginManager.Plugins"/>
    public IReadOnlyDictionary<string, PluginDescription> Plugins { get; }

    /// <inheritdoc cref="IPluginManager.Container"/>
    public IContainer Container { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public NullPluginLoader()
    {
        var builder = new ContainerBuilder();
        Container = builder.Build();
        Plugins = new Dictionary<string, PluginDescription>();
    }

    #endregion

    #region IPluginManager members

    /// <inheritdoc cref="IPluginManager.LoadPlugins"/>
    public void LoadPlugins()
    {
        // пустое тело метода
    }

    #endregion
}
