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

/* ServerWorker.cs -- рабочий поток, обслуживающий клиентские запросы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Server.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Рабочий поток, обслуживающий клиентские запросы.
    /// </summary>
    public sealed class ServerWorker
    {

        #region Properties

        /// <summary>
        /// Данные, необходимые для обслуживания клиента.
        /// </summary>
        public WorkData Data { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ServerWorker
            (
                WorkData data
            )
        {
            Data = data;
            data.Started = DateTime.Now;
            data.Task = new Task(DoWork);

        } // constructor

        #endregion

        #region Private members

        private void _WritePacket
            (
                string prefix,
                ReadOnlyMemory<byte>[] chunks
            )
        {
            // TODO async

            var engine = Data.Engine;
            var request = Data.Request;
            var fileName = string.Format
                (
                    "{0}_{1}_{2}.packet",
                    prefix,
                    request.ClientId,
                    request.CommandNumber
                );
            fileName = Path.Combine(engine.WorkDir, fileName);
            using (var stream = File.Create(fileName))
            {
                foreach (var chunk in chunks)
                {
                    stream.Write(chunk.Span);
                }
            }
        }

        private void _LogRequest()
        {
            var request = Data.Request;
            var memory = request.Memory;
            var savedPosition = memory.Position;
            memory.Position = 0;
            var packet = new ReadOnlyMemory<byte>[1];
            packet[0] = memory.ToArray();
            memory.Position = savedPosition;
            _WritePacket("rqst", packet);
        }

        private void _LogResponse()
        {
            var response = Data.Response;
            var memory = response.Memory;
            var savedPosition = memory.Position;
            memory.Position = 0;
            var packet = response.Encode(null);
            memory.Position = savedPosition; //-V3008
            _WritePacket("rsps", packet);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Do the work.
        /// </summary>
        public void DoWork()
        {
            try
            {
                var request = new ClientRequest(Data);
                Data.Request = request;

                _LogRequest();

                Magna.Trace("ServerWorker::DoWork: request: address="
                          + Data.Socket.GetRemoteAddress()
                          + ", command=" + request.CommandCode1
                          + ", login=" + request.Login
                          + ", workstation=" + request.Workstation);

                Data.Response = new ServerResponse(Data.Request);
                Data.Command = Data.Engine.Mapper.MapCommand(Data);
                Data.Command.Execute();

                _LogResponse();

                Magna.Trace("ServerWorker::DoWork: success: address="
                          + Data.Socket.GetRemoteAddress()
                          + ", command=" + request.CommandCode1
                          + ", login=" + request.Login
                          + ", workstation=" + request.Workstation);
            }
            catch (Exception exception)
            {
                Magna.TraceException (nameof(ServerWorker) + "::" + nameof(DoWork), exception);
            }
            finally
            {
                Data.Socket!.DisposeAsync().GetAwaiter().GetResult();
                lock (Data.Engine!.SyncRoot)
                {
                    Data.Engine.Workers.Remove(this);
                }
            }
        }

        #endregion

    } // class ServerWorker

} // namespace ManagedIrbis.Server
