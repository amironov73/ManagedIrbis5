// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* WhitespaceTokenizer.cs -- токенайзер для пробелов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Tokenizing;

/// <summary>
/// Токенайзер для пробелов.
/// </summary>
[PublicAPI]
public sealed class WhitespaceTokenizer
    : Tokenizer
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public WhitespaceTokenizer()
        : this (true)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="eatWhitespace">Съедать пробелы,
    /// т .е. не выдавать их как токены.</param>
    public WhitespaceTokenizer
        (
            bool eatWhitespace
        )
    {
        _eatWhitespace = eatWhitespace;
    }

    #endregion

    #region Private members

    private bool _eatWhitespace;

    #endregion

    #region Public methods

    /// <summary>
    /// Установка переключателя `eat`.
    /// </summary>
    public static void SwitchEat
        (
            IEnumerable<Tokenizer> tokenizers,
            bool eat
        )
    {
        Sure.NotNull (tokenizers);

        foreach (var tokenizer in tokenizers)
        {
            if (tokenizer is WhitespaceTokenizer whitespaceTokenizer)
            {
                whitespaceTokenizer._eatWhitespace = eat;
            }
        }
    }

    #endregion

    #region Tokenizer members

    /// <inheritdoc cref="Tokenizer.Parse"/>
    public override TokenizerResult Parse()
    {
        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;

        navigator.SkipWhitespace();

        if (navigator.Position != offset)
        {
            if (_eatWhitespace)
            {
                return TokenizerResult.Skip;
            }

            var text = navigator.Substring (offset, navigator.Position - offset).ToString();
            var token = new Token
                (
                    TokenKind.Whitespace,
                    text,
                    line,
                    column,
                    offset
                )
                {
                    UserData = text
                };

            return TokenizerResult.Success (token);
        }

        return TokenizerResult.Error;
    }

    #endregion
}
