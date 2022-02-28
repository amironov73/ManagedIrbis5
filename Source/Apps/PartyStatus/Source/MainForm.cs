// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- главная форма приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace PartyStatus;

/// <summary>
/// Главная форма приложения.
/// </summary>
public partial class MainForm
    : Form
{
    #region Construction

    public MainForm()
    {
        InitializeComponent();
    }

    #endregion

    #region Private members

    private MenuFile? _menu;

    // Количество обработанных названий книг.
    private int _titleCount;

    // Количество обработанных экземпляров.
    private int _exemplarCount;

    // Количество реально измененных экземпляров.
    private int _changeCount;

    private ISyncProvider GetProvider()
    {
        var result = ConnectionUtility.GetConnectionFromConfig();
        result.Connect();

        return result;
    }

    private async Task<bool> TestConnectionAsync()
    {
        var result = false;

        await Run (() =>
        {
            using var provider = GetProvider();
            if (!provider.Connected)
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

    private void WriteLine
        (
            string format
        )
    {
        Sure.NotNull (format);

        var text = format + Environment.NewLine;

        _logBox.InvokeIfRequired (() =>
        {
            var output = (TextBoxOutput)_logBox.Output;
            output.AppendText (text);
        });
    }

    private async Task Run
        (
            Action action
        )
    {
        Sure.NotNull (action);

        try
        {
            _busyStripe.Visible = true;
            _busyStripe.Moving = true;
            await Task.Factory.StartNew (action);
        }
        catch (Exception exception)
        {
            WriteLine ($"Exception: {exception}");
        }
        finally
        {
            _busyStripe.Moving = false;
            _busyStripe.Visible = false;
        }
    }

    private async void MainForm_Load
        (
            object sender,
            EventArgs e
        )
    {
        this.ShowVersionInfoInTitle();
        _logBox.Output.PrintSystemInformation();

        var testResult = await TestConnectionAsync();
        WriteLine
            (
                testResult
                    ? "Соединение с сервером успешно установлено"
                    : "Невозможно установить соединение с сервером"
            );

        if (!ReferenceEquals (_menu, null))
        {
            _statusBox.Items.AddRange (_menu.Entries.ToArray());
            if (_menu.Entries.Count != 0)
            {
                _statusBox.SelectedIndex = 0;
            }
        }
    }

    private void ProcessMfn
        (
            ISyncProvider provider,
            string number,
            int mfn,
            string newStatus
        )
    {
        WriteLine (string.Empty);
        WriteLine ($"MFN={mfn}");

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
            WriteLine (description);
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
            WriteLine ($"{inventory}: {message} меняем");
        }

        if (record.Modified)
        {
            WriteLine ("Сохраняем запись");
            provider.WriteRecord (record);
        }
    }

    private void ProcessNumber
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

        using (var provider = GetProvider())
        {
            WriteLine ($"КСУ {number}");

            var expression = $"\"NKSU={number}\"";
            var found = provider.Search (expression);
            WriteLine ($"Найдено: {found.Length}");

            foreach (var mfn in found)
            {
                ProcessMfn (provider, number, mfn, status);
            }
        }

        WriteLine (new string ('=', 70));
        stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;
        WriteLine ($"Затрачено: {elapsed.ToMinuteString()}");
        WriteLine ($"обработано названий: {_titleCount}");
        WriteLine ($"экземпляров: {_exemplarCount}");
        WriteLine ($"изменён статус: {_changeCount}");
        WriteLine (string.Empty);
    }

    private async void _goButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        var number = _numberBox.Text.Trim();
        if (string.IsNullOrEmpty (number))
        {
            return;
        }

        var entry = (MenuEntry)_statusBox.SelectedItem;
        var status = entry.Code.ThrowIfNull();

        await Run (() => { ProcessNumber (number, status); });
    }

    #endregion
}
