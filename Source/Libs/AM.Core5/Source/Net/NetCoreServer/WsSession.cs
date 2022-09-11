// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable InconsistentlySynchronizedField
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Net.Sockets;
using System.Text;

#endregion

#nullable enable

namespace NetCoreServer;

/// <summary>
/// WebSocket session
/// </summary>
/// <remarks> WebSocket session is used to read and write data from
/// the connected WebSocket client. Thread-safe.</remarks>
public class WsSession : HttpSession, IWebSocket
{
    internal readonly WebSocket WebSocket;

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="server"></param>
    public WsSession (WsServer server)
        : base (server)
    {
        WebSocket = new WebSocket (this);
    }

    #endregion

    // WebSocket connection methods
    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public virtual bool Close (int status)
    {
        SendCloseAsync (status, null, 0, 0);
        base.Disconnect();
        return true;
    }

    #region WebSocket send text methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public long SendText (byte[] buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_TEXT, false, buffer, offset, size);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public long SendText (string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_TEXT, false, data, 0, data.Length);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool SendTextAsync (byte[] buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_TEXT, false, buffer, offset, size);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool SendTextAsync (string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_TEXT, false, data, 0, data.Length);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    #endregion

    #region WebSocket send binary methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public long SendBinary (byte[] buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_BINARY, false, buffer, offset, size);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public long SendBinary (string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_BINARY, false, data, 0, data.Length);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool SendBinaryAsync (byte[] buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_BINARY, false, buffer, offset, size);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool SendBinaryAsync (string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_BINARY, false, data, 0, data.Length);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    #endregion

    #region WebSocket send close methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public long SendClose (int status, byte[] buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_CLOSE, false, buffer, offset, size, status);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public long SendClose (int status, string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_CLOSE, false, data, 0, data.Length, status);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool SendCloseAsync (int status, byte[]? buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_CLOSE, false, buffer, offset, size, status);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="status"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool SendCloseAsync (int status, string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_CLOSE, false, data, 0, data.Length, status);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    #endregion

    #region WebSocket send ping methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public long SendPing (byte[] buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_PING, false, buffer, offset, size);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public long SendPing (string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_PING, false, data, 0, data.Length);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool SendPingAsync (byte[] buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_PING, false, buffer, offset, size);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool SendPingAsync (string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_PING, true, data, 0, data.Length);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    #endregion

