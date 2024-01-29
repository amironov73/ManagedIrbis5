// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

#endregion

namespace RestSharp.Authenticators.Digest;

/// <summary>
///     DigestAuthenticatorManager class.
/// </summary>
internal class DigestAuthenticatorManager
{
    private static readonly Version _assemblyVersion;

    private readonly Uri _host;

    private readonly string _password;

    private readonly int _timeout;
    private readonly ILogger _logger;

    private readonly string _username;

    /// <summary>
    ///     The cnonce that is generated randomly by the application.
    /// </summary>
    private string? _cnonce;

    /// <summary>
    ///     The nonce that is returned by the first digest request (without the data).
    /// </summary>
    private string? _nonce;

    /// <summary>
    ///     The qop that is returned by the first digest request (without the data).
    /// </summary>
    private string? _qop;

    /// <summary>
    ///     The Realm that is returned by the first digest request (without the data).
    /// </summary>
    private string? _realm;

    static DigestAuthenticatorManager()
    {
        _assemblyVersion = Assembly.GetAssembly(typeof(DigestAuthenticatorManager)).GetName().Version;
    }

    /// <summary>
    ///     Creates a new instance of <see cref="DigestAuthenticatorManager" /> class.
    /// </summary>
    /// <param name="host">The host.</param>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <param name="timeout">The timeout.</param>
    /// <param name="logger"></param>
    public DigestAuthenticatorManager(Uri host, string username, string password, int timeout, ILogger logger)
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
            throw new ArgumentOutOfRangeException(nameof(timeout));
        }

        _host = host ?? throw new ArgumentNullException(nameof(host));
        _username = username;
        _password = password;
        _timeout = timeout;
        _logger = logger;
    }

    /// <summary>
    ///     Gets the digest auth header.
    /// </summary>
    /// <param name="path">The request path.</param>
    /// <param name="method">The request method.</param>
    /// <param name="proxy">The request proxy.</param>
    public async Task GetDigestAuthHeader(
        string path,
        Method method,
        IWebProxy? proxy = default)
    {
        _logger.LogDebug("Initiating GetDigestAuthHeader");
        var uri = new Uri(_host, path);
        var request = new RestRequest(uri, method);
        request.AddOrUpdateHeader("Connection", "Keep-Alive");
        request.AddOrUpdateHeader("Accept", "*/*");
        request.AddOrUpdateHeader("User-Agent", $"RestSharp.Authenticators.Digest/{_assemblyVersion}");
        request.AddOrUpdateHeader("Accept-Encoding", "gzip, deflate, br");
        request.Timeout = _timeout;
        using var client = new RestClient(new RestClientOptions()
        {
            Proxy = proxy
        });
        var response = await client.ExecuteAsync(request).ConfigureAwait(false);
        GetDigestDataFromFailResponse(response);
        _logger.LogDebug("GetDigestAuthHeader completed");
    }

    /// <summary>
    ///     Gets the digest header.
    /// </summary>
    /// <param name="digestUri">The digest uri.</param>
    /// <param name="method">The method.</param>
    /// <returns>The digest header.</returns>
    public string GetDigestHeader(string digestUri, Method method)
    {
        var hash1 = GenerateMD5($"{_username}:{_realm}:{_password}");
        var hash2 = GenerateMD5($"{method.ToString().ToUpperInvariant()}:{digestUri}");
        var digestResponse =
            GenerateMD5($"{hash1}:{_nonce}:{DigestHeader.NONCE_COUNT:00000000}:{_cnonce}:{_qop}:{hash2}");
        return $"Digest username=\"{_username}\"," +
               $"realm=\"{_realm}\"," +
               $"nonce=\"{_nonce}\"," +
               $"uri=\"{digestUri}\"," +
               "algorithm=MD5," +
               $"response=\"{digestResponse}\"," +
               $"qop={_qop}," +
               $"nc={DigestHeader.NONCE_COUNT:00000000}," +
               $"cnonce=\"{_cnonce}\"";
    }

    /// <summary>
    ///     Generate the MD5 Hash.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>The MD5.</returns>
    private static string GenerateMD5(string input)
    {
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hash = MD5.Create().ComputeHash(inputBytes);
        var stringBuilder = new StringBuilder();
        hash.ToList().ForEach(b => stringBuilder.Append(b.ToString("x2")));
        return stringBuilder.ToString();
    }

    private void GetDigestDataFromFailResponse(RestResponse response)
    {
        if (response.IsSuccessful)
        {
            _logger.LogInformation("Request is already authenticated");
            return;
        }

        if (response is not { StatusCode: HttpStatusCode.Unauthorized })
        {
            _logger.LogWarning("Response status code not supported for fail authentication. {StatusCode}", response.StatusCode);
            throw new Exception(response.ErrorMessage);
        }

        var header = response
            .Headers?
            .First(h => string.Equals(h.Name, "WWW-Authenticate", StringComparison.OrdinalIgnoreCase))
            .Value?
            .ToString();

        if (string.IsNullOrWhiteSpace(header))
        {
            _logger.LogError("Any WWW-Authenticate header is not found");
            throw new AuthenticationException("WWW-Authenticate header is empty.");
        }

        var digestHeader = new DigestHeader(header!, _logger);

        _cnonce = new Random()
            .Next(123400, 9999999)
            .ToString(CultureInfo.InvariantCulture);

        _realm = digestHeader.Realm;
        _nonce = digestHeader.Nonce;
        _qop = digestHeader.Qop;
    }
}
