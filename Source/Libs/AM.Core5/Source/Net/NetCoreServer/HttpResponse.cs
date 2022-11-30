// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* HttpResponse.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace NetCoreServer;

/// <summary>
/// HTTP response is used to create or process parameters of HTTP protocol response(status, headers, etc).
/// </summary>
/// <remarks>Not thread-safe.</remarks>
public class HttpResponse
{
    static HttpResponse()
    {
        _mimeTable = new Dictionary<string, string>
        {
            // Base content types
            { ".html", "text/html" },
            { ".css", "text/css" },
            { ".js", "text/javascript" },
            { ".vue", "text/html" },
            { ".xml", "text/xml" },

            // Application content types
            { ".atom", "application/atom+xml" },
            { ".fastsoap", "application/fastsoap" },
            { ".gzip", "application/gzip" },
            { ".json", "application/json" },
            { ".map", "application/json" },
            { ".pdf", "application/pdf" },
            { ".ps", "application/postscript" },
            { ".soap", "application/soap+xml" },
            { ".sql", "application/sql" },
            { ".xslt", "application/xslt+xml" },
            { ".zip", "application/zip" },
            { ".zlib", "application/zlib" },

            // Audio content types
            { ".aac", "audio/aac" },
            { ".ac3", "audio/ac3" },
            { ".mp3", "audio/mpeg" },
            { ".ogg", "audio/ogg" },

            // Font content types
            { ".ttf", "font/ttf" },

            // Image content types
            { ".bmp", "image/bmp" },
            { ".emf", "image/emf" },
            { ".gif", "image/gif" },
            { ".jpg", "image/jpeg" },
            { ".jpm", "image/jpm" },
            { ".jpx", "image/jpx" },
            { ".jrx", "image/jrx" },
            { ".png", "image/png" },
            { ".svg", "image/svg+xml" },
            { ".tiff", "image/tiff" },
            { ".wmf", "image/wmf" },

            // Message content types
            { ".http", "message/http" },
            { ".s-http", "message/s-http" },

            // Model content types
            { ".mesh", "model/mesh" },
            { ".vrml", "model/vrml" },

            // Text content types
            { ".csv", "text/csv" },
            { ".plain", "text/plain" },
            { ".richtext", "text/richtext" },
            { ".rtf", "text/rtf" },
            { ".rtx", "text/rtx" },
            { ".sgml", "text/sgml" },
            { ".strings", "text/strings" },
            { ".url", "text/uri-list" },

            // Video content types
            { ".H264", "video/H264" },
            { ".H265", "video/H265" },
            { ".mp4", "video/mp4" },
            { ".mpeg", "video/mpeg" },
            { ".raw", "video/raw" }
        };
    }

    /// <summary>
    /// Initialize an empty HTTP response
    /// </summary>
    public HttpResponse()
    {
        Clear();
    }

    /// <summary>
    /// Initialize a new HTTP response with a given status and protocol
    /// </summary>
    /// <param name="status">HTTP status</param>
    /// <param name="protocol">Protocol version (default is "HTTP/1.1")</param>
    public HttpResponse (int status, string protocol = "HTTP/1.1")
    {
        SetBegin (status, protocol);
    }

    /// <summary>
    /// Initialize a new HTTP response with a given status, status phrase and protocol
    /// </summary>
    /// <param name="status">HTTP status</param>
    /// <param name="statusPhrase">HTTP status phrase</param>
    /// <param name="protocol">Protocol version</param>
    public HttpResponse (int status, string statusPhrase, string protocol)
    {
        SetBegin (status, statusPhrase, protocol);
    }

    /// <summary>
    /// Is the HTTP response empty?
    /// </summary>
    public bool IsEmpty => (Cache.Size > 0);

    /// <summary>
    /// Is the HTTP response error flag set?
    /// </summary>
    public bool IsErrorSet { get; private set; }

    /// <summary>
    /// Get the HTTP response status
    /// </summary>
    public int Status { get; private set; }

    /// <summary>
    /// Get the HTTP response status phrase
    /// </summary>
    public string StatusPhrase => _statusPhrase.ThrowIfNull();

    /// <summary>
    /// Get the HTTP response protocol version
    /// </summary>
    public string Protocol => _protocol.ThrowIfNull();

