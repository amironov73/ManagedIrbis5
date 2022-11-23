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
// ReSharper disable UnusedParameter.Local

/* BusyWindow.cs -- окно с полоской "приложение чем-то занято"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Окно с полоской "приложение чем-то занято"
/// </summary>
public class BusyWindow
    : Window
{
    #region Properties

    /// <summary>
    /// Описание свойства "занято".
    /// </summary>
    public static readonly StyledProperty<bool> IsBusyProperty
        = AvaloniaProperty.Register<BusyWindow, bool> (nameof (IsBusy));

    /// <summary>
    /// Свойство "занято".
    /// </summary>
    public bool IsBusy
    {
        get => _busyStripe.Active;
        set => _busyStripe.Active = value;
    }

    /// <summary>
    /// Описание свойства "Заголовок полоски".
    /// </summary>
    public static readonly StyledProperty<string?> HeaderProperty
        = AvaloniaProperty.Register<BusyWindow, string?> (nameof (Header));

    /// <summary>
    /// Свойство "Заголовок полоски".
    /// </summary>
    public string? Header
    {
        get => _busyStripe.Text;
        set => _busyStripe.Text = value;
    }

    /// <summary>
    /// Описание свойство "содержимое окна".
    /// </summary>
    public static readonly StyledProperty<object?> WindowContentProperty
        = AvaloniaProperty.Register<BusyWindow, object?> (nameof (WindowContent));

    /// <summary>
    /// Содержимое окна.
    /// </summary>
    public object? WindowContent
    {
        get => _centralControl.Content;
        set => _centralControl.Content = value;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BusyWindow()
    {
        _busyStripe = new BusyStripe
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            IsVisible = false,
            Height = 17
        };
        _centralControl = new UserControl();
        Content = new DockPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,

            Children =
            {
                _busyStripe.DockTop(),
                _centralControl
            }
        };
    }

    #endregion

    #region Private members

    private readonly BusyStripe _busyStripe;
    private readonly UserControl _centralControl;

    #endregion

    #region Public methods

    /// <summary>
    /// Выполнение некоторого долгого действия.
    /// </summary>
    public async Task Run
        (
            Func<Task> action
        )
    {
        Sure.NotNull (action);

        try
        {
            await Dispatcher.UIThread.InvokeAsync
                (() =>
                {
                    _busyStripe.Active = true;
                    _centralControl.IsEnabled = false;
                });

            await action();
        }
        finally
        {
            await Dispatcher.UIThread.InvokeAsync
                (() =>
                {
                    _busyStripe.Active = false;
                    _centralControl.IsEnabled = true;
                });
        }
    }

    #endregion
}
