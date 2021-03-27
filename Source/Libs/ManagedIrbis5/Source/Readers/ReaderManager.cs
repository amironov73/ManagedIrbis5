// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* ReaderManager.cs -- основные операции с базой RDR
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.Batch;

#endregion

#nullable enable

namespace ManagedIrbis.Readers
{
    /// <summary>
    /// Основные операции с читателями и базой RDR.
    /// </summary>
    public sealed class ReaderManager
    {
        #region Constants

        /// <summary>
        /// Стандартный префикс идентификатора читателя.
        /// </summary>
        // TODO: брать индекс из настроек клиента
        public const string ReaderIdentifier = "RI=";

        #endregion

        #region Events

        /// <summary>
        /// Fired on batch read.
        /// </summary>
        public event EventHandler? BatchRead;

        #endregion

        #region Properties

        /// <summary>
        /// Клиент, общающийся с сервером.
        /// </summary>
        public ISyncIrbisProvider Connection { get; }

        /// <summary>
        /// Omit deleted records?
        /// </summary>
        public bool OmitDeletedRecords { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ReaderManager"/> class.
        /// </summary>
        public ReaderManager
            (
                ISyncIrbisProvider connection
            )
        {
            Connection = connection;
        }

        #endregion

        #region Private members

        private void HandleBatchRead(object? sender, EventArgs eventArgs) =>
            BatchRead?.Invoke(sender, eventArgs);

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива всех (не удалённых) читателей из базы данных.
        /// </summary>
        public ReaderInfo[] GetAllReaders
            (
                string database
            )
        {
            var result = new List<ReaderInfo>
                (
                    Connection.GetMaxMfn() + 1
                );

            var batch = BatchRecordReader.WholeDatabase
                (
                    Connection,
                    database,
                    500
                );

            var batch2 = batch as BatchRecordReader;
            if (!ReferenceEquals(batch2, null))
            {
                batch2.BatchRead += HandleBatchRead;
            }

            foreach (var record in batch)
            {
                if (!ReferenceEquals(record, null))
                {
                    if (OmitDeletedRecords && record.Deleted)
                    {
                        continue;
                    }

                    var reader = ReaderInfo.Parse(record);
                    result.Add(reader);
                }
            }

            if (!ReferenceEquals(batch2, null))
            {
                batch2.BatchRead -= HandleBatchRead;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение массива всех (не удалённых) читателей из базы данных.
        /// </summary>
        public ReaderInfo[] GetReaders
            (
                string database,
                IEnumerable<int> mfns
            )
        {
            var result = new List<ReaderInfo>();

            var batch = new BatchRecordReader
                (
                    Connection,
                    database,
                    500,
                    mfns
                );

            batch.BatchRead += HandleBatchRead;
            foreach (var record in batch)
            {
                if (!ReferenceEquals(record, null))
                {
                    if (OmitDeletedRecords && record.Deleted)
                    {
                        continue;
                    }

                    var reader = ReaderInfo.Parse(record);
                    result.Add(reader);
                }
            }

            batch.BatchRead -= HandleBatchRead;

            return result.ToArray();
        }

        /// <summary>
        /// Получение записи читателя по его идентификатору.
        /// </summary>
        public ReaderInfo? GetReader
            (
                string ticket
            )
        {
            /*
            var record = Connection.SearchReadOneRecord
                (
                    "{0}{1}",
                    ReaderIdentifier,
                    ticket
                );
            if (ReferenceEquals(record, null))
            {
                return null;
            }

            var result = ReaderInfo.Parse(record);

            return result;
            */

            return null;
        }

        /// <summary>
        /// Поиск читателя, соответствующего выражению.
        /// </summary>
        public ReaderInfo? FindReader
            (
                string format,
                params object[] args
            )
        {
            /*
            var record = Connection.SearchReadOneRecord(format, args);
            if (ReferenceEquals(record, null))
            {
                return null;
            }

            var result = ReaderInfo.Parse(record);

            return result;
            */

            return null;
        }

        /// <summary>
        /// Merge readers (i. e. from some databases).
        /// </summary>
        public static List<ReaderInfo> MergeReaders
            (
                IEnumerable<ReaderInfo> readers
            )
        {
            var grouped = readers
                .Where(r => !string.IsNullOrEmpty(r.Ticket))
                .GroupBy(r => r.Ticket);

            var result = new List<ReaderInfo>();

            foreach (var grp in grouped)
            {
                var first = grp.First();
                first.Visits = grp
                    .SelectMany(r => r.Visits ?? Array.Empty<VisitInfo>())
                    .OrderBy(v => v.DateGivenString)
                    .ToArray();

                first.Registrations = grp
                    .SelectMany(r => r.Registrations ?? Array.Empty<ReaderRegistration>())
                    .OrderBy(r => r.DateString)
                    .ToArray();

                first.Enrollment = grp
                    .SelectMany(r => r.Enrollment ?? Array.Empty<ReaderRegistration>())
                    .OrderBy(r => r.DateString)
                    .ToArray();

                first.Marked = grp.Any(r => r.Marked);
                result.Add(first);
            }

            return result;
        }

        #endregion

    } // class ReaderManager

} // namespace ManagedIrbis.Readers
