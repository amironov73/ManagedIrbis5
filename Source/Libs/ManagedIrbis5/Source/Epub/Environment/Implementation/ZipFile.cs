// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ZipFile.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO.Compression;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Environment.Implementation;

/// <summary>
///
/// </summary>
internal class ZipFile
    : IZipFile
{
    private readonly ZipArchive zipArchive;
    private bool isDisposed;

    public ZipFile (ZipArchive zipArchive)
    {
        this.zipArchive = zipArchive;
    }

    ~ZipFile()
    {
        Dispose (false);
    }

    public IZipFileEntry GetEntry (string entryName)
    {
        return new ZipFileEntry (zipArchive.GetEntry (entryName)!);
    }

    public void Dispose()
    {
        Dispose (true);
        GC.SuppressFinalize (this);
    }

    protected virtual void Dispose (bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                zipArchive.Dispose();
            }

            isDisposed = true;
        }
    }
}
