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

/* YaleReader.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
internal static class YaleReader
{
    public static void ReadChildren
        (
            char[] indexToChar,
            int nodeCount,
            BinaryReader reader,
            out int[] firstChildForNode,
            out YaleChild[] children
        )
    {
        firstChildForNode = new int[nodeCount + 1];

        var firstChildForNode_i = 0;
        var totalChildCount = reader.ReadInt32();

        children = new YaleChild [totalChildCount];

        firstChildForNode[nodeCount] = totalChildCount;

        var globalChild_i = 0;

        for (var child1_i = 0; child1_i < nodeCount; ++child1_i)
        {
            firstChildForNode[firstChildForNode_i++] = globalChild_i;

            var childCount = ReadInt (reader, indexToChar.Length + 1);

            for (ushort child_i = 0; child_i < childCount; ++child_i)
            {
                var charIndex = ReadInt (reader, indexToChar.Length);
                var childNodeIndex = reader.ReadInt32();

                children[globalChild_i++] = new YaleChild (childNodeIndex, charIndex);
            }
        }
    }

    public static ushort ReadInt
        (
            BinaryReader reader,
            int countOfPossibleValues
        )
    {
        return countOfPossibleValues > 256
            ? reader.ReadUInt16()
            : reader.ReadByte();
    }
}
