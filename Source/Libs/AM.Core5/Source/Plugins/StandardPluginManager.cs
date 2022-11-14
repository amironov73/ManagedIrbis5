// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StandardPluginLoader.cs -- стандартный загрузчик плагинов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.IO;
using System.Reflection;

using AM.Json;

using Autofac;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Plugins;

/// <summary>
/// Стандартный загрузчик плагинов.
/// </summary>
public sealed class StandardPluginManager
    : IPluginManager
{
    #region Properties

    /// <inheritdoc cref="IPluginManager.Plugins"/>
    public IReadOnlyDictionary<string, PluginDescription> Plugins { get; private set; }

    /// <inheritdoc cref="IPluginManager.Container"/>
    public IContainer Container { get; private set; }

    /// <summary>
    /// Директория с описаниями плагинов.
    /// </summary>
    public string PluginDirectory { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public StandardPluginManager()
        : this ("plugins")
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="pluginDirectory"></param>
    public StandardPluginManager
        (
            string pluginDirectory
        )
    {
        Sure.DirectoryExists (pluginDirectory);

        Plugins = new Dictionary<string, PluginDescription>();
        Container = null!;
        PluginDirectory = pluginDirectory;
        _logger = LoggingUtility.GetLogger (Magna.Host.Services, typeof (StandardPluginManager));
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;

    #endregion

    #region IPluginManager members

    /// <inheritdoc cref="IPluginManager.LoadPlugins"/>
    public void LoadPlugins()
    {
        var loadedPlugins = new Dictionary<string, PluginDescription>();
        var foundFiles = Directory.GetFiles
            (
                PluginDirectory,
                "*.plugin",
                SearchOption.TopDirectoryOnly
            );
        var builder = new ContainerBuilder();

        foreach (var descriptionFile in foundFiles)
        {
            _logger.LogInformation ("Plugin description {Description}", descriptionFile);
            var description = JsonUtility.ReadObjectFromFile<PluginDescription> (descriptionFile);
            if (description.Inactive)
            {
                _logger.LogInformation ("Inactive, skipping");
                continue;
            }

            var assemblyPath = Path.Combine (PluginDirectory, description.Dll.ThrowIfNullOrEmpty());
            _logger.LogInformation ("Loading assembly {Assembly}", assemblyPath);
            var assembly = Assembly.LoadFrom (assemblyPath);

            builder.RegisterAssemblyModules (assembly);
            loadedPlugins.Add
                (
                    description.Name.ThrowIfNullOrEmpty(),
                    description
                );
        }

        Plugins = loadedPlugins;
        Container = builder.Build();
    }

    #endregion
}
