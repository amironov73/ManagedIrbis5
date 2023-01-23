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
using System.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Токенайзер для идентификаторов.
/// </summary>
public sealed class IdentifierTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var firstChar = PeekChar();
        var firstIdentifierLetter = Settings.FirstIdentifierLetter;
        var nextIdentifierLetter = Settings.NextIdentifierLetter;
        if (Array.IndexOf (firstIdentifierLetter, firstChar) < 0)
        {
            return null;
        }

        var builder = new StringBuilder();
        var line = _navigator.Line;
        var column = _navigator.Column;
        builder.Append (ReadChar());

        while (!IsEof)
        {
            if (Array.IndexOf (nextIdentifierLetter, PeekChar()) < 0)
            {
                break;
            }

            builder.Append (ReadChar());
        }

        var value = builder.ToString();

        return new Token
            (
                IsReservedWord (value) ? TokenKind.ReservedWord : TokenKind.Identifier,
                value,
                line,
                column
            );
    }

    #endregion
}
