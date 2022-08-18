// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* DictionarySlim.Enumerator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.Slim;

public partial class DictionarySlim<TKey, TValue>
{
    /// <summary>
    ///
    /// </summary>
    public struct Enumerator
        : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        private readonly Map<TKey, TValue> _map;
        private int index;
        private KeyValuePair<TKey, TValue> _current;

        internal Enumerator (Map<TKey, TValue> map)
        {
            _map = map;
            index = -1;
            _current = default (KeyValuePair<TKey, TValue>);
        }

        public KeyValuePair<TKey, TValue> Current => _current;

        public bool MoveNext() => _map.TryGetNext (ref index, out _current);

        public void Dispose()
        {
        }

        object IEnumerator.Current => _current;

        void IEnumerator.Reset() => throw new NotSupportedException();
    }
}
