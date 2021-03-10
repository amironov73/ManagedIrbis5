// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ConnectionFactory.cs -- фабрика подключений к ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

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
        /// Общий экземпляр фабрики подключений.
        /// </summary>
        public static ConnectionFactory Shared { get; private set; } = new ();

        #endregion

        #region Public methods

        public virtual IIrbisConnection CreateConnection()
        {
            var socket = new PlainTcp4Socket();
            var result = new Connection(socket, Magna.Host.Services);

            return result;
        } // method CreateConnection


        /// <summary>
        /// Замена общего экземпляра фабрики на указанный.
        /// </summary>
        /// <param name="newFactory">Экземпляр, который отныне станет общим.
        /// </param>
        /// <returns>Предыдущий общий экземпляр.</returns>
        public static ConnectionFactory Replace
            (
                ConnectionFactory newFactory
            )
        {
            var result = Shared;
            Shared = newFactory;

            return result;
        } // method Replace

        #endregion

    } // class ConnectionFactory

} // namespace ManagedIrbis
