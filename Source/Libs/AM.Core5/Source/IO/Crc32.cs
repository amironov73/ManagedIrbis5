// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Crc32.cs -- хэш-функция
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Implements a 32-bit CRC hash algorithm compatible with Zip etc.
/// </summary>
/// <remarks>
/// Crc32 should only be used for backward compatibility with older file formats
/// and algorithms. It is not secure enough for new applications.
/// If you need to call multiple times for the same data either use the HashAlgorithm
/// interface or remember that the result of one Compute call needs to be ~ (XOR) before
/// being passed in as the seed for the next Compute call.
/// </remarks>
public static class Crc32
{
    #region Constants

    private const uint DefaultPolynomial = 0xedb88320u;
    private const uint DefaultSeed = 0xffffffffu;

    #endregion

    #region Private members

    private static uint[]? defaultTable;

    private static uint[] InitializeTable
        (
            uint polynomial
        )
    {
        if (polynomial == DefaultPolynomial && defaultTable != null)
        {
            return defaultTable;
        }

        var createTable = new uint[256];
        for (var i = 0; i < 256; i++)
        {
            var entry = (uint) i;
            for (var j = 0; j < 8; j++)
            {
                if ((entry & 1) == 1)
                {
                    entry = (entry >> 1) ^ polynomial;
                }
                else
                {
                    entry >>= 1;
                }
            }

            createTable[i] = entry;
        }

        if (polynomial == DefaultPolynomial)
        {
            defaultTable = createTable;
        }

        return createTable;
    }

    private static uint CalculateHash
        (
            uint[] table,
            uint seed,
            ReadOnlySpan<byte> buffer
        )
    {
        var hash = seed;
        foreach (var t in buffer)
        {
            hash = (hash >> 8) ^ table[t ^ hash & 0xff];
        }

        return hash;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Вычисление CRC32.
    /// </summary>
    public static uint Compute (byte[] buffer)
    {
        return Compute (DefaultSeed, buffer);
    }

    /// <summary>
    /// Вычисление CRC32.
    /// </summary>
    public static uint Compute
        (
            ReadOnlySpan<byte> buffer
        )
    {
        return ~CalculateHash (InitializeTable (DefaultPolynomial), DefaultSeed, buffer);
    }

    /// <summary>
    /// Вычисление CRC32.
    /// </summary>
    public static uint Compute
        (
            uint seed,
            ReadOnlySpan<byte> buffer
        )
    {
        return Compute (DefaultPolynomial, seed, buffer);
    }

    /// <summary>
    /// Вычисление CRC32.
    /// </summary>
    public static uint Compute
        (
            uint polynomial,
            uint seed,
            ReadOnlySpan<byte> buffer
        )
    {
        return ~CalculateHash (InitializeTable (polynomial), seed, buffer);
    }

    #endregion
}
