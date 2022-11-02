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
internal sealed class Program
    : IrbisApplication
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    public Program
        (
            string[] args
        )
        : base (args)
    {
        // пустое тело конструктора
    }

    private int DoTheWork()
    {
        var manager = new MagazineManager (Magna.Host, Connection);
        var magazineList = File.ReadLines ("magazine-list.txt");

        foreach (var title in magazineList)
        {
            if (Stop)
            {
                Logger.LogError ("Cancel key pressed");
                break;
            }

            var expression = Search.Magazine (title).ToString();
            var record = Connection.SearchReadOneRecord (expression);
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
            var message = $"{title}\t{issues.Length}\t{loanCount}";
            Logger.LogInformation ("Magazine: {Message}", magazine);
            Console.WriteLine (message);
        }

        return 0;
    }

    public static int Main
        (
            string[] args
        )
    {
        var program = new Program (args);
        program.ConfigureCancelKey();

        program.Run();
        var result = program.DoTheWork();
        program.Shutdown();

        return result;
    }
}
