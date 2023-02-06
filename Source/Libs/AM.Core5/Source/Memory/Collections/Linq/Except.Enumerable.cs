// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Except.Enumerable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Memory.Collections.Specialized;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

internal class ExceptExprEnumerable<T> : IPoolingEnumerable<T>
{
    private int _count;
    private IPoolingEnumerable<T> _src = default!;
    private IEqualityComparer<T>? _comparer;
    private PoolingDictionary<T, int> _except = default!;

    public ExceptExprEnumerable<T> Init
        (
            IPoolingEnumerable<T> src,
            PoolingDictionary<T, int> except,
            IEqualityComparer<T>? comparer = default
        )
    {
        _src = src;
        _except = except;
        _comparer = comparer;
        _count = 0;
        return this;
    }

    public IPoolingEnumerator<T> GetEnumerator()
    {
        _count++;
        return Pool<ExceptExprEnumerator>.Get().Init (this, _src.GetEnumerator());
    }

    private void Dispose()
    {
        if (_count == 0) return;
        _count--;
        if (_count == 0)
        {
            _src = default!;

            if (_except != null!)
            {
                _except.Dispose();
                Pool<PoolingDictionary<T, int>>.Return (_except);
                _except = default!;
            }

            Pool<ExceptExprEnumerable<T>>.Return (this);
        }
    }

    internal class ExceptExprEnumerator : IPoolingEnumerator<T>
    {
        private ExceptExprEnumerable<T> _parent = default!;
        private IPoolingEnumerator<T> _src = default!;

        public ExceptExprEnumerator Init (ExceptExprEnumerable<T> parent, IPoolingEnumerator<T> src)
        {
            _src = src;
            _parent = parent;
            return this;
        }

        public bool MoveNext()
        {
            while (_src.MoveNext())
            {
                if (_parent._except.ContainsKey (_src.Current)) continue;
                return true;
            }

            return false;
        }

        public void Reset() => _src.Reset();

        object IPoolingEnumerator.Current => Current!;

        public T Current => _src.Current;

        public void Dispose()
        {
            _src?.Dispose();
            _src = default!;

            _parent?.Dispose();
            _parent = default!;

            Pool<ExceptExprEnumerator>.Return (this);
        }
    }

    IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
}
