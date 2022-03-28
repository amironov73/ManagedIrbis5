// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Threading.Tasks;

using AM;
using AM.AppServices;

using ManagedIrbis;
using ManagedIrbis.AppServices;

using Minigun.Source;

#endregion

#nullable enable

internal sealed class Program
    : IrbisApplication
{
    #region Properties

    /// <summary>
    /// Количество одновременных клиентских подключений.
    /// </summary>
    public int ConcurrentClients { get; set; }

    /// <summary>
    /// Количество повторов.
    /// </summary>
    public int Repetitions { get; set; }

    #endregion

    #region Construction

    public Program
        (
            string[] args
        )
        : base(args)
    {
        ConcurrentClients = 5;
        Repetitions = 1000;
    }

    #endregion

    #region Private members

    private TaskContext[]? _contexts;
    private Task[]? _tasks;

    private void OneTask
        (
            object? arg
        )
    {
        var context = (TaskContext) arg.ThrowIfNull();

        Console.WriteLine ($"Start task {context.Id}");
        try
        {
            using var connection = context.Connection.ThrowIfNull();
            if (!connection.Connect() || !connection.Connected)
            {
                context.Success = false;
                return;
            }

            for (var repetition = 0; repetition < Repetitions; repetition++)
            {
                connection.NoOperation();
                context.Counter++;
            }

            context.Success = true;
        }
        catch (Exception exception)
        {
            context.Success = false;
            context.Exception = exception;
        }

        Console.WriteLine ($"End task {context.Id}");
    }

    #endregion

    #region Public methods

    public static int Main
        (
            string[] args
        )
    {
        try
        {
            new Program (args).Run();
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);

            return 1;
        }

        return 0;
    }

    #endregion

    #region IrbisApplication members

    /// <inheritdoc cref="MagnaApplication.ActualRun"/>
    protected override int ActualRun()
    {
        Debugger.Break();
        var mainConnection = (Connection as SyncConnection).ThrowIfNull();
        var settings = ConnectionSettings.FromConnection (mainConnection);

        _contexts = new TaskContext[ConcurrentClients];
        _tasks = new Task[ConcurrentClients];

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        for (var i = 0; i < ConcurrentClients; i++)
        {
            var connection = ConnectionFactory.Shared.CreateSyncConnection().ThrowIfNull();
            settings.Apply ((IIrbisProvider) connection);
            _contexts[i] = new TaskContext
            {
                Id = i,
                Connection = connection
            };

            _tasks[i] = new Task (OneTask, _contexts[i]);
            _tasks[i].Start();
        }

        Task.WaitAll (_tasks);
        stopwatch.Stop();
        var elapsed = stopwatch.Elapsed;
        var perSecond = ((double)ConcurrentClients * Repetitions) / elapsed.TotalSeconds;
        Console.WriteLine ($"Ops per second: {perSecond:N2}");

        return 0;
    }

    #endregion

}
