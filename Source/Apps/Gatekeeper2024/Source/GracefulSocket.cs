// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GracefulSocket.cs -- сокет, умеющий отваливаться быстро и безболезненно
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Net.Sockets;

using AM;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Сокет, умеющий отваливаться быстро и безболезненно.
/// </summary>
public sealed class GracefulSocket
    : ISyncClientSocket
{
    #region Properties

    /// <summary>
    /// Игнорируем.
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// Игнорируем.
    /// </summary>
    public int RetryDelay { get; set; }

    /// <summary>
    /// Тайм-аут подключения и сетевого обмена, миллисекунды.
    /// </summary>
    public int Timeout { get; set; } = 100;

    /// <summary>
    /// Подключение к ИРБИС-серверу, которое обслуживает данный сокет.
    /// </summary>
    public ISyncConnection? Connection { get; set; }

    #endregion

    #region ISyncClientSocket members

    /// <inheritdoc cref="ISyncClientSocket.TransactSync"/>
    public Response? TransactSync
        (
            SyncQuery query
        )
    {
        var connection = Connection.ThrowIfNull();
        connection.ThrowIfCancelled();

        var logger = connection.Logger;
        logger?.LogTrace ($"{nameof (SyncTcp4Socket)}::{nameof (TransactSync)}: enter");

        using var client = new TcpClient (AddressFamily.InterNetwork);
        try
        {
            // устанавливаем краткие тайм-ауты
            client.SendTimeout = Timeout;
            client.ReceiveTimeout = Timeout;

            var host = connection.Host.ThrowIfNull();
            logger?.LogTrace ("Connecting to {Host}", host);

            // вместо синхронной версии вызова
            // client.Connect (host, connection.Port);

            // используем асинхронную версию, хоть это и неправильно
            // зато дает возможность установить тайм-аут на подключение
            client.ConnectAsync (host, connection.Port).Wait (Timeout);
            if (!client.Connected)
            {
                logger?.LogError ("Error while connecting");
                connection.SetLastError (-100_002);

                return default;
            }

            logger?.LogTrace ("Connected to {Host}", host);
        }
        catch (Exception exception)
        {
            logger?.LogError (exception, "Error while connecting");
            connection.SetLastError (-100_002);

            return default;
        }

        connection.ThrowIfCancelled();

        var socket = client.Client;
        var length = query.GetLength();
        Span<byte> prefix = stackalloc byte[12];
        length = FastNumber.Int32ToBytes (length, prefix);
        prefix[length] = 10; // перевод строки
        prefix = prefix.Slice (0, length + 1);
        var body = query.GetBody();

        try
        {
            logger?.LogTrace ("Sending");
            socket.Send (prefix, SocketFlags.None);
            socket.Send (body.Span, SocketFlags.None);
            socket.Shutdown (SocketShutdown.Send);
            logger?.LogTrace ("Send OK");
        }
        catch (Exception exception)
        {
            logger?.LogError (exception, "Error while sending");
            connection.SetLastError (-100_002);

            return default;
        }

        logger?.LogTrace ("Receiving");
        var result = new Response (Connection.ThrowIfNull());
        try
        {
            while (true)
            {
                connection.ThrowIfCancelled();

                var buffer = new byte[2048];
                var chunk = new ArraySegment<byte> (buffer, 0, buffer.Length);
                var read = socket.Receive (chunk, SocketFlags.None);
                if (read <= 0)
                {
                    break;
                }

                chunk = new ArraySegment<byte> (buffer, 0, read);
                result.Add (chunk);
            }

            logger?.LogTrace ("Receive OK");
        }
        catch (Exception exception)
        {
            logger?.LogError (exception, "Error while receiving");
            connection.SetLastError (-100_002);

            return default;
        }

        logger?.LogTrace ($"{nameof (SyncTcp4Socket)}::{nameof (TransactSync)} OK");

        return result;
    }

    #endregion
}
