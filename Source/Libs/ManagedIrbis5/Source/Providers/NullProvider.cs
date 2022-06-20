// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
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

using AM;
using AM.Logging;
using AM.PlatformAbstraction;
using AM.Threading;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Records;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Providers;

/// <summary>
/// Пустой клиент для нужд тестирования.
/// Не выполняет никаких осмысленных действий.
/// </summary>
public sealed class NullProvider
    : ISyncProvider,
    IAsyncProvider
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public NullProvider()
    {
        Busy = new BusyState();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Вручную задаем состояние занятости.
    /// </summary>
    public void SetBusy (bool busy) => Busy.SetState(busy);

    /// <summary>
    /// Вручную задаем состояние подключения.
    /// </summary>
    public void SetConnected (bool connected) => Connected = connected;

    /// <summary>
    /// Вручную устанавливаем код ошибки.
    /// </summary>
    public void SetLastError (int code) => LastError = code;

    #endregion

    #region ICancellable members

    /// <inheritdoc cref="ICancellable.Busy"/>
    public BusyState Busy { get; }

    /// <inheritdoc cref="ICancellable.CancelOperation"/>
    public void CancelOperation() {}

    /// <inheritdoc cref="ICancellable.ThrowIfCancelled"/>
    public void ThrowIfCancelled() {}

    #endregion

    #region IIrbisProvider members

    /// <inheritdoc cref="IIrbisProvider.Disposing"/>
    public event EventHandler? Disposing;

    /// <inheritdoc cref="IIrbisProvider.PlatformAbstraction"/>
    public PlatformAbstractionLayer PlatformAbstraction { get; set; } =
        PlatformAbstractionLayer.Current;

    /// <inheritdoc cref="IIrbisProvider.Database"/>
    public string? Database { get; set; } = "IBIS";

    /// <inheritdoc cref="IIrbisProvider.Connected"/>
    public bool Connected { get; private set; }

    /// <inheritdoc cref="IGetLastError.LastError"/>
    public int LastError { get; private set; }

    /// <inheritdoc cref="IIrbisProvider.CheckProviderState"/>
    public bool CheckProviderState() => true;

    /// <inheritdoc cref="IIrbisProvider.Configure"/>
    public void Configure (string configurationString) {}

    /// <inheritdoc cref="IIrbisProvider.GetGeneration"/>
    public string GetGeneration() => "64";

    /// <inheritdoc cref="IIrbisProvider.GetWaitHandle"/>
    public WaitHandle GetWaitHandle() => Busy.WaitHandle;

    #endregion

    #region ISupportLogging members

    /// <inheritdoc cref="ISupportLogging.Logger"/>
    // TODO implement
    public ILogger? Logger => null;

    /// <inheritdoc cref="ISupportLogging.SetLogger"/>
    public void SetLogger(ILogger? logger)
        => throw new NotImplementedException();

    #endregion

    #region ISyncProvider members

    /// <inheritdoc cref="ISyncProvider.ActualizeRecord"/>
    public bool ActualizeRecord(ActualizeRecordParameters parameters) => default;

    /// <inheritdoc cref="ISyncProvider.Connect"/>
    public bool Connect() => default;

    /// <inheritdoc cref="ISyncProvider.CreateDatabase"/>
    public bool CreateDatabase(CreateDatabaseParameters parameters) => default;

    /// <inheritdoc cref="ISyncProvider.CreateDictionary"/>
    public bool CreateDictionary(string? databaseName = default) => default;

    /// <inheritdoc cref="ISyncProvider.DeleteDatabase"/>
    public bool DeleteDatabase(string? databaseName = default) => default;

    /// <inheritdoc cref="ISyncProvider.Disconnect"/>
    public bool Disconnect() => default;

    /// <inheritdoc cref="ISyncProvider.FileExist"/>
    public bool FileExist(FileSpecification specification) => default;

    /// <inheritdoc cref="ISyncProvider.FormatRecords"/>
    public bool FormatRecords(FormatRecordParameters parameters) => default;

    /// <inheritdoc cref="ISyncProvider.FullTextSearch"/>
    public FullTextResult? FullTextSearch(SearchParameters searchParameters, TextParameters textParameters) => default;

    /// <inheritdoc cref="ISyncProvider.GetDatabaseInfo"/>
    public DatabaseInfo? GetDatabaseInfo(string? databaseName = default) => default;

    /// <inheritdoc cref="ISyncProvider.GetMaxMfn"/>
    public int GetMaxMfn(string? databaseName = default) => default;

    /// <inheritdoc cref="ISyncProvider.GetServerStat"/>
    public ServerStat? GetServerStat() => default;

    /// <inheritdoc cref="ISyncProvider.GetServerVersion"/>
    public ServerVersion? GetServerVersion() => default;

    /// <inheritdoc cref="ISyncProvider.GlobalCorrection"/>
    public GblResult? GlobalCorrection(GblSettings settings) => default;

    /// <inheritdoc cref="ISyncProvider.ListFiles"/>
    public string[]? ListFiles(params FileSpecification[] specifications) => default;

    /// <inheritdoc cref="ISyncProvider.ListProcesses"/>
    public ProcessInfo[]? ListProcesses() => default;

    /// <inheritdoc cref="ISyncProvider.ListUsers"/>
    public UserInfo[]? ListUsers() => default;

    /// <inheritdoc cref="ISyncProvider.NoOperation"/>
    public bool NoOperation() => default;

    /// <inheritdoc cref="ISyncProvider.PrintTable"/>
    public string? PrintTable(TableDefinition definition) => default;

    /// <inheritdoc cref="ISyncProvider.ReadBinaryFile"/>
    public byte[]? ReadBinaryFile(FileSpecification specification) => default;

    /// <inheritdoc cref="ISyncProvider.ReadPostings"/>
    public TermPosting[]? ReadPostings(PostingParameters parameters) => default;

    /// <inheritdoc cref="ISyncProvider.ReadRecord{T}"/>
    public T? ReadRecord<T> (ReadRecordParameters parameters)
        where T: class, IRecord, new()
        => default;

    /// <inheritdoc cref="ISyncProvider.ReadRecordPostings"/>
    public TermPosting[]? ReadRecordPostings(ReadRecordParameters parameters, string prefix) => default;

    /// <inheritdoc cref="ISyncProvider.ReadTerms"/>
    public Term[]? ReadTerms(TermParameters parameters) => default;

    /// <inheritdoc cref="ISyncProvider.ReadTextFile"/>
    public string? ReadTextFile(FileSpecification specification) => default;

    /// <inheritdoc cref="ISyncProvider.ReloadDictionary"/>
    public bool ReloadDictionary(string? databaseName = default) => default;

    /// <inheritdoc cref="ISyncProvider.ReloadMasterFile"/>
    public bool ReloadMasterFile(string? databaseName = default) => default;

    /// <inheritdoc cref="ISyncProvider.RestartServer"/>
    public bool RestartServer() => default;

    /// <inheritdoc cref="ISyncProvider.Search"/>
    public FoundItem[]? Search(SearchParameters parameters) => default;

    /// <inheritdoc cref="ISyncProvider.TruncateDatabase"/>
    public bool TruncateDatabase(string? databaseName = default) => default;

    /// <inheritdoc cref="ISyncProvider.UnlockDatabase"/>
    public bool UnlockDatabase(string? databaseName = default) => default;

    /// <inheritdoc cref="ISyncProvider.UnlockRecords"/>
    public bool UnlockRecords(IEnumerable<int> mfnList, string? databaseName = default) => default;

    /// <inheritdoc cref="ISyncProvider.UpdateIniFile"/>
    public bool UpdateIniFile(IEnumerable<string> lines) => default;

    /// <inheritdoc cref="ISyncProvider.UpdateUserList"/>
    public bool UpdateUserList(IEnumerable<UserInfo> users) => default;

    /// <inheritdoc cref="ISyncProvider.WriteTextFile"/>
    public bool WriteTextFile(FileSpecification specification) => default;

    /// <inheritdoc cref="ISyncProvider.WriteRecord"/>
    public bool WriteRecord(WriteRecordParameters parameters) => default;

    #endregion

    #region IAsyncProvider members

    /// <inheritdoc cref="IAsyncProvider.ActualizeRecordAsync"/>
    public Task<bool> ActualizeRecordAsync(ActualizeRecordParameters parameters) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.ConnectAsync"/>
    public Task<bool> ConnectAsync() => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.CreateDatabaseAsync"/>
    public Task<bool> CreateDatabaseAsync(CreateDatabaseParameters parameters) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.CreateDictionaryAsync"/>
    public Task<bool> CreateDictionaryAsync(string? databaseName = default) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.DeleteDatabaseAsync"/>
    public Task<bool> DeleteDatabaseAsync(string? databaseName = default) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.DisconnectAsync"/>
    public Task<bool> DisconnectAsync() => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.FileExistAsync"/>
    public Task<bool> FileExistAsync(FileSpecification specification) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.FormatRecordsAsync"/>
    public Task<bool> FormatRecordsAsync(FormatRecordParameters parameters) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.FullTextSearchAsync"/>
    public Task<FullTextResult?> FullTextSearchAsync(SearchParameters searchParameters, TextParameters textParameters) => Task.FromResult<FullTextResult?>(default);

    /// <inheritdoc cref="IAsyncProvider.GetDatabaseInfoAsync"/>
    public Task<DatabaseInfo?> GetDatabaseInfoAsync(string? databaseName = default) => Task.FromResult<DatabaseInfo?>(default);

    /// <inheritdoc cref="IAsyncProvider.GetMaxMfnAsync"/>
    public Task<int> GetMaxMfnAsync(string? databaseName = default) => Task.FromResult<int>(default);

    /// <inheritdoc cref="IAsyncProvider.GetServerStatAsync"/>
    public Task<ServerStat?> GetServerStatAsync() => Task.FromResult<ServerStat?>(default);

    /// <inheritdoc cref="IAsyncProvider.GetServerVersionAsync"/>
    public Task<ServerVersion?> GetServerVersionAsync() => Task.FromResult<ServerVersion?>(default);

    /// <inheritdoc cref="IAsyncProvider.GlobalCorrectionAsync"/>
    public Task<GblResult?> GlobalCorrectionAsync(GblSettings settings) => Task.FromResult<GblResult?>(default);

    /// <inheritdoc cref="IAsyncProvider.ListFilesAsync"/>
    public Task<string[]?> ListFilesAsync(params FileSpecification[] specifications) => Task.FromResult<string[]?>(default);

    /// <inheritdoc cref="IAsyncProvider.ListProcessesAsync"/>
    public Task<ProcessInfo[]?> ListProcessesAsync() => Task.FromResult<ProcessInfo[]?>(default);

    /// <inheritdoc cref="IAsyncProvider.ListUsersAsync"/>
    public Task<UserInfo[]?> ListUsersAsync() => Task.FromResult<UserInfo[]?>(default);

    /// <inheritdoc cref="IAsyncProvider.NoOperationAsync"/>
    public Task<bool> NoOperationAsync() => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.PrintTableAsync"/>
    public Task<string?> PrintTableAsync(TableDefinition definition) => Task.FromResult<string?>(default);

    /// <inheritdoc cref="IAsyncProvider.ReadBinaryFileAsync"/>
    public Task<byte[]?> ReadBinaryFileAsync(FileSpecification specification) => Task.FromResult<byte[]?>(default);

    /// <inheritdoc cref="IAsyncProvider.ReadPostingsAsync"/>
    public Task<TermPosting[]?> ReadPostingsAsync(PostingParameters parameters) => Task.FromResult<TermPosting[]?>(default);

    /// <inheritdoc cref="IAsyncProvider.ReadRecordAsync{T}"/>
    public Task<T?> ReadRecordAsync<T>(ReadRecordParameters parameters)
        where T: class, IRecord, new()
        => Task.FromResult<T?>(default);

    /// <inheritdoc cref="IAsyncProvider.ReadRecordPostingsAsync"/>
    public Task<TermPosting[]?> ReadRecordPostingsAsync(ReadRecordParameters parameters, string prefix) => Task.FromResult<TermPosting[]?>(default);

    /// <inheritdoc cref="IAsyncProvider.ReadTermsAsync"/>
    public Task<Term[]?> ReadTermsAsync(TermParameters parameters) => Task.FromResult<Term[]?>(default);

    /// <inheritdoc cref="IAsyncProvider.ReadTextFileAsync"/>
    public Task<string?> ReadTextFileAsync(FileSpecification specification) => Task.FromResult<string?>(default);

    /// <inheritdoc cref="IAsyncProvider.ReloadDictionaryAsync"/>
    public Task<bool> ReloadDictionaryAsync(string? databaseName = default) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.ReloadMasterFileAsync"/>
    public Task<bool> ReloadMasterFileAsync(string? databaseName = default) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.RestartServerAsync"/>
    public Task<bool> RestartServerAsync() => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.SearchAsync"/>
    public Task<FoundItem[]?> SearchAsync(SearchParameters parameters) => Task.FromResult<FoundItem[]?>(default);

    /// <inheritdoc cref="IAsyncProvider.TruncateDatabaseAsync"/>
    public Task<bool> TruncateDatabaseAsync(string? databaseName = default) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.UnlockDatabaseAsync"/>
    public Task<bool> UnlockDatabaseAsync(string? databaseName = default) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.UnlockRecordsAsync"/>
    public Task<bool> UnlockRecordsAsync(IEnumerable<int> mfnList, string? databaseName = default) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.UpdateIniFileAsync"/>
    public Task<bool> UpdateIniFileAsync(IEnumerable<string> lines) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.UpdateUserListAsync"/>
    public Task<bool> UpdateUserListAsync(IEnumerable<UserInfo> users) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.WriteTextFileAsync"/>
    public Task<bool> WriteTextFileAsync(FileSpecification specification) => Task.FromResult<bool>(default);

    /// <inheritdoc cref="IAsyncProvider.WriteRecordAsync"/>
    public Task<bool> WriteRecordAsync(WriteRecordParameters parameters) => Task.FromResult<bool>(default);

    #endregion

    #region IServiceProvider members

    /// <inheritdoc cref="IServiceProvider.GetService"/>
    public object? GetService (Type serviceType) => null;

    #endregion

    #region IAsyncDisposable members

    /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
    public async ValueTask DisposeAsync()
    {
        Busy.Dispose();
        await Disposing.RaiseAsync(this);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    void IDisposable.Dispose()
    {
        Busy.Dispose();
        Disposing.Raise(this);
    }

    #endregion
}
