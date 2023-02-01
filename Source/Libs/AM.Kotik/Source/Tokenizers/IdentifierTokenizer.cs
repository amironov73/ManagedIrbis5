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

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для идентификаторов.
/// </summary>
public sealed class IdentifierTokenizer
    : Tokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        var firstChar = PeekChar();
        var firstIdentifierLetter = Settings.FirstIdentifierLetter;
        var nextIdentifierLetter = Settings.NextIdentifierLetter;
        if (Array.IndexOf (firstIdentifierLetter, firstChar) < 0)
        {
            return null;
        }

        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (ReadChar());
        while (!IsEof)
        {
            if (Array.IndexOf (nextIdentifierLetter, PeekChar()) < 0)
            {
                break;
            }

            builder.Append (ReadChar());
        }

        var value = builder.ReturnShared();

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
