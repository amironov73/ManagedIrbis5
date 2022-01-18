// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

internal static class FileStreamEx
{
    private const int DefaultBufferSize = 4096;

    public static FileStream OpenReadFileStream (string filePath)
    {
        return new (filePath, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize,
            FileOptions.SequentialScan);
    }

    public static FileStream OpenAsyncReadFileStream (string filePath)
    {
        return new (filePath, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize,
            FileOptions.Asynchronous | FileOptions.SequentialScan);
    }
}
