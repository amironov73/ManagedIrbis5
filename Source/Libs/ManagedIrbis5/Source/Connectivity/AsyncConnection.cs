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
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.IO;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Асинхронное подключение к серверу ИРБИС64.
    /// </summary>
    public class AsyncConnection
        : IAsyncConnection
    {
        #region Events

        /// <summary>
        /// Fired when <see cref="Busy"/> changed.
        /// </summary>
        public event EventHandler? BusyChanged;

        #endregion

        #region Properties

        /// <inheritdoc cref="IIrbisConnectionSettings.Host"/>
        public string Host { get; set; } = "127.0.0.1";

        /// <inheritdoc cref="IIrbisConnectionSettings.Port"/>
        public ushort Port { get; set; } = 6666;

        /// <inheritdoc cref="IIrbisConnectionSettings.Username"/>
        public string Username { get; set; } = string.Empty;

        /// <inheritdoc cref="IIrbisConnectionSettings.Password"/>
        public string Password { get; set; } = string.Empty;

        /// <inheritdoc cref="IBasicIrbisProvider.Database"/>
        public string Database { get; set; } = "IBIS";

        /// <inheritdoc cref="IIrbisConnectionSettings.Workstation"/>
        public string Workstation { get; set; } = "C";

        /// <inheritdoc cref="IIrbisConnectionSettings.ClientId"/>
        public int ClientId { get; protected internal set; }

        /// <inheritdoc cref="IIrbisConnectionSettings.QueryId"/>
        public int QueryId { get; protected internal set; }

        /// <inheritdoc cref="IBasicIrbisProvider.Connected"/>
        public bool Connected { get; protected internal set; }

        /// <inheritdoc cref="IBasicIrbisProvider.Busy"/>
        public bool Busy { get; protected internal set; }

        /// <inheritdoc cref="IBasicIrbisProvider.LastError"/>
        public int LastError { get; protected internal set; }

        /// <summary>
        /// Токен для отмены длительных операций.
        /// </summary>
        public CancellationToken Cancellation { get; }

        /// <summary>
        /// Версия сервера. Берется из ответа на регистрацию клиента.
        /// Сервер может прислать и пустую строку, надо быть
        /// к этому готовым.
        /// </summary>
        public string? ServerVersion { get; protected internal set; }

        /// <summary>
        /// INI-файл, присылвемый сервером в ответ на регистрацию клиента.
        /// </summary>
        public IniFile? IniFile { get; private set; }

        /// <summary>
        /// Интервал подтверждения на сервере, минуты.
        /// Берется из ответа сервера при регистрации клиента.
        /// Сервер может прислать и пустую строку, к этому надо
        /// быть готовым.
        /// </summary>
        public int Interval { get; private set; }

        /// <summary>
        /// Сокет.
        /// </summary>
        public IAsyncClientSocket Socket { get; }

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
            _logger = Magna.Factory.CreateLogger<IBasicIrbisProvider>();
            _provider = provider;
        }

        #endregion

        #region Private members

        protected internal ILogger _logger;

        protected internal IServiceProvider _provider;

        protected internal CancellationTokenSource _cancellation;

        private static readonly int[] _goodCodesForReadRecord = { -201, -600, -602, -603 };
        private static readonly int[] _goodCodesForReadTerms = { -202, -203, -204 };

        protected internal void SetBusy
            (
                bool busy
            )
        {
            if (Busy != busy)
            {
                _logger.LogTrace($"SetBusy: {busy}");
                Busy = busy;
                BusyChanged?.Invoke(this, EventArgs.Empty);
            }
        } // method SetBusy

        #endregion

        #region Public methods

                /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="asyncQuery">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        public async Task<Response?> ExecuteAsync
            (
                AsyncQuery asyncQuery
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
                        asyncQuery.Debug(Console.Out);
                    }
                    */

                    result = await Socket.TransactAsync(asyncQuery);
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

            var query = new AsyncQuery(this, command);
            foreach (var arg in args)
            {
                query.AddAnsi(arg.ToString());
            }

            var result = await ExecuteAsync(query);

            return result;
        } // method ExecuteAsync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <returns>Ответ сервера.</returns>
        public async Task<Response?> ExecuteAsync
            (
                string command
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(this, command);
            var result = await ExecuteAsync(query);

            return result;
        } // method ExecuteAsync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="arg1">Параметр команды.</param>
        /// <returns>Ответ сервера.</returns>
        public async Task<Response?> ExecuteAsync
            (
                string command,
                object arg1
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(this, command);
            query.AddAnsi(arg1.ToString());

            var result = await ExecuteAsync(query);

            return result;
        } // method ExecuteAsync

        #endregion

        #region IBasicIrbisProvider members

        /// <inheritdoc cref="IBasicIrbisProvider.CancelOperation"/>
        public void CancelOperation() => _cancellation.Cancel();

        /// <inheritdoc cref="IBasicIrbisProvider.CheckProviderState"/>
        public bool CheckProviderState()
        {
            if (!Connected)
            {
                LastError = -100_500;
            }

            return Connected;
        } // method CheckConnection

        /// <inheritdoc cref="IBasicIrbisProvider.Configure"/>
        public void Configure(string configurationString)
        {
            // ParseConnectionString
        }

        /// <inheritdoc cref="IBasicIrbisProvider.GetWaitHandle"/>
        public WaitHandle GetWaitHandle()
        {
            throw new NotImplementedException();
        } // method GetWaitHandle

        #endregion

        #region IAsyncConnection members

        /// <inheritdoc cref="IAsyncIrbisProvider.ActualizeRecordAsync"/>
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

        /// <inheritdoc cref="IAsyncIrbisProvider.ConnectAsync"/>
        public async Task<bool> ConnectAsync()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN: QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            var query = new AsyncQuery(this, CommandCode.RegisterClient);
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

        /// <inheritdoc cref="IAsyncIrbisProvider.CreateDatabaseAsync"/>
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
            var query = new AsyncQuery(this, CommandCode.CreateDatabase);
            query.AddAnsi(database);
            query.AddAnsi(parameters.Database);
            query.Add(parameters.ReaderAccess ? 1 : 0);
            var response = await ExecuteAsync(query);

            return response?.CheckReturnCode() ?? false;
        } // method CreateDatabaseAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.CreateDictionaryAsync"/>
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
            var query = new AsyncQuery(this, CommandCode.CreateDictionary);
            query.AddAnsi(database);
            var response = await ExecuteAsync(query);

            return response?.CheckReturnCode() ?? false;
        } // method CreateDictionaryAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.DeleteDatabaseAsync"/>
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
            var query = new AsyncQuery(this, CommandCode.DeleteDatabase);
            query.AddAnsi(database);
            var response = await ExecuteAsync(query);

            return response?.CheckReturnCode() ?? false;
        } // method DeleteDatabaseAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.DisconnectAsync"/>
        public async Task<bool> DisconnectAsync()
        {
            if (Connected)
            {
                var query = new AsyncQuery(this, CommandCode.UnregisterClient);
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

        /// <inheritdoc cref="IAsyncIrbisProvider.FormatRecordsAsync"/>
        public Task<bool> FormatRecordsAsync
            (
                FormatRecordParameters parameters
            )
        {
            throw new NotImplementedException();
        } // method FormatRecordsAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.FullTextSearchAsync"/>
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

            var query = new AsyncQuery(this, CommandCode.NewFulltextSearch);
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

        /// <inheritdoc cref="IAsyncIrbisProvider.GetDatabaseInfoAsync"/>
        public Task<DatabaseInfo?> GetDatabaseInfoAsync
            (
                string? databaseName = default
            )
        {
            throw new NotImplementedException();
        } // method GetDatabaseInfoAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.GetMaxMfnAsync"/>
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

        /// <inheritdoc cref="IAsyncIrbisProvider.GetServerStatAsync"/>
        public Task<ServerStat?> GetServerStatAsync()
        {
            throw new NotImplementedException();
        } // method GetServerStatAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.GetServerVersionAsync"/>
        public async Task<ServerVersion?> GetServerVersionAsync()
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(this, CommandCode.ServerInfo);
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

        /// <inheritdoc cref="IAsyncIrbisProvider.GlobalCorrectionAsync"/>
        public Task<GblResult?> GlobalCorrectionAsync
            (
                GblSettings settings
            )
        {
            throw new NotImplementedException();
        } // method GlobalCorrectionAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.ListFilesAsync"/>
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

            var query = new AsyncQuery(this, CommandCode.ListFiles);
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

        /// <inheritdoc cref="IAsyncIrbisProvider.ListProcessesAsync"/>
        public async Task<ProcessInfo[]?> ListProcessesAsync()
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(this, CommandCode.GetProcessList);
            var response = await ExecuteAsync(query);
            if (response is null)
            {
                return null;
            }

            response.CheckReturnCode();
            var result = ProcessInfo.Parse(response);

            return result;
        } // method ListProcessesAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.ListUsersAsync"/>
        public Task<UserInfo[]?> ListUsersAsync()
        {
            throw new NotImplementedException();
        } // method ListUsersAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.NoOperationAsync"/>
        public async Task<bool> NoOperationAsync()
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var response = await ExecuteAsync(CommandCode.Nop);

            return response?.CheckReturnCode() ?? false;
        } // method NoOperationAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.PrintTableAsync"/>
        public Task<string?> PrintTableAsync
            (
                TableDefinition definition
            )
        {
            throw new NotImplementedException();
        } // method PrintTableAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.ReadBinaryFileAsync"/>
        public Task<byte[]?> ReadBinaryFileAsync
            (
                FileSpecification specification
            )
        {
            throw new NotImplementedException();
        } // method ReadBinaryFileAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.ReadPostingsAsync"/>
        public async Task<TermPosting[]?> ReadPostingsAsync
            (
                PostingParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(this, CommandCode.ReadPostings);
            parameters.Encode(this, query);
            var response = await ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode(ConnectionUtility.GoodCodesForReadTerms))
            {
                return null;
            }

            return TermPosting.Parse(response);
        } // method ReadPostingsAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.ReadRecordAsync"/>
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
            var query = new AsyncQuery(this, CommandCode.ReadRecord);
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

        /// <inheritdoc cref="IAsyncIrbisProvider.ReadRecordPostingsAsync"/>
        public Task<TermPosting[]?> ReadRecordPostingsAsync
            (
                ReadRecordParameters parameters,
                string prefix
            )
        {
            throw new NotImplementedException();
        } // method ReadRecordPostingsAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.ReadTermsAsync"/>
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
            var query = new AsyncQuery(this, command);
            parameters.Encode(this, query);
            var response = await ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode(ConnectionUtility.GoodCodesForReadTerms))
            {
                return Array.Empty<Term>();
            }

            return Term.Parse(response);
        } // method ReadTermsAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.ReadTextFileAsync"/>
        public async Task<string?> ReadTextFileAsync
            (
                FileSpecification specification
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(this, CommandCode.ReadDocument);
            query.AddAnsi(specification.ToString());
            var response = await ExecuteAsync(query);
            if (response is null)
            {
                return null;
            }

            var result = IrbisText.IrbisToWindows(response.ReadAnsi());

            return result;
        } // method ReadTextFileAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.ReloadDictionaryAsync"/>
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

        /// <inheritdoc cref="IAsyncIrbisProvider.ReloadMasterFileAsync"/>
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

        /// <inheritdoc cref="IAsyncIrbisProvider.RestartServerAsync"/>
        public async Task<bool> RestartServerAsync()
        {
            var response = await ExecuteAsync(CommandCode.RestartServer);

            return response is not null;
        } // method RestartServerAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.SearchAsync"/>
        public async Task<FoundItem[]?> SearchAsync
            (
                SearchParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(this, CommandCode.Search);
            parameters.Encode(this, query);
            var response = await ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return Array.Empty<FoundItem>();
            }

            return FoundItem.Parse(response);
        } // method SearchAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.TruncateDatabaseAsync"/>
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

        /// <inheritdoc cref="IAsyncIrbisProvider.UnlockDatabaseAsync"/>
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

        /// <inheritdoc cref="IAsyncIrbisProvider.UnlockRecordsAsync"/>
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

            var query = new AsyncQuery(this, CommandCode.UnlockRecords);
            query.AddAnsi(databaseName ?? Database);
            foreach (var mfn in mfnList)
            {
                query.Add(mfn);
            }

            var response = await ExecuteAsync(query);

            return response is not null && response.CheckReturnCode();
        } // method UnlockRecordsAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.UpdateIniFileAsync"/>
        public async Task<bool> UpdateIniFileAsync
            (
                IEnumerable<string> lines
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new AsyncQuery(this, CommandCode.UpdateIniFile);
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

        /// <inheritdoc cref="IAsyncIrbisProvider.UpdateUserListAsync"/>
        public async Task<bool> UpdateUserListAsync
            (
                IEnumerable<UserInfo> users
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new AsyncQuery(this, CommandCode.SetUserList);
            foreach (var user in users)
            {
                query.AddAnsi(user.Encode());
            }

            var response = await ExecuteAsync(query);

            return response is not null;
        } // method UpdateUserListAsync

        /// <inheritdoc cref="IAsyncIrbisProvider.WriteRecordAsync"/>
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
            var query = new AsyncQuery(this, CommandCode.UpdateRecord);
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

        /// <inheritdoc cref="IAsyncIrbisProvider.WriteTextFileAsync"/>
        public async Task<bool> WriteTextFileAsync
            (
                FileSpecification specification
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new AsyncQuery(this, CommandCode.ReadDocument);
            query.AddAnsi(specification.ToString());

            var response = await ExecuteAsync(query);

            return response is not null;
        } // method WriteTextFileAsync

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public async ValueTask DisposeAsync()
        {
            if (Connected)
            {
                await DisconnectAsync().ConfigureAwait(false);
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
