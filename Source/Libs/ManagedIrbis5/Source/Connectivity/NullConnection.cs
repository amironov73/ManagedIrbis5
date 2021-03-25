// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* NullConnection.cs -- пустой клиент для нужд тестирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Пустой клиент для нужд тестирования.
    /// Не выполняет никаких осмысленных действий.
    /// </summary>
    public sealed class NullConnection
        : IIrbisConnection
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public string Host { get; set; }
        public ushort Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string Workstation { get; set; }
        public int ClientId { get; }
        public int QueryId { get; }
        public bool Connected { get; }
        public bool Busy { get; }
        public int LastError { get; }
        public bool CheckConnection()
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public Response? ExecuteSync(ref ValueQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ActualizeRecordAsync(ActualizeRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        Task<bool> IAsyncConnection.ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Response?> ExecuteAsync(Query query)
        {
            throw new NotImplementedException();
        }

        Task<bool> IAsyncIrbisProvider.ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateDatabaseAsync(CreateDatabaseParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateDictionaryAsync(string? databaseName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteDatabaseAsync(string? databaseName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DisconnectAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string?> FormatRecordAsync(string format, int mfn)
        {
            throw new NotImplementedException();
        }

        public Task<string?> FormatRecordAsync(string format, Record record)
        {
            throw new NotImplementedException();
        }

        public Task<FullTextResult?> FullTextSearchAsync(SearchParameters searchParameters, TextParameters textParameters)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMaxMfnAsync(string? databaseName)
        {
            throw new NotImplementedException();
        }

        public Task<ServerVersion?> GetServerVersionAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string[]?> ListFilesAsync(FileSpecification specification)
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

        public Task<TermPosting[]?> ReadPostingsAsync(PostingParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<Record?> ReadRecordAsync(ReadRecordParameters parameters)
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

        public Task<bool> ReloadDictionaryAsync(string? databaseName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReloadMasterFileAsync(string? databaseName)
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

        public Task<bool> TruncateDatabaseAsync(string? databaseName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnlockDatabaseAsync(string? databaseName)
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

        public Task<bool> UnlockRecordsAsync(IEnumerable<int> mfnList, string? databaseName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteFileAsync(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> WriteRecordAsync(WriteRecordParameters parameters)
        {
            throw new NotImplementedException();
        }
    } // class NullConnection

} // namespace ManagedIrbis
