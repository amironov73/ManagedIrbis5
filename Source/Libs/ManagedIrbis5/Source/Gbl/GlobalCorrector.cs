// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* GlobalCorrector.cs -- облегчение выполнения глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl;

/// <summary>
/// Обёртка для облегчения выполнения глобальной корректировки
/// порциями (например, по 100 записей за раз).
/// </summary>
public sealed class GlobalCorrector
{
    #region Events

    /// <summary>
    /// Вызывается после обработки очередной порции записей
    /// и в конце общей обработки.
    /// </summary>
    public event EventHandler? PortionProcessed;

    #endregion

    #region Properties

    /// <summary>
    /// Connection.
    /// </summary>
    public ISyncProvider Connection { get; }

    /// <summary>
    /// Database name.
    /// </summary>
    public string Database { get; private set; }

    /// <summary>
    /// Chunk size.
    /// </summary>
    public int ChunkSize { get; }

    /// <summary>
    /// Актуализировать ли словарь. По умолчанию <c>true</c>.
    /// </summary>
    public bool Actualize { get; set; }

    /// <summary>
    /// Выполнять ли autoin.gbl.
    /// По умолчанию <c>false</c>.
    /// </summary>
    public bool Autoin { get; set; }

    /// <summary>
    /// Выполнять ли формально-логический контроль.
    /// По умолчанию <c>false</c>.
    /// </summary>
    public bool FormalControl { get; set; }

    /// <summary>
    /// Was cancelled?
    /// </summary>
    public bool Cancel { get; set; }

