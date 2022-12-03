// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DictionarySlim.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.Slim;

/// <summary>
///
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public partial class DictionarySlim<TKey, TValue>
    : IDictionary<TKey, TValue>
    where TKey: notnull
{
    private Map<TKey, TValue> _map = Map<TKey, TValue>.Empty;
    private IEqualityComparer<TKey> _comparer;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DictionarySlim()
        : this (null)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="comparer"></param>
    public DictionarySlim
        (
            IEqualityComparer<TKey>? comparer
        )
    {
        // TODO: Propagate
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
    }

    /// <inheritdoc cref="IDictionary{TKey,TValue}.this"/>
    public TValue this [TKey key]
    {
        get => _map.TryGetValue (key, out var value) ? value : throw new KeyNotFoundException();
        set => _map = _map.Set (key, value);
    }

    /// <inheritdoc cref="IDictionary{TKey,TValue}.Keys"/>
    public ICollection<TKey> Keys => _map.Keys;

    /// <inheritdoc cref="IDictionary{TKey,TValue}.Values"/>
    public ICollection<TValue> Values => _map.Values;

    /// <inheritdoc cref="ICollection{T}.Count"/>
    public int Count => _map.Count;

    /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
    public bool IsReadOnly => false;

    /// <inheritdoc cref="IDictionary{TKey,TValue}.Add(TKey,TValue)"/>
    public void Add (TKey key, TValue value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICollection{T}.Add"/>
    public void Add (KeyValuePair<TKey, TValue> item)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICollection{T}.Clear"/>
    public void Clear() => _map = Map<TKey, TValue>.Empty;

    /// <inheritdoc cref="IDictionary{TKey,TValue}.ContainsKey"/>
    public bool ContainsKey (TKey key) => _map.TryGetValue (key, out _);

    /// <inheritdoc cref="IDictionary{TKey,TValue}.TryGetValue"/>
    public bool TryGetValue (TKey key, out TValue value) => _map.TryGetValue (key, out value);

    /// <inheritdoc cref="IDictionary{TKey,TValue}.Remove(TKey)"/>
    public bool Remove
        (
            TKey key
        )
    {
        _map = _map.TryRemove (key, out var success);
        return success;
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public Enumerator GetEnumerator() => new Enumerator (_map);

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() =>
        new Enumerator (_map);

    IEnumerator IEnumerable.GetEnumerator() => new Enumerator (_map);

    /// <inheritdoc cref="ICollection{T}.CopyTo"/>
    public void CopyTo
        (
            KeyValuePair<TKey, TValue>[] array,
            int arrayIndex
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICollection{T}.Contains"/>
    public bool Contains
        (
            KeyValuePair<TKey, TValue> item
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="ICollection{T}.Remove"/>
    public bool Remove
        (
            KeyValuePair<TKey, TValue> item
        )
    {
        throw new NotImplementedException();
    }
}
