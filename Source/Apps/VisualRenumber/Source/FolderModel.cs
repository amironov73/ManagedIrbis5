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
using AM.IO;
using AM.Text;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace VisualRenumber;

public class FolderModel
    : ReactiveObject
{
    #region Properties

    public FileRenumber Renumber { get; }


    [Reactive]
    public string? DirectoryName { get; set; }

    [Reactive]
    public int GroupNumber { get; set; } = 1;

    [Reactive]
    public bool DryRun { get; set; }

    [Reactive]
    public string? Prefix { get; set; }

    [Reactive]
    public int GroupWidth { get; set; }

    [Reactive]
    public FileItem[]? Files { get; set; }

    #endregion

    #region Construction

    public FolderModel()
    {
        Renumber = new ();
    }

    #endregion

    #region Private members

    internal void SyncRenumber()
    {
        Renumber.GroupNumber = GroupNumber - 1;
        Renumber.GroupWidth = GroupWidth;
        Renumber.DryRun = DryRun;
        Renumber.Prefix = Prefix;

        if (Files is not null)
        {
            foreach (var bunch in Files)
            {
                bunch.NewName = bunch.OldName;
            }

            var originalFiles = Files
                .Where (one => one.IsChecked)
                .Select (one => one.OldName!).ToList();
            var newNames = Renumber.GenerateNames (originalFiles);
            foreach (var bunch in newNames)
            {
                var found = Files.FirstOrDefault (one => one.OldName == bunch.OldName);
                if (found is not null)
                {
                    found.NewName = bunch.NewName;
                }
            }
        }
    }

    #endregion

    #region Public methods

    public void Refresh()
    {
        SyncRenumber();
    }

    public void CheckAll()
    {
        if (Files is not null)
        {
            foreach (var item in Files)
            {
                item.IsChecked = true;
            }
        }
        SyncRenumber();
    }

    public void CheckNone()
    {
        if (Files is not null)
        {
            foreach (var item in Files)
            {
                item.IsChecked = false;
            }
        }
        SyncRenumber();
    }

    public void CheckReverse()
    {
        if (Files is not null)
        {
            foreach (var item in Files)
            {
                item.IsChecked = !item.IsChecked;
            }
        }
        SyncRenumber();
    }

    public bool HasError
        (
            FileItem item
        )
    {
        if (!item.IsChecked)
        {
            return false;
        }

        if (string.CompareOrdinal (item.OldName, item.NewName) == 0)
        {
            return false;
        }

        var fullName = Path.Combine (DirectoryName!, item.NewName!);
        return File.Exists (fullName) || Directory.Exists (fullName);
    }

    public bool HasError()
    {
        if (Files is not null)
        {
            foreach (var item in Files)
            {
                if (HasError (item))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void RenameChecked()
    {
        if (HasError())
        {
            return;
        }

        if (Files is not null)
        {
            foreach (var item in Files)
            {
                if (item.IsChecked && string.CompareOrdinal (item.OldName, item.NewName) != 0)
                {
                    var fullOld = Path.Combine (DirectoryName!, item.OldName!);
                    var fullNew = Path.Combine (DirectoryName!, item.NewName!);
                    File.Move (fullOld, fullNew, false);
                }
            }
        }
    }

    public void ClearChecked()
    {
        Files = Files?.Where (item => !item.IsChecked).ToArray();
    }

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
        var unsorted = Directory.GetFiles (dirName, pattern, SearchOption.TopDirectoryOnly);
        var sorted = NumberText.Sort (unsorted).ToArray();
        Files = sorted.Select (one =>
        {
            var fileName = Path.GetFileName (one);
            return new FileItem
            {
                IsChecked = true,
                OldName = fileName,
                NewName = fileName
            };
        }).ToArray();

        SyncRenumber();
    }

    #endregion
}
