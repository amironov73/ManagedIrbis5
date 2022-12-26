// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* FolderModel.cs -- модель папки с файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Linq;

using AM;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace VisualRenumber;

public class FolderModel
    : ReactiveObject
{
    #region Properties

    [Reactive]
    public string? DirectoryName { get; set; }

    [Reactive]
    public FileItem[]? Files { get; set; }

    #endregion

    #region Public methods

    public void ReadDirectory
        (
            string directoryNameWithPattern
        )
    {
        Sure.NotNullNorEmpty (directoryNameWithPattern);

        string dirName;
        string pattern;
        if (Directory.Exists (directoryNameWithPattern))
        {
            dirName = directoryNameWithPattern;
            pattern = "*.*";
        }
        else
        {
            dirName = Path.GetDirectoryName (directoryNameWithPattern).ThrowIfNull();
            pattern = Path.GetFileName (directoryNameWithPattern).ThrowIfNull();
        }

        DirectoryName = dirName;
        var files = Directory.GetFiles (dirName, pattern, SearchOption.TopDirectoryOnly);
        Files = files.Select (one =>
        {
            var fileName = Path.GetFileName (one);
            return new FileItem
            {
                OldName = fileName,
                NewName = fileName
            };
        }).ToArray();
    }

    #endregion
}
