// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* IPackageExtractor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Updating.Services;

/// <summary>
/// Provider for extracting packages.
/// </summary>
public interface IPackageExtractor
{
    /// <summary>
    /// Extracts contents of the given package to the given output directory.
    /// </summary>
    Task ExtractPackageAsync (string sourceFilePath, string destDirPath,
        IProgress<double>? progress = null, CancellationToken cancellationToken = default);
}
