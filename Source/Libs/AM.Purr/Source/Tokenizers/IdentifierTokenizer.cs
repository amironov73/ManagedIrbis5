// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IdentifierTokenizer.cs -- токенайзер для идентификаторов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizers;

/// <summary>
/// Токенайзер для идентификаторов.
/// </summary>
[PublicAPI]
public sealed class IdentifierTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var firstIdentifierLetter = Settings.FirstIdentifierLetter;
        var nextIdentifierLetter = Settings.NextIdentifierLetter;
        if (Array.IndexOf (firstIdentifierLetter, PeekChar()) < 0)
        {
            return TokenizerResult.Error;
        }

        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;
        ReadChar();
        while (!IsEof)
        {
            if (Array.IndexOf (nextIdentifierLetter, PeekChar()) < 0)
            {
                break;
            }

            ReadChar();
        }

        var length = navigator.Position - offset;
        var memory = navigator.Substring (offset, length);
        var value = StringPool.Shared.GetOrAdd (memory.Span);
        var token = new Token
            (
                IsReservedWord (value) ? TokenKind.ReservedWord : TokenKind.Identifier,
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
