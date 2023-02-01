// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* StringTokenizer.cs -- токенайзер для обычных строк
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для обычных строк.
/// </summary>
public sealed class StringTokenizer
    : Tokenizer
{
    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;
        if (PeekChar() != '"')
        {
            return null;
        }

        ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = StringBuilderPool.Shared.Get();
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '\\')
            {
                builder.Append (chr);
                builder.Append (ReadChar());
                continue;
            }

            if (chr == '"')
            {
                break;
            }

            builder.Append (chr);
        }

        if (chr != '"')
        {
            StringBuilderPool.Shared.Return (builder);
            throw new SyntaxException (navigator);
        }

        var text = builder.ReturnShared();
        text = TextUtility.UnescapeText (text);

        return new Token (TokenKind.String, text, line, column, offset);
    }

    #endregion
}
