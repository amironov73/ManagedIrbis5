// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* AttachmentBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Shows list of attachments.
    /// </summary>
    public partial class AttachmentBox
        : Form
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AttachmentBox
            (
                IEnumerable<BinaryAttachment> attachments
            )
        {
            InitializeComponent();

            _listBox.Items.AddRange(attachments.ToArray());
        }

        #endregion

        #region Private members

        private void _saveButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_listBox.SelectedItem is BinaryAttachment attachment)
            {
                var rc = _saveFileDialog.ShowDialog(this);
                if (rc == DialogResult.OK)
                {
                    if (attachment.Content is { } content)
                    {
                        var fileName = _saveFileDialog.FileName;
                        File.WriteAllBytes
                        (
                            fileName,
                            content
                        );
                    }
                }
            }
        }

        #endregion
    }
}
