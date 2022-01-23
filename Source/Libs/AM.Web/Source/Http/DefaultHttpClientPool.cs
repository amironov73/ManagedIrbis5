﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* DefaultHttpClientPool.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

#endregion

#nullable enable

namespace AM.Web;

/// <summary>
/// The default implementation of <see cref="IHttpClientPool"/>.
/// </summary>
public class DefaultHttpClientPool
    : IHttpClientPool
{
    private readonly ConcurrentDictionary<HttpClientCacheKey, HttpClient> ClientPool = new ();
    private HttpSettings Settings { get; }

    /// <summary>
    /// Creates a new <see cref="DefaultHttpClientPool"/> based on the settings.
    /// </summary>
    /// <param name="settings">The settings to based this pool on.</param>
    public DefaultHttpClientPool (HttpSettings settings) => Settings = settings;

    /// <summary>
    /// Gets a <see cref="HttpClient"/> from the pool, based on the <see cref="IRequestBuilder"/>.
    /// </summary>
    /// <param name="builder">The builder to get a request from.</param>
    /// <returns>The found or created <see cref="HttpClient"/> from the pool.</returns>
    public HttpClient Get (IRequestBuilder builder) =>
        ClientPool.GetOrAdd (new HttpClientCacheKey (builder.Timeout, builder.Proxy), CreateHttpClient);

    private HttpClient CreateHttpClient (HttpClientCacheKey options)
    {
        var handler = new HttpClientHandler
        {
            UseCookies = false
        };
        var serverCertificateCustomValidationCallback = Settings?.ServerCertificateCustomValidationCallback;
        if (serverCertificateCustomValidationCallback != null)
        {
            handler.ServerCertificateCustomValidationCallback = serverCertificateCustomValidationCallback;
        }

        if (options.Proxy != null)
        {
            handler.UseProxy = true;
            handler.Proxy = options.Proxy;
        }

        if (handler.SupportsAutomaticDecompression)
        {
            handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
        }

        var client = new HttpClient (handler)
        {
            Timeout = options.Timeout,
            DefaultRequestHeaders =
            {
                AcceptEncoding =
                {
                    new StringWithQualityHeaderValue ("gzip"),
                    new StringWithQualityHeaderValue ("deflate")
                }
            }
        };

        if (!string.IsNullOrEmpty (Settings?.UserAgent))
        {
            client.DefaultRequestHeaders.Add ("User-Agent", Settings.UserAgent);
        }

        return client;
    }

    /// <summary>
    /// Clears the pool, causing all new <see cref="HttpClient"/>s to be created on the following calls.
    /// </summary>
    public void Clear() => ClientPool.Clear();

    private readonly struct HttpClientCacheKey
        : IEquatable<HttpClientCacheKey>
    {
        public TimeSpan Timeout { get; }
        public IWebProxy Proxy { get; }

        public HttpClientCacheKey (TimeSpan timeout, IWebProxy proxy)
        {
            Timeout = timeout;
            Proxy = proxy;
        }

        public override bool Equals (object? obj)
        {
            return obj is HttpClientCacheKey key && Equals (key);
        }

        public bool Equals (HttpClientCacheKey other)
        {
            return Timeout.Equals (other.Timeout)
                   && EqualityComparer<IWebProxy>.Default.Equals (Proxy, other.Proxy);
        }

        public static bool operator == (HttpClientCacheKey left, HttpClientCacheKey right)
            => left.Equals (right);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 647927907;
                hashCode = (hashCode * -1521134295) + EqualityComparer<TimeSpan>.Default.GetHashCode (Timeout);
                hashCode = (hashCode * -1521134295) + EqualityComparer<IWebProxy>.Default.GetHashCode (Proxy);
                return hashCode;
            }
        }

        public static bool operator != (HttpClientCacheKey left, HttpClientCacheKey right)
            => !(left == right);
    }
}
