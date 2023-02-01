// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* WhitespaceTokenizer.cs -- токенайзер для пробелов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для пробелов.
/// </summary>
public sealed class WhitespaceTokenizer
    : Tokenizer
{
    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="eatWhitespace">Съедать пробелы,
    /// т .е. не выдавать их как токены.</param>
    public WhitespaceTokenizer
        (
            bool eatWhitespace = true
        )
    {
        _eatWhitespace = eatWhitespace;
    }

    #endregion
    
    #region Private members

    private readonly bool _eatWhitespace;

    #endregion

    #region SubTokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;

        navigator.SkipWhitespace();
        if (_eatWhitespace)
        {
            return null;
        }

        if (navigator.Position != offset)
        {
            return new Token
                (
                    TokenKind.Whitespace,
                    navigator.Substring (offset, navigator.Position - offset).ToString(),
                    line,
                    column,
                    offset
                );
        }

        return null;
    }

    #endregion
}
