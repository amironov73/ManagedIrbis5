// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* IUpdateManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AM.Updating.Models;

#endregion

#nullable enable

namespace AM.Updating;

/// <summary>
/// Interface for <see cref="UpdateManager"/>.
/// </summary>
public interface IUpdateManager : IDisposable
{
    /// <summary>
    /// Information about the assembly, for which the updates are managed.
    /// </summary>
    AssemblyMetadata Updatee { get; }

    /// <summary>
    /// Checks for updates.
    /// </summary>
    Task<CheckForUpdatesResult> CheckForUpdatesAsync (CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether an update to given version has been prepared.
    /// </summary>
    bool IsUpdatePrepared (Version version);

    /// <summary>
    /// Gets a list of all prepared updates.
    /// </summary>
    IReadOnlyList<Version> GetPreparedUpdates();

    /// <summary>
    /// Prepares an update to specified version.
    /// </summary>
    Task PrepareUpdateAsync (Version version,
        IProgress<double>? progress = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Launches an external executable that will apply an update to given version, once this application exits.
    /// The updater can be instructed to also restart the application after it's updated.
    /// </summary>
    void LaunchUpdater (Version version, bool restart, string restartArguments);
}
