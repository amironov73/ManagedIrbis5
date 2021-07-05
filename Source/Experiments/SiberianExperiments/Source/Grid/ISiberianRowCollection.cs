// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedType.Global

/* ISiberianRowCollections.cs -- интерфейс коллекции строк грида
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
    /// Интерфейс коллекции строк грида.
    /// </summary>
    public interface ISiberianRowCollection
    {
        #region Public methods

        /// <summary>
        /// Добавление строки в коллекцию.
        /// </summary>
        void Add (ISiberianRow row);

        /// <summary>
        /// Количество строк в коллекции.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Грид, которому принадлежат строки.
        /// </summary>
        ISiberianGrid Grid { get; }

        /// <summary>
        /// Доступ по индексу.
        /// </summary>
        ISiberianRow this [int index] { get; }

        #endregion

    } // interface ISiberianRowCollection

} // namespace ManagedIrbis.WinForms.Grid
