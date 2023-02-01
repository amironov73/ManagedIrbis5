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

using System;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Tokenizers;

/// <summary>
/// Токенайзер для комментариев.
/// </summary>
public sealed class CommentTokenizer
    : SubTokenizer
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

    private readonly bool _eatComments;

    #endregion

    #region SubTokenizer members

    /// <inheritdoc cref="SubTokenizer.Parse"/>
    public override Token? Parse()
    {
        var line = _navigator.Line;
        var column = _navigator.Column;
        var offset = _navigator.Position;
        
        if (PeekChar() == '/')
        {
            var nextChar = _navigator.LookAhead();

            // комментарий до конца строки
            if (nextChar == '/')
            {
                // съедаем всю текущую строку до конца
                _navigator.ReadLine();
                
                return _eatComments
                    ? null
                    : new Token
                        (
                            TokenKind.Comment,
                            _navigator.Substring (offset, _navigator.Position - offset).ToString(),
                            line,
                            column,
                            offset
                        );
            }

            // многострочный комментарий
            if (nextChar == '*')
            {
                // проматываем всё до конца
                var position = _navigator.Position;
                _navigator.ReadTo ("*/");
                if (_navigator.Position == position)
                {
                    throw new SyntaxException (_navigator);
                }

                return _eatComments
                    ? null
                    : new Token 
                        (
                            TokenKind.Comment,
                            _navigator.Substring (offset, _navigator.Position - offset).ToString(),
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
