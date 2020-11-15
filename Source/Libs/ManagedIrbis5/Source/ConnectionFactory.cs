// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable CommentTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Infrastructure.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Фабрика подключений.
    /// </summary>
    public class ConnectionFactory
    {
        #region Properties

        /// <summary>
        /// Экземпляр фабрики по умолчанию.
        /// </summary>
        public static ConnectionFactory Default { get; } = new ();

        #endregion

        #region Private members

        #endregion

        #region Public methods

        public virtual Connection CreateConnection()
        {
            var socket = new PlainTcp4Socket();
            var result = new Connection(socket);

            return result;
        }

        #endregion
    }
}
