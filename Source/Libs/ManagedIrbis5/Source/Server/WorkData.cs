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

/* WorkData.cs -- рабочие данные для серверного обработчика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using ManagedIrbis.Server.Commands;
using ManagedIrbis.Server.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Рабочие данные, необходимые серверному обработчику
    /// для выполнения клиентского запроса.
    /// </summary>
    public sealed class WorkData
    {
        #region Properties

        /// <summary>
        /// Команда, выполняемая в настоящее время по запросу клиента.
        /// </summary>
        public ServerCommand? Command { get; set; }

        /// <summary>
        /// Контекст для выполнения запроса.
        /// </summary>
        public ServerContext? Context { get; set; }

        /// <summary>
        /// Серверный движок.
        /// </summary>
        public ServerEngine? Engine { get; set; }

        /// <summary>
        /// Ответ сервера.
        /// </summary>
        public ServerResponse? Response { get; set; }

        /// <summary>
        /// Разобранный пользовательский запрос.
        /// </summary>
        public ClientRequest? Request { get; set; }

        /// <summary>
        /// Сокет, обслуживающий подключенного клиента.
        /// </summary>
        public IAsyncServerSocket? Socket { get; set; }

        /// <summary>
        /// Текущая задача.
        /// </summary>
        public Task? Task { get; set; }

        /// <summary>
        /// Информация о пользователе системы.
        /// </summary>
        public UserInfo? User { get; set; }

        /// <summary>
        /// Рабочий поток.
        /// </summary>
        public ServerWorker? Worker { get; set; }

        /// <summary>
        /// Момент, когда началась обработка клиентского запроса.
        /// </summary>
        public DateTime Started { get; set; }

        #endregion

    } // class WorkData

} // namespace ManagedIrbis.Server
