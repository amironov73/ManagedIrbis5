// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

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

namespace CountMagazines
{
    class Program
        : IrbisApplication
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public Program(string[] args)
            : base(args)
        {
        } // constructor

        /// <inheritdoc cref="MagnaApplication.ActualRun"/>
        protected override int ActualRun()
        {
            var connection = Connection!;
            var manager = new MagazineManager (connection);
            var magazineList = File.ReadLines ("magazine-list.txt");

            foreach (var title in magazineList)
            {
                var expression = Search.Magazine (title).ToString();
                var record = connection.SearchReadOneRecord (expression);
                if (record is null)
                {
                    Logger.LogError ($"Can't find magazine {title}");
                    continue;
                }

                var magazine = MagazineInfo.Parse (record);
                if (magazine is null)
                {
                    Logger.LogError ($"Can't parse record for magazzine {title}");
                    continue;
                }

                var issues = manager.GetIssues (magazine)
                    .Where (issue => issue.Year.SafeToInt32() >= 2017)
                    .ToArray();
                var loanCount = issues.Sum (issue => issue.LoanCount);
                Console.WriteLine ($"{title}\t{issues.Length}\t{loanCount}");

            } // foreach

            return 0;

        } // method ActualRun

        static void Main (string[] args) => new Program (args).Run();

    } // class Program

} // namespace CountMagazines
