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

/* DictionaryCounter.cs -- простой словарь-счетчик
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Простой словарь-счетчик.
/// </summary>
public class DictionaryCounter<TKey, TValue>
    : Dictionary<TKey, TValue>
    where TKey : notnull
    where TValue : INumber<TValue>
{
    #region Properties

    /// <summary>
    /// Gets the total.
    /// </summary>
    public TValue Total
    {
        get
        {
            lock (SyncRoot)
            {
                var result = TValue.Zero;
                foreach (var value in Values)
                {
                    result += value;
                }

                return result;
            }
        }
    }

    /// <summary>
    /// Сортированные ключи.
    /// </summary>
    public TKey[] SortedKeys
    {
        get
        {
            var result = Keys.ToArray();
            Array.Sort (result);

            return result;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public DictionaryCounter()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="comparer">Сравнение для элементов.</param>
    public DictionaryCounter
        (
            IEqualityComparer<TKey> comparer
        )
        : base (comparer.ThrowIfNull())
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="capacity">Начальная емкость.</param>
    public DictionaryCounter
        (
            int capacity
        )
        : base (capacity)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="dictionary">Начальное наполнение словаря.</param>
    public DictionaryCounter
        (
            DictionaryCounter<TKey, TValue> dictionary
        )
        : base (dictionary)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Private members

    private object SyncRoot => ((ICollection)this).SyncRoot;

    #endregion

    #region Public methods

    /// <summary>
    /// Augments the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="increment">The value.</param>
    /// <returns>New value for given key.</returns>
    public TValue Augment
        (
            TKey key,
            TValue increment
        )
    {
        lock (SyncRoot)
        {
            if (TryGetValue (key, out var value))
            {
                value += increment;
            }
            else
            {
                value = increment;
            }

            this[key] = value;

            return value;
        }
    }

    /// <summary>
    /// Get accumulated value for the specified key.
    /// </summary>
    public TValue GetValue
        (
            TKey key
        )
    {
        lock (SyncRoot)
        {
            if (!TryGetValue (key, out var result))
            {
                result = TValue.Zero;
            }

            return result;
        }
    }

    /// <summary>
    /// Increment the specified key.
    /// </summary>
    public TValue Increment
        (
            TKey key
        )
    {
        return Augment (key, TValue.One);
    }

    #endregion
}
