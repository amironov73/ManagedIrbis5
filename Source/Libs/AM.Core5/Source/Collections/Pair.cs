// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Pair.cs -- holds pair of objects of given types
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Simple container that holds pair of objects of given types.
/// </summary>
/// <typeparam name="T1">Type of first object.</typeparam>
/// <typeparam name="T2">Type of second object.</typeparam>
[DebuggerDisplay("{First};{Second}")]
//[TypeConverter(typeof(IndexableConverter))]
public class Pair<T1, T2>
    : IList,
    IIndexable<object>,
    IReadOnly<Pair<T1, T2>>
{
    #region Properties

    private T1? _first;

    /// <summary>
    /// First element of the pair.
    /// </summary>
    /// <value>Value of first element.</value>
    [XmlElement("first")]
    [JsonPropertyName("first")]
    public T1? First
    {
        get => _first;
        [DebuggerStepThrough]
        set
        {
            ThrowIfReadOnly();
            _first = value;
        }
    }

    private T2? _second;

    /// <summary>
    /// Second element of the pair.
    /// </summary>
    /// <value>Value of second element.</value>
    [XmlElement("second")]
    [JsonPropertyName("second")]
    public T2? Second
    {
        get => _second;
        [DebuggerStepThrough]
        set
        {
            ThrowIfReadOnly();
            _second = value;
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public Pair()
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public Pair
        (
            Pair<T1, T2> pair
        )
    {
        First = pair.First;
        Second = pair.Second;
        _isReadOnly = pair._isReadOnly;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public Pair
        (
            T1? first
        )
    {
        First = first;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public Pair
        (
            T1? first,
            T2? second
        )
    {
        First = first;
        Second = second;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public Pair
        (
            T1? first,
            T2? second,
            bool readOnly
        )
    {
        First = first;
        Second = second;
        _isReadOnly = readOnly;
    }

    #endregion

    #region Public methods

    #endregion

    #region IList members

    ///<inheritdoc cref="IList.Add" />
    int IList.Add
        (
            object? value
        )
    {
        Magna.Logger.LogError
            (
                nameof (Pair<T1, T2>) + "::Add"
                + ": not supported"
            );

        throw new NotSupportedException();
    }

    ///<inheritdoc cref="IList.Contains" />
    bool IList.Contains
        (
            object? value
        )
    {
        foreach (object o in this)
        {
            if (!ReferenceEquals(o, null)
                && o.Equals(value))
            {
                return true;
            }
        }

        return false;
    }

    ///<inheritdoc cref="IList.Clear" />
    void IList.Clear()
    {
        Magna.Logger.LogError
            (
                nameof (Pair<T1, T2>) + "::Clear"
                + ": not supported"
            );

        throw new NotSupportedException();
    }

    ///<inheritdoc cref="IList.IndexOf" />
    int IList.IndexOf
        (
            object? value
        )
    {
        var index = 0;
        foreach (var o in this)
        {
            if (!ReferenceEquals(o, null))
            {
                if (o.Equals(value))
                {
                    return index;
                }
            }
            index++;
        }

        return -1;
    }

    ///<inheritdoc cref="IList.Insert" />
    void IList.Insert
        (
            int index,
            object? value
        )
    {
        Magna.Logger.LogError
            (
                nameof (Pair<T1, T2>) + "::Insert"
                + ": not supported"
            );

        throw new NotSupportedException();
    }

    ///<inheritdoc cref="IList.Remove" />
    void IList.Remove
        (
            object? value
        )
    {
        Magna.Logger.LogError
            (
                nameof (Pair<T1, T2>) + "::Remove"
                + ": not supported"
            );

        throw new NotSupportedException();
    }

    ///<inheritdoc cref="IList.RemoveAt" />
    void IList.RemoveAt
        (
            int index
        )
    {
        Magna.Logger.LogError
            (
                nameof (Pair<T1, T2>) + "::RemoveAt"
                + ": not supported"
            );

        throw new NotSupportedException();
    }

    /// <inheritdoc cref="IList.this"/>
    public object? this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return First;

                case 1:
                    return Second;

                default:
                    Magna.Logger.LogError
                        (
                            nameof (Pair<T1, T2>) + "::Indexer"
                            + "index={Index} is out of range",
                            index
                        );

                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
        set
        {
            if (_isReadOnly)
            {
                Magna.Logger.LogError
                    (
                        nameof (Pair<T1, T2>) + "::Indexer"
                        + ": is read-only"
                    );

                throw new NotSupportedException();
            }

            switch (index)
            {
                case 0:
                    First = (T1?)value;
                    break;

                case 1:
                    Second = (T2?)value;
                    break;

                default:
                    Magna.Logger.LogError
                        (
                            nameof (Pair<T1, T2>) + "::Indexer"
                            + ": index={Index} is out of range",
                            index
                        );

                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }
    }

    private bool _isReadOnly;

    /// <inheritdoc cref="IList.IsReadOnly" />
    bool IList.IsReadOnly => _isReadOnly;

    /// <inheritdoc cref="IList.IsFixedSize" />
    bool IList.IsFixedSize => true;

    /// <inheritdoc cref="ICollection.CopyTo" />
    void ICollection.CopyTo
        (
            Array array,
            int index
        )
    {
        array.SetValue(First, index);
        array.SetValue(Second, index + 1);
    }

    /// <inheritdoc cref="ICollection.Count" />
    public int Count => 2;

    /// <inheritdoc cref="ICollection.SyncRoot" />
    object ICollection.SyncRoot => this;

    /// <inheritdoc cref="ICollection.IsSynchronized" />
    bool ICollection.IsSynchronized => false;

    /// <inheritdoc cref="IEnumerable.GetEnumerator" />
    IEnumerator IEnumerable.GetEnumerator()
    {
        yield return First;
        yield return Second;
    }

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public object Clone()
    {
        return new Pair<T1, T2>
            (
                First,
                Second,
                ReadOnly
            );
    }

    #endregion

    #region IReadOnly<T> members

    /// <inheritdoc cref="IReadOnly{T}.ReadOnly" />
    public bool ReadOnly => _isReadOnly;

    /// <inheritdoc cref="IReadOnly{T}.AsReadOnly" />
    public Pair<T1, T2> AsReadOnly()
    {
        return new Pair<T1, T2>(First, Second, true);
    }

    /// <inheritdoc cref="IReadOnly{T}.SetReadOnly" />
    public void SetReadOnly()
    {
        _isReadOnly = true;
    }

    /// <inheritdoc cref="IReadOnly{T}.ThrowIfReadOnly" />
    public void ThrowIfReadOnly()
    {
        if (ReadOnly)
        {
            Magna.Logger.LogError
                (
                    nameof (Pair<T1, T2>) + "::" + nameof(ThrowIfReadOnly)
                );

            throw new ReadOnlyException();
        }
    }

    #endregion

    #region IEquatable<T> members

    ///<inheritdoc cref="IEquatable{T}.Equals(T)" />
    protected bool Equals
        (
            Pair<T1, T2> other
        )
    {
        return EqualityComparer<T1>.Default.Equals(_first, other._first)
               && EqualityComparer<T2>.Default.Equals(_second, other._second);
    }

    #endregion

    #region Object members

    ///<inheritdoc cref="object.Equals(object)" />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((Pair<T1, T2>)obj);
    }

    ///<inheritdoc cref="object.GetHashCode" />
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyFieldInGetHashCode
        unchecked
        {
            var first = _first is null ? 0 : EqualityComparer<T1>.Default.GetHashCode(_first);
            var second = _second is null ? 0 : EqualityComparer<T2>.Default.GetHashCode(_second);

            return (first * 397) ^ second;
        }
        // ReSharper restore NonReadonlyFieldInGetHashCode
    }

    ///<inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var first = _first is null ? "(null)" : _first.ToString();
        var second = _second is null ? "(null)" : _second.ToString();

        return first + ";" + second;
    }

    #endregion
}