    #region WebSocket send pong methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public long SendPong (byte[] buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_PONG, false, buffer, offset, size);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public long SendPong (string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_PONG, false, data, 0, data.Length);
            return base.Send (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool SendPongAsync (byte[] buffer, long offset, long size)
    {
        lock (WebSocket.WsSendLock)
        {
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_PONG, false, buffer, offset, size);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public bool SendPongAsync (string text)
    {
        lock (WebSocket.WsSendLock)
        {
            var data = Encoding.UTF8.GetBytes (text);
            WebSocket.PrepareSendFrame (WebSocket.WS_FIN | WebSocket.WS_PONG, false, data, 0, data.Length);
            return base.SendAsync (WebSocket.WsSendBuffer.ToArray());
        }
    }

    #endregion

    #region WebSocket receive methods

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string ReceiveText()
    {
        var result = new Buffer();

        if (!WebSocket.WsHandshaked)
        {
            return result.ExtractString (0, result.Data.Length);
        }

        var cache = new Buffer();

        // Receive WebSocket frame data
        while (!WebSocket.WsFinalReceived)
        {
            while (!WebSocket.WsFrameReceived)
            {
                var required = WebSocket.RequiredReceiveFrameSize();
                cache.Resize (required);
                var received = (int)base.Receive (cache.Data, 0, required);
                if (received != required)
                {
                    return result.ExtractString (0, result.Data.Length);
                }

                WebSocket.PrepareReceiveFrame (cache.Data, 0, received);
            }

            if (!WebSocket.WsFinalReceived)
            {
                WebSocket.PrepareReceiveFrame (null, 0, 0);
            }
        }

        // Copy WebSocket frame data
        result.Append (WebSocket.WsReceiveFinalBuffer.ToArray(), 0, WebSocket.WsReceiveFinalBuffer.Count);
        WebSocket.PrepareReceiveFrame (null, 0, 0);
        return result.ExtractString (0, result.Data.Length);
    }

    public Buffer ReceiveBinary()
    {
        var result = new Buffer();

        if (!WebSocket.WsHandshaked)
        {
            return result;
        }

        var cache = new Buffer();

        // Receive WebSocket frame data
        while (!WebSocket.WsFinalReceived)
        {
            while (!WebSocket.WsFrameReceived)
            {
                var required = WebSocket.RequiredReceiveFrameSize();
                cache.Resize (required);
                var received = (int)base.Receive (cache.Data, 0, required);
                if (received != required)
                {
                    return result;
                }

                WebSocket.PrepareReceiveFrame (cache.Data, 0, received);
            }

            if (!WebSocket.WsFinalReceived)
            {
                WebSocket.PrepareReceiveFrame (null, 0, 0);
            }
        }

        // Copy WebSocket frame data
        result.Append (WebSocket.WsReceiveFinalBuffer.ToArray(), 0, WebSocket.WsReceiveFinalBuffer.Count);
        WebSocket.PrepareReceiveFrame (null, 0, 0);
        return result;
    }

    #endregion

    #region Session handlers

    protected override void OnDisconnecting()
    {
        if (WebSocket.WsHandshaked)
        {
            OnWsDisconnecting();
        }
    }

    protected override void OnDisconnected()
    {
        // Disconnect WebSocket
        if (WebSocket.WsHandshaked)
        {
            WebSocket.WsHandshaked = false;
            OnWsDisconnected();
        }

        // Reset WebSocket upgrade HTTP request and response
        Request.Clear();
        Response.Clear();

        // Clear WebSocket send/receive buffers
        WebSocket.ClearWsBuffers();

        // Initialize new WebSocket random nonce
        WebSocket.InitWsNonce();
    }

    protected override void OnReceived (byte[] buffer, long offset, long size)
    {
        // Check for WebSocket handshaked status
        if (WebSocket.WsHandshaked)
        {
            // Prepare receive frame
            WebSocket.PrepareReceiveFrame (buffer, offset, size);
            return;
        }

        base.OnReceived (buffer, offset, size);
    }

    protected override void OnReceivedRequestHeader (HttpRequest request)
    {
        // Check for WebSocket handshaked status
        if (WebSocket.WsHandshaked)
        {
            return;
        }

        // Try to perform WebSocket upgrade
        if (!WebSocket.PerformServerUpgrade (request, Response))
        {
            base.OnReceivedRequestHeader (request);
            return;
        }
    }

    protected override void OnReceivedRequest (HttpRequest request)
    {
        // Check for WebSocket handshaked status
        if (WebSocket.WsHandshaked)
        {
            // Prepare receive frame from the remaining request body
            var body = Request.Body;
            var data = Encoding.UTF8.GetBytes (body);
            WebSocket.PrepareReceiveFrame (data, 0, data.Length);
            return;
        }

        base.OnReceivedRequest (request);
    }

    protected override void OnReceivedRequestError (HttpRequest request, string error)
    {
        // Check for WebSocket handshaked status
        if (WebSocket.WsHandshaked)
        {
            OnError (new SocketError());
            return;
        }

        base.OnReceivedRequestError (request, error);
    }

    #endregion

    #region Web socket handlers

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    public virtual void OnWsConnecting
        (
            HttpRequest request
        )
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="response"></param>
    public virtual void OnWsConnected
        (
            HttpResponse response
        )
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <returns></returns>
    public virtual bool OnWsConnecting
        (
            HttpRequest request,
            HttpResponse response
        )
    {
        return true;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="request"></param>
    public virtual void OnWsConnected (HttpRequest request)
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void OnWsDisconnecting()
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    public virtual void OnWsDisconnected()
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    public virtual void OnWsReceived (byte[] buffer, long offset, long size)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    public virtual void OnWsClose (byte[] buffer, long offset, long size)
    {
        Close (1000);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    public virtual void OnWsPing (byte[] buffer, long offset, long size)
    {
        SendPongAsync (buffer, offset, size);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="size"></param>
    public virtual void OnWsPong (byte[] buffer, long offset, long size)
    {
        // пустое тело метода
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="error"></param>
    public virtual void OnWsError (string error)
    {
        OnError (SocketError.SocketError);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="error"></param>
    public virtual void OnWsError (SocketError error)
    {
        OnError (error);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="response"></param>
    public void SendUpgrade (HttpResponse response)
    {
        SendResponseAsync (response);
    }

    #endregion
}
