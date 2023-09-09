// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TabulationRecognizer.cs -- распознает символы табуляции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает символы табуляции.
/// </summary>
[PublicAPI]
public sealed class TabulationRecognizer
    : ITokenRecognizer
{
    #region Constants

    /// <summary>
    /// Символ табуляции.
    /// </summary>
    public const char Tabulation = '\t';

    #endregion

    #region Public methods

    /// <summary>
    /// Создание экземпляра.
    /// </summary>
    public static ITokenRecognizer Create() => new TabulationRecognizer();

    #endregion

    #region ITokenRecognizer

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

        if (navigator.PeekChar() != Tabulation)
        {
            return null;
        }

        do
        {
            navigator.ReadChar();
        }
        while (navigator.PeekChar() == Tabulation);

        var length = navigator.Position - offset;
        var value = new string (Tabulation, length);
        var result = new Token (TokenKind.Char, value, line, column, offset)
        {
            UserData = value
        };

        return result;
    }

    #endregion
}
