// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Reactive.Linq;

using AM;
using AM.Avalonia;
using AM.Avalonia.AppServices;
using AM.Parameters;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.ReactiveUI;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Providers;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace DirectViewer;

/// <summary>
/// Главное окно приложения.
/// </summary>
public sealed class MainWindow
    : Window
{
    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = "Просмотр базы данных ИРБИС64";
        Width = MinWidth = 600;
        Height = MinHeight = 450;


        _statusTextBox = new TextBlock
        {
            Padding = new Thickness (5)
        };

        _mfnListBox = new ListBox
        {
        };

        _recordTextBox = new TextBox
        {
        };



        _splitView = new SplitView
        {
            IsPaneOpen = true,
            DisplayMode = SplitViewDisplayMode.Inline,
            OpenPaneLength = 200,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Pane = new DockPanel
            {
                Children =
                {
                    _mfnListBox
                }
            },

            Content = new DockPanel
            {
                Children =
                {
                    _recordTextBox
                }
            }
        };

        Content = new DockPanel
        {
            Children =
            {
                new Border
                {
                    BorderBrush = Brushes.DarkGray,
                    BorderThickness = new Thickness (1),
                    Padding = new Thickness (5),
                    Child = _statusTextBox
                }
                .DockBottom(),
                _splitView
            }
        };

        ReadInitialData();
    }

    protected override void OnClosing (CancelEventArgs e)
    {
        if (_provider != null!)
        {
            _provider.Dispose();
        }

        base.OnClosing (e);
    }

    #endregion

    #region Private members

    private SplitView _splitView = null!;
    private ListBox _mfnListBox = null!;
    private TextBox _recordTextBox = null!;
    private TextBlock _statusTextBox = null!;
    private ISyncProvider _provider = null!;

    private void ReadInitialData()
    {
        if (Magna.Args.Length < 1)
        {
            Close();
            return;
        }

        var argument = Magna.Args[0];

        if (string.IsNullOrEmpty (argument))
        {
            Close();
            return;
        }

        _provider = ProviderManager.GetAndConfigureProvider (argument);
        _provider.Connect();
        if (!_provider.IsConnected)
        {
            Close();
            return;
        }

        var maxMfn = _provider.GetMaxMfn();
        SetStatusText ($"Max MFN={maxMfn}");
    }

    private void SetStatusText
        (
            string text
        )
    {
        _statusTextBox.Text = text;
    }

    #endregion

    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    [STAThread]
    public static void Main
        (
            string[] args
        )
    {
        DesktopApplication
            .Run<MainWindow> (args);
    }

    #endregion
}
