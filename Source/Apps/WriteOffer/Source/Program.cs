// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
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

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;

#endregion

namespace WriteOffer;

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
        if (args.Length < 2)
        {
            Console.Error.WriteLine ("USAGE: WriteOffer <connection-string> <booklist> [actNumber]");
            return 1;
        }

        var connectionString = IrbisUtility.Decrypt (args[0]);
        var numbers = File.ReadLines (args[1]);
        string? actNumber = default;
        if (args.Length > 2)
        {
            actNumber = args[2];
        }

        using var connection = ConnectionFactory.Shared.CreateSyncConnection();
        connection.ParseConnectionString (connectionString);
        connection.Connect();

        if (!connection.IsConnected)
        {
            Console.Error.WriteLine ("Can't connect");
            Console.Error.WriteLine (IrbisException.GetErrorDescription (connection.LastError));
            return 1;
        }

        var counter = 0;
        try
        {
            foreach (var line in numbers)
            {
                var inventory = line.Trim();
                if (string.IsNullOrEmpty (inventory))
                {
                    continue;
                }

                var record = connection.ByInventory (inventory);
                if (record is null)
                {
                    Console.WriteLine ($"{inventory}: not found");
                    continue;
                }

                var exemplars = ExemplarInfo.ParseRecord (record);
                var exemplar = exemplars.FirstOrDefault (x => x.Number == inventory);
                if (exemplar is null)
                {
                    Console.WriteLine ($"{inventory}: no field found");
                    continue;
                }

                exemplar.Status = ExemplarStatus.WrittenOff;
                exemplar.ActNumber2 = actNumber;
                var field = exemplar.Field.ThrowIfNull();
                exemplar.ApplyToField (field);

                var parameters = new WriteRecordParameters
                {
                    Record = record,
                    Actualize = true,
                    Lock = false,
                    DontParse = true
                };
                var ok = connection.WriteRecord (parameters) ? "OK" : "FAIL";
                Console.WriteLine ($"{++counter}) {inventory}: {ok}");
            }

            Console.WriteLine ("ALL DONE");
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
