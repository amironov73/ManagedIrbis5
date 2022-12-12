// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Select.Enumerable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Memory.Collections.Linq;

internal class SelectExprEnumerable<T, TR>
    : IPoolingEnumerable<TR>
{
    private IPoolingEnumerable<T>? _src;
    private Func<T, TR>? _mutator;
    private int _count;

    public SelectExprEnumerable<T, TR> Init
        (
            IPoolingEnumerable<T> src,
            Func<T, TR> mutator
        )
    {
        _src = src;
        _count = 0;
        _mutator = mutator;
        return this;
    }

    public IPoolingEnumerator<TR> GetEnumerator()
    {
        _count++;
        return Pool<SelectExprEnumerator>.Get().Init (this, _src!.GetEnumerator(), _mutator!);
    }

    private void Dispose()
    {
        if (_count == 0) return;
        _count--;
        if (_count == 0)
        {
            _src = default;
            _count = 0;
            _mutator = default;
            Pool<SelectExprEnumerable<T, TR>>.Return (this);
        }
    }

    internal class SelectExprEnumerator
        : IPoolingEnumerator<TR>
    {
        private Func<T, TR>? _mutator;
        private SelectExprEnumerable<T, TR>? _parent;
        private IPoolingEnumerator<T>? _src;

        public SelectExprEnumerator Init
            (
                SelectExprEnumerable<T, TR> parent,
                IPoolingEnumerator<T> src,
                Func<T, TR> mutator
            )
        {
            _src = src;
            _parent = parent;
            _mutator = mutator;
            return this;
        }

        public bool MoveNext() => _src!.MoveNext();

        public void Reset() => _src!.Reset();

        object IPoolingEnumerator.Current => Current!;

        public TR Current => _mutator! (_src!.Current);

        public void Dispose()
        {
            _parent?.Dispose();
            _parent = default;
            _src?.Dispose();
            _src = default;
            Pool<SelectExprEnumerator>.Return (this);
        }
    }

    IPoolingEnumerator IPoolingEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
