// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* PackageNotFoundException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Updating.Exceptions;

/// <summary>
/// Thrown when a package of given version was not found by a resolver.
/// </summary>
public class PackageNotFoundException : Exception
{
    /// <summary>
    /// Package version.
    /// </summary>
    public Version Version { get; }

    /// <summary>
    /// Initializes an instance of <see cref="PackageNotFoundException"/>.
    /// </summary>
    public PackageNotFoundException (Version version)
        : base ($"Package version '{version}' was not found by the configured package resolver.")
    {
        Version = version;
    }
}
