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
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis.Fixing;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace CheckPar;

internal static class Program
{
    private static bool CheckSysPath
        (
            string sysPath,
            string datai,
            TextWriter output
        )
    {
        Sure.NotNullNorEmpty (sysPath);
        Sure.NotNullNorEmpty (datai);
        Sure.NotNull (output);

        if (!Unix.DirectoryExists (sysPath))
        {
            output.WriteLine ($"SysPath={sysPath} doesn't exist");
            return false;
        }

        if (!Unix.DirectoryExists (datai))
        {
            output.WriteLine ($"DataPath={datai} doesn't exist");
            return false;
        }

        var result = true;
        var parFiles = Directory.GetFiles (datai, "*.par", SearchOption.TopDirectoryOnly);
        foreach (var parFile in parFiles)
        {
            if (!new ParFileValidator (output).CheckParFile (parFile, sysPath))
            {
                output.WriteLine ($"{parFile} contains error(s)");
                result = false;
            }
        }

        var menuFiles = Directory.GetFiles (datai, "*.mnu", SearchOption.TopDirectoryOnly);
        foreach (var menuFile in menuFiles)
        {
            if (!new ParFileValidator (output).CheckMenuFile (menuFile, sysPath))
            {
                output.WriteLine ($"{menuFile} contains error(s)");
                result = false;
            }
        }

        return result;
    }

    private static bool CheckIniFile
        (
            string fileName,
            TextWriter output
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNull (output);

        var iniFile = new IniFile();
        iniFile.Read (fileName, IrbisEncoding.Ansi);
        var datai = iniFile.GetValue ("Main", "DataPath", null);
        if (string.IsNullOrEmpty (datai))
        {
            output.WriteLine ("DataPath not specified");
            return false;
        }

        var sysPath = iniFile.GetValue ("Main", "SysPath", null);
        if (string.IsNullOrEmpty (sysPath))
        {
            output.WriteLine ("SysPath not specified");
            return false;
        }

        return CheckSysPath (sysPath, datai, output);
    }

    public static int Main
        (
            string[] args
        )
    {
        if (args.Length != 1)
        {
            Console.Error.WriteLine ("CheckPar <ini-file>");
            return 1;
        }

        try
        {
            var argument = args[0];

            if (Directory.Exists (argument))
            {
                // это путь к системной папке сервера
                var sysPath = argument;
                var datai = Path.Combine (sysPath, "datai");
                if (!CheckSysPath (argument, datai, Console.Out))
                {
                    return 1;
                }
            }
            else
            {
                if (Path.GetExtension (argument).SameString (".ini"))
                {
                    // это путь к INI-файлу
                    if (!CheckIniFile (argument, Console.Out))
                    {
                        return 1;
                    }
                }
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);
            return 1;
        }

        return 0;
    }
}
