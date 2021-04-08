// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisLoginForm2.cs -- логин и пароль для входа в ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Окно с вводом адреса сервера, логина
    /// и пароля для входа в ИРБИС.
    /// </summary>
    public partial class IrbisLoginForm2
        : Form
    {
        #region Properties

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        public string? Host
        {
            get => _serverBox.Text;
            set => _serverBox.Text = value;
        }

        /// <summary>
        /// Порт для подключения.
        /// </summary>
        public string? Port
        {
            get => _portBox.Text;
            set => _portBox.Text = value;
        }

        /// <summary>
        /// Логин.
        /// </summary>
        public string? Username
        {
            get => _nameBox.Text;
            set => _nameBox.Text = value;
        }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string? Password
        {
            get => _passwordBox.Text;
            set => _passwordBox.Text = value;
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IrbisLoginForm2()
        {
            InitializeComponent();
        }

        #endregion

        #region Private members

        private void _okButton_Click
            (
                object sender,
                EventArgs e
            )
        {
            if (string.IsNullOrEmpty(_serverBox.Text))
            {
                _serverBox.Focus();
                DialogResult = DialogResult.None;
            }
            else if (string.IsNullOrEmpty(_portBox.Text))
            {
                _portBox.Focus();
                DialogResult = DialogResult.None;
            }
            else if (string.IsNullOrEmpty(_nameBox.Text))
            {
                _nameBox.Focus();
                DialogResult = DialogResult.None;
            }
            else if (string.IsNullOrEmpty(_passwordBox.Text))
            {
                _passwordBox.Focus();
                DialogResult = DialogResult.None;
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Apply the connection settings.
        /// </summary>
        public IrbisLoginForm2 ApplySettings
            (
                IIrbisConnectionSettings settings
            )
        {
            Host = settings.Host;
            Port = settings.Port.ToInvariantString();
            Username = settings.Username;
            Password = settings.Password;

            return this;
        }

        /// <summary>
        /// Convert to <see cref="ConnectionSettings"/>.
        /// </summary>
        public ConnectionSettings GatherSettings()
        {
            ConnectionSettings result = new ConnectionSettings
            {
                Host = Host,
                Port = Port.ThrowIfNull().ParseUInt16(),
                Username = Username,
                Password = Password
            };

            return result;
        }

        #endregion

    }
}