    /// <summary>
    /// Result.
    /// </summary>
    public GblResult Result { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public GlobalCorrector
        (
            ISyncProvider connection
        )
        : this
            (
                connection,
                connection.ThrowIfNull().Database.ThrowIfNull(),
                100
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public GlobalCorrector
        (
            ISyncProvider connection,
            string database
        )
        : this
            (
                connection,
                database.ThrowIfNull(),
                100
            )
    {
        // пусто тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public GlobalCorrector
        (
            ISyncProvider connection,
            string database,
            int chunkSize
        )
    {
        if (chunkSize < 1)
        {
            Magna.Logger.LogError
                (
                    nameof (GlobalCorrector) + "::Constructor"
                        + ": chunkSize={ChunkSize}",
                    chunkSize
                );

            throw new ArgumentOutOfRangeException (nameof (chunkSize));
        }

        Connection = connection;
        Database = database;
        ChunkSize = chunkSize;
        Actualize = true;
        Result = GblResult.GetEmptyResult();
    }

    #endregion

    #region Private members

    #endregion

    #region Public methods

    /// <summary>
    /// Создание <see cref="GlobalCorrector"/> по заданным настройкам
    /// <see cref="GblSettings"/>.
    /// </summary>
    public static GlobalCorrector FromSettings
        (
            ISyncProvider connection,
            GblSettings settings
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (settings);

        var result = new GlobalCorrector (connection)
        {
            Database = settings.Database ?? connection.Database.ThrowIfNull(),
            Actualize = settings.Actualize,
            Autoin = settings.Autoin,
            FormalControl = settings.FormalControl
        };

        return result;
    }

    /// <summary>
    /// Обработать интервал записей.
    /// </summary>
    public GblResult ProcessInterval
        (
            int minMfn,
            int maxMfn,
            GblStatement[] statements
        )
    {
        if (minMfn <= 0)
        {
            Magna.Logger.LogError
                (
                    nameof (GlobalCorrector) + "::" + nameof (ProcessInterval)
                    + ": minMfn={MinMfn}",
                    minMfn
                );

            throw new ArgumentOutOfRangeException (nameof (minMfn));
        }

        var limit = Connection.GetMaxMfn() - 1;
        if (limit <= 0)
        {
            Result = GblResult.GetEmptyResult();

            return Result;
        }

        if (minMfn > limit)
        {
            Magna.Logger.LogError
                (
                    nameof (GlobalCorrector) + "::" + nameof (ProcessInterval)
                    + ": MinMfn={MinMfn}, Limit={Limit}",
                    minMfn,
                    limit
                );

            throw new ArgumentOutOfRangeException (nameof (minMfn));
        }

        maxMfn = Math.Min (maxMfn, limit);
        if (minMfn > maxMfn)
        {
            Magna.Logger.LogError
                (
                    nameof (GlobalCorrector) + "::" + nameof (ProcessInterval)
                    + ": MinMfn={MinMfn}, MaxMfn={MaxMfn}",
                    minMfn,
                    maxMfn
                );

            throw new ArgumentOutOfRangeException (nameof (minMfn));
        }

        if (statements.Length == 0)
        {
            Result = GblResult.GetEmptyResult();

            return Result;
        }

        Result = GblResult.GetEmptyResult();
        Result.RecordsSupposed = maxMfn - minMfn + 1;

        var startMfn = minMfn;

        while (startMfn <= maxMfn)
        {
            var amount = Math.Min
                (
                    maxMfn - startMfn + 1,
                    ChunkSize
                );
            var endMfn = startMfn + amount - 1;

            try
            {
                var settings = ToSettings (statements);
                settings.MinMfn = startMfn;
                settings.MaxMfn = endMfn;

                var intermediateResult = Connection.GlobalCorrection (settings);
                if (intermediateResult is not null)
                {
                    Result.MergeResult (intermediateResult);
                }

                Result.TimeElapsed = DateTime.Now - Result.TimeStarted;

                PortionProcessed.Raise (this);

                if (Cancel || Result.Canceled)
                {
                    Result.Canceled = true;
                    break;
                }
            }
            catch (Exception exception)
            {
                Magna.Logger.LogError
                    (
                        exception,
                        nameof (GlobalCorrector) + "::" + nameof (ProcessInterval)
                    );

                Result.Exception = exception;
                break;
            }

            startMfn = endMfn + 1;
        }

        Result.TimeElapsed = DateTime.Now - Result.TimeStarted;

        return Result;
    }


    /// <summary>
    /// Обработать явно (вручную) заданное множество записей.
    /// </summary>
    public GblResult ProcessRecordset
        (
            IEnumerable<int> recordset,
            GblStatement[] statements
        )
    {
        Sure.NotNull (recordset);
        Sure.NotNull (statements);

        if (statements.Length == 0)
        {
            return GblResult.GetEmptyResult();
        }

        var list = recordset.ToList();
        if (list.Count == 0)
        {
            return GblResult.GetEmptyResult();
        }

        Result = GblResult.GetEmptyResult();
        Result.RecordsSupposed = list.Count;

        while (list.Count > 0)
        {
            var portion = list.Take (ChunkSize).ToArray();
            list = list.Skip (ChunkSize).ToList();
            try
            {
                var settings = ToSettings (statements);
                settings.FirstRecord = 0;
                settings.NumberOfRecords = 0;
                settings.MfnList = portion;

                var intermediateResult = Connection.GlobalCorrection (settings);
                if (intermediateResult is not null)
                {
                    Result.MergeResult (intermediateResult);
                }

                Result.TimeElapsed = DateTime.Now - Result.TimeStarted;

                PortionProcessed.Raise (this);

                if (Cancel || Result.Canceled)
                {
                    Result.Canceled = true;
                    break;
                }
            }
            catch (Exception exception)
            {
                Magna.Logger.LogError
                    (
                        exception,
                        nameof (GlobalCorrector) + "::" + nameof (ProcessRecordset)
                    );

                Result.Exception = exception;
                break;
            }
        }

        Result.TimeElapsed = DateTime.Now - Result.TimeStarted;

        return Result;
    }

    /// <summary>
    /// Обработать результат поиска.
    /// </summary>
    public GblResult ProcessSearchResult
        (
            string searchExpression,
            GblStatement[] statements
        )
    {
        Sure.NotNull (searchExpression);
        Sure.NotNull (statements);

        if (statements.Length == 0)
        {
            Result = GblResult.GetEmptyResult();
            return Result;
        }

        var found = Connection.Search (searchExpression);
        if (found.Length == 0)
        {
            Result = GblResult.GetEmptyResult();
            return Result;
        }

        return ProcessRecordset
            (
                found,
                statements
            );
    }

    /// <summary>
    /// Обработать базу данных в целом.
    /// </summary>
    public GblResult ProcessWholeDatabase
        (
            GblStatement[] statements
        )
    {
        Sure.NotNull (statements);

        if (statements.Length == 0)
        {
            Result = GblResult.GetEmptyResult();
            return Result;
        }

        var maxMfn = Connection.GetMaxMfn() - 1;
        return ProcessInterval
            (
                1,
                maxMfn,
                statements
            );
    }

    /// <summary>
    /// Convert <see cref="GlobalCorrector"/>
    /// to <see cref="GblSettings"/>.
    /// </summary>
    public GblSettings ToSettings
        (
            IEnumerable<GblStatement> statements
        )
    {
        Sure.NotNull (statements);

        var result = new GblSettings
            (
                Connection,
                statements
            )
            {
                Actualize = Actualize,
                Autoin = Autoin,
                Database = Database,
                FormalControl = FormalControl
            };

        return result;
    }

    #endregion
}
