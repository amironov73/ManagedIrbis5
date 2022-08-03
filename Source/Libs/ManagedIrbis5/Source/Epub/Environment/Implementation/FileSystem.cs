// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FileSystem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.IO.Compression;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Environment.Implementation;

/// <summary>
///
/// </summary>
internal class FileSystem
    : IFileSystem
{
    #region IFileSystem members

    /// <inheritdoc cref="IFileSystem.FileExists"/>
    public bool FileExists (string path)
    {
        return File.Exists (GetCompatibleFilePath (path));
    }

    /// <inheritdoc cref="IFileSystem.OpenZipFile(string)"/>
    public IZipFile OpenZipFile (string path)
    {
        return new ZipFile (System.IO.Compression.ZipFile.OpenRead (path));
    }

    /// <inheritdoc cref="IFileSystem.OpenZipFile(System.IO.Stream)"/>
    public IZipFile OpenZipFile (Stream stream)
    {
        return new ZipFile (new ZipArchive (stream, ZipArchiveMode.Read));
    }

    #endregion

    #region Private members

    private string GetCompatibleFilePath (string path)
    {
        return path;
    }

    #endregion
}
