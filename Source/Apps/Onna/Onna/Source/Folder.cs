// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Folder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using Avalonia.Platform.Storage;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

#endregion

namespace Onna;

/// <summary>
/// 
/// </summary>
public class Folder
    : ReactiveObject
{
    /// <summary>
    /// 
    /// </summary>
    [Reactive]
    public string? DirectoryName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Reactive]
    public string? SelectedFile { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Reactive]
    public string[]? Files { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public static Folder LoadFolder
        (
            string dirName
        )
    {
        var result = new Folder
        {
            DirectoryName = dirName,
            Files = Directory.GetFiles (dirName)
        };

        return result;
    }

    public override string ToString()
    {
        return $"Current: '{SelectedFile}'";
    }
}
