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
    : SubTokenizer
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

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        var offset = _navigator.Position;

        _navigator.SkipWhitespace();
        if (_eatWhitespace)
        {
            return null;
        }

        if (_navigator.Position != offset)
        {
            return new Token
                (
                    TokenKind.Whitespace,
                    _navigator.Substring (offset, _navigator.Position - offset).ToString(),
                    line,
                    column,
                    offset
                );
        }

        return null;
    }

    #endregion
}
