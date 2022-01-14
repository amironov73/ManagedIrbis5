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
using AM.AppServices;
using AM.Linq;
using AM.Text.Ranges;
using AM.Windows.DevEx;

using ManagedIrbis.AppServices;
using ManagedIrbis.Magazines;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
sealed class Program
    : IrbisApplication
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    private Program (string[] args)
        : base (args)
    {
    }

    private static volatile bool _stop;

    /// <inheritdoc cref="MagnaApplication.ActualRun"/>
    protected override int ActualRun()
    {
        try
        {
            using var connection = Connection!;

            var manager = new MagazineManager (connection);
            var magazines = manager.GetAllMagazines()
                .OrderBy (m => m.Title)
                .ToArray();
            // magazines = magazines.Take (50).ToArray();
            Logger.LogInformation ("Magazines found: {Length}", magazines.Length);

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
                if (_stop)
                {
                    Logger.LogError ("Cancel key pressed");
                    break;
                }

                var title = magazine.ExtendedTitle;
                Logger.LogInformation ("Magazine: {Title}", title);
                excel.WriteText (title).SetBorders();

                var issues = manager.GetIssues (magazine);
                var years = issues.Select (i => i.Year)
                    .NonEmptyLines()
                    .Distinct()
                    .OrderBy (s => s)
                    .ToArray();
                var cumulated = NumberRangeCollection.Cumulate (years).ToString();
                excel.WriteText (cumulated).SetBorders();

                // можно взять места хранения из зарегистрированных экземпляров
                // var places = issues.SelectMany
                //         (
                //             i => i.Exemplars ?? Array.Empty<ExemplarInfo>()
                //         )
                //     .Select (e => e.Place)
                //     .NonEmptyLines()
                //     .Distinct()
                //     .OrderBy (s => s)
                //     .JoinText ();

                // а можно - из специального поля, как в ИРНИТУ
                var places = magazine.Record?.FMA (2005).JoinText() ?? string.Empty;
                excel.WriteText (places).SetBorders();

                // срок хранения - в специальном поле
                var shelfLife = magazine.Record?.FM (2024) ?? string.Empty;
                if (shelfLife == "0")
                {
                    shelfLife = "постоянно";
                }
                excel.WriteText (shelfLife).Center().SetBorders();

                // пустая ячейка, чтобы не вылазило за пределы предыдущей ячейки
                excel.WriteText (string.Empty);

                excel.NewLine ();
            }

            if (!_stop)
            {
                excel.SaveAs ("magazines.xlsx");
                Logger.LogInformation ("Excel file successfully created");
            }
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error while building magazine list");
            return 1;
        }

        return 0;
    }

    static void Main 
        (
            string[] args
        )
    {
        Console.TreatControlCAsInput = false;
        Console.CancelKeyPress += (_, eventArgs) =>
        {
            _stop = true;
            eventArgs.Cancel = true;
        };
        new Program (args).Run();
    }
}
