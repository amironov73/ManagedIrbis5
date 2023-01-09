// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* BusyWindow.cs -- окно с полоской "приложение чем-то занято"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Threading;

using ReactiveUI;

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
    /// Содержимое окна как Avalonia-контрол.
    /// </summary>
    public InputElement? WindowContent => Content as InputElement;

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
            Height = 17,
            [!BusyStripe.TextProperty] = HeaderProperty
                .WhenAnyValue (x=>x).ToBinding()
        };
        Template = new FuncControlTemplate ((_, _) =>
            new DockPanel
            {
                [!BackgroundProperty] = new TemplateBinding (BackgroundProperty),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,

                Children =
                {
                    _busyStripe.DockTop(),
                    new ContentPresenter
                    {
                        [!ContentProperty] = new TemplateBinding (ContentProperty)
                    }
                }
            });
    }

    #endregion

    #region Private members

    private readonly BusyStripe _busyStripe;

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
                    var content = WindowContent;
                    if (content is not null)
                    {
                        content.IsEnabled = false;
                    }
                });

            await action();
        }
        finally
        {
            await Dispatcher.UIThread.InvokeAsync
                (() =>
                {
                    _busyStripe.Active = false;
                    var content = WindowContent;
                    if (content is not null)
                    {
                        content.IsEnabled = true;
                    }
                });
        }
    }

    #endregion
}
