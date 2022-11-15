// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* ArrayComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell.Infrastructure;

internal sealed class ArrayComparer<T>
    : IEqualityComparer<T[]>
    where T : IEquatable<T>
{
    public static readonly ArrayComparer<T> Default = new ();

    private static readonly StringComparer StringComparer =
        typeof (T) == typeof (string) ? StringComparer.Ordinal : throw new Exception();

    private static readonly EqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;

    /// <inheritdoc cref="IEqualityComparer{T}.Equals(T?,T?)"/>
    public bool Equals
        (
            T[]? x,
            T[]? y
        )
    {
        if (x is null)
        {
            return y == null;
        }

        if (y is null)
        {
            return false;
        }

        if (ReferenceEquals (x, y))
        {
            return true;
        }

        if (x.Length != y.Length)
        {
            return false;
        }

        if (typeof (T) == typeof (char) || typeof (T) == typeof (FlagValue))
        {
            return x.AsSpan().SequenceEqual (y.AsSpan());
        }

        if (typeof (T) == typeof (string))
        {
            return CompareStrings ((string[])(object)x, (string[])(object)y);
        }

        return CompareAnything (x, y);
    }

    private bool CompareStrings
        (
            IReadOnlyList<string> x,
            IReadOnlyList<string> y
        )
    {
        for (var i = 0; i < x.Count; i++)
        {
            if (!StringComparer.Equals (x[i], y[i]))
            {
                return false;
            }
        }

        return true;
    }

    private bool CompareAnything
        (
            IReadOnlyList<T> x,
            IReadOnlyList<T> y
        )
    {
        for (var i = 0; i < x.Count; i++)
        {
            if (!EqualityComparer.Equals (x[i], y[i]))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode
        (
            T[]? obj
        )
    {
        if (obj == null)
        {
            return 0;
        }

        if (obj.Length == 0)
        {
            return 17;
        }

        unchecked
        {
            var hash = 17 * 31 + obj.Length.GetHashCode();

            hash = hash * 31 + obj[0].GetHashCode();

            if (obj.Length > 1)
            {
                if (obj.Length > 2)
                {
                    hash = hash * 31 + obj[obj.Length / 2].GetHashCode();
                }

                hash = hash * 31 + obj[^1].GetHashCode();
            }

            return hash;
        }
    }
}
