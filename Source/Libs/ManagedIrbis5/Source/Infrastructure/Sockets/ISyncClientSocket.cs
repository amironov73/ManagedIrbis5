// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ISyncClientSocket.cs -- интерфейс синхронного клиентского сокета
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets;

/// <summary>
/// Интерфейс синхронного клиентского сокета.
/// </summary>
public interface ISyncClientSocket
{
    /// <summary>
    /// Количество повторов при сетевом сбое.
    /// </summary>
    int RetryCount { get; set; }

    /// <summary>
    /// Задержка при повторе, миллисекунды.
    /// </summary>
    int RetryDelay { get; set; }

    /// <summary>
    /// Подключение к ИРБИС-серверу, которое обслуживает данный сокет.
    /// </summary>
    ISyncConnection? Connection { get; set; }

    /// <summary>
    /// Собственно общение с сервером.
    /// </summary>
    Response? TransactSync
        (
            SyncQuery query
        );

}
