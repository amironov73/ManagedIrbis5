// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Cache.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags;

internal class Cache<TKey, TValue>
    : IEnumerable<TValue>
    where TKey: notnull
    where TValue: class
{
    #region Properties

    public Func<TKey, TValue> OnMissing
    {
        set => _onMissing = value;
    }

    public Func<TValue, TKey> GetKey { get; set; } = _ => throw new NotImplementedException();

    public int Count => _values.Count;

    public TValue? First => _values.Select (pair => pair.Value).FirstOrDefault();

    public IDictionary<TKey, TValue> Inner => _values;

    public TValue this [TKey key]
    {
        get
        {
            if (!_values.ContainsKey (key))
            {
                lock (_locker)
                {
                    if (!_values.ContainsKey (key))
                    {
                        TValue value = _onMissing (key);
                        _values.Add (key, value);
                    }
                }
            }

            return _values[key];
        }
        set
        {
            if (_values.ContainsKey (key))
            {
                _values[key] = value;
            }
            else
            {
                _values.Add (key, value);
            }
        }
    }

    #endregion

    #region Construction

    public Cache()
        : this (new Dictionary<TKey, TValue>())
    {
        // пустое тело конструктора
    }

    public Cache
        (
            Func<TKey, TValue> onMissing
        )
        : this (new Dictionary<TKey, TValue>(), onMissing)
    {
        // пустое тело конструктора
    }

    public Cache
        (
            IDictionary<TKey, TValue> dictionary,
            Func<TKey, TValue> onMissing
        )
        : this (dictionary)
    {
        Sure.NotNull (onMissing);

        _onMissing = onMissing;
    }

    public Cache
        (
            IDictionary<TKey, TValue> dictionary
        )
    {
        Sure.NotNull (dictionary);

        _values = dictionary;
    }

    #endregion

    #region Private members

    private readonly object _locker = new ();
    private readonly IDictionary<TKey, TValue> _values;

    private Func<TKey, TValue> _onMissing = key =>
    {
        var message = $"Key '{key}' could not be found";
        throw new KeyNotFoundException (message);
    };

    #endregion

    #region IEnumerable<TValue> members

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TValue>)this).GetEnumerator();

    public IEnumerator<TValue> GetEnumerator() => _values.Values.GetEnumerator();

    #endregion

    #region Public methods

    public IEnumerable<TKey> GetKeys() => _values.Keys;

    public void Fill (TKey key, TValue value)
    {
        if (_values.ContainsKey (key))
        {
            return;
        }

        _values.Add (key, value);
    }

    public bool TryRetrieve (TKey key, out TValue? value)
    {
        value = default;

        if (_values.ContainsKey (key))
        {
            value = _values[key];
            return true;
        }

        return false;
    }

    public void Each
        (
            Action<TValue> action
        )
    {
        Sure.NotNull (action);

        foreach (var pair in _values)
        {
            action (pair.Value);
        }
    }

    public void Each
        (
            Action<TKey, TValue> action
        )
    {
        foreach (var pair in _values)
        {
            action (pair.Key, pair.Value);
        }
    }

    public bool Has
        (
            TKey key
        )
    {
        return _values.ContainsKey (key);
    }

    public bool Exists
        (
            Predicate<TValue> predicate
        )
    {
        Sure.NotNull (predicate);

        var returnValue = false;

        Each (value => returnValue |= predicate (value));

        return returnValue;
    }

    public TValue? Find
        (
            Predicate<TValue> predicate
        )
    {
        Sure.NotNull (predicate);

        return _values.Where (pair => predicate (pair.Value))
            .Select (pair => pair.Value).FirstOrDefault();
    }

    public TValue[] GetAll()
    {
        var returnValue = new TValue[Count];
        _values.Values.CopyTo (returnValue, 0);

        return returnValue;
    }

    public void Remove (TKey key)
    {
        if (_values.ContainsKey (key))
        {
            _values.Remove (key);
        }
    }

    public void ClearAll() => _values.Clear();

    #endregion
}
