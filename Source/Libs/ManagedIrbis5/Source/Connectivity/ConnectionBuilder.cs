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

/* ConnectionBuilder.cs -- паттерн Builder для SyncConnection
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Infrastructure.Sockets;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Паттерн Builder для <see cref="SyncConnection"/>.
/// </summary>
public sealed class ConnectionBuilder
{
    #region Private members

    private string? _connectionString;
    private ILogger? _logger;
    private ISyncClientSocket? _socket;
    private IServiceProvider? _serviceProvider;

    #endregion

    #region Public methods

    /// <summary>
    /// Создание подключения.
    /// </summary>
    public SyncConnection Build()
    {
        // TODO: делать ISyncProvider

        var socket = _socket ?? new SyncTcp4Socket();
        var serviceProvider = _serviceProvider ?? Magna.Host.Services;
        var result = new SyncConnection (socket, serviceProvider);

        if (_logger is not null)
        {
            result._logger = _logger;
        }

        if (_connectionString is not null)
        {
            result.ParseConnectionString (_connectionString);
        }

        return result;
    }

    /// <summary>
    /// Установка провайдера сервисов.
    /// </summary>
    public ConnectionBuilder WithServiceProvider
        (
            IServiceProvider serviceProvider
        )
    {
        Sure.NotNull (serviceProvider);

        _serviceProvider = serviceProvider;

        return this;
    }

    /// <summary>
    /// Установка строки подключения.
    /// </summary>
    public ConnectionBuilder WithConnectionString
        (
            string connectionString
        )
    {
        Sure.NotNull (connectionString);

        _connectionString = connectionString;

        return this;
    }

    /// <summary>
    /// Установка логгера.
    /// </summary>
    public ConnectionBuilder WithLogger
        (
            ILogger logger
        )
    {
        Sure.NotNull (logger);

        _logger = logger;

        return this;
    }

    /// <summary>
    /// Установка сокета.
    /// </summary>
    public ConnectionBuilder WithSocket
        (
            ISyncClientSocket socket
        )
    {
        Sure.NotNull (socket);
        Sure.AssertState (socket.Connection is null);

        _socket = socket;

        return this;
    }

    #endregion
}
