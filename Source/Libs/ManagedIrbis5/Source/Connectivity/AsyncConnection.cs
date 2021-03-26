// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AsyncConnection.cs -- асинхронное подключение к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Асинхронное подключение к серверу ИРБИС64.
    /// </summary>
    public sealed class AsyncConnection
        : IAsyncConnection
    {
        #region Events

        /// <summary>
        /// Fired when <see cref="Busy"/> changed.
        /// </summary>
        public event EventHandler? BusyChanged;

        #endregion

        #region Properties

        /// <inheritdoc cref="IBasicConnection.Host"/>
        public string Host { get; set; } = "127.0.0.1";

        /// <inheritdoc cref="IBasicConnection.Port"/>
        public ushort Port { get; set; } = 6666;

        /// <inheritdoc cref="IBasicConnection.Username"/>
        public string Username { get; set; } = string.Empty;

        /// <inheritdoc cref="IBasicConnection.Password"/>
        public string Password { get; set; } = string.Empty;

        /// <inheritdoc cref="IBasicConnection.Database"/>
        public string Database { get; set; } = "IBIS";

        /// <summary>
        ///
        /// </summary>
        public string Workstation { get; set; } = "C";

        /// <summary>
        ///
        /// </summary>
        public int ClientId { get; protected internal set; }

        /// <summary>
        ///
        /// </summary>
        public int QueryId { get; internal set; }

        /// <summary>
        ///
        /// </summary>
        public bool Connected { get; internal set; } = false;

        /// <summary>
        /// Busy?
        /// </summary>
        public bool Busy { get; internal set; } = false;

        /// <summary>
        /// Last error code.
        /// </summary>
        public int LastError { get; internal set; } = 0;

        /// <summary>
        ///
        /// </summary>
        public CancellationToken Cancellation { get; }

        // TODO Implement properly

        /// <summary>
        /// Версия клиента.
        /// </summary>
        public static readonly Version ClientVersion = Assembly
                .GetExecutingAssembly()
                .GetName()
                .Version
            ?? throw new ApplicationException("ClientVersion not defined");

        /// <summary>
        ///
        /// </summary>
        public string? ServerVersion { get; private set; }

        // /// <summary>
        // ///
        // /// </summary>
        // public IniFile? IniFile { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public int Interval { get; private set; }

        /// <summary>
        /// Socket.
        /// </summary>
        public IAsyncClientSocket Socket { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsyncConnection
            (
                IAsyncClientSocket socket,
                IServiceProvider provider
            )
        {
            Socket = socket;
            socket.Connection = this;
            _cancellation = new CancellationTokenSource();
            Cancellation = _cancellation.Token;
            _logger = Magna.Factory.CreateLogger<Connection>();
            _provider = provider;
        }

        #endregion

        #region Private members

        internal ILogger _logger;

        internal IServiceProvider _provider;

        internal CancellationTokenSource _cancellation;

        private static readonly int[] _goodCodesForReadRecord = { -201, -600, -602, -603 };
        private static readonly int[] _goodCodesForReadTerms = { -202, -203, -204 };

        internal void SetBusy
            (
                bool busy
            )
        {
            if (Busy != busy)
            {
                _logger.LogTrace($"SetBusy{busy}");
                Busy = busy;
                BusyChanged?.Invoke(this, EventArgs.Empty);
            }
        } // method SetBusy

        #endregion

        #region IAsyncConnection members

        /// <summary>
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        public async Task<bool> ActualizeDatabaseAsync(string? database = default) =>
            await ActualizeRecordAsync(new () { Database = database, Mfn = 0 });

        /// <summary>
        /// Актуализация записи.
        /// </summary>
        public async Task<bool> ActualizeRecordAsync
            (
                ActualizeRecordParameters parameters
            )
        {
            var database = parameters.Database
                           ?? Database
                           ?? throw new IrbisException();

            var response = await ExecuteAsync
                (
                    CommandCode.ActualizeRecord,
                    database,
                    parameters.Mfn
                );

            return response is not null;
        } // method ActualizeRecordAsync

        /// <summary>
        /// Cancel the current operation.
        /// </summary>
        public void CancelOperation()
        {
            _cancellation.Cancel();
        } // method CancelOperation

        /// <summary>
        /// Проверка, установлено ли соединение.
        /// </summary>
        /// <returns></returns>
        public bool CheckProviderState()
        {
            if (!Connected)
            {
                LastError = -100_500;
            }

            return Connected;
        } // method CheckConnection

        /// <inheritdoc cref="IAsyncConnection.ConnectAsync"/>
        public async Task<bool> ConnectAsync()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN: QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            var query = new Query(this, CommandCode.RegisterClient);
            query.AddAnsi(Username);
            query.AddAnsi(Password);

            var response = await ExecuteAsync(query);
            if (response is null)
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
        } // method ConnectAsync

        /// <summary>
        /// Создание базы данных на сервере.
        /// </summary>
        public async Task<bool> CreateDatabaseAsync
            (
                CreateDatabaseParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var database = parameters.Database
                           ?? Database
                           ?? throw new IrbisException();
            var query = new Query(this, CommandCode.CreateDatabase);
            query.AddAnsi(database);
            query.AddAnsi(parameters.Database);
            query.Add(parameters.ReaderAccess ? 1 : 0);
            var response = await ExecuteAsync(query);

            return response?.CheckReturnCode() ?? false;
        } // method CreateDatabaseAsync

        /// <summary>
        /// Создание поискового словаря в указанной базе данных.
        /// </summary>
        public async Task<bool> CreateDictionaryAsync
            (
                string? databaseName = default
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var database = databaseName
                           ?? Database
                           ?? throw new IrbisException();
            var query = new Query(this, CommandCode.CreateDictionary);
            query.AddAnsi(database);
            var response = await ExecuteAsync(query);

            return response?.CheckReturnCode() ?? false;
        } // method CreateDictionaryAsync

        /// <summary>
        /// Удаление указанной базы данных на сервере.
        /// </summary>
        /// <param name="databaseName">Имя удалаемой базы данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> DeleteDatabaseAsync
            (
                string? databaseName = default
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var database = databaseName
                           ?? Database
                           ?? throw new IrbisException();
            var query = new Query(this, CommandCode.DeleteDatabase);
            query.AddAnsi(database);
            var response = await ExecuteAsync(query);

            return response?.CheckReturnCode() ?? false;
        } // method DeleteDatabaseAsync

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        /// <returns>Признак успешности завершения операции.</returns>
        public async Task<bool> DisconnectAsync()
        {
            if (Connected)
            {
                var query = new Query(this, CommandCode.UnregisterClient);
                query.AddAnsi(Username);
                try
                {
                    await ExecuteAsync(query);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }

                Connected = false;
            }

            return true;
        } // method DisconnectAsync

        /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        public async Task<Response?> ExecuteAsync
            (
                Query query
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
                    /*
                    if (_debug)
                    {
                        query.Debug(Console.Out);
                    }
                    */

                    result = await Socket.TransactAsync(query);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    return null;
                }

                if (result is not null)
                {
                    /*
                    if (_debug)
                    {
                        result.Debug(Console.Out);
                    }
                    */

                    result.Parse();
                }

                QueryId++;

                return result;
            }
            finally
            {
                SetBusy(false);
            }
        } // method ExecuteAsync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="args">Опциональные параметры команды
        /// (в кодировке ANSI).</param>
        /// <returns>Ответ сервера.</returns>
        public async Task<Response?> ExecuteAsync
            (
                string command,
                params object[] args
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new Query(this, command);
            foreach (var arg in args)
            {
                query.AddAnsi(arg.ToString());
            }

            var result = await ExecuteAsync(query);

            return result;
        } // method ExecuteAsync

        /// <summary>
        /// Форматирование указанной записи по ее MFN.
        /// </summary>
        public async Task<string?> FormatRecordAsync
            (
                string format,
                int mfn
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new Query(this, CommandCode.FormatRecord);
            query.AddAnsi(Database);
            var prepared = IrbisFormat.PrepareFormat(format);
            query.AddAnsi(prepared);
            query.Add(1);
            query.Add(mfn);
            var response = await ExecuteAsync(query);
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
        public Task<string?> FormatRecordAsync
            (
                string format,
                Record record
            )
        {
            throw new NotImplementedException();
        } // method FormatRecordAsync

        /// <summary>
        /// Полнотекстовый поиск ИРБИС64+.
        /// </summary>
        public async Task<FullTextResult?> FullTextSearchAsync
            (
                SearchParameters searchParameters,
                TextParameters textParameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new Query(this, CommandCode.NewFulltextSearch);
            searchParameters.Encode(this, query);
            textParameters.Encode(this, query);
            var response = await ExecuteAsync(query);
            if (!(response?.CheckReturnCode() ?? false))
            {
                return null;
            }

            var result = new FullTextResult();
            result.Decode(response);

            return result;
        } // method FullTextSearchAsync

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// По умолчанию используется текущая база данных.
        /// </summary>
        /// <param name="databaseName">Опциональное имя базы данных
        /// (<c>null</c> означает текущую базу данных).</param>
        /// <returns>Макисмальный MFN.</returns>
        public async Task<int> GetMaxMfnAsync
            (
                string? databaseName = default
            )
        {
            var database = databaseName
                           ?? Database
                           ?? throw new IrbisException();
            var response = await ExecuteAsync(CommandCode.GetMaxMfn, database);

            return response?.CheckReturnCode() ?? false
                ? response.ReturnCode
                : 0;
        } // method GetMaxMfnAsync

        /// <summary>
        /// Получение версии ИРБИС-сервера.
        /// </summary>
        public async Task<ServerVersion?> GetServerVersionAsync()
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new Query(this, CommandCode.ServerInfo);
            var response = await ExecuteAsync(query);
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
        /// Получение списка файлов на сервере,
        /// удовлетворяющих указанной спецификации.
        /// </summary>
        public async Task<string[]?> ListFilesAsync
            (
                FileSpecification specification
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new Query(this, CommandCode.ListFiles);
            query.AddAnsi(specification.ToString());

            var response = await ExecuteAsync(query);
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
        } // method ListFileasAsync

        /// <summary>
        /// Получение списка файлов на сервере,
        /// удовлетворяющих указанным спецификациям.
        /// </summary>
        public async Task<string[]?> ListFilesAsync
            (
                params FileSpecification[] specifications
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            if (specifications.Length == 0)
            {
                return Array.Empty<string>();
            }

            var query = new Query(this, CommandCode.ListFiles);
            foreach (var specification in specifications)
            {
                query.AddAnsi(specification.ToString());
            }

            var response = await ExecuteAsync(query);
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
        } // method ListFilesAsync

        /// <summary>
        /// Получение списка процессов, работающих в данный момент
        /// на ИРБИС-сервере.
        /// </summary>
        public async Task<ProcessInfo[]?> ListProcessesAsync()
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new Query(this, CommandCode.GetProcessList);
            var response = await ExecuteAsync(query);
            if (response is null)
            {
                return null;
            }

            response.CheckReturnCode();
            var result = ProcessInfo.Parse(response);

            return result;
        } // method ListProcessesAsync

        /// <summary>
        /// Получение списка пользователей, имеющих доступ к
        /// ИРБИС-серверу. Эти пользователи не обязательно должны
        /// быть залогинены в данный момент.
        /// </summary>
        public async Task<UserInfo[]?> ListUsersAsync()
        {
            throw new NotImplementedException();
        } // method ListUsersAsync

        /// <summary>
        /// Пустая операция, необходимая для поддержания связи
        /// с ИРБИС-сервером.
        /// </summary>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> NoOperationAsync()
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var response = await ExecuteAsync(CommandCode.Nop);

            return response?.CheckReturnCode() ?? false;
        } // method NoOperationAsync

        /// <summary>
        /// Чтение с сервера постингов для указанных терминов.
        /// </summary>
        /// <param name="parameters">Параметры постингов.</param>
        /// <returns>Массив прочитанных постингов.</returns>
        public async Task<TermPosting[]?> ReadPostingsAsync
            (
                PostingParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new Query(this, CommandCode.ReadPostings);
            parameters.Encode(this, query);
            var response = await ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode(ConnectionUtility.GoodCodesForReadTerms))
            {
                return null;
            }

            return TermPosting.Parse(response);
        } // method ReadPostingsAsync

        /// <summary>
        /// Чтение записи с сервера.
        /// </summary>
        public async Task<Record?> ReadRecordAsync
            (
                ReadRecordParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var database = parameters.Database
                           ?? Database
                           ?? throw new IrbisException();
            var query = new Query(this, CommandCode.ReadRecord);
            query.AddAnsi(database);
            query.Add(parameters.Mfn);
            // TODO: добавить обработку прочих параметров
            var response = await ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode(ConnectionUtility.GoodCodesForReadRecord) )
            {
                return null;
            }

            var result = new Record
            {
                Database = Database
            };
            result.Decode(response);

            return result;
        } // method ReadRecordAsync

        /// <summary>
        /// Чтение с сервера терминов поискового словаря.
        /// </summary>
        /// <param name="parameters">Параметры терминов.</param>
        /// <returns>Массив прочитанных терминов либо <c>null</c>.</returns>
        public async Task<Term[]?> ReadTermsAsync
            (
                TermParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var command = parameters.ReverseOrder
                ? CommandCode.ReadTermsReverse
                : CommandCode.ReadTerms;
            var query = new Query(this, command);
            parameters.Encode(this, query);
            var response = await ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode(ConnectionUtility.GoodCodesForReadTerms))
            {
                return Array.Empty<Term>();
            }

            return Term.Parse(response);
        } // method ReadTermsAsyncs

        /// <summary>
        /// Чтение текстового файла с ИРБИС-сервера.
        /// </summary>
        /// <param name="specification">Спецификация файла.</param>
        /// <returns>Содержимое файла либо <c>null</c>.</returns>
        public async Task<string?> ReadTextFileAsync
            (
                FileSpecification specification
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new Query(this, CommandCode.ReadDocument);
            query.AddAnsi(specification.ToString());
            var response = await ExecuteAsync(query);
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
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> ReloadDictionaryAsync
            (
                string? databaseName = default
            )
        {
            var response = await ExecuteAsync
                (
                    CommandCode.ReloadDictionary,
                    databaseName ?? Database
                );

            return response?.CheckReturnCode() ?? false;
        } // method ReloadDictionaryAsync

        /// <summary>
        /// Пересоздание мастер-файла для указанной базы данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> ReloadMasterFileAsync
            (
                string? databaseName = default
            )
        {
            var response = await ExecuteAsync
                (
                    CommandCode.ReloadMasterFile,
                    databaseName ?? Database
                );

            return response?.CheckReturnCode() ?? false;
        } // method ReloadMasterFileAsync


        /// <summary>
        /// Асинхронный перезапуск сервера без утери подключенных клиентов.
        /// </summary>
        /// <returns>Признак успешного завергения операции.</returns>
        public async Task<bool> RestartServerAsync()
        {
            var response = await ExecuteAsync(CommandCode.RestartServer);

            return response is not null;
        } // method RestartServerAsync

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="parameters">Параметры поиска.</param>
        /// <returns>Массив элементов, описывающих найденные записи.</returns>
        public async Task<FoundItem[]?> SearchAsync
            (
                SearchParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new Query(this, CommandCode.Search);
            parameters.Encode(this, query);
            var response = await ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return Array.Empty<FoundItem>();
            }

            return FoundItem.Parse(response);
        } // method SearchAsync

        /// <summary>
        /// Опустошение указанной базы данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> TruncateDatabaseAsync
            (
                string? databaseName = default
            )
        {
            var response = await ExecuteAsync
                (
                    CommandCode.EmptyDatabase,
                    databaseName ?? Database
                );

            return response is not null && response.CheckReturnCode();
        } // method TruncateDatabaseAsync

        /// Разблокирование указанной базы данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> UnlockDatabaseAsync
            (
                string? databaseName = default
            )
        {
            var response = await ExecuteAsync
                (
                    CommandCode.UnlockDatabase,
                    databaseName ?? Database
                );

            return response is not null && response.CheckReturnCode();
        } // method UnlockDatabaseAsync

        /// <summary>
        /// Разблокирование указанных записей в указанной базе данных.
        /// </summary>
        /// <param name="mfnList">Перечень MFN, подлежащих разблокировке.</param>
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию текущая база данных.</param>
        /// <returns>Признак успешности </returns>
        public async Task<bool> UnlockRecordsAsync
            (
                IEnumerable<int> mfnList,
                string? databaseName = default
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new Query(this, CommandCode.UnlockRecords);
            query.AddAnsi(databaseName ?? Database);
            foreach (var mfn in mfnList)
            {
                query.Add(mfn);
            }

            var response = await ExecuteAsync(query);

            return response is not null && response.CheckReturnCode();
        } // method UnlockRecordsAsync

        /// <summary>
        /// Обновление указанных строк серверного INI-файла.
        /// </summary>
        /// <param name="lines">Измененные строки INI-файла.</param>
        /// <returns>Признак успешности завершения операции.</returns>
        public async Task<bool> UpdateIniFileAsync
            (
                IEnumerable<string> lines
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new Query(this, CommandCode.UpdateIniFile);
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    query.AddAnsi(line);
                }
            }

            var response = await ExecuteAsync(query);

            return response is not null;
        } // method UpdateIniFileAsync


        /// <summary>

        /// <summary>
        /// Обновление списка пользователей системы на сервере.
        /// </summary>
        /// <param name="userList">Список пользователей.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> UpdateUserListAsync
            (
                IEnumerable<UserInfo> userList
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new Query(this, CommandCode.SetUserList);
            foreach (var user in userList)
            {
                query.AddAnsi(user.Encode());
            }

            var response = await ExecuteAsync(query);

            return response is not null;
        } // method UpdateUserListAsync

        /// <summary>
        /// Сохранение/обновление файла на сервере.
        /// </summary>
        /// <param name="specification">Спецификация файла
        /// (включает в себя содержимое файла).</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> WriteFileAsync(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Сохранение/обновление библиографической записи на сервере.
        /// </summary>
        /// <param name="parameters">Параметры сохранения/обновления.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        public async Task<bool> WriteRecordAsync
            (
                WriteRecordParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var record = parameters.Record as Record
                         ?? throw new IrbisException();
            var database = record.Database
                           ?? Database
                           ?? throw new IrbisException();
            var query = new Query(this, CommandCode.UpdateRecord);
            query.AddAnsi(database);
            query.Add(parameters.Lock ? 1 : 0);
            query.Add(parameters.Actualize ? 1 : 0);
            query.AddUtf(record.Encode());

            var response = await ExecuteAsync(query);
            if (response is null || !response.CheckReturnCode())
            {
                return false;
            }

            var result = response.ReturnCode;
            if (!parameters.DontParse)
            {
                record.Database ??= database;
                // TODO reparse the record
            }

            return true;
        } // method WriteRecordAsync

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public async ValueTask DisposeAsync()
        {
            if (Connected)
            {
                await DisconnectAsync();
            }
        } // method DisposeAsync

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() => DisposeAsync().GetAwaiter().GetResult();

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService(Type serviceType) =>
            _provider.GetService(serviceType);

        #endregion

        #region Object members

        #endregion

    } // class AsyncConnection

} // namespace ManagedIrbis
