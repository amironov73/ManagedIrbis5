// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ISiberianColumnCollection.cs -- интерфейс коллекции колонок грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Интерфейс коллекции колонок грида.
    /// </summary>
    public interface ISiberianColumnCollection
    {
        #region Public methods

        /// <summary>
        /// Добавление колонки в коллекцию.
        /// </summary>
        void Add (ISiberianColumn column);

        /// <summary>
        /// Количество колонок в коллекции.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Доступ по индексу.
        /// </summary>
        ISiberianColumn this [int index] { get; }

        #endregion

    } // interface ISiberianColumnCollection

} // namespace ManagedIrbis.WinForms.Grid
