// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ConnectionBuilder.cs -- паттерн Builder для Connection
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using ManagedIrbis.Infrastructure.Sockets;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Паттерн Builder для <see cref="SyncConnection"/>.
    /// </summary>
    public sealed class ConnectionBuilder
    {
        #region Private members

        private string? _connectionString;
        private ILogger? _logger;
        private ISyncClientSocket? _socket;

        #endregion

        #region Public methods

        /// <summary>
        /// Создание подключения.
        /// </summary>
        public SyncConnection Build()
        {
            // TODO: делать ISyncIrbisProvider

            var socket = _socket ?? new SyncTcp4Socket();
            var result = new SyncConnection(socket, Magna.Host.Services);

            if (_logger is not null)
            {
                result._logger = _logger;
            }

            if (_connectionString is not null)
            {
                result.ParseConnectionString(_connectionString);
            }

            return result;
        } // method Build

        /// <summary>
        /// Установка строки подключения.
        /// </summary>
        public ConnectionBuilder WithConnectionString
            (
                string connectionString
            )
        {
            _connectionString = connectionString;

            return this;
        } // method WithConnectionString

        /// <summary>
        /// Установка логгера.
        /// </summary>
        public ConnectionBuilder WithLogger
            (
                ILogger logger
            )
        {
            _logger = logger;

            return this;
        } // method WithLogger

        /// <summary>
        /// Установка сокета.
        /// </summary>
        public ConnectionBuilder WithSocket
            (
                ISyncClientSocket socket
            )
        {
            _socket = socket;

            return this;
        } // method WithSocket

        #endregion

    } // class ConnectionBuilder

} // namespace ManagedIrbis
