// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* CharToIndexPlusOneMap.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
internal static class CharToIndexPlusOneMap
{
    public static ushort[]? Get
        (
            char[] uniqueChars
        )
    {
        if (uniqueChars.Length == 0)
        {
            return null;
        }

        var charToIndex = new ushort [uniqueChars.Last() - uniqueChars.First() + 1];

        for (var i = 0; i < uniqueChars.Length; ++i)
        {
            charToIndex[uniqueChars[i] - uniqueChars.First()] = (ushort)(i + 1);
        }

        return charToIndex;
    }
}
