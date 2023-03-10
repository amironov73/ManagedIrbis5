﻿/* MainForm.cs
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using DevExpress.Utils.Taskbar.Core;
using DevExpress.XtraEditors;

using ManagedIrbis.Readers;

using CM=System.Configuration.ConfigurationManager;

#endregion

// ReSharper disable StringLiteralTypo

namespace InventoryControl
{
    /// <summary>
    /// Главная форма приложения
    /// </summary>
    public partial class MainForm
        : XtraForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private DataAccessLevel _dal;

        public ChairInfo CurrentPlace
        {
            get
            {
                ChairInfo result = _dbBox.SelectedItem as ChairInfo;
                return result;
            }
        }

        private void MainForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            AddVersionInfoToTitle();

            _dal = new DataAccessLevel(_logBox.Output);
            ChairInfo[] chairs = _dal.ListPlaces();
            _dbBox.Items.AddRange(chairs);
        }

        private void AddVersionInfoToTitle()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Version vi = assembly.GetName().Version;
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            FileInfo fi = new FileInfo(assembly.Location);
            Text += string.Format
                (
                    ": версия {0} (файл {1}) от {2}",
                    vi,
                    fvi.FileVersion,
                    fi.LastWriteTime.ToShortDateString()
                );
        }

        private void MainForm_FormClosed
            (
                object sender,
                FormClosedEventArgs e
            )
        {
            if (_dal != null)
            {
                _dal.Dispose();
                _dal = null;
            }
        }

        private void _SetProgress
            (
                double percent
            )
        {
            _taskbarAssistant.ProgressCurrentValue
                = (long) (percent * 100.0);
        }

        private void _extractButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ChairInfo place = CurrentPlace;
            if (place is null)
            {
                ShowMessage ("Не выбран фонд");
                return;
            }

            Stopwatch stopwatch = new Stopwatch();
            WriteDelimiter();
            WriteLine ("Извлечение начато");
            stopwatch.Start();
            try
            {
                _taskbarAssistant.ProgressCurrentValue = 0;
                _taskbarAssistant.ProgressMaximumValue = 100;
                _taskbarAssistant.ProgressMode = TaskbarButtonProgressMode.Normal;
                _dal.ExtractExemplars (_SetProgress);
            }
            catch (Exception ex)
            {
                WriteLine
                    (
                        "Ошибка: {0}",
                        ex
                    );
            }
            finally
            {
                _taskbarAssistant.ProgressMode
                    = TaskbarButtonProgressMode.NoProgress;
                TimeSpan elapsed = stopwatch.Elapsed;
                stopwatch.Stop();
                WriteLine("Извлечение завершено");
                WriteLine
                    (
                        "Затрачено времени: {0}",
                        elapsed
                    );
                WriteDelimiter();
            }
        }

        public void WriteDelimiter()
        {
            _logBox.Output.WriteLine
                (
                    new string('=', 60)
                );
        }

        public void WriteLine
            (
                string format,
                params object[] args
            )
        {
            string text = string.Format(format, args);
            _logBox.Output.WriteLine(text);
        }

        private void _dbBox_SelectedIndexChanged
            (
                object sender,
                EventArgs e
            )
        {
            ChairInfo place = CurrentPlace;
            if (place is not null)
            {
                _dal.Place = place.Code;
                string filename = _dal.FixFileName(place.Code);
                _dal.SwitchDatabase(filename);
            }

        }

        public void ShowMessage
            (
                string format,
                params object[] args
            )
        {
            string text = string.Format(format, args);
            XtraMessageBox.Show
                (
                    this,
                    text,
                    "Сообщение",
                    MessageBoxButtons.OK
                );
        }

        private void _createDbButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ChairInfo place = CurrentPlace;
            if (place is null)
            {
                ShowMessage("Не выбран фонд");
            }
            else
            {
                string filename = _dal.FixFileName(place.Code);

                _dal.DropDatabase
                    (
                        filename
                    );
                _dal.CreateDatabase
                    (
                        filename
                    );
                ShowMessage
                    (
                        "Успешно создана база данных '{0}'",
                        filename
                    );
            }
        }

        private void _timer1_Tick
            (
                object sender,
                EventArgs e
            )
        {
            if (_dal != null)
            {
                _dal.Client.NoOp();
                _logBox.Output.WriteLine("NO-OP");
            }
        }

        private void _ShowBooks
            (
                Func<string, string, BookInfo[]> selector,
                string appendix,
                bool sortByTitle,
                bool wrapDescriptions,
                bool numberIndex,
                int startNumber,
                string inventoryPattern,
                bool fromDateChecked,
                DateTime fromDate
            )
        {
            ChairInfo place = CurrentPlace;
            if (place == null)
            {
                ShowMessage("Не выбран фонд");
                return;
            }

            BookInfo[] books = selector(place.Code, inventoryPattern);
            _logBox.Output.WriteLine
                (
                    "Отобрано экземпляров: "
                    + books.Length
                );

            if (!string.IsNullOrEmpty(inventoryPattern))
            {
                Regex regex = new Regex
                    (
                        inventoryPattern,
                        RegexOptions.IgnoreCase
                        | RegexOptions.IgnorePatternWhitespace
                    );

                books = books.Where
                    (
                        book => regex.IsMatch(book.Number)
                    )
                    .ToArray();
                _logBox.Output.WriteLine
                    (
                        "Отфильтровано экземпляров: "
                        + books.Length
                    );
            }

            if (fromDateChecked)
            {
                books = books.Where
                        (
                            book => book.CheckedDate >= fromDate
                        )
                    .ToArray();
            }

            InvReport.ShowModal
                (
                    this,
                    _dal,
                    books,
                    "Фонд " + place.Code + appendix,
                    sortByTitle,
                    wrapDescriptions,
                    numberIndex,
                    startNumber
                );
        }

        private void _showAllButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _ShowBooks
                (
                    (s1, s2) => _dal.GetAllBooks(),
                    ": согласно ИРБИС",
                    _sortByTitleBox.Checked,
                    _wrapDescriptionBox.Checked,
                    _numberIndexBox.Checked,
                    (int)_startNumber.Value,
                    _patternBox.Text,
                    _fromDateCheck.Checked,
                    _fromDatePicker.Value
                );
        }

        private void _showSeenButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _ShowBooks
                (
                    _dal.GetSeenBooks,
                    string.Empty,
                    _sortByTitleBox.Checked,
                    _wrapDescriptionBox.Checked,
                    _numberIndexBox.Checked,
                    (int)_startNumber.Value,
                    _patternBox.Text,
                    _fromDateCheck.Checked,
                    _fromDatePicker.Value
                );
        }

        private void _showMissingButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _ShowBooks
                (
                    _dal.GetMissingBooks,
                    ": недостающие книги",
                    _sortByTitleBox.Checked,
                    _wrapDescriptionBox.Checked,
                    _numberIndexBox.Checked,
                    (int)_startNumber.Value,
                    _patternBox.Text,
                    _fromDateCheck.Checked,
                    _fromDatePicker.Value
                );
        }

        private void _showDefectButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _ShowBooks
                (
                    _dal.GetDefectBooks,
                    ": дефектные описания",
                    _sortByTitleBox.Checked,
                    _wrapDescriptionBox.Checked,
                    _numberIndexBox.Checked,
                    (int)_startNumber.Value,
                    _patternBox.Text,
                    _fromDateCheck.Checked,
                    _fromDatePicker.Value
                );
        }

        private void _nmhrButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ChairInfo place = CurrentPlace;
            if (place is null)
            {
                ShowMessage ("Не выбран фонд");
                return;
            }

            BookInfo[] missingBooks = _dal.GetMissingBooks
                (
                    place.Code,
                    null
                );
            string nmhr = CM.AppSettings["nmhr"];
            _dal.MoveBooks
                (
                    missingBooks,
                    place.Code,
                    nmhr
                );
        }

        private void _dayStatButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            ChairInfo place = CurrentPlace;
            if (place is null)
            {
                ShowMessage("Не выбран фонд");
                return;
            }

            BookInfo[] books = _dal.GetSeenBooks(place.Code, null);
            _dal.StatByDay(books);
        }

        private void _booksOnHandsButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _ShowBooks
                (
                    _dal.GetBooksOnHands,
                    ": книги на руках",
                    _sortByTitleBox.Checked,
                    _wrapDescriptionBox.Checked,
                    _numberIndexBox.Checked,
                    (int)_startNumber.Value,
                    _patternBox.Text,
                    _fromDateCheck.Checked,
                    _fromDatePicker.Value
                );
        }

        private void _SeenPlusHandsButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            _ShowBooks
                (
                    _dal.GetSeenPlusOnHands,
                    ": проверенные плюс документы на руках",
                    _sortByTitleBox.Checked,
                    _wrapDescriptionBox.Checked,
                    _numberIndexBox.Checked,
                    (int)_startNumber.Value,
                    _patternBox.Text,
                    _fromDateCheck.Checked,
                    _fromDatePicker.Value
                );
        }
    }
}
