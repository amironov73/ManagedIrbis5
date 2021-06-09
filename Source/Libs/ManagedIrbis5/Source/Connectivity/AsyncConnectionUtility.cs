// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsyncConnectionUtility.cs -- асинхронные методы для подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AM.Collections;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Асинхронные методы для подключения к серверу ИРБИС64.
    /// </summary>
    public static class AsyncConnectionUtility
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
        public static async Task<bool> ActualizeDatabaseAsync
            (
                this IAsyncProvider connection,
                string? database = default
            )
        {
            return await connection.ActualizeRecordAsync
                (
                    new() { Database = database, Mfn = 0 }
                );
        } // method ActualizeDatabaseAsync

        /// <summary>
        /// Форматирование указанной записи по ее MFN.
        /// </summary>
        public static async Task<string?> FormatRecordAsync
            (
                this AsyncConnection connection,
                string format,
                int mfn
            )
        {
            if (!connection.CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(connection, CommandCode.FormatRecord);
            query.AddAnsi(connection.Database);
            var prepared = IrbisFormat.PrepareFormat(format);
            query.AddAnsi(prepared);
            query.Add(1);
            query.Add(mfn);
            var response = await connection.ExecuteAsync(query);
            if (response is null)
            {
                return null;
            }

            response.CheckReturnCode();
            var result = response.ReadRemainingUtfText().TrimEnd();

            return result;
        } // method FormatRecordAsync

        /// <summary>
        /// Форматирование указанной записи.
        /// </summary>
        public static Task<string?> FormatRecordAsync
            (
                this AsyncConnection connection,
                string format,
                Record record
            )
        {
            throw new NotImplementedException();
        } // method FormatRecordAsync

        /// <summary>
        /// Асинхронное перечисление файлов.
        /// </summary>
        public static async Task<string[]?> ListFilesAsync
            (
                this AsyncConnection connection,
                FileSpecification specification
            )
        {
            if (!connection.CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(connection, CommandCode.ListFiles);
            query.AddAnsi(specification.ToString());

            var response = await connection.ExecuteAsync(query);
            if (response is null)
            {
                return null;
            }

            // TODO: вынести повторяющийся код в отдельный метод
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

        } // method ListFilesAsync

        /// <summary>
        /// Чтение библиографической записи с сервера.
        /// </summary>
        public static async Task<Record?> ReadRecordAsync
            (
                this IAsyncProvider connection,
                int mfn
            )
            =>
            await connection.ReadRecordAsync<Record>
                (
                    new ()
                    {
                        Database = connection.Database,
                        Mfn = mfn
                    }
                );

        /// <summary>
        /// Чтение библиографической записи с сервера.
        /// </summary>
        public static async Task<T?> ReadRecordAsync<T>
            (
                this IAsyncProvider connection,
                int mfn
            )
            where T: class, IRecord, new()
            =>
            await connection.ReadRecordAsync<T>
                (
                    new ()
                    {
                        Database = connection.Database,
                        Mfn = mfn
                    }
                );

        /// <summary>
        /// Чтение всех терминов с указанным префиксом.
        /// </summary>
        public static async Task<Term[]> ReadAllTermsAsync
            (
                this IAsyncProvider connection,
                string prefix
            )
        {
            if (!connection.CheckProviderState())
            {
                return Array.Empty<Term>();
            }

            prefix = prefix.ToUpperInvariant();
            var result = new List<Term>();
            var startTerm = prefix;
            var flag = true;
            while (flag)
            {
                var terms = await connection.ReadTermsAsync(startTerm, 1024);
                if (terms.IsNullOrEmpty())
                {
                    break;
                }

                var startIndex = 0;
                if (result.Count != 0)
                {
                    var lastTerm = result[^1];
                    var firstTerm = terms![0];
                    if (firstTerm.Text == lastTerm.Text)
                    {
                        startIndex = 1;
                    }
                }

                for (var i = startIndex; i < terms!.Length; i++)
                {
                    var term = terms[i];
                    var text = term.Text;
                    if (string.IsNullOrEmpty(text))
                    {
                        break;
                    }

                    if (!text.StartsWith(prefix))
                    {
                        flag = false;
                        break;
                    }

                    result.Add(term);
                }
            }

            return result.ToArray();

        } // method ReadAllTermsAsync


        /// <summary>
        /// Чтение постингов указанного термина.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="term">Термин.</param>
        /// <param name="numberOfPostings">Максимальное количество постингов</param>
        /// <returns>Массив прочитанных постингов.</returns>
        public static async Task<TermPosting[]?> ReadPostingsAsync
            (
                this IAsyncProvider connection,
                string term,
                int numberOfPostings
            )
        {
            var parameters = new PostingParameters
            {
                Database = connection.Database,
                Terms = new[] { term },
                NumberOfPostings = numberOfPostings
            };

            return await connection.ReadPostingsAsync(parameters);

        }

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="startTerm">Начальный термин.</param>
        /// <param name="numberOfTerms">Максимальное число терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public static async Task<Term[]?> ReadTermsAsync
            (
                this IAsyncProvider connection,
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

            return await connection.ReadTermsAsync(parameters);

        } // method ReadTermsAsync


        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Массив MFN найденных записей.</returns>
        public static async Task<int[]> SearchAsync
            (
                this AsyncConnection connection,
                string expression
            )
        {
            if (!connection.CheckProviderState())
            {
                return Array.Empty<int>();
            }

            var query = new AsyncQuery(connection, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression
            };
            parameters.Encode(connection, query);
            var response = await connection.ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return Array.Empty<int>();
            }

            return FoundItem.ParseMfn(response);

        } // method SearchAsync

        /// <summary>
        /// Определение количества записей, удовлетворяющих
        /// заданному запросу.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
        public static async Task<int> SearchCountAsync
            (
                this AsyncConnection connection,
                string expression
            )
        {
            if (!connection.CheckProviderState())
            {
                return -1;
            }

            var query = new AsyncQuery(connection, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression,
                FirstRecord = 0
            };
            parameters.Encode(connection, query);
            var response = await connection.ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return -1;
            }

            return response.ReadInteger();

        } // method SearchCountAsync

        /// <summary>
        /// Поиск записей на сервере.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="expression"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Task<Record[]> SearchReadAsync
            (
                this AsyncConnection connection,
                string expression,
                int limit = 0
            )
        {
            throw new NotImplementedException();

        } // method SearchReadAsync

        /// <summary>
        /// Поиск записей на сервере.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Task<Record?> SearchSingleRecordAsync
            (
                this AsyncConnection connection,
                string expression
            )
        {
            throw new NotImplementedException();

        } // method SearchSingleRecordAsync

        /// <summary>
        /// Асинхронное сохранение записей.
        /// </summary>
        public static Task<Record[]> WriteRecordsAsync
            (
                this AsyncConnection connection,
                Record[] records,
                bool lockFlag = false,
                bool actualize = true
            )
        {
            throw new NotImplementedException();

        } // method WriteRecordsAsync

        /// <summary>
        /// Запись/обновление файлов на сервере.
        /// </summary>
        public static async Task<bool> WriteTextFilesAsync
            (
                this AsyncConnection connection,
                FileSpecification[] specifications
            )
        {
            if (!connection.CheckProviderState())
            {
                return false;
            }

            var query = new AsyncQuery(connection, CommandCode.ReadDocument);
            foreach (var specification in specifications)
            {
                query.AddAnsi(specification.ToString());
            }

            var response = await connection.ExecuteAsync(query);

            return response is not null;

        } // method WriteFileAsync

        #endregion

    } // class ConenctionAsyncMethods

} // namespace ManagedIrbis
