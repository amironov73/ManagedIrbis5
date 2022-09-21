// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* SecureAccessMiddleware.cs -- ограничивает доступ к веб-приложению
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Web;

/// <summary>
/// Ограничивает доступ к веб-приложению,
/// организует белый список разрешенных адресов.
/// </summary>
public sealed class SecureAccessMiddleware
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="next">Следующий обработчик.</param>
    /// <param name="logger">Логгер.</param>
    /// <param name="safeList">Список разрешенных адресов,
    /// разделенных точкой с запятой.</param>
    public SecureAccessMiddleware
        (
            RequestDelegate next,
            ILogger<SecureAccessMiddleware> logger,
            string safeList
        )
    {
        _safeAddresses = new List<byte[]>();
        foreach (var one in safeList.Split (';'))
        {
            var address = IPAddress.Parse (one);
            _safeAddresses.Add (address.GetAddressBytes());
        }

        _next = next;
        _logger = logger;
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;
    private readonly RequestDelegate _next;
    private readonly List<byte[]> _safeAddresses;

    #endregion

    #region Public methods

    /// <summary>
    /// Обработка запроса.
    /// </summary>
    /// <param name="context">Контекст запроса.</param>
    public async Task Invoke
        (
            HttpContext context
        )
    {
        var remoteIp = context.Connection.RemoteIpAddress;
        if (remoteIp is null)
        {
            _logger.LogWarning ("Unknown remote address");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }

        _logger.LogInformation ("Request from remote IP address: {RemoteIp}", remoteIp);

        var bytes = remoteIp.GetAddressBytes();
        var allowed = false;
        foreach (var one in _safeAddresses)
        {
            if (bytes.SequenceEqual (one))
            {
                allowed = true;
                break;
            }
        }

        if (!allowed)
        {
            _logger.LogWarning ("This address is not allowed");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }

        await _next.Invoke (context);
    }

    #endregion
}
