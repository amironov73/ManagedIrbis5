// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Connection.cs -- подключение к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    // Синхронная часть соединения
    public static class ConnectionSyncMethods
    {
        #region Public methods

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="args">Опциональные параметры команды
        /// (в кодировке ANSI).</param>
        /// <returns>Ответ сервера.</returns>
        public static Response? ExecuteSync
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

            var query = new ValueQuery(connection, command);
            foreach (var arg in args)
            {
                query.AddAnsi(arg?.ToString());
            }

            var result = connection.ExecuteSync(ref query);

            return result;
        } // method ExecuteSync

        /// <summary>
        /// Форматирование записи с указанием её MFN.
        /// </summary>
        /// <param name="format">Спецификация формата.</param>
        /// <param name="mfn">MFN записи.</param>
        /// <returns>Результат расформатирования.</returns>
        public static string? FormatRecord
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

            var query = new ValueQuery(connection, CommandCode.FormatRecord);
            query.AddAnsi(connection.Database);
            var prepared = IrbisFormat.PrepareFormat(format);
            query.AddAnsi(prepared);
            query.Add(1);
            query.Add(mfn);
            var response = connection.ExecuteSync(ref query);
            if (ReferenceEquals(response, null))
            {
                return null;
            }

            response.CheckReturnCode();
            string result = response.ReadRemainingUtfText();
            if (!string.IsNullOrEmpty(result))
            {
                result = result.TrimEnd();
            }

            return result;
        } // method FormatRecord

        /// <summary>
        /// Полнотекстовый поиск ИРБИС64+.
        /// </summary>
        public static FullTextResult? FullTextSearch
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

            var query = new ValueQuery(connection, CommandCode.NewFulltextSearch);
            searchParameters.Encode(connection, ref query);
            textParameters.Encode(connection, ref query);
            query.DebugUtf(Console.Out);
            var response = connection.ExecuteSync(ref query);
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
        public static int GetMaxMfn
            (
                this IIrbisConnection connection,
                string? database = default
            )
        {
            if (!connection.CheckConnection())
            {
                return 0;
            }

            database ??= connection.Database;
            var query = new ValueQuery(connection, CommandCode.GetMaxMfn);
            query.AddAnsi(database);
            var response = connection.ExecuteSync(ref query);
            if (ReferenceEquals(response, null))
            {
                return 0;
            }

            if (!response.CheckReturnCode())
            {
                return 0;
            }

            return response.ReturnCode;
        } // method GetMaxMfn

        public static string FormatRecord
            (
                this IIrbisConnection connection,
                string format,
                Record record
            )
        {
            throw new NotImplementedException();
        }

        public static string[] FormatRecords
            (
                this IIrbisConnection connection,
                string database,
                string format,
                int[] mfns
            )
        {
            throw new NotImplementedException();
        }

        public static DatabaseInfo GetDatabaseInfo
            (
                this IIrbisConnection connection,
                string database
            )
        {
            throw new NotImplementedException();
        }

        public static string GetDatabaseStat
            (
                this IIrbisConnection connection,
                StatDefinition definition
            )
        {
            throw new NotImplementedException();
        }

        public static ServerStat GetServerStat
            (
                this IIrbisConnection connection
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        public static ServerVersion? GetServerVersion
            (
                this IIrbisConnection connection
            )
        {
            if (!connection.CheckConnection())
            {
                return null;
            }

            var query = new ValueQuery(connection, CommandCode.ServerInfo);
            var response = connection.ExecuteSync(ref query);
            if (response is null)
            {
                return null;
            }

            response.CheckReturnCode();
            var result = new ServerVersion();
            result.Parse(response);

            return result;
        } // method GetServerVersion

        public static GblResult GlobalCorrection
            (
                this IIrbisConnection connection,
                GblSettings settings
            )
        {
            throw new NotImplementedException();
        }

        public static DatabaseInfo[] ListDatabases
            (
                this IIrbisConnection connection,
                string specification
            )
        {
            throw new NotImplementedException();
        }

        public static string[] ListFiles
            (
                this IIrbisConnection connection,
                FileSpecification specification
            )
        {
            throw new NotImplementedException();
        }

        public static UserInfo[] ListUsers
            (
                this IIrbisConnection connection
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Пустая операция.
        /// </summary>
        /// <returns>Признак успешного завершения операции.</returns>
        public static bool Nop
            (
                this IIrbisConnection connection
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            var response = connection.ExecuteSync(CommandCode.Nop);

            return !ReferenceEquals(response, null)
                   && response.CheckReturnCode();
        } // method Nop

        public static TermPosting[] ReadPostings
            (
                this IIrbisConnection connection,
                PostingParameters parameters
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Чтение библиографической записи с сервера.
        /// </summary>
        public static Record? ReadRecord
            (
                this IIrbisConnection connection,
                int mfn
            )
        {
            if (!connection.CheckConnection())
            {
                return null;
            }

            var query = new ValueQuery(connection, CommandCode.ReadRecord);
            query.AddAnsi(connection.Database);
            query.Add(mfn);
            var response = connection.ExecuteSync(ref query);
            if (response is null)
            {
                return null;
            }

            if (!response.CheckReturnCode(ConnectionUtility.GoodCodesForReadRecord))
            {
                return null;
            }

            var result = new Record
            {
                Database = connection.Database
            };
            result.Decode(response);

            return result;
        } // method ReadRecord

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="startTerm">Параметры терминов.</param>
        /// <param name="numberOfTerms">Максимальное число терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public static Term[] ReadTerms
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

            return connection.ReadTerms(parameters);
        } // method ReadTerms

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="parameters">Параметры терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public static Term[] ReadTerms
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
            var query = new ValueQuery(connection, command);
            parameters.Encode(connection, ref query);
            var response = connection.ExecuteSync(ref query);
            if (response is null)
            {
                return Array.Empty<Term>();
            }

            if (!response.CheckReturnCode(ConnectionUtility.GoodCodesForReadTerms))
            {
                return Array.Empty<Term>();
            }

            return Term.Parse(response);
        } // method ReadTerms

        public static string? ReadTextFile
            (
                this IIrbisConnection connection,
                FileSpecification specification
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///
        /// </summary>
        public static string? ReadTextFile
            (
                this IIrbisConnection connection,
                string? specification
            )
        {
            if (!connection.CheckConnection())
            {
                return null;
            }

            if (string.IsNullOrEmpty(specification))
            {
                return null;
            }

            var query = new ValueQuery(connection, CommandCode.ReadDocument);
            query.AddAnsi(specification);
            var response = connection.ExecuteSync(ref query);
            if (response is null)
            {
                return null;
            }

            var result = IrbisText.IrbisToWindows(response.ReadAnsi());

            return result;
        } // method ReadTextFile

        public static bool RestartServer
            (
                this IIrbisConnection connection
            )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="parameters">Параметры поиска.</param>
        /// <returns>Массив элементов, описывающих найденные записи.</returns>
        public static FoundItem[] Search
            (
                this IIrbisConnection connection,
                SearchParameters parameters
            )
        {
            if (!connection.CheckConnection())
            {
                return Array.Empty<FoundItem>();
            }

            var query = new ValueQuery(connection, CommandCode.Search);
            parameters.Encode(connection, ref query);
            var response = connection.ExecuteSync(ref query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return Array.Empty<FoundItem>();
            }

            return FoundItem.Parse(response);
        } // method Search

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Массив MFN найденных записей.</returns>
        public static int[] Search
            (
                this IIrbisConnection connection,
                string expression
            )
        {
            if (!connection.CheckConnection())
            {
                return Array.Empty<int>();
            }

            var query = new ValueQuery(connection, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression
            };
            parameters.Encode(connection, ref query);
            var response = connection.ExecuteSync(ref query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return Array.Empty<int>();
            }

            return FoundItem.ParseMfn(response);
        } // method Search

        /// <summary>
        /// Определение количества записей, удовлетворяющих
        /// заданному запросу.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
        public static int SearchCount
            (
                this IIrbisConnection connection,
                string expression
            )
        {
            if (!connection.CheckConnection())
            {
                return -1;
            }

            var query = new ValueQuery(connection, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression,
                FirstRecord = 0
            };
            parameters.Encode(connection, ref query);
            var response = connection.ExecuteSync(ref query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return -1;
            }

            return response.ReadInteger();
        } // method SearchCount

        /// <summary>
        /// Сохранение/обновление записи на сервере.
        /// </summary>
        /// <param name="record">Запись, подлежащая сохранению.</param>
        /// <param name="lockFlag">Оставить запись заблокированной?</param>
        /// <param name="actualize">Актуализировать запись?</param>
        /// <param name="dontParse">Не разбирать ответ сервера?</param>
        /// <returns>Новый максимальный MFN в базе данных.</returns>
        public static int WriteRecord
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

            var query = new ValueQuery(connection, CommandCode.UpdateRecord);
            query.AddAnsi(record.Database ?? connection.Database);
            query.Add(lockFlag ? 1 : 0);
            query.Add(actualize ? 1 : 0);
            query.AddUtf(record.Encode());

            var response = connection.ExecuteSync(ref query);
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
        } // method WriteRecord

        #endregion

    } // class Connection

} // namespace ManagedIrbis
