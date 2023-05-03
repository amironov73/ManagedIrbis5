// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* HexTokenizer.cs -- токенайзер для чисел в шестнадцатеричной системе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizers;

/// <summary>
/// Токенайзер для чисел в шестнадцатеричной системе счичления.
/// </summary>
[PublicAPI]
public sealed class HexTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var offset = navigator.Position;
        var line = navigator.Line;
        var column = navigator.Column;

        // префикс '0x'
        var chr = PeekChar();
        if (chr != '0' || navigator.LookAhead (1) is not ('x' or 'X'))
        {
            return TokenizerResult.Error;
        }

        Span<char> buffer = stackalloc char[16];
        var builder = new ValueStringBuilder (buffer);
        var kind = TokenKind.Hex32;
        ReadChar();
        ReadChar();
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
            kind = TokenKind.Hex64;
        }

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
