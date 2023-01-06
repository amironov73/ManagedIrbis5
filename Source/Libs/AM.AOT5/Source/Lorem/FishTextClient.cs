// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FishTextClient.cs -- клиент сайта fish-text.ru
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using RestSharp;

#endregion

#nullable enable

namespace AM.Text.Lorem;

/// <summary>
/// Клиент сайта fish-text.ru
/// </summary>
public sealed class FishTextClient
{
    #region Constants

    /// <summary>
    /// Базовый URL.
    /// </summary>
    public const string BaseUrl = "https://fish-text.ru/get";

    #endregion

    #region Public methods

    /// <summary>
    /// Получение фраз от генератора.
    /// </summary>
    /// <param name="amount">Количество фраз (не более 500).</param>
    /// <returns>Полученные фразы либо <c>null</c> при ошибке.</returns>
    public string? GetSentences
        (
            int amount
        )
    {
        Sure.InRange (amount, 1, 500);

        var options = new RestClientOptions (BaseUrl)
        {
            ThrowOnAnyError = true,
            MaxTimeout = 1_000,
        };
        var client = new RestClient (options);
        var request = new RestRequest()
            .AddParameter ("type", "sentence")
            .AddParameter ("number", amount.ToInvariantString())
            .AddParameter ("format", "json");

        var response = client.Get (request);
        if (!response.IsSuccessful)
        {
            return null;
        }

        var content = response.Content;
        if (string.IsNullOrEmpty (content))
        {
            return null;
        }

        var document = System.Text.Json.JsonDocument.Parse (content);
        var status = document.RootElement.GetProperty ("status").GetString();
        if (status != "success")
        {
            return null;
        }

        var result = document.RootElement.GetProperty ("text").GetString();

        return result;
    }

    #endregion
}
