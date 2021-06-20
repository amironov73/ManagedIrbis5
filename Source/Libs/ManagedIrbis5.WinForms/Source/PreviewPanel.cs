// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PreviewPanel.cs -- панель предварительного просмотра карточки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Панель предварительного просмотра карточки.
    /// </summary>
    public partial class PreviewPanel
        : UserControl
    {
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
            // TODO: реализовать по-человечески
            if (!ReferenceEquals(_previewControl, _previewHtmlTextBox))
            {
                _DisposePreviousControl();
            }

            if (_previewHtmlTextBox == null)
            {
                _previewHtmlTextBox = new WebBrowser
                {
                    Dock = DockStyle.Fill
                };
                _viewPage.Controls.Add(_previewHtmlTextBox);
            }

            _previewHtmlTextBox.DocumentText = text;
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
            // TODO: реализовать по-человечески
            if (!ReferenceEquals(_previewControl, _previewTextBox))
            {
                _DisposePreviousControl();
            }

            if (_previewTextBox == null)
            {
                _previewTextBox = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    WordWrap = true,
                    ScrollBars = ScrollBars.Vertical
                };
                _viewPage.Controls.Add(_previewTextBox);
            }

            _previewTextBox.Text = text;
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
            // TODO: реализовать по-человечески
            if (!ReferenceEquals(_previewControl, _previewRichTextBox))
            {
                _DisposePreviousControl();
            }

            if (_previewRichTextBox == null)
            {
                _previewRichTextBox = new RichTextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    WordWrap = true
                };
                _viewPage.Controls.Add(_previewRichTextBox);
            }

            _previewRichTextBox.Rtf = text;
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
            var textKind = TextUtility.DetermineTextKind(text);

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

        } // method SetText

        #endregion

    } // class PreviewPanel

} // namespace ManagedIrbis.WinForms
