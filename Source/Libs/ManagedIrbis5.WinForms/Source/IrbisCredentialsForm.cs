// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisCredentialsForm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    ///
    /// </summary>
    public partial class IrbisCredentialsForm
        : Form
    {
        #region Properties

        /// <summary>
        /// Server.
        /// </summary>
        public string Server
        {
            get => _serverBox.Text;
            set => _serverBox.Text = value;
        }

        /// <summary>
        /// Port.
        /// </summary>
        public string Port
        {
            get => _portBox.Text;
            set => _portBox.Text = value;
        }

        /// <summary>
        /// User.
        /// </summary>
        public string User
        {
            get => _loginBox.Text;
            set => _loginBox.Text = value;
        }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password
        {
            get => _passwordBox.Text;
            set => _passwordBox.Text = value;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisCredentialsForm()
        {
            InitializeComponent();
        }

        #endregion
    }
}
