// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

/* SigurHandler.cs -- обработчик запросов от Sigur
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json;

#endregion


namespace Gatekeeper2024;

/// <summary>
/// Обработчик запросов от Sigur.
/// </summary>
internal class SigurHandler
{
    #region Construction

    public SigurHandler
        (
            WebApplication application
        )
    {
        _application = application;
        _logger = _application.Logger;
    }

    #endregion

    #region Private members

    private readonly WebApplication _application;
    private readonly ILogger _logger;

    #endregion

    #region Public methods

    /// <summary>
    /// Обработчик POST-запроса.
    /// </summary>
    public async Task HandleRequest
        (
            HttpContext context
        )
    {
        var request = await JsonSerializer.DeserializeAsync<SigurRequest> (context.Request.Body);
        if (request is null)
        {
            _logger.LogError ("Can't parse the request");
            return;
        }

        request.Arrived = TimeProvider.System.GetLocalNow();

        var response = StandardResolution();

        LogRequestAndResponse (request, response);

        await context.Response.WriteAsJsonAsync (response);
    }

    #endregion

    #region Private members

    /// <summary>
    /// Стандартное разрешение проходить.
    /// </summary>
    private SigurResponse StandardResolution() => new()
    {
        Allow = true,
        Message = "Проходи, не задерживайся"
    };

    /// <summary>
    /// Логирование запроса и ответа на него.
    /// </summary>
    private void LogRequestAndResponse
        (
            SigurRequest request,
            SigurResponse response
        )
    {
        _logger.LogInformation ("Got auth request: {Request}, send {Response}", request, response);
    }

    #endregion
}
