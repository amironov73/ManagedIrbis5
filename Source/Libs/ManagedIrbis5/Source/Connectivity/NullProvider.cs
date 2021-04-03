// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* NullProvider.cs -- пустой клиент для нужд тестирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AM.IO;
using AM.PlatformAbstraction;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Пустой клиент для нужд тестирования.
    /// Не выполняет никаких осмысленных действий.
    /// </summary>
    public sealed class NullProvider
        : ISyncIrbisProvider,
        IAsyncIrbisProvider
    {
        #region ISyncIrbisProvider members

        public PlatformAbstractionLayer PlatformAbstraction =>
            throw new NotImplementedException();

        public bool FileExist(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FileExistAsync(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public string? GetGeneration()
        {
            throw new NotImplementedException();
        }

        public void Configure(string configurationString)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public event EventHandler? BusyChanged;
        public string Database { get; set; }
        public bool Connected { get; }
        public bool Busy { get; }
        public int LastError { get; }
        public void CancelOperation()
        {
            throw new NotImplementedException();
        }

        public bool CheckProviderState()
        {
            throw new NotImplementedException();
        }

        public WaitHandle GetWaitHandle()
        {
            throw new NotImplementedException();
        }

        public bool ActualizeRecord(ActualizeRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public bool CreateDatabase(CreateDatabaseParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool CreateDictionary(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDatabase(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool FormatRecords(FormatRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public FullTextResult? FullTextSearch(SearchParameters searchParameters, TextParameters textParameters)
        {
            throw new NotImplementedException();
        }

        public DatabaseInfo? GetDatabaseInfo(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public int GetMaxMfn(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public ServerStat? GetServerStat()
        {
            throw new NotImplementedException();
        }

        public ServerVersion? GetServerVersion()
        {
            throw new NotImplementedException();
        }

        public GblResult? GlobalCorrection(GblSettings settings)
        {
            throw new NotImplementedException();
        }

        public string[]? ListFiles(params FileSpecification[] specifications)
        {
            throw new NotImplementedException();
        }

        public ProcessInfo[]? ListProcesses()
        {
            throw new NotImplementedException();
        }

        public UserInfo[]? ListUsers()
        {
            throw new NotImplementedException();
        }

        public bool NoOperation()
        {
            throw new NotImplementedException();
        }

        public string? PrintTable(TableDefinition definition)
        {
            throw new NotImplementedException();
        }

        public byte[]? ReadBinaryFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public TermPosting[]? ReadPostings(PostingParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Record? ReadRecord(ReadRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public TermPosting[]? ReadRecordPostings(ReadRecordParameters parameters, string prefix)
        {
            throw new NotImplementedException();
        }

        public Term[]? ReadTerms(TermParameters parameters)
        {
            throw new NotImplementedException();
        }

        public string? ReadTextFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public bool ReloadDictionary(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool ReloadMasterFile(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool RestartServer()
        {
            throw new NotImplementedException();
        }

        public FoundItem[]? Search(SearchParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool TruncateDatabase(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool UnlockDatabase(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool UnlockRecords(IEnumerable<int> mfnList, string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool UpdateIniFile(IEnumerable<string> lines)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserList(IEnumerable<UserInfo> users)
        {
            throw new NotImplementedException();
        }

        public bool WriteTextFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public bool WriteRecord(WriteRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IAsyncIrbisProvider members

        public Task<bool> ActualizeRecordAsync(ActualizeRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateDatabaseAsync(CreateDatabaseParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateDictionaryAsync(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDatabaseAsync(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> FormatRecordsAsync(FormatRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<FullTextResult?> FullTextSearchAsync(SearchParameters searchParameters, TextParameters textParameters)
        {
            throw new NotImplementedException();
        }

        public Task<DatabaseInfo?> GetDatabaseInfoAsync(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMaxMfnAsync(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public Task<ServerStat?> GetServerStatAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ServerVersion?> GetServerVersionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GblResult?> GlobalCorrectionAsync(GblSettings settings)
        {
            throw new NotImplementedException();
        }

        public Task<string[]?> ListFilesAsync(params FileSpecification[] specifications)
        {
            throw new NotImplementedException();
        }

        public Task<ProcessInfo[]?> ListProcessesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo[]?> ListUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> NoOperationAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string?> PrintTableAsync(TableDefinition definition)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]?> ReadBinaryFileAsync(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public Task<TermPosting[]?> ReadPostingsAsync(PostingParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<Record?> ReadRecordAsync(ReadRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<TermPosting[]?> ReadRecordPostingsAsync(ReadRecordParameters parameters, string prefix)
        {
            throw new NotImplementedException();
        }

        public Task<Term[]?> ReadTermsAsync(TermParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<string?> ReadTextFileAsync(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReloadDictionaryAsync(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReloadMasterFileAsync(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RestartServerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<FoundItem[]?> SearchAsync(SearchParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TruncateDatabaseAsync(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnlockDatabaseAsync(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnlockRecordsAsync(IEnumerable<int> mfnList, string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateIniFileAsync(IEnumerable<string> lines)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserListAsync(IEnumerable<UserInfo> users)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteTextFileAsync(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteRecordAsync(WriteRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        #endregion

    } // class NullProvider

} // namespace ManagedIrbis
