// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* VirtualAdapter.cs -- адаптер, подкачивающий данные в грид
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Адаптер, подкачивающий данные в грид.
    /// </summary>
    public class VirtualAdapter
    {
        #region Properties

        /// <summary>
        /// Предпочитаемый размер страницы.
        /// </summary>
        public virtual int PageSize => 500;

        #endregion

        #region Public methods

        /// <summary>
        /// Получение данных для колонки из объекта.
        /// </summary>
        public virtual object? ByIndex
            (
                object? value,
                int index
            )
        {
            if (value is Array array)
            {
                return index < array.Length ? array.GetValue(index) : default;
            }

            return default;

        } // method ByIndex

        /// <summary>
        /// Очистка.
        /// </summary>
        public virtual void Clear()
        {
        } // method Clear

        /// <summary>
        /// Подгрузка данных в кеш виртуального грида.
        /// </summary>
        public virtual VirtualData? PullData(int firstLine, int lineCount) => default;

        #endregion

    } // class VirtualAdapter

} // namespace AM.Windows.Forms
