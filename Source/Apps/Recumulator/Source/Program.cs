// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;

using ManagedIrbis.AppServices;
using ManagedIrbis.Magazines;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
internal sealed class Program
    : IrbisApplication
{
    /// <summary>
    /// Конструктор.
    /// </summary>
    private Program
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
        var cumulator = new Cumulator();
        var magazines = manager.GetAllMagazines()
            .OrderBy (m => m.Title)
            .ToArray();

        // magazines = magazines.Take (50).ToArray();
        Logger.LogInformation ("Magazines found: {Length}", magazines.Length);

        foreach (var magazine in magazines)
        {
            if (Stop)
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
            var cumulated = cumulator.Cumulate (issues, CumulationMethod.Complect);
            foreach (var cumulation in cumulated)
            {
                Logger.LogInformation ("{Cumulation}", cumulation.ToString());
                var field = cumulation.ToField();
                record.Add (field);
            }

            Connection.WriteRecord (record, dontParse: true);
        }

        if (!Stop)
        {
            Console.WriteLine ("ALL DONE");
        }

        return 0;
    }

    private static int Main
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
