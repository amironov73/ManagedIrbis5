// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ParFileValidator.cs -- проверяет пути в PAR-файлах
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

using ManagedIrbis.Menus;

#endregion

namespace ManagedIrbis.Fixing;

/// <summary>
/// Проверяет пути в PAR-файлах.
/// </summary>
public sealed class ParFileValidator
{
    #region Properties

    /// <summary>
    /// Поток для вывода ошибок.
    /// </summary>
    public TextWriter Output { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="output"></param>
    public ParFileValidator
        (
            TextWriter output
        )
    {
        Sure.NotNull (output);

        Output = output;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка указанного PAR-файла.
    /// </summary>
    public bool CheckParFile
        (
            string fileName,
            string basePath
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNullNorEmpty (basePath);

        try
        {
            var parFile = ParFile.ParseFile (fileName);
            var databaseName = Path.GetFileNameWithoutExtension (fileName);

            return parFile.CheckComponents (databaseName, basePath, Output);
        }
        catch (Exception exception)
        {
            Output.WriteLine ($"Exception during checks for {fileName}");
            Output.WriteLine (exception);
            return false;
        }
    }

    /// <summary>
    /// Проверка MNU-файла.
    /// </summary>
    public bool CheckMenuFile
        (
            string fileName,
            string basePath
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNullNorEmpty (basePath);

        var menu = MenuFile.ParseLocalFile (fileName);
        var result = true;
        foreach (var entry in menu.Entries)
        {
            var databaseName = entry.Code;
            if (string.IsNullOrEmpty (databaseName))
            {
                continue;
            }
            if (databaseName.StartsWith ('-'))
            {
                databaseName = databaseName[1..];
            }
            var menuDirectory = Path.GetDirectoryName (fileName);
            var parName = Path.Combine
                (
                    menuDirectory!,
                    databaseName + ".par"
                );
            if (!Unix.FileExists (parName))
            {
                Output.WriteLine ($"Can't locate '{parName}' mentioned in '{fileName}'");
                result = false;
            }
        }

        return result;
    }

    #endregion
}
