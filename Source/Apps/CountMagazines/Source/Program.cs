// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable TemplateIsNotCompileTimeConstantProblem

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM;
using AM.AppServices;

using ManagedIrbis;
using ManagedIrbis.AppServices;
using ManagedIrbis.Magazines;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace CountMagazines;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
sealed class Program
    : IrbisApplication
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public Program (string[] args)
        : base (args)
    {
    }

    private static bool _stop;

    /// <inheritdoc cref="MagnaApplication.ActualRun"/>
    protected override int ActualRun
        (
            Func<int>? action
        )
    {
        var connection = Connection!;
        var manager = new MagazineManager (connection);
        var magazineList = File.ReadLines ("magazine-list.txt");

        foreach (var title in magazineList)
        {
            if (_stop)
            {
                Logger.LogError ("Cancel key pressed");
                break;
            }

            var expression = Search.Magazine (title).ToString();
            var record = connection.SearchReadOneRecord (expression);
            if (record is null)
            {
                Logger.LogError ("Can't find magazine {Title}", title);
                continue;
            }

            var magazine = MagazineInfo.Parse (record);
            if (magazine is null)
            {
                Logger.LogError ("Can't parse record for magazzine {Title}", title);
                continue;
            }

            var issues = manager.GetIssues (magazine)
                .Where (issue => issue.Year.SafeToInt32() >= 2017)
                .ToArray();
            var loanCount = issues.Sum (issue => issue.LoanCount);
            Console.WriteLine ($"{title}\t{issues.Length}\t{loanCount}");
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
