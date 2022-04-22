// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsSingleQueryList.EnumerableVal.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Memory.Collections.Specialized;

public static partial class AsSingleQueryList
{
    private class EnumerableTyped<T>
        : IPoolingEnumerable<T>
    {
        public IPoolingEnumerable<T> Init
            (
                PoolingList<T> src
            )
        {
            _src = src;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            var src = _src;
            _src = default;
            Pool<EnumerableTyped<T>>.Return (this);
            return Pool<EnumeratorVal>.Get().Init (src);
        }

        #region Private members

        private PoolingList<T> _src;

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        private class EnumeratorVal
            : IPoolingEnumerator<T>
        {
            private PoolingList<T> _src;
            private IPoolingEnumerator<T> _enumerator;

            public IPoolingEnumerator<T> Init
                (
                    PoolingList<T> src
                )
            {
                _src = src;
                _enumerator = _src.GetEnumerator();
                return this;
            }

            public bool MoveNext() => _enumerator.MoveNext();

            public void Reset()
            {
                _enumerator?.Dispose();
                _enumerator = _src.GetEnumerator();
            }

            public T Current => _enumerator.Current;

            object IPoolingEnumerator.Current => Current;

            public void Dispose()
            {
                _enumerator?.Dispose();
                _src?.Dispose();
                Pool<PoolingList<T>>.Return (_src);
                Pool<EnumeratorVal>.Return (this);
                _src = default;
            }
        }
    }
}
