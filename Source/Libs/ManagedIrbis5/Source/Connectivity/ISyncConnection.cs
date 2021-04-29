// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* ISyncConnection.cs -- интерфейс синхронного подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Интерфейс синхронного подключения.
    /// </summary>
    public interface ISyncConnection
        : ISyncProvider,
        IConnectionSettings,
        ISetLastError
    {
        /// <summary>
        /// Обращение к серверу ИРБИС64 синхронным образом.
        /// </summary>
        /// <param name="query">Запрос.</param>
        /// <returns>Возвращенный сервером ответ
        /// либо <c>null</c>, если произошла
        /// ошибка сетевого обмена.</returns>
        Response? ExecuteSync(SyncQuery query);

    } // interface ISyncConnection

} // namespace ManagedIrbis
