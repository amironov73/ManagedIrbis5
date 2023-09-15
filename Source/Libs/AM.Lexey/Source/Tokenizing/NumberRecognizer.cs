// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* NumberRecognizer.cs -- распознает дробные числа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает дробные числа.
/// </summary>
[PublicAPI]
public sealed class NumberTokenizer
    : ITokenRecognizer
{
    #region ITokenRecognizer members

    /// <inheritdoc cref="ITokenRecognizer.RecognizeToken"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var line = navigator.Line;
        var column = navigator.Column;
        var state = navigator.SaveState();
        var digit = false; // флаг: нам встретилась как минимум одна цифра
        var dot = false; // флаг: нам встретилась точка
        var exponent = false; // флаг: нам встретилась экспонента
        var chr = navigator.PeekChar();
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
            return default;
        }

        Span<char> buffer = stackalloc char[16];
        var builder = new ValueStringBuilder (buffer);
        builder.Append (navigator.ReadChar());
        while (!navigator.IsEOF)
        {
            chr = navigator.PeekChar();

            if (chr is '_')
            {
                navigator.ReadChar();
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

            builder.Append (navigator.ReadChar());
        }

        // экспонента
        if (chr is 'e' or 'E')
        {
            exponent = true;
            builder.Append (navigator.ReadChar());
            chr = navigator.PeekChar();

            if (chr is '+' or '-')
            {
                builder.Append (navigator.ReadChar());
                chr = navigator.PeekChar();
                if (!chr.IsArabicDigit())
                {
                    throw new SyntaxException (navigator);
                }
            }

            while (!navigator.IsEOF)
            {
                chr = navigator.PeekChar();
                if (!chr.IsArabicDigit())
                {
                    break;
                }

                builder.Append (navigator.ReadChar());
            }
        }

        if (!dot && !exponent)
        {
            if (navigator.PeekChar() is not ('F' or 'f' or 'M' or 'm'))
            {
                // это целое число, для них есть отдельный токенайзер
                state.Restore();
                return default;
            }
        }

        if (!digit)
        {
            // это не похоже на число
            state.Restore();
            return default;
        }

        // суффиксы
        var kind = TokenKind.Double;
        chr = navigator.PeekChar();
        if (chr is 'F' or 'f')
        {
            kind = TokenKind.Single;
            navigator.ReadChar();
        }

        if (chr is 'M' or 'm')
        {
            kind = TokenKind.Decimal;
            navigator.ReadChar();
        }

        var span = builder.AsSpan();
        var value = StringPool.Shared.GetOrAdd (span);
        var result = new Token
            (
                kind,
                value,
                line,
                column
            )
            {
                UserData = value
            };

        return result;
    }

    #endregion
}
