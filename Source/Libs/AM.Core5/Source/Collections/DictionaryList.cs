// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* DictionaryList.cs -- hybrid of dictionary and list
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace AM.Collections;

/// <summary>
/// Hybrid of Dictionary and List.
/// </summary>
public class DictionaryList<TKey, TValue>
    : IEnumerable<Pair<TKey, TValue[]>>
    where TKey : notnull
{
    #region Properties

    /// <summary>
    /// Количество ключей, помещенных в словарь.
    /// </summary>
    public int Count
    {
        get
        {
            lock (_syncRoot)
            {
                return _dictionary.Count;
            }
        }
    }

    /// <summary>
    /// Keys.
    /// </summary>
    public TKey[] Keys
    {
        get
        {
            lock (_syncRoot)
            {
                var result = new List<TKey> (_dictionary.Keys);

                return result.ToArray();
            }
        }
    }

    /// <summary>
    /// Array of values for specified key.
    /// </summary>
    public TValue[] this [TKey key]
    {
        get
        {
            lock (_syncRoot)
            {
                var result = GetValues (key);

                return result?.ToArray() ?? Array.Empty<TValue>();
            }
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public DictionaryList()
    {
        _dictionary = new Dictionary<TKey, List<TValue>>();
        _syncRoot = new object();
    }

    #endregion

    #region Private members

    private readonly Dictionary<TKey, List<TValue>> _dictionary;

    private readonly object _syncRoot;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление элемента.
    /// </summary>
    public DictionaryList<TKey, TValue> Add
        (
            TKey key,
            TValue value
        )
    {
        lock (_syncRoot)
        {
            if (!_dictionary.TryGetValue (key, out var list))
            {
                list = new List<TValue>();
                _dictionary.Add (key, list);
            }

            list.Add (value);
        }

        return this;
    }

    /// <summary>
    /// Add some items with one key.
    /// </summary>
    public DictionaryList<TKey, TValue> AddRange
        (
            TKey key,
            IEnumerable<TValue> values
        )
    {
        lock (_syncRoot)
        {
            if (!_dictionary.TryGetValue (key, out var list))
            {
                list = new List<TValue>();
                _dictionary.Add (key, list);
            }

            list.AddRange (values);
        }

        return this;
    }

    /// <summary>
    /// Clear.
    /// </summary>
    public DictionaryList<TKey, TValue> Clear()
    {
        lock (_syncRoot)
        {
            _dictionary.Clear();
        }

        return this;
    }

    /// <summary>
    /// Get values for specified key.
    /// </summary>
    public List<TValue>? GetValues
        (
            TKey key
        )
    {
        lock (_syncRoot)
        {
            _dictionary.TryGetValue (key, out var result);

            return result;
        }
    }

    #endregion

    #region IEnumerable members

    /// <inheritdoc cref="IEnumerable.GetEnumerator" />
    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<Pair<TKey, TValue[]>> GetEnumerator()
    {
        foreach (var key in Keys)
        {
            var pair = new Pair<TKey, TValue[]>
                (
                    key,
                    this[key]
                );

            yield return pair;
        }
    }

    #endregion
}
