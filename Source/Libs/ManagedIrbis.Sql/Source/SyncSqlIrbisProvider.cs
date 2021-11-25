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
// ReSharper disable UnusedParameter.Local

/* SyncSqlIrbisProvider.cs -- синхронный SQL-провайдер для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Logging;
using AM.PlatformAbstraction;
using AM.Threading;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Records;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

// event not invoked
#pragma warning disable CS0067

namespace ManagedIrbis.Sql
{
    /// <summary>
    /// Синхронный SQL-провайдер для ИРБИС.
    /// </summary>
    public class SyncSqlIrbisProvider
        : ISyncProvider
    {
        #region Properties

        /// <summary>
        /// Используемая конфигурация.
        /// </summary>
        public IConfiguration Configuration => _configuration;

        /// <summary>
        /// Используемый логгер.
        /// </summary>
        public ILogger Logger => _logger;

        /// <summary>
        /// Токен для отмены длительных операций.
        /// </summary>
        public CancellationToken Cancellation { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SyncSqlIrbisProvider
            (
                IServiceProvider serviceProvider,
                IConfiguration configuration,
                string? connectionString = null
            )
        {
            Busy = new BusyState();
            _cancellation = new CancellationTokenSource();
            Cancellation = _cancellation.Token;
            PlatformAbstraction = PlatformAbstractionLayer.Current;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = serviceProvider.GetRequiredService<ILogger<SyncSqlIrbisProvider>>();

            _logger.LogTrace (nameof (SyncSqlIrbisProvider) + "::Constructor");

            _connectionString = connectionString ?? _configuration["sqlirbis"];
        }

        #endregion

        #region Private members

        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cancellation;

        #endregion

        #region ISyncProvider members

        /// <inheritdoc />
        public bool ActualizeRecord (ActualizeRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Connect()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool CreateDatabase (CreateDatabaseParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool CreateDictionary (string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool DeleteDatabase (string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool Disconnect()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool FileExist (FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool FormatRecords (FormatRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public FullTextResult? FullTextSearch (SearchParameters searchParameters, TextParameters textParameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public DatabaseInfo? GetDatabaseInfo (string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int GetMaxMfn (string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ServerStat? GetServerStat()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ServerVersion? GetServerVersion()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public GblResult? GlobalCorrection (GblSettings settings)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string[]? ListFiles (params FileSpecification[] specifications)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ProcessInfo[]? ListProcesses()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public UserInfo[]? ListUsers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool NoOperation()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? PrintTable (TableDefinition definition)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public byte[]? ReadBinaryFile (FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TermPosting[]? ReadPostings (PostingParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T? ReadRecord<T> (ReadRecordParameters parameters) where T : class, IRecord, new()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public TermPosting[]? ReadRecordPostings (ReadRecordParameters parameters, string prefix)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Term[]? ReadTerms (TermParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string? ReadTextFile (FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool ReloadDictionary (string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool ReloadMasterFile (string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool RestartServer()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public FoundItem[]? Search (SearchParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool TruncateDatabase (string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool UnlockDatabase (string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool UnlockRecords (IEnumerable<int> mfnList, string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool UpdateIniFile (IEnumerable<string> lines)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool UpdateUserList (IEnumerable<UserInfo> users)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool WriteTextFile (FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public bool WriteRecord (WriteRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IIrbisProvider members

        /// <inheritdoc />
        public event EventHandler? Disposing;

        /// <inheritdoc />
        public string? Database { get; set; }

        /// <inheritdoc />
        public bool Connected { get; }

        /// <inheritdoc />
        public PlatformAbstractionLayer PlatformAbstraction { get; set; }

        /// <inheritdoc />
        public bool CheckProviderState()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Configure (string configurationString)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string GetGeneration()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public WaitHandle GetWaitHandle()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICancellable members

        /// <inheritdoc />
        public BusyState Busy { get; }

        /// <inheritdoc />
        public void CancelOperation()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void ThrowIfCancelled()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISupportLogging members

        /// <inheritdoc cref="ISupportLogging.SetLogger"/>
        public void SetLogger (ILogger? logger)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGetLastError members

        /// <inheritdoc cref="IGetLastError.LastError"/>
        public int LastError { get; }

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService (Type serviceType)
        {
            return _serviceProvider.GetService (serviceType);
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
