// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Folder.cs -- папка с файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace Onna;

/// <summary>
/// Папка с файлами.
/// </summary>
public class Folder
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Имя директории.
    /// </summary>
    [Reactive]
    public string? DirectoryName { get; init; }

    /// <summary>
    /// Имя выбранного файла.
    /// </summary>
    [Reactive]
    public string? SelectedFile { get; set; }

    /// <summary>
    /// Файлы, найденные в папке.
    /// </summary>
    [Reactive]
    public string[]? Files { get; init; }

    #endregion

    /// <summary>
    /// Загрузка файлов из указанной папки.
    /// </summary>
    public static Folder LoadFolder
        (
            string dirName
        )
    {
        Sure.NotNullNorEmpty (dirName);

        string? fileToSelect = null;
        if (File.Exists (dirName))
        {
            var temporary = Path.GetDirectoryName (dirName);
            if (!string.IsNullOrEmpty (temporary))
            {
                fileToSelect = dirName;
                dirName = temporary;
            }
        }

        var result = new Folder
        {
            DirectoryName = dirName,
            Files = Directory.GetFiles (dirName),
            SelectedFile = fileToSelect
        };

        Array.Sort (result.Files);

        return result;
    }

    public override string ToString()
    {
        return $"Current: '{SelectedFile}'";
    }
}
