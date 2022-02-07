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

#nullable enable

namespace AM.Memory.Collections.Linq
{
    internal class OfTypeExprEnumerable<T> : IPoolingEnumerable<T>
    {
        private int _count;
        
        private IPoolingEnumerable _src;

        public OfTypeExprEnumerable<T> Init(IPoolingEnumerable src)
        {
            _src = src;
            _count = 0;
            return this;
        }

        public IPoolingEnumerator<T> GetEnumerator()
        {
            _count++;
            return Pool<OfTypeExprEnumerator>.Get().Init(_src.GetEnumerator(), this);
        }

        private void Dispose()
        {
            if(_count == 0) return;
            _count--;
            if (_count == 0)
            {
                _src = default;
                Pool<OfTypeExprEnumerable<T>>.Return(this);
            }
        }

        internal class OfTypeExprEnumerator : IPoolingEnumerator<T>
        {
            private IPoolingEnumerator _src;
            private OfTypeExprEnumerable<T> _parent;
        	
            public OfTypeExprEnumerator Init(IPoolingEnumerator src, OfTypeExprEnumerable<T> parent)
            {
                _src = src;
                _parent = parent;
                return this;
            }

            public bool MoveNext()
            {
                do
                {
                    var next = _src.MoveNext();
                    if (!next) return false;
                } while (!(_src.Current is T));

                return true;
            }

            public void Reset()
            {
                _src.Reset();
            }

            object IPoolingEnumerator.Current => Current;

            public T Current => (T)_src.Current;

            public void Dispose()
            {
                _parent?.Dispose();
                _parent = null;
                _src?.Dispose();
                _src = default;
                Pool<OfTypeExprEnumerator>.Return(this);
            }
        }

        IPoolingEnumerator IPoolingEnumerable.GetEnumerator() => GetEnumerator();
    }
}