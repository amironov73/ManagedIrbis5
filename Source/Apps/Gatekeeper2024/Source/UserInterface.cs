// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* UserInterface.cs -- отображение пользовательского интерфейса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Отображение пользовательского интерфейса.
/// </summary>
internal sealed class UserInterface
{
    #region Construciton

    public UserInterface
        (
            WebApplication application
        )
    {
        _application = application;
        _logger = application.Logger;
    }

    #endregion

    #region Private members

    private readonly WebApplication _application;
    private readonly ILogger _logger;
    private string? _pageBody;

    #endregion

    #region Public methods

    /// <summary>
    /// Обработчик Get-запроса.
    /// </summary>
    public async Task HandleRequest
        (
            HttpContext context
        )
    {
        _logger.LogInformation ("Got UI request from {Host}", context.Request.Host);

        _pageBody ??= await File.ReadAllTextAsync ("ui.html");
        var body = _pageBody.Replace ("{message}", GlobalState.Instance.Message);

        await WriteHtml (context, body);
    }

    #endregion

    #region Private members

    private async Task WriteHtml
        (
            HttpContext context,
            string body
        )
    {
        var response = context.Response;
        response.Headers.ContentType = "text/html; charset=utf-8";
        await response.WriteAsync (body);
    }

    #endregion
}
