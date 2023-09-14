// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RemainderRecognizer.cs -- выдает остаток текста
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Выдает остаток текста (возможно, пустую строку,
/// если уже достигнут конец текста).
/// </summary>
[PublicAPI]
public sealed class RemainderRecognizer
    : ITokenRecognizer
{
    #region ITokenRecognizer members

    /// <inheritdoc cref="ITokenRecognizer.RecognizeToken"/>
    public Token RecognizeToken
        (
            TextNavigator navigator
        )
    {
        Sure.NotNull (navigator);

        var line = navigator.Line;
        var column = navigator.Column;
        var offset = navigator.Position;
        var value = navigator.GetRemainingText().ToString();
        var result = new Token
            (
                TokenKind.Remainder,
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
