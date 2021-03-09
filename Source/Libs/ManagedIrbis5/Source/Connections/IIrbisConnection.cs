// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IIrbisConnection.cs -- наиболее общий интерфейс подключения для мока
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Наиболее общий интерфейс подключения к серверу ИРБИС64
    /// для мока.
    /// </summary>
    public interface IIrbisConnection
        : IDisposable
    {
        /*

        /// <summary>
        /// Fired when <see cref="Busy"/> changed.
        /// </summary>
        event EventHandler? BusyChanged;

        */

        /// <summary>
        ///
        /// </summary>
        string Host { get; set; }

        /// <summary>
        ///
        /// </summary>
        ushort Port { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Username { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Password { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Database { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Workstation { get; set; }

        /// <summary>
        ///
        /// </summary>
        int ClientId { get; }

        /// <summary>
        ///
        /// </summary>
        int QueryId { get; }

        /*

        /// <summary>
        ///
        /// </summary>
        string? ServerVersion { get; }

        /// <summary>
        ///
        /// </summary>
        int Interval { get; }

        */

        /// <summary>
        ///
        /// </summary>
        bool Connected { get; }

        /*

        /// <summary>
        /// Socket.
        /// </summary>
        ClientSocket Socket { get; }

        */

        /// <summary>
        /// Busy?
        /// </summary>
        bool Busy { get; }

        /*

        /// <summary>
        ///
        /// </summary>
        CancellationToken Cancellation { get; }

        */

        /// <summary>
        /// Last error code.
        /// </summary>
        int LastError { get; }

        bool CheckConnection();

        /*

        /// <summary>
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        Task<bool> ActualizeDatabaseAsync
            (
                string? database = default
            );

        /// <summary>
        /// Актуализация записи с указанным MFN.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN, подлежащий актуализации.</param>
        /// <returns>Признак успешности операции.</returns>
        Task<bool> ActualizeRecordAsync
            (
                string? database,
                int mfn
            );

        /// <summary>
        /// Cancel the current operation.
        /// </summary>
        void CancelOperation();

        /// <summary>
        /// Подключение к серверу ИРБИС64.
        /// </summary>
        Task<bool> ConnectAsync();

        /// <summary>
        /// Создание базы данных.
        /// </summary>
        /// <param name="database">Имя создаваемой базы.</param>
        /// <param name="description">Описание в свободной форме.</param>
        /// <param name="readerAccess">Читатель будет иметь доступ?</param>
        /// <returns>Признак успешности операции.</returns>
        Task<bool> CreateDatabaseAsync
            (
                string database,
                string description,
                bool readerAccess = true
            );

        /// <summary>
        /// Создание словаря в указанной базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        Task<bool> CreateDictionaryAsync
            (
                string? database = default
            );

        /// <summary>
        /// Удаление указанной базы данных.
        /// </summary>
        /// <param name="database">Имя удаляемой базы данных.</param>
        /// <returns>Признак успешности операции.</returns>
        Task<bool> DeleteDatabaseAsync
            (
                string? database = default
            );

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        /// <returns>Признак успешности завершения операции.</returns>
        Task<bool> DisconnectAsync();

        */


        /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        Task<Response?> ExecuteAsync
            (
                Query query
            );

        /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        Response? ExecuteSync
            (
                ref ValueQuery query
            );

        /*

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="args">Опциональные параметры команды
        /// (в кодировке ANSI).</param>
        /// <returns>Ответ сервера.</returns>
        Task<Response?> ExecuteAsync
            (
                string command,
                params object[] args
            );

        /// <summary>
        /// Форматирование записи с указанием её MFN.
        /// </summary>
        /// <param name="format">Спецификация формата.</param>
        /// <param name="mfn">MFN записи.</param>
        /// <returns>Результат расформатирования.</returns>
        Task<string?> FormatRecordAsync
            (
                string format,
                int mfn
            );

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// </summary>
        /// <param name="database">Опциональное имя базы данных
        /// (<c>null</c> означает текущую базу данных).</param>
        /// <returns>Макисмальный MFN.</returns>
        Task<int> GetMaxMfnAsync
            (
                string? database = default
            );

        /// <summary>
        ///
        /// </summary>
        Task<ServerVersion?> GetServerVersionAsync();

        /// <summary>
        /// Получение списка файлов на сервере.
        /// </summary>
        Task<string[]> ListFilesAsync
        (
            params string[] specifications
        );

        /// <summary>
        /// Получение списка процессов на сервере.
        /// </summary>
        Task<ProcessInfo[]> ListProcessesAsync();

        /// <summary>
        /// Пустая операция.
        /// </summary>
        /// <returns>Признак успешного завершения операции.</returns>
        Task<bool> NopAsync();

        /// <summary>
        /// Разбор строки подключения.
        /// </summary>
        void ParseConnectionString
            (
                string? connectionString
            );

        Task<Term[]> ReadAllTermsAsync
            (
                string prefix
            );

        /// <summary>
        /// Чтение библиографической записи с сервера.
        /// </summary>
        Task<Record?> ReadRecordAsync
            (
                int mfn
            );

        /// <summary>
        /// Чтение постингов указанного термина.
        /// </summary>
        /// <param name="term">Термин.</param>
        /// <param name="numberOfPostings">Максимальное количество постингов</param>
        /// <returns>Массив прочитанных постингов.</returns>
        Task<TermPosting[]> ReadPostingsAsync
            (
                string term,
                int numberOfPostings
            );

        /// <summary>
        /// Чтение постингов указанных терминов.
        /// </summary>
        /// <param name="parameters">Параметры постингов.</param>
        /// <returns>Массив прочитанных постингов.</returns>
        Task<TermPosting[]> ReadPostingsAsync
            (
                PostingParameters parameters
            );

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="startTerm">Параметры терминов.</param>
        /// <param name="numberOfTerms">Максимальное число терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        Task<Term[]> ReadTermsAsync
            (
                string startTerm,
                int numberOfTerms
            );

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="parameters">Параметры терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        Task<Term[]> ReadTermsAsync
            (
                TermParameters parameters
            );

        /// <summary>
        ///
        /// </summary>
        Task<string?> ReadTextFileAsync
            (
                string? specification
            );

        Task<bool> ReloadDictionaryAsync
            (
                string? database = default
            );

        Task<bool> ReloadMasterFileAsync
            (
                string? database = default
            );

        /// <summary>
        /// Перезапуск сервера.
        /// </summary>
        /// <returns></returns>
        Task<bool> RestartServerAsync();

        /// <summary>
        /// Восстановление флага подключения, ранее погашенного
        /// при помощи <see cref="Suspend"/>.
        /// </summary>
        void Rise();

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="parameters">Параметры поиска.</param>
        /// <returns>Массив элементов, описывающих найденные записи.</returns>
        Task<FoundItem[]> SearchAsync
            (
                SearchParameters parameters
            );

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Массив MFN найденных записей.</returns>
        Task<int[]> SearchAsync
            (
                string expression
            );

        /// <summary>
        /// Определение количества записей, удовлетворяющих
        /// заданному запросу.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
        Task<int> SearchCountAsync
            (
                string expression
            );

        Task<Record[]> SearchReadAsync
            (
                string expression,
                int limit = 0
            );

        Task<Record?> SearchSingleRecordAsync
            (
                string expression
            );

        /// <summary>
        /// Временно "закрывает" соединение с сервером
        /// (на самом деле соединение не разрывается)
        /// и сериализует его состояние в строку
        /// с возможностью последующего восстановления.
        /// </summary>
        /// <returns></returns>
        string Suspend();

        Task<bool> TruncateDatabaseAsync
            (
                string? database = default
            );

        Task<bool> UnlockDatabaseAsync
            (
                string? database = default
            );

        Task<bool> UnlockRecordsAsync
            (
                IEnumerable<int> mfnList,
                string? database = null
            );

        Task<bool> UpdateIniFileAsync
            (
                IEnumerable<string> lines
            );

        Task<bool> UpdateUserListAsync
            (
                IEnumerable<UserInfo> userList
            );

        Task<int> WriteRecordAsync
            (
                Record record,
                bool lockFlag = false,
                bool actualize = true,
                bool dontParse = false
            );

        Task<Record[]> WriteRecordsAsync
            (
                Record[] records,
                bool lockFlag = false,
                bool actualize = true
            );

        Task<bool> WriteTextFileAsync
            (
                params FileSpecification[] specifications
            );

            */

    } // interface IIrbisConnection

} // namespace ManagedIrbis
