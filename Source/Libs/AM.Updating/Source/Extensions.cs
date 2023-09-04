// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Extensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace AM.Updating;

/// <summary>
/// Extensions for update management.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Launches an external executable that will apply an update to given version, once this application exits.
    /// The updater can be instructed to also restart the application after it's updated.
    /// If the application is to be restarted, it will receive the same command line arguments as it did initially.
    /// </summary>
    public static void LaunchUpdater
        (
            this IUpdateManager manager,
            Version version,
            bool restart = true
        )
        => manager.LaunchUpdater (version, restart, Internal.EnvironmentEx.GetCommandLineWithoutExecutable());

    /// <summary>
    /// Checks for new version and performs an update if available.
    /// </summary>
    public static async Task CheckPerformUpdateAsync
        (
            this IUpdateManager manager,
            bool restart = true,
            IProgress<double>? progress = null,
            CancellationToken cancellationToken = default
        )
    {
        // Check
        var result = await manager.CheckForUpdatesAsync (cancellationToken);
        if (!result.CanUpdate || result.LastVersion == null)
        {
            return;
        }

        // Prepare
        await manager.PrepareUpdateAsync (result.LastVersion, progress, cancellationToken);

        // Apply
        manager.LaunchUpdater (result.LastVersion, restart);

        // Exit
        Environment.Exit (0);
    }
}
