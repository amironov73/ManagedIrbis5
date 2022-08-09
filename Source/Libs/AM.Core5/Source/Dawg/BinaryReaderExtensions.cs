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

/* BinaryReaderExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

#nullable enable

namespace AM.Dawg;

/// <summary>
///
/// </summary>
internal static class BinaryReaderExtensions
{
    public static T[] ReadArray<T>
        (
            this BinaryReader reader,
            Func<BinaryReader, T> read
        )
    {
        var len = reader.ReadInt32();

        return ReadSequence (reader, read).Take (len).ToArray();
    }

    static IEnumerable<T> ReadSequence<T> (BinaryReader reader, Func<BinaryReader, T> read)
    {
        for (;;) yield return read (reader);

        // ReSharper disable once IteratorNeverReturns
    }
}
