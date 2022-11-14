// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ManualPluginLoader.cs -- псевдозагрузчик плагинов (ручная регистрация)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using Autofac;

#endregion

#nullable enable

namespace AM.Plugins;

/// <summary>
/// Псевдозагрузчик плагинов (ручная регистрация плагинов).
/// Отлично подходит для тестовых сценариев.
/// </summary>
public sealed class ManualPluginManager
    : IPluginManager
{
    #region Properties

    /// <summary>
    /// Построитель контейнера.
    /// </summary>
    public ContainerBuilder Builder { get; }

    /// <inheritdoc cref="IPluginManager.Plugins"/>
    public IReadOnlyDictionary<string, PluginDescription> Plugins => _plugins;

    /// <inheritdoc cref="IPluginManager.Container"/>
    public IContainer Container { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ManualPluginManager()
    {
        Builder = new ContainerBuilder();
        _plugins = new ();
        Container = null!;
    }

    #endregion

    #region Private members

    private readonly Dictionary<string, PluginDescription> _plugins;

    #endregion

    #region IPluginManager members

    /// <inheritdoc cref="IPluginManager.LoadPlugins"/>
    public void LoadPlugins()
    {
        Container = Builder.Build();
    }

    #endregion
}
