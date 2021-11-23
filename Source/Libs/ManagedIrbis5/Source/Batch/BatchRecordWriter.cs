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

// field is never assigned to, and will have its default value

#pragma warning disable 649

/* BatchRecordWriter.cs -- накапливает записи для пакетного сохранения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Batch
{
    /// <summary>
    /// Накапливает записи для пакетного сохранения на сервере.
    /// </summary>
    public sealed class BatchRecordWriter
        : IDisposable
    {
        #region Events

        /// <summary>
        /// Событие возникает в момент пакетного сохранения.
        /// </summary>
        public event EventHandler? BatchWrite;

        #endregion

        #region Properties

        /// <summary>
        /// Актуализировать записи при сохранении?
        /// </summary>
        public bool Actualize { get; set; }

        /// <summary>
        /// Емкость (количество записей, по достижении которого
        /// происходит автоматическая отсылка на сервер).
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        public ISyncProvider Connection { get; }

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// Общее количество сохраненных записей.
        /// </summary>
        public int RecordsWritten { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
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
                        nameof (BatchRecordWriter) + "::Constructor"
                                                   + ": capacity="
                                                   + capacity
                    );

                throw new ArgumentOutOfRangeException (nameof (capacity));
            }

            Connection = connection;
            Database = database;
            Capacity = capacity;
            Actualize = true;
            _buffer = new List<Record> (capacity);
            _syncRoot = new object();
        }

        #endregion

        #region Private members

        private readonly List<Record> _buffer;

        private readonly object _syncRoot;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление многих записей одновременно.
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
                    Append (record);
                }
            }

            return this;
        }

        /// <summary>
        /// Добавление одной записи.
        /// </summary>
        public BatchRecordWriter Append
            (
                Record record
            )
        {
            lock (_syncRoot)
            {
                _buffer.Add (record);
                if (_buffer.Count >= Capacity)
                {
                    Flush();
                }
            }

            return this;
        }

        /// <summary>
        /// Принудительная отсылка записей на сервер.
        /// </summary>
        public BatchRecordWriter Flush()
        {
            lock (_syncRoot)
            {
                if (_buffer.Count != 0)
                {
                    var savedDatabase = Connection.Database;

                    try
                    {
                        var parameters = new WriteRecordParameters
                        {
                            Records = _buffer.ToArray(),
                            Actualize = Actualize,
                            Lock = false,
                            DontParse = true
                        };
                        Connection.Database = Database;
                        Connection.WriteRecord (parameters);

                        RecordsWritten += _buffer.Count;
                    }
                    finally
                    {
                        Connection.Database = savedDatabase;
                    }

                    BatchWrite.Raise (this);
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
    }
}
