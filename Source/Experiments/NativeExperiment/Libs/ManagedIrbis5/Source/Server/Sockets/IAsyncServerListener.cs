// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* IAsyncServerListener.cs -- интерфейс асинхронного серверного слушателя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Интерфейс асинхронного серверного слушателя.
    /// </summary>
    public interface IAsyncServerListener
        : IAsyncDisposable
    {

        /// <summary>
        /// Принятие клиентского подключения.
        /// </summary>
        Task<IAsyncServerSocket?> AcceptClientAsync();

        /// <summary>
        /// Локальный адрес.
        /// </summary>
        string GetLocalAddress();

        /// <summary>
        /// Начало прослушивания.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Окончание прослушивания.
        /// </summary>
        Task StopAsync();

    } // interface IAsyncServerListener

} // namespace ManagedIrbis.Server.Sockets
