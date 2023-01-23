// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ExternalTokenizer.cs -- токенайзер для внешнего кода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Токенайзер для внешнего кода.
/// </summary>
public sealed class ExternalTokenizer
    : SubTokenizer
{
    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;

        if (PeekChar() != '`')
        {
            return null;
        }

        ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = new StringBuilder();
        while (!IsEof)
        {
            chr = ReadChar();
            if (chr == '`')
            {
                break;
            }

            builder.Append (chr);
        }

        if (chr != '`')
        {
            throw new SyntaxException (_navigator);
        }

        var text = builder.ToString();
        return new Token (TokenKind.External, text, line, column);
    }

    #endregion
}
