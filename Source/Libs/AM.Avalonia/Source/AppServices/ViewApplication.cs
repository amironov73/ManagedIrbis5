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

/* ViewApplication.cs -- построитель приложения, запускающего View
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

#nullable enable

namespace AM.Avalonia.AppServices;

/// <summary>
/// Построитель приложения на Avalonia, запускающего View.
/// </summary>
public sealed class ViewApplication
    : IAvaloniaApplicationBuilder
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    private ViewApplication
        (
            string[] args
        )
    {
        Sure.NotNull (args);

        _instance = this;
        _args = args;
        _viewCreator = _ => new UserControl();
        _appBuilder = AppBuilder.Configure<AvaloniaApplication>()
            .UsePlatformDetect()
            .LogToTrace();
    }

    #endregion

    #region Private members

    private readonly string[] _args;
    internal static ViewApplication _instance = null!;
    internal static AppBuilder _appBuilder = null!;
    internal List<Action<HostBuilderContext, IServiceCollection>> _configurationActions = new();
    private Func<AvaloniaApplication, Control> _viewCreator;

    #endregion

    #region Public methods

    /// <summary>
    /// Создание приложения.
    /// </summary>
    public static ViewApplication BuildAvaloniaApp
        (
            string[] args
        )
    {
        return new ViewApplication (args);
    }

    /// <summary>
    /// Конфигурирование сервисов.
    /// </summary>
    public ViewApplication ConfigureServices
        (
            Action <HostBuilderContext, IServiceCollection> action
        )
    {
        Sure.NotNull (action);

        _configurationActions.Add (action);

        return this;
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
    /// Использовать указанный View.
    /// </summary>
    public ViewApplication UseMainView
        (
            Func<AvaloniaApplication, UserControl> viewCreator
        )
    {
        Sure.NotNull (viewCreator);

        _viewCreator = viewCreator;

        return this;
    }

    /// <summary>
    /// Использовать указанное окно.
    /// </summary>
    public ViewApplication UseMainView<TView>()
        where TView: UserControl, new()
    {
        _viewCreator = _ => new TView();

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

        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IAvaloniaApplicationBuilder.CreateMainWindow"/>
    public Control CreateMainView
        (
            AvaloniaApplication application
        )
    {
        Sure.NotNull (application);

        return _viewCreator (application);
    }

    #endregion
}
