// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PreviewPanel.cs --
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

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Preview documents.
    /// </summary>
    public partial class PreviewPanel
        : UserControl
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PreviewPanel()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private Control? _previewControl;
        private TextBox? _previewTextBox;
        private RichTextBox? _previewRichTextBox;
        private WebBrowser? _previewHtmlTextBox;

        private void _DisposePreviousControl()
        {
            if (_previewControl != null)
            {
                _viewPage.Controls.Remove(_previewControl);
                _previewControl.Dispose();
            }

            _previewControl = null;
            _previewTextBox = null;
            _previewRichTextBox = null;
            _previewHtmlTextBox = null;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Set HTML text in the preview pane.
        /// </summary>
        public void SetHtmlText
        (
            string? text
        )
        {
            _DisposePreviousControl();

            _previewHtmlTextBox = new WebBrowser
            {
                DocumentText = text,
                Dock = DockStyle.Fill
            };
            _viewPage.Controls.Add(_previewHtmlTextBox);
            _previewControl = _previewHtmlTextBox;
        }

        /// <summary>
        /// Set plain text in the preview pane.
        /// </summary>
        public void SetPlainText
            (
                string? text
            )
        {
            _DisposePreviousControl();

            _previewTextBox = new TextBox
            {
                Text = text,
                Dock = DockStyle.Fill,
                Multiline = true,
                WordWrap = true,
                ScrollBars = ScrollBars.Vertical
            };
            _viewPage.Controls.Add(_previewTextBox);
            _previewControl = _previewTextBox;
        }

        /// <summary>
        /// Set rich text in the preview pane.
        /// </summary>
        public void SetRichText
            (
                string? text
            )
        {
            _DisposePreviousControl();

            _previewRichTextBox = new RichTextBox
            {
                Rtf = text,
                Dock = DockStyle.Fill,
                Multiline = true,
                WordWrap = true
            };
            _viewPage.Controls.Add(_previewRichTextBox);
            _previewControl = _previewRichTextBox;
        }

        /// <summary>
        /// Set text to preview pane.
        /// </summary>
        public void SetText
            (
                string? text
            )
        {
            /*
            TextKind textKind
                = TextUtility.DetermineTextKind(text);

            switch (textKind)
            {
                case TextKind.PlainText:
                    SetPlainText(text);
                    break;

                case TextKind.Html:
                    SetHtmlText(text);
                    break;

                case TextKind.RichText:
                    SetRichText(text);
                    break;
            }

            */
        }

        #endregion
    }
}
