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

using AM.Linguistics.Hunspell.Infrastructure;

#if !NO_INLINE
using System.Runtime.CompilerServices;
#endif

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell
{
    public sealed class CharacterSet : ArrayWrapper<char>
    {
        public static readonly CharacterSet Empty = new (Array.Empty<char>());

        public static readonly ArrayWrapperComparer<char, CharacterSet> DefaultComparer = new ();

        public static CharacterSet Create (string values)
        {
            return values == null ? Empty : TakeArray (values.ToCharArray());
        }

        public static CharacterSet Create (char value)
        {
            return TakeArray (new[] { value });
        }

        internal static CharacterSet Create (ReadOnlySpan<char> values)
        {
            return TakeArray (values.ToArray());
        }

        internal static CharacterSet TakeArray (char[] values)
        {
#if DEBUG
            if (values == null) throw new ArgumentNullException (nameof (values));
#endif

            Array.Sort (values);
            return new CharacterSet (values);
        }

        private CharacterSet (char[] values)
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

        public bool Contains (char value)
        {
            return unchecked ((value & mask) != default)
                   &&
                   Array.BinarySearch (_items, value) >= 0;
        }

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        public string GetCharactersAsString() => new (_items);
    }
}
