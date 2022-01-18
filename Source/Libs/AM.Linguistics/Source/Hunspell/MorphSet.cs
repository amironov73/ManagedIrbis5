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

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

public sealed class MorphSet : ArrayWrapper<string>, IEquatable<MorphSet>
{
    public static readonly MorphSet Empty = TakeArray (Array.Empty<string>());

    public static readonly ArrayWrapperComparer<string, MorphSet> DefaultComparer = new ();

    public static MorphSet Create (IEnumerable<string> morphs)
    {
        return morphs == null ? Empty : TakeArray (morphs.ToArray());
    }

    internal static MorphSet TakeArray (string[] morphs)
    {
        return morphs == null ? Empty : new MorphSet (morphs);
    }

    internal static string[] CreateReversed (string[] oldMorphs)
    {
        var newMorphs = new string[oldMorphs.Length];
        var lastIndex = oldMorphs.Length - 1;
        for (var i = 0; i < oldMorphs.Length; i++) newMorphs[i] = oldMorphs[lastIndex - i].GetReversed();

        return newMorphs;
    }

    private MorphSet (string[] morphs)
        : base (morphs)
    {
    }

    internal string Join (string seperator)
    {
        return string.Join (seperator, items);
    }

    public bool Equals (MorphSet other)
    {
        return !ReferenceEquals (other, null)
               &&
               (
                   ReferenceEquals (this, other)
                   || ArrayComparer<string>.Default.Equals (other.items, items)
               );
    }

    public override bool Equals (object obj)
    {
        return Equals (obj as MorphSet);
    }

    public override int GetHashCode()
    {
        return ArrayComparer<string>.Default.GetHashCode (items);
    }
}
