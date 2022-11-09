// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ReaderController.cs -- контроллер API для доступа к БД читателей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using Istu.OldModel;
using Istu.OldModel.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace Studen.Controllers;

/// <summary>
/// Контроллер API для доступа к БД читателей.
/// </summary>
[ApiController]
public sealed class ReaderController
    : ControllerBase
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов</param>
    /// <param name="configuration">Конфигурация.</param>
    /// <param name="logger">Логгер.</param>
    public ReaderController
        (
            IServiceProvider serviceProvider,
            IConfiguration configuration,
            ILogger<ReaderController> logger
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

    #endregion

    #region Public methods

    /// <summary>
    /// Поиск читателя по номеру билета.
    /// </summary>
    /// <param name="ticket">Искомый номер</param>
    /// <returns>Найденный читатель.</returns>
    [HttpGet ("ticket/{ticket}")]
    public IActionResult FindByTicket
        (
            string? ticket
        )
    {
        _logger.LogInformation
            (
                $"{nameof (FindByTicket)}:: {nameof (ticket)}={ticket}"
            );

        if (string.IsNullOrEmpty (ticket))
        {
            return Problem ("Ticket not specified");
        }

        using var readerManager = GetReaderManager();
        var found = readerManager.GetReaderByTicket (ticket);

        return Ok (found);
    }

    /// <summary>
    /// Поиск читателя по штрих-коду.
    /// </summary>
    /// <param name="barcode">Искомый штрих-код</param>
    /// <returns>Найденный читатель.</returns>
    [HttpGet ("barcode/{barcode}")]
    public IActionResult FindByBarcode
        (
            string? barcode
        )
    {
        _logger.LogInformation
            (
                $"{nameof (FindByBarcode)}:: {nameof (barcode)}={barcode}"
            );

        if (string.IsNullOrEmpty (barcode))
        {
            return Problem ("Barcode not specified");
        }

        using var readerManager = GetReaderManager();
        var found = readerManager.GetReaderByBarcode (barcode);

        return Ok (found);
    }

    /// <summary>
    /// Поиск читателя по радио-метке.
    /// </summary>
    /// <param name="rfid">Искомая метка</param>
    /// <returns>Найденный читатель.</returns>
    [HttpGet ("rfid/{rfid}")]
    public IActionResult FindByRfid
        (
            string? rfid
        )
    {
        _logger.LogInformation
            (
                $"{nameof (FindByRfid)}:: {nameof (rfid)}={rfid}"
            );

        if (string.IsNullOrEmpty (rfid))
        {
            return Problem ("Rfid not specified");
        }

        using var readerManager = GetReaderManager();
        var found = readerManager.GetReaderByRfid (rfid);

        return Ok (found);
    }

    /// <summary>
    /// Поиск читателя по идентификатору ИРНИТУ.
    /// </summary>
    /// <param name="id">Искомый идентификатор</param>
    /// <returns>Найденный читатель.</returns>
    [HttpGet ("istu/{id}")]
    public IActionResult FindByIstu
        (
            string? id
        )
    {
        _logger.LogInformation
            (
                $"{nameof (FindByIstu)}:: {nameof (id)}={id}"
            );

        if (string.IsNullOrEmpty (id))
        {
            return Problem ("ID not specified");
        }

        var istuId = id.SafeToInt32();
        if (istuId <= 0)
        {
            return Problem ("Bad ID");
        }

        using var readerManager = GetReaderManager();
        var found = readerManager.GetReaderByIstuId (istuId);

        return Ok (found);
    }

    /// <summary>
    /// Поиск читателя по номеру билета и паролю.
    /// </summary>
    /// <param name="ticket">Искомый номер</param>
    /// <param name="password">Пароль</param>
    /// <returns>Найденный читатель.</returns>
    [HttpGet ("password/{ticket}/{password}")]
    public IActionResult FindByTicketAndPassword
        (
            string? ticket,
            string? password
        )
    {
        _logger.LogInformation
            (
                $"{nameof (FindByTicketAndPassword)}:: {nameof (ticket)}={ticket} {nameof (password)}={password}"
            );

        if (string.IsNullOrEmpty (ticket))
        {
            return Problem ("Ticket not specified");
        }

        if (string.IsNullOrEmpty (password))
        {
            return Problem ("Password not specified");
        }

        using var readerManager = GetReaderManager();
        var found = readerManager.GetReaderByTicketAndPassword (ticket, password);

        return Ok (found);
    }

    /// <summary>
    /// Поиск читателя по имени.
    /// </summary>
    /// <param name="name">Имя (возможно, маска).</param>
    /// <returns>Найденные читатели.</returns>
    [HttpGet ("name/{name}")]
    public IActionResult FindByName
        (
            string? name
        )
    {
        _logger.LogInformation
            (
                $"{nameof (FindByName)}:: + {nameof (name)}={name}"
            );

        if (string.IsNullOrEmpty (name))
        {
            return Problem ("Name not specified");
        }

        using var readerManager = GetReaderManager();
        var found = readerManager.FindReaders (ReaderSearchCriteria.Name, name, 100000);

        return Ok (found);
    }

    /// <summary>
    /// Поиск читателя по группе.
    /// </summary>
    /// <param name="group">Шифр группы (возможно, маска).</param>
    /// <returns>Найденные читатели.</returns>
    [HttpGet ("group/{group}")]
    public IActionResult FindByGroup
        (
            string? group
        )
    {
        _logger.LogInformation
            (
                $"{nameof (FindByGroup)}:: + {nameof (group)}={group}"
            );

        if (string.IsNullOrEmpty (group))
        {
            return Problem ("Group not specified");
        }

        using var readerManager = GetReaderManager();
        var found = readerManager.FindReaders (ReaderSearchCriteria.Group, group, 100000);

        return Ok (found);
    }

    /// <summary>
    /// Произвольный поиск читателей в базе.
    /// </summary>
    /// <param name="expression">SQL-выражение со всеми select, where и что там ещё</param>
    /// <returns>Найденные читатели</returns>
    [HttpGet ("readers/{expression}")]
    public IActionResult SearchReaders
        (
            string? expression
        )
    {
        _logger.LogInformation
            (
                $"{nameof (SearchReaders)}:: + {nameof (expression)}={expression}"
            );

        if (string.IsNullOrEmpty (expression))
        {
            return Problem ("Expression not specified");
        }

        using var readerManager = GetReaderManager();
        var found = readerManager.Search (expression);

        return Ok (found);
    }

    /// <summary>
    /// Удаление читателя по номеру билета.
    /// </summary>
    /// <param name="ticket">Номер билета</param>
    [HttpGet ("delete_reader/{ticket}")]
    public IActionResult DeleteByTicket
        (
            string? ticket
        )
    {
        _logger.LogInformation
            (
                $"{nameof (DeleteByTicket)}:: + {nameof (ticket)}={ticket}"
            );

        if (string.IsNullOrEmpty (ticket))
        {
            return Problem ("Ticket not specified");
        }

        using var readerManager = GetReaderManager();
        readerManager.DeleteReader (ticket);

        return Ok();
    }

    /// <summary>
    /// Создание читателя в базе.
    /// </summary>
    [HttpPost ("create_reader")]
    public IActionResult CreateReader
        (
            [FromBody] Reader reader
        )
    {
        _logger.LogInformation
            (
                $"{nameof (CreateReader)}:: + {nameof (reader)}={reader.Ticket}"
            );

        // TODO верифицировать

        reader.Registered = DateTime.Now.ToString ("dd.MM.yyyy");
        reader.Moment = DateTime.Now;
        reader.Operator = 1;

        using var readerManager = GetReaderManager();
        readerManager.CreateReader (reader);

        return Ok();
    }

    /// <summary>
    /// Обновление читателя в базе (по идентификатору, а не по номеру читательского!).
    /// </summary>
    [HttpPost ("update_reader")]
    public IActionResult UpdateReader
        (
            [FromBody] Reader reader
        )
    {
        _logger.LogInformation
            (
                $"{nameof (UpdateReader)}:: + {nameof (reader)}={reader.Ticket}"
            );

        // TODO верифицировать

        using var readerManager = GetReaderManager();
        readerManager.UpdateReaderInfo (reader);

        return Ok();
    }

/*

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
        }

*/

    #endregion
}
