// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

#pragma warning disable 8618

/* VirtualList.cs -- виртуальный список
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Виртуальный список.
/// </summary>
public sealed class VirtualList<T>
    : IList<T>
{
    #region Nested classes

    /// <summary>
    /// Параметры.
    /// </summary>
    public sealed class Parameters
    {
        #region Properties

        /// <summary>
        /// List.
        /// </summary>
        public VirtualList<T> List { get; }

        /// <summary>
        /// Index.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Up direction.
        /// </summary>
        public bool Up { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Parameters
            (
                VirtualList<T> list,
                int index,
                bool up
            )
        {
            List = list;
            Index = index;
            Up = up;
        }

        #endregion
    }

    #endregion

    #region Properties

    /// <summary>
    /// Cache size.
    /// </summary>
    public int CacheSize { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public VirtualList
        (
            Action<Parameters> retriever,
            int count,
            int cacheSize
        )
    {
        Sure.Positive (count);
        Sure.Positive (cacheSize);

        if (cacheSize > count)
        {
            cacheSize = count;
        }

        _retriever = retriever;
        Count = count;
        CacheSize = cacheSize;

        _cache = null;
        _cacheIndex = 0;
        _cacheLength = 0;
    }

    #endregion

    #region Private members

    private readonly Action<Parameters> _retriever;

    private T[]? _cache;

    private int _cacheIndex;

    private int _cacheLength;

    private void _ThrowReadonly()
    {
        Magna.Logger.LogError (nameof (VirtualList<T>) + "::" + nameof (_ThrowReadonly));
        throw new ReadOnlyException();
    }

    private void _Retrieve
        (
            int index
        )
    {
        if (index >= _cacheIndex
            && index < _cacheIndex + _cacheLength)
        {
            return;
        }

        var parameters = new Parameters
            (
                this,
                index,
                false
            );
        _retriever (parameters);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Get item by index.
    /// </summary>
    public T? GetItem
        (
            int index
        )
    {
        Sure.NonNegative (index);

        _Retrieve (index);
        var result = default (T);
        if (!ReferenceEquals (_cache, null)
            && index >= _cacheIndex
            && index <= _cacheIndex + _cacheLength)
        {
            result = _cache[index - _cacheIndex];
        }

        return result;
    }

    /// <summary>
    /// Set cache (called by retriever);
    /// </summary>
    public void SetCache
        (
            T[] cache,
            int index
        )
    {
        Sure.Positive (cache.Length);
        Sure.NonNegative (index);

        _cache = cache;
        _cacheIndex = index;
        _cacheLength = cache.Length;
        var end = _cacheIndex + _cacheLength;
        if (end > Count)
        {
            Count = end;
        }
    }

    #endregion

    #region IList<T> members

    /// <inheritdoc cref="ICollection{T}.Count" />
    public int Count { get; private set; }

    /// <inheritdoc cref="ICollection{T}.Add" />
    [ExcludeFromCodeCoverage]
    void ICollection<T>.Add (T item)
    {
        _ThrowReadonly();
    }

    /// <inheritdoc cref="ICollection{T}.Clear" />
    [ExcludeFromCodeCoverage]
    void ICollection<T>.Clear()
    {
        _ThrowReadonly();
    }

    /// <inheritdoc cref="ICollection{T}.Contains" />
    public bool Contains
        (
            T item
        )
    {
        if (ReferenceEquals (_cache, null))
        {
            GetItem (0);
        }

        if (ReferenceEquals (_cache, null))
        {
            return false;
        }

        foreach (var i in _cache)
        {
            if (EqualityComparer<T>.Default.Equals
                    (
                        item,
                        i
                    ))
            {
                return true;
            }
        }

        return false;
    }

    /// <inheritdoc cref="ICollection{T}.CopyTo" />
    [ExcludeFromCodeCoverage]
    void ICollection<T>.CopyTo
        (
            T[] array,
            int arrayIndex
        )
    {
        foreach (var item in this)
        {
            array.SetValue (item, arrayIndex++);
        }
    }

    /// <inheritdoc cref="IEnumerable.GetEnumerator" />
    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
    public IEnumerator<T> GetEnumerator()
    {
        if (ReferenceEquals (_cache, null))
        {
            GetItem (0);
        }

        //if (ReferenceEquals(_cache, null))
        //{
        //    throw new InvalidOperationException();
        //}

        foreach (var item in _cache!)
        {
            yield return item;
        }
    }

    /// <inheritdoc cref="ICollection{T}.IsReadOnly" />
    public bool IsReadOnly => true;

    /// <inheritdoc cref="IList{T}.IndexOf" />
    public int IndexOf (T item)
    {
        if (ReferenceEquals (_cache, null))
        {
            GetItem (0);
        }

        if (ReferenceEquals (_cache, null))
        {
            return -1;
        }

        for (var i = 0; i < _cache.Length; i++)
        {
            if (EqualityComparer<T>.Default.Equals
                    (
                        item,
                        _cache[i]
                    ))
            {
                return _cacheIndex + i;
            }
        }

        return -1;
    }

    /// <inheritdoc cref="IList{T}.Insert" />
    [ExcludeFromCodeCoverage]
    void IList<T>.Insert (int index, T item)
    {
        _ThrowReadonly();
    }

    /// <inheritdoc cref="ICollection{T}.Remove" />
    [ExcludeFromCodeCoverage]
    bool ICollection<T>.Remove (T item)
    {
        _ThrowReadonly();

        return false;
    }

    /// <inheritdoc cref="IList{T}.RemoveAt" />
    [ExcludeFromCodeCoverage]
    void IList<T>.RemoveAt (int index)
    {
        _ThrowReadonly();
    }

    /// <inheritdoc cref="IList{T}.this" />
    public T this [int index]
    {
        get
        {
            var result = GetItem (index) ?? throw new Exception();

            return result;
        }

        // ReSharper disable once ValueParameterNotUsed
        set => throw new ReadOnlyException();
    }

    #endregion
}
