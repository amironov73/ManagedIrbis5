// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* BindingMaster.cs -- умеет работать с журналами и подшивками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using AM;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.CommandLine;
using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Magazines;
using ManagedIrbis.Menus;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion

#pragma warning disable CS8618 // non-nullable members not initialized in constructor

#nullable enable

namespace Binder2022;

/// <summary>
/// Умеет работать с журналами и подшивками.
/// </summary>
internal sealed class BindingMaster
{
    #region Private members

    /// <summary>
    /// Активное подключение.
    /// </summary>
    public SyncConnection Connection => (SyncConnection) _program.Connection!;

    /// <summary>
    /// Менеджер журналов.
    /// </summary>
    public MagazineManager Manager { get; }

    public string BindingPlace { get; set; }
    public string BindingComplect { get; set; }
    public string BindingNumber { get; set; }
    public string BindingTitle { get; set; }
    public string BindingDescription { get; set; }
    public string BindingWithPrefix { get; set; }
    public string BindingRfid { get; set; }
    public MagazineIssueInfo[] BindingIssues { get; set; }
    public string BindingIndex { get; set; }
    public string BindingDestination { get; set; }


    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BindingMaster
        (
            Program program
        )
    {
        _program = program;
        Manager = new MagazineManager (_program.Connection.ThrowIfNull());
    }

    #endregion

    #region Private members

    private readonly Program _program;

    #endregion

    #region Public methods

    /// <summary>
    /// Получение списка всех журналов в каталоге.
    /// </summary>
    public List<MagazineInfo> GetAllMagazines()
    {
        return Manager
            .GetAllMagazines()
            .OrderBy(_ => _.Title)
            .ToList();
    }

    /// <summary>
    /// Получение перечня мест хранения.
    /// </summary>
    public MenuEntry[] GetPlaces()
    {
        var specification = new FileSpecification
        {
            Path = IrbisPath.MasterFile,
            Database = Connection.EnsureDatabase(),
            FileName = "mhr.mnu"
        };

        return Connection.ReadMenu (specification)
            .ThrowIfNull()
            .SortEntries (MenuSort.ByCode);
    }

    public bool _CheckIssue
        (
            MagazineIssueInfo issue
        )
    {
        var worksheet = issue.Worksheet;
        if (!worksheet.IsOneOf ("NJ", "NJP"))
        {
            MessageBox.Show
                (
                    "В номере "
                    + issue.Number
                    + " рабочий лист "
                    + issue.Worksheet
                );
            return false;
        }

        var infos = issue.Exemplars
            .Where(ex => ex.Number == BindingComplect)
            .ToArray();
        if (infos.Length == 0)
        {
            MessageBox.Show
                (
                    "В номере "
                    + issue.Number
                    + " отсутствует комплект "
                    + BindingComplect
                );
            return false;
        }

        if (infos.Length != 1)
        {
            MessageBox.Show
                (
                    "В номере "
                    + issue.Number
                    + " больше одного комплекта "
                    + BindingComplect
                );
            return false;
        }

        var complect = infos[0];
        if (complect.Place.SameString ("Ф403"))
        {
            MessageBox.Show
                (
                    "В номере "
                    + issue.Number
                    + " попытка подшить экземпляр КПИО"
                );
            return false;
        }

        if (!complect.Place.SameString (BindingPlace))
        {
            MessageBox.Show
                (
                    "В номере "
                    + issue.Number
                    + " комплект "
                    + BindingComplect
                    + " имеет сиглу '"
                    + complect.Place
                    + "'"
                );
            return false;
        }

        if (!complect.Status.SameString ("0"))
        {
            MessageBox.Show
                (
                    "В номере "
                    + issue.Number
                    + " комплект "
                    + BindingComplect
                    + " имеет статус '"
                    + complect.Status
                    + "'"
                );
            return false;
        }

        return true;
    }

    public void BindIssue
        (
            MagazineIssueInfo issue
        )
    {
        var exemplars = issue.Exemplars ?? Array.Empty<ExemplarInfo>();
        var exemplar = exemplars.First (ex => ex.Number == BindingComplect);
        var index = Array.IndexOf (exemplars, exemplar);

        exemplar.Status = "p";
        exemplar.BindingIndex = BindingIndex;
        exemplar.BindingNumber = BindingWithPrefix;

        var record = Connection.ReadRecord (issue.Mfn);
        if (record is null)
        {
            MessageBox.Show ("Фигня");
            return;
        }

        Field field;
        try
        {
            field = record.Fields.GetField(910)[index];
        }
        catch (IndexOutOfRangeException)
        {
            field = new Field(910);
            record.Fields.Add(field);
        }

        field.Subfields.Clear();
        field.Subfields.AddRange(exemplar.ToField().Subfields);
        field.SetSubFieldValue('d', BindingDestination);
        record.SetValue (920, "NJP");

        //bool found = false;
        //string v903 = record.FM("903");
        //foreach (RecordField v463 in record.Fields.GetField("463"))
        //{
        //    if (SameString(v463.GetFirstSubFieldText('w'), v903))
        //    {
        //        found = true;
        //        break;
        //    }
        //}
        //if (!found)
        //{
        //    record.AddField("463", 'w', v903);
        //}

        record.Add (463, 'w', BindingIndex);
        Connection.WriteRecord(record, false, true);
        WriteLog ($"Подшит номер: {issue}, экземпляр: {exemplar}");
    }

    public void WriteLog (string text)
    {
        throw new NotImplementedException();
    }

    public void CreateBinderRecord
        (
            MagazineIssueInfo firstIssue
        )
    {
        var regex = new Regex(@"\((.+)\)");
        var match = regex.Match(BindingDescription);

        var record = new Record();
        record.Add (933, CurrentMagazine.Index);
        record.Add (903, BindingIndex);
        record.Add (934, CurrentYear);
        if (!string.IsNullOrEmpty(firstIssue.Volume))
        {
            record.Add (935, firstIssue.Volume);
        }
        record.Add (936, BindingTitle + " " + BindingDescription);
        if (match.Success)
        {
            record.Add (931, match.Groups[1].Value);
        }
        var field = new Field(910);
        field.Add('A', "0");
        field.Add('B', BindingWithPrefix);
        field.Add('C', DateTime.Now.ToString("yyyyMMdd"));
        field.Add('D', BindingDestination);
        field.Add('H', BindingRfid);
        record.Fields.Add(field);
        record.Add (920, "NJK");
        record.Add (999, "0000000");

        Connection.WriteRecord(record, false, true);
        WriteLog ($"Создана запись подшивки: {BindingIndex}");
    }

    public void CumulateBinding()
    {
        var newField = false;

        var record = Connection.ReadRecord (CurrentMagazine.Mfn);
        var yearField = record.Fields
            .GetField("909")
            .GetField('q', CurrentYear)
            .GetField('d', BindingDestination)
            .GetField('k', BindingComplect)
            .FirstOrDefault();
        if (yearField == null)
        {
            newField = true;
            yearField = new Field (909)
                .Add ('q', CurrentYear)
                .Add ('d', BindingDestination)
                .Add ('k', BindingComplect);
            record.Fields.Add(yearField);
        }
        var text = yearField.GetFirstSubFieldText('h') ?? string.Empty;
        if (!string.IsNullOrEmpty(text))
        {
            text += ",";
        }
        text = text + BindingTitle + " " + BindingDescription;
        yearField.SetSubField('h', text);
        Connection.WriteRecord(record, false, true);

        WriteLog ($"Произведена кумуляция, новое поле: {newField}");
    }



    #endregion
}
