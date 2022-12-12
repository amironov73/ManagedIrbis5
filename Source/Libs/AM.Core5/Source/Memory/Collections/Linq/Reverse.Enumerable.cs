// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Reverse.Enumerable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Memory.Collections.Specialized;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

internal class ReverseExprEnumerable<T>
    : IPoolingEnumerable<T>
{
    private int _count;
    private PoolingList<T> _src = default!;

    public ReverseExprEnumerable<T> Init
        (
            PoolingList<T> src
        )
    {
        _src = src;
        _count = 0;
        return this;
    }

    public IPoolingEnumerator<T> GetEnumerator()
    {
        _count++;
        return Pool<ReverseExprEnumerator>.Get().Init (_src, this);
    }

    private void Dispose()
    {
        if (_count == 0) return;
        _count--;
        if (_count == 0)
        {
            _src?.Dispose();
            Pool<PoolingList<T>>.Return (_src);
            _src = default!;
            Pool<ReverseExprEnumerable<T>>.Return (this);
        }
    }

    internal class ReverseExprEnumerator
        : IPoolingEnumerator<T>
    {
        private PoolingList<T> _src = default!;
        private ReverseExprEnumerable<T> _parent = default!;
        private int _position;

        public ReverseExprEnumerator Init
            (
                PoolingList<T> src,
                ReverseExprEnumerable<T> parent
            )
        {
            _position = src.Count;
            _src = src;
            _parent = parent;
            return this;
        }

        public bool MoveNext()
        {
            if (_position == 0) return false;
            _position--;
            return true;
        }

        public void Reset() => _position = _src.Count;

        object IPoolingEnumerator.Current => Current!;

        public T Current => _src[_position];

        public void Dispose()
        {
            _parent?.Dispose();
            _parent = default!;
            _src = default!;
            _position = default;

            Pool<ReverseExprEnumerator>.Return (this);
        }
    }

    IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
}
