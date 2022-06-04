// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MemoryUtility.cs -- полезные методы для работы с памятью
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM.Memory;

/// <summary>
/// Полезные методы для работы с памятью.
/// </summary>
internal static class MemoryUtility
{
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    internal static int GetMaxSizeForBucket (int binIndex)
    {
        return 16 << binIndex;
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    internal static int GetBucket
        (
            int size
        )
    {
        if (size == 128 /*default chunk size*/)
        {
            return 7;
        }

        size--;
        var length = 0;
        while (size >= 16)
        {
            length++;
            size = size >> 1;
        }

        return length;
    }
}
