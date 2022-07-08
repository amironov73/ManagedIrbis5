// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* CheckForUpdatesResult.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Updating.Models;

/// <summary>
/// Result of checking for updates.
/// </summary>
public class CheckForUpdatesResult
{
    /// <summary>
    /// All available package versions.
    /// </summary>
    public IReadOnlyList<Version> Versions { get; }

    /// <summary>
    /// Last available package version.
    /// Null if there are no available packages.
    /// </summary>
    public Version? LastVersion { get; }

    /// <summary>
    /// Whether there is a package with higher version than the current version.
    /// </summary>
    public bool CanUpdate { get; }

    /// <summary>
    /// Initializes a new instance of <see cref="CheckForUpdatesResult"/>.
    /// </summary>
    public CheckForUpdatesResult (IReadOnlyList<Version> versions, Version? lastVersion, bool canUpdate)
    {
        Versions = versions;
        LastVersion = lastVersion;
        CanUpdate = canUpdate;
    }
}
