// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.Diagnostics;
using System.Linq;

using AM;

using ManagedIrbis;
using ManagedIrbis.Batch;

using Microsoft.Extensions.Configuration;

using static System.Console;

#endregion

#nullable enable

namespace RqstShrink
{
    internal class Program
    {
        private static int Main (string[] args)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                // Включаем конфигурирование в appsettings.json
                var config = new ConfigurationBuilder()
                    .SetBasePath (AppContext.BaseDirectory)
                    .AddJsonFile ("appsettings.json", true)
                    .Build();

                var connectionString = config.GetValue<string> ("ConnectionString");
                if (string.IsNullOrEmpty (connectionString))
                {
                    Error.WriteLine ("Connection string not specified");

                    return 1;
                }

                var rootCommand = new RootCommand ("RqstShrink");
                var csArgument = new Argument ("cs")
                {
                    ArgumentType = typeof (string),
                    Arity = ArgumentArity.ZeroOrOne,
                    Description = "Connection string"
                };
                var exprOption = new Option ("-e", "Search expression", typeof (string));
                rootCommand.AddArgument (csArgument);
                rootCommand.AddOption (exprOption);
                var builder = new CommandLineBuilder (rootCommand).Build();
                var parseResult = builder.Parse (args);
                var csValue = parseResult.ValueForArgument<string> (csArgument);
                if (!string.IsNullOrEmpty (csValue))
                {
                    connectionString = csValue;
                }

                using (var connection = ConnectionFactory.Shared.CreateSyncConnection())
                {
                    connection.ParseConnectionString (connectionString);
                    if (!connection.Connect())
                    {
                        Error.WriteLine ("Can't connect");
                        Error.WriteLine (IrbisException.GetErrorDescription (connection.LastError));

                        return 1;
                    }

                    var maxMfn = connection.GetMaxMfn();

                    var expression = config.GetValue<string> ("Expression");
                    var expressionValue = parseResult.ValueForOption<string> (exprOption);
                    if (!string.IsNullOrEmpty (expressionValue))
                    {
                        expression = expressionValue;
                    }

                    expression = expression.ThrowIfNull();

                    Write ("Reading good records ");

                    var goodRecords = BatchRecordReader.Search
                            (
                                connection,
                                connection.Database!,
                                expression,
                                1000,
                                batch => Write (".")
                            )
                        .ToArray();

                    WriteLine();
                    WriteLine ($"Good records loaded: {goodRecords.Length}");

                    if (goodRecords.Length == maxMfn)
                    {
                        WriteLine ("No truncation needed, exiting");

                        return 0;
                    }

                    connection.TruncateDatabase (connection.Database);

                    WriteLine ("Database truncated");

                    using (var writer = new BatchRecordWriter
                        (
                            connection,
                            connection.Database!,
                            500
                        ))
                    {
                        foreach (var record in goodRecords)
                        {
                            record.Version = 0;
                            record.Mfn = 0;
                            writer.Append (record);
                        }
                    }
                }

                WriteLine ("Good records restored");

                stopwatch.Stop();
                WriteLine ($"Elapsed: {stopwatch.Elapsed.ToAutoString()}");
            }
            catch (Exception ex)
            {
                Error.WriteLine (ex);

                return 1;
            }

            return 0;

        } // method Main

    } // class Program

} // namespace RqstShrink
