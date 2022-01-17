// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* SystemDirectoryIO.cs -- тривиальный доступ к директории
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Тривиальный доступ к директории
/// </summary>
public sealed class SystemDirectoryIO
    : IDirectoryIO
{
    #region IDirectoryIO members

    /// <inheritdoc cref="CreateDirectory"/>
    public void CreateDirectory
        (
            string path
        )
    {
        Directory.CreateDirectory (path);
    }

    /// <inheritdoc cref="IDirectoryIO.DeleteDirectory"/>
    public void DeleteDirectory
        (
            string path,
            bool recursive = true
        )
    {
        Directory.Delete (path, recursive);
    }

    /// <inheritdoc cref="IDirectoryIO.GetParentDirectory"/>
    public string? GetParentDirectory
        (
            string path
        )
    {
        return Directory.GetParent (path)?.Name;
    }

    /// <inheritdoc cref="IDirectoryIO.GetCreationTime"/>
    public DateTime GetCreationTime
        (
            string path
        )
    {
        return Directory.GetCreationTime (path);
    }

    /// <inheritdoc cref="IDirectoryIO.GetLastAccessTime"/>
    public DateTime GetLastAccessTime
        (
            string path
        )
    {
        return Directory.GetLastAccessTime (path);
    }

    /// <inheritdoc cref="IDirectoryIO.GetLastWriteTime"/>
    public DateTime GetLastWriteTime
        (
            string path
        )
    {
        return Directory.GetLastWriteTime (path);
    }

    #endregion
}
