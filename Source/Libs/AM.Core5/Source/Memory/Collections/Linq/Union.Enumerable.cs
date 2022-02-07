// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* 
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Memory.Collections.Specialized;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

internal class UnionExprEnumerable<T> : IPoolingEnumerable<T>
{
    private int _count;
    private PoolingDictionary<T, int> _src;

    public UnionExprEnumerable<T> Init (PoolingDictionary<T, int> src)
    {
        _src = src;
        _count = 0;
        return this;
    }

    public IPoolingEnumerator<T> GetEnumerator()
    {
        _count++;
        return Pool<UnionExprEnumerator>.Get().Init (this, _src.GetEnumerator());
    }

    private void Dispose()
    {
        if (_count == 0) return;
        _count--;
        if (_count == 0)
        {
            _src?.Dispose();
            Pool<PoolingDictionary<T, int>>.Return (_src);
            _src = default;

            Pool<UnionExprEnumerable<T>>.Return (this);
        }
    }

    internal class UnionExprEnumerator : IPoolingEnumerator<T>
    {
        private UnionExprEnumerable<T> _parent;
        private IPoolingEnumerator<KeyValuePair<T, int>> _src;

        public UnionExprEnumerator Init (UnionExprEnumerable<T> parent,
            IPoolingEnumerator<KeyValuePair<T, int>> src)
        {
            _src = src;
            _parent = parent;
            return this;
        }

        public bool MoveNext() => _src.MoveNext();

        public void Reset() => _src.Reset();

        object IPoolingEnumerator.Current => Current;

        public T Current => _src.Current.Key;

        public void Dispose()
        {
            _src?.Dispose();
            _src = null;

            _parent?.Dispose();
            _parent = default;

            Pool<UnionExprEnumerator>.Return (this);
        }
    }

    IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
}