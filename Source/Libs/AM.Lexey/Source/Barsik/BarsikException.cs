// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BarsikException.cs -- специфичное для Barsik исключение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Lexey.Barsik;

/// <summary>
/// Специфичное для Barsik исключение.
/// </summary>
[PublicAPI]
public class BarsikException
    : ApplicationException
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public BarsikException()
    {
        // тело конструктора оставлено пустым
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BarsikException
        (
            string? message
        )
        : base
        (
            message
        )
    {
        // тело конструктора оставлено пустым
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BarsikException
        (
            string? message,
            Exception? innerException
        )
        : base
        (
            message,
            innerException
        )
    {
        // тело конструктора оставлено пустым
    }

    #endregion
}
