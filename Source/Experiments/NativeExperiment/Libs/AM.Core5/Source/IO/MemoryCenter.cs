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

/* MemoryCenter.cs -- центр управления и экономии памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using Microsoft.IO;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Центр управления и экономии памяти.
/// </summary>
public static class MemoryCenter
{
    #region Private members

    private const string _tag = "ArsMagna";

    private static readonly RecyclableMemoryStreamManager _manager = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Получение потока памяти.
    /// </summary>
    public static MemoryStream GetMemoryStream()
    {
        return _manager.GetStream (_tag);
    }

    /// <summary>
    /// Получение потока памяти заданного размера.
    /// </summary>
    public static MemoryStream GetMemoryStream
        (
            int initialSize
        )
    {
        return initialSize > 0
            ? _manager.GetStream (_tag, initialSize)
            : _manager.GetStream (_tag);
    }

    #endregion
}
