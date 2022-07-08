// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ZipPackageExtractor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM.Updating.Internal;
using AM.Updating.Internal.Extensions;

#endregion

#nullable enable

namespace AM.Updating.Services;

/// <summary>
/// Extracts files from zip-archived packages.
/// </summary>
public class ZipPackageExtractor : IPackageExtractor
{
    /// <inheritdoc />
    public async Task ExtractPackageAsync (string sourceFilePath, string destDirPath,
        IProgress<double>? progress = null, CancellationToken cancellationToken = default)
    {
        // Read the zip
        using var archive = ZipFile.OpenRead (sourceFilePath);

        // For progress reporting
        var totalBytes = archive.Entries.Sum (e => e.Length);
        var totalBytesCopied = 0L;

        // Loop through all entries
        foreach (var entry in archive.Entries)
        {
            // Get destination paths
            var entryDestFilePath = Path.Combine (destDirPath, entry.FullName);
            var entryDestDirPath = Path.GetDirectoryName (entryDestFilePath);

            // Create directory
            if (!string.IsNullOrWhiteSpace (entryDestDirPath))
                Directory.CreateDirectory (entryDestDirPath);

            // If the entry is a directory - continue
            if (entry.FullName.Last() == Path.DirectorySeparatorChar ||
                entry.FullName.Last() == Path.AltDirectorySeparatorChar)
                continue;

            // Extract entry
            using var input = entry.Open();
            using var output = File.Create (entryDestFilePath);

            using var buffer = PooledBuffer.ForStream();
            int bytesCopied;
            do
            {
                bytesCopied = await input.CopyBufferedToAsync (output, buffer.Array, cancellationToken);
                totalBytesCopied += bytesCopied;
                progress?.Report (1.0 * totalBytesCopied / totalBytes);
            } while (bytesCopied > 0);
        }
    }
}
