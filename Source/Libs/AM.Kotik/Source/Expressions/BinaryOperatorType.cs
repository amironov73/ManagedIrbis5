// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* BinaryOperatorType.cs -- тип бинарного оператора
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Kotik;

/// <summary>
/// Ассоциативность бинарного оператора: левоассоциативный, правоассоциативный
/// или неассоциативный.
/// </summary>
public enum BinaryOperatorType
{
    /// <summary>
    /// Неассоциативный оператор.
    /// </summary>
    NonAssociative,

    /// <summary>
    /// Левоассоциативный оператор.
    /// </summary>
    LeftAssociative,

    /// <summary>
    /// Правоассоциативный оператор.
    /// </summary>
    RightAssociative
}
