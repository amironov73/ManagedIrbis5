// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeNotNullable
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

#pragma warning disable CA1816

/* SyncConnection.cs -- синхронное подключение к серверу ИРБИС64
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
    /// Синхронное подключение к серверу ИРБИС64.
    /// </summary>
    public class SyncConnection
        : ConnectionBase,
          ISyncConnection
    {
        #region Properties

        /// <summary>
        /// Сокет.
        /// </summary>
        public ISyncClientSocket Socket { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SyncConnection
            (
                ISyncClientSocket? socket = null,
                IServiceProvider? serviceProvider = null
            )
            : base (serviceProvider ?? Magna.Host.Services)
        {
            Socket = socket ?? new SyncTcp4Socket();
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
        public Response? ExecuteSync
            (
                string command,
                params object[] args
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            using var query = new SyncQuery(this, command);
            foreach (var arg in args)
            {
                query.AddAnsi(arg.ToString());
            }

            var result = ExecuteSync(query);

            return result;

        } // method ExecuteSync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <returns>Ответ сервера.</returns>
        public Response? ExecuteSync
            (
                string command
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            using var query = new SyncQuery(this, command);
            var result = ExecuteSync(query);

            return result;

        } // method ExecuteSync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="arg1">Первый и единственный параметр команды.</param>
        /// <returns>Ответ сервера.</returns>
        public Response? ExecuteSync
            (
                string command,
                object arg1
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            using var query = new SyncQuery(this, command);
            query.AddAnsi(arg1.ToString());

            var result = ExecuteSync(query);

            return result;

        } // method ExecuteSync

        #endregion

        #region ISyncConnection members

        /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        public Response? ExecuteSync
            (
                SyncQuery query
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

                    result = Socket.TransactSync(query);
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

                /*
                if (_debug)
                {
                    result.Debug(Console.Out);
                }
                */

                result.Parse();
                Interlocked.Increment(ref _queryId);

                return result;
            }
            finally
            {
                SetBusy(false);
            }

        } // method ExecuteSync

        /// <summary>
        /// Получение статистики по базе данных.
        /// </summary>
        public string? GetDatabaseStat
            (
                StatDefinition definition
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            using var query = new SyncQuery(this, CommandCode.DatabaseStat);
            definition.Encode(this, query);

            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = "{\\rtf1 "
                + response.ReadRemainingUtfText()
                + "}";

            return result;

        } // method GetDatabaseStat

        /// <summary>
        /// Переподключение к серверу.
        /// </summary>
        public bool Reconnect()
        {
            if (Connected)
            {
                Disconnect();
            }

            IniFile?.Dispose();
            IniFile = null;

            return Connect();

        } // method Reconnect

        /// <summary>
        /// Остановка сервера (расширенная команда).
        /// </summary>
        public bool StopServer()
        {
            var result = ExecuteSync("STOP") is not null;
            Connected = false;

            return result;

        } // method StopServer

        /// <summary>
        /// Разблокирование указанной записи (альтернативный вариант).
        /// </summary>
        public bool UnlockRecordAlt(int mfn) =>
            ExecuteSync("E", EnsureDatabase(), mfn).IsGood();

        #endregion

        #region ISyncProvider members

        /// <inheritdoc cref="ISyncProvider.ActualizeRecord"/>
        public bool ActualizeRecord (ActualizeRecordParameters parameters) =>
            ExecuteSync
                (
                    CommandCode.ActualizeRecord,
                    EnsureDatabase(parameters.Database),
                    parameters.Mfn
                ).IsGood();

        /// <inheritdoc cref="ISyncProvider.Connect"/>
        public bool Connect()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN:
            LastError = 0;
            QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            Response? response;
            using (var query = new SyncQuery(this, CommandCode.RegisterClient))
            {
                query.AddAnsi(Username);
                query.AddAnsi(Password);

                response = ExecuteSync(query);
                if (ReferenceEquals(response, null))
                {
                    LastError = -100_500;
                    return false;
                }
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

        } // method Connect

        /// <inheritdoc cref="ISyncProvider.CreateDatabase"/>
        public bool CreateDatabase
            (
                CreateDatabaseParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            using var query = new SyncQuery(this, CommandCode.CreateDatabase);
            query.AddAnsi(EnsureDatabase(parameters.Database));
            query.AddAnsi(parameters.Description);
            query.Add(parameters.ReaderAccess);
            var response = ExecuteSync(query);

            return response.IsGood();

        } // method CreateDatabase

        /// <inheritdoc cref="ISyncProvider.CreateDictionary"/>
        public bool CreateDictionary (string? databaseName = default) =>
            ExecuteSync(CommandCode.CreateDictionary,
                EnsureDatabase(databaseName)).IsGood();

        /// <inheritdoc cref="ISyncProvider.DeleteDatabase"/>
        public bool DeleteDatabase (string? databaseName = default) =>
            ExecuteSync(CommandCode.DeleteDatabase,
                EnsureDatabase(databaseName)).IsGood();

        /// <inheritdoc cref="ISyncProvider.Disconnect"/>
        public bool Disconnect()
        {
            if (Connected)
            {
                try
                {
                    OnDisposing();

                    ExecuteSync(CommandCode.UnregisterClient);
                }
                catch (Exception exception)
                {
                    _logger.LogError
                        (
                            exception,
                            nameof(SyncConnection) + "::" + nameof(Disconnect)
                        );
                }

                Connected = false;
            }

            return true;

        } // method Disconnect

        /// <inheritdoc cref="ISyncProvider.FileExist"/>
        public bool FileExist(FileSpecification specification) =>
            !string.IsNullOrEmpty(ReadTextFile(specification));

        /// <inheritdoc cref="ISyncProvider.FormatRecords"/>
        public bool FormatRecords
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

            using var query = new SyncQuery(this, CommandCode.FormatRecord);
            query.AddAnsi(EnsureDatabase(parameters.Database));
            query.AddFormat(parameters.Format);
            query.Add(parameters.Mfns.Length);
            foreach (var mfn in parameters.Mfns)
            {
                query.Add(mfn);
            }

            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return false;
            }

            parameters.Result = response.ReadRemainingUtfLines();

            return true;

        } // method FormatRecords

        /// <inheritdoc cref="ISyncProvider.FullTextSearch"/>
        public FullTextResult? FullTextSearch
            (
                SearchParameters searchParameters,
                TextParameters textParameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            using var query = new SyncQuery(this, CommandCode.NewFulltextSearch);
            searchParameters.Encode(this, query);
            textParameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = new FullTextResult();
            result.Decode(response);

            return result;

        } // method FullTextSearch

        /// <inheritdoc cref="IAsyncProvider.GetDatabaseInfoAsync"/>
        public DatabaseInfo? GetDatabaseInfo(string? databaseName = default) =>
            ExecuteSync(CommandCode.RecordList, EnsureDatabase(databaseName))
                .Transform
                    (
                        resp => DatabaseInfo.Parse
                            (
                                EnsureDatabase(databaseName),
                                resp
                            )
                    );

        /// <inheritdoc cref="ISyncProvider.GetMaxMfn"/>
        public int GetMaxMfn
            (
                string? databaseName = default
            )
        {
            var response = ExecuteSync(CommandCode.GetMaxMfn, EnsureDatabase(databaseName));

            return response.IsGood() ? response.ReturnCode : 0;

        } // method GetMaxMfn

        /// <inheritdoc cref="ISyncProvider.GetServerStat"/>
        public ServerStat? GetServerStat() =>
            ExecuteSync(CommandCode.GetServerStat).Transform(ServerStat.Parse);

        /// <inheritdoc cref="ISyncProvider.GetServerVersion"/>
        public ServerVersion? GetServerVersion() =>
            ExecuteSync(CommandCode.ServerInfo).Transform(ManagedIrbis.ServerVersion.Parse);

        /// <inheritdoc cref="ISyncProvider.GlobalCorrection"/>
        public GblResult? GlobalCorrection
            (
                GblSettings settings
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var database = EnsureDatabase(settings.Database);
            using var query = new SyncQuery(this, CommandCode.GlobalCorrection);
            query.AddAnsi(database);
            settings.Encode(query);

            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = new GblResult();
            result.Parse(response);

            return result;

        } // method GlobalCorrection

        /// <inheritdoc cref="ISyncProvider.ListFiles"/>
        public string[]? ListFiles
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

            using var query = new SyncQuery(this, CommandCode.ListFiles);
            foreach (var specification in specifications)
            {
                query.AddAnsi(specification.ToString());
            }

            var response = ExecuteSync(query);

            return SyncConnectionUtility.ListFiles(response);

        } // method ListFiles

        /// <inheritdoc cref="ISyncProvider.ListProcesses"/>
        public ProcessInfo[]? ListProcesses() =>
            ExecuteSync(CommandCode.GetProcessList).Transform(ProcessInfo.Parse);

        /// <inheritdoc cref="ISyncProvider.ListUsers"/>
        public UserInfo[]? ListUsers() =>
            ExecuteSync(CommandCode.GetUserList).Transform(UserInfo.Parse);

        /// <inheritdoc cref="ISyncProvider.NoOperation"/>
        public bool NoOperation() => ExecuteSync(CommandCode.Nop).IsGood();

        /// <inheritdoc cref="ISyncProvider.PrintTable"/>
        public string? PrintTable
            (
                TableDefinition definition
            )
        {
            using var query = new SyncQuery(this, CommandCode.Print);
            query.AddAnsi(EnsureDatabase(definition.DatabaseName));
            definition.Encode(query);

            var response = ExecuteSync(query);

            return response?.ReadRemainingUtfText();

        } // method PrintTable

        /// <inheritdoc cref="ISyncProvider.ReadBinaryFile"/>
        public byte[]? ReadBinaryFile
            (
                FileSpecification specification
            )
        {
            specification.BinaryFile = true;
            var response = ExecuteSync(CommandCode.ReadDocument, specification.ToString());
            if (response is null || !response.FindPreamble())
            {
                return null;
            }

            return response.RemainingBytes();

        } // method ReadBinaryFile

        /// <inheritdoc cref="ISyncProvider.ReadPostings"/>
        public TermPosting[]? ReadPostings
            (
                PostingParameters parameters
            )
        {
            using var query = new SyncQuery(this, CommandCode.ReadPostings);
            parameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (!response.IsGood(ConnectionUtility.GoodCodesForReadTerms))
            {
                return null;
            }

            return TermPosting.Parse(response);

        } // method ReadPosting

        /// <inheritdoc cref="ISyncProvider.ReadRecord{T}"/>
        public T? ReadRecord<T>
            (
                ReadRecordParameters parameters
            )
            where T: class, IRecord, new()
        {
            T? result;

            try
            {
                var database = EnsureDatabase(parameters.Database);
                using var query = new SyncQuery(this, CommandCode.ReadRecord);
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

                var response = ExecuteSync(query);
                if (!response.IsGood(ConnectionUtility.GoodCodesForReadRecord))
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
                    UnlockRecords(new [] { parameters.Mfn });
                }
            }
            catch (Exception exception)
            {
                throw new IrbisException
                (
                    nameof(ReadRecord) + " " + parameters,
                    exception
                );
            }

            return result;

        } // method ReadRecord

        /// <inheritdoc cref="ISyncProvider.ReadRecordPostings"/>
        public TermPosting[]? ReadRecordPostings
            (
                ReadRecordParameters parameters,
                string prefix
            )
        {
            if (!CheckProviderState() || string.IsNullOrEmpty(prefix))
            {
                return null;
            }

            using var query = new SyncQuery(this, CommandCode.GetRecordPostings);
            query.AddAnsi(EnsureDatabase(parameters.Database));
            query.Add(parameters.Mfn);
            query.AddUtf(prefix);

            return ExecuteSync(query).Transform(TermPosting.Parse);

        } // method ReadRecordPostings

        /// <inheritdoc cref="ISyncProvider.ReadTerms"/>
        public Term[]? ReadTerms
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
            using var query = new SyncQuery(this, command);
            parameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (!response.IsGood(ConnectionUtility.GoodCodesForReadTerms))
            {
                return null;
            }

            return Term.Parse(response);

        } // method ReadTerms

        /// <inheritdoc cref="IAsyncProvider.ReadTextFileAsync"/>
        public string? ReadTextFile (FileSpecification specification) =>
            ExecuteSync(CommandCode.ReadDocument, specification.ToString())
                .TransformNoCheck
                    (
                        resp => IrbisText.IrbisToWindows(resp.ReadAnsi())
                    );

        /// <inheritdoc cref="ISyncProvider.ReloadDictionary"/>
        public bool ReloadDictionary(string? databaseName = default) =>
            ExecuteSync(CommandCode.ReloadDictionary, EnsureDatabase(databaseName)).IsGood();

        /// <inheritdoc cref="ISyncProvider.ReloadMasterFile"/>
        public bool ReloadMasterFile (string? databaseName = default) =>
            ExecuteSync(CommandCode.ReloadMasterFile,
                databaseName ?? Database.ThrowIfNull(nameof(Database))).IsGood();

        /// <inheritdoc cref="ISyncProvider.RestartServer"/>
        public bool RestartServer() => ExecuteSync(CommandCode.RestartServer).IsGood();

        /// <inheritdoc cref="ISyncProvider.Search"/>
        public FoundItem[]? Search
            (
                SearchParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            using var query = new SyncQuery(this, CommandCode.Search);
            parameters.Encode(this, query);

            return ExecuteSync(query).Transform(FoundItem.Parse);

        } // method Search

        /// <inheritdoc cref="ISyncProvider.TruncateDatabase"/>
        public bool TruncateDatabase (string? databaseName = default) =>
            ExecuteSync(CommandCode.EmptyDatabase, EnsureDatabase(databaseName)).IsGood();

        /// <inheritdoc cref="ISyncProvider.UnlockDatabase"/>
        public bool UnlockDatabase (string? databaseName = default) =>
            ExecuteSync(CommandCode.UnlockDatabase, EnsureDatabase(databaseName)).IsGood();

        /// <inheritdoc cref="ISyncProvider.UnlockRecords"/>
        public bool UnlockRecords
            (
                IEnumerable<int> mfnList,
                string? databaseName = default
            )
        {
            using var query = new SyncQuery(this, CommandCode.UnlockRecords);
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

            return ExecuteSync(query).IsGood();

        } // method UnlockRecords

        /// <inheritdoc cref="ISyncProvider.UpdateIniFile"/>
        public bool UpdateIniFile
            (
                IEnumerable<string> lines
            )
        {
            using var query = new SyncQuery(this, CommandCode.UpdateIniFile);
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

            return ExecuteSync(query).IsGood();

        } // method UpdateIniFile

        /// <inheritdoc cref="ISyncProvider.UpdateUserList"/>
        public bool UpdateUserList
            (
                IEnumerable<UserInfo> users
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            using var query = new SyncQuery(this, CommandCode.SetUserList);
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

            return ExecuteSync(query).IsGood();

        } // method UpdateUserList

        /// <inheritdoc cref="ISyncProvider.WriteRecord"/>
        public bool WriteRecord
            (
                WriteRecordParameters parameters
            )
        {
            var record = parameters.Record;
            if (record is not null)
            {
                var database = EnsureDatabase(record.Database);
                using var query = new SyncQuery(this, CommandCode.UpdateRecord);
                query.AddAnsi(database);
                query.Add(parameters.Lock);
                query.Add(parameters.Actualize);
                query.AddUtf(record.Encode());

                var response = ExecuteSync(query);
                if (!response.IsGood())
                {
                    return false;
                }

                var result = response.ReturnCode;
                if (!parameters.DontParse)
                {
                    record.Database ??= database;
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
                var result2 = WriteRecord(parameters);
                parameters.Record = null;
                parameters.Records = records;

                return result2;
            }

            return this.WriteRecords
                (
                    records,
                    parameters.Lock,
                    parameters.Actualize,
                    parameters.DontParse
                );

        } // method WriteRecord

        /// <inheritdoc cref="ISyncProvider.WriteTextFile"/>
        public bool WriteTextFile(FileSpecification specification) =>
            ExecuteSync(CommandCode.ReadDocument, specification.ToString()).IsGood();

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public override ValueTask DisposeAsync()
        {
            Dispose();

            return ValueTask.CompletedTask;

        } // method DisposeAsync

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public override void Dispose()
        {
            Disconnect();

        } // method Dispose

        #endregion

    } // class SyncConnection

} // namespace ManagedIrbis
