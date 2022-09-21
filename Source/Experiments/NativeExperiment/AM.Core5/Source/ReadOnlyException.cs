// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

/* ReadOnlyException.cs -- specific for IReadOnly interface
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM;

/// <summary>
/// Specific for IReadOnly interface.
/// </summary>
public sealed class ReadOnlyException
    : ArsMagnaException
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ReadOnlyException()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReadOnlyException
        (
            string message
        )
        : base (message)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReadOnlyException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException)
    {
        // пустое тело конструктора
    }

    #endregion
}
