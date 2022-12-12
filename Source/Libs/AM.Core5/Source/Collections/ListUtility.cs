// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Listtility.cs -- работа со списками List
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Работа со списками <see cref="List{T}"/> и массивами.
/// </summary>
/// <remarks>Borrowed from Json.NET.</remarks>
public static class ListUtility
{
    #region Public methods

    /// <summary>
    /// Добавление в список элемента, при условии,
    /// что этот элемент осутствует в списке.
    /// </summary>
    public static bool AddDistinct<T>
        (
            this IList<T> list,
            T value
        )
    {
        Sure.NotNull (list);

        return list.AddDistinct
            (
                value,
                EqualityComparer<T>.Default
            );
    }

    /// <summary>
    /// Добавление в список элемента, при условии,
    /// что этот элемент осутствует в списке.
    /// </summary>
    public static bool AddDistinct<T>
        (
            this IList<T> list,
            T value,
            IEqualityComparer<T> comparer
        )
    {
        Sure.NotNull (list);
        Sure.NotNull (comparer);

        if (list.ContainsValue (value, comparer))
        {
            return false;
        }

        list.Add (value);

        return true;
    }

    /// <summary>
    /// Добавление в список элемента, при условии,
    /// что этот элемент осутствует в списке.
    /// </summary>
    public static bool AddRangeDistinct<T>
        (
            this IList<T> list,
            IEnumerable<T> values,
            IEqualityComparer<T> comparer
        )
    {
        Sure.NotNull (list);
        Sure.NotNull (values);
        Sure.NotNull (comparer);

        var allAdded = true;
        foreach (var value in values)
        {
            if (!list.AddDistinct (value, comparer))
            {
                allAdded = false;
            }
        }

        return allAdded;
    }

    /// <summary>
    /// Добавление в список элемента, при условии, что он не <c>null</c>.
    /// </summary>
    public static void AddNonNull<T>
        (
            this IList<T> list,
            T? value
        )
        where T: class
    {
        Sure.NotNull (list);

        if (value is not null)
        {
            list.Add (value);
        }
    }

    /// <summary>
    /// Добавление в список элемента, при условии, что он не <c>null</c>.
    /// </summary>
    public static void AddNonNull<T>
        (
            this IList<T> list,
            IEnumerable<T?> values
        )
        where T: class
    {
        Sure.NotNull (list);
        Sure.NotNull (values);

        foreach (var value in values)
        {
            if (value is not null)
            {
                list.Add (value);
            }
        }
    }

    /// <summary>
    /// Бинарный поиск в сортированном списке.
    /// Список должен быть отсортирован согласно
    /// заданной операции сравнения <paramref name="compare"/>.
    /// </summary>
    /// <param name="list">Сортированный список.</param>
    /// <param name="key">Искомый ключ.</param>
    /// <param name="compare">Операция сравнения.</param>
    /// <param name="index">В данную помещается первый индекс, по которому
    /// можно найти искомый ключ. Если значение, возвращенное методом,
    /// равно нулю, это указывает на то, что ключ отсутствует в списке,
    /// то в переменную помещается индекс, в который можно было бы вставить
    /// этот индекс с сохранением сортировки списка.</param>
    /// <returns>Количество ключей, равных заданному <paramref name="key"/>.
    /// Ноль означает, что ключ не найден.
    /// </returns>
    /// <remarks>
    /// Заимствовано из PowerCollections.
    /// </remarks>
    public static int BinarySearch<TItem, TKey>
        (
            IReadOnlyList<TItem> list,
            TKey key,
            Func<TItem, TKey, int> compare,
            out int index
        )
    {
        Sure.NotNull (list);
        Sure.NotNull (compare);

        var l = 0;
        var r = list.Count;
        while (r > l)
        {
            var m = l + (r - l) / 2;
            var middleItem = list[m];
            var comp = compare (middleItem, key);
            if (comp < 0)
            {
                // middleItem < key
                l = m + 1;
            }
            else if (comp > 0)
            {
                r = m;
            }
            else
            {
                // Found something equal to key at m. Now we need to find the start and end of this run of equal keys.
                int lFound = l, rFound = r, found = m;

                // Find the start of the run.
                l = lFound;
                r = found;
                while (r > l)
                {
                    m = l + (r - l) / 2;
                    middleItem = list[m];
                    comp = compare (middleItem, key);
                    if (comp < 0)
                    {
                        // middleItem < key
                        l = m + 1;
                    }
                    else
                    {
                        r = m;
                    }
                }

                Debug.Assert (l == r, "l == r");
                index = l;

                // Find the end of the run.
                l = found;
                r = rFound;
                while (r > l)
                {
                    m = l + (r - l) / 2;
                    middleItem = list[m];
                    comp = compare (middleItem, key);
                    if (comp <= 0)
                    {
                        // middleItem <= key
                        l = m + 1;
                    }
                    else
                    {
                        r = m;
                    }
                }

                Debug.Assert (l == r, "l == r");
                return l - index;
            }
        }

        // We did not find the key. l and r must be equal.
        Debug.Assert (l == r, "l == r");
        index = l;

        return 0;
    }

