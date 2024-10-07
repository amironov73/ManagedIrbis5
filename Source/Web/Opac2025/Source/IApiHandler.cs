// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IApiHandler.cs -- интерфейс обработчика API-запросов
 * Ars Magna project, http://arsmagna.ru
 */

namespace Opac2025;

/// <summary>
/// Интерфейс обработчика API-запросов
/// </summary>
internal interface IApiHandler
{
    /// <summary>
    /// Регистрация обработчика.
    /// </summary>
    public void Register
        (
            RouteGroupBuilder api
        );
}
