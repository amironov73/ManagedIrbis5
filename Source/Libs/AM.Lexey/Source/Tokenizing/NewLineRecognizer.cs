// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* NewLineRecognizer.cs -- распознает перевод строки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using CommunityToolkit.HighPerformance.Buffers;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Распознает перевод строки.
/// </summary>
[PublicAPI]
public sealed class NewLineRecognizer
    : ITokenRecognizer
{
    #region Properties

    /// <summary>
    /// Общий (разделяемый) экземпляр токена.
    /// </summary>
    public static readonly Token TokenInstance = new
        (
            TokenKind.NewLine,
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
    public static ITokenRecognizer Create() => new NewLineRecognizer();

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

        var chr = navigator.PeekChar();
        if (chr is not '\r' and not '\n')
        {
            return null;
        }

        navigator.ReadChar(); // съедаем символ
        if (chr is '\r' // обрабатываем сочетание '\r\n'
            && navigator.PeekChar() is '\n')
        {
            navigator.ReadChar();
        }

        if (UseSharedTokenInstance)
        {
            return TokenInstance;
        }

        var memory = navigator.Substring
            (
                offset,
                navigator.Position - offset - 1
            );
        var value = StringPool.Shared.GetOrAdd (memory.Span);
        var token = new Token
            (
                TokenKind.NewLine,
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
