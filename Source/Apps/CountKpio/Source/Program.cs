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
using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Magazines;

#endregion

namespace CountKpio;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
class Program
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
        if (!connection.IsConnected)
        {
            Console.WriteLine (IrbisException.GetErrorDescription (connection.LastError));
            return 1;
        }

        var manager = new MagazineManager (Magna.Host, connection);
        var newspapers = manager.GetAllMagazines ("V=01");
        Console.WriteLine ($"Всего названий газет: {newspapers.Length}");
        newspapers = newspapers.OrderBy (static n => n.Title!.Trim ('"')).ToArray();

        var counter = new DictionaryCounter<int, int>();
        foreach (var newspaper in newspapers)
        {
            var issues = manager.GetIssues (newspaper);
            issues = issues.Where (static i => HavePlace (i, "Ф403")).ToArray();
            var years = issues.Select (static i => i.Year.SafeToInt32()).Distinct()
                .OrderBy (static y => y).ToArray();
            Console.Write ($"{newspaper.Title}: выпусков {issues.Length}, годы [{string.Join (',', years)}]");
            foreach (var year in years)
            {
                counter.Increment (year);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine (new string ('=', 80));
        Console.WriteLine();

        var sortedYears = counter.Keys.OrderBy (static y => y).ToArray();
        foreach (var year in sortedYears)
        {
            Console.WriteLine ($"{year}\t{counter[year]}");
        }

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
