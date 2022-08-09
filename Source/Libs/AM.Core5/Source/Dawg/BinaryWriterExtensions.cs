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

/* BinaryWriterExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
internal static class BinaryWriterExtensions
{
    public static void WriteArray<T>
        (
            this BinaryWriter writer,
            ICollection<T> array,
            Action<BinaryWriter, T> writeElement
        )
    {
        writer.Write (array.Count);

        foreach (var elem in array)
        {
            writeElement (writer, elem);
        }
    }

    public static void WriteArray
        (
            this BinaryWriter writer,
            ICollection<char> array
        )
    {
        writer.WriteArray (array, (binaryWriter, c) => binaryWriter.Write (c));
    }
}
