// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IFileSystem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Environment;

/// <summary>
///
/// </summary>
internal interface IFileSystem
{
    /// <summary>
    ///
    /// </summary>
    bool FileExists (string path);

    /// <summary>
    ///
    /// </summary>
    IZipFile OpenZipFile (string path);

    /// <summary>
    ///
    /// </summary>
    IZipFile OpenZipFile (Stream stream);
}
