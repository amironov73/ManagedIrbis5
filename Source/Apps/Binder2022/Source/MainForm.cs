// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* MainForm.cs -- главная форма
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using AM;
using AM.Linq;
using AM.Text;

using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Magazines;

using ComboBox = System.Windows.Forms.ComboBox;

#endregion

#nullable enable

namespace Binder2022;

internal sealed partial class MainForm
    : XtraForm
{
    #region Properties

    public MagazineInfo? CurrentMagazine => _magazineBox.SelectedItem as MagazineInfo;

    public string? CurrentYear => _yearBox.SelectedItem as string;

    public MagazineIssueInfo? CurrentIssue => _numberBox.SelectedItem as MagazineIssueInfo;

    private List<MagazineIssueInfo> CheckedIssues
    {
        get
        {
            var result = new List<MagazineIssueInfo>();
            foreach (MagazineIssueInfo mii in _numberBox.CheckedItems)
            {
                result.Add(mii);
            }

            return result;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainForm
        (
            Program program
        )
    {
        _program = program;
        _master = new BindingMaster(_program);

        InitializeComponent();
    }

    #endregion

    #region Private members

    private readonly Program _program;
    private readonly BindingMaster _master;
    private MagazineManager _manager;
    private List<MagazineInfo> _magazines;
    private MagazineIssueInfo[] _issues;

    private void MainForm_Load
        (
            object sender,
            EventArgs e
        )
    {
        _magazines = _master.GetAllMagazines();
        _magazineBox.DataSource = new BindingList<MagazineInfo>(_magazines);

        var places = _master.GetPlaces();
        _placeBox.Items.AddRange (places);
        _destinationBox.Items.AddRange (places);

        _FillBindingBox();
    }

    private void MainForm_FormClosed
        (
            object sender,
            FormClosedEventArgs e
        )
    {
        //if (_client != null)
        //{
        //    _client.Disconnect();
        //}
    }

    private void _magazineBox_SelectedIndexChanged
        (
            object? sender,
            EventArgs e
        )
    {
        _yearBox.DataSource = null;
        _numberBox.DataSource = null;
        _exemplarBox.DataSource = null;

        var currentMagazine = CurrentMagazine;
        if (currentMagazine is null)
        {
            return;
        }

        _resultBrowser.DocumentText = _master.Connection.FormatRecord("@JW_H", currentMagazine.Mfn);
        _issues = _manager.GetIssues(currentMagazine);

        var years = _issues
            .Select (issue => issue.Year)
            .NonEmptyLines()
            .Distinct()
            .OrderByDescending(item => item)
            .ToArray();
        _yearBox.DataSource = new BindingList<string>(years);
    }

    private void _yearBox_SelectedIndexChanged
        (
            object sender,
            EventArgs e
        )
    {
        _numberBox.DataSource = null;
        _exemplarBox.DataSource = null;
        _complectBox.Items.Clear();
        _allBox.Checked = false;

        var currentYear = CurrentYear;
        if (string.IsNullOrEmpty(currentYear))
        {
            return;
        }

        var issues = _issues
            .Where(issue => issue.Year == currentYear)
            .ToArray();

        Array.Sort (issues, MagazineIssueInfo.CompareNumbers);

        _numberBox.DataSource = new BindingList<MagazineIssueInfo>(issues);

        var complects = issues
            .SelectMany (issue => issue.Exemplars ?? Array.Empty<ExemplarInfo>())
            .Select (issue => issue.Number)
            .NonEmptyLines()
            .Distinct()
            .OrderBy (item => item)
            .ToArray();
        _complectBox.Items.AddRange (complects);
    }

    private void _numberBox_SelectedIndexChanged
        (
            object sender,
            EventArgs e
        )
    {
        _exemplarBox.DataSource = null;

        var current = CurrentIssue;
        if (current == null)
        {
            return;
        }

        var ex = new List<ExemplarInfo> (current.Exemplars ?? Array.Empty<ExemplarInfo>())
            .ToArray();
        Array.Sort (ex, ExemplarInfo.CompareNumbers);
        _exemplarBox.DataSource = new BindingList<ExemplarInfo> (ex);
    }

    public static bool SameString
        (
            string one,
            string two
        )
    {
        return string.Compare
            (
                one,
                two,
                StringComparison.CurrentCultureIgnoreCase
            ) == 0;
    }

    private void _DoBind()
    {
        BindingPlace = _GetSelectedPlace(_placeBox);
        if (string.IsNullOrEmpty(BindingPlace))
        {
            MessageBox.Show("Не задан фонд");
            return;
        }

        BindingComplect = _complectBox.Text.Trim();
        if (string.IsNullOrEmpty(BindingComplect))
        {
            MessageBox.Show("Не задан комплект");
            return;
        }

        BindingNumber = _inventoryBox.Text.Trim();
        if (string.IsNullOrEmpty(BindingNumber))
        {
            MessageBox.Show("Не задан инвентарный номер подшивки");
            return;
        }

        string prefix = CM.AppSettings["prefix"];
        BindingWithPrefix = prefix + BindingNumber;
        if (_client.SearchReadOneRecord
            (
                "\"IN={0}\"",
                BindingWithPrefix
            ) != null)
        {
            MessageBox.Show("Подшивка с указанным номером уже существует");
            return;
        }

        BindingDescription = _descriptionBox.Text.Trim();
        if (string.IsNullOrEmpty(BindingDescription))
        {
            MessageBox.Show("Не задано описание подшивки");
            return;
        }

        if (!_dontCheck.Checked)
        {
            var regex = new Regex(@"^(янв\.|февр\.|март|апр\.|май|июнь|июль|авг\.|сент\.|окт\.|нояб.|дек\.)"
                                  + @"(?:-(янв\.|февр\.|март|апр\.|май|июнь|июль|авг\.|сент\.|окт\.|нояб.|дек\.))?\s"
                                  + @"\(\d+(?:[-,]\d+)*\)$");
            if (!regex.IsMatch(BindingDescription))
            {
                MessageBox.Show("Неверное описание подшивки");
                return;
            }
        }

        BindingRfid = _rfidBox.Text.Trim();
        if (string.IsNullOrEmpty(BindingRfid))
        {
            MessageBox.Show("Не задана RFID-метка подшивки");
            return;
        }

        if (_client.SearchReadOneRecord
            (
                "\"IN={0}\"",
                BindingRfid
            ) != null)
        {
            MessageBox.Show("Метка уже используется");
            return;
        }

        BindingIssues = CheckedIssues;
        if ((BindingIssues == null)
            || (BindingIssues.Length == 0))
        {
            MessageBox.Show("Не выбраны номера для подшивки");
            return;
        }

        //if (BindingIssues.Length < 2)
        //{
        //    MessageBox.Show("Выбрано слишком мало номеров для подшивки");
        //    return;
        //}

        BindingDestination = _GetSelectedPlace(_destinationBox);
        if (string.IsNullOrEmpty(BindingDestination))
        {
            MessageBox.Show("Не задан фонд назначения");
            return;
        }

        if (!BindingIssues.All(_CheckIssue))
        {
            return;
        }

        WriteLog
            (
                "Выбрано номеров: {0}, проверено",
                BindingIssues.Length
            );

        BindingTitle = string.Format
            (
                CM.AppSettings["title"],
                BindingNumber
            );

        BindingIndex = CurrentMagazine.Index
            + "/"
            + CurrentYear
            + "/";
        MagazineIssueInfo firstIssue = BindingIssues[0];
        if (!string.IsNullOrEmpty(firstIssue.Volume))
        {
            BindingIndex += (firstIssue.Volume + "/");
        }
        BindingIndex += (BindingTitle + " " + BindingDescription);

        WriteLog("Подшивка номеров");

        _CreateBinderRecord(firstIssue);

        foreach (MagazineIssueInfo theIssue in BindingIssues)
        {
            _BindIssue
                (
                    theIssue
                );
        }

        _CumulateBinding();

        for (var i = 0; i < _numberBox.Items.Count; i++)
        {
            _numberBox.SetItemChecked(i, false);

        }

        _descriptionBox.Clear();
        _rfidBox.Clear();

        _FillBindingBox();

        _magazineBox_SelectedIndexChanged(this, EventArgs.Empty);

        WriteLog
            (
                new string('=', 60)
            );

        NumberText number = BindingNumber;
        number = number.Increment();
        _inventoryBox.Text = number.ToString();
        _dontCheck.Checked = false;
    }

    private void _bindButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        _DoBind();
    }

    private void _keyBox_TextChanged
        (
            object sender,
            EventArgs e
        )
    {
        _timer1.Enabled = !string.IsNullOrEmpty(_keyBox.Text);
    }

    private void _timer1_Tick
        (
            object sender,
            EventArgs e
        )
    {
        _timer1.Enabled = false;
        var text = _keyBox.Text.Trim();
        for (var i = 0; i < _magazineBox.Items.Count; i++)
        {
            var candidate = _magazineBox.Items[i].ToString();
            if (string.Compare(candidate, text,
                StringComparison.CurrentCultureIgnoreCase)
                >= 0)
            {
                _magazineBox.SelectedIndex = i;
                break;
            }
        }
    }

    public string _GetSelectedPlace
        (
            ComboBox combo
        )
    {
        IrbisMenu.Entry entry = combo.SelectedItem as IrbisMenu.Entry;
        if (entry == null)
        {
            return combo.Text.Trim();
        }
        return entry.Code;
    }

    private bool _FixIssue
        (
            MagazineIssueInfo issue
        )
    {
        var exemplar = issue.Exemplars
            .FirstOrDefault(ex => ex.Number == BindingComplect);
        if (exemplar == null)
        {
            exemplar = new ExemplarInfo
            {
                Status = "0",
                Number = BindingComplect,
                Date = "?",
                Place = BindingPlace,
                BindingIndex = null,
                BindingNumber = null
            };
            issue.Exemplars = new List<ExemplarInfo>(issue.Exemplars)
                {
                    exemplar
                }
            .ToArray();
            WriteLog
                (
                    "Номер {0}, создан экземпляр: {1}",
                    issue,
                    exemplar
                );
        }
        else
        {
            if (SameString(exemplar.Status, "p"))
            {
                throw new ApplicationException
                    (
                        string.Format
                        (
                            "Экземпляр {0} в номере {1} уже подшит!",
                            exemplar,
                            issue
                        )
                    );
            }

            if (SameString(exemplar.Place, "Ф403"))
            {
                throw new ApplicationException
                    (
                        string.Format
                        (
                            "Экземпляр {0} в номере {1} принадлежит КПИО!",
                            exemplar, issue
                        )
                    );
            }

            exemplar.Status = "0";
            exemplar.Place = BindingPlace;
            exemplar.BindingIndex = null;
            exemplar.BindingNumber = null;
            WriteLog
                (
                    "Номер {0}, исправлен экземпляр: {1}",
                    issue,
                    exemplar
                );
        }
        return true;
    }

    private void _fixButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        BindingPlace = _GetSelectedPlace(_placeBox);
        if (string.IsNullOrEmpty(BindingPlace))
        {
            MessageBox.Show("Не задан фонд");
            return;
        }

        BindingComplect = _complectBox.Text.Trim();
        if (string.IsNullOrEmpty(BindingComplect))
        {
            MessageBox.Show("Не задан комплект");
            return;
        }

        BindingIssues = CheckedIssues;
        if ((BindingIssues == null)
            || (BindingIssues.Length == 0))
        {
            MessageBox.Show("Не выбраны номера для подшивки");
            return;
        }

        WriteLog("Исправление сведений об экземплярах");

        try
        {
            foreach (MagazineIssueInfo issue in BindingIssues)
            {
                if (!_FixIssue(issue))
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }

        WriteLog(new string('-', 60));
    }

    private void _StartIrbis
        (
            string mfn
        )
    {
        string sourceIni = CM.AppSettings["source-ini"];
        string targetIni = CM.AppSettings["target-ini"];
        string exePath = CM.AppSettings["exe-path"];
        string exeArgument = CM.AppSettings["exe-argument"];
        string workingDirectory = CM.AppSettings["working-directory"];

        var encoding = Encoding.Default;

        var iniText = File.ReadAllText(sourceIni, encoding)
                      + Environment.NewLine
                      + "CURMFN="
                      + mfn
                      + Environment.NewLine;
        File.WriteAllText(targetIni, iniText, encoding);
        var startInfo = new ProcessStartInfo
            (
                exePath,
                exeArgument
            )
        {
            WorkingDirectory = workingDirectory,
            UseShellExecute = false
        };
        Process.Start(startInfo);
    }

    private void _editButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        var issue = CurrentIssue;
        if (issue != null)
        {
            _StartIrbis
                (
                    issue.Mfn.ToString(CultureInfo.InvariantCulture)
                );
        }
    }

    private void _CreateNumber
        (
            string year,
            string number
        )
    {
        var index = string.Format
            (
                "{0}/{1}/{2}",
                CurrentMagazine.Index,
                year,
                number
            );
        if (_client.SearchReadOneRecord
            (
                "\"I={0}\"",
                index
            ) != null)
        {
            WriteLog
                (
                    "Номер {0}/{1} уже есть, не создаём",
                    year,
                    number
                );
            return;
        }

        IrbisRecord record = new IrbisRecord();
        record.AddField("933", CurrentMagazine.Index);
        record.AddField
            (
                "903",
                index
            );
        record.AddField("934", year);
        record.AddField("936", number);
        RecordField field = new RecordField("910");
        field.AddSubField('A', "0");
        field.AddSubField('B', BindingComplect);
        field.AddSubField('C', "?");
        field.AddSubField('D', BindingPlace);
        record.Fields.Add(field);
        record.AddField("920", "NJ");
        record.AddField("999", "0000000");
        _client.WriteRecord(record, false, true);
        WriteLog
            (
                "Создан номер: {0}/{1}",
                year,
                number
            );
    }

    private void _addButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        BindingPlace = _GetSelectedPlace(_placeBox);
        if (string.IsNullOrEmpty(BindingPlace))
        {
            MessageBox.Show("Не задан фонд");
            return;
        }

        BindingComplect = _complectBox.Text.Trim();
        if (string.IsNullOrEmpty(BindingComplect))
        {
            MessageBox.Show("Не задан номер комплекта");
            return;
        }

        WriteLog("Добавление номеров");
        var text = _addBox.Text.Trim();
        var year = CurrentYear;
        if (text.Contains(':'))
        {
            var parts = text.Split(new[] { ':' }, 2);
            year = parts[0];
            text = parts[1].Trim();
        }

        var numbers = NumberText
            .ParseRanges(text)
            .Select(_ => _.ToString())
            .ToArray();
        foreach (var number in numbers)
        {
            _CreateNumber
                (
                    year,
                    number
                );
        }

        WriteLog(new string('=', 60));

        _FillBindingBox();

        _magazineBox_SelectedIndexChanged(this, EventArgs.Empty);
    }

    public void WriteLog
        (
            string text
        )
    {
        text += Environment.NewLine;
        _logBox.AppendText(text);
        _logBox.SelectionStart = _logBox.TextLength;
        _logBox.SelectionLength = 0;
        _logBox.ScrollToCaret();
        Application.DoEvents();
    }

    private void _editMainButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        _StartIrbis
            (
                CurrentMagazine.Mfn.ToString(CultureInfo.InvariantCulture)
            );
    }

    private void _allBox_CheckedChanged
        (
            object sender,
            EventArgs e
        )
    {
        var flag = _allBox.Checked;
        var max = _numberBox.Items.Count;
        for (var i = 0; i < max; i++)
        {
            var issue = _numberBox.Items[i] as MagazineIssueInfo;
            if (issue != null)
            {
                if (issue.Number.Contains("Подшивка"))
                {
                    continue;
                }
            }
            _numberBox.SetItemChecked(i, flag);
        }
    }

    private void _unbindButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        var issue = CurrentIssue;
        if (issue == null)
        {
            return;
        }
        var mainRecord = _client.ReadRecord(issue.Mfn);
        string worksheet = mainRecord.FM("920");
        if (worksheet != "NJK")
        {
            MessageBox.Show("Это не подшивка");
            return;
        }
        var documentCode = issue.DocumentCode;
        var allBound = _client.SearchRead
            (
                "\"II={0}\"",
                documentCode
            );
        foreach (var boundRecord in allBound)
        {
            var field = boundRecord.Fields
                .GetField("910")
                .GetField('p', documentCode)
                .FirstOrDefault();
            if (field != null)
            {
                field.SetSubField('A', "0");
                field.RemoveSubField('P');
                field.RemoveSubField('I');
            }

            field = boundRecord.Fields
                .GetField("463")
                .GetField('w', documentCode)
                .FirstOrDefault();
            if (field != null)
            {
                boundRecord.Fields.Remove(field);
            }

            var otherBound = boundRecord.Fields
                .GetField("901")
                .GetField('A', "p");
            if (otherBound.Length == 0)
            {
                boundRecord.SetField("920", "NJ");
                boundRecord.RemoveField("463");
            }

            _client.WriteRecord(boundRecord, false, true);
            WriteLog
                (
                    "Расшит номер: {0}",
                    boundRecord.FM("903")
                );

            //IrbisRecord magazineRecord = _client.ReadRecord(CurrentMagazine.Mfn);
            //_client.WriteRecord(magazineRecord, false, true);
        }

        _client.DeleteRecord(mainRecord, true);
        WriteLog("Удалена запись: {0}", documentCode);
        WriteLog(new string('=', 60));

        _FillBindingBox();

        _magazineBox_SelectedIndexChanged(this, EventArgs.Empty);
    }

    private void _updateButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        _FillBindingBox();

        _magazineBox_SelectedIndexChanged(this, EventArgs.Empty);
    }

    private void _FillBindingBox()
    {
        string prefix = CM.AppSettings["prefix"];
        _bindingBox.Items.Clear();
        string[] bindingList = _client.GetSearchEntries
            (
                string.Format
                (
                    "IN={0}",
                    prefix
                )
            );

        bindingList = bindingList
            .Select(_ => prefix + _)
            .ToArray();

        Array.Sort
            (
                bindingList,
                NumberText.Compare
            );
        _bindingBox.Items.AddRange(bindingList);
    }

    private void _removeDublesButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        BindingComplect = _complectBox.Text.Trim();
        if (string.IsNullOrEmpty(BindingComplect))
        {
            MessageBox.Show("Не задан номер комплекта");
            return;
        }

        WriteLog("Удаление повторов комплектов");

        var was = false;

        MagazineIssueInfo[] selected = CheckedIssues;
        foreach (var issue in selected)
        {
            var record = _client.ReadRecord(issue.Mfn);
            var fields = record.Fields
                .GetField("910")
                .GetField('B', BindingComplect);
            if (fields.Length > 1)
            {
                for (var i = 1; i < fields.Length; i++)
                {
                    record.Fields.Remove(fields[i]);
                }
                WriteLog
                    (
                        "Выпуск {0}: удалено повторов: {1}",
                        issue,
                        fields.Length - 1
                    );
                _client.WriteRecord(record, false, true);
                was = true;
            }
            else
            {
                WriteLog("Выпуск {0}: нет повторов", issue);
            }
        }

        WriteLog(new string('=', 60));

        if (was)
        {
            _updateButton_Click(this, EventArgs.Empty);
        }
    }

    private void _printButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        //BindingDescription = _descriptionBox.Text.Trim();
        //if (string.IsNullOrEmpty(BindingDescription))
        //{
        //    MessageBox.Show("Не задано описание подшивки");
        //    return;
        //}

        //Regex regex = new Regex(@"^(?<month>(янв\.|февр\.|март|апр\.|май|июнь|июль|авг\.|сент\.|окт\.|нояб.|дек\.)"
        //    + @"(?:-(янв\.|февр\.|март|апр\.|май|июнь|июль|авг\.|сент\.|окт\.|нояб.|дек\.))?)\s"
        //    + @"\((?<number>\d+(?:[-,]\d+)*)\)$");
        //Match match = regex.Match(BindingDescription);
        //if (!match.Success)
        //{
        //    if (!_dontCheck.Checked)
        //    {
        //        MessageBox.Show("Неверное описание подшивки");
        //        return;
        //    }
        //}


        //string months = match.Groups["month"]
        //    .Value
        //    .Replace("янв.", "январь")
        //    .Replace("февр.", "февраль")
        //    .Replace("апр.", "апрель")
        //    .Replace("авг.", "август")
        //    .Replace("сент.", "сентябрь")
        //    .Replace("окт.", "октябрь")
        //    .Replace("нояб.", "ноябрь")
        //    .Replace("дек.", "декабрь")
        //    .Replace("-", " – ");

        //PreacherInfo preacher = new PreacherInfo
        //{
        //    Title = CurrentMagazine.Title,
        //    Year = CurrentYear,
        //    Months = months,
        //    Numbers = match.Groups["number"].Value
        //};

        //PreacherReport report = new PreacherReport
        //{
        //    DataSource = new BindingSource(preacher, null)
        //};

        //ReportPrintTool tool = new ReportPrintTool(report);
        //tool.ShowPreviewDialog(this, null);
    }

    private void _numberBox_ItemCheck
        (
            object sender,
            ItemCheckEventArgs e
        )
    {
        _timer2.Enabled = false;

        _timer2.Enabled = true;
    }

    private static string _FromChunkedText
        (
            NumberText number,
            bool first
        )
    {
        if (first)
        {
            return number.GetValue(0).ToString(CultureInfo.InvariantCulture);
        }
        var pos = number.Length - 1;
        return number.GetValue(pos).ToString(CultureInfo.InvariantCulture);
    }

    private void _timer2_Tick(object sender, EventArgs e)
    {
        _timer2.Enabled = false;

        _descriptionBox.Clear();

        MagazineIssueInfo[] issues = CheckedIssues;
        if (issues.Length == 0)
        {
            return;
        }

        var months = new List<int>();
        string[] monthNames1 =
        {
                "янв", "фев", "мар", "апр", "май", "июн",
                "июл", "авг", "сен", "окт", "ноя", "дек"
            };
        string[] monthNames2 =
        {
                "янв.", "февр.", "март", "апр.",
                "май", "июнь", "июль", "авг.",
                "сент.", "окт.", "нояб.", "дек."
            };

        foreach (var issue in issues)
        {
            if (!string.IsNullOrEmpty(issue.Supplement))
            {
                for (var i = 0; i < monthNames1.Length; i++)
                {
                    if (issue.Supplement.Contains(monthNames1[i]))
                    {
                        months.Add(i);
                        break;
                    }
                }
                if (issue.Supplement.Contains("мая"))
                {
                    months.Add(4);
                }
            }
        }

        var number = issues
            .Select
            (
                _ => new NumberText(_.Number)
            ).ToArray();

        if ((number.Length == 0)
            || (months.Count == 0))
        {
            return;
        }

        var minIssue = number.Min();
        var maxIssue = number.Max();
        var minMonth = monthNames2[months.Min()];
        var maxMonth = monthNames2[months.Max()];

        var result = new StringBuilder();

        if (minMonth == maxMonth)
        {
            result.Append
                (
                    minMonth
                );
        }
        else
        {
            result.AppendFormat
                (
                    "{0}-{1}",
                    minMonth,
                    maxMonth
                );
        }

        result.Append(" ");

        if (minIssue == maxIssue)
        {
            result.AppendFormat
                (
                    "({0})",
                    _FromChunkedText(minIssue, true)
                );
        }
        else
        {
            result.AppendFormat
                (
                    "({0}-{1})",
                    _FromChunkedText(minIssue, true),
                    _FromChunkedText(maxIssue, false)
                );
        }

        _descriptionBox.Text = result.ToString();
    }

    private void _idleTimer_Tick
        (
            object sender,
            EventArgs e
        )
    {
        if ((_client != null)
            && (_client.Connected))
        {
            try
            {
                _client.NoOp();
                WriteLog("NO-OP");
            }
            catch
            {
                _idleTimer.Enabled = false;
            }
        }
    }

    private void _deleteComplectButton_Click
        (
            object sender,
            EventArgs e
        )
    {
        BindingComplect = _complectBox.Text.Trim();
        if (string.IsNullOrEmpty(BindingComplect))
        {
            MessageBox.Show("Не задан номер комплекта");
            return;
        }

        WriteLog("Удаление комплекта номер " + BindingComplect);
        var was = false;

        MagazineIssueInfo[] selected = CheckedIssues;
        foreach (var issue in selected)
        {
            var record = _client.ReadRecord(issue.Mfn);
            var fields = record.Fields
                .GetField("910")
                .GetField('B', BindingComplect);
            if (fields.Length != 0)
            {
                for (var i = 0; i < fields.Length; i++)
                {
                    record.Fields.Remove(fields[i]);
                }
                WriteLog
                    (
                        "Выпуск {0}: удалено: {1}",
                        issue,
                        fields.Length
                    );
                _client.WriteRecord(record, false, true);
                was = true;
            }
        }

        WriteLog(new string('=', 60));

        if (was)
        {
            _updateButton_Click(this, EventArgs.Empty);
        }
    }

    private void _printButton2_Click
        (
            object sender,
            EventArgs e
        )
    {
        //MagazineIssueInfo issue = CurrentIssue;
        //if (issue == null)
        //{
        //    return;
        //}
        //IrbisRecord mainRecord = _client.ReadRecord(issue.Mfn);
        //string worksheet = mainRecord.FM("920");
        //if (worksheet != "NJK")
        //{
        //    MessageBox.Show("Это не подшивка");
        //    return;
        //}

        //string bindingDescription = mainRecord.FM("936");
        //if (string.IsNullOrEmpty(bindingDescription))
        //{
        //    MessageBox.Show("Не задано описание подшивки");
        //    return;
        //}

        //Regex regex = new Regex(@"(?<month>(янв\.|февр\.|март|апр\.|май|июнь|июль|авг\.|сент\.|окт\.|нояб.|дек\.)"
        //    + @"(?:-(янв\.|февр\.|март|апр\.|май|июнь|июль|авг\.|сент\.|окт\.|нояб.|дек\.))?)\s*"
        //    + @"(?:\((?<number>\d+(?:[-,]\d+)*)\))?$");
        //Match match = regex.Match(bindingDescription);
        //if (!match.Success)
        //{
        //    MessageBox.Show("Неверное описание подшивки");
        //    return;
        //}

        //string months = match.Groups["month"]
        //    .Value
        //    .Replace("янв.", "январь")
        //    .Replace("февр.", "февраль")
        //    .Replace("апр.", "апрель")
        //    .Replace("авг.", "август")
        //    .Replace("сент.", "сентябрь")
        //    .Replace("окт.", "октябрь")
        //    .Replace("нояб.", "ноябрь")
        //    .Replace("дек.", "декабрь")
        //    .Replace("-", " – ");

        //PreacherInfo preacher = new PreacherInfo
        //{
        //    Title = CurrentMagazine.Title,
        //    Year = CurrentYear,
        //    Months = months,
        //    Numbers = match.Groups["number"].Value
        //};

        //PreacherReport report = new PreacherReport
        //{
        //    DataSource = new BindingSource(preacher, null)
        //};

        //ReportPrintTool tool = new ReportPrintTool(report);
        //tool.ShowPreviewDialog(this, null);

    }

    #endregion
}
