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
using System.IO;
using System.Threading.Tasks;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Server;

/// <summary>
/// Рабочий поток, обслуживающий клиентские запросы.
/// </summary>
public sealed class ServerWorker
{
    #region Properties

    /// <summary>
    /// Данные, необходимые для обслуживания клиента.
    /// </summary>
    public WorkData Data { get; }

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
        data.Task = new Task (DoWork);
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

        var engine = Data.Engine.ThrowIfNull (nameof (Data.Engine));
        var request = Data.Request.ThrowIfNull (nameof (Data.Request));
        var fileName = Path.Combine
            (
                engine.WorkDir,
                $"{prefix}_{request.ClientId}_{request.CommandNumber}.packet"
            );
        using var stream = File.Create (fileName);
        foreach (var chunk in chunks)
        {
            stream.Write (chunk.Span);
        }
    }

    private void _LogRequest()
    {
        var request = Data.Request.ThrowIfNull (nameof (Data.Request));
        var memory = request.Memory.ThrowIfNull (nameof (request.Memory));
        var savedPosition = memory.Position;
        memory.Position = 0;
        var packet = new ReadOnlyMemory<byte>[1];
        packet[0] = memory.ToArray();
        memory.Position = savedPosition;
        _WritePacket ("rqst", packet);
    }

    private void _LogResponse()
    {
        var response = Data.Response.ThrowIfNull (nameof (Data.Response));
        var memory = response.Memory;
        var savedPosition = memory.Position;
        memory.Position = 0;
        var packet = response.Encode (null);
        memory.Position = savedPosition; //-V3008
        _WritePacket ("rsps", packet);
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
            var request = new ClientRequest (Data);
            Data.Request = request;

            _LogRequest();

            var socket = Data.Socket.ThrowIfNull (nameof (Data.Socket));
            Magna.Logger.LogTrace
                (
                    nameof (ServerWorker) + "::" + nameof (DoWork)
                    + ": begin: address={Address}, command={Command}, "
                    + "login={Login}, workstation={Workstation}",
                    socket.GetRemoteAddress(),
                    request.CommandCode1.ToVisibleString(),
                    request.Login.ToVisibleString(),
                    request.Workstation.ToVisibleString()
                );

            var engine = Data.Engine.ThrowIfNull (nameof (Data.Engine));
            Data.Response = new ServerResponse (Data.Request);
            Data.Command = engine.Mapper.MapCommand (Data);
            Data.Command.Execute();

            _LogResponse();

            // TODO залогировать также код возврата
            Magna.Logger.LogTrace
                (
                    nameof (ServerWorker) + "::" + nameof (DoWork)
                    + ": success: address={Address}, command={Command}, "
                    + "login={Login}, workstation={Workstation}",
                    socket.GetRemoteAddress(),
                    request.CommandCode1.ToVisibleString(),
                    request.Login.ToVisibleString(),
                    request.Workstation.ToVisibleString()
                );

        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (ServerWorker) + "::" + nameof (DoWork)
                );
        }
        finally
        {
            // TODO исправить прямой доступ к Data
            Data.Socket!.DisposeAsync().GetAwaiter().GetResult();
            lock (Data.Engine!.SyncRoot)
            {
                Data.Engine.Workers.Remove (this);
            }
        }
    }

    #endregion
}
