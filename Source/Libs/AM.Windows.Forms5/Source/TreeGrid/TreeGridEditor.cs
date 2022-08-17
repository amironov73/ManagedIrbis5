// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TreeGridEditor.cs -- редактор для ячейки иерархического грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Редактор для ячейки ирерархического грида.
/// </summary>
public abstract class TreeGridEditor
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Получение контрола, ассоциированного с ячейкой (колонкой).
    /// </summary>
    public abstract Control? Control { get; }

    #endregion

    #region Public methods

    /// <summary>
    /// Установка значения, хранящегося в ячейке.
    /// </summary>
    public abstract void SetValue (string? value);

    /// <summary>
    /// Получение значения, хранящегося в ячейке.
    /// </summary>
    public abstract string? GetValue();

    /// <summary>
    /// Подсветка диапазона текста (если возможно).
    /// </summary>
    public abstract void SelectText (int start, int length);

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public virtual void Dispose()
    {
        Control?.Dispose();
    }

    #endregion
}
