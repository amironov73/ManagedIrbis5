// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* AsnUtility.cs -- полезные методы для работы с ASN.1
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Formats.Asn1;

#endregion

#nullable enable

namespace AM.Asn1;

/// <summary>
/// Полезные методы для работы с ASN.1
/// </summary>
public static class AsnUtility
{
    #region Public methods

    /// <summary>
    /// Чтение одного среднего целого со знаком.
    /// </summary>
    /// <remarks>
    /// При неудаче бросает исключение.
    /// </remarks>
    public static int ReadInt32
        (
            this AsnReader reader
        )
    {
        Sure.NotNull (reader);

        if (!reader.TryReadInt32 (out var result))
        {
            throw new ApplicationException();
        }

        return result;
    }

    /// <summary>
    /// Чтение одного длинного целого со знаком.
    /// </summary>
    /// <remarks>
    /// При неудаче бросает исключение.
    /// </remarks>
    public static long ReadInt64
        (
            this AsnReader reader
        )
    {
        Sure.NotNull (reader);

        if (!reader.TryReadInt64 (out var result))
        {
            throw new ApplicationException();
        }

        return result;
    }

    /// <summary>
    /// Чтение одного среднего целого без знака.
    /// </summary>
    /// <remarks>
    /// При неудаче бросает исключение.
    /// </remarks>
    public static uint ReadUInt32
        (
            this AsnReader reader
        )
    {
        Sure.NotNull (reader);

        if (!reader.TryReadUInt32 (out var result))
        {
            throw new ApplicationException();
        }

        return result;
    }

    /// <summary>
    /// Чтение одного длинного целого без знака.
    /// </summary>
    /// <remarks>
    /// При неудаче бросает исключение.
    /// </remarks>
    public static ulong ReadUInt64
        (
            this AsnReader reader
        )
    {
        Sure.NotNull (reader);

        if (!reader.TryReadUInt64 (out var result))
        {
            throw new ApplicationException();
        }

        return result;
    }

    #endregion
}
