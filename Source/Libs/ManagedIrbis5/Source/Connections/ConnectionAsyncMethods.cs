// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ConnectionAsyncMethods.cs -- асинхронные методы для подключения
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
    /// Асинхронные методы для подключения к серверу ИРБИС64.
    /// </summary>
    public static class ConnectionAsyncMethods
    {
        #region Public methods

        /// <summary>
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        public static async Task<bool> ActualizeDatabaseAsync
            (
                this IIrbisConnection connection,
                string? database = default
            )
        {
            var result = await connection.ActualizeRecordAsync(database, 0);

            return result;
        } // method ActualizeDatabase

        /// <summary>
        /// Актуализация записи с указанным MFN.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN, подлежащий актуализации.</param>
        /// <returns>Признак успешности операции.</returns>
        public static async Task<bool> ActualizeRecordAsync
            (
                this IIrbisConnection connection,
                string? database,
                int mfn
            )
        {
            database ??= connection.Database;
            var response = await connection.ExecuteAsync
                (
                    CommandCode.ActualizeRecord,
                    database,
                    mfn
                );

            return response is not null;
        } // method ActualizeRecordAsync

        /// <summary>
        /// Создание базы данных.
        /// </summary>
        /// <param name="database">Имя создаваемой базы.</param>
        /// <param name="description">Описание в свободной форме.</param>
        /// <param name="readerAccess">Читатель будет иметь доступ?</param>
        /// <returns>Признак успешности операции.</returns>
        public static async Task<bool> CreateDatabaseAsync
            (
                this IIrbisConnection connection,
                string database,
                string description,
                bool readerAccess = true
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            var query = new Query(connection, CommandCode.CreateDatabase);
            query.AddAnsi(database);
            query.AddAnsi(description);
            query.Add(readerAccess ? 1 : 0);
            var response = await connection.ExecuteAsync(query);
            if (response is null || !response.CheckReturnCode())
            {
                return false;
            }

            return true;
        } // method CreateDatabaseAsync


        /// <summary>
        /// Создание словаря в указанной базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        public static async Task<bool> CreateDictionaryAsync
            (
                this IIrbisConnection connection,
                string? database = default
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            database ??= connection.Database;
            var query = new Query(connection, CommandCode.CreateDictionary);
            query.AddAnsi(database);
            var response = await connection.ExecuteAsync(query);
            if (response is null || !response.CheckReturnCode())
            {
                return false;
            }

            return true;
        } // method CreateDictionaryAsync

        /// <summary>
        /// Удаление указанной базы данных.
        /// </summary>
        /// <param name="database">Имя удаляемой базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        public static async Task<bool> DeleteDatabaseAsync
            (
                this IIrbisConnection connection,
                string? database = default
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            database ??= connection.Database;
            var query = new Query(connection, CommandCode.DeleteDatabase);
            query.AddAnsi(database);
            var response = await connection.ExecuteAsync(query);
            if (response is null || !response.CheckReturnCode())
            {
                return false;
            }

            return true;
        } // method DeleteDatabaseAsync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="args">Опциональные параметры команды
        /// (в кодировке ANSI).</param>
        /// <returns>Ответ сервера.</returns>
        public static async Task<Response?> ExecuteAsync
            (
                this IIrbisConnection connection,
                string command,
                params object[] args
            )
        {
            if (!connection.CheckConnection())
            {
                return null;
            }

            var query = new Query(connection, command);
            foreach (var arg in args)
            {
                query.AddAnsi(arg.ToString());
            }

            var result = await connection.ExecuteAsync(query);

            return result;
        } // method ExecuteAsync

        /// <summary>
        /// Форматирование записи с указанием её MFN.
        /// </summary>
        /// <param name="format">Спецификация формата.</param>
        /// <param name="mfn">MFN записи.</param>
        /// <returns>Результат расформатирования.</returns>
        public static async Task<string?> FormatRecordAsync
            (
                this IIrbisConnection connection,
                string format,
                int mfn
            )
        {
            if (!connection.CheckConnection())
            {
                return null;
            }

            var query = new Query(connection, CommandCode.FormatRecord);
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
            var result = response.ReadRemainingUtfText();
            if (!string.IsNullOrEmpty(result))
            {
                result = result.TrimEnd();
            }

            return result;
        } // method FormatRecordAsync

        /// <summary>
        /// Полнотекстовый поиск ИРБИС64+.
        /// </summary>
        public static async Task<FullTextResult?> FullTextSearchAsync
            (
                this IIrbisConnection connection,
                SearchParameters searchParameters,
                TextParameters textParameters
            )
        {
            if (!connection.CheckConnection())
            {
                return null;
            }

            var query = new Query(connection, CommandCode.NewFulltextSearch);
            searchParameters.Encode(connection, query);
            textParameters.Encode(connection, query);
            //query.DebugUtf(Console.Out);
            var response = await connection.ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return null;
            }

            var result = new FullTextResult();
            result.Decode(response);

            return result;
        }

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// </summary>
        /// <param name="database">Опциональное имя базы данных
        /// (<c>null</c> означает текущую базу данных).</param>
        /// <returns>Макисмальный MFN.</returns>
        public static async Task<int> GetMaxMfnAsync
            (
                this IIrbisConnection connection,
                string? database = default
            )
        {
            database ??= connection.Database;
            var response = await connection.ExecuteAsync(CommandCode.GetMaxMfn, database);

            return response is null || !response.CheckReturnCode()
                ? 0
                : response.ReturnCode;
        } // method GetMaxMfnAsync

        /// <summary>
        ///
        /// </summary>
        public static async Task<ServerVersion?> GetServerVersionAsync
            (
                this IIrbisConnection connection
            )
        {
            if (!connection.CheckConnection())
            {
                return null;
            }

            var query = new Query(connection, CommandCode.ServerInfo);
            var response = await connection.ExecuteAsync(query);
            if (response is null)
            {
                return null;
            }

            response.CheckReturnCode();
            var result = new ServerVersion();
            result.Parse(response);

            return result;
        } // method GetServerVersionAsync

        /// <summary>
        /// Получение списка файлов на сервере.
        /// </summary>
        public static async Task<string[]> ListFilesAsync
            (
                this IIrbisConnection connection,
                params string[] specifications
            )
        {
            if (!connection.CheckConnection()
                || specifications.Length == 0)
            {
                return Array.Empty<string>();
            }

            var query = new Query(connection, CommandCode.ListFiles);
            foreach (var specification in specifications)
            {
                query.AddAnsi(specification);
            }

            var response = await connection.ExecuteAsync(query);
            if (response is null)
            {
                return Array.Empty<string>();
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
        } // method ListFilesAsync

        /// <summary>
        /// Получение списка процессов на сервере.
        /// </summary>
        public static async Task<ProcessInfo[]> ListProcessesAsync
            (
                this IIrbisConnection connection
            )
        {
            if (!connection.CheckConnection())
            {
                return Array.Empty<ProcessInfo>();
            }

            var query = new Query(connection, CommandCode.GetProcessList);
            var response = await connection.ExecuteAsync(query);
            if (response is null)
            {
                return Array.Empty<ProcessInfo>();
            }

            response.CheckReturnCode();
            var result = ProcessInfo.Parse(response);

            return result;
        } // method ListProcessesAsync

        /// <summary>
        /// Пустая операция.
        /// </summary>
        /// <returns>Признак успешного завершения операции.</returns>
        public static async Task<bool> NopAsync
            (
                this IIrbisConnection connection
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            var response = await connection.ExecuteAsync(CommandCode.Nop);

            return response is not null
                   && response.CheckReturnCode();
        } // method NopAsync


        /// <summary>
        /// Чтение всех терминов с указанным префиксом.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static async Task<Term[]> ReadAllTermsAsync
            (
                this IIrbisConnection connection,
                string prefix
            )
        {
            if (!connection.CheckConnection())
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
                if (terms.Length == 0)
                {
                    break;
                }

                int startIndex = 0;
                if (result.Count != 0)
                {
                    var lastTerm = result[^1];
                    var firstTerm = terms[0];
                    if (firstTerm.Text == lastTerm.Text)
                    {
                        startIndex = 1;
                    }
                }

                for (var i = startIndex; i < terms.Length; i++)
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
        /// Чтение библиографической записи с сервера.
        /// </summary>
        public static async Task<Record?> ReadRecordAsync
            (
                this IIrbisConnection connection,
                int mfn
            )
        {
            if (!connection.CheckConnection())
            {
                return null;
            }

            var query = new Query(connection, CommandCode.ReadRecord);
            query.AddAnsi(connection.Database);
            query.Add(mfn);
            var response = await connection.ExecuteAsync(query);
            if (response is null
                /* || !response.CheckReturnCode(_goodCodesForReadRecord) */ )
            {
                return null;
            }

            var result = new Record
            {
                Database = connection.Database
            };
            result.Decode(response);

            return result;
        } // method ReadRecordAsync

        /// <summary>
        /// Чтение постингов указанного термина.
        /// </summary>
        /// <param name="term">Термин.</param>
        /// <param name="numberOfPostings">Максимальное количество постингов</param>
        /// <returns>Массив прочитанных постингов.</returns>
        public static async Task<TermPosting[]> ReadPostingsAsync
            (
                this IIrbisConnection connection,
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
        /// Чтение постингов указанных терминов.
        /// </summary>
        /// <param name="parameters">Параметры постингов.</param>
        /// <returns>Массив прочитанных постингов.</returns>
        public static async Task<TermPosting[]> ReadPostingsAsync
            (
                this IIrbisConnection connection,
                PostingParameters parameters
            )
        {
            if (!connection.CheckConnection())
            {
                return Array.Empty<TermPosting>();
            }

            var query = new Query(connection, CommandCode.ReadPostings);
            parameters.Encode(connection, query);
            var response = await connection.ExecuteAsync(query);
            if (response is null
                /* || !response.CheckReturnCode(_goodCodesForReadTerms) */)
            {
                return Array.Empty<TermPosting>();
            }

            return TermPosting.Parse(response);
        } // method ReadPostingsAsync

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="startTerm">Параметры терминов.</param>
        /// <param name="numberOfTerms">Максимальное число терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public static async Task<Term[]> ReadTermsAsync
            (
                this IIrbisConnection connection,
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
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="parameters">Параметры терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public static async Task<Term[]> ReadTermsAsync
            (
                this IIrbisConnection connection,
                TermParameters parameters
            )
        {
            if (!connection.CheckConnection())
            {
                return Array.Empty<Term>();
            }

            var command = parameters.ReverseOrder
                ? CommandCode.ReadTermsReverse
                : CommandCode.ReadTerms;
            var query = new Query(connection, command);
            parameters.Encode(connection, query);
            var response = await connection.ExecuteAsync(query);
            if (response is null
                /* || !response.CheckReturnCode(_goodCodesForReadTerms) */)
            {
                return Array.Empty<Term>();
            }

            return Term.Parse(response);
        } // method ReadTermsAsync

        /// <summary>
        /// Чтение указанного текстового файла с сервера.
        /// </summary>
        public static async Task<string?> ReadTextFileAsync
            (
                this IIrbisConnection connection,
                string? specification
            )
        {
            if (!connection.CheckConnection()
                || !string.IsNullOrWhiteSpace(specification))
            {
                return null;
            }

            var query = new Query(connection, CommandCode.ReadDocument);
            query.AddAnsi(specification);
            var response = await connection.ExecuteAsync(query);
            if (response is null)
            {
                return null;
            }

            var result = IrbisText.IrbisToWindows(response.ReadAnsi());

            return result;
        } // method ReadTextFileAsync

        /// <summary>
        /// Пересоздание словаря для указанной базы данных.
        /// </summary>
        /// <param name="database">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public static async Task<bool> ReloadDictionaryAsync
            (
                this IIrbisConnection connection,
                string? database = default
            )
        {
            var response = await connection.ExecuteAsync
                (
                    CommandCode.ReloadDictionary,
                    database ?? connection.Database
                );

            return response is not null && response.CheckReturnCode();
        } // method ReloadDictionaryAsync

        /// <summary>
        /// Пересоздание мастер-файла для указанной базы данных.
        /// </summary>
        /// <param name="database">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns></returns>
        public static async Task<bool> ReloadMasterFileAsync
            (
                this IIrbisConnection connection,
                string? database = default
            )
        {
            var response = await connection.ExecuteAsync
                (
                    CommandCode.ReloadMasterFile,
                    database ?? connection.Database
                );

            return response is not null && response.CheckReturnCode();
        } // method ReloadMasterFileAsync

        /// <summary>
        /// Асинхронный ерезапуск сервера без утери подключенных клиентов.
        /// </summary>
        /// <returns>Признак успешного завергения операции.</returns>
        public static async Task<bool> RestartServerAsync
            (
                this IIrbisConnection connection
            )
        {
            var response = await connection.ExecuteAsync(CommandCode.RestartServer);

            return response is not null;
        } // method RestartServerAsync

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="parameters">Параметры поиска.</param>
        /// <returns>Массив элементов, описывающих найденные записи.</returns>
        public static async Task<FoundItem[]> SearchAsync
            (
                this IIrbisConnection connection,
                SearchParameters parameters
            )
        {
            if (!connection.CheckConnection())
            {
                return Array.Empty<FoundItem>();
            }

            var query = new Query(connection, CommandCode.Search);
            parameters.Encode(connection, query);
            var response = await connection.ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return Array.Empty<FoundItem>();
            }

            return FoundItem.Parse(response);
        } // method SearchAsync

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Массив MFN найденных записей.</returns>
        public static async Task<int[]> SearchAsync
            (
                this IIrbisConnection connection,
                string expression
            )
        {
            if (!connection.CheckConnection())
            {
                return Array.Empty<int>();
            }

            var query = new Query(connection, CommandCode.Search);
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
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
        public static async Task<int> SearchCountAsync
            (
                this IIrbisConnection connection,
                string expression
            )
        {
            if (!connection.CheckConnection())
            {
                return -1;
            }

            var query = new Query(connection, CommandCode.Search);
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
        /// <param name="expression"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Task<Record[]> SearchReadAsync
            (
                this IIrbisConnection connection,
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
                this IIrbisConnection connection,
                string expression
            )
        {
            throw new NotImplementedException();
        } // method SearchSingleRecordAsync

        /// <summary>
        /// Опустошение указанной базы данных.
        /// </summary>
        /// <param name="database">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public static async Task<bool> TruncateDatabaseAsync
            (
                this IIrbisConnection connection,
                string? database = default
            )
        {
            var response = await connection.ExecuteAsync
                (
                    CommandCode.EmptyDatabase,
                    database ?? connection.Database
                );

            return response is not null && response.CheckReturnCode();
        } // method TruncateDatabaseAsync

        /// <summary>
        /// Разблокирование указанной базы данных.
        /// </summary>
        /// <param name="database">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public static async Task<bool> UnlockDatabaseAsync
            (
                this IIrbisConnection connection,
                string? database = default
            )
        {
            var response = await connection.ExecuteAsync
                (
                    CommandCode.UnlockDatabase,
                    database ?? connection.Database
                );

            return response is not null && response.CheckReturnCode();
        } // method UnlockDatabaseAsync

        /// <summary>
        /// Разблокирование указанных записей в указанной базе данных.
        /// </summary>
        /// <param name="mfnList">Перечень MFN, подлежащих разблокировке.</param>
        /// <param name="database">Имя базы данных.
        /// По умолчанию текущая база данных.</param>
        /// <returns>Признак успешности </returns>
        public static async Task<bool> UnlockRecordsAsync
            (
                this IIrbisConnection connection,
                IEnumerable<int> mfnList,
                string? database = default
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            var query = new Query(connection, CommandCode.UnlockRecords);
            query.AddAnsi(database ?? connection.Database);
            foreach (var mfn in mfnList)
            {
                query.Add(mfn);
            }

            var response = await connection.ExecuteAsync(query);

            return response is not null && response.CheckReturnCode();
        } // method UnlockRecordsAsync

        /// <summary>
        /// Обновление указанных строк серверного INI-файла.
        /// </summary>
        /// <param name="lines">Измененные строки INI-файла.</param>
        /// <returns>Признак успешности завершения операции.</returns>
        public static async Task<bool> UpdateIniFileAsync
            (
                this IIrbisConnection connection,
                IEnumerable<string> lines
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            var query = new Query(connection, CommandCode.UpdateIniFile);
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    query.AddAnsi(line);
                }
            }

            var response = await connection.ExecuteAsync(query);

            return response is not null;
        } // method UpdateIniFileAsync

        /// <summary>
        /// Обновление списка пользователей системы на сервере.
        /// </summary>
        /// <param name="userList">Список пользователей.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public static async Task<bool> UpdateUserListAsync
            (
                this IIrbisConnection connection,
                IEnumerable<UserInfo> userList
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            var query = new Query(connection, CommandCode.SetUserList);
            foreach (var user in userList)
            {
                query.AddAnsi(user.Encode());
            }

            var response = await connection.ExecuteAsync(query);

            return response is not null;
        } // method UpdateUserListAsync

        /// <summary>
        /// Сохранение/обновление записи на сервере.
        /// </summary>
        /// <param name="record">Запись, подлежащая сохранению.</param>
        /// <param name="lockFlag">Оставить запись заблокированной?</param>
        /// <param name="actualize">Актуализировать запись?</param>
        /// <param name="dontParse">Не разбирать ответ сервера?</param>
        /// <returns>Новый максимальный MFN в базе данных.</returns>
        public static async Task<int> WriteRecordAsync
            (
                this IIrbisConnection connection,
                Record record,
                bool lockFlag = false,
                bool actualize = true,
                bool dontParse = false
            )
        {
            if (!connection.CheckConnection())
            {
                return 0;
            }

            var query = new Query(connection, CommandCode.UpdateRecord);
            query.AddAnsi(record.Database ?? connection.Database);
            query.Add(lockFlag ? 1 : 0);
            query.Add(actualize ? 1 : 0);
            query.AddUtf(record.Encode());

            var response = await connection.ExecuteAsync(query);
            if (response is null || !response.CheckReturnCode())
            {
                return 0;
            }

            var result = response.ReturnCode;
            if (!dontParse)
            {
                record.Database ??= connection.Database;
                // TODO reparse the record
            }

            return result;
        } // method WriteRecordAsync

        public static Task<Record[]> WriteRecordsAsync
            (
                this IIrbisConnection connection,
                Record[] records,
                bool lockFlag = false,
                bool actualize = true
            )
        {
            throw new NotImplementedException();
        } // method WriteRecordsAsync

        public static async Task<bool> WriteTextFileAsync
            (
                this IIrbisConnection connection,
                params FileSpecification[] specifications
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            var query = new Query(connection, CommandCode.ReadDocument);
            foreach (var specification in specifications)
            {
                query.AddAnsi(specification.ToString());
            }
            var response = await connection.ExecuteAsync(query);
            if (response is null)
            {
                return false;
            }

            throw new NotImplementedException();
        } // method WriteTextFileAsync

        #endregion

    } // class ConenctionAsyncMethods

} // namespace ManagedIrbis
