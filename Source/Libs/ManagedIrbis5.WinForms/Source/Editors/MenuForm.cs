// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* MenuForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Editors
{
    /// <summary>
    ///
    /// </summary>
    public partial class MenuForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Current menu entry.
        /// </summary>
        public MenuEntry? CurrentEntry
        {
            get
            {
                var row = _grid.CurrentRow;

                if (ReferenceEquals(row, null))
                {
                    return null;
                }

                MenuEntry result = (MenuEntry) row.Data;

                return result;
            }
        }

        /// <summary>
        /// Entries
        /// </summary>
        public MenuEntry[] Entries { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuForm()
        {
            InitializeComponent();

            Entries = new MenuEntry[0];
            _grid.Focus();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Set entries.
        /// </summary>
        public void SetEntries
            (
                MenuEntry[] entries
            )
        {
            Entries = entries;
            _grid.Load(entries);
        }

        #endregion

        private void _grid_DoubleClick
            (
                object sender,
                EventArgs e
            )
        {
            DialogResult = DialogResult.OK;
        }

        private void _grid_PreviewKeyDown
            (
                object sender,
                PreviewKeyDownEventArgs e
            )
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.IsInputKey = false;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
