// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMethodReturnValue.Local

/* DebtorManager.cs -- работа с задолжниками
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Работа с задолжниками.
/// </summary>
public sealed class DebtorManager
{
    #region Events

    /// <summary>
    /// Fired on batch read.
    /// </summary>
    public event EventHandler? BatchRead;

    #endregion

    #region Properties

    /// <summary>
    /// Connection.
    /// </summary>
    public ISyncProvider Connection { get; private set; }

    /// <summary>
    /// Database name.
    /// </summary>
    public string? Database { get; set; }

    /// <summary>
    /// Кафедра обслуживания.
    /// </summary>
    public string? Department { get; set; }

    /// <summary>
    /// С какой даты задолженность?
    /// </summary>
    public DateTime? FromDate { get; set; }

    /// <summary>
    /// По какую дату задолженность.
    /// </summary>
    public DateTime? ToDate { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DebtorManager
        (
            ISyncProvider connection
        )
    {
        Sure.NotNull (connection);

        Database = "RDR";
        Connection = connection;
    }

    #endregion

    #region Private members

    private string? _fromDate, _toDate;

    private void HandleBatchRead
        (
            object? sender,
            EventArgs eventArgs
        )
    {
        BatchRead?.Invoke (sender, eventArgs);
    }

    /// <summary>
    /// Setup <see cref="FromDate"/> and
    /// <see cref="ToDate"/>.
    /// </summary>
    /// <returns><c>true</c> on success,
    /// <c>false</c> otherwise.</returns>
    private bool SetupDates()
    {
        if (!FromDate.HasValue
            || !ToDate.HasValue)
        {
            return false;
        }

        _fromDate = IrbisDate.ConvertDateToString (FromDate.Value);
        _toDate = IrbisDate.ConvertDateToString (ToDate.Value);

        return true;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Fill additional fields.
    /// </summary>
    public VisitInfo FillInfo
        (
            VisitInfo debt,
            string? format
        )
    {
        var database = Connection.EnsureDatabase (debt.Database);
        var index = debt.Index.ThrowIfNull ("Index not specified");

        var previousDatabase = Connection.Database;
        Connection.Database = database;
        var bookRecord = Connection.ByIndex (index);
        Connection.Database = previousDatabase;

        if (bookRecord is not null)
        {
            if (!string.IsNullOrEmpty (format))
            {
                var parameters = new FormatRecordParameters
                {
                    Database = database,
                    Format = format,
                    Mfn = bookRecord.Mfn
                };
                if (Connection.FormatRecords (parameters))
                {
                    debt.Description = parameters.Result.AsSingle();
                }
            }

            debt.Price = debt.GetBookPrice (bookRecord);
            debt.Year = VisitInfo.GetBookYear (bookRecord);
        }

        return debt;
    }

    /// <summary>
    /// Get debt from the reader
    /// </summary>
    public VisitInfo[] GetDebt
        (
            ReaderInfo reader
        )
    {
        var result = reader.Visits!.GetDebt
            (
                _fromDate!,
                _toDate!
            );

        if (!string.IsNullOrEmpty (Department))
        {
            result = result.Where
                    (
                        item => item.Department.SameString
                            (
                                Department
                            )
                    )
                .ToArray();
        }

        return result.ToArray();
    }


    /// <summary>
    /// Get debtor from the reader.
    /// </summary>
    /// <returns><c>null</c> if reader is not debtor.
    /// </returns>
    public DebtorInfo? GetDebtor
        (
            ReaderInfo reader
        )
    {
        var debt = GetDebt (reader);

        if (debt.Length == 0)
        {
            return null;
        }

        var result = DebtorInfo.FromReader
            (
                reader,
                debt
            );

        return result;
    }

    /// <summary>
    /// Получение списка задолжников.
    /// </summary>
    public DebtorInfo[] GetDebtors()
    {
        SetupDates();

        var manager = new ReaderManager (Connection);
        manager.BatchRead += HandleBatchRead;

        var database = Connection.EnsureDatabase (Database);
        var readers = manager.GetAllReaders (database);
        var result = new List<DebtorInfo> (readers.Length);
        string? fromDate = null;
        if (FromDate.HasValue)
        {
            fromDate = IrbisDate.ConvertDateToString (FromDate.Value);
        }

        foreach (var reader in readers)
        {
            var visits = reader.Visits.ThrowIfNull();
            var debt = ToDate.HasValue
                ? visits.GetDebt (ToDate.Value)
                : visits.GetDebt();

            if (!string.IsNullOrEmpty (fromDate))
            {
                debt = debt.Where
                        (
                            loan => string.CompareOrdinal
                                (
                                    loan.DateExpectedString,
                                    fromDate
                                )
                                 >= 0
                        )
                    .ToArray();
            }

            if (!string.IsNullOrEmpty (Department))
            {
                debt = debt.Where
                        (
                            loan => loan.Department.SameString
                                (
                                    Department
                                )
                        )
                    .ToArray();
            }

            if (debt.Length != 0)
            {
                var debtor = DebtorInfo.FromReader
                    (
                        reader,
                        debt
                    );
                result.Add (debtor);
            }
        }

        manager.BatchRead -= HandleBatchRead;

        return result.ToArray();
    }

    /// <summary>
    /// Получение списка задолжников.
    /// </summary>
    public DebtorInfo[] GetDebtors
        (
            IEnumerable<int> mfns
        )
    {
        SetupDates();

        var manager = new ReaderManager (Connection);
        manager.BatchRead += HandleBatchRead;

        var database = Connection.EnsureDatabase (Database);
        var readers = manager.GetReaders (database, mfns);
        var result = new List<DebtorInfo> (readers.Length);
        string? fromDate = null;
        if (FromDate.HasValue)
        {
            fromDate = IrbisDate.ConvertDateToString (FromDate.Value);
        }

        foreach (var reader in readers)
        {
            var visits = reader.Visits.ThrowIfNull();
            var debt = ToDate.HasValue
                ? visits.GetDebt (ToDate.Value)
                : visits.GetDebt();

            if (!string.IsNullOrEmpty (fromDate))
            {
                debt = debt.Where
                        (
                            loan => string.CompareOrdinal
                                (
                                    loan.DateExpectedString,
                                    fromDate
                                ) >= 0
                        )
                    .ToArray();
            }

            if (!string.IsNullOrEmpty (Department))
            {
                debt = debt.Where
                        (
                            loan => loan.Department.SameString
                                (
                                    Department
                                )
                        )
                    .ToArray();
            }

            if (debt.Length != 0)
            {
                var debtor = DebtorInfo.FromReader
                    (
                        reader,
                        debt
                    );
                result.Add (debtor);
            }
        }

        manager.BatchRead -= HandleBatchRead;

        return result.ToArray();
    }

    #endregion
}
