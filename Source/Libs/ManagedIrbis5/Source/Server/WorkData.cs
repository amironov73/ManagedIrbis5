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

        // /// <summary>
        // /// Command.
        // /// </summary>
        // public ServerCommand Command { get; set; }
        //
        // /// <summary>
        // /// Context.
        // /// </summary>
        // public ServerContext Context { get; set; }
        //
        // /// <summary>
        // /// Engine
        // /// </summary>
        // public IrbisServerEngine Engine { get; set; }
        //
        // /// <summary>
        // /// Response.
        // /// </summary>
        // public ServerResponse Response { get; set; }

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

        // /// <summary>
        // /// User info.
        // /// </summary>
        // public UserInfo User { get; set; }
        //
        // /// <summary>
        // /// Worker.
        // /// </summary>
        // public ServerWorker Worker { get; set; }

        /// <summary>
        /// Когда началась обработка клиентского запроса.
        /// </summary>
        public DateTime Started { get; set; }

        #endregion

    } // class WorkData

} // namespace ManagedIrbis.Server
