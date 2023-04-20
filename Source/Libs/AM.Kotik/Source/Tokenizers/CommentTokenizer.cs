// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* CommentTokenizer.cs -- токенайзер для комментариев
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для комментариев.
/// </summary>
[PublicAPI]
public sealed class CommentTokenizer
    : Tokenizer
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public CommentTokenizer()
        : this (true)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="eatComments">Съедать комментарии,
    /// т. е. не выдавать их как токены.</param>
    public CommentTokenizer
        (
            bool eatComments
        )
    {
        _eatComments = eatComments;
    }

    #endregion

    #region Private members

    private bool _eatComments;

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
            if (tokenizer is CommentTokenizer commentTokenizer)
            {
                commentTokenizer._eatComments = eat;
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

        if (PeekChar() == '/')
        {
            var nextChar = navigator.LookAhead();

            // комментарий до конца строки
            if (nextChar == '/')
            {
                // съедаем всю текущую строку до конца
                while (!IsEof)
                {
                    if (PeekChar() is '\r' or '\n')
                    {
                        break;
                    }

                    ReadChar();
                }

                if (_eatComments)
                {
                    return TokenizerResult.Skip;
                }

                var memory = navigator.Substring (offset, navigator.Position - offset);
                var value = memory.ToString();
                var token = new Token
                    (
                        TokenKind.Comment,
                        value,
                        line,
                        column,
                        offset
                    )
                    {
                        UserData = value
                    };

                return TokenizerResult.Success (token);
            }

            // многострочный комментарий
            if (nextChar == '*')
            {
                // проматываем всё до конца
                var position = navigator.Position;
                navigator.ReadTo ("*/");
                if (navigator.Position == position)
                {
                    throw new SyntaxException (navigator);
                }

                if (_eatComments)
                {
                    return TokenizerResult.Skip;
                }

                var memory = navigator.Substring (offset, navigator.Position - offset);
                var value = memory.ToString();
                var token = new Token
                    (
                        TokenKind.Comment,
                        value,
                        line,
                        column,
                        offset
                    )
                    {
                        UserData = value
                    };

                return TokenizerResult.Success (token);
            }
        }

        return TokenizerResult.Error;
    }

    #endregion
}
