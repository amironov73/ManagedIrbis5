// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SyncConnectionUtility.cs -- синхронные методы для подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Linq;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Синхронные методы для подключения к серверу ИРБИС64.
/// </summary>
public static class SyncConnectionUtility
{
    #region Public methods

    /// <summary>
    /// Форматирование указанной записи по ее MFN.
    /// </summary>
    public static string? FormatRecord
        (
            this SyncConnection connection,
            string format,
            int mfn
        )
    {
        if (!connection.CheckProviderState())
        {
            return null;
        }

        using var query = new SyncQuery (connection, CommandCode.FormatRecord);
        query.AddAnsi (connection.Database);
        query.AddFormat (format);
        query.Add (1);
        query.Add (mfn);
        var response = connection.ExecuteSync (query);
        if (!response.IsGood())
        {
            return null;
        }

        var result = response.ReadRemainingUtfText().TrimEnd();

        return result;
    }

    /// <summary>
    /// Форматирование записи в клиентском представлении.
    /// </summary>
    public static string? FormatRecord
        (
            this SyncConnection connection,
            string format,
            Record record
        )
    {
        if (!connection.CheckProviderState())
        {
            return null;
        }

        if (string.IsNullOrEmpty (format))
        {
            return string.Empty;
        }

        using var query = new SyncQuery (connection, CommandCode.FormatRecord);
        query.AddAnsi (connection.EnsureDatabase (string.Empty));
        query.AddFormat (format);
        query.Add (-2);
        query.AddUtf (record.Encode());
        var response = connection.ExecuteSync (query);
        if (!response.IsGood())
        {
            return null;
        }

        var result = response.ReadRemainingUtfText().TrimEnd();

        return result;
    }

    /// <summary>
    /// Получение списка файлов, соответствующих спецификации.
    /// </summary>
    public static string[]? ListFiles
        (
            this SyncConnection connection,
            string specification
        )
    {
        if (!connection.CheckProviderState())
        {
            return null;
        }

        if (string.IsNullOrEmpty (specification))
        {
            return Array.Empty<string>();
        }

        var response = connection.ExecuteSync (CommandCode.ListFiles, specification);

        return ListFiles (response);
    }

    /// <summary>
    /// Получение списка файлов из ответа сервера.
    /// </summary>
    public static string[]? ListFiles
        (
            Response? response
        )
    {
        if (response is null)
        {
            return null;
        }

        var lines = response.ReadRemainingAnsiLines();
        var result = new List<string>();
        foreach (var line in lines)
        {
            var files = IrbisText.SplitIrbisToLines (line);
            foreach (var file1 in files)
            {
                if (!string.IsNullOrEmpty (file1))
                {
                    foreach (var file2 in file1.Split (IrbisText.WindowsDelimiter))
                    {
                        if (!string.IsNullOrEmpty (file2))
                        {
                            result.Add (file2);
                        }
                    }
                }
            }
        }

        return result.ToArray();
    } // method ListFiles

