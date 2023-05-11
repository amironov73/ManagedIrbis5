// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IntegerTokenizer.cs -- токенайзер для целых чисел
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizing;

/// <summary>
/// Токенайзер для целых чисел.
/// </summary>
[PublicAPI]
public sealed class IntegerTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var offset = navigator.Position;
        var line = navigator.Line;
        var column = navigator.Column;

        var chr = PeekChar();
        if (!chr.IsArabicDigit())
        {
            return TokenizerResult.Error;
        }

        Span<char> buffer = stackalloc char[16];
        var builder = new ValueStringBuilder (buffer);
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

        var isLong = false;
        var isUnsigned = false;
        while (!IsEof)
        {
            chr = PeekChar();
            if (chr is 'l' or 'L')
            {
                if (isLong)
                {
                    throw new SyntaxException (navigator);
                }

                ReadChar();
                isLong = true;
            }
            else if (chr is 'u' or 'U')
            {
                if (isUnsigned)
                {
                    throw new SyntaxException (navigator);
                }

                ReadChar();
                isUnsigned = true;
            }
            else if (chr is '.' or 'e' or 'E' or 'f' or 'F' or 'm' or 'M')
            {
                // это число с плавающей (или фиксированной) точкой
                navigator.RestorePosition (offset);
                return TokenizerResult.Error;
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
        var value = StringPool.Shared.GetOrAdd (span);
        var token = new Token
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

        return TokenizerResult.Success (token);
    }

    #endregion
}
