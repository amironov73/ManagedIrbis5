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
using System.Diagnostics;
using System.Threading;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    // Синхронная часть соединения
    partial class Connection
    {
        #region Public methods

        /// <summary>
        /// Подключение к серверу ИРБИС64.
        /// </summary>
        public bool Connect()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN: QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            var query = new ValueQuery(this, CommandCode.RegisterClient);
            query.AddAnsi(Username);
            query.AddAnsi(Password);

            var response = ExecuteSync(ref query);
            if (ReferenceEquals(response, null))
            {
                return false;
            }

            if (response.GetReturnCode() == -3337)
            {
                goto AGAIN;
            }

            if (response.ReturnCode < 0)
            {
                return false;
            }

            Connected = true;
            ServerVersion = response.ServerVersion;
            Interval = response.ReadInteger();
            // TODO Read INI-file

            return true;
        } // method Connect

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        /// <returns>Признак успешности завершения операции.</returns>
        public bool Disconnect()
        {
            if (Connected)
            {
                var query = new ValueQuery(this, CommandCode.UnregisterClient);
                query.AddAnsi(Username);
                try
                {
                    ExecuteSync(ref query);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }

                Connected = false;
            }

            return true;
        } // method Disconnect

        /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        public Response? ExecuteSync
            (
                ref ValueQuery query
            )
        {
            SetBusy(true);
            try
            {
                if (_cancellation.IsCancellationRequested)
                {
                    _cancellation = new CancellationTokenSource();
                }

                Response? result;
                try
                {
                    if (_debug)
                    {
                        query.Debug(Console.Out);
                    }

                    result = Socket.TransactSync(ref query);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    return null;
                }

                if (ReferenceEquals(result, null))
                {
                    return null;
                }

                if (_debug)
                {
                    result.Debug(Console.Out);
                }

                result.Parse();
                QueryId++;

                return result;
            }
            finally
            {
                SetBusy(false);
            }
        } // method ExecuteSync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="args">Опциональные параметры команды
        /// (в кодировке ANSI).</param>
        /// <returns>Ответ сервера.</returns>
        public Response? ExecuteSync
            (
                string command,
                params object[] args
            )
        {
            if (!CheckConnection())
            {
                return null;
            }

            var query = new ValueQuery(this, command);
            foreach (var arg in args)
            {
                query.AddAnsi(arg?.ToString());
            }

            var result = ExecuteSync(ref query);

            return result;
        } // method ExecuteSync

        /// <summary>
        /// Форматирование записи с указанием её MFN.
        /// </summary>
        /// <param name="format">Спецификация формата.</param>
        /// <param name="mfn">MFN записи.</param>
        /// <returns>Результат расформатирования.</returns>
        public string? FormatRecord
            (
                string format,
                int mfn
            )
        {
            if (!CheckConnection())
            {
                return null;
            }

            var query = new ValueQuery(this, CommandCode.FormatRecord);
            query.AddAnsi(Database);
            var prepared = IrbisFormat.PrepareFormat(format);
            query.AddAnsi(prepared);
            query.Add(1);
            query.Add(mfn);
            var response = ExecuteSync(ref query);
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
        /// Получение максимального MFN для указанной базы данных.
        /// </summary>
        /// <param name="database">Опциональное имя базы данных
        /// (<c>null</c> означает текущую базу данных).</param>
        /// <returns>Макисмальный MFN.</returns>
        public int GetMaxMfn
            (
                string? database = default
            )
        {
            if (!CheckConnection())
            {
                return 0;
            }

            database ??= Database;
            var query = new ValueQuery(this, CommandCode.GetMaxMfn);
            query.AddAnsi(database);
            var response = ExecuteSync(ref query);
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

        /// <summary>
        /// Пустая операция.
        /// </summary>
        /// <returns>Признак успешного завершения операции.</returns>
        public bool Nop()
        {
            if (!CheckConnection())
            {
                return false;
            }

            var response = ExecuteSync(CommandCode.Nop);

            return !ReferenceEquals(response, null)
                   && response.CheckReturnCode();
        } // method Nop

        /// <summary>
        /// Чтение библиографической записи с сервера.
        /// </summary>
        public Record? ReadRecord
            (
                int mfn
            )
        {
            if (!CheckConnection())
            {
                return null;
            }

            var query = new ValueQuery(this, CommandCode.ReadRecord);
            query.AddAnsi(Database);
            query.Add(mfn);
            var response = ExecuteSync(ref query);
            if (response is null)
            {
                return null;
            }

            if (!response.CheckReturnCode(_goodCodesForReadRecord))
            {
                return null;
            }

            var result = new Record
            {
                Database = Database
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
        public Term[] ReadTerms
            (
                string startTerm,
                int numberOfTerms
            )
        {
            var parameters = new TermParameters
            {
                Database = Database,
                StartTerm = startTerm,
                NumberOfTerms = numberOfTerms
            };

            return ReadTerms(parameters);
        } // method ReadTerms

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="parameters">Параметры терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public Term[] ReadTerms
            (
                TermParameters parameters
            )
        {
            if (!CheckConnection())
            {
                return Array.Empty<Term>();
            }

            var command = parameters.ReverseOrder
                ? CommandCode.ReadTermsReverse
                : CommandCode.ReadTerms;
            var query = new ValueQuery(this, command);
            parameters.Encode(this, ref query);
            var response = ExecuteSync(ref query);
            if (response is null)
            {
                return Array.Empty<Term>();
            }

            if (!response.CheckReturnCode(_goodCodesForReadTerms))
            {
                return Array.Empty<Term>();
            }

            return Term.Parse(response);
        } // method ReadTerms

        /// <summary>
        ///
        /// </summary>
        public string? ReadTextFile
            (
                string? specification
            )
        {
            if (!CheckConnection())
            {
                return null;
            }

            if (string.IsNullOrEmpty(specification))
            {
                return null;
            }

            var query = new ValueQuery(this, CommandCode.ReadDocument);
            query.AddAnsi(specification);
            var response = ExecuteSync(ref query);
            if (response is null)
            {
                return null;
            }

            var result = IrbisText.IrbisToWindows(response.ReadAnsi());

            return result;
        } // method ReadTextFile

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="parameters">Параметры поиска.</param>
        /// <returns>Массив элементов, описывающих найденные записи.</returns>
        public FoundItem[] Search
            (
                SearchParameters parameters
            )
        {
            if (!CheckConnection())
            {
                return Array.Empty<FoundItem>();
            }

            var query = new ValueQuery(this, CommandCode.Search);
            parameters.Encode(this, ref query);
            var response = ExecuteSync(ref query);
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
        public int[] Search
            (
                string expression
            )
        {
            if (!CheckConnection())
            {
                return Array.Empty<int>();
            }

            var query = new ValueQuery(this, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = Database,
                Expression = expression
            };
            parameters.Encode(this, ref query);
            var response = ExecuteSync(ref query);
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
        public int SearchCount
            (
                string expression
            )
        {
            if (!CheckConnection())
            {
                return -1;
            }

            var query = new ValueQuery(this, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = Database,
                Expression = expression,
                FirstRecord = 0
            };
            parameters.Encode(this, ref query);
            var response = ExecuteSync(ref query);
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
        public int WriteRecord
            (
                Record record,
                bool lockFlag = false,
                bool actualize = true,
                bool dontParse = false
            )
        {
            if (!CheckConnection())
            {
                return 0;
            }

            var query = new ValueQuery(this, CommandCode.UpdateRecord);
            query.AddAnsi(record.Database ?? Database);
            query.Add(lockFlag ? 1 : 0);
            query.Add(actualize ? 1 : 0);
            query.AddUtf(record.Encode());

            var response = ExecuteSync(ref query);
            if (response is null || !response.CheckReturnCode())
            {
                return 0;
            }

            var result = response.ReturnCode;
            if (!dontParse)
            {
                record.Database ??= Database;
                // TODO reparse the record
            }

            return result;
        } // method WriteRecord

        #endregion

    } // class Connection

} // namespace ManagedIrbis