    /// <summary>
    /// Get the HTTP response headers count
    /// </summary>
    public long Headers => _headers.Count;

    /// <summary>
    /// Get the HTTP response header by index
    /// </summary>
    public (string, string) Header (int i)
    {
        Debug.Assert ((i < _headers.Count), "Index out of bounds!");
        if (i >= _headers.Count)
            return ("", "");

        return _headers[i];
    }

    /// <summary>
    /// Get the HTTP response body as string
    /// </summary>
    public string Body => Cache.ExtractString (_bodyIndex, _bodySize);

    /// <summary>
    /// Get the HTTP request body as byte array
    /// </summary>
    public byte[] BodyBytes => Cache.Data[_bodyIndex..(_bodyIndex + _bodySize)];

    /// <summary>
    /// Get the HTTP request body as read-only byte span
    /// </summary>
    public ReadOnlySpan<byte> BodySpan => new ReadOnlySpan<byte> (Cache.Data, _bodyIndex, _bodySize);

    /// <summary>
    /// Get the HTTP response body length
    /// </summary>
    public long BodyLength => _bodyLength;

    /// <summary>
    /// Get the HTTP response cache content
    /// </summary>
    public Buffer Cache { get; } = new ();

    /// <summary>
    /// Get string from the current HTTP response
    /// </summary>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.AppendLine ($"Status: {Status}");
        builder.AppendLine ($"Status phrase: {StatusPhrase}");
        builder.AppendLine ($"Protocol: {Protocol}");
        builder.AppendLine ($"Headers: {Headers}");
        for (var i = 0; i < Headers; i++)
        {
            var header = Header (i);
            builder.AppendLine ($"{header.Item1} : {header.Item2}");
        }

        builder.AppendLine ($"Body: {BodyLength}");
        builder.AppendLine (Body);

