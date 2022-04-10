// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode

/* SplitCharactedComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.TernarySearchTree;

internal class SplitCharacterComparer<TValue>
    : IComparer<Node<TValue>>
{
    public int Compare
        (
            Node<TValue>? x,
            Node<TValue>? y
        )
    {
        Sure.NotNull (x);
        Sure.NotNull (y);

        return x!.SplitCharacter.CompareTo (y!.SplitCharacter);
    }
}
