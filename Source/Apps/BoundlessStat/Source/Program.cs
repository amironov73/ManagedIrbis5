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

using ManagedIrbis;
using ManagedIrbis.AppServices;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace BoundlessStat;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
internal sealed class Program
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
    protected override int ActualRun
        (
            Func<int>? action
        )
    {
        try
        {
            using var connection = (ISyncConnection) Connection!;
            var database = connection.EnsureDatabase();

            var allBooks = connection.SearchAll ("V=KN");
            // var allBooks = connection.Search ("V=KN");
            Logger.LogInformation ("Found: {Length}", allBooks.Length);

            var chunks = allBooks.Chunk (1000).ToArray();

            foreach (var chunk in chunks)
            {
                var records = connection.ReadRecords (database, chunk);
                if (records is null)
                {
                    Logger.LogError ("Error reading records");
                    return 1;
                }

                var formatted = connection.FormatRecords (chunk, IrbisFormat.Brief);
                if (formatted is null)
                {
                    Logger.LogError ("Error formatting records");
                    return 1;
                }

                if (records.Length != formatted.Length)
                {
                    Logger.LogError ("records.Length != formatted.Length");
                    return 1;
                }

                var length = records.Length;
                for (var i = 0; i < length; i++)
                {
                    var record = records[i];
                    var brief = formatted[i];
                    var knowledge = record.FM (60) ?? "нет"; // раздел знаний
                    var count = record.FM (999).SafeToInt32();
                    Console.WriteLine ($"{brief}\t{knowledge}\t{count}");
                }
            }

            if (!_stop)
            {
                Console.WriteLine ("ALL DONE");
            }
        }
        catch (Exception exception)
        {
            Logger.LogError (exception, "Error during stat");
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
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
