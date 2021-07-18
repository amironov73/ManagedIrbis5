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

using AM;

using ManagedIrbis;
using ManagedIrbis.CommandLine;
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

        private IActionResult DbInfo(string? db) => Ok($"Информация о базе '{db}'");

        private IActionResult ListDb(string? spec) => Ok($"Список баз данных '{spec}'");

        private IActionResult ListFiles(string? spec) => Ok($"Список файлов '{spec}'");

        private IActionResult ListProcesses() => Ok();

        private IActionResult ListTerms(string? db, string? prefix) => Ok();

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
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

        private IActionResult ReadRecord(string? db, string? mfn) => Ok();

        private IActionResult ReadMenu(string? spec) => Ok();

        private IActionResult ReadOpt(string? spec) => Ok();

        private IActionResult ReadTerms() => Ok();

        private IActionResult ReadTextFile(string? spec) => Ok();

        private IActionResult Restart() => Ok();

        private IActionResult Scenarios() => Ok();

        private IActionResult Search(string? db, string? expr) => Ok();

        private IActionResult SearchCount(string? db, string? expr) => Ok();

        private IActionResult SearchFormat(string? db, string? expr, string? format) => Ok();

        private IActionResult ServerStat() => Ok();

        private IActionResult UserList() => Ok();

        private IActionResult Version() => Ok();

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
                string? format
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
                    return ReadTerms();

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

            // return Ok("Hello from controller: " + op);
        }

        #endregion

    } // class ApiController

} // namespace Restaurant
