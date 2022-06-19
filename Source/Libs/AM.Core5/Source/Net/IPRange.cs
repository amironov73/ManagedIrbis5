// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* IPRange.cs -- диапазон IP-адресов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Net;

/// <summary>
/// Диапазон IP-адресов.
/// </summary>
public sealed class IPRange
    : IEquatable<IPRange>
{
    #region Properties

    /// <summary>
    /// Начальный адрес диапазона.
    /// </summary>
    public IPAddress Begin { get; }

    /// <summary>
    /// Конечный адрес диапазона.
    /// </summary>
    public IPAddress End { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IPRange
        (
            IPAddress begin,
            IPAddress end
        )
    {
        Sure.NotNull (begin);
        Sure.NotNull (end);

        Begin = begin;
        End = end;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Содержит ли диапазон указанный IP-адрес?
    /// </summary>
    public bool Contains
        (
            IPAddress address
        )
    {
        Sure.NotNull (address);

        var bytes = address.GetAddressBytes();

        return Bits.MoreOrEqual (Begin.GetAddressBytes(), bytes)
               && Bits.LessOrEqual (End.GetAddressBytes(), bytes);
    }

    /// <summary>
    /// Содержит ли наш диапазон указанный поддиапазон?
    /// </summary>
    public bool Contains
        (
            IPRange range
        )
    {
        Sure.NotNull (range);

        return Contains (range.Begin) && Contains (range.End);
    }

    /// <summary>
    /// Разбор текстовой спецификации диапазона.
    /// </summary>
    public static IPRange Parse
        (
            string specification
        )
    {
        Sure.NotNullNorEmpty (specification);

        specification = specification.Trim();
        if (specification == "*")
        {
            return new IPRange
                (
                    IPAddress.Any,
                    IPAddress.Broadcast
                );
        }

        specification = specification.Replace
            (
                " ",
                string.Empty
            );

        var match = Regex.Match
            (
                specification,
                @"^(?<addr>[\da-f\.]+)/(?<maskLen>\d+)$",
                RegexOptions.IgnoreCase
            );
        if (match.Success)
        {
            var addressBytes = IPAddress.Parse (match.Groups["addr"].Value)
                .GetAddressBytes();
            var bitMask = Bits.GetBitMask
                (
                    addressBytes.Length,
                    match.Groups["maskLen"].Value.ParseInt32()
                );
            addressBytes = Bits.And (addressBytes, bitMask);
            var begin = new IPAddress (addressBytes);
            var end = new IPAddress (Bits.Or (addressBytes, Bits.Not (bitMask)));

            return new IPRange (begin, end);
        }

        match = Regex.Match
            (
                specification,
                @"^(?<addr>[\da-f\.]+)$",
                RegexOptions.IgnoreCase
            );
        if (match.Success)
        {
            var both = IPAddress.Parse (specification);

            return new IPRange (both, both);
        }

        match = Regex.Match
            (
                specification,
                @"^(?<begin>[\da-f\.]+)-(?<end>[\da-f\.]+)$",
                RegexOptions.IgnoreCase
            );
        if (match.Success)
        {
            var begin = IPAddress.Parse (match.Groups["begin"].Value);
            var end = IPAddress.Parse (match.Groups["end"].Value);

            return new IPRange (begin, end);
        }

        throw new ArgumentOutOfRangeException
            (
                nameof (specification),
                "Bad IP range specification"
            );
    }

    #endregion

    #region IEquatable<T> members

    /// <inheritdoc cref="IEquatable{T}.Equals(T?)"/>
    public bool Equals
        (
            IPRange? other
        )
    {
        return other is not null
               && Begin.Equals (other.Begin)
               && End.Equals (other.End);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    [ExcludeFromCodeCoverage]
    public override bool Equals
        (
            object? obj
        )
    {
        return ReferenceEquals (this, obj)
            || obj is IPRange other && Equals (other);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    [ExcludeFromCodeCoverage]
    public override int GetHashCode()
    {
        return HashCode.Combine (Begin, End);
    }

    /// <inheritdoc cref="ToString"/>
    public override string ToString()
    {
        return Begin.Equals (End)
            ? Begin.ToString()
            : $"{Begin}-{End}";
    }

    #endregion
}
