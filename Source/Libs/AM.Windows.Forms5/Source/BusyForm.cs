// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BusyForm.cs -- диалог "ИРБИС занят, подождите"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Диалог "ИРБИС занят, подождите"
    /// </summary>
    public partial class BusyForm
        : Form
    {
        #region Events

        /// <summary>
        /// Break button pressed.
        /// </summary>
        public event EventHandler? BreakPressed;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BusyForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void _breakButton_Click(object sender, EventArgs e)
            => BreakPressed?.Invoke(this, e);

        #endregion

        #region Public methods

        /// <summary>
        /// Set form title.
        /// </summary>
        public void SetTitle
            (
                string title
            )
        {
            Text = title;
        }

        /// <summary>
        /// Set form message.
        /// </summary>
        public void SetMessage
            (
                string message
            )
        {
            _messageLabel.Text = message;
        }

        #endregion

    }
}
