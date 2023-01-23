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

namespace AM.Kotik;

/// <summary>
/// Токенайзер для отдельных символов.
/// </summary>
public sealed class CharacterTokenizer
    : SubTokenizer
{
    #region SubTokenizer methods

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        if (PeekChar() != '\'')
        {
            return null;
        }

        ReadChar(); // съедаем открывающий апостроф
        var result = ReadChar();
        var chr = ReadChar();
        if (chr == '\\')
        {
            // TODO реализовать
        }

        if (chr != '\'')
        {
            throw new SyntaxException (_navigator);
        }

        return new Token (TokenKind.Char, result.ToString(), line, column);
    }

    #endregion
}
