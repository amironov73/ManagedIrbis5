// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* RawStringTokenizer.cs -- токенайзер для сырых строк
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для сырых строк.
/// </summary>
public sealed class RawStringTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;

        if (PeekChar() != '"' || PeekChar (1) != '"' || PeekChar (2) != '"')
        {
            return null;
        }

        ReadChar(); // съедаем открывающие кавычки
        ReadChar();
        ReadChar();

        var success = false;
        var builder = new StringBuilder();
        while (!IsEof)
        {
            var chr = ReadChar();
            if (chr == '"' && PeekChar() == '"' && PeekChar (1) == '"')
            {
                ReadChar();
                ReadChar();
                success = true;
                break;
            }

            builder.Append (chr);
        }

        if (!success)
        {
            throw new SyntaxException (_navigator);
        }

        return new Token (TokenKind.String, builder.ToString(), line, column);
    }

    #endregion
}
