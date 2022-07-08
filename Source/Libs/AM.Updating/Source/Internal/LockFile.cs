// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* LockFile.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.Updating.Internal;

internal partial class LockFile : IDisposable
{
    private readonly FileStream _fileStream;

    public LockFile (FileStream fileStream) =>
        _fileStream = fileStream;

    public void Dispose() => _fileStream.Dispose();
}

internal partial class LockFile
{
    public static LockFile? TryAcquire (string filePath)
    {
        try
        {
            var fileStream = File.Open (filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            return new LockFile (fileStream);
        }
        catch (IOException) // This is the most specific exception for "access denied"
        {
            return null;
        }
    }
}
