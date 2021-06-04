// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* Program.cs -- утилита для удаления отметки об инвентаризации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ClearInventarization
{
    class Program
    {
        private static string _connectionString = string.Empty;
        private static string _searchExpression = string.Empty;
        private static Regex _placeRegex = new ("^$");
        private static readonly SyncConnection _connection
            = ConnectionFactory.Shared.CreateSyncConnection();

        private static void _ProcessRecord(Record record)
        {
            var exemplars = ExemplarInfo.Parse(record);

            var found = false;
            foreach (var exemplar in exemplars)
            {
                var place = exemplar.Place;
                if (string.IsNullOrWhiteSpace(place))
                {
                    continue;
                }

                if (!_placeRegex.IsMatch(place))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(exemplar.RealPlace))
                {
                    exemplar.Field!.SetSubFieldValue('!', null);
                    found = true;
                }

                if (!string.IsNullOrEmpty(exemplar.CheckedDate))
                {
                    exemplar.Field!.SetSubFieldValue('s', null);
                    found = true;
                }
            }

            if (found)
            {
                 _connection.WriteRecord(record);
                Console.Write('!');
            }
            else
            {
                Console.Write('.');
            }
        }

        static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("USAGE: ClearInventarization <connectionString> <searchExpression> <placeRegex>");
                return 1;
            }

            _connectionString = args[0];
            _searchExpression = args[1];
            _placeRegex = new Regex(args[2]);

            try
            {
                _connection.ParseConnectionString(_connectionString);
                _connection.Connect();

                if (!_connection.Connected)
                {
                    Console.Error.WriteLine("Can't connect");
                    Console.Error.WriteLine(IrbisException.GetErrorDescription(_connection.LastError));

                    return 1;
                }

                var found = _connection.Search(_searchExpression);
                Console.WriteLine($"Found: {found.Length}");

                var batch = new BatchRecordReader(_connection, found);

                foreach (var record in batch)
                {
                    _ProcessRecord(record);
                }

                Console.WriteLine();
                Console.WriteLine("ALL DONE");
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
                return 1;
            }
            finally
            {
                _connection.Dispose();
            }

            return 0;
        }
    }
}
