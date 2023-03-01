// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* StringBuilder.AppendJoin.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using AM.Collections;

#endregion

#nullable enable

namespace AM.Buffers.Text;

public partial struct Utf16ValueStringBuilder
{
    /// <summary>
    /// Concatenates the string representations of the elements in the provided array of objects, using the specified char separator between each member, then appends the result to the current instance of the string builder.
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            params T[] values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal<T> (s, values.AsSpan());
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            List<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, (IReadOnlyList<T>)values);
    }

    /// <summary>
    /// /
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            ReadOnlySpan<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values);
    }

    /// <summary>
    /// Concatenates and appends the members of a collection, using the specified char separator between each member.
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            IEnumerable<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            ICollection<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values.AsEnumerable());
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            IList<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            IReadOnlyList<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            IReadOnlyCollection<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values.AsEnumerable());
    }

    /// <summary>
    /// Concatenates the string representations of the elements in the provided array of objects, using the specified separator between each member, then appends the result to the current instance of the string builder.
    /// </summary>
    public void AppendJoin<T> 
        (
            string separator,
            params T[] values
        )
    {
        AppendJoinInternal<T> (separator.AsSpan(), values.AsSpan());
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            List<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), (IReadOnlyList<T>)values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            ReadOnlySpan<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values);
    }

    /// <summary>
    /// Concatenates and appends the members of a collection, using the specified separator between each member.
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            IEnumerable<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            ICollection<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values.AsEnumerable());
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            IList<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            IReadOnlyList<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            IReadOnlyCollection<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values.AsEnumerable());
    }

    internal void AppendJoinInternal<T>
        (
            ReadOnlySpan<char> separator,
            IList<T> values
        )
    {
        var readOnlyList = values as IReadOnlyList<T>;

        // Boxing will occur, but JIT will be de-virtualized.
        readOnlyList = readOnlyList ?? new ReadOnlyListAdaptor<T> (values);
        AppendJoinInternal (separator, readOnlyList);
    }

    internal void AppendJoinInternal<T>
        (
            ReadOnlySpan<char> separator,
            IReadOnlyList<T> values
        )
    {
        var count = values.Count;
        for (var i = 0; i < count; i++)
        {
            if (i != 0)
            {
                Append (separator);
            }

            var item = values[i];
            if (typeof (T) == typeof (string))
            {
                var s = Unsafe.As<string> (item);
                if (!string.IsNullOrEmpty (s))
                {
                    Append (s);
                }
            }
            else
            {
                Append (item);
            }
        }
    }

    internal void AppendJoinInternal<T>
        (
            ReadOnlySpan<char> separator,
            ReadOnlySpan<T> values
        )
    {
        for (var i = 0; i < values.Length; i++)
        {
            if (i != 0)
            {
                Append (separator);
            }

            var item = values[i];
            if (typeof (T) == typeof (string))
            {
                var s = Unsafe.As<string> (item);
                if (!string.IsNullOrEmpty (s))
                {
                    Append (s);
                }
            }
            else
            {
                Append (item);
            }
        }
    }

    internal void AppendJoinInternal<T>
        (
            ReadOnlySpan<char> separator,
            IEnumerable<T> values
        )
    {
        var isFirst = true;
        foreach (var item in values)
        {
            if (!isFirst)
            {
                Append (separator);
            }
            else
            {
                isFirst = false;
            }

            if (typeof (T) == typeof (string))
            {
                var s = Unsafe.As<string> (item);
                if (!string.IsNullOrEmpty (s))
                {
                    Append (s);
                }
            }
            else
            {
                Append (item);
            }
        }
    }
}

public partial struct Utf8ValueStringBuilder
{
    /// <summary>
    /// Concatenates the string representations of the elements in the provided array of objects, using the specified char separator between each member, then appends the result to the current instance of the string builder.
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            params T[] values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal<T> (s, values.AsSpan());
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            List<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, (IReadOnlyList<T>)values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            ReadOnlySpan<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values);
    }

    /// <summary>
    /// Concatenates and appends the members of a collection, using the specified char separator between each member.
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            IEnumerable<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            ICollection<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values.AsEnumerable());
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            IList<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            IReadOnlyList<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            char separator,
            IReadOnlyCollection<T> values
        )
    {
        ReadOnlySpan<char> s = stackalloc char[1] { separator };
        AppendJoinInternal (s, values.AsEnumerable());
    }

    /// <summary>
    /// Concatenates the string representations of the elements in the provided array of objects, using the specified separator between each member, then appends the result to the current instance of the string builder.
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            params T[] values
        )
    {
        AppendJoinInternal<T> (separator.AsSpan(), values.AsSpan());
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            List<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), (IReadOnlyList<T>)values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            ReadOnlySpan<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values);
    }

    /// <summary>
    /// Concatenates and appends the members of a collection, using the specified separator between each member.
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            IEnumerable<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            ICollection<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values.AsEnumerable());
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            IList<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T>
        (
            string separator,
            IReadOnlyList<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values);
    }

    /// <summary>
    /// 
    /// </summary>
    public void AppendJoin<T> 
        (
            string separator,
            IReadOnlyCollection<T> values
        )
    {
        AppendJoinInternal (separator.AsSpan(), values.AsEnumerable());
    }

    internal void AppendJoinInternal<T>
        (
            ReadOnlySpan<char> separator, 
            IList<T> values
        )
    {
        var readOnlyList = values as IReadOnlyList<T>;

        // Boxing will occur, but JIT will be de-virtualized.
        readOnlyList = readOnlyList ?? new ReadOnlyListAdaptor<T> (values);
        AppendJoinInternal (separator, readOnlyList);
    }

    internal void AppendJoinInternal<T>
        (
            ReadOnlySpan<char> separator,
            IReadOnlyList<T> values
        )
    {
        var count = values.Count;
        for (var i = 0; i < count; i++)
        {
            if (i != 0)
            {
                Append (separator);
            }

            var item = values[i];
            if (typeof (T) == typeof (string))
            {
                var s = Unsafe.As<string> (item);
                if (!string.IsNullOrEmpty (s))
                {
                    Append (s);
                }
            }
            else
            {
                Append (item);
            }
        }
    }

    internal void AppendJoinInternal<T>
        (
            ReadOnlySpan<char> separator,
            ReadOnlySpan<T> values
        )
    {
        for (var i = 0; i < values.Length; i++)
        {
            if (i != 0)
            {
                Append (separator);
            }

            var item = values[i];
            if (typeof (T) == typeof (string))
            {
                var s = Unsafe.As<string> (item);
                if (!string.IsNullOrEmpty (s))
                {
                    Append (s);
                }
            }
            else
            {
                Append (item);
            }
        }
    }

    internal void AppendJoinInternal<T>
        (
            ReadOnlySpan<char> separator,
            IEnumerable<T> values
        )
    {
        var isFirst = true;
        foreach (var item in values)
        {
            if (!isFirst)
            {
                Append (separator);
            }
            else
            {
                isFirst = false;
            }

            if (typeof (T) == typeof (string))
            {
                var s = Unsafe.As<string> (item);
                if (!string.IsNullOrEmpty (s))
                {
                    Append (s);
                }
            }
            else
            {
                Append (item);
            }
        }
    }
}