    /// <summary>
    /// Проверка, не содержит ли список указанного значения.
    /// </summary>
    [Pure]
    public static bool ContainsValue<TSource>
        (
            this IEnumerable<TSource> source,
            TSource value,
            IEqualityComparer<TSource> comparer
        )
    {
        Sure.NotNull (source);
        Sure.NotNull (comparer);

        foreach (var local in source)
        {
            if (comparer.Equals (local, value))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///
    /// </summary>
    public static int IndexOf<T>
        (
            this IEnumerable<T> collection,
            Func<T, bool> predicate
        )
    {
        Sure.NotNull (collection);
        Sure.NotNull (predicate);

        var index = 0;
        foreach (var value in collection)
        {
            if (predicate (value))
            {
                return index;
            }

            index++;
        }

        return -1;
    }

    /// <summary>
    /// Is the list is <c>null</c> or empty?
    /// </summary>
    [Pure]
    public static bool IsNullOrEmpty<T>
        (
            [NotNullWhen ((false))] this IReadOnlyList<T>? list
        )
    {
        if (list is not null)
        {
            return list.Count == 0;
        }

        return true;
    }

    /// <summary>
    /// Removes items from the list that match the specified predicate.
    /// </summary>
    /// <typeparam name="T">The type.</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="predicate">The predicate that determines the items to remove.</param>
    /// <returns>The number of items removed from the collection.</returns>
    public static int RemoveWhere<T>
        (
            this IList<T> list,
            Func<T, bool> predicate
        )
    {
        Sure.NotNull (list);
        Sure.NotNull (predicate);

        var originalCount = list.Count;
        var count = originalCount;
        var index = 0;
        while (index < count)
        {
            if (predicate (list[index]))
            {
                list.RemoveAt (index);
                count--;
            }
            else
            {
                index++;
            }
        }

        return originalCount - count;
    }

    /// <summary>
    /// Throw <see cref="ArgumentNullException"/>
    /// if the list is <c>null</c> or empty.
    /// </summary>
    public static IList<T> ThrowIfNullOrEmpty<T>
        (
            this IList<T>? list
        )
    {
        if (ReferenceEquals (list, null))
        {
            Magna.Logger.LogError
                (
                    nameof (ListUtility)
                    + "::"
                    + nameof (ThrowIfNullOrEmpty)
                    + ": list is null"
                );

            throw new ArgumentNullException();
        }

        if (list.Count == 0)
        {
            Magna.Logger.LogError
                (
                    nameof (ListUtility)
                    + "::"
                    + nameof (ThrowIfNullOrEmpty)
                    + ": list is empty"
                );

            throw new ArgumentException();
        }

        return list;
    }

    /// <summary>
    /// Throw <see cref="ArgumentNullException"/>
    /// if the list is <c>null</c> or empty.
    /// </summary>
    [Pure]
    public static IList<T> ThrowIfNullOrEmpty<T>
        (
            this IList<T>? list,
            string message
        )
    {
        if (ReferenceEquals (list, null))
        {
            Magna.Logger.LogError
                (
                    nameof (ListUtility)
                    + "::"
                    + nameof (ThrowIfNullOrEmpty)
                    + ": list is null"
                );

            throw new ArgumentNullException (message);
        }

        if (list.Count == 0)
        {
            Magna.Logger.LogError
                (
                    nameof (ListUtility)
                    + "::"
                    + nameof (ThrowIfNullOrEmpty)
                    + ": list is empty"
                );

            throw new ArgumentException (message);
        }

        return list;
    }

    /// <summary>
    /// Throw <see cref="ArgumentNullException"/>
    /// if the array is <c>null</c> or empty.
    /// </summary>
    [Pure]
    public static T[] ThrowIfNullOrEmpty<T>
        (
            this T[]? array
        )
    {
        if (ReferenceEquals (array, null))
        {
            Magna.Logger.LogError
                (
                    nameof (ListUtility)
                    + "::"
                    + nameof (ThrowIfNullOrEmpty)
                    + ": array is null"
                );

            throw new ArgumentNullException();
        }

        if (array.Length == 0)
        {
            Magna.Logger.LogError
                (
                    nameof (ListUtility)
                    + "::"
                    + nameof (ThrowIfNullOrEmpty)
                    + ": array is empty"
                );

            throw new ArgumentException();
        }

        return array;
    }

    /// <summary>
    /// Throw <see cref="ArgumentNullException"/>
    /// if the array is <c>null</c> or empty.
    /// </summary>
    [Pure]
    public static T[] ThrowIfNullOrEmpty<T>
        (
            this T[]? array,
            string message
        )
    {
        if (ReferenceEquals (array, null))
        {
            Magna.Logger.LogError
                (
                    nameof (ListUtility)
                    + "::"
                    + nameof (ThrowIfNullOrEmpty)
                    + ": array is null"
                );

            throw new ArgumentNullException (message);
        }

        if (array.Length == 0)
        {
            Magna.Logger.LogError
                (
                    nameof (ListUtility)
                    + "::"
                    + nameof (ThrowIfNullOrEmpty)
                    + ": array is empty"
                );

            throw new ArgumentException (message);
        }

        return array;
    }

    #endregion
}
