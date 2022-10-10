// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedParameter.Local

/* IAsyncConnection.cs -- интерфейс асинхронного подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Интерфейс асинхронного подключения.
    /// </summary>
    public interface IAsyncConnection
        : IAsyncProvider,
        IConnectionSettings,
        ISetLastError
    {
        /// <summary>
        /// Обращение к серверу ИРБИС64 асинхронным образом.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <returns>Возвращенный сервером ответ
        /// либо <c>null</c>, если произошла
        /// ошибка сетевого обмена.</returns>
        Task<Response?> ExecuteAsync(AsyncQuery query);

    } // interface IAsyncConnection

} // namespace ManagedIrbis
