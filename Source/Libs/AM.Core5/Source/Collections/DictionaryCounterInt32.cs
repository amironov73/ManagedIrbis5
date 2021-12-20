// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DictionaryCounterInt32.cs -- простой словарь-счетчик с 32-битными целыми числами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Простой словарь-счетчик с 32-битными целыми числами.
/// </summary>
public sealed class DictionaryCounterInt32<TKey>
    : Dictionary<TKey, int>
    where TKey : notnull
{
    #region Properties

    /// <summary>
    /// Gets the total.
    /// </summary>
    public int Total
    {
        get
        {
            lock (SyncRoot)
            {
                var result = 0;
                foreach (var value in Values)
                {
                    result += value;
                }

                return result;
            }
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public DictionaryCounterInt32()
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="comparer">The comparer.</param>
    public DictionaryCounterInt32
        (
            IEqualityComparer<TKey> comparer
        )
        : base (comparer)
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="capacity">The capacity.</param>
    public DictionaryCounterInt32
        (
            int capacity
        )
        : base (capacity)
    {
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="DictionaryCounterInt32{TKey}"/> class.
    /// </summary>
    /// <param name="dictionary">The dictionary.</param>
    public DictionaryCounterInt32
        (
            DictionaryCounterInt32<TKey> dictionary
        )
        : base (dictionary)
    {
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
    public int Augment
        (
            TKey key,
            int increment
        )
    {
        lock (SyncRoot)
        {
            TryGetValue (key, out int value);
            value += increment;
            this[key] = value;

            return value;
        }
    }

    /// <summary>
    /// Get accumulated value for the specified key.
    /// </summary>
    public int GetValue
        (
            TKey key
        )
    {
        lock (SyncRoot)
        {
            TryGetValue (key, out int result);

            return result;
        }
    }

    /// <summary>
    /// Increment the specified key.
    /// </summary>
    public int Increment
        (
            TKey key
        )
    {
        return Augment (key, 1);
    }

    #endregion
}
