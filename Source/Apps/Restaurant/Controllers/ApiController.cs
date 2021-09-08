// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ApiController.cs -- контроллер API
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Collections;
using ManagedIrbis;
using ManagedIrbis.CommandLine;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Opt;
using ManagedIrbis.Providers;

using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace Restaurant.Controllers
{
    /// <summary>
    /// Контроллер API.
    /// </summary>
    [ApiController]
    [Route("/api")]
    public sealed class ApiController
        : ControllerBase
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="logger">Логгер.</param>
        public ApiController
            (
                IConfiguration configuration,
                ILogger<ApiController> logger
            )
        {
            _configuration = configuration;
            _logger = logger;
        }

        #endregion

        #region Private members

        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Построение настроек подключения.
        /// </summary>
        /// <remarks>
        /// Метод обязан вернуть корректные настройки подключения
        /// либо выбросить исключение.
        /// </remarks>
        private static ConnectionSettings BuildConnectionSettings
            (
                IConfiguration configuration,
                string[]? args = null
            )
        {
            args ??= Array.Empty<string>();

            // сначала берем настройки из стандартного JSON-файла конфигурации
            var connectionString = ConnectionUtility.GetConfiguredConnectionString(configuration)
                                   ?? ConnectionUtility.GetStandardConnectionString();

            var result = new ConnectionSettings();
            if (!string.IsNullOrEmpty(connectionString))
            {
                result.ParseConnectionString(connectionString);
            }

            // затем из опционального файла с настройками подключения
            connectionString = ConnectionUtility.GetConnectionStringFromFile();
            if (!string.IsNullOrEmpty(connectionString))
            {
                result.ParseConnectionString(connectionString);
            }

            // затем из переменных окружения
            CommandLineUtility.ConfigureConnectionFromEnvironment(result);

            // наконец, из командной строки
            CommandLineUtility.ConfigureConnectionFromCommandLine(result, args);

            // Применяем настройки по умолчанию, если соответствующие элементы не заданы
            result.ApplyDefaults();

            // Logger.LogInformation($"Using connection settings: {Settings}");

            if (!result.Verify(false))
            {
                throw new IrbisException("Can't build connection settings");
            }

            return result;

        } // method BuildConnectionSettings

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        private ISyncProvider GetConnection()
        {
            var settings = BuildConnectionSettings(_configuration);
            var result = new SyncConnection();
            settings.Apply((ISyncProvider) result);
            result.SetLogger(_logger);
            result.Connect();

            return result;

        } // method GetConnection

        /// <summary>
        /// Получение информации о базе данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        [HttpGet("db_info/{database}")]
        public IActionResult DbInfo
            (
                string? database
            )
        {
            if (database.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.GetDatabaseInfo(database);
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method DbInfo

        /// <summary>
        /// Форматирование записи.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN записи.</param>
        /// <param name="format">Спецификация формата.</param>
        /// <returns>Результат расформатирования.</returns>
        [HttpGet("format/{database}/{mfn}/{format?}")]
        public IActionResult Format
            (
                string? database,
                string? mfn,
                string? format
            )
        {
            if (string.IsNullOrEmpty(mfn))
            {
                return Problem("MFN not specified");
            }

            var mfnValue = mfn.SafeToInt32();
            if (mfnValue <= 0)
            {
                return Problem("Bad MFN");
            }

            if (string.IsNullOrEmpty(database))
            {
                database = "IBIS";
            }

            if (string.IsNullOrEmpty(format))
            {
                format = "@brief";
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            connection.Database = database;

            var result = connection.FormatRecord(format, mfnValue);

            return Ok(result);

        } // method Format

        /// <summary>
        /// Поиск книги по инвентарному номеру
        /// с последующим расформатированием найденной записи.
        /// </summary>
        /// <param name="number">Искомый инвентарный номер.</param>
        /// <param name="database">Имя базы данных.</param>
        /// <returns>Библиографическое описание найденной книги либо
        /// "Not found".</returns>
        /// <remarks>
        /// Сделано специально для Политеха.
        /// </remarks>
        [HttpGet("inventory/{number}/{database?}")]
        public IActionResult Inventory
            (
                string? number,
                string? database = null
            )
        {
            if (number.IsEmpty())
            {
                return BadRequest("Number not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var parameters = new SearchParameters
            {
                Database = database ?? "ISTU",
                Expression = $"\"IN={number}\"",
                NumberOfRecords = 1,
                Format = "@brief"
            };
            var found = connection.Search(parameters);
            if (found.IsNullOrEmpty())
            {
                return Ok("Not found");
            }

            return Ok(found[0].Text);

        } // method Inventory

        /// <summary>
        /// Поиск книги по номеру карточки безынвентарного учета
        /// с последующим расформатированием найденной записи.
        /// </summary>
        /// <param name="number">Искомый номер карточки.</param>
        /// <param name="database">Имя базы данных.</param>
        /// <returns>Библиографическое описание найденной книги либо
        /// "Not found".</returns>
        /// <remarks>
        /// Сделано специально для Политеха.
        /// </remarks>
        [HttpGet("kk/{number}/{database?}")]
        public IActionResult Kk
            (
                string? number,
                string? database = null
            )
        {
            if (number.IsEmpty())
            {
                return BadRequest("Number not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var parameters = new SearchParameters
            {
                Database = database ?? "ISTU",
                Expression = $"\"NS={number}\"",
                NumberOfRecords = 1,
                Format = "@brief"
            };
            var found = connection.Search(parameters);
            if (found.IsNullOrEmpty())
            {
                return Ok("Not found");
            }

            return Ok(found[0].Text);

        } // method Inventory

        /// <summary>
        /// Получение списка баз данных.
        /// </summary>
        /// <param name="spec">Спецификация MNU-файла со списком баз.</param>
        [HttpGet("list_db/{spec?}")]
        public IActionResult ListDb
            (
                string? spec
            )
        {
            spec ??= "dbnam3.mnu";

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.ListDatabases(spec);

            return Ok(result);

        } // method ListDb

        /// <summary>
        /// Получение списка файлов.
        /// </summary>
        /// <param name="pattern">Спецификация.</param>
        [HttpGet("list_files/{pattern}")]
        public IActionResult ListFiles
            (
                string? pattern
            )
        {
            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            pattern ??= $"2.{connection.Database}.*.*";
            var specification = FileSpecification.Parse(pattern);
            var result = connection.ListFiles(specification);
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method ListFiles

        /// <summary>
        /// Получение списка серверных процессов.
        /// </summary>
        [HttpGet("list_proc")]
        public IActionResult ListProcesses()
        {
            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.ListProcesses();
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method ListProcesses

        /// <summary>
        /// Получение списка терминов поискового словаря.
        /// </summary>
        /// <param name="database">Имя базы данных</param>
        /// <param name="prefix">Префикс</param>
        [HttpGet("list_terms/{databaseName}/{prefix}")]
        public IActionResult ListTerms
            (
                string? database,
                string? prefix
            )
        {
            if (database.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            if (prefix.IsEmpty())
            {
                return BadRequest("Prefix not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            connection.Database = database;
            var result = connection.ReadAllTerms(prefix);
            if (connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(Term.TrimToString(result, prefix));

        } // method ListTerms

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        [HttpGet("max_mfn/{database}")]
        public IActionResult MaxMfn
            (
                string? database
            )
        {
            if (database.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.GetMaxMfn(database);
            if (result < 0)
            {
                return Problem(IrbisException.GetErrorDescription(result));
            }

            if (connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method MaxMfn

        /// <summary>
        /// Получение меню с сервера.
        /// </summary>
        /// <param name="fileName">Спецификация.</param>
        [HttpGet("read_menu/{fileName}")]
        public IActionResult ReadMenu
            (
                string? fileName
            )
        {
            if (fileName.IsEmpty())
            {
                return BadRequest("File name not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var specification = FileSpecification.Parse(fileName);
            var result = connection.ReadMenu(specification);
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method ReadMenu

        /// <summary>
        /// Чтение записи с сервера.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="mfn">MFN записи.</param>
        [HttpGet("read/{database}/{mfn}")]
        public IActionResult ReadRecord
            (
                string? database,
                string? mfn
            )
        {
            if (database.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            if (mfn.IsEmpty())
            {
                return BadRequest("MFN not specified");
            }

            var number = mfn.SafeToInt32();
            if (number <= 0)
            {
                return BadRequest("Bad MFN value");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.ReadRecord(number);
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method ReadRecord

        /// <summary>
        /// Чтение настроек оптимизации показа.
        /// </summary>
        /// <param name="fileName">Спецификация файла</param>
        [HttpGet("read_opt/{fileName}")]
        public IActionResult ReadOpt
            (
                string? fileName
            )
        {
            if (fileName.IsEmpty())
            {
                return BadRequest("File name not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var specification = FileSpecification.Parse(fileName);
            var result = connection.ReadOptFile(specification);
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method ReadOpt

        /// <summary>
        /// Чтение терминов поискового словаря.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.</param>
        /// <param name="startTerm">Начальный термин.</param>
        /// <param name="count">Количество считываемых терминов.</param>
        public IActionResult ReadTerms
            (
                string? databaseName,
                string? startTerm,
                string? count
            )
        {
            if (databaseName.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            if (startTerm.IsEmpty())
            {
                return BadRequest("Start term not specified");
            }

            var number = count.SafeToInt32(100);
            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var paremeters = new TermParameters()
            {
                Database = databaseName,
                StartTerm = startTerm,
                NumberOfTerms = number
            };
            var result = connection.ReadTerms(paremeters);
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method ReadTerms

        /// <summary>
        /// Получение текстового файла с сервера.
        /// </summary>
        [HttpGet("read_text/{fileName}")]
        public IActionResult ReadTextFile
            (
                string? fileName
            )
        {
            if (fileName.IsEmpty())
            {
                return BadRequest("File name not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var specification = FileSpecification.Parse(fileName);
            var result = connection.ReadTextFile(specification);
            if (connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method ReadTextFile

        /// <summary>
        /// Перезапуск сервиса.
        /// </summary>
        [HttpGet("restart")]
        public IActionResult Restart()
        {
            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            if (!connection.RestartServer())
            {
                return Problem("Can't restart server");
            }

            return Ok();

        } // method Restart

        /// <summary>
        /// Получение поисковых сценариев.
        /// </summary>
        [HttpGet("scenarios")]
        public IActionResult Scenarios()
        {
            // TODO поддержать стандартный сценарий поиска

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            return BadRequest("Not supported");

        } // method Scenarios

        /// <summary>
        /// Поиск записей.
        /// </summary>
        /// <param name="database">Имя базы данных.</param>
        /// <param name="expression">Выражение на поисковом языке ИРБИС64.</param>
        /// <param name="start">Стартовое смещение (опционально).</param>
        /// <param name="count">Максимальное количество записей (опционально).</param>
        [HttpGet("search/{database}/{expression}/{count?}/{start?}")]
        public IActionResult Search
            (
                string? database,
                string? expression,
                string? start,
                string? count
            )
        {
            if (database.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            if (expression.IsEmpty())
            {
                return BadRequest("Expression not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var parameters = new SearchParameters
            {
                Database = database,
                Expression = expression
            };

            if (start is not null)
            {
                parameters.FirstRecord = start.SafeToInt32();
            }

            if (count is not null)
            {
                parameters.NumberOfRecords = count.SafeToInt32();
            }

            var found = connection.Search(parameters);
            if (connection.LastError < 0 || found is null)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            var result = FoundItem.ToMfn(found);

            return Ok(result);

        } // method Search

        /// <summary>
        /// Получение количества записей, удовлетворяющих запросу.
        /// </summary>
        /// <param name="database">Имя базы данных</param>
        /// <param name="expression">Поисковое выражение</param>
        [HttpGet("search_count/{database}/{expression}")]
        public IActionResult SearchCount
            (
                string? database,
                string? expression
            )
        {
            if (database.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            if (expression.IsEmpty())
            {
                return BadRequest("Expression not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            // var parameters = new SearchParameters
            // {
            //     Database = databaseName,
            //     Expression = expression,
            //     NumberOfRecords = 0
            // };
            var result = connection switch
            {
                ISyncConnection syncConnection => syncConnection.SearchCount(expression),

                // TODO поддерка других типов подключений

                _ => throw new IrbisException()
            };

            if (connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method SearchCount

        /// <summary>
        /// Поиск с форматированием.
        /// </summary>
        /// <param name="database">Имя базы данных</param>
        /// <param name="expression">Поисковое выражение.</param>
        /// <param name="start">Стартовое смещение (опционально).</param>
        /// <param name="count">Максимальное количество найденных записей (опционально).</param>
        /// <param name="format">Формат.</param>
        [HttpGet("search_format/{database}/{expression}/{format}/{count?}/{start?}")]
        public IActionResult SearchFormat
            (
                string? database,
                string? expression,
                string? start,
                string? count,
                string? format
            )
        {
            if (database.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            if (expression.IsEmpty())
            {
                return BadRequest("Expression not specified");
            }

            if (format.IsEmpty())
            {
                return BadRequest("Format not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var parameters = new SearchParameters
            {
                Database = database,
                Expression = expression,
                Format = format
            };

            if (start is not null)
            {
                parameters.FirstRecord = start.SafeToInt32();
            }

            if (count is not null)
            {
                parameters.NumberOfRecords = count.SafeToInt32();
            }

            var found = connection.Search(parameters);
            if (found is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            var result = found.Select(item => item.Text).ToArray();

            return Ok(result);

        } // method SearchFormat

        /// <summary>
        /// Получение статистики работы сервера ИРБИС64.
        /// </summary>
        [HttpGet("server_stat")]
        public IActionResult ServerStat()
        {
            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.GetServerStat();
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method ServerStat

        /// <summary>
        /// Получение списка зарегистрированных в система пользователей.
        /// </summary>
        [HttpGet("user_list")]
        public IActionResult UserList()
        {
            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.ListUsers();
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method UserList

        /// <summary>
        /// Получение версии сервера ИРБИС64.
        /// </summary>
        [HttpGet("version")]
        public IActionResult Version()
        {
            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.GetServerVersion();
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            _logger.LogInformation($"Version: {result}");

            return Ok(result);

        } // method Version

        #endregion

        #region Public methods

        /// <summary>
        /// Выполнение различных запросов к серверу ИРБИС64.
        /// </summary>
        /// <param name="op">Код запрошенной операции.</param>
        /// <param name="db">Имя базы данных.</param>
        /// <param name="prefix">Префикс.</param>
        /// <param name="spec">Спецификация файла.</param>
        /// <param name="mfn">MFN</param>
        /// <param name="expr">Поисковое выражение</param>
        /// <param name="format">Формат</param>
        /// <param name="start">Стартовый термин</param>
        /// <param name="count">Количество считываемых терминов</param>
        /// <param name="number">Инвентарный номер или номер карточки комплектования</param>
        /// <returns>Результат выполнения.</returns>
        [HttpGet]
        public IActionResult Index
            (
                string? op,
                string? db,
                string? prefix,
                string? spec,
                string? mfn,
                string? expr,
                string? format,
                string? start,
                string? count,
                string? number
            )
        {
            _logger.LogTrace(Request.Path);

            if (string.IsNullOrWhiteSpace(op))
            {
                return BadRequest("operation not specified");
            }

            op = op.ToLowerInvariant().Trim();

            switch (op)
            {
                case "db_info":
                    return DbInfo(db);

                case "format":
                    return Format(db, mfn, format);

                case "inventory":
                    return Inventory(number, db);

                case "kk":
                    return Kk(number, db);

                case "list_db":
                    return ListDb(spec);

                case "list_files":
                    return ListFiles(spec);

                case "list_proc":
                    return ListProcesses();

                case "list_terms":
                    return ListTerms(db, prefix);

                case "max_mfn":
                    return MaxMfn(db);

                case "read":
                    return ReadRecord(db, mfn);

                case "read_menu":
                    return ReadMenu(spec);

                case "read_opt":
                    return ReadOpt(spec);

                case "read_terms":
                    return ReadTerms(db, start, count);

                case "read_text":
                    return ReadTextFile(spec);

                case "restart":
                    return Restart();

                case "scenarios":
                    return Scenarios();

                case "search":
                    return Search(db, expr, start, count);

                case "search_count":
                    return SearchCount(db, expr);

                case "search_format":
                    return SearchFormat(db, expr, start, count, format);

                case "server_stat":
                    return ServerStat();

                case "user_list":
                    return UserList();

                case "version":
                    return Version();

                default:
                    return BadRequest("unknown operation");
            }

        } // method Index

        #endregion

    } // class ApiController

} // namespace Restaurant
