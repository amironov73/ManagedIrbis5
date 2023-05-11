// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LiteralParser.cs -- парсер для литералов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Numerics;

using AM.Purr.Tokenizing;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Парсер для литералов: чисел, строк, логических значений.
/// </summary>
[PublicAPI]
public sealed class LiteralParser
    : Parser<object>
{
    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out object result
        )
    {
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var current = state.Current;
        var value = current.Value!;
        var invariant = CultureInfo.InvariantCulture;
        switch (current.Kind)
        {
            case TokenKind.Char:
                result = value[0];
                state.Advance();
                break;

            case TokenKind.String:
                result = value;
                state.Advance();
                break;

            case TokenKind.Int32:
                result = int.Parse (value, invariant);
                state.Advance();
                break;

            case TokenKind.UInt32:
                result = uint.Parse (value, invariant);
                state.Advance();
                break;

            case TokenKind.Int64:
                result = long.Parse (value, invariant);
                state.Advance();
                break;

            case TokenKind.UInt64:
                result = ulong.Parse (value, invariant);
                state.Advance();
                break;

            case TokenKind.Hex32:
                result = Convert.ToUInt32 (value, 16);
                break;

            case TokenKind.Hex64:
                result = Convert.ToUInt64 (value, 16);
                break;

            case TokenKind.Single:
                result = float.Parse (value, invariant);
                state.Advance();
                break;

            case TokenKind.Double:
                result = double.Parse (value, invariant);
                state.Advance();
                break;

            case TokenKind.Decimal:
                result = decimal.Parse (value, invariant);
                state.Advance();
                break;

            case TokenKind.BigInteger:
                result = BigInteger.Parse (value, invariant);
                state.Advance();
                break;

            case TokenKind.ReservedWord:
                switch (value)
                {
                    case "null":
                        result = null!;
                        state.Advance();
                        break;

                    case "true":
                        result = true;
                        state.Advance();
                        break;

                    case "false":
                        result = false;
                        state.Advance();
                        break;

                    default:
                        return DebugSuccess (state, false);
                }

                break;

            default:
                return DebugSuccess (state, false);
        }

        return DebugSuccess (state, true);
    }

    #endregion
}
