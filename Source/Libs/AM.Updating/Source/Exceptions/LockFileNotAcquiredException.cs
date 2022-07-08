// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* LockFileAcquiredException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Updating.Exceptions;

/// <summary>
/// Thrown when an attempt to acquire a lock file failed.
/// </summary>
public class LockFileNotAcquiredException : Exception
{
    /// <summary>
    /// Initializes an instance of <see cref="LockFileNotAcquiredException"/>.
    /// </summary>
    public LockFileNotAcquiredException()
        : base (
            "Could not acquire a lock file. Most likely, another instance of this application currently owns the lock file.")
    {
    }
}
