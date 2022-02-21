// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* TokenKind.cs -- типы BibTex-токенов
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.BibTex;

/// <summary>
/// Типы BibTex-токенов.
/// </summary>
public enum TokenKind
{
    /// <summary>
    /// Нет токена (достигнут конец потока).
    /// </summary>
    None = -1,

    /// <summary>
    /// Комментарий.
    /// </summary>
    Comment = 0,

    /// <summary>
    /// Открывающая фигурная скобка.
    /// </summary>
    OpenParenthesis = 1,

    /// <summary>
    /// Закрывающая фигурная скобка.
    /// </summary>
    CloseParenthesis = 2,

    /// <summary>
    /// Литерал, например, метка поля или значение поля.
    /// </summary>
    Literal = 3,

    /// <summary>
    /// Знак равенства
    /// </summary>
    Equals = 4,

    /// <summary>
    /// Запятая.
    /// </summary>
    Comma = 5,
}
