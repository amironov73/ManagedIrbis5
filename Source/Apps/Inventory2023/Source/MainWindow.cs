// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

using AM;
using AM.Avalonia;
using AM.Collections;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Configuration;

using ReactiveUI;

using static AM.Avalonia.AvaloniaUtility;

#endregion

namespace Inventory2023;

/// <summary>
/// Главное окно приложения.
/// </summary>
public sealed class MainWindow
    : Window
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainWindow()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath (AppContext.BaseDirectory)
            .AddJsonFile ("appsettings.json", optional: false)
            .Build();

        _prefix = "IN=";
        DataContext = _model = new ();
        _connection = ConnectionFactory.Shared.CreateSyncConnection();
        _logBox = new ListBox
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [!ItemsControl.ItemsSourceProperty]
                = new Binding (nameof (_model.Log))
        };

        _okButton = new Button
        {
            IsDefault = true,
            Command = ReactiveCommand.Create (CheckNumber),
            Content = new Image { Source = this.LoadBitmapFromAssets ("Assets/enter.png") }
        };
    }

    #endregion

    #region Window members

    /// <summary>
    /// Вызывается, когда окно проинициализировано фреймворком.
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Title = "Инвентаризация фондов";
        Width = MinWidth = 800;
        Height = MinHeight = 550;

        Styles.Add
            (
                new Style (x => x.Class ("error"))
                {
                    Setters =
                    {
                        new Setter (ForegroundProperty, Brushes.Red)
                    }
                }
            );

        var confirmCommand = ReactiveCommand.Create (ConfirmExemplar);
        var descriptionBox = new TextBox
        {
            Padding = new Thickness (10),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            TextWrapping = TextWrapping.Wrap,
            IsReadOnly = true,
            [!TextBox.TextProperty] = new Binding (nameof (_model.Description))
        };
        descriptionBox.BindClass ("error", new Binding (nameof (_model.HasError)), null!);

        Content = new Grid
        {
            RowDefinitions = RowDefinitions.Parse ("Auto, *, 200, 100"),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Children =
            {
                new StackPanel
                    {
                        Margin = new Thickness (10),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        Spacing = 5,
                        Children =
                        {
                            new Grid
                            {
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                ColumnDefinitions = ColumnDefinitions.Parse ("*, 30, *"),
                                Margin = new Thickness (10),
                                Children =
                                {
                                    VerticalGroup
                                        (
                                            new Label { Content = "Проверяемый фонд" },
                                            new ComboBox
                                            {
                                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                                [!SelectingItemsControl.SelectedItemProperty]
                                                    = new Binding (nameof (_model.CurrentFond)),
                                                [!ItemsControl.ItemsSourceProperty]
                                                    = new Binding (nameof (_model.KnownFonds))
                                            }
                                        ),

                                    VerticalGroup
                                            (
                                                new Label { Content = "Проверяемый экземпляр" },
                                                new TextBox
                                                {
                                                    HorizontalAlignment = HorizontalAlignment.Stretch,
                                                    InnerRightContent = _okButton,
                                                    [!TextBox.TextProperty]
                                                        = new Binding (nameof (_model.CurrentNumber))
                                                }
                                            )
                                        .SetColumn (2)
                                }
                            },

                            new Button
                            {
                                HorizontalAlignment = HorizontalAlignment.Stretch,
                                HorizontalContentAlignment = HorizontalAlignment.Center,
                                Content = "[F2] Я подтверждаю, что экземпляр соответствует библиографическому описанию",
                                Command = confirmCommand,
                            }
                        }
                    }
                    .SetRow (0),

                // библиографическое описание
                descriptionBox.SetRow (1),

                // список обработанных записей
                new ListBox
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        [!ItemsControl.ItemsSourceProperty] = new Binding (nameof (_model.ConfirmedBooks))
                    }
                    .SetRow (2),

                // логи
                _logBox.SetRow (3)
            }
        };

        try
        {
            var connectionString = _configuration["connectionString"]
                .ThrowIfNullOrEmpty ("connectionString not set");
            _connection.ParseConnectionString (connectionString);
            _connection.Connect();
            if (_connection.IsConnected)
            {
                _model.Description = "Ready";
            }
            else
            {
                var errorDescription = IrbisException.GetErrorDescription (_connection.LastError);
                _model.Description = $"Can't connect\n{errorDescription}";
            }

            WriteLog ($"Connected to {_connection.Host}");

            var mhrName = _configuration["mhr"].ThrowIfNullOrEmpty();
            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = _connection.Database,
                FileName = mhrName
            };
            var menu = _connection.RequireMenu (specification);
            var entries = menu.SortEntries (MenuSort.ByCode);
            _model.KnownFonds = new ObservableCollection<MenuEntry> (entries);
        }
        catch (Exception exception)
        {
            _model.Description = exception.ToString();
        }

        KeyBindings.Add (new KeyBinding
        {
            Gesture = new KeyGesture (Key.F2),
            Command = confirmCommand
        });

        DispatcherTimer.Run (IdleOperation, TimeSpan.FromMinutes (1));
    }

    protected override void OnClosing
        (
            WindowClosingEventArgs eventArgs
        )
    {
        _connection.Disconnect();
        WriteLog ("Disconnected");

        base.OnClosing (eventArgs);
    }

    #endregion

    #region Private members

    private readonly InventoryModel _model;
    private readonly ISyncConnection _connection;
    private readonly IConfiguration _configuration;
    private readonly ListBox _logBox;
    private readonly string _prefix;
    private readonly Button _okButton;

    private bool IdleOperation()
    {
        try
        {
            WriteLog ("IDLE");
            _connection.NoOperation();
        }
        catch (Exception exception)
        {
            _model.Description = exception.ToString();
        }

        return true;
    }

    private void CheckNumber()
    {
        var number = _model.CurrentNumber?.Trim();
        _model.HasError = false;
        _model.Description = string.Empty;
        _model.CurrentRecord = null;
        _model.CurrentExemplar = null;

        if (string.IsNullOrEmpty (number))
        {
            return;
        }

        var found = _connection.SearchRead ($"\"{_prefix}{number}\"");
        if (found.IsNullOrEmpty())
        {
            _model.HasError = true;
            _model.Description = $"Не найдено: {number}";
            return;
        }

        if (found.Length != 1)
        {
            _model.HasError = true;
            _model.Description = $"Много записей: {number}";
            return;
        }

        var record = found[0];
        var fields = record.Fields
            .GetField (910)
            .GetField (new[] { 'b', 'h' }, number);

        if (fields.IsNullOrEmpty())
        {
            _model.HasError = true;
            _model.Description = $"Не найдено поле: {number}";
            return;
        }

        if (fields.Length != 1)
        {
            _model.HasError = true;
            _model.Description = $"Много полей: {number}";
            return;
        }

        var exemplar = ExemplarInfo.ParseField (fields[0]);
        exemplar.UserData = fields[0];

        var diagnosis = new StringBuilder();
        if (!number.SameString (exemplar.Number))
        {
            diagnosis.Append ($"{number} -> {exemplar.Number}\n");
        }

        if (!string.IsNullOrEmpty (exemplar.RealPlace))
        {
            _model.HasError = true;
            diagnosis.Append ($"Экземпляр уже был проверен в фонде {exemplar.RealPlace} ({exemplar.CheckedDate})");
        }

        if (exemplar.Status != ExemplarStatus.Free)
        {
            _model.HasError = true;
            diagnosis.Append ($"Неверный статус экземпляра: {exemplar.Status}\n");
        }

        var currentFond = _model.CurrentFond?.Code?.Trim();
        if (string.IsNullOrEmpty (currentFond))
        {
            _model.HasError = true;
            diagnosis.Append ("Не задан проверяемый фонд\n");
            return;
        }

        if (!exemplar.Place.SameString (currentFond))
        {
            _model.HasError = true;
            diagnosis.Append ($"Неверное место хранения экземпляра: {exemplar.Place}\n");
        }

        diagnosis.AppendLine();

        var briefDescription = _connection.FormatRecord ("@brief", record.Mfn)?.Trim();
        if (string.IsNullOrEmpty (briefDescription))
        {
            _model.HasError = true;
            briefDescription = "ОШИБКА при получении библиографического описания";
        }

        diagnosis.Append (briefDescription);
        var diagnosticText = diagnosis.ToString();
        record.UserData = diagnosticText;

        _model.Description = diagnosticText;
        _model.CurrentRecord = record;
        _model.CurrentExemplar = exemplar;
    }

    private void ConfirmExemplar()
    {
        if (_model.HasError)
        {
            return;
        }

        var currentFond = _model.CurrentFond?.Code;
        if (string.IsNullOrEmpty (currentFond))
        {
            WriteLog ("Не задан фонд");
            return;
        }

        var currentRecord = _model.CurrentRecord;
        var currentExemplar = _model.CurrentExemplar;
        if (currentRecord is null || currentExemplar is null)
        {
            return;
        }

        var field = currentExemplar.Field;
        if (field is null)
        {
            return;
        }

        var timestamp = Stopwatch.GetTimestamp();
        var currentDate = IrbisDate.TodayText;
        field.SetSubFieldValue ('!', currentFond);
        field.SetSubFieldValue ('s', currentDate);
        var parameters = new WriteRecordParameters
        {
            Record = currentRecord,
            Actualize = true,
            Lock = false,
            DontParse = true
        };

        try
        {
            _connection.WriteRecord (parameters);
        }
        catch (Exception exception)
        {
            _model.Description = exception.ToString();
            return;
        }

        var number = currentExemplar.Number;
        var description = ((string?) currentRecord.UserData)?.Trim();
        if (string.IsNullOrEmpty (description))
        {
            _model.Description = "Нет библиографического описания";
            return;
        }

        var elapsed = Stopwatch.GetElapsedTime (timestamp);
        WriteLog ($"Подтвержден номер {number}: {description} ({elapsed})");

        var book = new InventoryBookInfo
        {
            Number = number,
            Description = description
        };
        _model.ConfirmedBooks.Add (book);

        _model.CurrentRecord = null;
        _model.CurrentExemplar = null;
    }

    private void WriteLog
        (
            string text
        )
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            _model.Log.Add (text);
            _logBox.ScrollIntoView (_logBox.ItemCount - 1);
        }
    }

    #endregion
}
