// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Text.Ranges;
using AM.Windows.DevEx;

using DevExpress.Spreadsheet;

using ManagedIrbis;
using ManagedIrbis.Magazines;

#endregion

#nullable enable

namespace KpioRegistry;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static int Main
        (
            string[] args
        )
    {
        args.NotUsed();

        const string connectionString = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=IBIS;";
        using var connection = ConnectionFactory.Shared.CreateSyncConnection();
        connection.ParseConnectionString (connectionString);
        connection.Connect();
        if (!connection.Connected)
        {
            Console.WriteLine (IrbisException.GetErrorDescription (connection.LastError));
            return 1;
        }

        var manager = new MagazineManager (connection);
        var newspapers = manager.GetAllMagazines ("V=01");
        // newspapers = newspapers.Take (50).ToArray();
        Console.WriteLine ($"Всего названий газет: {newspapers.Length}");
        newspapers = newspapers.OrderBy (n => n.Title!.Trim ('"')).ToArray();

        var allIssues = new List<MagazineIssueInfo>();
        foreach (var newspaper in newspapers)
        {
            var issues = manager.GetIssues (newspaper);
            issues = issues.Where (i => HavePlace (i, "Ф403")).ToArray();
            foreach (var issue in issues)
            {
                issue.UserData = newspaper;
            }
            allIssues.AddRange (issues);
            Console.WriteLine ($"{newspaper.Title}: вып. {issues.Length}");
        }

        using var excel = new EasyExcel();
        excel.NewLine();

        var column = excel.Worksheet.Columns[0];
        column.WidthInCharacters = 20;
        column.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;

        column = excel.Worksheet.Columns[1];
        column.WidthInCharacters = 10;
        column.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;

        column = excel.Worksheet.Columns[2];
        column.WidthInCharacters = 50;
        column.Alignment.Vertical = SpreadsheetVerticalAlignment.Top;

        var years = allIssues.Select (i => i.Year.SafeToInt32())
            .Distinct().OrderBy (i => i).ToArray();
        foreach (var year in years)
        {
            Console.WriteLine (year);
            var yearIssues = allIssues.Where (i => i.Year.SafeToInt32() == year).ToArray();
            var yearNewspapers = yearIssues.Select (i => ((MagazineInfo) i.UserData!))
                .DistinctBy (i => i.Index).OrderBy (i => i.Title!.Trim ('"')).ToArray();

            excel.NewLine();
            excel.NewLine();
            excel.WriteText ($"ГОД {year}: {yearNewspapers.Length} компл.").Bold();
            excel.NewLine();

            foreach (var title in yearNewspapers)
            {
                Console.WriteLine (title.Title);
                var titleIssues = yearIssues.Where (i => i.MagazineCode == title.Index).ToArray();
                titleIssues = titleIssues.OrderBy (i => i.Number).ToArray();
                excel.NewLine();
                excel.WriteText (title.Title!).SetBorders();
                // excel.WriteText ($"{titleIssues.Length} вып.").SetBorders();
                var numbers = titleIssues.Select (i => i.Number).ToArray();
                var range = NumberRangeCollection.Cumulate (numbers!);
                var cumulated = range.ToString();
                excel.WriteText (cumulated).SetBorders().Alignment.WrapText = true;
            }
        }

        Console.WriteLine();
        Console.WriteLine (new string ('=', 80));
        Console.WriteLine();

        excel.SaveAs ("magazines.xlsx");

        return 0;
    }

    private static bool HavePlace
        (
            MagazineIssueInfo issue,
            string place
        )
    {
        foreach (var exemplar in issue.Exemplars!)
        {
            if (exemplar.Place.SameString (place))
            {
                return true;
            }
        }

        return false;
    }
}
