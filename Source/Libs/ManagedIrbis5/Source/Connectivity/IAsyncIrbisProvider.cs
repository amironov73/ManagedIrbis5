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

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Интерфейс асинхронного ИРБИС-провайдера.
    /// </summary>
    public interface IAsyncIrbisProvider
        : IBasicIrbisProvider
    {
        /// <summary>
        /// Актуализация записи.
        /// </summary>
        Task<bool> ActualizeRecordAsync(ActualizeRecordParameters parameters);

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        Task<bool> ConnectAsync();

        /// <summary>
        /// Создание базы данных на сервере.
        /// </summary>
        Task<bool> CreateDatabaseAsync(CreateDatabaseParameters parameters);

        /// <summary>
        /// Создание поискового словаря в указанной базе данных.
        /// </summary>
        Task<bool> CreateDictionaryAsync(string? databaseName);

        /// <summary>
        /// Удаление указанной базы данных на сервере.
        /// </summary>
        /// <param name="databaseName">Имя удалаемой базы данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        Task<bool> DeleteDatabaseAsync(string? databaseName);

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        Task<bool> DisconnectAsync();

        /// <summary>
        /// Форматирование указанной записи.
        /// </summary>
        Task<string?> FormatRecordAsync(string format, int mfn);

        /// <summary>
        /// Форматирование указанной записи.
        /// </summary>
        Task<string?> FormatRecordAsync(string format, Record record);

        /// <summary>
        /// Полнотекстовый поиск ИРБИС64+.
        /// </summary>
        Task<FullTextResult?> FullTextSearchAsync(SearchParameters searchParameters,
            TextParameters textParameters);

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// По умолчанию используется текущая база данных.
        /// </summary>
        Task<int> GetMaxMfnAsync(string? databaseName = default);

        /// <summary>
        /// Получение версии ИРБИС-сервера.
        /// </summary>
        Task<ServerVersion?> GetServerVersionAsync();

        Task<GblResult?> GlobalCorrection(GblSettings settings);

        /// <summary>
        /// Получение списка файлов на сервере,
        /// удовлетворяющих указанной спецификации.
        /// </summary>
        Task<string[]?> ListFilesAsync(FileSpecification specification);

        /// <summary>
        /// Получение списка файлов на сервере,
        /// удовлетворяющих указанным спецификациям.
        /// </summary>
        Task<string[]?> ListFilesAsync(params FileSpecification[] specifications);

        /// <summary>
        /// Получение списка процессов, работающих в данный момент
        /// на ИРБИС-сервере.
        /// </summary>
        /// <returns></returns>
        Task<ProcessInfo[]?> ListProcessesAsync();

        /// <summary>
        /// Получение списка пользователей, имеющих доступ к
        /// ИРБИС-серверу. Эти пользователи не обязательно должны
        /// быть залогинены в данный момент.
        /// </summary>
        Task<UserInfo[]?> ListUsersAsync();

        /// <summary>
        /// Пустая операция, необходимая для поддержания связи
        /// с ИРБИС-сервером.
        /// </summary>
        Task<bool> NoOperationAsync();

        Task<TermPosting[]?> ReadPostingsAsync(PostingParameters parameters);

        Task<Record?> ReadRecordAsync(ReadRecordParameters parameters);

        Task<Term[]?> ReadTermsAsync(TermParameters parameters);

        Task<string?> ReadTextFileAsync(FileSpecification specification);

        Task<bool> ReloadDictionaryAsync(string? databaseName);

        Task<bool> ReloadMasterFileAsync(string? databaseName);

        Task<bool> RestartServerAsync();

        Task<FoundItem[]?> SearchAsync(SearchParameters parameters);

        Task<bool> TruncateDatabaseAsync(string? databaseName);

        Task<bool> UnlockDatabaseAsync(string? databaseName);

        Task<bool> UpdateIniFileAsync(IEnumerable<string> lines);

        Task<bool> UpdateUserListAsync(IEnumerable<UserInfo> users);

        Task<bool> UnlockRecordsAsync(IEnumerable<int> mfnList, string? databaseName);

        Task<bool> WriteFileAsync(FileSpecification specification);

        Task<bool> WriteRecordAsync(WriteRecordParameters parameters);

    } // interface IAsyncIrbisProvider

} // namespace ManagedIrbis
