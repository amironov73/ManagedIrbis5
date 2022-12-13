// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EnvironmentDependencies.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Epub.Environment.Implementation;

#endregion

#nullable enable

namespace ManagedIrbis.Epub.Environment;

/// <summary>
///
/// </summary>
internal static class EnvironmentDependencies
{
    static EnvironmentDependencies()
    {
        FileSystem = new FileSystem();
    }

    /// <summary>
    ///
    /// </summary>
    public static IFileSystem FileSystem { get; internal set; }
}
