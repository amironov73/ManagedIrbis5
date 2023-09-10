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
    #region Public methods

    /// <summary>
    /// Создание экземпляра.
    /// </summary>
    public static ITokenRecognizer Create() => new RemainderRecognizer();

    #endregion

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
        var text = navigator.GetRemainingText().ToString();
        var result = new Token ("remainder", text, line, column, offset)
        {
            UserData = text
        };

        return result;
    }

    #endregion
}
