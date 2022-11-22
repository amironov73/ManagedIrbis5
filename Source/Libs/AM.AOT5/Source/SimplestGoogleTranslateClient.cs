// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SimplestGoogleTranslateClient.cs -- простейший клиент Google Translate
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using RestSharp;

#endregion

#nullable enable

namespace AM.AOT;

/// <summary>
/// Простейший клиент Google Translate. Не предназначен для
/// профессионального применения.
/// </summary>
public sealed class SimplestGoogleTranslateClient
{
    #region Constants

    /// <summary>
    /// Базовый URL.
    /// </summary>
    public const string BaseUrl = "https://translate.googleapis.com/translate_a/single";

    #endregion

    #region Public methods

    /// <summary>
    /// Перевод небольшого фрагмента текста.
    /// </summary>
    /// <param name="sourceLanguage">Двухсимвольный код исходного языка,
    /// например, <c>"en"</c>.</param>
    /// <param name="targetLanguage">Двухсимвольный код языка назначения,
    /// например, <c>"ru"</c>.</param>
    /// <param name="originalText">Текст, подлежащий переводу.</param>
    /// <returns>Результат перевода либо <c>null</c>,
    /// если что-то пошло не так.</returns>
    public string? Translate
        (
            string sourceLanguage,
            string targetLanguage,
            string originalText
        )
    {
        Sure.NotNullNorEmpty (sourceLanguage);
        Sure.NotNullNorEmpty (targetLanguage);
        Sure.NotNullNorEmpty (originalText);

        var client = new RestClient (BaseUrl);
        var request = new RestRequest (Method.GET)
            .AddParameter ("client", "gtx")
            .AddParameter ("sl", sourceLanguage)
            .AddParameter ("tl", targetLanguage)
            .AddParameter ("dt", "t")
            .AddParameter ("q", originalText);

        var response = client.Execute (request);
        if (!response.IsSuccessful)
        {
            return null;
        }

        var document = System.Text.Json.JsonDocument.Parse (response.Content);
        var result = document.RootElement[0][0][0].GetString();

        return result;
    }

    #endregion
}
