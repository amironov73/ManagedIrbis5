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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using AM;
using AM.Avalonia;
using AM.Avalonia.AppServices;
using AM.Text;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

using DynamicData;

using ManagedIrbis;
using ManagedIrbis.Formatting;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

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

        this.AttachDevTools();

        Title = "Просмотр базы данных ИРБИС64";
        Width = MinWidth = 600;
        Height = MinHeight = 450;


        _statusTextBox = new TextBlock
        {
            Padding = new Thickness (5)
        };

        _mfnButton = new Button
        {
            Margin = new Thickness (5, 0, 0, 0),
            Content = "Go"
        };
        _mfnButton.Click += MfnButton_Click;

        _mfnTextBox = new TextBox();

        _mfnListBox = new ListBox
        {
            SelectionMode = SelectionMode.Single,
            AutoScrollToSelectedItem = true,
            // VirtualizationMode = ItemVirtualizationMode.None
        };
        _mfnListBox.SelectionChanged += MfnListBox_SelectionChanged;

        _recordTextBox = new TextBox();

        _splitView = new SplitView
        {
            IsPaneOpen = true,
            DisplayMode = SplitViewDisplayMode.Inline,
            OpenPaneLength = 200,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Pane = new DockPanel
            {
                Margin = new Thickness (5),
                Children =
                {
                    new DockPanel
                    {
                        Children =
                        {
                            _mfnButton.DockRight(),
                            _mfnTextBox
                        }
                    }
                    .DockTop(),

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

    private ObservableCollection<object> _mfnList = null!;
    private SplitView _splitView = null!;
    private ListBox _mfnListBox = null!;
    private TextBox _recordTextBox = null!;
    private TextBox _mfnTextBox = null!;
    private Button _mfnButton = null!;
    private TextBlock _statusTextBox = null!;
    private ISyncProvider _provider = null!;
    private HardFormat _format = null!;

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

        _format = new HardFormat (Magna.Host, _provider);

        var maxMfn = _provider.GetMaxMfn();
        SetStatusText ($"Max MFN={maxMfn}");

        _mfnList = new ObservableCollection<object>();
        for (var mfn = 1; mfn <= maxMfn; mfn++)
        {
            _mfnList.Add (mfn);
        }

        _mfnListBox.Items = _mfnList;
        AddDescriptionsNear (1);
    }

    private void SetStatusText
        (
            string text
        )
    {
        _statusTextBox.Text = text;
    }

    private void AddDescriptionsNear
        (
            int selectionIndex
        )
    {
        if (_mfnListBox.Scroll is { } scroll)
        {

        }
        else
        {
            var min = Math.Max (1, selectionIndex - 50);
            var max = Math.Min (_mfnList.Count, selectionIndex + 50);
            for (var mfn = min; mfn < max; mfn++)
            {
                AddDescriptionIfNotYet (mfn);
            }
        }
    }

    private void AddDescriptionIfNotYet (int mfn)
    {
        if (_mfnList[mfn - 1] is MfnListItem listItem)
        {
            if (!string.IsNullOrEmpty (listItem.Description))
            {
                return;
            }
        }

        var newItem = new MfnListItem
        {
            Mfn = mfn
        };
        _mfnList[mfn - 1] = null!;
        _mfnList[mfn - 1] = newItem;

        var parameters = new ReadRecordParameters
        {
            Mfn = mfn
        };
        var record = _provider.ReadRecord<Record> (parameters);
        AddDescriptionIfNotYet (newItem, record);
    }

    private void AddDescriptionIfNotYet
        (
            MfnListItem item,
            Record? record
        )
    {
        if (record is not null)
        {
            if (string.IsNullOrEmpty (item.Description))
            {
                item.Description = GetBriefDescription (record);

                _mfnList[item.Mfn - 1] = null!;
                _mfnList[item.Mfn - 1] = item;
            }

            _recordTextBox.Text = record.ToPlainText();
        }
    }

    private void GotoMfn (MfnListItem item)
    {
        _recordTextBox.Text = null;
        var parameters = new ReadRecordParameters
        {
            Mfn = item.Mfn
        };
        var record = _provider.ReadRecord<Record> (parameters);
        AddDescriptionIfNotYet (item, record);
        AddDescriptionsNear (item.Mfn - 1);
    }

    private void MfnButton_Click
        (
            object? sender,
            RoutedEventArgs eventArgs
        )
    {
        if (int.TryParse (_mfnTextBox.Text, out var mfn))
        {
            var mfnList = _mfnListBox.Items;
            if (mfnList is not null)
            {
                foreach (var item in mfnList)
                {
                    if (item is int itemMfn && itemMfn == mfn)
                    {
                        _mfnListBox.SelectedItem = item;
                        return;
                    }
                    if (item is MfnListItem listItem && listItem.Mfn == mfn)
                    {
                        _mfnListBox.SelectedItem = listItem;
                        return;
                    }
                }
            }
        }
    }

    private string? GetBriefDescription
        (
            Record? record
        )
    {
        if (record is null)
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        _format.Brief (builder, record);

        return builder.ReturnShared();
    }

    private void MfnListBox_SelectionChanged
        (
            object? sender,
            SelectionChangedEventArgs eventArgs
        )
    {
        var selectedItem = _mfnListBox.SelectedItem;
        if (selectedItem is int mfn)
        {
            var newItem = new MfnListItem
            {
                Mfn = mfn
            };
            _mfnList[mfn - 1] = null!;
            _mfnList[mfn - 1] = newItem;
            GotoMfn (newItem);
        }
        if (selectedItem is MfnListItem listItem)
        {
            GotoMfn (listItem);
        }
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
