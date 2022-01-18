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
    public class PatternSet : ArrayWrapper<PatternEntry>
    {
        public static readonly PatternSet Empty = TakeArray (Array.Empty<PatternEntry>());

        public static PatternSet Create (IEnumerable<PatternEntry> patterns)
        {
            return patterns == null ? Empty : TakeArray (patterns.ToArray());
        }

        internal static PatternSet TakeArray (PatternEntry[] patterns)
        {
            return patterns == null ? Empty : new PatternSet (patterns);
        }

        private PatternSet (PatternEntry[] patterns)
            : base (patterns)
        {
        }

        /// <summary>
        /// Forbid compoundings when there are special patterns at word bound.
        /// </summary>
        internal bool Check (string word, int pos, WordEntry r1, WordEntry r2, bool affixed)
        {
#if DEBUG
            if (r1 == null) throw new ArgumentNullException (nameof (r1));
            if (r2 == null) throw new ArgumentNullException (nameof (r2));
#endif

            var wordAfterPos = word.AsSpan (pos);

            foreach (var patternEntry in items)
                if (
                        HunspellTextFunctions.IsSubset (patternEntry.Pattern2, wordAfterPos)
                        &&
                        (
                            patternEntry.Condition.IsZero
                            ||
                            r1.ContainsFlag (patternEntry.Condition)
                        )
                        &&
                        (
                            patternEntry.Condition2.IsZero
                            ||
                            r2.ContainsFlag (patternEntry.Condition2)
                        )
                        &&

                        // zero length pattern => only TESTAFF
                        // zero pattern (0/flag) => unmodified stem (zero affixes allowed)
                        (
                            string.IsNullOrEmpty (patternEntry.Pattern)
                            ||
                            PatternWordCheck (word, pos,
                                patternEntry.Pattern.StartsWith ('0') ? r1.Word : patternEntry.Pattern)
                        )
                    )
                    return true;

            return false;
        }

#if !NO_INLINE
        [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
        private static bool PatternWordCheck (string word, int pos, string other) =>
            other.Length <= pos
            && word.AsSpan (pos - other.Length).StartsWith (other.AsSpan());
    }
}
