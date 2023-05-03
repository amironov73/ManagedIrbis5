// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* BigIntegerTokenizer.cs -- токенайзер для BigInteger
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Numerics;

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizers;

/// <summary>
/// Токенайзер для <see cref="BigInteger"/>.
/// </summary>
[PublicAPI]
public sealed class BigIntegerTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var position = navigator.Position;
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

        chr = PeekChar();
        if (chr is not 'b' and not 'B') // TODO подобрать подходящий суффикс
        {
            navigator.RestorePosition (position);
            return TokenizerResult.Error;
        }

        ReadChar();
        var span = builder.AsSpan();
        var value = StringPool.Shared.GetOrAdd (span);

        var result = new Token
            (
                TokenKind.BigInteger,
                value,
                line,
                column,
                position
            );

        return TokenizerResult.Success (result);
    }

    #endregion
}
