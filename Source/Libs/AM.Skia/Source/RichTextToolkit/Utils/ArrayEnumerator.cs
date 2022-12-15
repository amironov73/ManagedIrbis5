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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Skia.RichTextKit
{
    class ArraySliceEnumerator<T> : IEnumerator<T>, IEnumerator
    {
        public ArraySliceEnumerator(T[] arr, int start, int length)
        {
            _arr = arr;
            _start = start;
            _end = start + length;
            _current = _start - 1;
        }

        T[] _arr;
        int _start;
        int _end;
        int _current;

        public T Current
        {
            get
            {
                if (_current < _end)
                    return _arr[_current];
                else
                    return default(T);
            }
        }


        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_current < _end)
                _current++;

            return _current < _end;
        }

        public void Reset()
        {
            _current = _start - 1;
        }

        public void Dispose()
        {
        }
    }
}
