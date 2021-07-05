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

// Событие никогда не используется
#pragma warning disable CS0067

/* SiberianGrid.cs -- дефолтная реализация грида
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
    /// Дефолтная реализация грида.
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class SiberianGrid
        : Control,
        ISiberianGrid
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SiberianGrid
            (
                ISiberianProvider provider,
                ISiberianColumnCollection? columns,
                ISiberianRowCollection? rows
            )
        {
            Columns = columns ?? new SiberianColumnCollection();
            Rows = rows ?? new SiberianRowCollection(this, provider);

        } // constructor

        #endregion

        #region ISiberianGrid members

        /// <inheritdoc cref="ISiberianGrid.GridClick"/>
        public event EventHandler<SiberianClickEventArgs>? GridClick;

        /// <inheritdoc cref="ISiberianGrid.Columns"/>
        public ISiberianColumnCollection Columns { get; }

        /// <inheritdoc cref="ISiberianGrid.Rows"/>
        public ISiberianRowCollection Rows { get; }

        #endregion

    } // class SiberianGrid

} // namespace ManagedIrbis.WinForms.Grid
