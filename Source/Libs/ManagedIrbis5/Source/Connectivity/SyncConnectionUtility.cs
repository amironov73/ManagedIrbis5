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

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Синхронные методы для подключения к серверу ИРБИС64.
    /// </summary>
    public static class SyncConnectionUtility
    {
        #region Public methods

        /// <summary>
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="database">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешности завершения операции.</returns>
        public static bool ActualizeDatabase
            (
                this SyncConnection connection,
                string? database = default
            )
        {
            return connection.ActualizeRecord
                (
                    new() { Database = database, Mfn = 0 }
                );

        } // method ActualizeDatabase

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

            using var query = new SyncQuery(connection, CommandCode.FormatRecord);
            query.AddAnsi(connection.Database);
            query.AddFormat(format);
            query.Add(1);
            query.Add(mfn);
            var response = connection.ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = response.ReadRemainingUtfText().TrimEnd();

            return result;

        } // method FormatRecord

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

            using var query = new SyncQuery(connection, CommandCode.FormatRecord);
            query.AddAnsi(connection.Database);
            query.AddFormat(format);
            query.Add(-2);
            query.AddUtf(record.Encode());
            var response = connection.ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = response.ReadRemainingUtfText().TrimEnd();

            return result;

        } // method FormatRecord

        /// <summary>
        /// Форматирование записей по их MFN.
        /// </summary>
        public static string[] FormatRecords
            (
                this ISyncProvider conneciton,
                int[] mfns,
                string format
            )
        {
            throw new NotImplementedException();

        } // method FormatRecords

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

            if (string.IsNullOrEmpty(specification))
            {
                return Array.Empty<string>();
            }

            var response = connection.ExecuteSync(CommandCode.ListFiles, specification);

            return ListFiles(response);

        } // method ListFiles

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
                var files = IrbisText.SplitIrbisToLines(line);
                foreach (var file1 in files)
                {
                    if (!string.IsNullOrEmpty(file1))
                    {
                        foreach (var file2 in file1.Split(IrbisText.WindowsDelimiter))
                        {
                            if (!string.IsNullOrEmpty(file2))
                            {
                                result.Add(file2);
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
                        .FromItem(connection.ReadRecord(mfns[0]))
                        .NonNullItems()
                        .ToArray();
            }


            // TODO: implement

            return Array.Empty<Record>();

        } // method ReadRecords

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="startTerm">Параметры терминов.</param>
        /// <param name="numberOfTerms">Максимальное число терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public static Term[]? ReadTerms
            (
                this SyncConnection connection,
                string startTerm,
                int numberOfTerms
            )
        {
            var parameters = new TermParameters
            {
                Database = connection.Database,
                StartTerm = startTerm,
                NumberOfTerms = numberOfTerms
            };

            return connection.ReadTerms(parameters);

        } // method ReadTerms

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
            if (!connection.CheckProviderState())
            {
                return Array.Empty<int>();
            }

            using var query = new SyncQuery(connection, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression
            };
            parameters.Encode(connection, query);
            var response = connection.ExecuteSync(query);
            if (!response.IsGood())
            {
                return Array.Empty<int>();
            }

            return FoundItem.ParseMfn(response);

        } // method Search

        /// <summary>
        /// Упрощенный поиск.
        /// </summary>
        public static int[] Search
            (
                this ISyncProvider connection,
                string expression
            )
        {
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression
            };
            var lines = connection.Search(parameters);
            var result = FoundItem.ToMfn(lines);

            return result;

        } // method Search

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

            using var query = new SyncQuery(connection, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression,
                FirstRecord = 0
            };
            parameters.Encode(connection, query);
            var response = connection.ExecuteSync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return -1;
            }

            return response.ReadInteger();

        } // method SearchCount

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

            var found = connection.Search(expression);
            var result = new List<Record>(found.Length);
            foreach (var mfn in found)
            {
                var record = connection.ReadRecord(mfn);
                if (record is not null)
                {
                    result.Add(record);
                }
            }

            return result.ToArray();

        } // method SearchRead

        /// <summary>
        /// Сохранение/обновление записи в базе данных.
        /// </summary>
        public static bool WriteRecord
            (
                this ISyncProvider connection,
                IRecord record,
                bool actualize = true,
                bool lockRecord = false,
                bool dontParse = false
            )
        {
            var parameters = new WriteRecordParameters
                {
                    Record = record,
                    Actualize = actualize,
                    Lock = lockRecord,
                    DontParse = dontParse
                };

            return connection.WriteRecord(parameters);

        } // method WriteRecord

        #endregion

    } // class SyncConnectionUtility

} // namespace ManagedIrbis
