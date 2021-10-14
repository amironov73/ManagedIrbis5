// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* TokenPair.cs -- пара токенов
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace SimplestLanguage
{
    /// <summary>
    /// Пара токенов.
    /// </summary>
    struct TokenPair
    {
        #region Properties

        /// <summary>
        /// Открывающий токен.
        /// </summary>
        public TokenKind Open { get; set; }

        /// <summary>
        /// Закрывающий токен.
        /// </summary>
        public TokenKind Close { get; set; }

        #endregion
    }
}
