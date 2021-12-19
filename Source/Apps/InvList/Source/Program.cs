// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* Program.cs -- утилита для дампа экземпляров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis;
using ManagedIrbis.Fields;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace InvList;

class Program
{
    private static string _connectionString = string.Empty;
    private static string _searchExpression = string.Empty;

    private static readonly SyncConnection _connection
        = ConnectionFactory.Shared.CreateSyncConnection();

    private static void _ProcessRecord
        (
            int mfn
        )
    {
        var record = _connection.ReadRecord (mfn);
        if (record is null)
        {
            return;
        }

        var description = _connection.FormatRecord ("@brief", mfn);
        var worklist = record.FM (920);
        var exemplars = ExemplarInfo.ParseRecord (record);
        var count = record.FM (999).SafeToInt32();

        foreach (var exemplar in exemplars)
        {
            Console.WriteLine (
                $"{exemplar.Number}\t{exemplar.Place}\t{worklist}\t{exemplar.Status}\t{record.Mfn}\t{description}\t{count}");
        }
    }

    static int Main
        (
            string[] args
        )
    {
        if (args.Length != 2)
        {
            Console.WriteLine ("USAGE: InvList <connectionString> <search>");
            return 1;
        }

        _connectionString = args[0];
        _searchExpression = args[1];

        try
        {
            _connection.ParseConnectionString (_connectionString);
            _connection.Connect();

            if (!_connection.Connected)
            {
                Console.Error.WriteLine ("Can't connect");
                Console.Error.WriteLine (IrbisException.GetErrorDescription (_connection.LastError));

                return 1;
            }

            var found = _connection.Search (_searchExpression);

            //Parallel.ForEach(found, _ProcessRecord);

            foreach (var mfn in found)
            {
                _ProcessRecord (mfn);
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }
        finally
        {
            _connection.Dispose();
        }

        return 0;
    }
}
