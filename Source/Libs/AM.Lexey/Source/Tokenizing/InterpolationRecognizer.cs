// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InterpolationRecognizer.cs -- распознает интерполяцию строк
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives


using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает интерполяцию строк.
/// </summary>
[PublicAPI]
public sealed class InterpolationRecognizer
    : ITokenRecognizer
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public InterpolationRecognizer()
        : this ('$', '"', '\\')
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="marker">Маркер интерполяции.</param>
    /// <param name="limiter">Ограничитель строки.</param>
    /// <param name="escape">Символ экранирования.</param>
    public InterpolationRecognizer
        (
            char marker,
            char limiter,
            char escape
        )
    {
        _marker = marker;
        _limiter = limiter;
        _escape = escape;
    }

    #endregion

    #region Private members

    private readonly char _marker;
    private readonly char _limiter;
    private readonly char _escape;

    #endregion

    #region ITokenRecognizer members

    /// <inheritdoc cref="ITokenRecognizer.RecognizeToken"/>
    public Token? RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var offset = navigator.Position;
        var line = navigator.Line;
        var column = navigator.Column;

        if (navigator.PeekChar() != _marker
            || navigator.PeekChar (1) != _limiter)
        {
            return default;
        }

        navigator.ReadChar(); // съедаем доллар
        navigator.ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        var builder = StringBuilderPool.Shared.Get();
        while (!navigator.IsEOF)
        {
            chr = navigator.ReadChar();
            if (chr == _escape)
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

        var value = builder.ReturnShared();
        value = TextUtility.UnescapeText (value);
        var result = new Token
            (
                TokenKind.Interpolation,
                value,
                line,
                column,
                offset
            )
            {
                UserData = value
            };

        return result;
    }

    #endregion
}
