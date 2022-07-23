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

/* IrbisMonitor.cs -- мониторинг работы ИРБИС
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

namespace ManagedIrbis.Monitoring;

/// <summary>
/// Мониторинг работы ИРБИС.
/// </summary>
public sealed class IrbisMonitor
{
    #region Constants

    /// <summary>
    /// Default interval value, milliseconds.
    /// </summary>
    public const int DefaultInterval = 60 * 1000;

    #endregion

    #region Properties

    /// <summary>
    /// Whether monitoring is active.
    /// </summary>
    public bool Active { get; private set; }

    /// <summary>
    /// Connection.
    /// </summary>
    public ISyncProvider Connection { get; private set; }

    /// <summary>
    /// Database names.
    /// </summary>
    public List<string> Databases { get; private set; }

    /// <summary>
    /// Interval between measuring, milliseconds.
    /// </summary>
    public int Interval
    {
        get => _interval;
        set
        {
            if (value < 1)
            {
                Magna.Logger.LogError
                    (
                        nameof (IrbisMonitor) + "::" + nameof (Interval)
                        + ": value={Value}",
                        value
                    );

                throw new ArgumentException (nameof (value));
            }

            _interval = value;
        }
    }

    /// <summary>
    /// Sink to write data.
    /// </summary>
    public MonitoringSink Sink { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public IrbisMonitor
        (
            ISyncProvider connection,
            IEnumerable<string> databases
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull ((object?) databases);

        Connection = connection;
        Databases = new ();
        foreach (var database in databases)
        {
            Databases.Add (database);
        }

        _interval = DefaultInterval;
        Sink = new NullMonitoringSink();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    #endregion

    #region Private members

    private int _interval;

    private Task? _workerTask;

    private readonly CancellationTokenSource _cancellationTokenSource;

    private void _MonitoringRoutine()
    {
        while (Active)
        {
            try
            {
                var data = GetDataPortion();
                if (!Sink.WriteData (data))
                {
                    Active = false;
                    break;
                }
            }
            catch (Exception exception)
            {
                Magna.Logger.LogError
                    (
                        exception,
                        nameof (IrbisMonitor) + "::" + nameof (_MonitoringRoutine)
                    );
            }

            if (!Active)
            {
                break;
            }

            Task.Delay (Interval).Wait (_cancellationTokenSource.Token);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Get portion of monitoring data.
    /// </summary>
    public MonitoringData GetDataPortion()
    {
        var result = new MonitoringData
        {
            Moment = DateTime.Now
        };

        try
        {
            var serverStat = Connection.GetServerStat();
            var clients = serverStat?.RunningClients;
            result.Clients = clients?.Length ?? 0;
            result.Commands = serverStat?.TotalCommandCount ?? 0;

            var list = new List<DatabaseData> (Databases.Count);
            foreach (var database in Databases)
            {
                var databaseInfo = Connection.GetDatabaseInfo (database);
                if (databaseInfo is not null)
                {
                    var data = new DatabaseData();
                    if (!ReferenceEquals (databaseInfo.LogicallyDeletedRecords, null))
                    {
                        data.DeletedRecords = databaseInfo.LogicallyDeletedRecords.Length;
                    }

                    if (!ReferenceEquals (databaseInfo.LockedRecords, null))
                    {
                        data.LockedRecords = databaseInfo.LockedRecords;
                    }

                    list.Add (data);
                }
            }

            result.Databases = list.ToArray();
        }
        catch (Exception exception)
        {
            result.ErrorMessage = exception.GetType().Name + ": " + exception.Message;
        }

        return result;
    }

    /// <summary>
    /// Start monitoring.
    /// </summary>
    public void StartMonitoring()
    {
        if (Active)
        {
            return;
        }

        Active = true;
        _workerTask = new Task (_MonitoringRoutine);
        _workerTask.Start();
    }

    /// <summary>
    /// Stop monitoring.
    /// </summary>
    public void StopMonitoring()
    {
        if (!Active)
        {
            return;
        }

        Active = false;

        if (!ReferenceEquals (_workerTask, null))
        {
            _cancellationTokenSource.Cancel();

            //_workerTask.Wait();
            _workerTask = null;
        }
    }

    #endregion
}
