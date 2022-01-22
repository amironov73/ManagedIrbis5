// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* GenericItemResultEventArgs.cs -- аргумент события, позволяющий вычислить результат произвольного типа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Аргумент события, позволяющий вычислить результат произвольного типа.
/// </summary>
/// <typeparam name="T">Тип вычисляемого результата.</typeparam>
public class GenericItemResultEventArgs<T> : EventArgs
{
    #region Properties

    /// <summary>
    /// Индекс элемента.
    /// </summary>
    public int ItemIndex { get; internal set; }

    /// <summary>
    /// Результат вычисления.
    /// </summary>
    public T Result;

    #endregion
}
