// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

#endregion

namespace RestSharp.Authenticators.Digest;

/// <summary>
///     Digest middleware for <see cref="RestClient" />.
/// </summary>
internal class DigestAuthenticator : IAuthenticator
{
    private const int DEFAULT_TIMEOUT = 100000;
    private readonly string _password;
    private readonly ILogger _logger;

    private readonly string _username;
    private readonly int _timeout;

    /// <summary>
    ///     Creates a new instance of <see cref="DigestAuthenticator" /> class.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <param name="logger">The optional logger.</param>
    /// <param name="timeout">The request timeout.</param>
    public DigestAuthenticator(string username, string password, int timeout = DEFAULT_TIMEOUT, ILogger? logger = null)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(username));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
        }

        if (timeout <= 0)
        {
            throw new ArgumentException("Value cannot be less than or equal to zero.", nameof(timeout));
        }

        _username = username;
        _password = password;
        _timeout = timeout;
        _logger = logger ?? NullLogger.Instance;
    }

    /// <inheritdoc cref="IAuthenticator" />
    public async ValueTask Authenticate(IRestClient client, RestRequest request)
    {
        _logger.LogDebug("Initiate Digest authentication");
        var uri = client.BuildUri(request);
        var manager = new DigestAuthenticatorManager(client.BuildUri(new RestRequest()), _username, _password, _timeout, _logger);
        await manager.GetDigestAuthHeader(uri.PathAndQuery, request.Method,client.Options.Proxy).ConfigureAwait(false);
        var digestHeader = manager.GetDigestHeader(uri.PathAndQuery, request.Method);
        request.AddOrUpdateHeader("Connection", "Keep-Alive");
        request.AddOrUpdateHeader(KnownHeaders.Authorization, digestHeader);
        _logger.LogDebug("Digest authentication completed");
    }
}
