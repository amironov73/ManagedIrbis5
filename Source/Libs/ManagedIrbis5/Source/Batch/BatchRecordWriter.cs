// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable DelegateSubtraction
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Local
// ReSharper disable RedundantAssignment
// ReSharper disable ReturnValueOfPureMethodIsNotUsed
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

#pragma warning disable 649

/* BatchRecordWriter.cs -- batch record writer
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// Batch writer for <see cref="Record"/>.
    /// </summary>
    public sealed class BatchRecordWriter
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Raised on batch write.
        /// </summary>
        public event EventHandler? BatchWrite;

        #endregion

        #region Properties

        /// <summary>
        /// Actualize records when writing.
        /// </summary>
        public bool Actualize { get; set; }

        /// <summary>
        /// Capacity.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Connection.
        /// </summary>
        public ISyncProvider Connection { get; private set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        /// Total number of records written.
        /// </summary>
        public int RecordsWritten { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BatchRecordWriter
            (
                ISyncProvider connection,
                string database,
                int capacity
            )
        {
            if (capacity < 1)
            {
                Magna.Error
                    (
                        "BatchRecordWriter::Constructor: "
                        + "capacity="
                        + capacity
                    );

                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            Connection = connection;
            Database = database;
            Capacity = capacity;
            Actualize = true;
            _buffer = new List<Record>(capacity);
            _syncRoot = new object();
        }

        #endregion

        #region Private members

        private readonly List<Record> _buffer;

        private readonly object _syncRoot;

        #endregion

        #region Public methods

        /// <summary>
        /// Add many records.
        /// </summary>
        public BatchRecordWriter AddRange
            (
                IEnumerable<Record> records
            )
        {
            lock (_syncRoot)
            {
                foreach (var record in records)
                {
                    Append(record);
                }
            }

            return this;
        }

        /// <summary>
        /// Append one record.
        /// </summary>
        public BatchRecordWriter Append
            (
                Record record
            )
        {
            lock(_syncRoot)
            {
                _buffer.Add(record);
                if (_buffer.Count >= Capacity)
                {
                    Flush();
                }
            }

            return this;
        }

        /// <summary>
        /// Flush the buffer.
        /// </summary>
        public BatchRecordWriter Flush()
        {
            lock(_syncRoot)
            {
                if (_buffer.Count != 0)
                {
                    try
                    {
                        /*

                        Connection.PushDatabase(Database);
                        Connection.WriteRecords
                            (
                                _buffer.ToArray(),
                                false,
                                Actualize
                            );

                        */

                        RecordsWritten += _buffer.Count;
                    }
                    finally
                    {
                        /*
                        Connection.PopDatabase();
                        */
                    }

                    BatchWrite.Raise(this);
                }

                _buffer.Clear();
            }

            return this;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Flush();
        }

        #endregion

    } // class BatchRecordWriter

} // namespace ManagedIrbis.Batch
