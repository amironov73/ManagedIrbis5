// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* UnaryOperatorType.cs -- тип унарного оператора: префиксный либо постфиксный
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Kotik;

/// <summary>
/// Тип унарного оператора: префиксный либо постфиксный.
/// </summary>
public enum UnaryOperatorType
{
    /// <summary>
    /// Префиксный оператор.
    /// </summary>
    Prefix,

    /// <summary>
    /// Постфиксный оператор.
    /// </summary>
    Postfix
}
