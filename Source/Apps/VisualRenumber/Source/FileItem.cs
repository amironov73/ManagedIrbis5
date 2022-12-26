// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* FileItem.cs -- один файл в папке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

#nullable enable

namespace VisualRenumber;

public class FileItem
    : ReactiveObject
{
    #region Properties

    [Reactive]
    public bool IsChecked { get; set; }

    [Reactive]
    public string? OldName { get; set; }

    [Reactive]
    public string? NewName { get; set; }

    #endregion
}
