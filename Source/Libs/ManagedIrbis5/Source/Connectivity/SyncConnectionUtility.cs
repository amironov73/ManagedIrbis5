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
using System.Diagnostics;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;

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
                this ISyncProvider connection,
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
            var prepared = IrbisFormat.PrepareFormat(format);
            query.AddAnsi(prepared);
            query.Add(1);
            query.Add(mfn);
            var response = connection.ExecuteSync(query);
            if (response is null)
            {
                return null;
            }

            response.CheckReturnCode();
            var result = response.ReadRemainingUtfText().TrimEnd();

            return result;
        } // method FormatRecord

        public static string? FormatRecord
            (
                this ISyncProvider conneciton,
                string format,
                int mfn
            )
        {
            throw new NotImplementedException();
        } // method FormatRecord

        public static string? FormatRecord
            (
                this ISyncProvider conneciton,
                string format,
                Record record
            )
        {
            throw new NotImplementedException();
        } // method FormatRecord

        public static string[] FormatRecords
            (
                this ISyncProvider conneciton,
                int[] mfns,
                string format
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Форматирование указанной записи.
        /// </summary>
        public static Task<string?> FormatRecordAsync
            (
                this SyncConnection connection,
                string format,
                Record record
            )
        {
            throw new NotImplementedException();
        } // method FormatRecord

        /// <summary>
        /// Чтение записи с сервера.
        /// </summary>
        public static Record? ReadRecord
            (
                this ISyncProvider connection,
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Database = connection.Database,
                Mfn = mfn
            };

            return connection.ReadRecord(parameters);
        } // method ReadRecord

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
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
            if (response is null
                || !response.CheckReturnCode())
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
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
        public static int SearchCount
            (
                this SyncConnection connection,
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
        }

        #endregion

    } // class SyncConnectionUtility

} // namespace ManagedIrbis
