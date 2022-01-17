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

/* IDirectoryIO.cs -- абстракция доступа к директории
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Абстракция доступа к директории.
/// </summary>
public interface IDirectoryIO
{
    /// <summary>
    /// Создание директории.
    /// </summary>
    void CreateDirectory (string path);

    /// <summary>
    /// Удаление директории.
    /// </summary>
    void DeleteDirectory (string path, bool recursive = true);

    /// <summary>
    /// Получение родительской директории (если есть).
    /// </summary>
    string? GetParentDirectory (string path);

    /// <summary>
    /// Получение времени создания директории.
    /// </summary>
    DateTime GetCreationTime (string path);

    /// <summary>
    /// Получение времени последнего доступа к директории.
    /// </summary>
    DateTime GetLastAccessTime (string path);

    /// <summary>
    /// Получение времени последней записи в директорию.
    /// </summary>
    DateTime GetLastWriteTime (string path);

}
