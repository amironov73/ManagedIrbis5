// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ExternalCodeRecognizer.cs -- распознает внешний код
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает внешний код, т.е. код, расположенный
/// между двух обратных апострофов (grave accent) '`'.
/// </summary>
[PublicAPI]
public sealed class ExternalCodeRecognizer
    : ITokenRecognizer
{
    #region Constants

    /// <summary>
    /// Символ-ограничитель.
    /// Типографский клавиатурный знак обратного апострофа.
    /// </summary>
    public const char Limiter = '`';

    #endregion

    #region Public methods

    /// <summary>
    /// Создание экземпляра.
    /// </summary>
    public static ITokenRecognizer Create() => new ExternalCodeRecognizer();

    #endregion


    #region Tokenizer members

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

        if (navigator.PeekChar() != Limiter)
        {
            return null;
        }

        navigator.ReadChar(); // съедаем открывающую кавычку
        char chr = default;
        while (!navigator.IsEOF)
        {
            chr = navigator.ReadChar();
            if (chr == Limiter)
            {
                break;
            }
        }

        if (chr != Limiter)
        {
            return null;
        }

        var memory = navigator.Substring
            (
                offset + 1,
                navigator.Position - offset - 2
            );
        var value = StringPool.Shared.GetOrAdd (memory.Span);
        var token = new Token
            (
                TokenKind.External,
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

    #endregion
}
