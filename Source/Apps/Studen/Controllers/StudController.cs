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
using AM.Globalization;
using Istu.OldModel;
using Istu.OldModel.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace Studen.Controllers
{
    /// <summary>
    /// Контроллер API.
    /// </summary>
    [ApiController]
    [Route("/stud")]
    public sealed class StudController
        : ControllerBase
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов</param>
        /// <param name="configuration">Конфигурация.</param>
        /// <param name="logger">Логгер.</param>
        public StudController
            (
                IServiceProvider serviceProvider,
                IConfiguration configuration,
                ILogger<StudController> logger
            )
        {
            _logger = logger;

            _storehouse = new Storehouse (serviceProvider, configuration);

        } // constructor

        #endregion

        #region Private members

        private readonly ILogger _logger;
        private readonly Storehouse _storehouse;

        private IReaderManager GetReaderManager() => _storehouse.GetRequiredService<IReaderManager>();

        /// <summary>
        /// Поиск читателя по номеру билета.
        /// </summary>
        /// <param name="ticket">Искомый номер</param>
        /// <returns>Найденный читатель.</returns>
        private IActionResult FindByTicket
            (
                string? ticket
            )
        {
            if (string.IsNullOrEmpty(ticket))
            {
                return Problem("Ticket not specified");
            }

            using var readerManager = GetReaderManager();
            var found = readerManager.GetReaderByTicket(ticket);

            return Ok(found);

        } // method FindByTicket

        /// <summary>
        /// Поиск читателя по штрих-коду.
        /// </summary>
        /// <param name="barcode">Искомый штрих-код</param>
        /// <returns>Найденный читатель.</returns>
        private IActionResult FindByBarcode
            (
                string? barcode
            )
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return Problem("Barcode not specified");
            }

            using var readerManager = GetReaderManager();
            var found = readerManager.GetReaderByBarcode(barcode);

            return Ok(found);

        } // method FindByBarcode

        /// <summary>
        /// Поиск читателя по радио-метке.
        /// </summary>
        /// <param name="rfid">Искомая метка</param>
        /// <returns>Найденный читатель.</returns>
        private IActionResult FindByRfid
            (
                string? rfid
            )
        {
            if (string.IsNullOrEmpty(rfid))
            {
                return Problem("Rfid not specified");
            }

            using var readerManager = GetReaderManager();
            var found = readerManager.GetReaderByRfid(rfid);

            return Ok(found);

        } // method FindByRfid

        /// <summary>
        /// Поиск читателя по идентификатору ИРНИТУ.
        /// </summary>
        /// <param name="id">Искомый идентификатор</param>
        /// <returns>Найденный читатель.</returns>
        private IActionResult FindByIstu
            (
                string? id
            )
        {
            if (string.IsNullOrEmpty(id))
            {
                return Problem("ID not specified");
            }

            var istuId = id.SafeToInt32();
            if (istuId <= 0)
            {
                return Problem("Bad ID");
            }

            using var readerManager = GetReaderManager();
            var found = readerManager.GetReaderByIstuID(istuId);

            return Ok(found);

        } // method FindByIstu

        /// <summary>
        /// Поиск читателя по номеру билета и паролю.
        /// </summary>
        /// <param name="ticket">Искомый номер</param>
        /// <param name="password">Пароль</param>
        /// <returns>Найденный читатель.</returns>
        private IActionResult FindByTickedAndPassword
            (
                string? ticket,
                string? password
            )
        {
            if (string.IsNullOrEmpty(ticket))
            {
                return Problem("Ticket not specified");
            }

            if (string.IsNullOrEmpty(password))
            {
                return Problem("Password not specified");
            }

            using var readerManager = GetReaderManager();
            var found = readerManager.GetReaderByTicketAndPassword(ticket, password);

            return Ok(found);

        } // method FindByTicket

        /// <summary>
        /// Поиск читателя по имени.
        /// </summary>
        /// <param name="name">Имя (возможно, маска).</param>
        /// <returns>Найденные читатели.</returns>
        private IActionResult FindByName
            (
                string? name
            )
        {
            if (string.IsNullOrEmpty(name))
            {
                return Problem("Name not specified");

            }

            using var readerManager = GetReaderManager();
            var found = readerManager.FindReaders(ReaderSearchCriteria.Name, name, 100000);

            return Ok(found);

        } // method FindByName

        /// <summary>
        /// Поиск читателя по группе.
        /// </summary>
        /// <param name="group">Шифр группы (возможно, маска).</param>
        /// <returns>Найденные читатели.</returns>
        private IActionResult FindByGroup
            (
                string? group
            )
        {
            if (string.IsNullOrEmpty(group))
            {
                return Problem("Group not specified");

            }

            using var readerManager = GetReaderManager();
            var found = readerManager.FindReaders(ReaderSearchCriteria.Group, group, 100000);

            return Ok(found);

        } // method FindByGroup

        /// <summary>
        /// Произвольный поиск читателей в базе.
        /// </summary>
        /// <param name="expression">SQL-выражение со всеми select, where и что там ещё</param>
        /// <returns>Найденные читатели</returns>
        private IActionResult Search
            (
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return Problem("Expression not specified");
            }

            using var readerManager = GetReaderManager();
            var found = readerManager.Search(expression);

            return Ok(found);

        } // method Search

        /// <summary>
        /// Удаление читателя по номеру билета.
        /// </summary>
        /// <param name="ticket">Номер билета</param>
        private IActionResult DeleteByTicket
            (
                string? ticket
            )
        {
            if (string.IsNullOrEmpty(ticket))
            {
                return Problem("Ticket not specified");
            }

            using var readerManager = GetReaderManager();
            readerManager.DeleteReader(ticket);

            return Ok();

        } // method Delete

        #endregion

        #region Public methods

        /// <summary>
        /// Создание читателя в базе.
        /// </summary>
        [HttpPost("create")]
        public IActionResult Create
            (
                [FromBody] Reader reader
            )
        {
            // TODO верифицировать

            reader.Registered = DateTime.Now.ToString("dd.MM.yyyy");
            reader.Moment = DateTime.Now;
            reader.Operator = 1;

            using var readerManager = GetReaderManager();
            readerManager.CreateReader(reader);

            return Ok();

        } // method Create

        /// <summary>
        /// Обновление читателя в базе (по идентификатору, а не по номеру читательского!).
        /// </summary>
        [HttpPost("update")]
        public IActionResult Update
            (
                [FromBody] Reader reader
            )
        {
            // TODO верифицировать

            using var readerManager = GetReaderManager();
            readerManager.UpdateReaderInfo(reader);

            return Ok();

        } // method Update

        /// <summary>
        /// Поиск читателя по номеру билета.
        /// </summary>
        [HttpGet("ticket/{ticket}")]
        public IActionResult Ticket(string? ticket) => FindByTicket(ticket);

        /// <summary>
        /// Поиск читателя по штрих-коду.
        /// </summary>
        [HttpGet("barcode/{barcode}")]
        public IActionResult Barcode(string? barcode) => FindByBarcode(barcode);

        /// <summary>
        /// Поиск читателя по радио-метке.
        /// </summary>
        [HttpGet("rfid/{rfid}")]
        public IActionResult Rfid(string? rfid) => FindByRfid(rfid);

        /// <summary>
        /// Поиск читателя по идентификатору ИРНИТУ.
        /// </summary>
        [HttpGet("istu/{id}")]
        public IActionResult Istu(string? id) => FindByIstu(id);

        /// <summary>
        /// Поиск читателя по номеру билета и паролю.
        /// </summary>
        /// <returns></returns>
        [HttpGet("password/{ticket}/{password}")]
        public IActionResult Password(string? ticket, string? password) =>
            FindByTickedAndPassword(ticket, password);

        /// <summary>
        /// Поиск читателей по шифру группы.
        /// </summary>
        [HttpGet("group/{group}")]
        public IActionResult Group(string? group) => FindByGroup(group);

        /// <summary>
        /// Удаление читателя по номеру билета.
        /// </summary>
        [HttpGet("delete/{ticket}")]
        public IActionResult Delete(string? ticket) => DeleteByTicket(ticket);

        /// <summary>
        /// Выполнение различных запросов к серверу MSSQL.
        /// </summary>
        /// <param name="op">Код запрошенной операции.</param>
        /// <param name="value">Искомое значение.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Результат выполнения.</returns>
        [HttpGet]
        public IActionResult Index
            (
                string? op,
                string? value,
                string? password
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
                case "ticket":
                    return FindByTicket(value);

                case "barcode":
                    return FindByBarcode(value);

                case "rfid":
                    return FindByRfid(value);

                case "istu":
                    return FindByIstu(value);

                case "password":
                    return FindByTickedAndPassword(value, password);

                case "name":
                    return FindByName(value);

                case "group":
                    return FindByGroup(value);

                case "search":
                    return Search(value);

                case "delete":
                    return DeleteByTicket(value);

                default:
                    return BadRequest("unknown operation");
            }

        } // method Index

        #endregion

    } // class ApiController

} // namespace Restaurant
