// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ITokenizer.cs -- интерфейс токенайзера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Интерфейс токенайзера.
/// </summary>
[PublicAPI]
public interface ITokenizer
{
    /// <summary>
    /// Распознание следужющего токена.
    /// </summary>
    /// <returns><c>null</c>, если токен не распознан.</returns>
    Token? RecognizeToken (TextNavigator navigator);
}
