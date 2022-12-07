// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* FlagValue.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;

using AM.Linguistics.Hunspell.Infrastructure;

#endregion

#nullable enable

namespace AM.Linguistics.Hunspell;

/// <summary>
///
/// </summary>
public readonly struct FlagValue
    : IEquatable<FlagValue>,
    IEquatable<int>,
    IEquatable<char>,
    IComparable<FlagValue>,
    IComparable<int>,
    IComparable<char>
{
    private const char ZeroValue = '\0';

    /// <summary>
    ///
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static implicit operator int (FlagValue flag) => flag.value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static implicit operator char (FlagValue flag) => flag.value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator != (FlagValue a, FlagValue b) => a.value != b.value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator == (FlagValue a, FlagValue b) => a.value == b.value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator >= (FlagValue a, FlagValue b) => a.value >= b.value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator <= (FlagValue a, FlagValue b) => a.value <= b.value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator > (FlagValue a, FlagValue b) => a.value > b.value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool operator < (FlagValue a, FlagValue b) => a.value < b.value;

    internal static FlagValue Create (char high, char low) => new (unchecked ((char)((high << 8) | low)));

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <param name="mode"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool TryParseFlag (string text, FlagMode mode, out FlagValue value) =>
        TryParseFlag (text.AsSpan(), mode, out value);

    internal static bool TryParseFlag (ReadOnlySpan<char> text, FlagMode mode, out FlagValue value)
    {
        if (text.IsEmpty)
        {
            value = default;
            return false;
        }

        switch (mode)
        {
            case FlagMode.Char:
                value = new FlagValue (text[0]);
                return true;

            case FlagMode.Long:
                var a = text[0];
                value = text.Length >= 2
                    ? Create (a, text[1])
                    : new FlagValue (a);
                return true;

            case FlagMode.Num:
                return TryParseNumberFlag (text, out value);

            case FlagMode.Uni:
            default:
                throw new NotSupportedException();
        }
    }

    private static bool TryParseNumberFlag (ReadOnlySpan<char> text, out FlagValue value)
    {
        if (!text.IsEmpty && IntEx.TryParseInvariant (text, out var integerValue) &&
            integerValue is >= char.MinValue and <= char.MaxValue)
        {
            value = new FlagValue (unchecked ((char)integerValue));
            return true;
        }

        value = default;
        return false;
    }

    internal static FlagValue[] ParseFlagsInOrder (ReadOnlySpan<char> text, FlagMode mode)
    {
        return mode switch
        {
            FlagMode.Char => text.IsEmpty ? Array.Empty<FlagValue>() : ConvertCharsToFlagsInOrder (text),
            FlagMode.Long => ParseLongFlagsInOrder (text),
            FlagMode.Num => ParseNumberFlagsInOrder (text).ToArray(),
            _ => throw new NotSupportedException()
        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public static FlagSet ParseFlags (string text, FlagMode mode)
    {
        return ParseFlags (text.AsSpan(), mode);
    }

    internal static FlagSet ParseFlags (ReadOnlySpan<char> text, FlagMode mode) =>
        FlagSet.TakeArray (ParseFlagsInOrder (text, mode));

    private static FlagValue[] ParseLongFlagsInOrder (ReadOnlySpan<char> text)
    {
        if (text.IsEmpty) return Array.Empty<FlagValue>();

        var flags = new FlagValue[(text.Length + 1) / 2];
        var flagWriteIndex = 0;
        var lastIndex = text.Length - 1;
        for (var i = 0; i < lastIndex; i += 2, flagWriteIndex++)
            flags[flagWriteIndex] = Create (text[i], text[i + 1]);

        if (flagWriteIndex < flags.Length) flags[flagWriteIndex] = new FlagValue (text[lastIndex]);

        return flags;
    }

    private static List<FlagValue> ParseNumberFlagsInOrder (ReadOnlySpan<char> text)
    {
        if (text.IsEmpty) return new List<FlagValue> (0);

        var flags = new List<FlagValue>();
        text.Split (',', (part, _) =>
        {
            if (TryParseNumberFlag (part, out var value)) flags.Add (value);

            return true;
        });

        return flags;
    }

    private static FlagValue[] ConvertCharsToFlagsInOrder (ReadOnlySpan<char> text)
    {
        var values = new FlagValue[text.Length];
        for (var i = 0; i < values.Length; i++) values[i] = new FlagValue (text[i]);

        return values;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public FlagValue (char value) => this.value = value;

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    public FlagValue (int value) => this.value = checked ((char)value);

    private readonly char value;

    /// <summary>
    ///
    /// </summary>
    public bool HasValue => value != ZeroValue;

    /// <summary>
    ///
    /// </summary>
    public bool IsZero => value == ZeroValue;

    /// <summary>
    ///
    /// </summary>
    public bool IsWildcard => value is '*' or '?';

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals (FlagValue other) => other.value == value;

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals (int other) => other == value;

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals (char other) => other == value;

    /// <inheritdoc cref="ValueType.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        return obj switch
        {
            FlagValue flagValue => Equals (flagValue),
            int intValue => Equals (intValue),
            char charValue => Equals (charValue),
            _ => false
        };
    }

    /// <inheritdoc cref="ValueType.GetHashCode"/>
    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    /// <inheritdoc cref="IComparable{T}.CompareTo"/>
    public int CompareTo (FlagValue other)
    {
        return value.CompareTo (other.value);
    }

    /// <inheritdoc cref="IComparable{T}.CompareTo"/>
    public int CompareTo (int other)
    {
        return ((int)value).CompareTo (other);
    }

    /// <inheritdoc cref="IComparable{T}.CompareTo"/>
    public int CompareTo (char other)
    {
        return value.CompareTo (other);
    }

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return ((int)value).ToString (CultureInfo.InvariantCulture);
    }
}
