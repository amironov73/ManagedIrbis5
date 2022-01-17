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

/* NullDirectoryIO.cs -- нулевой доступ к директории
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Нулевой доступ к директории (например, для тестирования).
/// </summary>
public sealed class NullDirectoryIO
    : IDirectoryIO
{
    #region IDirectoryIO members

    /// <inheritdoc cref="CreateDirectory"/>
    public void CreateDirectory
        (
            string path
        )
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IDirectoryIO.DeleteDirectory"/>
    public void DeleteDirectory
        (
            string path,
            bool recursive = true
        )
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IDirectoryIO.GetParentDirectory"/>
    public string? GetParentDirectory
        (
            string path
        )
    {
        return null;
    }

    /// <inheritdoc cref="IDirectoryIO.GetCreationTime"/>
    public DateTime GetCreationTime
        (
            string path
        )
    {
        return DateTime.MinValue;
    }

    /// <inheritdoc cref="IDirectoryIO.GetLastAccessTime"/>
    public DateTime GetLastAccessTime
        (
            string path
        )
    {
        return DateTime.MinValue;
    }

    /// <inheritdoc cref="IDirectoryIO.GetLastWriteTime"/>
    public DateTime GetLastWriteTime
        (
            string path
        )
    {
        return DateTime.MinValue;
    }

    #endregion
}
