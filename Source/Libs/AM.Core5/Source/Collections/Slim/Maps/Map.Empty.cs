// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Map.Empty.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.Slim;

internal abstract partial class Map<TKey, TValue>
{
    // Instance without any key/value pairs. Used as a singleton.
    private sealed class EmptyMap : Map<TKey, TValue>
    {
        public override int Count => 0;

        public override Map<TKey, TValue> Set (TKey key, TValue value)
        {
            // Create a new one-element map to store the key/value pair
            return new OneElementKeyedMap (key, value);
        }

        public override bool TryGetValue (TKey key, out TValue value)
        {
            // Nothing here
            value = default!;
            return false;
        }

        public override Map<TKey, TValue> TryRemove (TKey key, out bool success)
        {
            // Nothing to remove
            success = false;
            return this;
        }

        public override bool TryGetNext (ref int index, out KeyValuePair<TKey, TValue> value)
        {
            value = default;
            return false;
        }

        public override ICollection<TKey> Keys => Array.Empty<TKey>();

        public override ICollection<TValue> Values => Array.Empty<TValue>();
    }
}
