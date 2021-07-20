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

using ManagedIrbis;
using ManagedIrbis.CommandLine;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
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

        private readonly ILogger<ApiController> _logger;
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
        /// <param name="databaseName">Имя базы данных.</param>
        private IActionResult DbInfo
            (
                string? databaseName
            )
        {
            if (databaseName.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.GetDatabaseInfo(databaseName);
            if (result is null || connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method DbInfo

        /// <summary>
        /// Получение списка баз данных.
        /// </summary>
        /// <param name="spec">Спецификация MNU-файла со списком баз.</param>
        private IActionResult ListDb
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
        private IActionResult ListFiles
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
        private IActionResult ListProcesses()
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
        /// <param name="databaseName">Имя базы данных</param>
        /// <param name="prefix">Префикс</param>
        private IActionResult ListTerms
            (
                string? databaseName,
                string? prefix
            )
        {
            if (databaseName.IsEmpty())
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

            connection.Database = databaseName;
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
        /// <param name="databaseName">Имя базы данных.</param>
        private IActionResult MaxMfn
            (
                string? databaseName
            )
        {
            if (databaseName.IsEmpty())
            {
                return BadRequest("Database not specified");
            }

            using var connection = GetConnection();
            if (!connection.Connected)
            {
                return Problem("Can't connect to IRBIS64");
            }

            var result = connection.GetMaxMfn(databaseName);
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
        private IActionResult ReadMenu
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
        /// <param name="databaseName">Имя базы данных.</param>
        /// <param name="mfn">MFN записи.</param>
        private IActionResult ReadRecord
            (
                string? databaseName,
                string? mfn
            )
        {
            if (databaseName.IsEmpty())
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

        private IActionResult ReadOpt(string? spec) => Ok();

        /// <summary>
        /// Чтение терминов поискового словаря.
        /// </summary>
        /// <param name="databaseName">Имя базы данных.</param>
        /// <param name="startTerm">Начальный термин.</param>
        /// <param name="count">Количество считываемых терминов.</param>
        private IActionResult ReadTerms
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
        private IActionResult ReadTextFile
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
        private IActionResult Restart()
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
        private IActionResult Scenarios()
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
        /// <param name="databaseName"></param>
        /// <param name="expression"></param>
        private IActionResult Search
            (
                string? databaseName,
                string? expression
            )
        {
            if (databaseName.IsEmpty())
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

            var result = connection.Search(expression);
            if (connection.LastError < 0)
            {
                return Problem(IrbisException.GetErrorDescription(connection.LastError));
            }

            return Ok(result);

        } // method Search

        /// <summary>
        /// Получение количества записей, удовлетворяющих запросу.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <param name="expression"></param>
        private IActionResult SearchCount
            (
                string? databaseName,
                string? expression
            )
        {
            if (databaseName.IsEmpty())
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
        /// <param name="databaseName">Имя базы данных</param>
        /// <param name="expression">Поисковое выражение.</param>
        /// <param name="format">Формат.</param>
        private IActionResult SearchFormat
            (
                string? databaseName,
                string? expression,
                string? format
            )
        {
            if (databaseName.IsEmpty())
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
                Database = databaseName,
                Expression = expression,
                Format = format
            };
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
        private IActionResult ServerStat()
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

        } // method ServerStat

        /// <summary>
        /// Получение списка зарегистрированных в система пользователей.
        /// </summary>
        private IActionResult UserList()
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
        private IActionResult Version()
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
                string? count
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
                    return Search(db, expr);

                case "search_count":
                    return SearchCount(db, expr);

                case "search_format":
                    return SearchFormat(db, expr, format);

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
