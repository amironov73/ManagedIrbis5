// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IniFileFixer.cs -- исправляет пути в серверном INI-файле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis.Infrastructure;

#endregion

namespace ManagedIrbis.Fixing;

/// <summary>
/// Исправляет пути в серверном INI-файле.
/// </summary>
public sealed class IniFilePathFixer
{
    #region Public methods

    /// <summary>
    /// Исправление путей в указанном INI-файле.
    /// </summary>
    /// <param name="originalFile">Оригинальный файл.</param>
    /// <param name="fixedFile">Файл, в который будет помещен результат.</param>
    /// <param name="rootPath">Корневой путь, который необходимо установить.</param>
    public void ChangeRootPath
        (
            string originalFile,
            string fixedFile,
            string rootPath
        )
    {
        Sure.FileExists (originalFile);
        Sure.NotNullNorEmpty (fixedFile);
        Sure.NotNullNorEmpty (rootPath);

        File.Delete (fixedFile);

        var ansi = IrbisEncoding.Ansi;
        using var input = new StreamReader (originalFile, ansi);
        using var output = new StreamWriter (fixedFile,false, ansi);
        while (input.ReadLine() is { } originalLine)
        {
            var fixedLine = _FixRootPath (originalLine, rootPath);
            output.WriteLine (fixedLine);
        }
    }

    /// <summary>
    /// Исправление путей в указанном INI-файле
    /// (через промежуточный временный файл).
    /// </summary>
    /// <param name="iniFile">INI-файл, в котором пути подлежат исправлению.</param>
    /// <param name="rootPath">Корневой путь, который необходимо установить.</param>
    public void ChangeRootPath
        (
            string iniFile,
            string rootPath
        )
    {
        Sure.FileExists (iniFile);
        Sure.NotNullNorEmpty (rootPath);

        var temporaryFile = $"{iniFile}_{Guid.NewGuid():N}";
        ChangeRootPath (iniFile, temporaryFile, rootPath);
        if (File.Exists (temporaryFile))
        {
            File.Replace
                (
                    sourceFileName: temporaryFile,
                    destinationFileName: iniFile,
                    destinationBackupFileName: null,
                    ignoreMetadataErrors: false
                );
        }
    }

    #endregion

    #region Private members

    private string _FixRootPath
        (
            string line,
            string rootPath
        )
    {
        if (!line.Contains ('='))
        {
            return line;
        }

        var parts = line.Split ('=', 2);
        if (parts.Length != 2)
        {
            return line;
        }

        var key = parts[0].Trim().ToUpperInvariant();
        var value = key switch
        {
            "WEBCGIPATH" => PathUtility.AppendBackslash (Path.Combine (rootPath, "cgi")),
            "DATAPATH" => PathUtility.AppendBackslash (Path.Combine (rootPath, "datai")),
            "WORKDIR" => Path.Combine (rootPath, "workdir"),
            "ACTABPATH" => Path.Combine (rootPath, "isisacw"),
            "UCTABPATH" => Path.Combine (rootPath, "isisucw"),
            "LIBDIR" => PathUtility.AppendBackslash (rootPath),
            "SYSPATH" => PathUtility.AppendBackslash (rootPath),

            _ => parts[1]
        };

        return parts[0] + "=" + value;
    }

    #endregion
}
