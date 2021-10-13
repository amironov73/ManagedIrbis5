// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* TokenKind.cs -- вид синтаксического токена
 * Ars Magna project, http://arsmagna.ru
 */

namespace SimplestLanguage
{
    /// <summary>
    /// Вид синтаксического токена.
    /// </summary>
    public enum TokenKind
    {
        /// <summary>
        /// Нет токена.
        /// </summary>
        None,

        /// <summary>
        /// Литерал-число.
        /// </summary>
        NumericLiteral,

        /// <summary>
        /// Знак "плюс".
        /// </summary>
        Plus,

        /// <summary>
        /// Знак "minus".
        /// </summary>
        Minus,

        /// <summary>
        /// Знак равенства.
        /// </summary>
        Equals,

        /// <summary>
        /// Идентификатор.
        /// </summary>
        Identifier,

        /// <summary>
        /// Открывающая круглая скобка.
        /// </summary>
        LeftParenthesis,

        /// <summary>
        /// Закрывающая правая скобка.
        /// </summary>
        RightParenthesis

    } // enum TokenKind

} // namespace SimplestLanguage
