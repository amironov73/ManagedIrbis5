// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* GroupByEnumerable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Memory.Collections.Specialized;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

internal sealed class GroupedEnumerable<TSource, TKey, TElement>
    : IPoolingEnumerable<IPoolingGrouping<TKey, TElement>>
{
    private IPoolingEnumerable<TSource> _source = default!;
    private Func<TSource, TKey> _keySelector = default!;
    private Func<TSource, TElement> _elementSelector = default!;
    private IEqualityComparer<TKey> _comparer = default!;
    private int _count;

    public GroupedEnumerable<TSource, TKey, TElement> Init
        (
            IPoolingEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer
        )
    {
        _source = source ?? throw new ArgumentNullException (nameof (source));
        _keySelector = keySelector ?? throw new ArgumentNullException (nameof (keySelector));
        _elementSelector = elementSelector ?? throw new ArgumentNullException (nameof (elementSelector));
        _comparer = comparer ?? EqualityComparer<TKey>.Default;
        _count = 0;
        return this;
    }

    public IPoolingEnumerator<IPoolingGrouping<TKey, TElement>> GetEnumerator()
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

            grp.InternalList.Add (_elementSelector (item));
        }

        _count++;
        return Pool<PoolingGroupingEnumerator>.Get().Init (this, tmpDict);
    }

    private void Dispose()
    {
        if (_count == 0) return;
        _count--;

        if (_count == 0)
        {
            _comparer = default!;
            _elementSelector = default!;
            _keySelector = default!;
            Pool<GroupedEnumerable<TSource, TKey, TElement>>.Return (this);
        }
    }

    IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

    internal class PoolingGroupingEnumerator : IPoolingEnumerator<IPoolingGrouping<TKey, TElement>>
    {
        private PoolingDictionary<TKey, PoolingGrouping> _src = default!;
        private GroupedEnumerable<TSource, TKey, TElement> _parent = default!;
        private IPoolingEnumerator<KeyValuePair<TKey, PoolingGrouping>> _enumerator = default!;

        public PoolingGroupingEnumerator Init (
            GroupedEnumerable<TSource, TKey, TElement> parent,
            PoolingDictionary<TKey, PoolingGrouping> src)
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
            _src?.Dispose();
            Pool<PoolingDictionary<TKey, PoolingGrouping>>.Return (_src);
            _src = default!;

            _enumerator?.Dispose();
            _enumerator = default!;

            _parent?.Dispose();
            _parent = default!;

            Pool<PoolingGroupingEnumerator>.Return (this);
        }

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();

        public IPoolingGrouping<TKey, TElement> Current => _enumerator.Current.Value;

        object IPoolingEnumerator.Current => Current;
    }

    internal class PoolingGrouping
        : IPoolingGrouping<TKey, TElement>, IDisposable
    {
        private PoolingList<TElement> _elements = default!;

        public PoolingGrouping Init (TKey key)
        {
            _elements = Pool<PoolingList<TElement>>.Get().Init();
            Key = key;
            return this;
        }

        internal PoolingList<TElement> InternalList => _elements;

        public IPoolingEnumerator<TElement> GetEnumerator() => _elements.GetEnumerator();

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        ///
        /// </summary>
        public TKey Key { get; private set; }

        public void Dispose()
        {
            _elements.Dispose();
            Pool<PoolingList<TElement>>.Return (_elements);
            _elements = default!;

            Key = default!;
        }
    }
}
