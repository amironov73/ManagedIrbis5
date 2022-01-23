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
public class HttpClientException : Exception
{
    /// <summary>
    /// The status code, if known, for this exception.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// The requested URI, if known, for this exception.
    /// </summary>
    public Uri Uri { get; }

    internal HttpClientException() { }
    internal HttpClientException(string message) : base(message) { }
    internal HttpClientException(string message, Exception innerException) : base(message, innerException) { }
    internal HttpClientException(SerializationInfo info, StreamingContext context) : base(info, context) { }

    internal HttpClientException(string message, HttpStatusCode statusCode, Uri uri) : base(message)
    {
        StatusCode = statusCode;
        Uri = uri;
    }

    internal HttpClientException(string message, Uri uri) : base(message)
    {
        Uri = uri;
    }

    internal HttpClientException(string message, Uri uri, Exception innerException) : base(message, innerException)
    {
        Uri = uri;
    }
}
