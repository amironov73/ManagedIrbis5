// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SyncConnection.cs -- синхронное подключение к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.IO;
using AM.PlatformAbstraction;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Синхронное подключение к серверу ИРБИС64.
    /// </summary>
    public sealed class SyncConnection
        : ISyncConnection
    {
        #region Events

        /// <summary>
        /// Fired when <see cref="Busy"/> changed.
        /// </summary>
        public event EventHandler? BusyChanged;

        /// <inheritdoc cref="IBasicIrbisProvider.Disposing"/>
        public event EventHandler? Disposing;

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
        public ISyncClientSocket Socket { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SyncConnection
            (
                ISyncClientSocket socket,
                IServiceProvider provider
            )
        {
            Socket = socket;
            socket.Connection = this;
            _cancellation = new CancellationTokenSource();
            Cancellation = _cancellation.Token;
            _logger = Magna.Factory.CreateLogger<IBasicIrbisProvider>();
            _provider = provider;
            PlatformAbstraction = PlatformAbstractionLayer.Current;
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

            var query = new SyncQuery(this, command);
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

            var query = new SyncQuery(this, command);
            var result = ExecuteSync(query);

            return result;
        } // method ExecuteSync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="arg1">Параметр команды.</param>
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

            var query = new SyncQuery(this, command);
            query.AddAnsi(arg1.ToString());

            var result = ExecuteSync(query);

            return result;
        } // method ExecuteSync

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

        /// <inheritdoc cref="IBasicIrbisProvider.GetGeneration"/>
        public string GetGeneration() => "64";

        /// <inheritdoc cref="IBasicIrbisProvider.GetWaitHandle"/>
        public WaitHandle GetWaitHandle()
        {
            throw new NotImplementedException();
        } // method GetWaitHandle

        public PlatformAbstractionLayer PlatformAbstraction { get; set; }

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

                if (ReferenceEquals(result, null))
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
                QueryId++;

                return result;
            }
            finally
            {
                SetBusy(false);
            }
        } // method ExecuteSync

        #endregion

        #region ISyncIrbisProvider members

        /// <inheritdoc cref="ISyncIrbisProvider.ActualizeRecord"/>
        public bool ActualizeRecord
            (
                ActualizeRecordParameters parameters
            )
        {
            var database = parameters.Database
                           ?? Database
                           ?? throw new IrbisException();

            var response = ExecuteSync
                (
                    CommandCode.ActualizeRecord,
                    database,
                    parameters.Mfn
                );

            return response is not null;
        } // method ActualizeRecord

        /// <inheritdoc cref="ISyncConnection.Connect"/>
        public bool Connect()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN: QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            var query = new SyncQuery(this, CommandCode.RegisterClient);
            query.AddAnsi(Username);
            query.AddAnsi(Password);

            var response = ExecuteSync(query);
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

        /// <inheritdoc cref="ISyncIrbisProvider.CreateDatabase"/>
        public bool CreateDatabase
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
            var query = new SyncQuery(this, CommandCode.CreateDatabase);
            query.AddAnsi(database);
            query.AddAnsi(parameters.Database);
            query.Add(parameters.ReaderAccess ? 1 : 0);
            var response = ExecuteSync(query);

            return response?.CheckReturnCode() ?? false;
        } // method CreateDatabase

        /// <inheritdoc cref="ISyncIrbisProvider.CreateDictionary"/>
        public bool CreateDictionary
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
            var query = new SyncQuery(this, CommandCode.CreateDictionary);
            query.AddAnsi(database);
            var response = ExecuteSync(query);

            return response?.CheckReturnCode() ?? false;
        } // method CreateDictionary

        /// <inheritdoc cref="ISyncIrbisProvider.DeleteDatabase"/>
        public bool DeleteDatabase
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
            var query = new SyncQuery(this, CommandCode.DeleteDatabase);
            query.AddAnsi(database);
            var response = ExecuteSync(query);

            return response?.CheckReturnCode() ?? false;
        } // method DeleteDatabase

        /// <inheritdoc cref="ISyncIrbisProvider.Disconnect"/>
        public bool Disconnect()
        {
            if (Connected)
            {
                var query = new SyncQuery(this, CommandCode.UnregisterClient);
                query.AddAnsi(Username);
                try
                {
                    ExecuteSync(query);
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }

                Connected = false;

                Disposing?.Invoke(this, EventArgs.Empty);
            }

            return true;
        } // method Disconnect

        /// <inheritdoc cref="ISyncIrbisProvider.FileExist"/>
        public bool FileExist(FileSpecification specification)
        {
            throw new NotImplementedException();
        } // method FileExist

        /// <inheritdoc cref="ISyncIrbisProvider.FormatRecords"/>
        public bool FormatRecords
            (
                FormatRecordParameters parameters
            )
        {
            throw new NotImplementedException();
        } // method FormatRecords

        /// <inheritdoc cref="ISyncIrbisProvider.FullTextSearch"/>
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

            var query = new SyncQuery(this, CommandCode.NewFulltextSearch);
            searchParameters.Encode(this, query);
            textParameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (!(response?.CheckReturnCode() ?? false))
            {
                return null;
            }

            var result = new FullTextResult();
            result.Decode(response);

            return result;
        } // method FullTextSearch

        /// <inheritdoc cref="IAsyncIrbisProvider.GetDatabaseInfoAsync"/>
        public DatabaseInfo? GetDatabaseInfo
            (
                string? databaseName = default
            )
        {
            throw new NotImplementedException();
        } // method GetDatabaseInfo

        /// <inheritdoc cref="ISyncIrbisProvider.GetMaxMfn"/>
        public int GetMaxMfn
            (
                string? databaseName = default
            )
        {
            var database = databaseName
                           ?? Database
                           ?? throw new IrbisException();
            var response = ExecuteSync(CommandCode.GetMaxMfn, database);

            return response?.CheckReturnCode() ?? false
                ? response.ReturnCode
                : 0;
        } // method GetMaxMfn

        /// <inheritdoc cref="ISyncIrbisProvider.GetServerStat"/>
        public ServerStat? GetServerStat()
        {
            throw new NotImplementedException();
        } // method GetServerStat

        /// <inheritdoc cref="ISyncIrbisProvider.GetServerVersion"/>
        public ServerVersion? GetServerVersion()
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, CommandCode.ServerInfo);
            var response = ExecuteSync(query);
            if (response is null)
            {
                return null;
            }

            response.CheckReturnCode();
            var result = new ServerVersion();
            result.Parse(response);

            return result;
        } // method GetServerVersion

        /// <inheritdoc cref="ISyncIrbisProvider.GlobalCorrection"/>
        public GblResult? GlobalCorrection
            (
                GblSettings settings
            )
        {
            throw new NotImplementedException();
        } // method GlobalCorrection

        /// <inheritdoc cref="ISyncIrbisProvider.ListFiles"/>
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

            var query = new SyncQuery(this, CommandCode.ListFiles);
            foreach (var specification in specifications)
            {
                query.AddAnsi(specification.ToString());
            }

            var response = ExecuteSync(query);
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
        } // method ListFiles

        /// <inheritdoc cref="ISyncIrbisProvider.ListProcesses"/>
        public ProcessInfo[]? ListProcesses()
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, CommandCode.GetProcessList);
            var response = ExecuteSync(query);
            if (response is null)
            {
                return null;
            }

            response.CheckReturnCode();
            var result = ProcessInfo.Parse(response);

            return result;
        } // method ListProcesses

        /// <inheritdoc cref="ISyncIrbisProvider.ListUsers"/>
        public UserInfo[]? ListUsers()
        {
            throw new NotImplementedException();
        } // method ListUsers

        /// <inheritdoc cref="ISyncIrbisProvider.NoOperation"/>
        public bool NoOperation()
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var response = ExecuteSync(CommandCode.Nop);

            return response?.CheckReturnCode() ?? false;
        } // method NoOperation

        /// <inheritdoc cref="ISyncIrbisProvider.PrintTable"/>
        public string? PrintTable
            (
                TableDefinition definition
            )
        {
            throw new NotImplementedException();
        } // method PrintTableAsync

        /// <inheritdoc cref="ISyncIrbisProvider.ReadBinaryFile"/>
        public byte[]? ReadBinaryFile
            (
                FileSpecification specification
            )
        {
            throw new NotImplementedException();
        } // method ReadBinaryFile

        /// <inheritdoc cref="ISyncIrbisProvider.ReadPostings"/>
        public TermPosting[]? ReadPostings
            (
                PostingParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, CommandCode.ReadPostings);
            parameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (response is null
                || !response.CheckReturnCode(ConnectionUtility.GoodCodesForReadTerms))
            {
                return null;
            }

            return TermPosting.Parse(response);
        } // method ReadPosting

        /// <inheritdoc cref="ISyncIrbisProvider.ReadRecord"/>
        public Record? ReadRecord
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
            var query = new SyncQuery(this, CommandCode.ReadRecord);
            query.AddAnsi(database);
            query.Add(parameters.Mfn);
            // TODO: добавить обработку прочих параметров
            var response = ExecuteSync(query);
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
        } // method ReadRecord

        /// <inheritdoc cref="ISyncIrbisProvider.ReadRecordPostings"/>
        public TermPosting[]? ReadRecordPostings
            (
                ReadRecordParameters parameters,
                string prefix
            )
        {
            throw new NotImplementedException();
        } // method ReadRecordPostings

        /// <inheritdoc cref="ISyncIrbisProvider.ReadTerms"/>
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
            var query = new SyncQuery(this, command);
            parameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (response is null
                || !response.CheckReturnCode(ConnectionUtility.GoodCodesForReadTerms))
            {
                return Array.Empty<Term>();
            }

            return Term.Parse(response);
        } // method ReadTerms

        /// <inheritdoc cref="IAsyncIrbisProvider.ReadTextFileAsync"/>
        public string? ReadTextFile
            (
                FileSpecification specification
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, CommandCode.ReadDocument);
            query.AddAnsi(specification.ToString());
            var response = ExecuteSync(query);
            if (response is null)
            {
                return null;
            }

            var result = IrbisText.IrbisToWindows(response.ReadAnsi());

            return result;
        } // method ReadTextFile

        /// <inheritdoc cref="ISyncIrbisProvider.ReloadDictionary"/>
        public bool ReloadDictionary
            (
                string? databaseName = default
            )
        {
            var response = ExecuteSync
                (
                    CommandCode.ReloadDictionary,
                    databaseName ?? Database
                );

            return response?.CheckReturnCode() ?? false;
        } // method ReloadDictionary

        /// <inheritdoc cref="ISyncIrbisProvider.ReloadMasterFile"/>
        public bool ReloadMasterFile
            (
                string? databaseName = default
            )
        {
            var response = ExecuteSync
                (
                    CommandCode.ReloadMasterFile,
                    databaseName ?? Database
                );

            return response?.CheckReturnCode() ?? false;
        } // method ReloadMasterFile

        /// <inheritdoc cref="ISyncIrbisProvider.RestartServer"/>
        public bool RestartServer()
        {
            var response = ExecuteSync(CommandCode.RestartServer);

            return response is not null;
        } // method RestartServer

        /// <inheritdoc cref="ISyncIrbisProvider.Search"/>
        public FoundItem[]? Search
            (
                SearchParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, CommandCode.Search);
            parameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return Array.Empty<FoundItem>();
            }

            return FoundItem.Parse(response);
        } // method Search

        /// <inheritdoc cref="ISyncIrbisProvider.TruncateDatabase"/>
        public bool TruncateDatabase
            (
                string? databaseName = default
            )
        {
            var response = ExecuteSync
                (
                    CommandCode.EmptyDatabase,
                    databaseName ?? Database
                );

            return response is not null && response.CheckReturnCode();
        } // method TruncateDatabase

        /// <inheritdoc cref="ISyncIrbisProvider.UnlockDatabase"/>
        public bool UnlockDatabase
            (
                string? databaseName = default
            )
        {
            var response = ExecuteSync
                (
                    CommandCode.UnlockDatabase,
                    databaseName ?? Database
                );

            return response is not null && response.CheckReturnCode();
        } // method UnlockDatabase

        /// <inheritdoc cref="ISyncIrbisProvider.UnlockRecords"/>
        public bool UnlockRecords
            (
                IEnumerable<int> mfnList,
                string? databaseName = default
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery(this, CommandCode.UnlockRecords);
            query.AddAnsi(databaseName ?? Database);
            foreach (var mfn in mfnList)
            {
                query.Add(mfn);
            }

            var response = ExecuteSync(query);

            return response is not null && response.CheckReturnCode();
        } // method UnlockRecords

        /// <inheritdoc cref="ISyncIrbisProvider.UpdateIniFile"/>
        public bool UpdateIniFile
            (
                IEnumerable<string> lines
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery(this, CommandCode.UpdateIniFile);
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    query.AddAnsi(line);
                }
            }

            var response = ExecuteSync(query);

            return response is not null;
        } // method UpdateIniFile

        /// <inheritdoc cref="ISyncIrbisProvider.UpdateUserList"/>
        public bool UpdateUserList
            (
                IEnumerable<UserInfo> users
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery(this, CommandCode.SetUserList);
            foreach (var user in users)
            {
                query.AddAnsi(user.Encode());
            }

            var response = ExecuteSync(query);

            return response is not null;
        } // method UpdateUserList

        /// <inheritdoc cref="ISyncIrbisProvider.WriteRecord"/>
        public bool WriteRecord
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
            var query = new SyncQuery(this, CommandCode.UpdateRecord);
            query.AddAnsi(database);
            query.Add(parameters.Lock ? 1 : 0);
            query.Add(parameters.Actualize ? 1 : 0);
            query.AddUtf(record.Encode());

            var response = ExecuteSync(query);
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
        } // method WriteRecord

        /// <inheritdoc cref="ISyncIrbisProvider.WriteTextFile"/>
        public bool WriteTextFile
            (
                FileSpecification specification
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery(this, CommandCode.ReadDocument);
            query.AddAnsi(specification.ToString());

            var response = ExecuteSync(query);

            return response is not null;
        } // method WriteTextFile

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Disconnect();
        }

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService(Type serviceType) =>
            _provider.GetService(serviceType);

        #endregion

    } // class SyncConnection

} // namespace ManagedIrbis
