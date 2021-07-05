// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianCellCollection.cs -- дефолтная реализация коллекции ячеек
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Дефолтная реализация коллекции ячеек.
    /// </summary>
    public class SiberianCellCollection
        : ISiberianCellCollection
    {
        #region Properties

        /// <summary>
        /// Грид, которому принадлежит коллекция ячеек.
        /// </summary>
        public ISiberianGrid Grid { get; }

        /// <summary>
        /// Строка, которой принадлежит коллекция ячеек.
        /// </summary>
        public ISiberianRow Row { get; internal set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SiberianCellCollection
            (
                ISiberianGrid grid,
                ISiberianRow row
            )
        {
            Grid = grid;
            Row = row;
        }

        #endregion

        #region ISiberianCellCollection members

        /// <inheritdoc cref="ISiberianCellCollection.this"/>
        public ISiberianCell this[int index] => Grid.Columns[index].CreateCell(Row);

        #endregion

    } // class SiberianCellCollection

} // namespace ManagedIrbis.WinForms.Grid
