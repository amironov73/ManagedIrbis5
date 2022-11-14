// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo

/* IApplicationBackbone.cs -- интерфейс хребта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Plugins;

using Autofac;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.AppServices;

/// <summary>
/// Интерфейс хребта приложения.
/// </summary>
public interface IMagnaBackbone
    : IServiceProvider
{
    #region Properties

    /// <summary>
    /// Хост приложения.
    /// </summary>
    IHost ApplicationHost { get; }

    /// <summary>
    /// Конфигурация приложения.
    /// </summary>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Менеджер плагинов.
    /// </summary>
    public IPluginManager PluginManager { get; }

    /// <summary>
    /// Централизованный логгер.
    /// </summary>
    ILogger Logger { get; }

    #endregion

    #region Methods

    /// <summary>
    /// Настройка логирования.
    /// </summary>
    void ConfigureLogging
        (
            Action<HostBuilderContext, ILoggingBuilder> configureDelegate
        );

    /// <summary>
    /// Добавление сервисов в контейнер. Метод может быть вызван несколько раз.
    /// </summary>
    void ConfigureServices
        (
            Action<HostBuilderContext, IServiceCollection> configureDelegate
        );

    /// <summary>
    /// Добавление сервисов в контейнер. Метод может быть вызван несколько раз.
    /// </summary>
    void ConfigureServices
        (
            Action<IServiceCollection> configureDelegate
        );

    /// <summary>
    /// Инициализация хребта.
    /// </summary>
    void Initialize();

    /// <summary>
    /// Загрузка плагинов.
    /// </summary>
    void LoadPlugins
        (
            IPluginManager? loader = null
        );

    /// <summary>
    /// Завершение работы хребта.
    /// </summary>
    void Shutdown();

    /// <summary>
    /// Ожидание завершения приложения.
    /// </summary>
    void WaitForShutdown();

    #endregion
}
