// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ITokenRefiner.cs -- обрабатывает список токенов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Tokenizing;

/// <summary>
/// Обрабатывает список токенов.
/// </summary>
[PublicAPI]
public interface ITokenRefiner
{
    /// <summary>
    /// Обработка списка токенов, например,
    /// выбрасывание из списка пробельных токенов.
    /// </summary>
    IList<Token> RefineTokens (IList<Token> tokens) => tokens;
}
