// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CaseInsensitiveSortedList.cs -- сортированный список, нечувствительный к регистру символов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Сортированный список, нечувствительный к регистру символов.
/// </summary>
public class CaseInsensitiveSortedList<T>
    : SortedList<string, T>
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public CaseInsensitiveSortedList()
        : base (_GetComparer())
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CaseInsensitiveSortedList
        (
            int capacity
        )
        : base (capacity, _GetComparer())
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CaseInsensitiveSortedList
        (
            IDictionary<string, T> dictionary
        )
        : base (dictionary, _GetComparer())
    {
    }

    #endregion

    #region Private members

    /// <summary>
    /// Получение компарера для списка.
    /// </summary>
    private static IComparer<string> _GetComparer()
    {
        return StringComparer.OrdinalIgnoreCase;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Поиск наиболее похожего ключа (в том числе и полного совпадения).
    /// </summary>
    public (int Index, string Value) Search
        (
            string key
        )
    {
        var keys = Keys;
        if (keys.Count == 0)
        {
            return (-1, string.Empty);
        }

        // ВНИМАНИЕ: это похоже на BinarySearch,
        // но возвращает предшествующий искомому элемент,
        // если точного совпадения не было найдено

        var first = 0;
        var last = Count - 1;

        while (first < last)
        {
            var middle = first + ((last - first) >> 1);
            var value = keys[middle];
            var order = Comparer.Compare (value, key);
            if (order == 0)
            {
                return (middle, value);
            }

            if (order < 0)
            {
                first = middle + 1;
            }
            else
            {
                last = middle - 1;
            }
        }

        return (first, keys[first]);
    }

    #endregion
}
