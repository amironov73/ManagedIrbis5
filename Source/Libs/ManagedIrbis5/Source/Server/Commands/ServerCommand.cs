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

/* ServerCommand.cs -- абстрактная команда, выполняемая сервером
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Commands
{
    /// <summary>
    /// Абстрактная команда, выполняемая сервером по запросу клиента.
    /// </summary>
    public abstract class ServerCommand
    {
        #region Properties

        /// <summary>
        /// Data.
        /// </summary>
        public WorkData Data { get; private set; }

        /// <summary>
        /// Send version to the client.
        /// </summary>
        public virtual bool SendVersion => false;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        protected ServerCommand
            (
                WorkData data
            )
        {
            Data = data;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка клиенту кода ошибки.
        /// </summary>
        public void SendError
            (
                int errorCode
            )
        {
            if (errorCode >= 0)
            {
                errorCode = -8888;
            }

            var request = Data.Request.ThrowIfNull();
            var response = new ServerResponse (request);
            Data.Response = response;
            response.WriteInt32 (errorCode).NewLine();
            SendResponse();
        }

        /// <summary>
        /// Отправка клиенту нормального ответа.
        /// </summary>
        public void SendResponse()
        {
            var response = Data.Response.ThrowIfNull();
            string? versionString = null;
            if (SendVersion)
            {
                var serverVersion = ServerUtility.GetServerVersion();
                versionString = serverVersion.Version;
            }

            var packet = response.Encode (versionString);
            var socket = Data.Socket.ThrowIfNull();
            socket.SendAsync (packet);
        }

        /// <summary>
        /// Update the context.
        /// </summary>
        public void UpdateContext()
        {
            var context = Data.Context.ThrowIfNull();
            context.LastActivity = DateTime.Now;
            context.LastCommand = Data.Request!.CommandCode1;
            context.CommandCount++;
        }

        /// <summary>
        /// Execute the command.
        /// </summary>
        public abstract void Execute();

        #endregion

    } // class WorkData

} // namespace ManagedIrbis.Server
