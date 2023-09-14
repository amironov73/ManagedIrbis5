// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* WhitespaceRecognizer.cs -- распознает пробелы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает пробелы.
/// </summary>
[PublicAPI]
public sealed class WhitespaceRecognizer
    : ITokenRecognizer
{
    #region Properties

    /// <summary>
    /// Общий (разделяемый) экземпляр токена.
    /// </summary>
    public static readonly Token TokenInstance = new
        (
            TokenKind.Whitespace,
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
    public static ITokenRecognizer Create() => new WhitespaceRecognizer();

    #endregion

    #region ITokenizer members

    /// <inheritdoc cref="ITokenRecognizer"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;

        navigator.SkipWhitespace();

        if (navigator.Position != offset)
        {
            if (UseSharedTokenInstance)
            {
                return TokenInstance;
            }

            var text = navigator.Substring (offset, navigator.Position - offset).ToString();
            var result = new Token
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

            return result;
        }

        return default;
    }

    #endregion
}
