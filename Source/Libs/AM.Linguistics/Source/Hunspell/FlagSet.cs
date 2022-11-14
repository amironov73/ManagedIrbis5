// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* .cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM.Linguistics.Hunspell.Infrastructure;

#if !NO_INLINE
using System.Runtime.CompilerServices;
#endif

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell
{
    public sealed class FlagSet : ArrayWrapper<FlagValue>, IEquatable<FlagSet>
    {
        public static readonly FlagSet Empty = new (Array.Empty<FlagValue>());

        public static readonly ArrayWrapperComparer<FlagValue, FlagSet> DefaultComparer = new ();

        public static FlagSet Create (IEnumerable<FlagValue> given)
        {
            return given == null ? Empty : TakeArray (given.Distinct().ToArray());
        }

        public static FlagSet Union (FlagSet a, FlagSet b)
        {
            return Create (Enumerable.Concat (a, b));
        }

        public static bool ContainsAny (FlagSet a, FlagSet b)
        {
            if (a != null && !a.IsEmpty && b != null && !b.IsEmpty)
            {
                if (a.Count == 1) return b.Contains (a[0]);
                if (b.Count == 1) return a.Contains (b[0]);

                if (a.Count > b.Count) ReferenceHelpers.Swap (ref a, ref b);

                foreach (var item in a)
                    if (b.Contains (item))
                        return true;
            }

            return false;
        }

        internal static FlagSet TakeArray (FlagValue[] values)
        {
            if (values == null || values.Length == 0) return Empty;

            Array.Sort (values);
            return new FlagSet (values);
        }

        internal static FlagSet Union (FlagSet set, FlagValue value)
        {
            var valueIndex = Array.BinarySearch (set._items, value);
            if (valueIndex >= 0) return set;

            valueIndex = ~valueIndex; // locate the best insertion point

            var newItems = new FlagValue[set._items.Length + 1];
            if (valueIndex >= set._items.Length)
            {
                Array.Copy (set._items, newItems, set._items.Length);
                newItems[set._items.Length] = value;
            }
            else
            {
                Array.Copy (set._items, newItems, valueIndex);
                Array.Copy (set._items, valueIndex, newItems, valueIndex + 1, set._items.Length - valueIndex);
                newItems[valueIndex] = value;
            }

            return new FlagSet (newItems);
        }

        private FlagSet (FlagValue[] values)
            : base (values)
        {
            mask = default;
            for (var i = 0; i < values.Length; i++)
                unchecked
                {
                    mask |= values[i];
                }
        }

        private readonly char mask;

        public bool Contains (FlagValue value)
        {
            if (!value.HasValue || IsEmpty) return false;
            if (_items.Length == 1) return value.Equals (_items[0]);

            return unchecked (value & mask) != default
                   && value >= _items[0]
                   && value <= _items[_items.Length - 1]
                   && Array.BinarySearch (_items, value) >= 0;
        }

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        public bool ContainsAny (FlagSet values) => ContainsAny (this, values);

        public bool ContainsAny (FlagValue a, FlagValue b)
        {
            return HasItems && (Contains (a) || Contains (b));
        }

        public bool ContainsAny (FlagValue a, FlagValue b, FlagValue c)
        {
            return HasItems && (Contains (a) || Contains (b) || Contains (c));
        }

        public bool ContainsAny (FlagValue a, FlagValue b, FlagValue c, FlagValue d)
        {
            return HasItems && (Contains (a) || Contains (b) || Contains (c) || Contains (d));
        }

        public bool Equals (FlagSet other)
        {
            return !ReferenceEquals (other, null)
                   &&
                   (
                       ReferenceEquals (this, other)
                       || ArrayComparer<FlagValue>.Default.Equals (other._items, _items)
                   );
        }

        public override bool Equals (object obj)
        {
            return Equals (obj as FlagSet);
        }

        public override int GetHashCode()
        {
            return ArrayComparer<FlagValue>.Default.GetHashCode (_items);
        }
    }
}
