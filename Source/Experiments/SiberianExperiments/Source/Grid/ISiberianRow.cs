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

// Событие никогда не используется
#pragma warning disable CS0067

/* ISiberianRow.cs -- интерфейс строки грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Интерфейс строки грида.
    /// </summary>
    public interface ISiberianRow
    {
        #region Events

        /// <summary>
        /// Событие, возникающее при щелчке по строке.
        /// </summary>
        event EventHandler<SiberianClickEventArgs>? Click;

        #endregion

        #region Properties

        /// <summary>
        /// Ячейки, образующие строку грида.
        /// </summary>
        ISiberianCellCollection Cells { get; }

        /// <summary>
        /// Данные, общие для строки (опционально).
        /// </summary>
        object? Data { get; set; }

        /// <summary>
        /// Грид, которому принадлежит строка.
        /// </summary>
        ISiberianGrid Grid { get; }

        /// <summary>
        /// Высота строки в пикселах.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Порядковый номер строки (нумерация с 0).
        /// </summary>
        int Index { get; }

        #endregion

    } // interface ISiberianRow

} // namespace ManagedIrbis.WinForms.Grid
