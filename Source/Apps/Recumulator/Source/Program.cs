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
using System.Linq;

using AM;
using AM.AppServices;
using AM.Linq;
using AM.Text.Ranges;

using ManagedIrbis.AppServices;
using ManagedIrbis.Magazines;
using ManagedIrbis.Providers;

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

            foreach (var magazine in magazines)
            {
                if (_stop)
                {
                    Logger.LogError ("Cancel key pressed");
                    break;
                }

                // кумуляция у нас проживает в 909 поле
                var record = magazine.Record.ThrowIfNull();
                record.RemoveField (909);

                var title = magazine.ExtendedTitle;
                Logger.LogInformation ("Magazine: {Title}", title);

                var issues = manager.GetIssues (magazine);
                var years = issues.Select (i => i.Year)
                    .NonEmptyLines()
                    .TrimLines()
                    .NonEmptyLines()
                    .Distinct()
                    .OrderBy (s => s)
                    .ToArray();
                foreach (var year in years)
                {
                    var numbers = issues
                        .Where (i => i.Year == year)
                        .Select (i => i.Number)
                        .NonEmptyLines()
                        .Distinct()
                        .OrderBy (s => s)
                        .ToArray();
                    var cumulated = NumberRangeCollection.Cumulate (numbers).ToString();

                    // ReSharper disable TemplateIsNotCompileTimeConstantProblem
                    Logger.LogInformation ($"{year}: {cumulated}");
                    // ReSharper restore TemplateIsNotCompileTimeConstantProblem

                    record.Add (909, 'q', year, 'h', cumulated);
                }

                // connection.WriteRecord (record);
            }

            if (!_stop)
            {
                Console.WriteLine ("ALL DONE");
            }
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error during recumulation");
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
