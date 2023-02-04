// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DirectoryModel.cs -- модель папки с картинками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;

using Avalonia.Platform.Storage;
using Avalonia.Platform.Storage.FileIO;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace Peeper;

/// <summary>
/// Модель папки с картинками
/// </summary>
internal class FolderModel
    : ReactiveObject
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    [Reactive]
    public string? DirectoryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Reactive]
    //public string? SelectedFile { get; set; }
    public IStorageItem? SelectedFile { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Reactive]
    public IStorageItem[]? Files { get; set; }

    #endregion

    #region Public methods

    public static FolderModel LoadFolder
        (
            string folderName
        )
    {
        Sure.DirectoryExists (folderName);

        throw new NotImplementedException();
        // return LoadFolder (new BclStorageFolder (folderName));
    }

    public static FolderModel LoadFolder
        (
            IStorageFolder folder
        )
    {
        var result = new FolderModel
        {
            DirectoryName = folder.Name,
            Files = folder.GetItemsAsync().Result.ToArray()
        };

        return result;
    }

    #endregion
}
