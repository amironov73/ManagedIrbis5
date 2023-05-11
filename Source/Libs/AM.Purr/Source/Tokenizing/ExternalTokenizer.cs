// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ExternalTokenizer.cs -- токенайзер для внешнего кода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizing;

/// <summary>
/// Токенайзер для внешнего кода.
/// </summary>
[PublicAPI]
public sealed class ExternalTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var offset = navigator.Position;
        var line = navigator.Line;
        var column = navigator.Column;

        if (PeekChar() != '`')
        {
            return TokenizerResult.Error;
        }

        ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '`')
            {
                break;
            }
        }

        if (chr != '`')
        {
            return TokenizerResult.Error;
        }

        var memory = navigator.Substring
            (
                offset + 1,
                navigator.Position - offset - 2
            );
        var value = StringPool.Shared.GetOrAdd (memory.Span);
        var token = new Token
            (
                TokenKind.External,
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
