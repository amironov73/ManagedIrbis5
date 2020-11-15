// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using ManagedIrbis;

using static System.Console;

#endregion

#nullable enable

internal class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    private static async Task<int> Main(string[] args)
    {
        try
        {
            using var connection = ConnectionFactory.Default
                .CreateConnection();

            connection.Host = "127.0.0.1";
            connection.Username = "librarian";
            connection.Password = "secret";

            var success = await connection.ConnectAsync();
            if (!success)
            {
                Error.WriteLine("Can't connect");
                return 1;
            }

            WriteLine("Successfully connected");

            var version = await connection.GetServerVersionAsync();
            WriteLine(version);

            var processes = await connection.ListProcessesAsync();
            WriteLine("Processes: "
                + string.Join<ProcessInfo>(" | ", processes));

            var maxMfn = await connection.GetMaxMfnAsync();
            WriteLine($"Max MFN={maxMfn}");

            await connection.NopAsync();
            WriteLine("NOP");

            var record = await connection.ReadRecordAsync(1);
            WriteLine($"ReadRecord={record?.FM(200, 'a')}");

            var formatted = await connection.FormatRecordAsync("@brief", 1);
            WriteLine($"Formatted={formatted}");

            var files = await connection.ListFilesAsync("2.IBIS.*.mnu");
            WriteLine("Files: " + string.Join(",", files));

            var fileText = await connection.ReadTextFileAsync("2.IBIS.brief.pft");
            WriteLine($"BRIEF: {fileText}");
            WriteLine();

            await connection.DisconnectAsync();
            WriteLine("Successfully disconnected");
        }
        catch (Exception exception)
        {
            WriteLine(exception);
            return 1;
        }

        return 0;
    }
}
