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

using System;

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для дробных чисел.
/// </summary>
public sealed class NumberTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var position = navigator.Position;
        var digit = false; // флаг: нам встретилась как минимум одна цифра
        var dot = false; // флаг: нам встретилась точка
        var exponent = false; // флаг: нам встретилась экспонента
        var chr = PeekChar();
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

        Span<char> buffer = stackalloc char[16];
        var builder = new ValueStringBuilder (buffer);
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
                    throw new SyntaxException (navigator);
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
                    throw new SyntaxException (navigator);
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
            if (PeekChar() is not ('F' or 'f' or 'M' or 'm'))
            {
                // это целое число, для них есть отдельный токенайзер
                navigator.RestorePosition (position);
                return null;
            }
        }

        if (!digit)
        {
            // это не похоже на число
            navigator.RestorePosition(position);
            return null;
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

        var span = builder.AsSpan();
        var value = StringPool.Shared.GetOrAdd (span);

        var result = new Token
            (
                kind,
                value,
                line,
                column
            );

        return result;
    }

    #endregion
}
