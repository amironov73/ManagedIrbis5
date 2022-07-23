// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* ConnectionPool.cs -- пул соединений с сервером.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pooling;

/// <summary>
/// Пул соединений с сервером.
/// </summary>
public class ConnectionPool
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Строка подключения по умолчанию.
    /// </summary>
    public static string? DefaultConnectionString { get; set; }

    /// <summary>
    /// Количество одновременных подключений по умолчанию.
    /// </summary>
    public static int DefaultCapacity
    {
        get => _defaultCapacity;
        set
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException ("value");
            }

            _defaultCapacity = value;
        }
    }

    /// <summary>
    /// Количество одновременных подключений.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Строка подключения к серверу.
    /// </summary>
    public string ConnectionString { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ConnectionPool()
    {
        Magna.Logger.LogTrace (nameof (ConnectionPool) + "::Constructor");

        Capacity = DefaultCapacity;
        ConnectionString = DefaultConnectionString.ThrowIfNull();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConnectionPool
        (
            int capacity
        )
        : this()
    {
        if (capacity < 1)
        {
            capacity = DefaultCapacity;
        }

        Capacity = capacity;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConnectionPool
        (
            string connectionString
        )
        : this()
    {
        Sure.NotNullNorEmpty (connectionString);

        ConnectionString = connectionString;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConnectionPool
        (
            int capacity,
            string connectionString
        )
        : this (capacity)
    {
        Sure.NotNullNorEmpty (connectionString);

        ConnectionString = connectionString;
    }

    #endregion

    #region Private members

    private static int _defaultCapacity = 5;

    private readonly List<ISyncProvider> _activeConnections = new ();
    private readonly List<ISyncProvider> _idleConnections = new ();
    private readonly AutoResetEvent _event = new (false);
    private readonly object _syncRoot = new ();

    private ISyncProvider? _GetNewClient()
    {
        if (_activeConnections.Count >= Capacity)
        {
            Magna.Logger.LogTrace
                (
                    nameof (ConnectionPool) + "::" + nameof (_GetNewClient)
                    + ": capacity exhausted"
                );

            return null;
        }

        var result = ConnectionFactory.Shared.CreateSyncConnection();
        result.ParseConnectionString (ConnectionString);
        result.Connect();
        _activeConnections.Add (result);

        return result;
    }

    private ISyncProvider? _GetIdleClient()
    {
        if (_idleConnections.Count == 0)
        {
            Magna.Logger.LogTrace
                (
                    nameof (ConnectionPool) + "::" + nameof (_GetIdleClient)
                    + ": no idle clients"
                );

            return null;
        }

        var result = _idleConnections[0];
        _idleConnections.RemoveAt (0);
        _activeConnections.Add (result);

        return result;
    }

    private ISyncProvider _WaitForClient()
    {
        while (true)
        {
            if (!_event.WaitOne())
            {
                Magna.Logger.LogError
                    (
                        nameof (ConnectionPool) + "::" + nameof (_WaitForClient)
                        + ": WaitOne failed"
                    );

                throw new IrbisException ("WaitOne failed");
            }

            lock (_syncRoot)
            {
                var result = _GetIdleClient();
                if (!ReferenceEquals (result, null))
                {
                    return result;
                }
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Требование нового подключения к серверу.
    /// </summary>
    /// <remarks>Может подвесить поток на неопределённое время.
    /// </remarks>
    public ISyncProvider AcquireConnection()
    {
        // TODO сделать версию с таймаутом

        ISyncProvider? result;

        lock (_syncRoot)
        {
            result = _GetIdleClient() ?? _GetNewClient();
        }

        return result ?? _WaitForClient();
    }

    /// <summary>
    /// Требование нового подключения к серверу.
    /// </summary>
    public Task<ISyncProvider> AcquireConnectionAsync()
    {
        var result = Task<ISyncProvider>.Factory.StartNew (AcquireConnection);

        return result;
    }


    /// <summary>
    /// Исполнение некоторых действий на подключении из пула.
    /// </summary>
    public void Execute
        (
            Action<ISyncProvider> action
        )
    {
        Sure.NotNull (action);

        using var guard = new PoolGuard (this);
        action (guard.Connection);
    }

    /// <summary>
    /// Исполнение некоторых действий на подключении из пула.
    /// </summary>
    public Task ExecuteAsync
        (
            Action<ISyncProvider> action
        )
    {
        Sure.NotNull (action);

        var result = Task.Factory.StartNew
            (
                () => { Execute (action); }
            );

        return result;
    }

    /// <summary>
    /// Исполнение некоторых действий на подключении из пула.
    /// </summary>
    public void Execute<T>
        (
            Action<ISyncProvider, T> action,
            T userData
        )
    {
        Sure.NotNull (action);

        using var guard = new PoolGuard (this);
        action (guard.Connection, userData);
    }

    /// <summary>
    /// Исполнение некоторых действий на подключении из пула.
    /// </summary>
    public TResult Execute<TResult, T1>
        (
            Func<ISyncProvider, T1, TResult> function,
            T1 userData
        )
    {
        Sure.NotNull (function);

        using var guard = new PoolGuard (this);
        var result = function (guard.Connection, userData);

        return result;
    }

    /// <summary>
    /// Возвращение подключения в пул.
    /// </summary>
    public void ReleaseConnection
        (
            ISyncProvider connection
        )
    {
        Sure.NotNull (connection);

        lock (_syncRoot)
        {
            if (!_activeConnections.Contains (connection))
            {
                Magna.Logger.LogError
                    (
                        nameof (ConnectionPool) + "::" + nameof (ReleaseConnection)
                        + ": foreign connection detected"
                    );

                throw new IrbisException ("Foreign connection detected");
            }

            _activeConnections.Remove (connection);
            if (connection.Connected)
            {
                _idleConnections.Add (connection);
            }

            _event.Set();
        }
    }

    /// <summary>
    /// Закрывает простаивающие соединения.
    /// </summary>
    public void ReleaseIdleConnections()
    {
        lock (_syncRoot)
        {
            while (_idleConnections.Count != 0)
            {
                _idleConnections[0].Dispose();
                _idleConnections.RemoveAt (0);
            }
        }
    }

    /// <summary>
    /// Ожидание закрытия всех активных подключений.
    /// </summary>
    public void WaitForAllConnections()
    {
        while (true)
        {
            if (!_event.WaitOne())
            {
                throw new IrbisException ("WaitOne failed");
            }

            lock (_syncRoot)
            {
                if (_activeConnections.Count == 0)
                {
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Ожидание закрытия всех активных подключений.
    /// </summary>
    public Task WaitForAllConnectionsAsync()
    {
        var result = Task.Factory.StartNew (WaitForAllConnections);

        return result;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        Magna.Logger.LogTrace (nameof (ConnectionPool) + "::" + nameof (Dispose) + ": enter");

        lock (_syncRoot)
        {
            if (_activeConnections.Count != 0)
            {
                Magna.Logger.LogError
                    (
                        nameof (ConnectionPool) + "::" + nameof (Dispose)
                        + ": have active connections: {Count}",
                        _activeConnections.Count
                    );

                throw new IrbisException ($"Have active connections: {_activeConnections.Count}");
            }

            foreach (var client in _idleConnections)
            {
                client.Dispose();
            }
        }

        Magna.Logger.LogTrace (nameof (ConnectionPool) + "::" + nameof (Dispose) + ": leave");
    }

    #endregion
}
