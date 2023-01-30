// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* NumberTokenizer.cs -- токенайзер для дробных чисел
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Токенайзер для дробных чисел.
/// </summary>
public sealed class NumberTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        var position = _navigator.Position;
        var digit = false; // флаг: нам встретилась как минимум одна цифра
        var dot = false; // флаг: нам встретилась точка
        var exponent = false; // флаг: нам встретилась экспонента
        char chr = PeekChar();
        if (chr == '.')
        {
            dot = true;
        }
        else if (chr.IsArabicDigit())
        {
            digit = true;
        }
        else
        {
            return null;
        }

        var builder = new StringBuilder();
        builder.Append (ReadChar());
        while (!IsEof)
        {
            chr = PeekChar();

            if (chr is '_')
            {
                ReadChar();
                continue;
            }

            if (chr == '.')
            {
                if (dot)
                {
                    throw new SyntaxException (_navigator);
                }

                dot = true;
            }
            else if (chr.IsArabicDigit())
            {
                digit = true;
            }
            else
            {
                break;
            }

            builder.Append (ReadChar());
        }

        // экспонента
        if (chr is 'e' or 'E')
        {
            exponent = true;
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

        if (!dot && !exponent)
        {
            // это целое число
            _navigator.RestorePosition (position);
            return null;
        }

        if (!digit)
        {
            throw new SyntaxException (_navigator);
        }

        // суффиксы
        var kind = TokenKind.Double;
        chr = PeekChar();
        if (chr is 'F' or 'f')
        {
            kind = TokenKind.Single;
            ReadChar();
        }

        if (chr is 'M' or 'm')
        {
            kind = TokenKind.Decimal;
            ReadChar();
        }

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
