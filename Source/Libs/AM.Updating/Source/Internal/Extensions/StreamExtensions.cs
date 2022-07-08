// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* StreamExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Updating.Internal.Extensions;

internal static class StreamExtensions
{
    public static async Task<int> CopyBufferedToAsync (
        this Stream source,
        Stream destination,
        byte[] buffer,
        CancellationToken cancellationToken = default)
    {
        var bytesCopied = await source.ReadAsync (buffer, cancellationToken);
        await destination.WriteAsync (buffer, 0, bytesCopied, cancellationToken);

        return bytesCopied;
    }

    public static async Task CopyToAsync (
        this Stream source,
        Stream destination,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        using var buffer = PooledBuffer.ForStream();

        var totalBytesCopied = 0L;
        int bytesCopied;
        do
        {
            bytesCopied = await source.CopyBufferedToAsync (destination, buffer.Array, cancellationToken);
            totalBytesCopied += bytesCopied;

            progress?.Report (1.0 * totalBytesCopied / source.Length);
        } while (bytesCopied > 0);
    }
}
