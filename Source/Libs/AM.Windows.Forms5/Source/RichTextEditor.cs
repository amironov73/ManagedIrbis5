﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* RichTextEditor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public partial class RichTextEditor
        : UserControl
    {
        #region Properties

        private string? FileName { get; set; }

        /// <summary>
        /// Text box.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RichTextBox RichTextBox
        {
            get { return _richTextBox; }
        }

        /// <summary>
        /// Formatted text.
        /// </summary>
        public string Rtf
        {
            get { return _richTextBox.Rtf; }
            set { _richTextBox.Rtf = value; }
        }

        /// <inheritdoc />
        public override string Text
        {
            get { return _richTextBox.Text; }
            set { _richTextBox.Text = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RichTextEditor ()
        {
            InitializeComponent ();
        }

        #endregion

        #region Private members

        private void _newToolStripButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void _openToolStripButton_Click(object sender, EventArgs e)
        {
            LoadFromFile();
        }

        private void _saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveToFile();
        }

        private void _cutToolStripButton_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void _copyToolStripButton_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void _pasteToolStripButton_Click(object sender, EventArgs e)
        {
            Paste();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Clear the text area.
        /// </summary>
        public void Clear()
        {
            _richTextBox.Clear();
        }

        /// <summary>
        /// Copy selected text to the clipboard.
        /// </summary>
        public void Copy()
        {
            _richTextBox.Copy();
        }

        /// <summary>
        /// Cut the selected text to the clipboard.
        /// </summary>
        public void Cut()
        {
            _richTextBox.Cut();
        }

        /// <summary>
        /// Load text from file.
        /// </summary>
        public void LoadFromFile()
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = _openFileDialog.FileName.ThrowIfNull();

                LoadFromFile (FileName);
            }
        }

        /// <summary>
        /// Load text from the file.
        /// </summary>
        public void LoadFromFile
            (
                string fileName
            )
        {
            Rtf = File.ReadAllText(fileName, Encoding.UTF8);
        }

        /// <summary>
        /// Paste text from the clipboard.
        /// </summary>
        public void Paste()
        {
            _richTextBox.Paste();
        }

        /// <summary>
        /// Save text to file.
        /// </summary>
        public void SaveToFile()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                if (_saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                FileName = _saveFileDialog.FileName.ThrowIfNull();
            }
            SaveToFile(FileName);
        }

        /// <summary>
        /// Save text to the file.
        /// </summary>
        public void SaveToFile
            (
                string fileName
            )
        {
            File.WriteAllText (fileName, Rtf, Encoding.UTF8);
        }

        #endregion
    }
}
