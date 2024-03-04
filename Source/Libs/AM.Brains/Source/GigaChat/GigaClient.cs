// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* GigaClient.cs - клиент для GigaChat
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using JetBrains.Annotations;

using RestSharp;

#endregion

namespace AM.Brains.GigaChat;

/// <summary>
/// Клиент для GigaChat.
/// </summary>
[PublicAPI]
public sealed class GigaClient
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public GigaClient
        (
            string clientId,
            string clientSecret,
            string? baseUrl = null
        )
    {
        Sure.NotNullNorEmpty (clientId);
        Sure.NotNullNorEmpty (clientSecret);

        _baseUrl = baseUrl ?? "https://gigachat.devices.sberbank.ru/api/v1";
        _clientId = clientId;
        _clientSecret = clientSecret;
    }

    #endregion

    #region Private members

    private readonly string _baseUrl;

    private readonly string _clientId;

    private readonly string _clientSecret;

    private string? _accessToken;

    private DateTime _expiresAt;

    private async Task GetAccessToken()
    {
        var options = new RestClientOptions
        {
            MaxTimeout = -1,
        };
        var client = new RestClient (options);
        var request = new RestRequest ("https://ngw.devices.sberbank.ru:9443/api/v2/oauth", Method.Post);
        request.AddHeader (KnownHeaders.ContentType, "application/x-www-form-urlencoded");
        request.AddHeader (KnownHeaders.Accept, "application/json");
        request.AddHeader ("RqUID", Guid.NewGuid().ToString ("D"));

        var authorization = $"{_clientId}.{_clientSecret}";
        authorization = "Basic " + Convert.ToBase64String (Encoding.ASCII.GetBytes (authorization));
        request.AddHeader (KnownHeaders.Authorization, authorization);
        request.AddParameter("scope", "GIGACHAT_API_PERS");

        var response = await client.ExecuteAsync<TokenInfo> (request);
        _accessToken = response.Data?.AccessToken;
        if (response.Data is not null)
        {
            _expiresAt = new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds (response.Data.ExpiresAt).ToLocalTime();
        }
    }

    private bool IsAccessTokenExpired() => string.IsNullOrEmpty (_accessToken)
        || _expiresAt < DateTime.Now;

    private async Task GetAccessTokenIfExpired()
    {
        if (IsAccessTokenExpired())
        {
            await GetAccessToken();
        }

        if (IsAccessTokenExpired())
        {
            throw new ApplicationException();
        }
    }

    #endregion
}