        return builder.ReturnShared();
    }

    /// <summary>
    /// Clear the HTTP response cache
    /// </summary>
    public HttpResponse Clear()
    {
        IsErrorSet = false;
        Status = 0;
        _statusPhrase = "";
        _protocol = "";
        _headers.Clear();
        _bodyIndex = 0;
        _bodySize = 0;
        _bodyLength = 0;
        _bodyLengthProvided = false;

        Cache.Clear();
        _cacheSize = 0;
        return this;
    }

    /// <summary>
    /// Set the HTTP response begin with a given status and protocol
    /// </summary>
    /// <param name="status">HTTP status</param>
    /// <param name="protocol">Protocol version (default is "HTTP/1.1")</param>
    public HttpResponse SetBegin (int status, string protocol = "HTTP/1.1")
    {
        string statusPhrase = status switch
        {
            100 => "Continue",
            101 => "Switching Protocols",
            102 => "Processing",
            103 => "Early Hints",

            200 => "OK",
            201 => "Created",
            202 => "Accepted",
            203 => "Non-Authoritative Information",
            204 => "No Content",
            205 => "Reset Content",
            206 => "Partial Content",
            207 => "Multi-Status",
            208 => "Already Reported",
            226 => "IM Used",

            300 => "Multiple Choices",
            301 => "Moved Permanently",
            302 => "Found",
            303 => "See Other",
            304 => "Not Modified",
            305 => "Use Proxy",
            306 => "Switch Proxy",
            307 => "Temporary Redirect",
            308 => "Permanent Redirect",

            400 => "Bad Request",
            401 => "Unauthorized",
            402 => "Payment Required",
            403 => "Forbidden",
            404 => "Not Found",
            405 => "Method Not Allowed",
            406 => "Not Acceptable",
            407 => "Proxy Authentication Required",
            408 => "Request Timeout",
            409 => "Conflict",
            410 => "Gone",
            411 => "Length Required",
            412 => "Precondition Failed",
            413 => "Payload Too Large",
            414 => "URI Too Long",
            415 => "Unsupported Media Type",
            416 => "Range Not Satisfiable",
            417 => "Expectation Failed",
            421 => "Misdirected Request",
            422 => "Unprocessable Entity",
            423 => "Locked",
            424 => "Failed Dependency",
            425 => "Too Early",
            426 => "Upgrade Required",
            427 => "Unassigned",
            428 => "Precondition Required",
            429 => "Too Many Requests",
            431 => "Request Header Fields Too Large",
            451 => "Unavailable For Legal Reasons",

            500 => "Internal Server Error",
            501 => "Not Implemented",
            502 => "Bad Gateway",
            503 => "Service Unavailable",
            504 => "Gateway Timeout",
            505 => "HTTP Version Not Supported",
            506 => "Variant Also Negotiates",
            507 => "Insufficient Storage",
            508 => "Loop Detected",
            510 => "Not Extended",
            511 => "Network Authentication Required",
            _ => "Unknown"
        };

        SetBegin (status, statusPhrase, protocol);
        return this;
    }

    /// <summary>
    /// Set the HTTP response begin with a given status, status phrase and protocol
    /// </summary>
    /// <param name="status">HTTP status</param>
    /// <param name="statusPhrase"> HTTP status phrase</param>
    /// <param name="protocol">Protocol version</param>
    public HttpResponse SetBegin (int status, string statusPhrase, string protocol)
    {
        // Clear the HTTP response cache
        Clear();

        // Append the HTTP response protocol version
        Cache.Append (protocol);
        _protocol = protocol;

        Cache.Append (" ");

        // Append the HTTP response status
        Cache.Append (status.ToString());
        Status = status;

        Cache.Append (" ");

        // Append the HTTP response status phrase
        Cache.Append (statusPhrase);
        _statusPhrase = statusPhrase;

        Cache.Append ("\r\n");
        return this;
    }

    /// <summary>
    /// Set the HTTP response content type
    /// </summary>
    /// <param name="extension">Content extension</param>
    public HttpResponse SetContentType (string extension)
    {
        // Try to lookup the content type in mime table
        if (_mimeTable.TryGetValue (extension, out var mime))
            return SetHeader ("Content-Type", mime);

        return this;
    }

    /// <summary>
    /// Set the HTTP response header
    /// </summary>
    /// <param name="key">Header key</param>
    /// <param name="value">Header value</param>
    public HttpResponse SetHeader (string key, string value)
    {
        // Append the HTTP response header's key
        Cache.Append (key);

        Cache.Append (": ");

        // Append the HTTP response header's value
        Cache.Append (value);

        Cache.Append ("\r\n");

        // Add the header to the corresponding collection
        _headers.Add ((key, value));
        return this;
    }

    /// <summary>
    /// Set the HTTP response cookie
    /// </summary>
    /// <param name="name">Cookie name</param>
    /// <param name="value">Cookie value</param>
    /// <param name="maxAge">Cookie age in seconds until it expires (default is 86400)</param>
    /// <param name="path">Cookie path (default is "")</param>
    /// <param name="domain">Cookie domain (default is "")</param>
    /// <param name="secure">Cookie secure flag (default is true)</param>
    /// <param name="strict">Cookie strict flag (default is true)</param>
    /// <param name="httpOnly">Cookie HTTP-only flag (default is true)</param>
    public HttpResponse SetCookie (string name, string value, int maxAge = 86400, string path = "",
        string domain = "", bool secure = true, bool strict = true, bool httpOnly = true)
    {
        var key = "Set-Cookie";

        // Append the HTTP response header's key
        Cache.Append (key);

        Cache.Append (": ");

        // Append the HTTP response header's value
        var valueIndex = (int)Cache.Size;

        // Append cookie
        Cache.Append (name);
        Cache.Append ("=");
        Cache.Append (value);
        Cache.Append ("; Max-Age=");
        Cache.Append (maxAge.ToString());
        if (!string.IsNullOrEmpty (domain))
        {
            Cache.Append ("; Domain=");
            Cache.Append (domain);
        }

        if (!string.IsNullOrEmpty (path))
        {
            Cache.Append ("; Path=");
            Cache.Append (path);
        }

        if (secure)
            Cache.Append ("; Secure");
        if (strict)
            Cache.Append ("; SameSite=Strict");
        if (httpOnly)
            Cache.Append ("; HttpOnly");

        var valueSize = (int)Cache.Size - valueIndex;

        var cookie = Cache.ExtractString (valueIndex, valueSize);

        Cache.Append ("\r\n");

        // Add the header to the corresponding collection
        _headers.Add ((key, cookie));
        return this;
    }

    /// <summary>
    /// Set the HTTP response body
    /// </summary>
    /// <param name="body">Body string content (default is "")</param>
    public HttpResponse SetBody (string body = "")
    {
        var length = string.IsNullOrEmpty (body) ? 0 : Encoding.UTF8.GetByteCount (body);

        // Append content length header
        SetHeader ("Content-Length", length.ToString());

        Cache.Append ("\r\n");

        var index = (int)Cache.Size;

        // Append the HTTP response body
        Cache.Append (body);
        _bodyIndex = index;
        _bodySize = length;
        _bodyLength = length;
        _bodyLengthProvided = true;
        return this;
    }

    /// <summary>
    /// Set the HTTP response body
    /// </summary>
    /// <param name="body">Body binary content</param>
    public HttpResponse SetBody (byte[] body)
    {
        // Append content length header
        SetHeader ("Content-Length", body.Length.ToString());

        Cache.Append ("\r\n");

        var index = (int)Cache.Size;

        // Append the HTTP response body
        Cache.Append (body);
        _bodyIndex = index;
        _bodySize = body.Length;
        _bodyLength = body.Length;
        _bodyLengthProvided = true;
        return this;
    }

    /// <summary>
    /// Set the HTTP response body
    /// </summary>
    /// <param name="body">Body buffer content</param>
    public HttpResponse SetBody (Buffer body)
    {
        // Append content length header
        SetHeader ("Content-Length", body.Size.ToString());

        Cache.Append ("\r\n");

        var index = (int)Cache.Size;

        // Append the HTTP response body
        Cache.Append (body.Data, body.Offset, body.Size);
        _bodyIndex = index;
        _bodySize = (int)body.Size;
        _bodyLength = (int)body.Size;
        _bodyLengthProvided = true;
        return this;
    }

    /// <summary>
    /// Set the HTTP response body length
    /// </summary>
    /// <param name="length">Body length</param>
    public HttpResponse SetBodyLength (int length)
    {
        // Append content length header
        SetHeader ("Content-Length", length.ToString());

        Cache.Append ("\r\n");

        var index = (int)Cache.Size;

        // Clear the HTTP response body
        _bodyIndex = index;
        _bodySize = 0;
        _bodyLength = length;
        _bodyLengthProvided = true;
        return this;
    }

    /// <summary>
    /// Make OK response
    /// </summary>
    /// <param name="status">OK status (default is 200 (OK))</param>
    public HttpResponse MakeOkResponse (int status = 200)
    {
        Clear();
        SetBegin (status);
        SetBody();
        return this;
    }

    /// <summary>
    /// Make ERROR response
    /// </summary>
    /// <param name="content">Error content (default is "")</param>
    /// <param name="contentType">Error content type (default is "text/plain; charset=UTF-8")</param>
    public HttpResponse MakeErrorResponse (string content = "", string contentType = "text/plain; charset=UTF-8")
    {
        return MakeErrorResponse (500, content, contentType);
    }

    /// <summary>
    /// Make ERROR response
    /// </summary>
    /// <param name="status">Error status</param>
    /// <param name="content">Error content (default is "")</param>
    /// <param name="contentType">Error content type (default is "text/plain; charset=UTF-8")</param>
    public HttpResponse MakeErrorResponse (int status, string content = "",
        string contentType = "text/plain; charset=UTF-8")
    {
        Clear();
        SetBegin (status);
        if (!string.IsNullOrEmpty (contentType))
            SetHeader ("Content-Type", contentType);
        SetBody (content);
        return this;
    }

    /// <summary>
    /// Make HEAD response
    /// </summary>
    public HttpResponse MakeHeadResponse()
    {
        Clear();
        SetBegin (200);
        SetBody();
        return this;
    }

    /// <summary>
    /// Make GET response
    /// </summary>
    /// <param name="content">String content (default is "")</param>
    /// <param name="contentType">Content type (default is "text/plain; charset=UTF-8")</param>
    public HttpResponse MakeGetResponse (string content = "", string contentType = "text/plain; charset=UTF-8")
    {
        Clear();
        SetBegin (200);
        if (!string.IsNullOrEmpty (contentType))
            SetHeader ("Content-Type", contentType);
        SetBody (content);
        return this;
    }

    /// <summary>
    /// Make GET response
    /// </summary>
    /// <param name="content">String content</param>
    /// <param name="contentType">Content type (default is "")</param>
    public HttpResponse MakeGetResponse (byte[] content, string contentType = "")
    {
        Clear();
        SetBegin (200);
        if (!string.IsNullOrEmpty (contentType))
            SetHeader ("Content-Type", contentType);
        SetBody (content);
        return this;
    }

    /// <summary>
    /// Make GET response
    /// </summary>
    /// <param name="content">String content</param>
    /// <param name="contentType">Content type (default is "")</param>
    public HttpResponse MakeGetResponse (Buffer content, string contentType = "")
    {
        Clear();
        SetBegin (200);
        if (!string.IsNullOrEmpty (contentType))
            SetHeader ("Content-Type", contentType);
        SetBody (content);
        return this;
    }

    /// <summary>
    /// Make OPTIONS response
    /// </summary>
    /// <param name="allow">Allow methods (default is "HEAD,GET,POST,PUT,DELETE,OPTIONS,TRACE")</param>
    public HttpResponse MakeOptionsResponse (string allow = "HEAD,GET,POST,PUT,DELETE,OPTIONS,TRACE")
    {
        Clear();
        SetBegin (200);
        SetHeader ("Allow", allow);
        SetBody();
        return this;
    }

    /// <summary>
    /// Make TRACE response
    /// </summary>
    /// <param name="request">Request string content</param>
    public HttpResponse MakeTraceResponse (string request)
    {
        Clear();
        SetBegin (200);
        SetHeader ("Content-Type", "message/http");
        SetBody (request);
        return this;
    }

    /// <summary>
    /// Make TRACE response
    /// </summary>
    /// <param name="request">Request binary content</param>
    public HttpResponse MakeTraceResponse (byte[] request)
    {
        Clear();
        SetBegin (200);
        SetHeader ("Content-Type", "message/http");
        SetBody (request);
        return this;
    }

    /// <summary>
    /// Make TRACE response
    /// </summary>
    /// <param name="request">Request buffer content</param>
    public HttpResponse MakeTraceResponse (Buffer request)
    {
        Clear();
        SetBegin (200);
        SetHeader ("Content-Type", "message/http");
        SetBody (request);
        return this;
    }

    // HTTP response status phrase
    private string? _statusPhrase;

    // HTTP response protocol
    private string? _protocol;

    // HTTP response headers
    private readonly List<(string, string)> _headers = new ();

    // HTTP response body
    private int _bodyIndex;
    private int _bodySize;
    private int _bodyLength;
    private bool _bodyLengthProvided;

    // HTTP response cache
    private int _cacheSize;

    // HTTP response mime table
    private static readonly Dictionary<string, string> _mimeTable;

    // Is pending parts of HTTP response
    internal bool IsPendingHeader()
    {
        return (!IsErrorSet && (_bodyIndex == 0));
    }

    internal bool IsPendingBody()
    {
        return (!IsErrorSet && (_bodyIndex > 0) && (_bodySize > 0));
    }

    // Receive parts of HTTP response
    internal bool ReceiveHeader (byte[] buffer, int offset, int size)
    {
        // Update the request cache
        Cache.Append (buffer, offset, size);

        // Try to seek for HTTP header separator
        for (var i = _cacheSize; i < (int)Cache.Size; i++)
        {
            // Check for the request cache out of bounds
            if ((i + 3) >= (int)Cache.Size)
                break;

            // Check for the header separator
            if ((Cache[i + 0] == '\r') && (Cache[i + 1] == '\n') && (Cache[i + 2] == '\r') &&
                (Cache[i + 3] == '\n'))
            {
                var index = 0;

                // Set the error flag for a while...
                IsErrorSet = true;

                // Parse protocol version
                var protocolIndex = index;
                var protocolSize = 0;
                while (Cache[index] != ' ')
                {
                    protocolSize++;
                    index++;
                    if (index >= (int)Cache.Size)
                        return false;
                }

                index++;
                if ((index >= (int)Cache.Size))
                    return false;
                _protocol = Cache.ExtractString (protocolIndex, protocolSize);

                // Parse status code
                var statusIndex = index;
                var statusSize = 0;
                while (Cache[index] != ' ')
                {
                    if ((Cache[index] < '0') || (Cache[index] > '9'))
                        return false;
                    statusSize++;
                    index++;
                    if (index >= (int)Cache.Size)
                        return false;
                }

                Status = 0;
                for (var j = statusIndex; j < (statusIndex + statusSize); j++)
                {
                    Status *= 10;
                    Status += Cache[j] - '0';
                }

                index++;
                if (index >= (int)Cache.Size)
                    return false;

                // Parse status phrase
                var statusPhraseIndex = index;
                var statusPhraseSize = 0;
                while (Cache[index] != '\r')
                {
                    statusPhraseSize++;
                    index++;
                    if (index >= (int)Cache.Size)
                        return false;
                }

                index++;
                if ((index >= (int)Cache.Size) || (Cache[index] != '\n'))
                    return false;
                index++;
                if (index >= (int)Cache.Size)
                    return false;
                _statusPhrase = Cache.ExtractString (statusPhraseIndex, statusPhraseSize);

                // Parse headers
                while ((index < (int)Cache.Size) && (index < i))
                {
                    // Parse header name
                    var headerNameIndex = index;
                    var headerNameSize = 0;
                    while (Cache[index] != ':')
                    {
                        headerNameSize++;
                        index++;
                        if (index >= i)
                            break;
                        if (index >= (int)Cache.Size)
                            return false;
                    }

                    index++;
                    if (index >= i)
                        break;
                    if (index >= (int)Cache.Size)
                        return false;

                    // Skip all prefix space characters
                    while (char.IsWhiteSpace ((char)Cache[index]))
                    {
                        index++;
                        if (index >= i)
                            break;
                        if (index >= (int)Cache.Size)
                            return false;
                    }

                    // Parse header value
                    var headerValueIndex = index;
                    var headerValueSize = 0;
                    while (Cache[index] != '\r')
                    {
                        headerValueSize++;
                        index++;
                        if (index >= i)
                            break;
                        if (index >= (int)Cache.Size)
                            return false;
                    }

                    index++;
                    if ((index >= (int)Cache.Size) || (Cache[index] != '\n'))
                        return false;
                    index++;
                    if (index >= (int)Cache.Size)
                        return false;

                    // Validate header name and value (sometimes value can be empty)
                    if (headerNameSize == 0)
                        return false;

                    // Add a new header
                    var headerName = Cache.ExtractString (headerNameIndex, headerNameSize);
                    var headerValue = Cache.ExtractString (headerValueIndex, headerValueSize);
                    _headers.Add ((headerName, headerValue));

                    // Try to find the body content length
                    if (string.Compare (headerName, "Content-Length", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        _bodyLength = 0;
                        for (var j = headerValueIndex; j < (headerValueIndex + headerValueSize); j++)
                        {
                            if ((Cache[j] < '0') || (Cache[j] > '9'))
                                return false;
                            _bodyLength *= 10;
                            _bodyLength += Cache[j] - '0';
                            _bodyLengthProvided = true;
                        }
                    }
                }

                // Reset the error flag
                IsErrorSet = false;

                // Update the body index and size
                _bodyIndex = i + 4;
                _bodySize = (int)Cache.Size - i - 4;

                // Update the parsed cache size
                _cacheSize = (int)Cache.Size;

                return true;
            }
        }

        // Update the parsed cache size
        _cacheSize = ((int)Cache.Size >= 3) ? ((int)Cache.Size - 3) : 0;

        return false;
    }

    internal bool ReceiveBody (byte[] buffer, int offset, int size)
    {
        // Update the request cache
        Cache.Append (buffer, offset, size);

        // Update the parsed cache size
        _cacheSize = (int)Cache.Size;

        // Update body size
        _bodySize += size;

        // Check if the body length was provided
        if (_bodyLengthProvided)
        {
            // Was the body fully received?
            if (_bodySize >= _bodyLength)
            {
                _bodySize = _bodyLength;
                return true;
            }
        }
        else
        {
            // Check the body content to find the response body end
            if (_bodySize >= 4)
            {
                var index = _bodyIndex + _bodySize - 4;

                // Was the body fully received?
                if ((Cache[index + 0] == '\r') && (Cache[index + 1] == '\n') && (Cache[index + 2] == '\r') &&
                    (Cache[index + 3] == '\n'))
                {
                    _bodyLength = _bodySize;
                    return true;
                }
            }
        }

        // Body was received partially...
        return false;
    }
}
