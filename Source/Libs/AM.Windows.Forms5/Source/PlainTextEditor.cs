// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* PlainTextEditor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

using AM.Drawing.Printing;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public partial class PlainTextEditor
        : UserControl
    {
        #region Properties

        private string? FileName { get; set; }

        /// <inheritdoc />
        public override string Text
        {
            get => _textBox.Text;
            set => _textBox.Text = value;
        }

        /// <summary>
        /// Text box.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TextBox TextBox => _textBox;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlainTextEditor()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void _newToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Clear();
        }

        private void _openToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            LoadFromFile();
        }

        private void _saveToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            SaveToFile();
        }

        private void _printToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Print();
        }

        private void _cutToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Cut();
        }

        private void _copyToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Copy();
        }

        private void _pasteToolStripButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Paste();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add the button to the toolbox.
        /// </summary>
        public PlainTextEditor AddButton
            (
                ToolStripButton button
            )
        {
            _toolStrip.Items.Add(button);

            return this;
        }

        /// <summary>
        /// Clear the text area.
        /// </summary>
        public void Clear()
        {
            _textBox.Clear();
        }

        /// <summary>
        /// Copy selected text to the clipboard.
        /// </summary>
        public void Copy()
        {
            _textBox.Copy();
        }

        /// <summary>
        /// Cut selected text to the clipboard.
        /// </summary>
        public void Cut()
        {
            _textBox.Cut();
        }

        /// <summary>
        /// Load text from file.
        /// </summary>
        public void LoadFromFile()
        {
            if (_openFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = _openFileDialog.FileName
                    .ThrowIfNull("FileName");

                LoadFromFile(FileName);
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
            Text = File.ReadAllText(fileName, Encoding.UTF8);
        }

        /// <summary>
        /// Paste text from the clipboard.
        /// </summary>
        public void Paste()
        {
            _textBox.Paste();
        }

        /// <summary>
        /// Print the text.
        /// </summary>
        public void Print()
        {
            var printer = new PlainTextPrinter();
            printer.Print(Text);
        }

        /// <summary>
        /// Save the text to file.
        /// </summary>
        public void SaveToFile()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                if (_saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                FileName = _saveFileDialog.FileName
                    .ThrowIfNull("FileName");
            }
            SaveToFile(FileName);
        }

        /// <summary>
        /// Save the text to the file.
        /// </summary>
        public void SaveToFile
            (
                string fileName
            )
        {
            File.WriteAllText(fileName, Text, Encoding.UTF8);
        }

        #endregion
    }
}
