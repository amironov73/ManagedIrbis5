// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* HttpClientException.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Net;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Web;

/// <summary>
/// An exception from an <see cref="Http"/> call.
/// </summary>
public class HttpClientException
    : Exception
{
    #region Properties

    /// <summary>
    /// The status code, if known, for this exception.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// The requested URI, if known, for this exception.
    /// </summary>
    public Uri? Uri { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal HttpClientException()
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal HttpClientException
        (
            string message
        )
        : base (message)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal HttpClientException
        (
            string message,
            Exception innerException
        )
        : base (message, innerException)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal HttpClientException
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal HttpClientException
        (
            string message,
            HttpStatusCode statusCode,
            Uri uri
        )
        : base (message)
    {
        StatusCode = statusCode;
        Uri = uri;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal HttpClientException
        (
            string message,
            Uri? uri
        )
        : base (message)
    {
        Uri = uri;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    internal HttpClientException
        (
            string message,
            Uri uri,
            Exception innerException
        )
        : base (message, innerException)
    {
        Uri = uri;
    }

    #endregion
}
