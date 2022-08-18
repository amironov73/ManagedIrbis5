// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Map.One.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.Slim;

internal abstract partial class Map<TKey, TValue>
{
    // Instance with one key/value pair.
    private sealed class OneElementKeyedMap : Map<TKey, TValue>
    {
        private readonly TKey _key1;
        private TValue _value1;

        public override int Count => 1;

        public OneElementKeyedMap (TKey key, TValue value)
        {
            _key1 = key;
            _value1 = value;
        }

        public override Map<TKey, TValue> Set (TKey key, TValue value)
        {
            // If the key matches one already contained in this map update it,
            // otherwise create a two-element map with the additional key/value.
            if (Comparer.Equals (key, _key1))
            {
                _value1 = value;
                return this;
            }
            else
            {
                return new TwoElementKeyedMap (_key1, _value1, key, value);
            }
        }

        public override bool TryGetValue (TKey key, out TValue value)
        {
            if (Comparer.Equals (key, _key1))
            {
                value = _value1;
                return true;
            }
            else
            {
                value = default (TValue);
                return false;
            }
        }

        public override Map<TKey, TValue> TryRemove (TKey key, out bool success)
        {
            success = Comparer.Equals (key, _key1);
            if (success)
            {
                // Return the Empty singleton
                return Empty;
            }

            return this;
        }

        public override bool TryGetNext (ref int index, out KeyValuePair<TKey, TValue> value)
        {
            index++;
            if (index == 0)
            {
                value = new KeyValuePair<TKey, TValue> (_key1, _value1);
                return true;
            }

            value = default (KeyValuePair<TKey, TValue>);
            return false;
        }

        public override ICollection<TKey> Keys => new[] { _key1 };
        public override ICollection<TValue> Values => new[] { _value1 };
    }
}
