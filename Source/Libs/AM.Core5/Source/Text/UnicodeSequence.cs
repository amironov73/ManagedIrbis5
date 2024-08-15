// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* UnicodeSequence.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// A UnicodeSequence is a combination of one or more codepoints.
/// </summary>
public struct UnicodeSequence
    : IComparable<UnicodeSequence>,
    IEquatable<UnicodeSequence>,
    IEquatable<string>
{
    private static readonly char[] SequenceSplitChars = new[] { ',', ' ' };

    private readonly Codepoint[] _codepoints;
    /// <summary>
    ///
    /// </summary>
    public IReadOnlyList<Codepoint> Codepoints => _codepoints;

    /// <summary>
    ///
    /// </summary>
    /// <param name="sequence"></param>
    /// <exception cref="InvalidRangeException"></exception>
    public UnicodeSequence (string sequence)
    {
        if (sequence.Contains ("-"))
        {
            var values = sequence.Split ('-');

            if (values.Length == 2)
            {
                Codepoint begin = new Codepoint (values[0]);
                Codepoint end = new Codepoint (values[1]);
                if (end.Value < begin.Value)
                {
                    throw new InvalidRangeException();
                }

                _codepoints = new Codepoint[end.Value - begin.Value + 1];
                for (int i = 0; begin.Value + i <= end.Value; ++i)
                {
                    _codepoints[i] = new Codepoint (begin.Value + i);
                }
            }
            else
            {
                throw new InvalidRangeException();
            }
        }
        else
        {
            var values = sequence.Split (SequenceSplitChars, StringSplitOptions.RemoveEmptyEntries);
            _codepoints = values.Select (x => new Codepoint (x)).ToArray();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="codepoints"></param>
    // The usage of `params` here should hopefully allocate on the stack for short lengths,
    // making this the most optimized version of the routine.
    public UnicodeSequence (params Codepoint[] codepoints)
    {
        _codepoints = codepoints;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="codepoints"></param>
    public UnicodeSequence (IEnumerable<Codepoint> codepoints)
    {
        _codepoints = codepoints.ToArray();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="codepoint"></param>
    /// <returns></returns>
    public bool Contains (Codepoint codepoint)
    {
        return _codepoints.Contains (codepoint);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IEnumerable<uint> AsUtf32()
    {
        foreach (var cp in _codepoints)
        {
            yield return cp.AsUtf32();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IEnumerable<byte> AsUtf32Bytes()
    {
        foreach (var u32 in AsUtf32())
        {
            // Little Endian byte order
            yield return (byte)(u32 & 0xFF);
            yield return (byte)((u32 >> 8) & 0xFF);
            yield return (byte)((u32 >> 16) & 0xFF);
            yield return (byte)(u32 >> 24);
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IEnumerable<UInt16> AsUtf16()
    {
#if NETSTANDARD2_1_OR_GREATER
            var words = new UInt16[2];
#endif
        foreach (var cp in _codepoints)
        {
#if NETSTANDARD2_1_OR_GREATER
                int count = cp.AsUtf16(words);
                yield return words[0];
                if (count == 2)
                {
                    yield return words[1];
                }
#else
            foreach (var u16 in cp.AsUtf16())
            {
                yield return u16;
            }
#endif
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    // Little Endian byte order
    public IEnumerable<byte> AsUtf16Bytes()
    {
#if NETSTANDARD2_1_OR_GREATER
            var bytes = new byte[4];
            foreach (var cp in _codepoints)
            {
                var count = cp.AsUtf16Bytes(bytes);
                for (int i = 0; i < count; ++i)
                {
                    yield return bytes[i];
                }
            }
#else
        foreach (var u16 in AsUtf16())
        {
            yield return (byte)(u16 & 0xFF);
            yield return (byte)(u16 >> 8);
        }
#endif
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public IEnumerable<byte> AsUtf8()
    {
        foreach (var cp in _codepoints)
        {
            foreach (var b in cp.AsUtf8())
            {
                yield return b;
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string AsString()
    {
        return Encoding.Unicode.GetString (AsUtf16Bytes().ToArray());
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo (UnicodeSequence other)
    {
        return MemoryExtensions.SequenceCompareTo<Codepoint> (_codepoints, other._codepoints);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals (UnicodeSequence other)
    {
        return MemoryExtensions.SequenceEqual<Codepoint> (_codepoints, other._codepoints);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator == (UnicodeSequence a, UnicodeSequence b)
    {
        return a.Equals (b);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator != (UnicodeSequence a, UnicodeSequence b)
    {
        return !a.Equals (b);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator < (UnicodeSequence a, UnicodeSequence b)
    {
        return a.CompareTo (b) < 0;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator > (UnicodeSequence a, UnicodeSequence b)
    {
        return a.CompareTo (b) > 0;
    }

    /// <inheritdoc cref="ValueType.Equals(object?)"/>
    public override bool Equals (object? b)
    {
        return b is UnicodeSequence other && Equals (other);
    }

    /// <inheritdoc cref="ValueType.GetHashCode"/>
    public override int GetHashCode()
    {
        return _codepoints.GetHashCode();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals (string? other)
    {
        return other is not null && other.Codepoints().SequenceEqual (_codepoints);
    }

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return string.Join (" ", Codepoints);
    }
}
