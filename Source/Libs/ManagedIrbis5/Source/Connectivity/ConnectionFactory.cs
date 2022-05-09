// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConnectionFactory.cs -- фабрика подключений к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Infrastructure.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Фабрика подключений к серверу ИРБИС64.
/// </summary>
/// <remarks>
/// Предполагается создание классов-потомков,
/// определенным образом настраивающих создаваемое подключение.
/// </remarks>
public class ConnectionFactory
{
    #region Properties

    /// <summary>
    /// Общий экземпляр фабрики подключений.
    /// </summary>
    public static ConnectionFactory Shared { get; private set; } = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Создание синхронного подключения с настройками по умолчанию.
    /// </summary>
    public virtual SyncConnection CreateSyncConnection()
    {
        var socket = new SyncTcp4Socket();
        var result = new SyncConnection (socket, Magna.Host.Services);

        return result;
    }

    /// <summary>
    /// Создание асинхронного подключения с настройками по умолчанию.
    /// </summary>
    public virtual AsyncConnection CreateAsyncConnection()
    {
        var socket = new AsyncTcp4Socket();
        var result = new AsyncConnection (socket, Magna.Host.Services);

        return result;
    }

    /// <summary>
    /// Замена общего экземпляра фабрики на указанный.
    /// </summary>
    /// <param name="newFactory">Экземпляр, который отныне станет общим.
    /// </param>
    /// <returns>Предыдущий общий экземпляр.</returns>
    public static ConnectionFactory Replace
        (
            ConnectionFactory newFactory
        )
    {
        Sure.NotNull (newFactory);

        var result = Shared;
        Shared = newFactory;

        return result;
    }

    #endregion
}
