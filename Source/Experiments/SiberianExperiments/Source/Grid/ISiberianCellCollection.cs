// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ISiberianCellCollection.cs -- интерфейс коллекции ячеек в строке грида
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
    /// Интерфейс коллекции ячеек в строке грида.
    /// </summary>
    public interface ISiberianCellCollection
    {
        #region Public methods

        /// <summary>
        /// Доступ к ячейкам по индексу.
        /// </summary>
        ISiberianCell this [int index] { get; }

        #endregion

    } // interface ISiberianCellCollection

} // namespace ManagedIrbis.WinForms.Grid
