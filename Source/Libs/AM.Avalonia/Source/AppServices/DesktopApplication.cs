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
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

using AM.AppServices;
using AM.Interactivity;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml.Styling;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NLog.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Avalonia.AppServices;

/// <summary>
/// Построитель десктопного приложения на Avalonia.
/// </summary>
public sealed class DesktopApplication
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
        _args = args;
        _windowCreator = _ => new Window();
        _appBuilder = AppBuilder.Configure<AvaloniaApplication>()
            .UsePlatformDetect()
            .LogToTrace();
    }

    #endregion

    #region Private members

    private readonly AppBuilder _appBuilder;
    private readonly string[] _args;
    private Func<AvaloniaApplication, Window> _windowCreator;

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
        return new DesktopApplication (args);
    }

    /// <summary>
    /// Запуск приложения.
    /// </summary>
    /// <returns></returns>
    public int Run()
    {
        _appBuilder.StartWithClassicDesktopLifetime (_args);

        return 0;
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

    #endregion
}
