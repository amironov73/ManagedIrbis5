// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DataType.cs -- тип данных
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Data;

/// <summary>
/// Тип данных.
/// </summary>
public enum DataType
{
    /// <summary>
    /// Нет.
    /// </summary>
    None,

    /// <summary>
    /// Логический.
    /// </summary>
    Boolean,

    /// <summary>
    /// Целое число.
    /// </summary>
    Integer,

    /// <summary>
    /// Денежный тип.
    /// </summary>
    Money,

    /// <summary>
    /// Текстовый тип.
    /// </summary>
    Text,
}
