// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* DesktopApplication.cs -- построитель десктопного приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using AM.Avalonia.Dialogs;

using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Avalonia.AppServices;

/// <summary>
/// Построитель десктопного приложения на Avalonia.
/// </summary>
public sealed class DesktopApplication
    : IAvaloniaApplicationBuilder
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    private DesktopApplication
        (
            string[] args
        )
    {
        Sure.NotNull (args);

        _instance = this;
        _args = args;
        _windowCreator = _ => new Window();
        _appBuilder = AppBuilder.Configure<AvaloniaApplication>()
            .UsePlatformDetect()
            .UseReactiveUI()
            .LogToTrace();
    }

    #endregion

    #region Private members

    private readonly string[] _args;
    internal static DesktopApplication _instance = null!;
    internal static AppBuilder _appBuilder = null!;
    internal List<Action<HostBuilderContext, IServiceCollection>> _configurationActions = new();
    private Func<AvaloniaApplication, Window> _windowCreator;
    internal static string? _applicationName;
    internal static NativeMenu? _nativeMenu;

    #endregion

    #region Public methods

    /// <summary>
    /// Создание приложения.
    /// </summary>
    public static DesktopApplication BuildAvaloniaApp
        (
            string[] args
        )
    {
        Magna.Initialize (args);

        return new DesktopApplication (args);
    }

    /// <summary>
    /// Конфигурирование сервисов.
    /// </summary>
    public DesktopApplication ConfigureServices
        (
            Action <HostBuilderContext, IServiceCollection> action
        )
    {
        Sure.NotNull (action);

        _configurationActions.Add (action);

        return this;
    }

    /// <summary>
    /// Запуск приложения в простейшей конфигурации
    /// с указанным главным окном.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <typeparam name="TWindow">Класс главного окна.</typeparam>
    /// <returns>Код возврата.</returns>
    public static int Run<TWindow>
        (
            string[] args
        )
        where TWindow: Window, new()
    {
        try
        {
            BuildAvaloniaApp (args)
            .UseMainWindow<TWindow>()
            .Run();
        }
        catch (Exception exception)
        {
            Magna.Logger.LogCritical (exception, "Can't run the application");

            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Запуск приложения.
    /// </summary>
    /// <returns>Код, который необходимо вернуть операционной системе</returns>
    public AvaloniaApplication Run()
    {
        _appBuilder.StartWithClassicDesktopLifetime (_args);

        return (AvaloniaApplication) _appBuilder.Instance.ThrowIfNull();
    }

    /// <summary>
    /// Использовать указанное окно.
    /// </summary>
    public DesktopApplication UseMainWindow
        (
            Func<AvaloniaApplication, Window> windowCreator
        )
    {
        Sure.NotNull (windowCreator);

        _windowCreator = windowCreator;

        return this;
    }

    /// <summary>
    /// Использовать указанное окно.
    /// </summary>
    public DesktopApplication UseMainWindow<TWindow>()
        where TWindow: Window, new()
    {
        _windowCreator = _ => new TWindow();

        return this;
    }

    /// <summary>
    /// Имя приложения (для OSX -- отображается в верхней общей строке меню).
    /// </summary>
    public DesktopApplication WithApplicationName
        (
            string? applicationName = null
        )
    {
        if (string.IsNullOrEmpty (applicationName))
        {
            var location = Assembly.GetEntryAssembly()?.Location;
            if (!string.IsNullOrEmpty (location))
            {
                applicationName = Path.GetFileNameWithoutExtension (location);
            }
        }

        _applicationName = applicationName;

        return this;
    }

    /// <summary>
    /// Нативное меню (для OSX -- отображается в верхней общей строке меню).
    /// </summary>
    public DesktopApplication WithNativeMenu
        (
            NativeMenu? nativeMenu = null
        )
    {
        _nativeMenu = nativeMenu ?? AboutDialog.BuildNativeMenuAboutApplication();

        return this;
    }

    /// <returns></returns>
    public DesktopApplication With<T>
        (
            T options
        )
    {
        _appBuilder.With (options);

        return this;
    }

    #endregion

    #region IAvaloniaApplicationBuilder members

    /// <inheritdoc cref="IAvaloniaApplicationBuilder.CreateMainWindow"/>
    public Window CreateMainWindow
        (
            AvaloniaApplication application
        )
    {
        Sure.NotNull (application);

        return _windowCreator (application);
    }

    /// <inheritdoc cref="IAvaloniaApplicationBuilder.CreateMainView"/>
    public Control CreateMainView
        (
            AvaloniaApplication application
        )
    {
        Sure.NotNull (application);

        throw new NotImplementedException();
    }

    #endregion
}
