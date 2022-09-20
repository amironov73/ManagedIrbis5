// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ConnectionBase.cs -- общая функциональность для синхронного и асинхронного подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Logging;
using AM.PlatformAbstraction;
using AM.Threading;

using ManagedIrbis.Performance;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Общая функциональность для синхронного и асинхронного подключения.
/// </summary>
public abstract class ConnectionBase
    : IIrbisProvider
{
    #region Events

    /// <inheritdoc cref="IIrbisProvider.Disposing"/>
    public event EventHandler? Disposing;

    #endregion

    #region Properties

    /// <inheritdoc cref="IConnectionSettings.Host"/>
    public string? Host { get; set; } = "127.0.0.1";

    /// <inheritdoc cref="IConnectionSettings.Port"/>
    public ushort Port { get; set; } = 6666;

    /// <inheritdoc cref="IConnectionSettings.Username"/>
    public string? Username { get; set; } = string.Empty;

    /// <inheritdoc cref="IConnectionSettings.Password"/>
    public string? Password { get; set; } = string.Empty;

    /// <inheritdoc cref="IIrbisProvider.Database"/>
    public string? Database { get; set; } = "IBIS";

    /// <inheritdoc cref="IConnectionSettings.Workstation"/>
    public string? Workstation { get; set; } = "C";

    /// <inheritdoc cref="IConnectionSettings.ClientId"/>
    public int ClientId { get; protected internal set; }

    /// <inheritdoc cref="IConnectionSettings.QueryId"/>
    public int QueryId
    {
        get => _queryId; // переменная нужна для Interlocked.Increment
        protected internal set => _queryId = value;
    }

    /// <inheritdoc cref="IIrbisProvider.IsConnected"/>
    public bool IsConnected { get; protected internal set; }

    /// <inheritdoc cref="IGetLastError.LastError"/>
    public int LastError { get; protected internal set; }

    /// <summary>
    /// Токен для отмены длительных операций.
    /// </summary>
    public CancellationToken Cancellation { get; protected set; }

    /// <summary>
    /// Версия сервера. Берется из ответа на регистрацию клиента.
    /// Сервер может прислать и пустую строку, надо быть
    /// к этому готовым.
    /// </summary>
    public string? ServerVersion { get; protected internal set; }

    /// <summary>
    /// INI-файл, присылвемый сервером в ответ на регистрацию клиента.
    /// </summary>
    public IniFile? IniFile { get; protected set; }

    /// <summary>
    /// Интервал подтверждения на сервере, минуты.
    /// Берется из ответа сервера при регистрации клиента.
    /// Сервер может прислать и пустую строку, к этому надо
    /// быть готовым.
    /// </summary>
    public int Interval { get; protected set; }

    #endregion

    #region ISupportLogging members

    /// <inheritdoc cref="ISupportLogging.Logger"/>

    // TODO implement
    public virtual ILogger? Logger => _logger;

    /// <inheritdoc cref="ISupportLogging.SetLogger"/>
    public virtual void SetLogger
        (
            ILogger? logger
        )
    {
        _logger = logger;
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов.</param>
    protected ConnectionBase
        (
            IServiceProvider serviceProvider
        )
    {
        Sure.NotNull (serviceProvider);

        Busy = new BusyState();
        _serviceProvider = serviceProvider;
        _cancellation = new CancellationTokenSource();
        Cancellation = _cancellation.Token;
        _logger = Magna.Factory.CreateLogger<IIrbisProvider>();
        PlatformAbstraction = PlatformAbstractionLayer.Current;
    }

    #endregion

    #region Private members

    /// <summary>
    /// Логгер.
    /// </summary>
    protected internal ILogger? _logger;

    /// <summary>
    /// Сборщик сведений о производительности.
    /// </summary>
    protected internal IPerformanceCollector? _performanceCollector;

    /// <summary>
    /// Провайдер сервисов.
    /// </summary>
    protected internal IServiceProvider _serviceProvider;

    /// <summary>
    /// Отмена выполнения операций.
    /// </summary>
    protected internal CancellationTokenSource _cancellation;

    /// <summary>
    /// Номер запроса.
    /// </summary>
    protected int _queryId;

    private Stack<string>? _databaseStack;

    /// <summary>
    /// Установка состояния занятости.
    /// </summary>
    protected internal void SetBusy
        (
            bool busy
        )
    {
        if (Busy.State != busy)
        {
            _logger?.LogTrace ($"SetBusy: {busy}");
            Busy.SetState (busy);

            // TODO: нужно ли это здесь?
            // BusyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// Вызывает событие <see cref="Disposing"/>.
    /// </summary>
    protected void OnDisposing()
    {
        if (IsConnected)
        {
            Disposing.Raise (this);
        }
    }

    /// <summary>
    /// Вызывает событие <see cref="Disposing"/>.
    /// </summary>
    protected async ValueTask OnDisposingAsync()
    {
        if (IsConnected)
        {
            await Disposing.RaiseAsync (this);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Подстановка имени текущей базы данных, если она не задана явно.
    /// </summary>
    /// <param name="database">Имя базы данных. <c>null</c> означает
    /// текущую базу данных.</param>
    public string EnsureDatabase
        (
            string? database = null
        )
    {
        return string.IsNullOrEmpty (database)
            ? string.IsNullOrEmpty (Database)
                ? throw new ArgumentException (nameof (Database))
                : Database
            : database;
    }

    /// <summary>
    /// Запоминание текущей базы данных.
    /// </summary>
    /// <param name="newDtabase">Новая база данных,
    /// на которую надо переключиться.</param>
    /// <returns>Старая база данных.</returns>
    public string PushDatabase
        (
            string newDtabase
        )
    {
        Sure.NotNullNorEmpty (newDtabase);

        _databaseStack ??= new Stack<string>();
        var result = EnsureDatabase (newDtabase);

        _databaseStack.Push (result);
        Database = newDtabase;

        return result;
    }

    /// <summary>
    /// Восстановление ранее запомненной базы данных.
    /// </summary>
    public string PopDatabase()
    {
        var result = EnsureDatabase (string.Empty);
        if (_databaseStack is not null)
        {
            if (_databaseStack.TryPop (out var temp))
            {
                Database = temp;
            }

            if (_databaseStack.Count == 0)
            {
                _databaseStack = null;
            }
        }

        return result;
    }

    #endregion

    #region ICancellable members

    /// <inheritdoc cref="ICancellable.Busy"/>
    public BusyState Busy { get; protected internal set; }

    /// <inheritdoc cref="ICancellable.CancelOperation"/>
    public void CancelOperation() => _cancellation.Cancel();

    /// <inheritdoc cref="ICancellable.ThrowIfCancelled"/>
    public void ThrowIfCancelled() => Cancellation.ThrowIfCancellationRequested();

    #endregion

    #region IIrbisProvider members

    /// <inheritdoc cref="IIrbisProvider.CheckProviderState"/>
    public bool CheckProviderState()
    {
        if (!IsConnected)
        {
            LastError = -100_500;
        }

        return IsConnected;
    }

    /// <inheritdoc cref="IIrbisProvider.Configure"/>
    public void Configure (string configurationString) =>
        this.ParseConnectionString (configurationString);

    /// <inheritdoc cref="IIrbisProvider.GetGeneration"/>
    public string GetGeneration() => "64";

    /// <inheritdoc cref="IIrbisProvider.GetWaitHandle"/>
    public WaitHandle GetWaitHandle() => Busy.WaitHandle;

    /// <summary>
    /// Абстракция от платформы.
    /// </summary>
    public PlatformAbstractionLayer PlatformAbstraction { get; set; }

    #endregion

    #region IServiceProvider members

    /// <inheritdoc cref="IServiceProvider.GetService"/>
    public object? GetService
        (
            Type serviceType
        )
    {
        return _serviceProvider.ThrowIfNull (nameof (_serviceProvider)).GetService (serviceType);
    }

    #endregion

    #region ISetLastError members

    /// <inheritdoc cref="ISetLastError.SetLastError"/>
    public int SetLastError (int code) => LastError = code;

    #endregion

    #region IAsyncDisposable members

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public abstract ValueTask DisposeAsync();

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public abstract void Dispose();

    #endregion
}
