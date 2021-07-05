// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

// Событие никогда не используется
#pragma warning disable CS0067

/* SiberianRow.cs -- дефолтная реализация строки грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Дефолтная реализация строки грида.
    /// </summary>
    public class SiberianRow
        : ISiberianRow
    {
        #region ISiberianRow members

        /// <inheritdoc cref="ISiberianRow.Click"/>
        public event EventHandler<SiberianClickEventArgs>? Click;

        /// <inheritdoc cref="ISiberianRow.Cells"/>
        public ISiberianCellCollection Cells { get; }

        /// <inheritdoc cref="ISiberianRow.Data"/>
        public object? Data { get; set; }

        /// <inheritdoc cref="Grid"/>
        public ISiberianGrid Grid { get; }

        /// <inheritdoc cref="ISiberianRow.Height"/>
        public int Height { get; set; }

        /// <inheritdoc cref="ISiberianRow.Index"/>
        public int Index { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SiberianRow
            (
                ISiberianGrid grid,
                ISiberianCellCollection cells
            )
        {
            Grid = grid;
            Cells = cells;

        } // constructor

        #endregion

    } // class SiberianRow

} // namespace ManagedIrbis.WinForms.Grid
