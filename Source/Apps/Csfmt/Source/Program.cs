// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using ManagedIrbis;
using ManagedIrbis.Scripting.Sharping;

#endregion

#nullable enable

namespace Csfmt;

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
        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        if (args.Length < 3)
        {
            return 1;
        }

        if (!Directory.Exists ("ScriptCache"))
        {
            Directory.CreateDirectory ("ScriptCache");
        }

        var connectionString = args[0];
        var searchExpression = args[1];
        var scriptFileName = args[2];

        try
        {
            var sourceCode = File.ReadAllText (scriptFileName);

            using var provider = ConnectionFactory.Shared.CreateSyncConnection();
            provider.ParseConnectionString (connectionString);
            provider.Connect();

            if (!provider.Connected)
            {
                Console.Error.WriteLine ("Can't connect");
                return 1;
            }

            var foundRecords = provider.SearchRead (searchExpression);
            if (foundRecords is null)
            {
                Console.Error.WriteLine ("Error during search");
                return 1;
            }

            using var cache = new ScriptCache ("ScriptCache");
            using var formatter = new ScriptFormatter (provider, cache);
            var instance = formatter.GetContextInstance (sourceCode, Console.Out);
            if (instance is null)
            {
                Console.Error.WriteLine ("Can't create formatter instance");
                return 1;
            }

            instance.BeforeAll();
            instance.UserData = formatter.UserData;
            foreach (var record in foundRecords)
            {
                instance.Record = record;
                instance.FormatRecord();
            }

            instance.Record = null;
            instance.AfterAll();
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
