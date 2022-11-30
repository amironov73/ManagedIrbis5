// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* HttpServer.cs -- HTTP-сервер
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;
using System.Net;
using System.IO;

#endregion

#nullable enable

namespace NetCoreServer;

/// <summary>
/// HTTP-сервер используется для создания HTTP-веб-сервера и связи
/// с клиентами по протоколу HTTP. Он позволяет получать запросы
/// GET, POST, PUT, DELETE и отправлять HTTP-ответы.
/// </summary>
/// <remarks>Thread-safe.</remarks>
public class HttpServer
    : TcpServer
{
    #region Properties

    /// <summary>
    /// Кеш для статичного содержимого.
    /// </summary>
    public FileCache Cache { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Создание HTTP-сервера с заданным IP-адресом и номером порта.
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="port">Port number</param>
    public HttpServer
        (
            IPAddress address,
            int port
        )
        : base (address, port)
    {
        Cache = new FileCache();
    }

    /// <summary>
    /// Создание HTTP-сервера с заданным IP-адресом и номером порта.
    /// </summary>
    /// <param name="address">IP address</param>
    /// <param name="port">Port number</param>
    public HttpServer
        (
            string address,
            int port
        )
        : base (address, port)
    {
        Cache = new FileCache();
    }

    /// <summary>
    /// Создание HTTP-сервера с заданной конечной точкой DNS.
    /// </summary>
    /// <param name="endpoint">DNS endpoint</param>
    public HttpServer
        (
            DnsEndPoint endpoint
        )
        : base (endpoint)
    {
        Cache = new FileCache();
    }

    /// <summary>
    /// Создание HTTP-сервера с заданной конечной точкой IP.
    /// </summary>
    /// <param name="endpoint">IP endpoint</param>
    public HttpServer
        (
            IPEndPoint endpoint
        )
        : base (endpoint)
    {
        Cache = new FileCache();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление в кеш статического контента.
    /// </summary>
    /// <param name="path">Static content path</param>
    /// <param name="prefix">Cache prefix (default is "/")</param>
    /// <param name="filter">Cache filter (default is "*.*")</param>
    /// <param name="timeout">Refresh cache timeout (default is 1 hour)</param>
    public void AddStaticContent
        (
            string path,
            string prefix = "/",
            string filter = "*.*",
            TimeSpan? timeout = null
        )
    {
        timeout ??= TimeSpan.FromHours (1);

        bool Handler (FileCache cache, string key, byte[] value, TimeSpan timespan)
        {
            var response = new HttpResponse();
            response.SetBegin (200);
            response.SetContentType (Path.GetExtension (key));
            response.SetHeader ("Cache-Control", $"max-age={timespan.Seconds}");
            response.SetBody (value);
            return cache.Add (key, response.Cache.Data, timespan);
        }

        Cache.InsertPath (path, prefix, filter, timeout.Value, Handler);
    }

    /// <summary>
    /// Удалить кеш статического контента.
    /// </summary>
    /// <param name="path">Static content path</param>
    public void RemoveStaticContent
        (
            string path
        )
    {
        Cache.RemovePath (path);
    }

    /// <summary>
    /// Очистка кеша статического контента.
    /// </summary>
    public void ClearStaticContent()
    {
        Cache.Clear();
    }

    #endregion

    /// <summary>
    /// Создание сессии.
    /// </summary>
    protected override TcpSession CreateSession()
    {
        return new HttpSession (this);
    }

    #region IDisposable implementation

    // Disposed flag.
    private bool _disposed;

    /// <summary>
    /// Очистка ресурсов.
    /// </summary>
    /// <param name="disposingManagedResources"></param>
    protected override void Dispose
        (
            bool disposingManagedResources
        )
    {
        if (!_disposed)
        {
            if (disposingManagedResources)
            {
                // Dispose managed resources here...
                Cache.Dispose();
            }

            // Dispose unmanaged resources here...

            // Set large fields to null here...

            // Mark as disposed.
            _disposed = true;
        }

        // Call Dispose in the base class.
        base.Dispose (disposingManagedResources);
    }

    // The derived class does not have a Finalize method
    // or a Dispose method without parameters because it inherits
    // them from the base class.

    #endregion
}
