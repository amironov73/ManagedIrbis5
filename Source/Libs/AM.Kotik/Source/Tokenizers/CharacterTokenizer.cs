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

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для отдельных символов.
/// </summary>
public sealed class CharacterTokenizer
    : Tokenizer
{
    #region Tokenizer methods

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        var offset = navigator.Position;
        var line = navigator.Line;
        var column = navigator.Column;
        if (PeekChar() != '\'')
        {
            return null;
        }

        ReadChar(); // съедаем открывающий апостроф
        var result = ReadChar();
        if (result == '\\')
        {
            if (IsEof)
            {
                throw new SyntaxException (navigator);
            }

            var builder = StringBuilderPool.Shared.Get();
            builder.Append (result);
            while (!IsEof)
            {
                result = PeekChar();
                if (result == '\'')
                {
                    break;
                }

                ReadChar();
                builder.Append (result);
            }

            var text = TextUtility.UnescapeText (builder.ReturnShared()).ThrowIfNullOrEmpty();
            if (text.Length != 1)
            {
                throw new SyntaxException (navigator);
            }
            result = text[0];
        }

        if (ReadChar() != '\'')
        {
            throw new SyntaxException (navigator);
        }

        return new Token (TokenKind.Char, result.ToString(), line, column, offset);
    }

    #endregion
}
