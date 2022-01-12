// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ExceptionEventArgsT.cs -- информация об исключении для события
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

namespace AM;

/// <summary>
/// Информация для события о произошедшем исключении.
/// </summary>
[DebuggerDisplay("{Exception} {Handled}")]
public sealed class ExceptionEventArgs<T>
    : EventArgs
    where T: Exception
{
    #region Properties

    /// <summary>
    /// Собственно исключение.
    /// </summary>
    public T Exception { get; }

    /// <summary>
    /// Обработано?
    /// </summary>
    public bool Handled { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public ExceptionEventArgs (T exception) => Exception = exception;

    #endregion
}
