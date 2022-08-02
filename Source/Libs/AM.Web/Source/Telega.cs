// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Telega.cs -- простейшая отсылка сообщений через Telegram
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Net.Http;
using System.Web;

#endregion

#nullable enable

namespace AM.Web;

/// <summary>
/// Простейшая отсылка сообщений через Telegram.
/// </summary>
/// <example>
/// <code>
/// Telega.BotId = "12345"; // идентификатор бота
/// Telega.ChatId = "54321" // идентификатор пользователя
/// ...
/// Telega.SendMessage ("Произошло нечто неописуемое");
/// ...
/// </code>
/// <para>Главное, не вставлять эту строчку в самый глубокий цикл.</para>
/// </example>
public static class Telega
{
    #region Properties

    /// <summary>
    /// Telegram URI.
    /// </summary>
    public static string Api { get; set; } = "https://api.telegram.org/";

    /// <summary>
    /// Идентификатор бота, от имени которого будут посылаться сообщения.
    /// </summary>
    public static string? BotId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, которому будут посылаться сообщения.
    /// </summary>
    public static string? ChatId { get; set; }

    #endregion

    #region Private members

    private static HttpClient? _client;

    #endregion

    #region Public methods

    /// <summary>
    /// Отсылка сообщения.
    /// </summary>
    public static void SendMessage
        (
            string text
        )
    {
        if (string.IsNullOrEmpty (text)
            || string.IsNullOrEmpty (BotId)
            || string.IsNullOrEmpty (ChatId)
            || string.IsNullOrEmpty (Api))
        {
            return;
        }

        _client ??= new HttpClient();

        var request = new HttpRequestMessage
            (
                HttpMethod.Get,
                Api + BotId + "/sendMessage?chat_id" + ChatId + "&text="
                + HttpUtility.UrlEncode (text)
            );

        try
        {
            _client.Send (request);
        }
        catch
        {
            // ignored
        }
    }

    #endregion
}
