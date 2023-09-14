// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StringRecognizer.cs -- распознает обычные строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает обычные строки, т. е. некоторое количество символов,
/// заключенных в двойные кавычки.
/// Кроме того, распознается экранирование с помощью символа <c>'\'</c>.
/// </summary>
[PublicAPI]
public sealed class StringRecognizer
    : ITokenRecognizer
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public StringRecognizer()
        : this ('"')
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StringRecognizer
        (
            char limiter
        )
    {
        _limiter = limiter;
    }

    #endregion

    #region Private members

    private readonly char _limiter;

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
        if (navigator.PeekChar() != _limiter)
        {
            return default;
        }

        navigator.ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = StringBuilderPool.Shared.Get();
        while (!navigator.IsEOF)
        {
            chr = navigator.ReadChar();
            if (chr == '\\')
            {
                builder.Append (chr);
                builder.Append (navigator.ReadChar());
                continue;
            }

            if (chr == _limiter)
            {
                break;
            }

            builder.Append (chr);
        }

        if (chr != _limiter)
        {
            StringBuilderPool.Shared.Return (builder);
            throw new SyntaxException (navigator);
        }

        var text = builder.ReturnShared();
        text = TextUtility.UnescapeText (text);
        var result = new Token (TokenKind.String, text, line, column, offset)
        {
            UserData = text
        };

        return result;
    }

    #endregion
}
