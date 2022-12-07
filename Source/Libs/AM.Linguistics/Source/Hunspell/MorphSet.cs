// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* MorphSet.cs --
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

/// <summary>
///
/// </summary>
public sealed class MorphSet
    : ArrayWrapper<string>, IEquatable<MorphSet>
{
    /// <summary>
    ///
    /// </summary>
    public static readonly MorphSet Empty = TakeArray (Array.Empty<string>());

    /// <summary>
    ///
    /// </summary>
    public static readonly ArrayWrapperComparer<string, MorphSet> DefaultComparer = new ();

    /// <summary>
    ///
    /// </summary>
    /// <param name="morphs"></param>
    /// <returns></returns>
    public static MorphSet Create
        (
            IEnumerable<string>? morphs
        )
    {
        return morphs is null ? Empty : TakeArray (morphs.ToArray());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="morphs"></param>
    /// <returns></returns>
    internal static MorphSet TakeArray
        (
            string[]? morphs
        )
    {
        return morphs is null ? Empty : new MorphSet (morphs);
    }

    internal static string[] CreateReversed
        (
            string[] oldMorphs
        )
    {
        var newMorphs = new string[oldMorphs.Length];
        var lastIndex = oldMorphs.Length - 1;
        for (var i = 0; i < oldMorphs.Length; i++)
        {
            newMorphs[i] = oldMorphs[lastIndex - i].GetReversed()!;
        }

        return newMorphs;
    }

    private MorphSet
        (
            string[] morphs
        )
        : base (morphs)
    {
        // пустое тело конструктора
    }

    internal string Join (string seperator)
    {
        return string.Join (seperator, _items);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals
        (
            MorphSet? other
        )
    {
        return !ReferenceEquals (other, null)
               &&
               (
                   ReferenceEquals (this, other)
                   || ArrayComparer<string>.Default.Equals (other._items, _items)
               );
    }

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        return Equals (obj as MorphSet);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return ArrayComparer<string>.Default.GetHashCode (_items);
    }
}
