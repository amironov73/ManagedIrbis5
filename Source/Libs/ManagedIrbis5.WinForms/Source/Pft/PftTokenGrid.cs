﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftTokenGrid.cs -- таблица, отображающая токены PFT-скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using ManagedIrbis.Pft.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Pft
{
    /// <summary>
    /// Таблица, отображающая токены PFT-скрипта.
    /// </summary>
    public partial class PftTokenGrid
        : UserControl
    {
        #region Events

        /// <summary>
        /// Cell double click.
        /// </summary>
        public event EventHandler? CellDoubleClick;

        #endregion

        #region Properites

        /// <summary>
        /// Selected token.
        /// </summary>
        public PftToken? SelectedToken => _grid.CurrentRow?.DataBoundItem as PftToken;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftTokenGrid()
        {
            InitializeComponent();

            _grid.CellDoubleClick += _grid_CellDoubleClick;
        }

        #endregion

        #region Private members

        void _grid_CellDoubleClick
            (
                object? sender,
                DataGridViewCellEventArgs e
            )
        {
            CellDoubleClick?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear.
        /// </summary>
        public void Clear()
        {
            _grid.DataSource = null;
        }

        /// <summary>
        /// Set tokens.
        /// </summary>
        public void SetTokens
            (
                PftTokenList tokens
            )
        {
            _grid.AutoGenerateColumns = false;
            _grid.DataSource = tokens.ToArray();
        }

        #endregion

    } // class PftTokenGrid

} // namespace ManagedIrbis.WinForms.Pft
