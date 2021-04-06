// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripCustomizationForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Customization form for ToolStrip.
    /// </summary>
    partial class ToolStripCustomizationForm
        : Form
    {
        #region Properties

        /// <summary>
        /// ToolStrip.
        /// </summary>
        public ToolStrip ToolStrip { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ToolStripCustomizationForm
            (
                ToolStrip toolStrip
            )
        {
            ToolStrip = toolStrip;

            InitializeComponent();
        }

        #endregion

        #region Private members

        private void ToolStripCustomizationForm_Load
            (
                object sender,
                EventArgs e
            )
        {
            try
            {
                _listBox.BeginUpdate();
                _listBox.Items.Clear();
                foreach (ToolStripItem item in ToolStrip.Items)
                {
                    _listBox.Items.Add(item, item.Available);
                }
            }
            finally
            {
                _listBox.EndUpdate();
            }
        }

        private void _applyButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            foreach (ToolStripItem item in _listBox.Items)
            {
                int index = _listBox.Items.IndexOf(item);
                item.Available = _listBox.GetItemChecked(index);
            }
        }

        #endregion
    }
}
