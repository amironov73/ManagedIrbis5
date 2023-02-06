// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* GroupBy.ResultEnumerable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Memory.Collections.Specialized;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

internal sealed class GroupedResultEnumerable<TSource, TKey, TResult>
    : IPoolingEnumerable<TResult>
{
    private IPoolingEnumerable<TSource> _source = default!;
    private Func<TSource, TKey> _keySelector = default!;
    private Func<TKey, IPoolingEnumerable<TSource>, TResult> _resultSelector = default!;
    private IEqualityComparer<TKey> _comparer = default!;
    private int _count;

    public GroupedResultEnumerable<TSource, TKey, TResult> Init
        (
            IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TKey, IPoolingEnumerable<TSource>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer
        )
    {
        _source = source ?? throw new ArgumentNullException (nameof (source));
        _keySelector = keySelector ?? throw new ArgumentNullException (nameof (keySelector));
        _resultSelector = resultSelector ?? throw new ArgumentNullException (nameof (resultSelector));
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
        _count = 0;
        return this;
    }

    public IPoolingEnumerator<TResult> GetEnumerator()
    {
        var tmpDict = Pool<PoolingDictionary<TKey, PoolingGrouping>>.Get()
            .Init (0, _comparer);

        PoolingGrouping grp;
        foreach (var item in _source)
        {
            var key = _keySelector (item);
            if (!tmpDict.TryGetValue (key, out grp))
            {
                tmpDict[key] = grp = Pool<PoolingGrouping>.Get().Init (key);
            }

            grp.InternalList.Add (item);
        }

        _count++;
        return Pool<GroupedResultEnumerator>.Get().Init (this, tmpDict);
    }

    private void Dispose()
    {
        if (_count == 0)
        {
            return;
        }

        _count--;

        if (_count == 0)
        {
            _comparer = default!;
            _resultSelector = default!;
            _keySelector = default!;
            Pool<GroupedResultEnumerable<TSource, TKey, TResult>>.Return (this);
        }
    }

    IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

    internal class GroupedResultEnumerator
        : IPoolingEnumerator<TResult>
    {
        private PoolingDictionary<TKey, PoolingGrouping> _src = default!;
        private GroupedResultEnumerable<TSource, TKey, TResult> _parent = default!;
        private IPoolingEnumerator<KeyValuePair<TKey, PoolingGrouping>> _enumerator = default!;

        public GroupedResultEnumerator Init 
            (
                GroupedResultEnumerable<TSource, TKey, TResult> parent,
                PoolingDictionary<TKey, PoolingGrouping> src
            )
        {
            _src = src;
            _parent = parent;
            _enumerator = _src.GetEnumerator();
            return this;
        }

        public void Dispose()
        {
            // Cleanup contents
            foreach (var grouping in _src)
            {
                grouping.Value.Dispose();
                Pool<PoolingGrouping>.Return (grouping.Value);
            }

            // cleanup collection
            if (_src != null!)
            {
                _src.Dispose();
                Pool<PoolingDictionary<TKey, PoolingGrouping>>.Return (_src);
                _src = default!;
            }

            _enumerator?.Dispose();
            _enumerator = default!;

            _parent?.Dispose();
            _parent = default!;

            Pool<GroupedResultEnumerator>.Return (this);
        }

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();

        public TResult Current =>
            _parent._resultSelector (_enumerator.Current.Key, _enumerator.Current.Value.InternalList);

        object IPoolingEnumerator.Current => Current!;
    }

    internal class PoolingGrouping
        : IPoolingGrouping<TKey, TSource>, IDisposable
    {
        private PoolingList<TSource> _elements = default!;

        public PoolingGrouping Init (TKey key)
        {
            _elements = Pool<PoolingList<TSource>>.Get().Init();
            Key = key;
            return this;
        }

        internal PoolingList<TSource> InternalList => _elements;

        public IPoolingEnumerator<TSource> GetEnumerator() => _elements.GetEnumerator();

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///
        /// </summary>
        public TKey Key { get; private set; }

        public void Dispose()
        {
            if (_elements != null!)
            {
                _elements.Dispose();
                Pool<PoolingList<TSource>>.Return (_elements);
                _elements = default!;
            }

            Key = default!;
        }
    }
}
