// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UnsafeUtility.cs -- возня вокруг unsafe
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

using static System.Runtime.InteropServices.MemoryMarshal;

#endregion

namespace AM;

/// <summary>
/// Возня вокруг unsafe.
/// </summary>
public static class UnsafeUtility
{
    #region Public methods

    /// <summary>
    /// Превращаем ссылку на структуру в спан.
    /// </summary>
    public static Span<byte> AsSpan<T>
        (
            ref T value
        )
        where T: struct
    {
        var size = Unsafe.SizeOf<T>();

        return AsBytes (CreateSpan (ref value, size));
    }

    #endregion
}
