// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable InconsistentlySynchronizedField
// ReSharper disable UnusedMember.Global

/* WssSession.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Net;
using System.Text;

#endregion

#nullable enable

namespace NetCoreServer;

/// <summary>
/// WebSocket server
/// </summary>
/// <remarks> WebSocket server is used to communicate with clients using WebSocket protocol. Thread-safe.</remarks>
public class WsServer
    : HttpServer, IWebSocket
{
    internal readonly WebSocket WebSocket;

    /// <summary>
    /// Initialize WebSocket server with a given IP address and port number
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="port">Port number</param>
    public WsServer
        (
            IPAddress address,
            int port
        )
        : base (address, port)
    {
        WebSocket = new WebSocket (this);
    }

    /// <summary>
    /// Initialize WebSocket server with a given IP address and port number
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="port">Port number</param>
    public WsServer
        (
            string address,
            int port
        )
        : base (address, port)
    {
        WebSocket = new WebSocket (this);
    }

    /// <summary>
    /// Initialize WebSocket server with a given DNS endpoint
    /// </summary>
    /// <param name="endpoint">DNS endpoint</param>
    public WsServer
        (
            DnsEndPoint endpoint
        )
        : base (endpoint)
    {
        WebSocket = new WebSocket (this);
    }

    /// <summary>
    /// Initialize WebSocket server with a given IP endpoint
    /// </summary>
    /// <param name="endpoint">IP endpoint</param>
    public WsServer
        (
            IPEndPoint endpoint
        )
        : base (endpoint)
    {
        WebSocket = new WebSocket (this);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public virtual bool CloseAll
        (
            int status
        )
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame
                (
                    opcode: WebSocket.WS_FIN | WebSocket.WS_CLOSE,
                    mask: false,
                    buffer: null,
                    offset: 0,
                    size: 0,
                    status
                );
            if (!Multicast (WebSocket.WsSendBuffer.ToArray()))
            {
                return false;
            }

            return base.DisconnectAll();
        }
    }

    /// <inheritdoc cref="TcpServer.Multicast(byte[],long,long)"/>
    public override bool Multicast
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        if (!IsStarted)
        {
            return false;
        }

        if (size == 0)
        {
            return true;
        }

        // Multicast data to all WebSocket sessions
        foreach (var session in Sessions.Values)
        {
            if (session is WsSession { WebSocket.WsHandshaked: true } wsSession)
            {
                wsSession.SendAsync (buffer, offset, size);
            }
        }

        return true;
    }

    #region WebSocket multicast text methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool MulticastText
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame
                (
                    opcode: WebSocket.WS_FIN | WebSocket.WS_TEXT,
                    mask: false,
                    buffer,
                    offset,
                    size
                );
            return Multicast (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool MulticastText
        (
            string text
        )
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame
                (
                    opcode: WebSocket.WS_FIN | WebSocket.WS_TEXT,
                    mask: false,
                    data,
                    offset: 0,
                    data.Length
                );
            return Multicast (WebSocket.WsSendBuffer.ToArray());
        }
    }

    #endregion

    #region WebSocket multicast binary methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool MulticastBinary
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame
                (
                    opcode: WebSocket.WS_FIN | WebSocket.WS_BINARY,
                    mask: false,
                    buffer,
                    offset,
                    size
                );
            return Multicast (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool MulticastBinary
        (
            string text
        )
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame
                (
                    opcode: WebSocket.WS_FIN | WebSocket.WS_BINARY,
                    mask: false,
                    data,
                    offset: 0,
                    data.Length
                );
            return Multicast (WebSocket.WsSendBuffer.ToArray());
        }
    }

    #endregion

    #region WebSocket multicast ping methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool SendPing
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame
                (
                    opcode: WebSocket.WS_FIN | WebSocket.WS_PING,
                    mask: false,
                    buffer,
                    offset,
                    size
                );
            return Multicast (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool SendPing
        (
            string text
        )
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame
                (
                    opcode: WebSocket.WS_FIN | WebSocket.WS_PING,
                    mask: false,
                    data,
                    offset: 0,
                    data.Length
                );
            return Multicast (WebSocket.WsSendBuffer.ToArray());
        }
    }

    #endregion

    #region WebSocket multicast pong methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool SendPong
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame
                (
                    opcode: WebSocket.WS_FIN | WebSocket.WS_PONG,
                    mask: false,
                    buffer,
                    offset,
                    size
                );
            return Multicast (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool SendPong (string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame
                (
                    opcode: WebSocket.WS_FIN | WebSocket.WS_PONG,
                    mask: false,
                    data,
                    offset: 0,
                    data.Length
                );
            return Multicast (WebSocket.WsSendBuffer.ToArray());
        }
    }

    #endregion

    /// <inheritdoc cref="HttpServer.CreateSession"/>
    protected override TcpSession CreateSession()
    {
        return new WsSession (this);
    }
}
