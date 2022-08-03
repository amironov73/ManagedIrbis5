// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ZipFileEntry.cs --
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
internal class ZipFileEntry
    : IZipFileEntry
{
    private readonly ZipArchiveEntry zipArchiveEntry;

    public ZipFileEntry(ZipArchiveEntry zipArchiveEntry)
    {
        this.zipArchiveEntry = zipArchiveEntry;
    }

    public long Length => zipArchiveEntry.Length;

    public Stream Open()
    {
        return zipArchiveEntry.Open();
    }
}
