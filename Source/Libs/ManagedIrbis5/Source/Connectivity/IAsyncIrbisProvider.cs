// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* IAsyncIrbisProvider.cs -- интерфейс асинхронного ИРБИС-провайдера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Интерфейс асинхронного ИРБИС-провайдера.
    /// </summary>
    public interface IAsyncIrbisProvider
        : IBasicConnection
    {

        Task<bool> ActualizeRecordAsync();

        Task<bool> ConnectAsync();

        Task<bool> CreateDatabaseAsync(CreateDatabaseParameters parameters);

        Task<bool> CreateDictionaryAsync(string? databaseName);

        Task<bool> DisconnectAsync();

        Task<bool> FormatRecordAsync();

        Task<int> GetMaxMfnAsync();

        Task<bool> ListFilesAsync(FileSpecification specification);

        Task<bool> ListProcessesAsync();

        Task<bool> ListUsersAsync();

        Task<bool> NoOperationAsync();

        Task<bool> ReadFileAsync(FileSpecification specification);

        Task<bool> ReadPostingsAsync(PostingParameters parameters);

        Task<bool> ReadRecordAsync(ReadRecordParameters parameters);

        Task<bool> ReadTermsAsync(TermParameters parameters);

        Task<bool> ReloadDictionaryAsync();

        Task<bool> ReloadMasterFileAsync();

        Task<bool> RestartServerAsync();

        Task<bool> SearchAsync(SearchParameters parameters);

        Task<bool> TruncateDatabaseAsync(string? databaseName);

        Task<bool> UpdateIniFileAsync(IList<string> lines);

        Task<bool> UpdateUserListAsync(UserInfo[] users);

        Task<bool> UnlockDatabaseAsync(string? databaseName);

        Task<bool> WriteFileAsync(FileSpecification specification);

        Task<bool> WriteRecordAsync(WriteRecordParameters parameters);

    } // interface IAsyncIrbisProvider

} // namespace ManagedIrbis
