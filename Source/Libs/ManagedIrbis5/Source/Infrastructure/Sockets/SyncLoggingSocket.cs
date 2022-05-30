// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SyncLoggingSocket.cs -- синхронный логирующий сокет
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure.Sockets;

/// <summary>
/// Синхронный логирующий сокет.
/// Записывает все исходящие и входящие пакеты.
/// </summary>
public sealed class SyncLoggingSocket
    : SyncNestedSocket
{
    #region Properties

    /// <summary>
    /// Директория, в которой сохраняются пакеты.
    /// </summary>
    public string LoggingPath { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SyncLoggingSocket
        (
            ISyncClientSocket innerSocket,
            string loggingPath
        )
        : base (innerSocket)
    {
        if (!Directory.Exists (loggingPath))
        {
            Directory.CreateDirectory (loggingPath);
        }

        LoggingPath = loggingPath;
        _counter = 0;
    }

    #endregion

    #region Private members

    private int _counter;

    private void LogPacket
        (
            SyncQuery query
        )
    {
        var fileName = Path.Combine (LoggingPath, $"{_counter:000000000}up.packet");
        var body = query.GetBody();
        File.WriteAllBytes (fileName, body.ToArray());
    }

    private void LogPacket
        (
            Response? response
        )
    {
        var fileName = Path.Combine (LoggingPath, $"{_counter:00000000}dn.packet");
        var body = response?.RemainingBytes() ?? Array.Empty<byte>();
        File.WriteAllBytes (fileName, body);
    }

    #endregion

    #region ISyncClientSocket members

    /// <inheritdoc cref="ISyncClientSocket.TransactSync"/>
    public override Response? TransactSync
        (
            SyncQuery query
        )
    {
        LogPacket (query);

        var result = base.TransactSync (query);
        LogPacket (result);
        ++_counter;

        return result;
    }

    #endregion
}
