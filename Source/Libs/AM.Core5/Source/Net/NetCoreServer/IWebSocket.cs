// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global

/* HttpsServer.cs -- HTTPS-сервер
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System.Net.Sockets;

#endregion

#nullable enable

namespace NetCoreServer;

/// <summary>
///
/// </summary>
public interface IWebSocket
{
    /// <summary>
    /// Handle WebSocket client connecting notification
    /// </summary>
    /// <remarks>Notification is called when WebSocket client
    /// is connecting to the server. You can handle the connection
    /// and change WebSocket upgrade HTTP request by providing your
    /// own headers.</remarks>
    /// <param name="request">WebSocket upgrade HTTP request</param>
    void OnWsConnecting
        (
            HttpRequest request
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle WebSocket client connected notification
    /// </summary>
    /// <param name="response">WebSocket upgrade HTTP response</param>
    void OnWsConnected
        (
            HttpResponse response
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle WebSocket server session validating notification
    /// </summary>
    /// <remarks>Notification is called when WebSocket client
    /// is connecting to the server. You can handle the connection
    /// and validate WebSocket upgrade HTTP request.</remarks>
    /// <param name="request">WebSocket upgrade HTTP request</param>
    /// <param name="response">WebSocket upgrade HTTP response</param>
    /// <returns>return 'true' if the WebSocket update request is valid,
    /// 'false' if the WebSocket update request is not valid</returns>
    bool OnWsConnecting
        (
            HttpRequest request,
            HttpResponse response
        )
    {
        return true;
    }

    /// <summary>
    /// Handle WebSocket server session connected notification
    /// </summary>
    /// <param name="request">WebSocket upgrade HTTP request</param>
    void OnWsConnected
        (
            HttpRequest request
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle WebSocket client disconnecting notification
    /// </summary>
    void OnWsDisconnecting()
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle WebSocket client disconnected notification
    /// </summary>
    void OnWsDisconnected()
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle WebSocket received notification
    /// </summary>
    /// <param name="buffer">Received buffer</param>
    /// <param name="offset">Received buffer offset</param>
    /// <param name="size">Received buffer size</param>
    void OnWsReceived
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle WebSocket client close notification
    /// </summary>
    /// <param name="buffer">Received buffer</param>
    /// <param name="offset">Received buffer offset</param>
    /// <param name="size">Received buffer size</param>
    void OnWsClose
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle WebSocket ping notification
    /// </summary>
    /// <param name="buffer">Received buffer</param>
    /// <param name="offset">Received buffer offset</param>
    /// <param name="size">Received buffer size</param>
    void OnWsPing
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle WebSocket pong notification
    /// </summary>
    /// <param name="buffer">Received buffer</param>
    /// <param name="offset">Received buffer offset</param>
    /// <param name="size">Received buffer size</param>
    void OnWsPong
        (
            byte[] buffer,
            long offset,
            long size
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle WebSocket error notification
    /// </summary>
    /// <param name="error">Error message</param>
    void OnWsError
        (
            string error
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Handle socket error notification
    /// </summary>
    /// <param name="error">Socket error</param>
    void OnWsError
        (
            SocketError error
        )
    {
        // пустое тело метода
    }

    /// <summary>
    /// Send WebSocket server upgrade response
    /// </summary>
    /// <param name="response">WebSocket upgrade HTTP response</param>
    void SendUpgrade
        (
            HttpResponse response
        )
    {
        // пустое тело метода
    }
}
