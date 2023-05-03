// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* CharTokenizer.cs -- токенайзер для отдельных символов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizers;

/// <summary>
/// Токенайзер для отдельных символов.
/// </summary>
[PublicAPI]
public sealed class CharacterTokenizer
    : Tokenizer
{
    #region Tokenizer methods

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var offset = navigator.Position;
        var line = navigator.Line;
        var column = navigator.Column;
        if (PeekChar() != '\'')
        {
            return TokenizerResult.Error;
        }

        ReadChar(); // съедаем открывающий апостроф
        var result = ReadChar();
        if (result == '\\')
        {
            if (IsEof)
            {
                throw new SyntaxException (navigator);
            }

            Span<char> buffer1 = stackalloc char[8];
            Span<char> buffer2 = stackalloc char[8];
            var builder1 = new ValueStringBuilder (buffer1);
            var builder2 = new ValueStringBuilder (buffer2);
            builder1.Append (result);
            while (!IsEof)
            {
                result = PeekChar();
                if (result == '\'')
                {
                    break;
                }

                ReadChar();
                builder1.Append (result);
            }

            var raw = builder1.AsSpan();
            TextUtility.UnescapeText (ref builder2, raw);
            var unescaped = builder2.AsSpan();
            if (unescaped.Length != 1)
            {
                throw new SyntaxException (navigator);
            }
            result = unescaped[0];
        }

        if (ReadChar() != '\'')
        {
            return TokenizerResult.Error;
        }

        var value = result.ToString();
        var token = new Token (TokenKind.Char, value, line, column, offset)
        {
            UserData = value
        };

        return TokenizerResult.Success (token);
    }

    #endregion
}
