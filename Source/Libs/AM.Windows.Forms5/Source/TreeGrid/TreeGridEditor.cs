// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridEditor.cs -- редактор для иерархического грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Редактор для ирерархического грида.
    /// </summary>
    public abstract class TreeGridEditor
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Получаем ассоциированный контрол.
        /// </summary>
        public abstract Control? Control { get; }

        #endregion

        #region Public methods

        /// <summary>
        /// Установка значения.
        /// </summary>
        public abstract void SetValue(string? value);

        /// <summary>
        /// Получение значения.
        /// </summary>
        public abstract string? GetValue();

        /// <summary>
        /// Подсветка диапазона текста.
        /// </summary>
        public abstract void SelectText(int start, int length);

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose()
        {
            Control?.Dispose();
        }

        #endregion

    } // class TreeGridEditor

} // namespace AM.Windows.Forms
