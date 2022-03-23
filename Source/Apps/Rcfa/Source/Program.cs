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

using AM;
using AM.IO;
using AM.Text.Output;

using ManagedIrbis.Catalog;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Server;

#endregion

#nullable enable

namespace Barsik;

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
        if (args.Length < 4 || args.Length > 5)
        {
            Console.Error.WriteLine ("USAGE: Rcfa <archive> <irbis_server.ini> <original> <target>");
            return 1;
        }

        var archive = Path.GetFullPath (args[0]);
        var datai = Path.GetFullPath (args[1]);
        var original = args[2];
        var target = args[3];
        var ethalon = args.SafeAt (4, "IBIS");

        if (File.Exists (datai))
        {
            var ini = new IniFile (datai, IrbisEncoding.Ansi);
            var server = new ServerIniFile (ini);
            datai = server.DataPath;
        }

        if (!Directory.Exists (datai))
        {
            Console.Error.WriteLine ($"Directory {datai} doesn't exist");
            return 2;
        }

        try
        {
            var output = AbstractOutput.Console;
            var manager = new LocalCatalogManager (datai, output);
            var parameters = new RestoreFromArchiveParameters
            {
                ArchiveFile = archive,
                Original = original,
                Target = target,
                Ethalon = ethalon
            };

            manager.RestoreFromArchive (parameters);

            Console.WriteLine ("ALL DONE");
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 3;
        }

        return 0;
    }
}
