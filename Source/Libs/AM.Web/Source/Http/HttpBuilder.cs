// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* HttpBuilder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

#endregion

#pragma warning disable SYSLIB0014 // obsolete types warning

#nullable enable

namespace AM.Web;

internal class HttpBuilder
    : IRequestBuilder
{
    #region Events

    /// <summary>
    ///
    /// </summary>
    public event EventHandler<HttpExceptionArgs>? BeforeExceptionLog;

    #endregion

    #region Properties

    /// <summary>
    ///
    /// </summary>
    public HttpSettings? Settings { get; }

    /// <summary>
    ///
    /// </summary>
    public HttpRequestMessage Message { get; }

    /// <summary>
    ///
    /// </summary>
    public bool LogErrors { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<HttpStatusCode> IgnoredResponseStatuses { get; set; } = Enumerable.Empty<HttpStatusCode>();

    /// <summary>
    ///
    /// </summary>
    public TimeSpan Timeout { get; set; }

    /// <summary>
    ///
    /// </summary>
    public IWebProxy? Proxy { get; set; }

    /// <summary>
    ///
    /// </summary>
    public bool BufferResponse { get; set; } = true;

    /// <summary>
    ///
    /// </summary>
    public IHttpClientPool? ClientPool { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public HttpBuilder
        (
            string uri,
            HttpSettings? settings,
            string callerName,
            string callerFile,
            int callerLine
        )
    {
        Message = new HttpRequestMessage
        {
            RequestUri = new Uri (uri, UriKind.RelativeOrAbsolute)
        };
        Settings = settings;
        Timeout = (settings ?? Http.DefaultSettings).DefaultTimeout;
        Proxy = (settings ?? Http.DefaultSettings).DefaultProxyFactory.Invoke();
        _callerName = callerName;
        _callerFile = callerFile;
        _callerLine = callerLine;
    }

    #endregion

    #region Private members

    private readonly string _callerName, _callerFile;
    private readonly int _callerLine;

    internal void AddExceptionData
        (
            Exception? ex
        )
    {
        if (ex is null)
        {
            return;
        }

        try
        {
            var servicePoint = ServicePointManager.FindServicePoint (Message.RequestUri!);
            if (servicePoint != null!)
            {
                ex.AddLoggedData ("ServicePoint.ConnectionLimit", servicePoint.ConnectionLimit)
                    .AddLoggedData ("ServicePoint.CurrentConnections", servicePoint.CurrentConnections)
                    .AddLoggedData ("ServicePointManager.CurrentConnections",
                        ServicePointManager.DefaultConnectionLimit);
            }
        }
        catch (Exception exception)
        {
            Debug.WriteLine (exception.Message);
        }

        ex.AddLoggedData ("Caller.Name", _callerName)
            .AddLoggedData ("Caller.File", _callerFile)
            .AddLoggedData ("Caller.Line", _callerLine.ToString());
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="args"></param>
    public void OnBeforeExceptionLog
        (
            HttpExceptionArgs args
        )
    {
        BeforeExceptionLog?.Invoke (this, args);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="handler"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IRequestBuilder<T> WithHandler<T>
        (
            Func<HttpResponseMessage, Task<T>> handler
        )
    {
        return new HttpBuilder<T> (this, handler);
    }

    #endregion
}

internal class HttpBuilder<T>
    : IRequestBuilder<T>
{
    public IRequestBuilder Inner { get; }
    public Func<HttpResponseMessage, Task<T>> Handler { get; }

    public HttpBuilder (HttpBuilder builder, Func<HttpResponseMessage, Task<T>> handler)
    {
        Inner = builder;
        Handler = handler;
    }
}
