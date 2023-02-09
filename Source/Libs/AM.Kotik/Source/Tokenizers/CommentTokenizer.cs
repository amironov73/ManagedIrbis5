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

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для комментариев.
/// </summary>
public sealed class CommentTokenizer
    : Tokenizer
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="eatComments">Съедать комментарии,
    /// т. е. не выдавать их как токены.</param>
    public CommentTokenizer
        (
            bool eatComments = true
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
    public override Token? Parse()
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

                var memory = navigator.Substring (offset, navigator.Position - offset);

                return _eatComments
                    ? null
                    : new Token
                        (
                            TokenKind.Comment,
                            memory.ToString(),
                            line,
                            column,
                            offset
                        );
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

                var memory = navigator.Substring (offset, navigator.Position - offset);

                return _eatComments
                    ? null
                    : new Token 
                        (
                            TokenKind.Comment,
                            memory.ToString(),
                            line,
                            column,
                            offset
                        );
            }
        }

        return null;
    }

    #endregion
}
