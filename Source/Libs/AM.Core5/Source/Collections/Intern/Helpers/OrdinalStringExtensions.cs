// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* OrdinalStringExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using AM.Collections.Generic;

#endregion

#nullable enable

namespace AM.Collections.Intern;

internal static class OrdinalStringExtensions
{
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    internal static int GetNonRandomizedHashCode (this string str)
        => str.AsSpan().GetNonRandomizedHashCode();

    internal static int GetNonRandomizedHashCode (this ReadOnlySpan<char> value)
    {
        ref char currentChar = ref MemoryMarshal.GetReference (value);
        ref uint currentUint = ref Unsafe.As<char, uint> (ref currentChar);

        uint hash1 = (5381 << 16) + 5381;
        uint hash2 = hash1;

        int length = value.Length;

        while (length >= 4)
        {
            length -= 4;

            uint Uint0 = currentUint;
            uint Uint1 = Unsafe.AddByteOffset (ref currentUint, (IntPtr)sizeof (uint));
            currentUint = ref Unsafe.AddByteOffset (ref currentUint, (IntPtr)(sizeof (uint) * 2));

            hash1 = (RotateLeft (hash1, 5) + hash1) ^ Uint0;
            hash2 = (RotateLeft (hash2, 5) + hash2) ^ Uint1;
        }

        if (length >= 2)
        {
            length -= 2;

            hash1 = (RotateLeft (hash1, 5) + hash1) ^ currentUint;

            currentChar = ref Unsafe.As<uint, char> (ref Unsafe.AddByteOffset (ref currentUint, (IntPtr)sizeof (uint)));
        }

        if (length > 0)
        {
            uint val = currentChar;
            hash2 = (RotateLeft (hash2, 5) + hash2) ^ val;
        }

        return (int)(hash1 + (hash2 * 1566083941));
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int GetRandomizedHashCode (this string str)
        => GetRandomizedHashCode (str.AsSpan());

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    public static int GetRandomizedHashCode (this ReadOnlySpan<char> value)
    {
        ulong seed = Marvin.DefaultSeed;

        // Multiplication below will not overflow since going from positive Int32 to UInt32.
        return Marvin.ComputeHash32 (ref Unsafe.As<char, byte> (ref MemoryMarshal.GetReference (value)),
            (uint)value.Length * 2 /* in bytes, not chars */, (uint)seed, (uint)(seed >> 32));
    }

    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    private static uint RotateLeft (uint value, int offset)
        => (value << offset) | (value >> (32 - offset));
}
