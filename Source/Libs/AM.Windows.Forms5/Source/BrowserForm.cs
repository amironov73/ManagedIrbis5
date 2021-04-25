// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BrowserForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public partial class BrowserForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Gets or sets the document text.
        /// </summary>
        [DefaultValue(null)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DocumentText
        {
            get => _webBrowser.DocumentText;
            set => _webBrowser.DocumentText = value;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="BrowserForm"/> class.
        /// </summary>
        public BrowserForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void _copyButton_Click(object sender, System.EventArgs e)
        {
            Clipboard.SetText(DocumentText, TextDataFormat.Html);
        }

        private void _openButton_Click(object sender, System.EventArgs e)
        {
            if (_openFileDialog.ShowDialog(this)
                 == DialogResult.OK)
            {
                _webBrowser.Navigate(_openFileDialog.FileName);
            }
        }

        private void _saveButton_Click(object sender, System.EventArgs e)
        {
            if (_saveFileDialog.ShowDialog(this)
                 == DialogResult.OK)
            {
                File.WriteAllText(_saveFileDialog.FileName, DocumentText);
            }
        }

        private void _pageSetupButton_Click
            (
                object? sender,
                System.EventArgs e
            )
        {
            _webBrowser.ShowPageSetupDialog();
        }

        private void _pasteButton_Click
            (
                object? sender,
                System.EventArgs e
            )
        {
            var text = Clipboard.GetText(TextDataFormat.Html);

            if (!string.IsNullOrEmpty(text))
            {
                DocumentText = text;
            }
        }

        private void _printButton_Click(object sender, System.EventArgs e)
        {
            _webBrowser.ShowPrintDialog();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the document.
        /// </summary>
        public HtmlDocument GetDocument()
        {
            if (_webBrowser.Document == null)
            {
                _webBrowser.Navigate("about:blank");
            }

            return _webBrowser.Document.ThrowIfNull("_webBrowser.Document");
        }

        #endregion
    }
}
