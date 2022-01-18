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
    public sealed class SingleReplacement : ReplacementEntry
    {
        public SingleReplacement(string pattern, string outString, ReplacementValueType type)
            : base(pattern)
        {
            OutString = outString;
            Type = type;
        }

        public string OutString { get; }

        public ReplacementValueType Type { get; }

        public override string Med => this[ReplacementValueType.Med];

        public override string Ini => this[ReplacementValueType.Ini];

        public override string Fin => this[ReplacementValueType.Fin];

        public override string Isol => this[ReplacementValueType.Isol];

        public override string this[ReplacementValueType type]
        {
#if !NO_INLINE
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get => Type == type ? OutString : null;
        }
    }
}
