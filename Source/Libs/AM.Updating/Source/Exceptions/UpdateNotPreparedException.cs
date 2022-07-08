// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* UpdateNotPreparedException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Updating.Exceptions;

/// <summary>
/// Thrown when launching the updater to install an update that was not prepared.
/// </summary>
public class UpdateNotPreparedException : Exception
{
    /// <summary>
    /// Package version.
    /// </summary>
    public Version Version { get; }

    /// <summary>
    /// Initializes an instance of <see cref="UpdateNotPreparedException"/>.
    /// </summary>
    public UpdateNotPreparedException (Version version)
        : base ($"Update to version '{version}' is not prepared. Please prepare an update before applying it.")
    {
        Version = version;
    }
}
