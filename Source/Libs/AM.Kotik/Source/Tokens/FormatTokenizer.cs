// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* FormatTokenizer.cs -- токенайзер для форматных строк
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Токенайзер для форматных строк.
/// </summary>
public sealed class FormatTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;

        if (PeekChar() != '$' || PeekChar (1) != '"')
        {
            return null;
        }

        ReadChar(); // съедаем доллар
        ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = new StringBuilder();
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
            throw new SyntaxException (_navigator);
        }

        var text = builder.ToString();
        text = UnescapeText (text);

        return new Token (TokenKind.Format, text, line, column);
    }

    #endregion
}
