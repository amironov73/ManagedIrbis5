// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InfixOperatorKind.cs -- вид инфиксного оператора
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace AM.Lexey.Expressions;

/// <summary>
/// Ассоциативность инфиксного (бинарного) оператора:
/// левоассоциативный, правоассоциативный/ или неассоциативный.
/// </summary>
[PublicAPI]
public enum InfixOperatorKind
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
