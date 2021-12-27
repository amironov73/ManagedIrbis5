// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReturnException.cs -- исключение, используемое для возврата значений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Исключение, используемое для возврата значений.
/// </summary>
sealed class ReturnException
    : Exception
{
    #region Properties

    /// <summary>
    /// Возвращаемое значение.
    /// </summary>
    public dynamic? Value { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReturnException
        (
            dynamic? value
        )
    {
        Value = value;
    }

    #endregion
}
