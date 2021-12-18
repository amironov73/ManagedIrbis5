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
using ManagedIrbis.Performance;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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

            _logger = (ILogger) (serviceProvider?.GetService (typeof(ILogger<SyncConnection>))
                    ?? NullLogger.Instance);

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
            // не надо query.Dispose();

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
            // не надо query.Dispose();

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
            // не надо query.Dispose();

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
            PerfRecord? perfRecord = null;
            Stopwatch? stopwatch = null;
            if (_performanceCollector is not null)
            {
                stopwatch = new Stopwatch();
                stopwatch.Start();
                perfRecord = new PerfRecord
                {
                    Moment = DateTime.Now,
                    Host = Host,
                    Code = "none", // TODO: нужно где-то прикопать код операции
                    OutgoingSize = asyncQuery.GetLength()
                };

            } // if

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
                    if (perfRecord is not null)
                    {
                        perfRecord.ElapsedTime = (int) stopwatch!.ElapsedMilliseconds;
                        perfRecord.ErrorMessage = exception.Message;
                        _performanceCollector!.Collect (perfRecord);
                    }

                    return null;
                }

                if (result is null)
                {
                    if (perfRecord is not null)
                    {
                        perfRecord.ElapsedTime = (int) stopwatch!.ElapsedMilliseconds;
                        perfRecord.ErrorMessage = "No response";
                        _performanceCollector!.Collect (perfRecord);
                    }

                    return null;
                }

                result.Parse();
                if (perfRecord is not null)
                {
                    perfRecord.ElapsedTime = (int) stopwatch!.ElapsedMilliseconds;
                    perfRecord.IncomingSize = result.AnswerSize;
                    _performanceCollector!.Collect (perfRecord);
                }

                Interlocked.Increment(ref _queryId);

                return result;
            }
            finally
            {
                SetBusy(false);
                asyncQuery.Dispose();
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

            using var response = await ExecuteAsync(query);
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
            using var response = await ExecuteAsync("STOP");
            var result = response is not null;
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

            // нельзя использовать using из-за goto
            var response = await ExecuteAsync(query);
            if (response is null)
            {
                LastError = -100_500;
                return false;
            }

            if (response.GetReturnCode() == -3337)
            {
                response.Dispose();
                goto AGAIN;
            }

            if (response.ReturnCode < 0)
            {
                LastError = response.ReturnCode;
                response.Dispose();
                return false;
            }

            try
            {
                ServerVersion = response.ServerVersion;
                Interval = response.ReadInteger();

                IniFile = new IniFile();
                var remainingText = response.RemainingText(IrbisEncoding.Ansi);
                var reader = new StringReader(remainingText);
                IniFile.Read(reader);
                Connected = true;
            }
            finally
            {
                response.Dispose();
            }

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
            using var response = await ExecuteAsync(query);
            query.Dispose();

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
                    using var _ = await ExecuteAsync(CommandCode.UnregisterClient);
                }
                catch (Exception exception)
                {
                    _logger?.LogError
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

            using var response = await ExecuteAsync(query);
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
            using var response = await ExecuteAsync(query);
            if (!response.IsGood(false))
            {
                return null;
            }

            var result = new FullTextResult();
            result.Decode(response);

            return result;

        } // method FullTextSearchAsync

        /// <inheritdoc cref="IAsyncProvider.GetDatabaseInfoAsync"/>
        public async Task<DatabaseInfo?> GetDatabaseInfoAsync (string? databaseName = default) =>
            await ExecuteAsync(CommandCode.RecordList, EnsureDatabase(databaseName))
                .TransformAsync
                (
                    resp => DatabaseInfo.Parse
                    (
                        EnsureDatabase(databaseName),
                        resp
                    )
                );

        /// <inheritdoc cref="IAsyncProvider.GetMaxMfnAsync"/>
        public async Task<int> GetMaxMfnAsync
            (
                string? databaseName = default
            )
        {
            using var response = await ExecuteAsync(CommandCode.GetMaxMfn, EnsureDatabase(databaseName));

            return response.IsGood(false) ? response.ReturnCode : 0;

        } // method GetMaxMfnAsync

        /// <inheritdoc cref="IAsyncProvider.GetServerStatAsync"/>
        public async Task<ServerStat?> GetServerStatAsync() =>
            await ExecuteAsync(CommandCode.GetServerStat).TransformAsync(ServerStat.Parse);

        /// <inheritdoc cref="IAsyncProvider.GetServerVersionAsync"/>
        public async Task<ServerVersion?> GetServerVersionAsync() =>
            await ExecuteAsync(CommandCode.ServerInfo).TransformAsync(ManagedIrbis.ServerVersion.Parse);

        /// <inheritdoc cref="IAsyncProvider.GlobalCorrectionAsync"/>
        public async Task<GblResult?> GlobalCorrectionAsync
            (
                GblSettings settings
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var database = EnsureDatabase(settings.Database);
            var query = new AsyncQuery(this, CommandCode.GlobalCorrection);
            query.AddAnsi(database);
            settings.Encode(query);

            using var response = await ExecuteAsync(query);
            if (!response.IsGood(false))
            {
                return null;
            }

            var result = new GblResult();
            result.Parse(response);

            return result;

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

            using var response = await ExecuteAsync(query);
            if (response is null)
            {
                return null;
            }

            return SyncConnectionUtility.ListFiles(response);

        } // method ListFilesAsync

        /// <inheritdoc cref="IAsyncProvider.ListProcessesAsync"/>
        public async Task<ProcessInfo[]?> ListProcessesAsync() =>
            await ExecuteAsync(CommandCode.GetProcessList).TransformAsync(ProcessInfo.Parse);

        /// <inheritdoc cref="IAsyncProvider.ListUsersAsync"/>
        public async Task<UserInfo[]?> ListUsersAsync() =>
            await ExecuteAsync(CommandCode.GetUserList).TransformAsync(UserInfo.Parse);

        /// <inheritdoc cref="IAsyncProvider.NoOperationAsync"/>
        public async Task<bool> NoOperationAsync() => await ExecuteAsync(CommandCode.Nop).IsGoodAsync();

        /// <inheritdoc cref="IAsyncProvider.PrintTableAsync"/>
        public async Task<string?> PrintTableAsync
            (
                TableDefinition definition
            )
        {
            var query = new AsyncQuery(this, CommandCode.Print);
            query.AddAnsi(EnsureDatabase(definition.DatabaseName));
            definition.Encode(query);

            using var response = await ExecuteAsync(query);

            return response?.ReadRemainingUtfText();

        } // method PrintTableAsync

        /// <inheritdoc cref="IAsyncProvider.ReadBinaryFileAsync"/>
        public async Task<byte[]?> ReadBinaryFileAsync
            (
                FileSpecification specification
            )
        {
            specification.BinaryFile = true;
            using var response = await ExecuteAsync(CommandCode.ReadDocument, specification.ToString());
            if (response is null || !response.FindPreamble())
            {
                return null;
            }

            return response.RemainingBytes();

        } // method ReadBinaryFileAsync

        /// <inheritdoc cref="IAsyncProvider.ReadPostingsAsync"/>
        public async Task<TermPosting[]?> ReadPostingsAsync
            (
                PostingParameters parameters
            )
        {
            var query = new AsyncQuery(this, CommandCode.ReadPostings);
            parameters.Encode(this, query);

            using var response = await ExecuteAsync(query);
            if (!response.IsGood(false, ConnectionUtility.GoodCodesForReadTerms))
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
            T? result;

            try
            {
                var database = EnsureDatabase(parameters.Database);
                var query = new AsyncQuery(this, CommandCode.ReadRecord);
                query.AddAnsi(database);
                query.Add(parameters.Mfn);
                if (parameters.Version != 0)
                {
                    query.Add(parameters.Version);
                }
                else
                {
                    query.Add(parameters.Lock);
                }

                query.AddFormat(parameters.Format);

                using var response = await ExecuteAsync(query);
                if (!response.IsGood(false, ConnectionUtility.GoodCodesForReadRecord))
                {
                    return null;
                }

                result = new T
                {
                    Database = Database
                };

                switch ((ReturnCode) response.ReturnCode)
                {
                    case ReturnCode.PreviousVersionNotExist:
                        result.Status |= RecordStatus.Absent;
                        break;

                    case ReturnCode.PhysicallyDeleted:
                    case ReturnCode.PhysicallyDeleted1:
                        result.Status |= RecordStatus.PhysicallyDeleted;
                        break;

                    default:
                        result.Decode(response);
                        break;
                }

                if (parameters.Version != 0)
                {
                    await UnlockRecordsAsync(new [] { parameters.Mfn });
                }
            }
            catch (Exception exception)
            {
                throw new IrbisException
                (
                    nameof(ReadRecordAsync) + " " + parameters,
                    exception
                );
            }

            return result;

        } // method ReadRecordAsync

        /// <inheritdoc cref="IAsyncProvider.ReadRecordPostingsAsync"/>
        public async Task<TermPosting[]?> ReadRecordPostingsAsync
            (
                ReadRecordParameters parameters,
                string prefix
            )
        {
            if (!CheckProviderState() || string.IsNullOrEmpty(prefix))
            {
                return null;
            }

            var query = new AsyncQuery(this, CommandCode.GetRecordPostings);
            query.AddAnsi(EnsureDatabase(parameters.Database));
            query.Add(parameters.Mfn);
            query.AddUtf(prefix);

            return await ExecuteAsync(query).TransformAsync(TermPosting.Parse);

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
            using var response = await ExecuteAsync(query);

            return !response.IsGood(false, ConnectionUtility.GoodCodesForReadTerms)
                ? null
                : Term.Parse(response);

        } // method ReadTermsAsync

        /// <inheritdoc cref="IAsyncProvider.ReadTextFileAsync"/>
        public async Task<string?> ReadTextFileAsync (FileSpecification specification) =>
            await ExecuteAsync(CommandCode.ReadDocument, specification.ToString())
                .TransformNoCheckAsync
                (
                    resp => IrbisText.IrbisToWindows(resp.ReadAnsi())
                );

        /// <inheritdoc cref="IAsyncProvider.ReloadDictionaryAsync"/>
        public async Task<bool> ReloadDictionaryAsync (string? databaseName = default) =>
            await ExecuteAsync(CommandCode.ReloadDictionary, EnsureDatabase(databaseName)).IsGoodAsync();

        /// <inheritdoc cref="IAsyncProvider.ReloadMasterFileAsync"/>
        public async Task<bool> ReloadMasterFileAsync (string? databaseName = default) =>
            await ExecuteAsync(CommandCode.ReloadMasterFile, EnsureDatabase(databaseName)).IsGoodAsync();

        /// <inheritdoc cref="IAsyncProvider.RestartServerAsync"/>
        public async Task<bool> RestartServerAsync() =>
            await ExecuteAsync(CommandCode.RestartServer).IsGoodAsync();

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

            return await ExecuteAsync(query).TransformAsync(FoundItem.Parse);

        } // method SearchAsync

        /// <inheritdoc cref="IAsyncProvider.TruncateDatabaseAsync"/>
        public async Task<bool> TruncateDatabaseAsync (string? databaseName = default) =>
            await ExecuteAsync(CommandCode.EmptyDatabase, EnsureDatabase(databaseName)).IsGoodAsync();

        /// <inheritdoc cref="IAsyncProvider.UnlockDatabaseAsync"/>
        public async Task<bool> UnlockDatabaseAsync(string? databaseName = default) =>
            await ExecuteAsync(CommandCode.UnlockDatabase, EnsureDatabase(databaseName)).IsGoodAsync();

        /// <inheritdoc cref="IAsyncProvider.UnlockRecordsAsync"/>
        public async Task<bool> UnlockRecordsAsync
            (
                IEnumerable<int> mfnList,
                string? databaseName = default
            )
        {
            var query = new AsyncQuery(this, CommandCode.UnlockRecords);
            query.AddAnsi(EnsureDatabase(databaseName));
            var counter = 0;
            foreach (var mfn in mfnList)
            {
                query.Add(mfn);
                ++counter;
            }

            // Если список MFN пуст, считаем операцию успешной
            if (counter == 0)
            {
                return true;
            }

            return await ExecuteAsync(query).IsGoodAsync();

        } // method UnlockRecordsAsync

        /// <inheritdoc cref="IAsyncProvider.UpdateIniFileAsync"/>
        public async Task<bool> UpdateIniFileAsync
            (
                IEnumerable<string> lines
            )
        {
            var query = new AsyncQuery(this, CommandCode.UpdateIniFile);
            var counter = 0;
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    query.AddAnsi(line);
                    ++counter;
                }
            }

            // Если список обновляемых строк пуст, считаем операцию успешной
            if (counter == 0)
            {
                return true;
            }

            return await ExecuteAsync(query).IsGoodAsync();

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
            var counter = 0;
            foreach (var user in users)
            {
                query.AddAnsi(user.Encode());
                ++counter;
            }

            // Если список обновляемых пользователей пуст, считаем операцию неуспешной
            if (counter == 0)
            {
                return false;
            }

            return await ExecuteAsync(query).IsGoodAsync();

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

                using var response = await ExecuteAsync(query);
                if (!response.IsGood(false))
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

            return await this.WriteRecordsAsync
                (
                    records,
                    parameters.Lock,
                    parameters.Actualize,
                    parameters.DontParse
                );

        } // method WriteRecordAsync

        /// <inheritdoc cref="IAsyncProvider.WriteTextFileAsync"/>
        public async Task<bool> WriteTextFileAsync (FileSpecification specification) =>
            await ExecuteAsync(CommandCode.ReadRecord, specification.ToString()).IsGoodAsync();

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
