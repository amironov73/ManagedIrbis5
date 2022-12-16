// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BinarySearchExtension.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit.Utils;

/// <summary>
/// Extension methods for binary searching an IReadOnlyList collection
/// </summary>
public static class BinarySearchExtension
{
    private static int GetMedian (int low, int hi)
    {
        System.Diagnostics.Debug.Assert (low <= hi);
        System.Diagnostics.Debug.Assert (hi - low >= 0, "Length overflow!");
        return low + ((hi - low) >> 1);
    }

    /// <summary>
    /// Performs a binary search on the entire contents of an IReadOnlyList
    /// </summary>
    /// <typeparam name="T">The list element type</typeparam>
    /// <param name="list">The list to be searched</param>
    /// <param name="value">The value to search for</param>
    /// <returns>The index of the found item; otherwise the bitwise complement of the index of the next larger item</returns>
    public static int BinarySearch<T> (this IReadOnlyList<T> list, T value) where T : IComparable
    {
        return list.BinarySearch (value, (a, b) => a.CompareTo (b));
    }


    /// <summary>
    /// Performs a binary search on the entire contents of an IReadOnlyList
    /// </summary>
    /// <typeparam name="T">The list element type</typeparam>
    /// <typeparam name="U">The value type being searched for</typeparam>
    /// <param name="list">The list to be searched</param>
    /// <param name="value">The value to search for</param>
    /// <param name="compare">A comparison function</param>
    /// <returns>The index of the found item; otherwise the bitwise complement of the index of the next larger item</returns>
    public static int BinarySearch<T, U> (this IReadOnlyList<T> list, U value, Func<T, U, int> compare)
    {
        return BinarySearch (list, 0, list.Count, value, compare);
    }

    /// <summary>
    /// Performs a binary search on a a subset of an IReadOnlyList
    /// </summary>
    /// <typeparam name="T">The list element type</typeparam>
    /// <typeparam name="U">The value type being searched for</typeparam>
    /// <param name="list">The list to be searched</param>
    /// <param name="index">The start of the range to be searched</param>
    /// <param name="length">The length of the range to be searched</param>
    /// <param name="value">The value to search for</param>
    /// <param name="compare">A comparison function</param>
    /// <returns>The index of the found item; otherwise the bitwise complement of the index of the next larger item</returns>
    public static int BinarySearch<T, U> (this IReadOnlyList<T> list, int index, int length, U value,
        Func<T, U, int> compare)
    {
        // Based on this: https://referencesource.microsoft.com/#mscorlib/system/array.cs,957
        var lo = index;
        var hi = index + length - 1;
        while (lo <= hi)
        {
            var i = GetMedian (lo, hi);
            var c = compare (list[i], value);
            if (c == 0)
            {
                return i;
            }

            if (c < 0)
            {
                lo = i + 1;
            }
            else
            {
                hi = i - 1;
            }
        }

        return ~lo;
    }
}
