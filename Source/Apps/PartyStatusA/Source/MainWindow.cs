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

using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
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
    private Button _button = null!;
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
        MinWidth = 600;
        Height = 350;
        MinHeight = 350;
        Padding = new Thickness (10);
        Title = "Статус партии";

        InitializeControls();
        InitializeConnectionAsync()
            .ContinueWith (_ =>
            {
                Dispatcher.UIThread.InvokeAsync
                    (
                        () => _button.IsEnabled = _menu is not null
                    );
            })
            .Forget();
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

        _button = new Button
        {
            Content = "Установить",
            IsEnabled = false,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Stretch,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        _button.SetValue (Grid.ColumnProperty, 4);
        _button.Click += Button_Click;

        _busyStripe = new BusyStripe
        {
            Text = "Выполнение операции на сервере",
            Active = false
        };
        _busyStripe.SetValue (Grid.RowProperty, 2);
        _busyStripe.SetValue (Grid.ColumnSpanProperty, 5);

        _logBox = new LogBox
        {
            FontFamily = new FontFamily ("Courier"),
            FontSize = 12.0,
            IsReadOnly = true
        };
        _logBox.SetValue (Grid.RowProperty, 4);
        _logBox.SetValue (Grid.ColumnSpanProperty, 5);

        Content = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions ("*, 5, *, 5, Auto"),
            RowDefinitions = new RowDefinitions ("Auto, 5, 20, 5, *"),
            Children =
            {
                _numberBox,
                _statusBox,
                _button,
                _busyStripe,
                _logBox
            }
        };
    }

    private void WriteLine
        (
            string format
        )
    {
        Dispatcher.UIThread.InvokeAsync
            (
                () =>
                {
                    _logBox.Text += format + _logBox.NewLine;
                    _logBox.CaretIndex = int.MaxValue;
            })
            .Forget();
    }

    private async Task Run
        (
            Func<Task> function
        )
    {
        await Dispatcher.UIThread.InvokeAsync (() =>
        {
            _button.IsEnabled = false;
            Cursor = new Cursor (StandardCursorType.Wait);
            return _busyStripe.Active = true;
        });
        try
        {
            await function();
        }
        catch (Exception exception)
        {
            WriteLine ($"Exception: {exception}");
        }

        await Dispatcher.UIThread.InvokeAsync (() =>
        {
            Cursor = Cursor.Default;
            _button.IsEnabled = true;
            return _busyStripe.Active = false;
        });
    }

    private async Task<IAsyncProvider> GetProviderAsync()
    {
        var connectionString = ConnectionUtility.GetStandardConnectionString();
        if (string.IsNullOrEmpty (connectionString))
        {
            throw new IrbisException
                (
                    "Connection string not specified!"
                );
        }

        var result = ConnectionFactory.Shared.CreateAsyncConnection();
        result.ParseConnectionString (connectionString);
        await result.ConnectAsync();

        return result;
    }

    private async Task<bool> TestConnectionAsync()
    {
        try
        {
            await using var provider = await GetProviderAsync();
            if (!provider.IsConnected)
            {
                return false;
            }

            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = provider.Database,
                FileName = "ste.mnu"
            };
            _menu = await provider.ReadMenuAsync (specification);
            if (_menu is null || !_menu.Verify (false))
            {
                return false;
            }
        }
        catch (Exception exception)
        {
            WriteLine (exception.Message);
            return false;
        }

        return true;
    }


    private async Task InitializeConnectionAsync()
    {
        this.ShowVersionInfoInTitle();
        _logBox.Output.PrintSystemInformation();

        await Run (async () =>
        {
            var connected = await TestConnectionAsync();
            WriteLine
                (
                    connected
                        ? "Соединение с сервером успешно установлено"
                        : "Невозможно установить соединение с сервером"
                );

            if (connected && _menu is not null)
            {
                _statusBox.Items = _menu.Entries.ToArray();
                if (_menu.Entries.Count != 0)
                {
                    _statusBox.SelectedIndex = 0;
                }

                await Dispatcher.UIThread.InvokeAsync (() => { _button.IsEnabled = true; });
            }
        });
    }

    private async Task ProcessMfnAsync
        (
            IAsyncProvider provider,
            string number,
            int mfn,
            string newStatus
        )
    {
        WriteLine (string.Empty);
        WriteLine ($"MFN={mfn}");

        var recordParameters = new ReadRecordParameters
        {
            Database = provider.EnsureDatabase(),
            Mfn = mfn
        };
        var record = await provider.ReadRecordAsync (recordParameters);
        if (record is null)
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

        var formatParameters = new FormatRecordParameters
        {
            Record = record,
            Format = "@sbrief"
        };
        await provider.FormatRecordsAsync (formatParameters);
        var description = formatParameters.Result.AsSingle();
        if (!string.IsNullOrEmpty (description))
        {
            WriteLine (description);
        }

        record.NotModified();
        foreach (var field in fields)
        {
            _exemplarCount++;
            var oldStatus = field.GetFirstSubFieldValue ('a');
            var inventory = field.GetFirstSubFieldValue ('b');
            var flag = false;
            if (!oldStatus.SameString (newStatus))
            {
                _changeCount++;
                record.MarkAsModified();
                field.SetSubFieldValue ('a', newStatus);
                flag = true;
            }

            var message = flag ? "[" + oldStatus + "]" : "НЕ";
            WriteLine ($"{inventory}: {message} меняем");
        }

        if (record.Modified)
        {
            WriteLine ("Сохраняем запись");
            await provider.WriteRecordAsync (record, dontParse: true);
        }
    }

    private async Task ProcessNumberAsync
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

        await using (var provider = await GetProviderAsync())
        {
            WriteLine ($"КСУ {number}");

            var expression = $"\"NKSU={number}\"";
            var searchParameters = new SearchParameters
            {
                Database = provider.EnsureDatabase(),
                Expression = expression
            };
            var found = await provider.SearchAsync (searchParameters);
            if (found is not null)
            {
                WriteLine ($"Найдено: {found.Length}");

                foreach (var item in found)
                {
                    await ProcessMfnAsync (provider, number, item.Mfn, status);
                }
            }
        }

        WriteLine (new string ('=', 50));
        stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;
        WriteLine ($"Затрачено: {elapsed.ToMinuteString()}");
        WriteLine ($"обработано названий: {_titleCount}");
        WriteLine ($"экземпляров: {_exemplarCount}");
        WriteLine ($"изменён статус: {_changeCount}");
        WriteLine (string.Empty);
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

        await Run (async () => { await ProcessNumberAsync (number, status); });
    }
}
