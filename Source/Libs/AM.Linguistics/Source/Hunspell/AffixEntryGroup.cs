// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* AffixEntryGroup.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Linguistics.Hunspell.Infrastructure;

#if !NO_INLINE
using System.Runtime.CompilerServices;
#endif

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell
{
    /// <summary>
    /// Contains a set of <see cref="AffixEntry"/> instances based on the same <see cref="AFlag"/>.
    /// </summary>
    /// <typeparam name="TEntry">The specific entry type.</typeparam>
    public sealed class AffixEntryGroup<TEntry>
        where TEntry : AffixEntry
    {
        public sealed class Builder
        {
            /// <summary>
            /// All of the entries that make up this group.
            /// </summary>
            public List<TEntry> Entries { get; set; }

            /// <summary>
            /// ID used to represent the affix group.
            /// </summary>
            public FlagValue AFlag { get; set; }

            /// <summary>
            /// Options for this affix group.
            /// </summary>
            public AffixEntryOptions Options { get; set; }

            public AffixEntryGroup<TEntry> ToGroup()
            {
                return new (AFlag, Options, AffixEntryCollection<TEntry>.Create (Entries));
            }
        }

        public AffixEntryGroup (FlagValue aFlag, AffixEntryOptions options, AffixEntryCollection<TEntry> entries)
        {
            Entries = entries;
            AFlag = aFlag;
            Options = options;
        }

        /// <summary>
        /// All of the entries that make up this group.
        /// </summary>
        public AffixEntryCollection<TEntry> Entries { get; }

        /// <summary>
        /// ID used to represent the affix group.
        /// </summary>
        public FlagValue AFlag { get; }

        /// <summary>
        /// Options for this affix group.
        /// </summary>
        public AffixEntryOptions Options { get; }

        /// <summary>
        /// Indicates if a group has the <see cref="AffixEntryOptions.CrossProduct"/> option enabled.
        /// </summary>
        /// <seealso cref="AffixEntryOptions"/>
        public bool AllowCross
        {
#if !NO_INLINE
            [MethodImpl (MethodImplOptions.AggressiveInlining)]
#endif
            get => EnumEx.HasFlag (Options, AffixEntryOptions.CrossProduct);
        }

        internal Affix<TEntry>[] CreateAffixes()
        {
            var source = Entries._items;
            var result = new Affix<TEntry>[source.Length];
            for (var i = 0; i < source.Length; i++) result[i] = Affix<TEntry>.Create (source[i], this);

            return result;
        }
    }
}
