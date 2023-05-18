// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* WhitespaceTokenizer.cs -- токенайзер для пробелов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Токенайзер для пробелов.
/// </summary>
[PublicAPI]
public sealed class WhitespaceTokenizer
    : ITokenizer
{
    #region ITokenizer members

    /// <inheritdoc cref="ITokenizer"/>
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
            var text = navigator.Substring (offset, navigator.Position - offset).ToString();
            var token = new Token
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

            return token;
        }

        return null;
    }

    #endregion
}
