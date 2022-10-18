// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CodepointRange.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// A <c>Range</c> constitutes a range of <see>Codepoint</see> defined by the
/// <c>Begin</c> and <c>End</c> values, both of which are inclusive.
/// </summary>
public class CodepointRange
    : IComparable<CodepointRange>, IEquatable<CodepointRange>
{
    /// <summary>
    /// The first codepoint in the range, inclusive.
    /// </summary>
    public readonly Codepoint Begin;
    /// <summary>
    /// The last codepoint in the range, inclusive.
    /// </summary>
    public readonly Codepoint End;

    /// <summary>
    /// Create a range constituting <c>Begin</c> and <c>End</c> codepoints. The two
    /// values may be the same, but <paramref name="begin"/> must be less than or
    /// equal to <paramref name="end"/>
    /// </summary>
    public CodepointRange(Codepoint begin, Codepoint end)
    {
        Begin = begin;
        End = end;
    }

    /// <summary>
    /// Create a range constituting a single codepoint (<c>Begin == End</c>).
    /// </summary>
    public CodepointRange(Codepoint value)
    {
        Begin = value;
        End = value;
    }

    public bool Contains(Codepoint codepoint)
    {
        return codepoint >= Begin && codepoint <= End;
    }

    static readonly string[] RangeSplit = new[] { "-", "–", "—", ".." };
    // Either a single hex codepoint or two separated by a hyphen

    /// <summary>
    /// Create a range from a string (hexadecimal) description of the range. A range
    /// may be a single codepoint (in which case <c>Begin == End</c>) or a start and
    /// end codepoint separated by a hyphen or two dots.
    ///
    /// Examples of valid ranges: <c>CodepointRange("0030..0039")</c>, <c>CodepointRange("0040")</c>,
    /// and <c>CodepointRange("0600–06FF")</c>
    /// </summary>
    public CodepointRange(string range)
    {
        // These are all different hyphens used on Wikipedia and in the UTR
        var values = range.Split(RangeSplit, StringSplitOptions.RemoveEmptyEntries);
        Begin = UInt32.Parse(values[0], System.Globalization.NumberStyles.HexNumber);

        if (values.Length == 1)
        {
            End = Begin;
        }
        else if (values.Length == 2)
        {
            End = UInt32.Parse(values[1], System.Globalization.NumberStyles.HexNumber);
        }
        else
        {
            throw new InvalidRangeException();
        }
    }

    public IEnumerable<UInt32> AsUtf32Sequence
    {
        get
        {
            for (UInt32 i = 0; Begin + i <= End; ++i)
            {
                yield return new Codepoint(Begin + i).AsUtf32();
            }
        }
    }

    public IEnumerable<UInt16> AsUtf16Sequence
    {
        get
        {
            for (var i = 0; Begin + i <= End; ++i)
            {
                foreach (var utf16 in new Codepoint(Begin + i).AsUtf16())
                {
                    yield return utf16;
                }
            }
        }
    }

    public IEnumerable<byte> AsUtf8Sequence
    {
        get
        {
            for (var i = 0; Begin + i <= End; ++i)
            {
                foreach (var utf8 in new Codepoint(Begin + i).AsUtf8())
                {
                    yield return utf8;
                }
            }
        }
    }

    public int CompareTo(CodepointRange? other)
    {
        if (other is null)
        {
            return 1;
        }

        int compare = (int)Begin.Value - (int)other.Begin.Value;
        if (compare != 0)
        {
            return compare;
        }

        return End.CompareTo(other.End);
    }

    public bool Equals(CodepointRange? other)
    {
        return other is CodepointRange range
               && Begin == range.Begin
               && End == range.End;
    }

    public override bool Equals(Object? obj)
    {
        return obj is CodepointRange other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Begin.GetHashCode() ^ End.GetHashCode();
    }

    public static bool operator ==(CodepointRange? lhs, CodepointRange? rhs)
    {
        if (lhs is null || rhs is null)
        {
            return object.Equals(lhs, rhs);
        }

        return lhs.Equals(rhs);
    }

    public static bool operator !=(CodepointRange? lhs, CodepointRange? rhs)
    {
        return !(lhs == rhs);
    }

    public override string ToString()
    {
        return $"{Begin}..{End}";
    }
}
