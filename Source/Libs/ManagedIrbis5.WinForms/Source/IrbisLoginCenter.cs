// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisLoginCenter.cs -- вход в ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Windows.Forms;
using AM;
using ManagedIrbis;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms
{
    /// <summary>
    /// Вход в ИРБИС.
    /// </summary>
    public static class IrbisLoginCenter
    {
        #region Public methods

        /// <summary>
        /// Простой вход в сервер.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static bool Login
            (
                this ISyncConnection connection,
                IWin32Window? owner
            )
        {
            using var form = new IrbisLoginForm
            {
                Username = connection.Username, Password = connection.Password
            };
            form.Text = $"{form.Text} - {connection.Host}";

            if (ReferenceEquals(owner, null))
            {
                form.StartPosition = FormStartPosition.CenterScreen;
            }

            var result = form.ShowDialog (owner);
            if (result == DialogResult.OK)
            {
                connection.Username = form.Username;
                connection.Password = form.Password;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Вход в сервер с указанием адреса.
        /// </summary>
        public static bool Login2
            (
                this ISyncConnection connection,
                IWin32Window? owner
            )
        {
            using var form = new IrbisLoginForm2
            {
                Host = connection.Host,
                Port = connection.Port.ToString(CultureInfo.InvariantCulture),
                Username = connection.Username,
                Password = connection.Password
            };
            form.Text = $"{form.Text} - {connection.Host}";

            var result = form.ShowDialog(owner);
            if (result == DialogResult.OK)
            {
                connection.Host = form.Host;
                connection.Port = form.Port.ParseUInt16();
                connection.Username = form.Username;
                connection.Password = form.Password;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Try to login.
        /// </summary>
        public static bool TryLogin
            (
                this ISyncConnection connection,
                IWin32Window? owner
            )
        {
            while (Login(connection, owner))
            {
                try
                {
                    connection.Connect();
                    if (connection.IsConnected)
                    {
                        return true;
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Try to login with host name.
        /// </summary>
        public static bool TryLogin2
            (
                this ISyncConnection connection,
                IWin32Window? owner
            )
        {
            while (Login2(connection, owner))
            {
                try
                {
                    connection.Connect();
                    if (connection.IsConnected)
                    {
                        return true;
                    }
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                    return false;
                }
            }

            return false;
        }

        #endregion
    }
}
