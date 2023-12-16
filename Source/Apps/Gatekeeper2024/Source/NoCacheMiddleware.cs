// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* NoCacheMiddleware.cs -- добавляет HTTP-заголовок Cache-Control: no-cache
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Устанавливает HTTP-заголовок <c>Cache-Control: no-cache</c>.
/// </summary>
[PublicAPI]
internal sealed class NoCacheMiddleware
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NoCacheMiddleware
        (
            RequestDelegate next
        )
    {
        _next = next;
    }

    #endregion

    #region Private members

    private readonly RequestDelegate _next;

    #endregion

    #region Public methods

    public async Task Invoke
        (
            HttpContext context
        )
    {
        context.Response.Headers.CacheControl = "no-cache";
        context.Response.Headers.Pragma = "no-cache";
        context.Response.Headers.Expires = "-1";

        await _next.Invoke (context);
    }

    #endregion
}
