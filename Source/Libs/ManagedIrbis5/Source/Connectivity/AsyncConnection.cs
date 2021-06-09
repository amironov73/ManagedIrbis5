// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* AsyncConnection.cs -- асинхронное подключение к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Асинхронное подключение к серверу ИРБИС64.
    /// </summary>
    public class AsyncConnection
        : ConnectionBase,
          IAsyncConnection
    {
        #region Properties

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
                IAsyncClientSocket? socket = null,
                IServiceProvider? serviceProvider = null
            )
            : base (serviceProvider ?? Magna.Host.Services)
        {
            Socket = socket ?? new AsyncTcp4Socket();
            Socket.Connection = this;

        } // constructor

        #endregion

        #region Public methods

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

        #region IAsyncConnection members

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

                if (result is null)
                {
                    return null;
                }

                result.Parse();
                Interlocked.Increment(ref _queryId);

                return result;
            }
            finally
            {
                SetBusy(false);
            }

        } // method ExecuteAsync

        /// <summary>
        /// Получение статистики по базе данных.
        /// </summary>
        public async Task<string?> GetDatabaseStatAsync
            (
                StatDefinition definition
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new AsyncQuery(this, CommandCode.DatabaseStat);
            definition.Encode(this, query);

            var response = await ExecuteAsync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = "{\\rtf1 "
                + response.ReadRemainingUtfText()
                + "}";

            return result;

        } // method GetDatabaseStatAsync

        /// <summary>
        /// Переподключение к серверу.
        /// </summary>
        public async Task<bool> ReconnectAsync()
        {
            if (Connected)
            {
                await DisconnectAsync();
            }

            IniFile?.Dispose();
            IniFile = null;

            return await ConnectAsync();

        } // method ReconnectAsync

        /// <summary>
        /// Остановка сервера (расширенная команда).
        /// </summary>
        public async Task<bool> StopServerAsync()
        {
            var result = await ExecuteAsync("STOP") is not null;
            Connected = false;

            return result;

        } // method StopServerAsync

        /// <summary>
        /// Разблокирование указанной записи (альтернативный вариант).
        /// </summary>
        public async Task<bool> UnlockRecordAltAsync(int mfn) =>
            await ExecuteAsync("E", EnsureDatabase(), mfn).IsGoodAsync();

        #endregion

        #region IAsyncProvider members

        /// <inheritdoc cref="IAsyncProvider.ActualizeRecordAsync"/>
        public async Task<bool> ActualizeRecordAsync(ActualizeRecordParameters parameters) =>
            await ExecuteAsync
                    (
                        CommandCode.ActualizeRecord,
                        EnsureDatabase(parameters.Database),
                        parameters.Mfn
                    )
                .IsGoodAsync();

        /// <inheritdoc cref="IAsyncProvider.ConnectAsync"/>
        public async Task<bool> ConnectAsync()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN:
            LastError = 0;
            QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            var query = new AsyncQuery(this, CommandCode.RegisterClient);
            query.AddAnsi(Username);
            query.AddAnsi(Password);

            var response = await ExecuteAsync(query);
            if (response is null)
            {
                LastError = -100_500;
                return false;
            }

            if (response.GetReturnCode() == -3337)
            {
                goto AGAIN;
            }

            if (response.ReturnCode < 0)
            {
                LastError = response.ReturnCode;
                return false;
            }

            Connected = true;
            ServerVersion = response.ServerVersion;
            Interval = response.ReadInteger();

            IniFile = new IniFile();
            var remainingText = response.RemainingText(IrbisEncoding.Ansi);
            var reader = new StringReader(remainingText);
            IniFile.Read(reader);

            return true;

        } // method ConnectAsync

        /// <inheritdoc cref="IAsyncProvider.CreateDatabaseAsync"/>
        public async Task<bool> CreateDatabaseAsync
            (
                CreateDatabaseParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new AsyncQuery(this, CommandCode.CreateDatabase);
            query.AddAnsi(EnsureDatabase(parameters.Database));
            query.AddAnsi(parameters.Description);
            query.Add(parameters.ReaderAccess);
            var response = await ExecuteAsync(query);

            return response.IsGood();

        } // method CreateDatabaseAsync

        /// <inheritdoc cref="IAsyncProvider.CreateDictionaryAsync"/>
        public async Task<bool> CreateDictionaryAsync (string? databaseName = default) =>
            await ExecuteAsync(CommandCode.CreateDictionary,
                EnsureDatabase(databaseName)).IsGoodAsync();

        /// <inheritdoc cref="IAsyncProvider.DeleteDatabaseAsync"/>
        public async Task<bool> DeleteDatabaseAsync (string? databaseName = default) =>
            await ExecuteAsync(CommandCode.DeleteDatabase,
                EnsureDatabase(databaseName)).IsGoodAsync();

        /// <inheritdoc cref="IAsyncProvider.DisconnectAsync"/>
        public async Task<bool> DisconnectAsync()
        {
            if (Connected)
            {
                await OnDisposingAsync();

                try
                {
                    await ExecuteAsync(CommandCode.UnregisterClient);
                }
                catch (Exception exception)
                {
                    _logger.LogError
                        (
                            exception,
                            nameof(SyncConnection) + "::" + nameof(DisconnectAsync)
                        );
                }

                Connected = false;
            }

            return true;

        } // method DisconnectAsync

        /// <inheritdoc cref="IAsyncProvider.FileExistAsync"/>
        public async Task<bool> FileExistAsync (FileSpecification specification) =>
            !string.IsNullOrEmpty (await ReadTextFileAsync(specification));

        /// <inheritdoc cref="IAsyncProvider.FormatRecordsAsync"/>
        public async Task<bool> FormatRecordsAsync
            (
                FormatRecordParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            // TODO: обнаруживать Records

            if (parameters.Mfns.IsNullOrEmpty())
            {
                return false;
            }

            var query = new AsyncQuery(this, CommandCode.FormatRecord);
            query.AddAnsi(EnsureDatabase(parameters.Database));
            query.AddFormat(parameters.Format);
            query.Add(parameters.Mfns.Length);
            foreach (var mfn in parameters.Mfns)
            {
                query.Add(mfn);
            }

            var response = await ExecuteAsync(query);
            if (!response.IsGood())
            {
                return false;
            }

            parameters.Result = response.ReadRemainingUtfLines();

            return true;

        } // method FormatRecordsAsync

        /// <inheritdoc cref="IAsyncProvider.FullTextSearchAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.GetDatabaseInfoAsync"/>
        public Task<DatabaseInfo?> GetDatabaseInfoAsync
            (
                string? databaseName = default
            )
        {
            throw new NotImplementedException();

        } // method GetDatabaseInfoAsync

        /// <inheritdoc cref="IAsyncProvider.GetMaxMfnAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.GetServerStatAsync"/>
        public Task<ServerStat?> GetServerStatAsync()
        {
            throw new NotImplementedException();

        } // method GetServerStatAsync

        /// <inheritdoc cref="IAsyncProvider.GetServerVersionAsync"/>
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
            var result = ManagedIrbis.ServerVersion.Parse(response);

            return result;

        } // method GetServerVersionAsync

        /// <inheritdoc cref="IAsyncProvider.GlobalCorrectionAsync"/>
        public Task<GblResult?> GlobalCorrectionAsync
            (
                GblSettings settings
            )
        {
            throw new NotImplementedException();
        } // method GlobalCorrectionAsync

        /// <inheritdoc cref="IAsyncProvider.ListFilesAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.ListProcessesAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.ListUsersAsync"/>
        public Task<UserInfo[]?> ListUsersAsync()
        {
            throw new NotImplementedException();

        } // method ListUsersAsync

        /// <inheritdoc cref="IAsyncProvider.NoOperationAsync"/>
        public async Task<bool> NoOperationAsync()
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var response = await ExecuteAsync(CommandCode.Nop);

            return response?.CheckReturnCode() ?? false;

        } // method NoOperationAsync

        /// <inheritdoc cref="IAsyncProvider.PrintTableAsync"/>
        public Task<string?> PrintTableAsync
            (
                TableDefinition definition
            )
        {
            throw new NotImplementedException();

        } // method PrintTableAsync

        /// <inheritdoc cref="IAsyncProvider.ReadBinaryFileAsync"/>
        public Task<byte[]?> ReadBinaryFileAsync
            (
                FileSpecification specification
            )
        {
            throw new NotImplementedException();

        } // method ReadBinaryFileAsync

        /// <inheritdoc cref="IAsyncProvider.ReadPostingsAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.ReadRecordAsync{T}"/>
        public async Task<T?> ReadRecordAsync<T>
            (
                ReadRecordParameters parameters
            )
            where T: class, IRecord, new()
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var database =
                parameters.Database
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

            var result = new T
            {
                Database = Database
            };
            result.Decode(response);

            return result;

        } // method ReadRecordAsync

        /// <inheritdoc cref="IAsyncProvider.ReadRecordPostingsAsync"/>
        public Task<TermPosting[]?> ReadRecordPostingsAsync
            (
                ReadRecordParameters parameters,
                string prefix
            )
        {
            throw new NotImplementedException();
        } // method ReadRecordPostingsAsync

        /// <inheritdoc cref="IAsyncProvider.ReadTermsAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.ReadTextFileAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.ReloadDictionaryAsync"/>
        public async Task<bool> ReloadDictionaryAsync
            (
                string? databaseName = default
            )
        {
            var response = await ExecuteAsync
                (
                    CommandCode.ReloadDictionary,
                    databaseName ?? Database.ThrowIfNull("Database")
                );

            return response?.CheckReturnCode() ?? false;
        } // method ReloadDictionaryAsync

        /// <inheritdoc cref="IAsyncProvider.ReloadMasterFileAsync"/>
        public async Task<bool> ReloadMasterFileAsync
            (
                string? databaseName = default
            )
        {
            var response = await ExecuteAsync
                (
                    CommandCode.ReloadMasterFile,
                    databaseName ?? Database.ThrowIfNull("Database")
                );

            return response?.CheckReturnCode() ?? false;
        } // method ReloadMasterFileAsync

        /// <inheritdoc cref="IAsyncProvider.RestartServerAsync"/>
        public async Task<bool> RestartServerAsync()
        {
            var response = await ExecuteAsync(CommandCode.RestartServer);

            return response is not null;
        } // method RestartServerAsync

        /// <inheritdoc cref="IAsyncProvider.SearchAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.TruncateDatabaseAsync"/>
        public async Task<bool> TruncateDatabaseAsync
            (
                string? databaseName = default
            )
        {
            var response = await ExecuteAsync
                (
                    CommandCode.EmptyDatabase,
                    databaseName ?? Database.ThrowIfNull("Database")
                );

            return response is not null && response.CheckReturnCode();
        } // method TruncateDatabaseAsync

        /// <inheritdoc cref="IAsyncProvider.UnlockDatabaseAsync"/>
        public async Task<bool> UnlockDatabaseAsync
            (
                string? databaseName = default
            )
        {
            var response = await ExecuteAsync
                (
                    CommandCode.UnlockDatabase,
                    databaseName ?? Database.ThrowIfNull("Database")
                );

            return response is not null && response.CheckReturnCode();
        } // method UnlockDatabaseAsync

        /// <inheritdoc cref="IAsyncProvider.UnlockRecordsAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.UpdateIniFileAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.UpdateUserListAsync"/>
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

        /// <inheritdoc cref="IAsyncProvider.WriteRecordAsync"/>
        public async Task<bool> WriteRecordAsync
            (
                WriteRecordParameters parameters
            )
        {
            var record = parameters.Record;
            if (record is not null)
            {
                var database = EnsureDatabase(record.Database);
                var query = new AsyncQuery(this, CommandCode.UpdateRecord);
                query.AddAnsi(database);
                query.Add(parameters.Lock);
                query.Add(parameters.Actualize);
                query.AddUtf(record.Encode());

                var response = await ExecuteAsync(query);
                if (!response.IsGood())
                {
                    return false;
                }

                var result = response.ReturnCode;
                if (!parameters.DontParse)
                {
                    record.Database ??= database;
                    //ProtocolText.ParseResponseForWriteRecord(response, record);
                    record.Decode(response);
                }

                parameters.MaxMfn = result;

                return true;

            }

            var records = parameters.Records.ThrowIfNull(nameof(parameters.Records));
            if (records.Length == 0)
            {
                return true;
            }

            if (records.Length == 1)
            {
                parameters.Record = records[0];
                parameters.Records = null;
                var result2 = await WriteRecordAsync(parameters);
                parameters.Record = null;
                parameters.Records = records;

                return result2;
            }

            // return await this.WriteRecordsAsync
            //     (
            //         records,
            //         parameters.Lock,
            //         parameters.Actualize,
            //         parameters.DontParse
            //     );

            return false;

        } // method WriteRecordAsync

        /// <inheritdoc cref="IAsyncProvider.WriteTextFileAsync"/>
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
        public override async ValueTask DisposeAsync()
        {
            await DisconnectAsync();

        } // method DisposeAsync

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose() => DisposeAsync().GetAwaiter().GetResult();

        #endregion

        #region Object members

        #endregion

    } // class AsyncConnection

} // namespace ManagedIrbis
