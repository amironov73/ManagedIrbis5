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

#if !NO_INLINE
using System.Runtime.CompilerServices;
#endif

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell
{
    public class WordEntryDetail : IEquatable<WordEntryDetail>
    {
        public static bool operator == (WordEntryDetail a, WordEntryDetail b)
        {
            return ReferenceEquals (a, null) ? ReferenceEquals (b, null) : a.Equals (b);
        }

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        public static bool operator != (WordEntryDetail a, WordEntryDetail b) => !(a == b);

        public static WordEntryDetail Default { get; } = new (FlagSet.Empty, MorphSet.Empty, WordEntryOptions.None);

        public WordEntryDetail (FlagSet flags, MorphSet morphs, WordEntryOptions options)
        {
            Flags = flags ?? FlagSet.Empty;
            Morphs = morphs ?? MorphSet.Empty;
            Options = options;
        }

        public FlagSet Flags { get; }

        public MorphSet Morphs { get; }

        public WordEntryOptions Options { get; }

        public bool HasFlags
        {
#if !NO_INLINE
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
            get => Flags.HasItems;
        }

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        public bool ContainsFlag (FlagValue flag) => Flags.Contains (flag);

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        public bool ContainsAnyFlags (FlagValue a, FlagValue b) => Flags.ContainsAny (a, b);

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        public bool ContainsAnyFlags (FlagValue a, FlagValue b, FlagValue c) => Flags.ContainsAny (a, b, c);

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        public bool ContainsAnyFlags (FlagValue a, FlagValue b, FlagValue c, FlagValue d) =>
            Flags.ContainsAny (a, b, c, d);

        public bool Equals (WordEntryDetail other)
        {
            if (ReferenceEquals (other, null)) return false;
            if (ReferenceEquals (this, other)) return true;

            return other.Options == Options
                   && other.Flags.Equals (Flags)
                   && other.Morphs.Equals (Morphs);
        }

        public override bool Equals (object obj)
        {
            return obj is WordEntryDetail d && Equals (d);
        }

        public override int GetHashCode()
        {
            return unchecked (Flags.GetHashCode() ^ Morphs.GetHashCode() ^ Options.GetHashCode());
        }

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        internal WordEntry ToEntry (string word) =>
            new (word, this);
    }
}
