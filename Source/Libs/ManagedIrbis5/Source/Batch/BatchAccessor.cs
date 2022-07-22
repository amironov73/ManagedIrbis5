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

/* BatchAccessor.cs -- чтение записей большими блоками в параллель
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using AM;

using ManagedIrbis.ImportExport;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Batch;

/// <summary>
/// Чтение записей большими блоками в параллель.
/// </summary>
public sealed class BatchAccessor
{
    #region Properties

    /// <summary>
    /// Throw <see cref="IrbisException"/>
    /// when empty record received/decoded.
    /// </summary>
    public static bool ThrowOnEmptyRecord { get; set; }

    /// <summary>
    /// Connection.
    /// </summary>
    public ISyncProvider Connection { get; }

    #endregion

    #region Construction

    static BatchAccessor()
    {
        ThrowOnEmptyRecord = true;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BatchAccessor
        (
            ISyncProvider connection
        )
    {
        Sure.NotNull (connection);

        Connection = connection;
    }

    #endregion

    #region Private members

    private static void _ThrowIfEmptyRecord
        (
            Record record,
            string line
        )
    {
        if (ThrowOnEmptyRecord && record.Fields.Count == 0)
        {
            Magna.Logger.LogError
                (
                    nameof (BatchAccessor) + "::" + nameof (_ThrowIfEmptyRecord)
                    + ": empty record detected"
                );

            /*
            byte[] bytes = Encoding.UTF8.GetBytes(line);
            string dump = IrbisNetworkUtility.DumpBytes(bytes);
            */
            var message = "Empty record detected in BatchAccessor";
            var exception = new IrbisException (message);
            /*
            var attachment = new BinaryAttachment
                (
                    "response",
                    bytes
                );
            exception.Attach(attachment);

            */
            throw exception;
        }
    }

    private BlockingCollection<Record>? _records;

    private void _ParseRecord
        (
            string line,
            string database
        )
    {
        if (!string.IsNullOrEmpty (line))
        {
            var result = new Record
            {
                Database = database
            };

            result = ProtocolText.ParseResponseForAllFormat
                (
                    line,
                    result
                );

            if (!ReferenceEquals (result, null))
            {
                _ThrowIfEmptyRecord (result, line);

                if (!result.Deleted)
                {
                    result.Modified = false;
                    _records?.Add (result);
                }
            }
        }
    }

    private void _ParseRecord<T>
        (
            string line,
            string database,
            Func<Record, T> func,
            BlockingCollection<T> collection
        )
    {
        if (!string.IsNullOrEmpty (line))
        {
            var record = new Record
            {
                Database = database
            };

            record = ProtocolText.ParseResponseForAllFormat
                (
                    line,
                    record
                );

            if (record is { Deleted: false })
            {
                var result = func (record);

                collection.Add (result);
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Параллельное чтение множества записей.
    /// </summary>
    public Record[] ReadRecords
        (
            string? database,
            IEnumerable<int> mfnList
        )
    {
        Sure.NotNull (database ??= Connection.Database);

        var array = mfnList.ToArray();

        if (array.Length == 0)
        {
            return Array.Empty<Record>();
        }

        /*

        if (array.Length == 1)
        {
            int mfn = array[0];

            Record record = Connection.ReadRecord
                (
                    database,
                    mfn,
                    false,
                    null
                );

            return new[] { record };
        }

        using (_records = new BlockingCollection<Record>(array.Length))
        {
            int[][] slices = array.Chunk(1000).ToArray();

            foreach (int[] slice in slices)
            {
                if (slice.Length == 1)
                {
                    Record record = Connection.ReadRecord
                        (
                            database: database,
                            mfn: slice[0],
                            lockFlag: false,
                            format: null
                        );

                    _records.Add(record);
                }
                else
                {
                    FormatCommand command = new FormatCommand
                    {
                        Database = database,
                        FormatSpecification = IrbisFormat.All
                    };
                    command.MfnList.AddRange(slice);

                    Connection.ExecuteCommand(command);

                    string[] lines = command.FormatResult
                        .ThrowIfNullOrEmpty(nameof(command.FormatResult));

                    Debug.Assert
                        (
                            lines.Length == slice.Length,
                            Resources.BatchAccessor_SomeRecordsNotRetrieved
                        );

                    Parallel.ForEach
                        (
                            lines,
                            line => _ParseRecord(line, database)
                        );
                }
            }

            _records.CompleteAdding();

            return _records.ToArray();
        }

        */

        return Array.Empty<Record>();
    }

    /// <summary>
    /// Read and transform multiple records.
    /// </summary>
    public T[] ReadRecords<T>
        (
            string? database,
            IEnumerable<int> mfnList,
            Func<Record, T> func
        )
    {
        (database ??= Connection.Database).ThrowIfNull();

        var array = mfnList.ToArray();

        if (array.Length == 0)
        {
            return Array.Empty<T>();
        }

        /*

        if (array.Length == 1)
        {
            int mfn = array[0];

            Record record = Connection.ReadRecord
                (
                    database: database,
                    mfn: mfn,
                    lockFlag: false,
                    format: null
                );

            T result1 = func(record);

            return new[] { result1 };
        }

        using (BlockingCollection<T> collection
            = new BlockingCollection<T>(array.Length))
        {

            int[][] slices = array.Chunk(1000).ToArray();

            foreach (int[] slice in slices)
            {
                if (slice.Length == 1)
                {
                    Record record = Connection.ReadRecord
                        (
                            database,
                            slice[0],
                            false,
                            null
                        );

                    _records.Add(record);
                }
                else
                {
                    FormatCommand command = new FormatCommand
                    {
                        Database = database,
                        FormatSpecification = IrbisFormat.All
                    };
                    command.MfnList.AddRange(slice);

                    Connection.ExecuteCommand(command);

                    string[] lines = command.FormatResult
                        .ThrowIfNullOrEmpty(nameof(command.FormatResult));

                    Debug.Assert
                        (
                            lines.Length == slice.Length,
                            Resources.BatchAccessor_SomeRecordsNotRetrieved
                        );

                    Parallel.ForEach
                        (
                            lines,
                            line => _ParseRecord
                                (
                                    line,
                                    database,
                                    func,

                                    // ReSharper disable once AccessToDisposedClosure
                                    collection
                                )
                        );
                }
            }

            collection.CompleteAdding();

            return collection.ToArray();

        }

        */

        return Array.Empty<T>();
    }

    #endregion
}