    /// <summary>
    /// Чтение указанных записей.
    /// </summary>
    public static Record[]? ReadRecords
        (
            this ISyncConnection connection,
            string database,
            IEnumerable<int> batch
        )
    {
        if (!connection.CheckProviderState())
        {
            return null;
        }

        int[] mfns = batch is int[] array ? array : batch.ToArray();

        switch (mfns.Length)
        {
            case 0:
                return Array.Empty<Record>();

            case 1:
                // TODO: use database parameter

                return Sequence
                    .FromItem (connection.ReadRecord (mfns[0]))
                    .NonNullItems()
                    .ToArray();
        }

        var parameters = new FormatRecordParameters
        {
            Database = database,
            Mfns = mfns,
            Format = IrbisFormat.All
        };
        if (!connection.FormatRecords (parameters))
        {
            return null;
        }

        var lines = parameters.Result.AsArray();
        if (lines.Length == 0)
        {
            return null;
        }

        var result = new List<Record> (lines.Length);
        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty (line))
            {
                var converted = IrbisText.SplitIrbisToLines (line);
                if (converted.Length > 3)
                {
                    var record = new Record();
                    record.Decode (converted[1..]);
                    result.Add (record);
                }
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Чтение несколькних текстовых файлов с сервера.
    /// </summary>
    public static string[]? ReadTextFiles
        (
            this ISyncConnection connection,
            FileSpecification[] specifications
        )
    {
        using var query = new SyncQuery (connection, CommandCode.ReadDocument);
        foreach (var specification in specifications)
        {
            query.AddAnsi (specification.ToString());
        }

        var response = connection.ExecuteSync (query);

        return response.IsGood() ? response.ReadRemainingAnsiLines() : null;
    }

    /// <summary>
    /// Упрощенный поиск.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="expression">Выражение для поиска по словарю.</param>
    /// <returns>Массив MFN найденных записей.</returns>
    public static int[] Search
        (
            this ISyncConnection connection,
            string expression
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (expression);

        if (!connection.CheckProviderState())
        {
            return Array.Empty<int>();
        }

        using var query = new SyncQuery (connection, CommandCode.Search);
        var parameters = new SearchParameters
        {
            Database = connection.Database,
            Expression = expression
        };
        parameters.Encode (connection, query);
        var response = connection.ExecuteSync (query);
        if (!response.IsGood())
        {
            return Array.Empty<int>();
        }

        return FoundItem.ParseMfn (response);
    }

    /// <summary>
    /// Поиск всех записей, удовлетворяющих данному запросу,
    /// даже если их окажется больше 32 тысяч.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="expression">Выражение для поиска по словарю.</param>
    /// <returns>Массив MFN найденных записей.</returns>
    /// <remarks>Метод может создать большую нагрузку на сервер!
    /// </remarks>
    public static int[] SearchAll
        (
            this ISyncConnection connection,
            string expression
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (expression);

        if (!connection.CheckProviderState())
        {
            return Array.Empty<int>();
        }

        var result = new List<int>();
        var first = 1;
        var expected = 0;
        // var iteration = 0;
        while (true)
        {
            using var query = new SyncQuery (connection, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression,
                NumberOfRecords = 32_000,
                FirstRecord = first
            };
            parameters.Encode (connection, query);
            var response = connection.ExecuteSync (query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return Array.Empty<int>();
            }

            // response.DebugAnsi ($"search_{iteration++:0000}.txt");
            if (first == 1)
            {
                expected = response.RequireInt32();
                if (expected == 0)
                {
                    break;
                }

                result.EnsureCapacity (expected);
            }
            else
            {
                response.RequireInt32();
            }

            while (!response.EOT)
            {
                var line = response.ReadAnsi();
                if (string.IsNullOrEmpty (line))
                {
                    break;
                }

                var mfn = FastNumber.ParseInt32 (line);
                result.Add (mfn);
                ++first;
            }

            if (first >= expected)
            {
                break;
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Определение количества записей, удовлетворяющих
    /// заданному запросу.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="expression">Выражение для поиска по словарю.</param>
    /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
    public static int SearchCount
        (
            this ISyncConnection connection,
            string expression
        )
    {
        if (!connection.CheckProviderState())
        {
            return -1;
        }

        using var query = new SyncQuery (connection, CommandCode.Search);
        var parameters = new SearchParameters
        {
            Database = connection.Database,
            Expression = expression,
            FirstRecord = 0
        };
        parameters.Encode (connection, query);
        var response = connection.ExecuteSync (query);
        if (response is null
            || !response.CheckReturnCode())
        {
            return -1;
        }

        return response.ReadInteger();
    }

    /// <summary>
    /// Поиск с последующим расформатированием записей.
    /// </summary>
    public static string?[]? SearchFormat
        (
            this ISyncConnection connection,
            string expression,
            string format
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (expression);
        Sure.NotNullNorEmpty (format);

        if (!connection.CheckProviderState())
        {
            return null;
        }

        // TODO: сделать оптимально

        var found = connection.Search (expression);
        var result = new List<string?> (found.Length);
        foreach (var mfn in found)
        {
            var record = connection.FormatRecord (format, mfn);
            if (record is not null)
            {
                result.Add (record);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Поиск с последующим чтением записей.
    /// </summary>
    public static Record[]? SearchRead
        (
            this ISyncConnection connection,
            string expression
        )
    {
        if (!connection.CheckProviderState())
        {
            return null;
        }

        // TODO: сделать оптимально

        var found = connection.Search (expression);
        var result = new List<Record> (found.Length);
        foreach (var mfn in found)
        {
            var record = connection.ReadRecord (mfn);
            if (record is not null)
            {
                result.Add (record);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Сохранение записей на сервере.
    /// </summary>
    public static bool WriteRecords
        (
            this ISyncConnection connection,
            IEnumerable<Record> records,
            bool lockFlag = false,
            bool actualize = true,
            bool dontParse = true
        )
    {
        // TODO: сделать IRecord

        if (!connection.CheckProviderState())
        {
            return false;
        }

        var query = new SyncQuery (connection, CommandCode.SaveRecordGroup);
        query.Add (lockFlag);
        query.Add (actualize);
        var recordList = new List<Record>();
        foreach (var record in records)
        {
            var line = connection.EnsureDatabase (record.Database)
                       + IrbisText.IrbisDelimiter
                       + ProtocolText.EncodeRecord (record);
            query.AddUtf (line);
            recordList.Add (record);
        }

        if (recordList.Count == 0)
        {
            return true;
        }

        var response = connection.ExecuteSync (query);
        if (!response.IsGood())
        {
            return false;
        }

        if (!dontParse)
        {
            foreach (var record in recordList)
            {
                ProtocolText.ParseResponseForWriteRecords (response, record);
            }
        }

        return true;
    }

    /// <summary>
    /// Запись/обновление файлов на сервере.
    /// </summary>
    public static bool WriteTextFiles
        (
            this SyncConnection connection,
            IEnumerable<FileSpecification> specifications
        )
    {
        if (!connection.CheckProviderState())
        {
            return false;
        }

        var query = new SyncQuery (connection, CommandCode.ReadDocument);
        var count = 0;
        foreach (var specification in specifications)
        {
            query.AddAnsi (specification.ToString());
            ++count;
        }

        if (count is 0)
        {
            return true;
        }

        var response = connection.ExecuteSync (query);

        return response is not null;
    }

    #endregion
}
