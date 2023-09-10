// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* CommentRecognizer.cs -- распознает комментарии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает комментарии.
/// </summary>
[PublicAPI]
public sealed class CommentRecognizer
    : ITokenRecognizer
{
    #region Properties

    /// <summary>
    /// Общий (разделяемый) экземпляр токена.
    /// </summary>
    public static readonly Token TokenInstance = new
        (
            TokenKind.Comment,
            string.Empty
        );

    /// <summary>
    /// Использовать общий экземпляр токена?
    /// </summary>
    public static bool UseSharedTokenInstance { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Создание экземпляра.
    /// </summary>
    public static ITokenRecognizer Create() => new CommentRecognizer();

    #endregion

    #region ITokenRecognizer members

    /// <inheritdoc cref="ITokenRecognizer.RecognizeToken"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;

        if (navigator.PeekChar() == '/')
        {
            var nextChar = navigator.LookAhead();

            // комментарий до конца строки
            if (nextChar == '/')
            {
                // съедаем всю текущую строку до конца
                while (!navigator.IsEOF)
                {
                    if (navigator.PeekChar() is '\r' or '\n')
                    {
                        break;
                    }

                    navigator.ReadChar();
                }

                if (UseSharedTokenInstance)
                {
                    return TokenInstance;
                }

                var length = navigator.Position - offset;
                var memory = navigator.Substring (offset, length);
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

                return token;
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

                if (UseSharedTokenInstance)
                {
                    return TokenInstance;
                }

                var length = navigator.Position - offset;
                var memory = navigator.Substring (offset, length);
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

                return token;
            }
        }

        return null;
    }

    #endregion
}
