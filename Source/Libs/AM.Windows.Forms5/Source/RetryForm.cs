// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* RetryForm.cs --
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
    ///
    /// </summary>
    public partial class RetryForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Message text.
        /// </summary>
        public string Message
        {
            get { return _messageLabel.Text; }
            set { _messageLabel.Text = value; }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RetryForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private static bool _Resolver
            (
                Exception exception
            )
        {
            using (RetryForm form = new RetryForm())
            {
                form.Message = exception.Message;
                DialogResult result = form.ShowDialog();

                return result == DialogResult.Yes;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get resolver.
        /// </summary>
        public static Func<Exception, bool> GetResolver()
        {
            return _Resolver;
        }

        #endregion
    }
}
