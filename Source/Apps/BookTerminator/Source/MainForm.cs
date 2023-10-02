// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MainForm.cs -- главная форма приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AM;
using AM.Text.Output;
using AM.Windows.Forms;

using ManagedIrbis;

using Microsoft.Extensions.Configuration;

#endregion

#nullable enable

namespace BookTerminator;

/// <summary>
/// Главная форма приложения.
/// </summary>
public sealed partial class MainForm
    : Form
{
    #region Properties

    /// <summary>
    /// Подключение.
    /// </summary>
    public AsyncConnection Connection { get; }

    /// <summary>
    /// Output.
    /// </summary>
    public AbstractOutput Output => _logBox.Output;

    /// <summary>
    /// Текущий MFN.
    /// </summary>
    public int CurrentMfn { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainForm
        (
            IConfiguration configuration
        )
    {
        _configuration = configuration;

        InitializeComponent();

        Connection = new AsyncConnection();
        _busyStripe.SubscribeTo (Connection.Busy);

        // Через дизайнер добавить WebBrowser
        // невозможно - якобы несовместимо,
        // приходится создавать его явно.
        // WebBrowser обернут в панель, потому что
        // сам по себе он неверно реализует Dock = Fill

        _browser = new WebBrowser()
        {
            Dock = DockStyle.Fill
        };
        _dummyPanel.Controls.Add (_browser);

        // SetHtml("<p>Введите MFN записи и нажмите <tt>Enter</tt>!</p>");
    }

    #endregion

    #region Private members

    private readonly IConfiguration _configuration;
    private readonly WebBrowser _browser;

    private void WriteLine
        (
            string format,
            params object[] args
        )
    {
        Output.WriteLine (format, args);
    }

    private void WaitForBrowser()
    {
        Application.DoEvents();
        while (_browser.IsBusy)
        {
            Application.DoEvents();
            Thread.Sleep (20);
        }

        Application.DoEvents();
    }

    private void SetHtml
        (
            string text
        )
    {
        if (_browser.Document == null)
        {
            _browser.Navigate ("about:blank");
            WaitForBrowser();
        }

        // One more time
        if (_browser.Document == null)
        {
            _browser.Navigate ("about:blank");
            WaitForBrowser();
        }

        _browser.DocumentText = text;
        WaitForBrowser();
    }

    private async void Form_Load
        (
            object sender, EventArgs e
        )
    {
        this.ShowVersionInfoInTitle();
        Output.PrintSystemInformation();

        var connectionString = _configuration["irbis-connection"];
        Connection.ParseConnectionString (connectionString);
        await Connection.ConnectAsync();

        if (!Connection.IsConnected)
        {
            await ShowErrorHtml ("Невозможно подключиться к серверу ИРБИС64");
            WriteLine ("Can't connect");
            return;
        }

        await ShowEmbeddedHtml ("Intro.html");
        _mfnBox.Focus();
    }

    private async Task ShowErrorHtml
        (
            string errorText
        )
    {
        var additionalText = "<pre>" + errorText + "</pre>";

        await ShowEmbeddedHtml
            (
                "Error.html",
                additionalText
            );
    }

    private async Task ShowEmbeddedHtml
        (
            string fileName,
            string? additionalText = null
        )
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = nameof (BookTerminator) + "." + fileName;
        await using var stream = assembly.GetManifestResourceStream (resourceName);
        if (stream is null)
        {
            SetHtml ($"Ошибка: отсутствует ресурс '{resourceName}'");
        }
        else
        {
            using var reader = new StreamReader (stream);
            var introText = await reader.ReadToEndAsync();
            if (additionalText is not null)
            {
                introText += additionalText;
            }

            SetHtml (introText);
        }
    }

    private async Task<string> FormatRecord
        (
            int mfn
        )
    {
        var format = _configuration["format"];
        if (string.IsNullOrEmpty (format))
        {
            return "(no format configured)";
        }

        var formatted = await Connection.FormatRecordAsync
            (
                format,
                mfn
            );

        var record = await Connection.ReadRecordAsync (mfn);
        if (record is null)
        {
            return "<strong>MISSING RECORD</strong><br/>"
                   + formatted;
        }

        if (record.Deleted)
        {
            return "<strong>DELETED</strong><br/>"
                   + formatted;
        }

        return formatted ?? "(null)";
    }

    /// <summary>
    /// Нажата кнопка "Перейти".
    /// </summary>
    private async void _goButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        SetHtml (string.Empty);

        var mfnText = _mfnBox.Text.Trim();
        if (string.IsNullOrEmpty (mfnText))
        {
            return;
        }

        var mfn = mfnText.SafeToInt32();
        if (mfn <= 0)
        {
            WriteLine ("Bad MFN: {0}", mfnText);
            return;
        }

        try
        {
            var formatted = await FormatRecord (mfn);
            SetHtml (formatted);
            WriteLine ("MFN: {0}", mfn);
            CurrentMfn = mfn;
        }
        catch (Exception ex)
        {
            WriteLine ("Searching for MFN: {0}", mfn);
            WriteLine ("Exception: {0}", ex);
        }
    }

    /// <summary>
    /// Нажата кнопка "Удалить".
    /// </summary>
    private async void _deleteButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        if (CurrentMfn != 0)
        {
            try
            {
                WriteLine
                    (
                        "Delete MFN: {0}",
                        CurrentMfn
                    );

                await Connection.NoOperationAsync();

                // await Connection.DeleteRecordAsync(CurrentMfn);
            }
            catch (Exception ex)
            {
                WriteLine ("Deleting MFN: {0}", CurrentMfn);
                WriteLine ("Exception: {0}", ex);
            }
        }

        _mfnBox.Clear();
        CurrentMfn = 0;
        SetHtml (string.Empty);
        _mfnBox.Focus();
    }

    /// <summary>
    /// Перед закрытием формы.
    /// </summary>
    private async void _FormClosing
        (
            object sender,
            FormClosingEventArgs e
        )
    {
        await Connection.DisconnectAsync();
        Console.WriteLine ("Disconnected");
        _busyStripe.UnsubscribeFrom (Connection.Busy);
    }

    /// <summary>
    /// Нажатие клавиш в строке ввода MFN.
    /// </summary>
    private void _mfnBox_KeyDown
        (
            object sender,
            KeyEventArgs e
        )
    {
        if (e.KeyData == Keys.Enter)
        {
            _goButton_Click (sender, e);
            e.Handled = true;
        }
        else if (e.KeyData == Keys.F2)
        {
            _deleteButton_Click (sender, e);
            e.Handled = true;
        }
    }

    /// <summary>
    /// Срабатывание таймера для отсылки NOP на сервер.
    /// </summary>
    private async void _idleTimer_Tick
        (
            object sender,
            EventArgs e
        )
    {
        if (Connection.IsConnected)
        {
            await Connection.NoOperationAsync();
            WriteLine ("NOP");
        }
    }

    #endregion
}
