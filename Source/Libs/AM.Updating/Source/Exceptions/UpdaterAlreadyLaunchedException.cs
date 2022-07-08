// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* UpdaterAlreadyLaunchedException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Updating.Exceptions;

/// <summary>
/// Thrown when launching the updater after it has already been launched.
/// </summary>
public class UpdaterAlreadyLaunchedException : Exception
{
    /// <summary>
    /// Initializes an instance of <see cref="UpdaterAlreadyLaunchedException"/>.
    /// </summary>
    public UpdaterAlreadyLaunchedException()
        : base ("Updater has already been launched, either by this or another instance of this application.")
    {
    }
}
