// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverInvoked.Global
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMemberInSuper.Global

/* ISiberianColumn.cs -- интерфейс колонки грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Интерфейс колонки грида.
    /// </summary>
    public interface ISiberianColumn
    {
        #region Events

        /// <summary>
        /// Событие, возникающее при щелчке по колонке.
        /// </summary>
        event EventHandler<SiberianClickEventArgs>? Click;

        #endregion

        #region Properties

        /// <summary>
        /// Грид, которому принадлежит колонка.
        /// </summary>
        ISiberianGrid Grid { get; }

        /// <summary>
        /// Порядковый номер колонки (нумерация с 0).
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Имя свойства, содержащего данные для отображения/редактирования.
        /// </summary>
        string? Member { get; }

        /// <summary>
        /// Колонка только для чтения?
        /// </summary>
        bool ReadOnly { get; }

        /// <summary>
        /// Заголовок колонки.
        /// </summary>
        string? Title { get; }

        /// <summary>
        /// Ширина колонки в пикселах.
        /// </summary>
        int Width { get; }

        #endregion

        #region Public methods

        /// <summary>
        /// Создание ячейки соответствующего типа.
        /// </summary>
        public ISiberianCell CreateCell(ISiberianRow row);

        /// <summary>
        /// Извлечение соответствующего свойства из данных.
        /// </summary>
        public object? GetMemberData(object? obj);

        /// <summary>
        /// Установка соответствующего свойства в данных.
        /// </summary>
        public void PutMemberData(object? obj, object? value);

        #endregion

    } // interface ISiberianColumn

} // namespace ManagedIrbis.WinForms.Grid
