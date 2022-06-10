// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* NumberParser.cs -- парсер барсиковых чисел-констант
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

using Pidgin;

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Парсер барсиковых чисел-констант.
/// </summary>
internal sealed class NumberParser
    : Parser<Token, ConstantNode>
{
    #region Private members

    private static readonly char[] _suffixes = new[]
    {
        'f', 'F', 'l', 'L', 'm', 'M', 'u', 'U'
    };

    private static readonly string[] _kinds =
    {
        TokenKind.Int32, TokenKind.Int64, TokenKind.UInt32,
        TokenKind.UInt64, TokenKind.Single, TokenKind.Double,
        TokenKind.Decimal
    };

    private static ReadOnlySpan<char> _Trim (string? text)
        => text.ThrowIfNullOrEmpty ().AsSpan().TrimEnd (_suffixes);

    #endregion

    #region Parser<T1,T2> members

    /// <inheritdoc cref="Parser{TToken,T}.TryParse"/>
    public override bool TryParse
        (
            ref ParseState<Token> state,
            ref PooledList<Expected<Token>> expecteds,
            out ConstantNode result
        )
    {
        result = default!;
        if (!state.HasCurrent)
        {
            return false;
        }

        var current = state.Current;
        if (current.Kind.IsOneOf (_kinds))
        {
            const NumberStyles none = NumberStyles.None;
            const NumberStyles floating = NumberStyles.AllowDecimalPoint
                | NumberStyles.AllowLeadingSign;
            var invariant = CultureInfo.InvariantCulture;
            var value = current.Kind switch
            {
                TokenKind.Int32 => int.Parse
                    (
                        _Trim (current.Value),
                        none,
                        invariant
                    ) as object,
                TokenKind.Int64 => long.Parse
                    (
                        _Trim (current.Value),
                        none,
                        invariant
                    ),
                TokenKind.UInt32 => uint.Parse
                    (
                        _Trim (current.Value),
                        none,
                        invariant
                    ),
                TokenKind.UInt64 => ulong.Parse
                    (
                        _Trim (current.Value),
                        none,
                        invariant
                    ),
                TokenKind.Single => float.Parse
                    (
                        _Trim (current.Value),
                        floating,
                        invariant
                    ),
                TokenKind.Double => double.Parse
                    (
                        _Trim (current.Value),
                        floating,
                        invariant
                    ),
                TokenKind.Decimal => decimal.Parse
                    (
                        _Trim (current.Value),
                        floating,
                        invariant
                    ),

                _ => throw new ArgumentOutOfRangeException()
            };

            result = new ConstantNode (value);
            state.Advance();
            return true;
        }

        return false;
    }

    #endregion
}
