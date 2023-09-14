// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IntegerRecognizer.cs -- распознает целые числа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает целые числа без знака.
/// </summary>
[PublicAPI]
public sealed class IntegerRecognizer
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

        var state = navigator.SaveState();
        var line = navigator.Line;
        var column = navigator.Column;

        var chr = navigator.PeekChar();
        if (!chr.IsArabicDigit())
        {
            return default;
        }

        Span<char> buffer = stackalloc char[16];
        var builder = new ValueStringBuilder (buffer);
        while (!navigator.IsEOF)
        {
            chr = navigator.PeekChar();

            if (chr is '_')
            {
                navigator.ReadChar();
                continue;
            }

            if (!chr.IsArabicDigit())
            {
                break;
            }

            builder.Append (navigator.ReadChar());
        }

        var isLong = false;
        var isUnsigned = false;
        while (!navigator.IsEOF)
        {
            chr = navigator.PeekChar();
            if (chr is 'l' or 'L')
            {
                if (isLong)
                {
                    throw new SyntaxException (navigator);
                }

                navigator.ReadChar();
                isLong = true;
            }
            else if (chr is 'u' or 'U')
            {
                if (isUnsigned)
                {
                    throw new SyntaxException (navigator);
                }

                navigator.ReadChar();
                isUnsigned = true;
            }
            else if (chr is '.' or 'e' or 'E' or 'f' or 'F' or 'm' or 'M')
            {
                // это число с плавающей (или фиксированной) точкой
                state.Restore();
                return default;
            }
            else
            {
                break;
            }
        }

        var kind = isLong
            ? isUnsigned
                ? TokenKind.UInt64 : TokenKind.Int64
            : isUnsigned
                ? TokenKind.UInt32 : TokenKind.Int32;

        var span = builder.AsSpan();
        var offset = state.Position;
        var value = StringPool.Shared.GetOrAdd (span);
        var result = new Token
            (
                kind,
                value,
                line,
                column,
                offset
            )
            {
                UserData = value
            };

        return result;
    }

    #endregion
}
