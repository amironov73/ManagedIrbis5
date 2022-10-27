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
using System.Diagnostics;
using System.Threading.Tasks;

using AM;

using Avalonia;
using Avalonia.Controls;

using AM.Avalonia.Controls;
using AM.Avalonia.Source;

using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Threading;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace PartyStatusA;

/// <summary>
/// Главное окно приложения
/// </summary>
public sealed class MainWindow
    : Window
{
    private LabeledTextBox _numberBox = null!;
    private LabeledComboBox _statusBox = null!;
    private LogBox _logBox = null!;
    private BusyStripe _busyStripe = null!;
    private MenuFile? _menu;

    // Количество обработанных названий книг.
    private int _titleCount;

    // Количество обработанных экземпляров.
    private int _exemplarCount;

    // Количество реально измененных экземпляров.
    private int _changeCount;


    public MainWindow()
    {
        Width = 600;
        Height = 450;
        Padding = new Thickness (10);
        Title = "Статус партии";

        InitializeControls();
        InitializeConnection().Forget();
    }

    private void InitializeControls()
    {
        _numberBox = new LabeledTextBox
        {
            Label = "Номер партии (номер КСУ)"
        };

        _statusBox = new LabeledComboBox
        {
            Label = "Статус",
        };
        _statusBox.SetValue (Grid.ColumnProperty, 2);

        var button = new Button
        {
            Content = "Установить",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        button.SetValue (Grid.ColumnProperty, 4);
        button.Click += Button_Click;

        _busyStripe = new BusyStripe
        {
            Text = "Выполнение операции на сервере"
        };
        _busyStripe.SetValue (Grid.RowProperty, 2);
        _busyStripe.SetValue (Grid.ColumnSpanProperty, 5);

        _logBox = new LogBox();
        _logBox.SetValue (Grid.RowProperty, 4);
        _logBox.SetValue (Grid.ColumnSpanProperty, 5);

        Content = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions ("*, 5, *, 5, Auto"),
            RowDefinitions = new RowDefinitions ("Auto, 5, 15, 5, *"),
            Children =
            {
                _numberBox,
                _statusBox,
                button,
                _logBox
            }
        };
    }

    private async Task InitializeConnection()
    {
        this.ShowVersionInfoInTitle();
        _logBox.Output.PrintSystemInformation();

        var testResult = await TestConnectionAsync();
        await WriteLineAsync
            (
                testResult
                    ? "Соединение с сервером успешно установлено"
                    : "Невозможно установить соединение с сервером"
            );

        if (!ReferenceEquals (_menu, null))
        {
            _statusBox.Items = _menu.Entries.ToArray();
            if (_menu.Entries.Count != 0)
            {
                _statusBox.SelectedIndex = 0;
            }
        }
    }

    private ISyncProvider GetProvider()
    {
        var result = ConnectionUtility.GetConnectionFromConfig();
        result.Connect();

        return result;
    }

    private async Task WriteLineAsync
        (
            string format
        )
    {
        await Dispatcher.UIThread.InvokeAsync
            (
                () => _logBox.Text += format + _logBox.NewLine
            );
    }

    private async Task Run
        (
            Action action
        )
    {
        await Dispatcher.UIThread.InvokeAsync(() => _busyStripe.Active = true);
        try
        {
            await Task.Factory.StartNew (action);
        }
        catch (Exception exception)
        {
            await WriteLineAsync ($"Exception: {exception}");
        }
        finally
        {
            await Dispatcher.UIThread.InvokeAsync(() => _busyStripe.Active = false);
        }
    }

    private async Task<bool> TestConnectionAsync()
    {
        var result = false;

        await Run (() =>
        {
            using var provider = GetProvider();
            if (!provider.IsConnected)
            {
                return;
            }

            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = provider.Database,
                FileName = "ste.mnu"
            };
            _menu = provider.ReadMenu (specification);
            if (_menu is null || !_menu.Verify (false))
            {
                return;
            }

            result = true;
        });

        return result;
    }

    private async Task ProcessMfn
        (
            ISyncProvider provider,
            string number,
            int mfn,
            string newStatus
        )
    {
        await WriteLineAsync (string.Empty);
        await WriteLineAsync ($"MFN={mfn}");

        var record = provider.ReadRecord (mfn);
        if (ReferenceEquals (record, null))
        {
            return;
        }

        _titleCount++;

        var fields = record.Fields
            .GetField (910)
            .GetField ('u', number);
        if (fields.Length == 0)
        {
            return;
        }

        var parameters = new FormatRecordParameters
        {
            Record = record,
            Format = "@sbrief"
        };
        provider.FormatRecords (parameters);
        var description = parameters.Result.AsSingle();
        if (!string.IsNullOrEmpty (description))
        {
            await WriteLineAsync (description);
        }

        foreach (var field in fields)
        {
            _exemplarCount++;
            var oldStatus = field.GetFirstSubFieldValue ('a');
            var inventory = field.GetFirstSubFieldValue ('b');
            var flag = false;
            if (!oldStatus.SameString (newStatus))
            {
                _changeCount++;
                field.SetSubFieldValue ('a', newStatus);
                flag = true;
            }

            var message = flag ? "[" + oldStatus + "]" : "НЕ";
            await WriteLineAsync ($"{inventory}: {message} меняем");
        }

        if (record.Modified)
        {
            await WriteLineAsync ("Сохраняем запись");
            provider.WriteRecord (record);
        }
    }

    private async void ProcessNumber
        (
            string number,
            string status
        )
    {
        _titleCount = 0;
        _exemplarCount = 0;
        _changeCount = 0;

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        await using (var provider = GetProvider())
        {
            await WriteLineAsync ($"КСУ {number}");

            var expression = $"\"NKSU={number}\"";
            var found = provider.Search (expression);
            await WriteLineAsync ($"Найдено: {found.Length}");

            foreach (var mfn in found)
            {
                await ProcessMfn (provider, number, mfn, status);
            }
        }

        await WriteLineAsync (new string ('=', 70));
        stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;
        await WriteLineAsync ($"Затрачено: {elapsed.ToMinuteString()}");
        await WriteLineAsync ($"обработано названий: {_titleCount}");
        await WriteLineAsync ($"экземпляров: {_exemplarCount}");
        await WriteLineAsync ($"изменён статус: {_changeCount}");
        await WriteLineAsync (string.Empty);
    }

    private async void Button_Click
        (
            object? sender,
            RoutedEventArgs e
        )
    {
        var number = _numberBox.Text?.Trim();
        if (string.IsNullOrEmpty (number))
        {
            return;
        }

        var entry = (MenuEntry?) _statusBox.SelectedItem;
        if (entry is null)
        {
            return;
        }

        var status = entry.Code.ThrowIfNull();

        await Run (() => { ProcessNumber (number, status); });
    }
}
