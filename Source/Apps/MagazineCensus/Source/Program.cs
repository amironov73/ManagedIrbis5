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
using System.Drawing;
using System.Linq;

using AM;
using AM.Linq;
using AM.Text.Ranges;
using AM.Windows.DevExpress;

using ManagedIrbis;
using ManagedIrbis.Magazines;

#endregion

#nullable enable

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
static class Program
{
    public static int Main ()
    {
        try
        {
            using var connection = ConnectionFactory.Shared.CreateSyncConnection();
            connection.ParseConnectionString ("host=127.0.0.1;user=librarian;password=secret;db=PERIO;");
            connection.Connect();
            if (!connection.Connected)
            {
                Console.WriteLine ("Not connected");
                Console.WriteLine (IrbisException.GetErrorDescription (connection.LastError));
                return 1;
            }

            var manager = new MagazineManager (connection);
            var magazines = manager.GetAllMagazines()
                .OrderBy (m => m.Title)
                .ToArray();
            //magazines = magazines.Take (50).ToArray();
            Console.WriteLine (magazines.Length);

            using var excel = new EasyExcel();
            excel.NewLine ();

            excel.MergeCells (3);
            excel.WriteText ("Перечень журналов").Bold().Center().Font.Size = 16.0;
            excel.NewLine (2);

            var column = excel.Worksheet.Columns[0];
            column.WidthInCharacters = 50;

            column = excel.Worksheet.Columns[1];
            column.WidthInCharacters = 10;

            column = excel.Worksheet.Columns[2];
            column.WidthInCharacters = 10;

            column = excel.Worksheet.Columns[3];
            column.WidthInCharacters = 10;

            excel.WriteText ("Журнал/газета").Center();
            excel.WriteText ("В наличии").Center();
            excel.WriteText ("Место").Center();
            excel.WriteText ("Срок").Center();
            excel.CurrentRow.Background (Color.Black).TextColor (Color.White).Bold();
            excel.WriteText (string.Empty);
            excel.FreezeRows();

            excel.NewLine ();

            foreach (var magazine in magazines)
            {
                var title = magazine.ExtendedTitle;
                Console.WriteLine (title);
                excel.WriteText (title).SetBorders();

                var issues = manager.GetIssues (magazine);
                var years = issues.Select (i => i.Year)
                    .NonEmptyLines()
                    .Distinct()
                    .OrderBy (s => s)
                    .ToArray();
                var cumulated = NumberRangeCollection.Cumulate (years).ToString();
                excel.WriteText (cumulated).SetBorders();

                // var places = issues.SelectMany
                //         (
                //             i => i.Exemplars ?? Array.Empty<ExemplarInfo>()
                //         )
                //     .Select (e => e.Place)
                //     .NonEmptyLines()
                //     .Distinct()
                //     .OrderBy (s => s)
                //     .JoinText ();
                var places = magazine.Record?.FMA (2005).JoinText() ?? string.Empty;
                excel.WriteText (places).SetBorders();

                var shelfLife = magazine.Record?.FM (2024) ?? string.Empty;
                if (shelfLife == "0")
                {
                    shelfLife = "постоянно";
                }
                excel.WriteText (shelfLife).SetBorders();

                // пустая ячейка, чтобы не вылазило за пределы предыдущей ячейки
                excel.WriteText (string.Empty);

                excel.NewLine ();
            }

            excel.SaveAs ("magazines.xlsx");
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
