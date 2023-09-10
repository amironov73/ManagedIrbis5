// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Constants.cs -- общие для ИРБИС64 константы
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Общие для ИРБИС64 константы.
    /// </summary>
    public static class Constants
    {
        #region Коды команд для сервера ИРБИС64

        /// <summary>
        /// Получение признака монопольной блокировки базы данных.
        /// </summary>
        public const string ExclusiveDatabaseLock = "#";

        /// <summary>
        /// Новый полнотекстовый поиск для ИРБИС64+.
        /// </summary>
        public const string NewFulltextSearch = "&";

        /// <summary>
        /// Фасеты по результатам поиска.
        /// </summary>
        public const string SearchCell = "$";

        /// <summary>
        /// Получение списка удаленных, неактуализированных
        /// и заблокированных записей.
        /// </summary>
        public const string RecordList = "0";

        /// <summary>
        /// Получение версии сервера.
        /// </summary>
        public const string ServerInfo = "1";

        /// <summary>
        /// Получение статистики по базе данных.
        /// </summary>
        public const string DatabaseStat = "2";

        /// <summary>
        /// IRBIS_FORMAT_ISO_GROUP.
        /// </summary>
        public const string FormatIsoGroup = "3";

        /// <summary>
        /// Сбросить запущенную задачу только на сервере.
        /// </summary>
        public const string StopClientProcess = "4";

        /// <summary>
        /// Глобальная корректировка.
        /// </summary>
        /// <remarks>IRBIS_GBL</remarks>
        public const string GlobalCorrection = "5";

        /// <summary>
        /// Сохранение группы записей.
        /// </summary>
        public const string SaveRecordGroup = "6";

        /// <summary>
        /// Печать.
        /// </summary>
        public const string Print = "7";

        /// <summary>
        /// Запись параметров в ini-файл, расположенный на сервере.
        /// </summary>
        public const string UpdateIniFile = "8";

        /// <summary>
        /// IRBIS_IMPORT_ISO.
        /// </summary>
        public const string ImportIso = "9";

        /// <summary>
        /// Регистрация клиента на сервере.
        /// </summary>
        /// <remarks>IRBIS_REG</remarks>
        public const string RegisterClient = "A";

        /// <summary>
        /// Разрегистрация клиента.
        /// </summary>
        /// <remarks>IRBIS_UNREG</remarks>
        public const string UnregisterClient = "B";

        /// <summary>
        /// Чтение записи, ее расформатирование.
        /// </summary>
        /// <remarks>IRBIS_READ</remarks>
        public const string ReadRecord = "C";

        /// <summary>
        /// Сохранение записи.
        /// </summary>
        /// <remarks>IRBIS_UPDATE</remarks>
        public const string UpdateRecord = "D";

        /// <summary>
        /// Разблокировка записи.
        /// </summary>
        /// <remarks>IRBIS_RUNLOCK</remarks>
        public const string UnlockRecord = "E";

        /// <summary>
        /// Актуализация записи.
        /// </summary>
        /// <remarks>IRBIS_RECIFUPDATE</remarks>
        public const string ActualizeRecord = "F";

        /// <summary>
        /// Форматирование записи или группы записей.
        /// </summary>
        /// <remarks>IRBIS_SVR_FORMAT</remarks>
        public const string FormatRecord = "G";

        /// <summary>
        /// Получение терминов и ссылок словаря, форматирование записей
        /// </summary>
        /// <remarks>IRBIS_TRM_READ</remarks>
        public const string ReadTerms = "H";

        /// <summary>
        /// Получение ссылок для термина (списка терминов).
        /// </summary>
        /// <remarks>IRBIS_POSTING</remarks>
        public const string ReadPostings = "I";

        /// <summary>
        /// Глобальная корректировка виртуальной записи.
        /// </summary>
        /// <remarks>IRBIS_GBL_RECORD</remarks>
        public const string CorrectVirtualRecord = "J";

        /// <summary>
        /// Поиск записей с опциональным форматированием
        /// (также последовательный поиск).
        /// </summary>
        /// <remarks>IRBIS_SEARCH</remarks>
        public const string Search = "K";

        /// <summary>
        /// Получение/сохранение текстового файла, расположенного
        /// на сервере (группы текстовых файлов).
        /// </summary>
        public const string ReadDocument = "L";

        /// <summary>
        /// IRBIS_BACKUP.
        /// </summary>
        public const string Backup = "M";

        /// <summary>
        /// Пустая операция. Периодическое подтверждение
        /// соединения с сервером.
        /// </summary>
        /// <remarks>IRBIS_NOOP</remarks>
        public const string Nop = "N";

        /// <summary>
        /// Получение максимального MFN для базы данных.
        /// </summary>
        /// <remarks>IRBIS_MAXMFN</remarks>
        public const string GetMaxMfn = "O";

        /// <summary>
        /// Получение терминов и ссылок словаря в обратном порядке.
        /// </summary>
        public const string ReadTermsReverse = "P";

        /// <summary>
        /// Разблокирование записей.
        /// </summary>
        public const string UnlockRecords = "Q";

        /// <summary>
        /// Полнотекстовый поиск.
        /// </summary>
        /// <remarks>IRBIS_FULLTEXT_SEARCH</remarks>
        public const string FullTextSearch = "R";

        /// <summary>
        /// Опустошение базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_EMPTY</remarks>
        public const string EmptyDatabase = "S";

        /// <summary>
        /// Создание базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_NEW</remarks>
        public const string CreateDatabase = "T";

        /// <summary>
        /// Разблокирование базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_UNLOCK</remarks>
        public const string UnlockDatabase = "U";

        /// <summary>
        /// Чтение ссылок для заданного MFN.
        /// </summary>
        /// <remarks>IRBIS_MFN_POSTINGS</remarks>
        public const string GetRecordPostings = "V";

        /// <summary>
        /// Удаление базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_DELETE</remarks>
        public const string DeleteDatabase = "W";

        /// <summary>
        /// Реорганизация мастер-файла.
        /// </summary>
        /// <remarks>IRBIS_RELOAD_MASTER</remarks>
        public const string ReloadMasterFile = "X";

        /// <summary>
        /// Реорганизация словаря.
        /// </summary>
        /// <remarks>IRBIS_RELOAD_DICT</remarks>
        public const string ReloadDictionary = "Y";

        /// <summary>
        /// Создание поискового словаря заново.
        /// </summary>
        /// <remarks>IRBIS_CREATE_DICT</remarks>
        public const string CreateDictionary = "Z";

        /// <summary>
        /// Получение статистики работы сервера.
        /// </summary>
        /// <remarks>IRBIS_STAT</remarks>
        public const string GetServerStat = "+1";

        /// <summary>
        /// Список запущенных потоков.
        /// </summary>
        public const string GetThreadList = "+2";

        /// <summary>
        /// Получение списка запущенных процессов.
        /// </summary>
        public const string GetProcessList = "+3";

        /// <summary>
        /// Сбросить запущенную задачу.
        /// </summary>
        public const string StopProcess = "+4";

        /// <summary>
        /// Сбросить запущенный поток.
        /// </summary>
        public const string StopThread = "+5";

        /// <summary>
        /// Сбросить зарегистрированного клиента.
        /// </summary>
        public const string StopClient = "+6";

        /// <summary>
        /// Сохранение списка пользователей.
        /// </summary>
        public const string SetUserList = "+7";

        /// <summary>
        /// Перезапуск сервера.
        /// </summary>
        public const string RestartServer = "+8";

        /// <summary>
        /// Получение списка пользователей.
        /// </summary>
        public const string GetUserList = "+9";

        /// <summary>
        /// Получение списка файлов на сервере.
        /// </summary>
        public const string ListFiles = "!";

        #endregion

        #region Прочие константы

        /// <summary>
        /// Начало диапазона валидных кодов подполей.
        /// </summary>
        public const char FirstCode = '!';

        /// <summary>
        /// Конец диапазона валидных кодов подполей (включая!).
        /// </summary>
        public const char LastCode = '~';

        /// <summary>
        /// Имя файла, содержащего список баз данных для администратора.
        /// </summary>
        public const string AdministratorDatabaseList = "dbnam1.mnu";

        /// <summary>
        /// Имя файла, содержащего список баз данных для каталогизатора.
        /// </summary>
        public const string CatalogerDatabaseList = "dbnam2.mnu";

        /// <summary>
        /// Максимальная длина (размер полки) - ограничение формата.
        /// </summary>
        public const int MaxRecord = 32000;

        /// <summary>
        /// Максимальное количество постингов в пакете.
        /// </summary>
        public const int MaxPostings = 32758;

        /// <summary>
        /// Имя файла, содержащего список баз данных для читателя.
        /// </summary>
        public const string ReaderDatabaseList = "dbnam3.mnu";

        /// <summary>
        /// Стандартный разделитель строк ИРБИС.
        /// </summary>
        public const string IrbisDelimiter = "\x001F\x001E";

        /// <summary>
        /// Короткий разделитель строк в ИРБИС.
        /// </summary>
        public static readonly char[] ShortIrbisDelimiterBytes = { '\x1F' };

        /// <summary>
        /// Стандартный разделитель строк ИРБИС.
        /// </summary>
        public static readonly byte[] IrbisDelimiterBytes = { 0x1F, 0x1E };

        /// <summary>
        /// Стандартный разделитель строк в DOS/Windows.
        /// </summary>
        public const string WindowsDelimiter = "\r\n";

        /// <summary>
        /// Стандартный разделитель строк в DOS/Windows.
        /// </summary>
        public static readonly byte[] WindowsDelimiterBytes = { 13, 10 };

        /// <summary>
        /// Точка с запятой.
        /// </summary>
        public static readonly char[] Semicolon = { ';' };

        /// <summary>
        /// Знак равенства.
        /// </summary>
        public static readonly char[] EqualSign = { '=' };

        /// <summary>
        /// Решетка.
        /// </summary>
        public static readonly char[] NumberSign = { '#' };

        /// <summary>
        /// Преамбула для двоичных файлов.
        /// IRBIS_BINARY_DATA
        /// </summary>
        public static readonly byte[] Preamble =
        {
            73, 82, 66, 73, 83, 95, 66, 73, 78, 65, 82, 89, 95, 68,
            65, 84, 65
        };

        /// <summary>
        /// Допустимые коды для чтения записей с сервера.
        /// </summary>
        public static readonly int[] GoodCodesForReadRecord = { -201, -600, -602, -603 };

        /// <summary>
        /// Допустимые коды для чтения терминов с сервера.
        /// </summary>
        public static readonly int[] GoodCodesForReadTerms = { -202, -203, -204 };

        #endregion
    }
}
