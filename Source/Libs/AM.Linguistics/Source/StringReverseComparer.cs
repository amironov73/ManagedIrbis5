// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* StringReverseComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Linguistics;

public class StringReverseComparer<T>
    : IComparer<T>
{
    #region IComparer<T> members

    /// <inheritdoc cref="IComparer{T}.Compare"/>
    public int Compare
        (
            T? obj1,
            T? obj2
        )
    {
        return CompareStrings (obj1?.ToString(), obj2?.ToString());
    }

    #endregion

    public static int CompareStrings
        (
            string? word1,
            string? word2
        )
    {
        if (word1 is null)
        {
            if (word2 is null)
            {
                return 0;
            }

            return -1;
        }

        if (word2 is null)
        {
            return 1;
        }

        var res = 1;
        var l1 = word1.Length;
        var l2 = word2.Length;
        var l = Math.Min (l1, l2);
        for (var i = 1; i <= l; i++)
        {
            var r = word1[l1 - i].CompareTo (word2[l2 - i]);
            if (r != 0)
            {
                return r > 0 ? res : -res;
            }

            res++;
        }

        return Math.Sign (l1.CompareTo (l2)) * res;
    }
}
