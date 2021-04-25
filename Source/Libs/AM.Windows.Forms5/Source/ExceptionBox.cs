// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* ExceptionBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Windows.Forms;

using AM.Windows.Forms.Printing;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public partial class ExceptionBox
        : ModalForm
    {
        #region Properties

        /// <summary>
        /// Exception.
        /// </summary>
        public Exception? Exception
        {
            get => _exception;
            set
            {
                _exception = value;
                _textBox.Text = value
                    .ThrowIfNull()
                    .ToString();
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ExceptionBox()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private Exception? _exception;

        private BinaryAttachment[]?_attachments;

        private void _abortButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Application.Exit();
        }

        private void _closeButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            DialogResult = DialogResult.OK;
        }

        private void _copyButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            Clipboard.SetText(_textBox.Text);
        }

        private void _printButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            using var printer = new PlainTextPrinter();
            printer.Print(_textBox.Text);
        }

        private void _saveButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (_saveFileDialog.ShowDialog(this)
                == DialogResult.OK)
            {
                File.WriteAllText
                    (
                        _saveFileDialog.FileName,
                        _textBox.Text
                    );
            }

        }

        private void _attachmentsButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            using var box = new AttachmentBox
                (
                    _attachments ?? Array.Empty<BinaryAttachment>()
                );
            box.ShowDialog(this);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Shows the specified exception.
        /// </summary>
        public static void Show
            (
                IWin32Window? parent,
                Exception exception
            )
        {
            using var box = new ExceptionBox
            {
                _typeLabel = {Text = exception.GetType().ToString()},
                _messageLabel = {Text = exception.Message},
                _textBox = {Text = exception.ToString()}
            };

            // ReSharper disable SuspiciousTypeConversion.Global
            if (exception is IAttachmentContainer container)
            {
                box._attachments = container.ListAttachments();
                if (box._attachments.Length != 0)
                {
                    box._attachmentsButton.Enabled = true;
                }
            }
            // ReSharper restore SuspiciousTypeConversion.Global

            box.ShowDialog(parent);
        }

        /// <summary>
        /// Shows the specified exception.
        /// </summary>
        public static void Show
            (
                Exception exception
            )
        {
            Show(null, exception);
        }

        #endregion
    }
}
