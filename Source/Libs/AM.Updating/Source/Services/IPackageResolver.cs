// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* IPackageResolver.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Updating.Services;

/// <summary>
/// Provider for resolving packages.
/// </summary>
public interface IPackageResolver
{
    /// <summary>
    /// Gets all available package versions.
    /// </summary>
    Task<IReadOnlyList<Version>> GetPackageVersionsAsync (CancellationToken cancellationToken = default);

    /// <summary>
    /// Downloads given package version.
    /// </summary>
    Task DownloadPackageAsync (Version version, string destFilePath,
        IProgress<double>? progress = null, CancellationToken cancellationToken = default);
}
