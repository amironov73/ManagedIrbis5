// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsSingleQueryList.EnumerableRef.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System;

namespace AM.Memory.Collections.Specialized;

/// <summary>
///
/// </summary>
public static partial class AsSingleQueryList
{
    private class EnumerableShared<T>
        : IPoolingEnumerable<T>
        where T : class
    {
        private int _count;

        public IPoolingEnumerable<T> Init (PoolingListCanon<T> src)
        {
            _src = src;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool<EnumeratorRef>.Get().Init (this, _src);
        }

        #region Private members

        private PoolingListCanon<T> _src;

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        private void Dispose()
        {
            if (_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src?.Dispose();
                _src = default;
                Pool<EnumerableShared<T>>.Return (this);
            }
        }

        private class EnumeratorRef
            : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator<T>? _enumerator;
            private EnumerableShared<T>? _parent;

            public IPoolingEnumerator<T> Init (EnumerableShared<T> parent, IPoolingEnumerable<T> src)
            {
                _parent = parent;
                _enumerator = src.GetEnumerator();
                return this;
            }

            public bool MoveNext() => _enumerator.ThrowIfNull().MoveNext();

            public void Reset() => _enumerator.ThrowIfNull().Reset();

            public T Current => _enumerator.ThrowIfNull().Current;

            object IPoolingEnumerator.Current => Current;

            /// <inheritdoc cref="IDisposable.Dispose"/>
            public void Dispose()
            {
                _enumerator?.Dispose();
                _enumerator = default;

                _parent?.Dispose();
                _parent = default;

                Pool<EnumeratorRef>.Return (this);
            }
        }
    }
}
