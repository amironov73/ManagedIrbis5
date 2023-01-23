// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* NumberTokenizer.cs -- токенайзер для чисел
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Токенайзер для целых и дробных чисел (включая шестнадцатиричные).
/// </summary>
public sealed class NumberTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        StringBuilder builder;
        var line = _navigator.Line;
        var column = _navigator.Column;
        var isFloat = false;
        char chr = PeekChar();
        if (!chr.IsArabicDigit())
        {
            if (chr is '.' && _navigator.LookAhead (1).IsArabicDigit())
            {
                builder = new StringBuilder();
                goto IS_FLOAT;
            }

            return null;
        }

        builder = new StringBuilder();

        // префикс '0x'
        if (chr == '0' && _navigator.LookAhead (1) is 'x')
        {
            ReadChar();
            ReadChar();

            var hex = TokenKind.Hex32;
            while (!IsEof)
            {
                chr = PeekChar();

                if (chr is '_')
                {
                    ReadChar();
                    continue;
                }

                if (!"0123456789ABCDEFabcdef".Contains (chr))
                {
                    break;
                }

                builder.Append (ReadChar());
            }

            if (chr is 'L' or 'l')
            {
                ReadChar();
                hex = TokenKind.Hex64;
            }

            return new Token (hex, builder.ToString(), line, column);
        }

        while (!IsEof)
        {
            chr = PeekChar();

            if (chr is '_')
            {
                ReadChar();
                continue;
            }

            if (!chr.IsArabicDigit())
            {
                break;
            }

            builder.Append (ReadChar());
        }

        // дробное число
        IS_FLOAT: if (chr == '.')
        {
            isFloat = true;
            builder.Append (ReadChar());

            while (!IsEof)
            {
                chr = PeekChar();

                if (chr is '_')
                {
                    ReadChar();
                    continue;
                }

                if (!chr.IsArabicDigit())
                {
                    break;
                }

                builder.Append (ReadChar());
            }
        }

        // экспонента
        if (chr is 'e' or 'E')
        {
            isFloat = true;
            builder.Append (ReadChar());
            chr = PeekChar();

            if (chr is '+' or '-')
            {
                builder.Append (ReadChar());
                chr = PeekChar();
                if (!chr.IsArabicDigit())
                {
                    throw new SyntaxException (_navigator);
                }
            }

            while (!IsEof)
            {
                chr = PeekChar();
                if (!chr.IsArabicDigit())
                {
                    break;
                }

                builder.Append (ReadChar());
            }
        }

        var isU = false;
        var isL = false;
        var isF = false;
        var isM = false;

        // суффиксы
        if (isFloat)
        {
            chr = PeekChar();
            if (chr is 'F' or 'f')
            {
                isF = true;
                ReadChar();
            }

            if (chr is 'M' or 'm')
            {
                isM = true;
                ReadChar();
            }
        }
        else
        {
            while (!IsEof)
            {
                var canContinue = true;
                switch (chr)
                {
                    case 'u':
                    case 'U':
                        if (isU || isF || isFloat || isM)
                        {
                            // нельзя указывать больше одного раза
                            throw new SyntaxException (_navigator);
                        }

                        isU = true;
                        break;

                    case 'l':
                    case 'L':
                        if (isL || isF || isFloat || isM)
                        {
                            // нельзя указывать больше одного раза
                            throw new SyntaxException (_navigator);
                        }

                        isL = true;
                        break;

                    case 'f':
                    case 'F':
                        if (isU || isF || isL || isM)
                        {
                            throw new SyntaxException (_navigator);
                        }

                        isFloat = true;
                        isF = true;
                        break;

                    case 'm':
                    case 'M':
                        if (isU || isL || isF || isM)
                        {
                            throw new SyntaxException (_navigator);
                        }

                        isM = true;
                        break;

                    default:
                        canContinue = false;
                        break;
                }

                if (!canContinue)
                {
                    break;
                }

                ReadChar();
                chr = PeekChar();
            }
        }

        var kind = isFloat
            ? (
                isM
                    ? TokenKind.Decimal
                    : isF
                        ? TokenKind.Single
                        : TokenKind.Double
            )
            : (
                isM
                    ? TokenKind.Decimal
                    : isL
                        ? isU ? TokenKind.UInt64 : TokenKind.Int64
                        : isU
                            ? TokenKind.UInt32
                            : TokenKind.Int32
            );

        var result = new Token
            (
                kind,
                builder.ToString(),
                line,
                column
            );

        return result;
    }

    #endregion
}
