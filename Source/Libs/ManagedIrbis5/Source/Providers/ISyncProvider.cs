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

/* ISyncProvider.cs -- интерфейс синхронного ИРБИС-провайдера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Интерфейс синхронного ИРБИС-провайдера.
    /// </summary>
    public interface ISyncProvider
        : IIrbisProvider
    {
        /// <summary>
        /// Актуализация записи.
        /// </summary>
        /// <param name="parameters">Параметры команды.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool ActualizeRecord
            (
                ActualizeRecordParameters parameters
            );

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        /// <returns>Признак успешного завершения операции.</returns>
        bool Connect();

        /// <summary>
        /// Создание базы данных на сервере.
        /// </summary>
        /// <param name="parameters">Параметры команды.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool CreateDatabase
            (
                CreateDatabaseParameters parameters
            );

        /// <summary>
        /// Создание поискового словаря в указанной базе данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.
        /// <c>null</c> означает текущую базу данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool CreateDictionary
            (
                string? databaseName = default
            );

        /// <summary>
        /// Удаление указанной базы данных на сервере.
        /// </summary>
        /// <param name="databaseName">Имя удалаемой базы данных.
        /// <c>null</c> означает текущую базу данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool DeleteDatabase
            (
                string? databaseName = default
            );

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        /// <returns>Признак успешного завершения операции.
        /// Как правило, его игнорируют.</returns>
        bool Disconnect();

        /// <summary>
        /// Существует ли указанный файл?
        /// </summary>
        /// <param name="specification">Спецификация пути к файлу.</param>
        /// <returns>Результат проверки.</returns>
        bool FileExist(FileSpecification specification);

        /// <summary>
        /// Форматирование записей.
        /// </summary>
        /// <param name="parameters">Параметры команды.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool FormatRecords
            (
                FormatRecordParameters parameters
            );

        /// <summary>
        /// Полнотекстовый поиск ИРБИС64+.
        /// </summary>
        /// <param name="searchParameters">Параметры поиска.</param>
        /// <param name="textParameters">Параметры полнотекстовых операций.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        FullTextResult? FullTextSearch
            (
                SearchParameters searchParameters,
                TextParameters textParameters
            );

        /// <summary>
        /// Получение информации об указанной базе данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных (опционально).
        /// <c>null</c> означает текущую базу данных.</param>
        /// <returns>Информация о базе данных.</returns>
        DatabaseInfo? GetDatabaseInfo
            (
                string? databaseName = default
            );

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// По умолчанию используется текущая база данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных (опционально).
        /// <c>null</c> означает текущую базу данных.</param>
        /// <returns>Максимальный MFN либо код ошибки.</returns>
        int GetMaxMfn
            (
                string? databaseName = default
            );

        /// <summary>
        /// Получение статистики с сервера.
        /// </summary>
        /// <returns>Серверная статистика.</returns>
        ServerStat? GetServerStat();

        /// <summary>
        /// Получение версии ИРБИС-сервера.
        /// </summary>
        /// <returns>Версия сервера.</returns>
        ServerVersion? GetServerVersion();

        /// <summary>
        /// Глобальная корректировка.
        /// </summary>
        /// <param name="settings">Настройки корректировки.</param>
        /// <returns>Результат корректировки.</returns>
        GblResult? GlobalCorrection
            (
                GblSettings settings
            );

        /// <summary>
        /// Получение списка файлов на сервере,
        /// удовлетворяющих указанным спецификациям.
        /// </summary>
        /// <param name="specifications">Массив спецификаций файлов.</param>
        /// <remarks>Массив найденных на сервере файлов.</remarks>
        string[]? ListFiles
            (
                params FileSpecification[] specifications
            );

        /// <summary>
        /// Получение списка процессов, работающих в данный момент
        /// на ИРБИС-сервере.
        /// </summary>
        /// <returns>Массив серверных процессов.</returns>
        ProcessInfo[]? ListProcesses();

        /// <summary>
        /// Получение списка пользователей, имеющих доступ к
        /// ИРБИС-серверу. Эти пользователи не обязательно должны
        /// быть залогинены в данный момент.
        /// </summary>
        /// <remarks>Массив известных системе пользователей.</remarks>
        UserInfo[]? ListUsers();

        /// <summary>
        /// Пустая операция, необходимая для поддержания связи
        /// с ИРБИС-сервером.
        /// </summary>
        /// <returns>Признак успешного завершения операции
        /// (как правило, игнорируется).</returns>
        bool NoOperation();

        /// <summary>
        /// Расформатирование таблицы на сервере
        /// </summary>
        /// <param name="definition">Определение таблицы.</param>
        /// <returns>RTF-текст, полученный в результате
        /// расформатирования.</returns>
        string? PrintTable
            (
                TableDefinition definition
            );

        /// <summary>
        /// Чтение двоичного файла (например, картинки) с сервера ИРБИС64.
        /// </summary>
        /// <param name="specification">Спецификация пути к файлу.</param>
        /// <returns>Содержимое файла.</returns>
        byte[]? ReadBinaryFile
            (
                FileSpecification specification
            );

        /// <summary>
        /// Считывание постингов для указанных терминов поискового словаря.
        /// </summary>
        /// <param name="parameters">Параметры операции.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        TermPosting[]? ReadPostings
            (
                PostingParameters parameters
            );

        /// <summary>
        /// Чтение записи с сервера.
        /// </summary>
        /// <param name="parameters">Параметры операции.</param>
        /// <returns>Прочитанная запись.</returns>
        Record? ReadRecord
            (
                ReadRecordParameters parameters
            );

        /// <summary>
        /// Получение постингов для указанных записи и префикса.
        /// </summary>
        /// <param name="parameters">Параметры чтения записи.</param>
        /// <param name="prefix">Префикс в виде <c>"A=$"</c></param>
        /// <returns>Массив прочитанных постингов.</returns>
        TermPosting[]? ReadRecordPostings
            (
                ReadRecordParameters parameters,
                string prefix
            );

        /// <summary>
        /// Получение терминов поискового словаря.
        /// </summary>
        /// <param name="parameters">Параметры операции.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        Term[]? ReadTerms
            (
                TermParameters parameters
            );

        /// <summary>
        /// Получение содержимого текстового файла с сервера
        /// согласно спецификации.
        /// </summary>
        /// <param name="specification">Спецификация пути к файлу.</param>
        /// <returns>Содержимое файла.</returns>
        string? ReadTextFile
            (
                FileSpecification specification
            );

        /// <summary>
        /// Пересоздание словаря для указанной базы данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool ReloadDictionary
            (
                string? databaseName = default
            );

        /// <summary>
        /// Пересоздание мастер-файла для указанной базы данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool ReloadMasterFile
            (
                string? databaseName = default
            );

        /// <summary>
        /// Асинхронный перезапуск сервера без утери подключенных клиентов.
        /// </summary>
        /// <returns>Признак успешного завергения операции.</returns>
        bool RestartServer();

        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="parameters">Параметры поиска.</param>
        /// <returns>Массив элементов, описывающих найденные записи.</returns>
        FoundItem[]? Search
            (
                SearchParameters parameters
            );

        /// <summary>
        /// Опустошение указанной базы данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool TruncateDatabase
            (
                string? databaseName = default
            );

        /// <summary>
        /// Разблокирование указанной базы данных.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию - текущая база данных.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool UnlockDatabase
            (
                string? databaseName = default
            );

        /// <summary>
        /// Разблокирование указанных записей в указанной базе данных.
        /// </summary>
        /// <param name="mfnList">Перечень MFN, подлежащих разблокировке.</param>
        /// <param name="databaseName">Имя базы данных.
        /// По умолчанию текущая база данных.</param>
        /// <returns>Признак успешности завершения операции.</returns>
        bool UnlockRecords
            (
                IEnumerable<int> mfnList,
                string? databaseName = default
            );

        /// <summary>
        /// Обновление указанных строк серверного INI-файла.
        /// </summary>
        /// <param name="lines">Измененные строки INI-файла.</param>
        /// <returns>Признак успешности завершения операции.</returns>
        bool UpdateIniFile
            (
                IEnumerable<string> lines
            );

        /// <summary>
        /// Обновление списка пользователей системы на сервере.
        /// </summary>
        /// <param name="users">Список известных системе пользователей.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool UpdateUserList
            (
                IEnumerable<UserInfo> users
            );

        /// <summary>
        /// Сохранение/обновление файла на сервере.
        /// </summary>
        /// <param name="specification">Спецификация файла
        /// (включает в себя содержимое файла).</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool WriteTextFile
            (
                FileSpecification specification
            );

        /// <summary>
        /// Сохранение/обновление библиографической записи на сервере.
        /// </summary>
        /// <param name="parameters">Параметры операции.</param>
        /// <returns>Признак успешного завершения операции.</returns>
        bool WriteRecord
            (
                WriteRecordParameters parameters
            );

    } // interface ISyncProvider

} // namespace ManagedIrbis
