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

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для идентификаторов.
/// </summary>
public sealed class IdentifierTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        var firstIdentifierLetter = Settings.FirstIdentifierLetter;
        var nextIdentifierLetter = Settings.NextIdentifierLetter;
        if (Array.IndexOf (firstIdentifierLetter, PeekChar()) < 0)
        {
            return null;
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

        return new Token
            (
                IsReservedWord (value) ? TokenKind.ReservedWord : TokenKind.Identifier,
                value,
                line,
                column,
                offset
            );
    }

    #endregion
}
