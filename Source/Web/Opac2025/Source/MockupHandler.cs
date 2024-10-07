// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* Mockup.cs -- заглушка для тестирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using Swashbuckle.AspNetCore.Annotations;

#endregion

namespace Opac2025;

/// <summary>
/// Заглушка для тестирования.
/// </summary>
internal sealed class MockupHandler
    : IApiHandler
{
    #region Properties

    /// <summary>
    /// Список книг в фонде.
    /// </summary>
    public readonly List<Book> Books =
    [
        new ()
        {
            Id = "K1/2",
            Description = "Паустовский, Константин. Исаак Левитан : повесть. — М., 1937",
            Cover = "cover/332441.png",
            Links =
            [
                new ()
                {
                    Url = "https://lanbook.com",
                    Description = "ЭБС"
                }
            ],
            Exemplars =
            [
                new ()
                {
                    Number = "A6001",
                    Status = "ok",
                    Sigla = "ФКХ"
                },
                new ()
                {
                    Number = "A60002",
                    Status = "absent",
                    Sigla = "ЧЗ"
                }
            ]
        },
        new ()
        {
            Id = "K2/3",
            Description = "Федоров-Давыдов, А. А. И. И. Левитан. Жизнь и творчество. — М., 1960",
            Cover = "cover/398241.png",
            Exemplars =
            [
                new ()
                {
                    Number = "A6003",
                    Status = "ok",
                    Sigla = "ФКХ"
                },
                new ()
                {
                    Number = "A60004",
                    Status = "waiting",
                    Sigla = "ЧЗ"
                }
            ]
        },
        new ()
        {
            Id = "K3/4",
            Description = "Левитан М. А. Палеоокеанология Индийского океана в мелунеогене. - М.: Наука. 1992. 244с.",
            Cover = "cover/450241.png",
            Links =
            [
                new ()
                {
                    Url = "https://lanbook.com",
                    Description = "ЭБС"
                }
            ],
            Exemplars =
            [
                new ()
                {
                    Number = "A6005",
                    Status = "ok",
                    Sigla = "ЧЗ"
                },
                new ()
                {
                    Number = "A60006",
                    Status = "ok",
                    Sigla = "ФКХ"
                }
            ]
        },
        new ()
        {
            Id = "K3/4",
            Description = "Бродский Б. И. Романтические ведуты: Рассказы об удивительных городах и знаменитых постройках древнего мира. — М.: Советский художник. — (Страницы истории искусств)",
            Cover = "cover/458841.png",
            Exemplars =
            [
                new ()
                {
                    Number = "A6005",
                    Status = "ok",
                    Sigla = "ЧЗ"
                },
                new ()
                {
                    Number = "A60006",
                    Status = "loan",
                    Sigla = "ФКХ"
                }
            ]
        },
        new ()
        {
            Id = "K5/7",
            Description = "Кузнецов Э. Д. Звери и птицы Евгения Чарушина. — М.: Советский художник, 1983. — 160 с. — (Рассказы о художниках)",
            Cover = "cover/477141.png",
            Exemplars =
            [
                new ()
                {
                    Number = "A6007",
                    Status = "ok",
                    Sigla = "ЧЗ"
                },
                new ()
                {
                    Number = "A60008",
                    Status = "loan",
                    Sigla = "ФКХ"
                }
            ]
        },
        new ()
        {
            Id = "L711",
            Description = "Сагалович М. В. По следам Фернана Леже. — М.: Советский художник, 1983. — 352 с. — (Рассказы о художниках)",
            // Cover = "cover/483641.png",
            Links =
            [
                new ()
                {
                    Url = "https://lanbook.com",
                    Description = "ЭБС"
                }
            ],
            Exemplars =
            [
                new ()
                {
                    Number = "B6102",
                    Status = "ok",
                    Sigla = "ЧЗ"
                },
                new ()
                {
                    Number = "B6103",
                    Status = "loan",
                    Sigla = "ФКХ"
                }
            ]
        },
        new ()
        {
            Id = "M712",
            Description = "Варшавский А. Подвиг художника. — М.: Советский художник, 1965. — 176 с. — (Страницы истории искусств)",
            Cover = "cover/484541.png",
            Links =
            [
                new ()
                {
                    Url = "https://lanbook.com",
                    Description = "ЭБС"
                }
            ],
            Exemplars =
            [
                new ()
                {
                    Number = "M1112",
                    Status = "ok",
                    Sigla = "ФКХ"
                },
                new ()
                {
                    Number = "M1113",
                    Status = "ok",
                    Sigla = "ЧЗ"
                }
            ]
        },
    ];

    /// <summary>
    /// Список доступных баз данных.
    /// </summary>
    public readonly List<Database> Databases =
    [
        new ()
        {
            Name = "ISTU",
            Description = "Учебники, монографии и продолжающиеся издания"
        },
        new ()
        {
            Name = "HUDO",
            Description = "Художественная литература"
        },
        new ()
        {
            Name = "NTD",
            Description = "Нормативно-техническая документация"
        },
        new ()
        {
            Name = "USO",
            Description = "Литература в филиалах ИРНИТУ"
        },
    ];

    /// <summary>
    /// Список читателей
    /// </summary>
    public readonly List<Reader> Readers =
    [
        new ()
        {
            Ticket = "00001",
            Name = "Билибин Иван Яковлевич",
            Mail = "bilibin@mail.ru",
            Loans = []
        },
        new ()
        {
            Ticket = "00002",
            Name = "Васнецов Юрий Алексеевич",
            Mail = "vasyur@mail.ru",
            Loans = []
        },
        new ()
        {
            Ticket = "00003",
            Name = "Репин Илья Ефимович",
            Mail = "repin1844@gmail.com",
            Loans =
            [
                new Loan
                {
                    Instance  = new Instance
                    {
                        Database = "ISTU",
                        Book = "L711",
                        Number = "B6103"
                    },
                    Description = "Сагалович М. В. По следам Фернана Леже. — М.: Советский художник, 1983. — 352 с. — (Рассказы о художниках)",
                    Date = OpacUtility.LocalDate (2024, 9, 1),
                    Deadline = OpacUtility.LocalDate (2024, 10, 1),
                    Prolongation = 0
                },
            ]
        },
        new ()
        {
            Ticket = "00004",
            Name = "Рерих Николай Константинович",
            Mail = "tibetman@mail.ru",
            Loans =
            [
                new Loan
                {
                    Instance  = new Instance
                    {
                        Database = "ISTU",
                        Book = "K3/4",
                        Number = "A60006"
                    },
                    Description = "Бродский Б. И. Романтические ведуты: Рассказы об удивительных городах и знаменитых постройках древнего мира. — М.: Советский художник. — (Страницы истории искусств)",
                    Date = OpacUtility.LocalDate (2024, 9, 1),
                    Deadline = OpacUtility.LocalDate (2024, 10, 1),
                    Prolongation = 0
                },
            ]
        },
        new Reader
        {
            Ticket = "00005",
            Name = "Левитан Исаак Ильич",
            Mail = "canvas@mail.ru",
            Loans = []
        },
    ];

    /// <summary>
    /// Список сценариев поиска.
    /// </summary>
    public readonly List<Scenario> Scenarios =
    [
        new ()
        {
            Prefix  = "A=",
            Description = "Автор"
        },
        new ()
        {
            Prefix  = "T=",
            Description = "Заглавие"
        },
        new ()
        {
            Prefix  = "K=",
            Description = "Ключевое слово"
        },
    ];

    /// <summary>
    /// Список заказов.
    /// </summary>
    public readonly List<Order> Orders =
    [
        new ()
        {
            Id = 1,
            Ticket = "123",
            Description = "Какая-то книжка",
            Instance = new Instance
            {
                Database = "ISTU",
                Book = "V1/2",
                Number = "W0001"
            },
            Date = OpacUtility.LocalDate (2024, 1, 1),
            Status = Constants.New
        },

        new ()
        {
            Id = 2,
            Ticket = "124",
            Description = "Еще какая-то книжка",
            Instance = new Instance
            {
                Database = "ISTU",
                Book = "V2/3",
                Number = "W0002"
            },
            Date = OpacUtility.LocalDate (2024, 1, 2),
            Status = Constants.Done
        },

        new ()
        {
            Id = 3,
            Ticket = "125",
            Description = "Третья книжка",
            Instance = new Instance
            {
                Database = "ISTU",
                Book = "V3/4",
                Number = "W0003"
            },
            Date = OpacUtility.LocalDate (2024, 1, 3),
            Status = Constants.Ready
        },

        new ()
        {
            Id = 4,
            Ticket = "126",
            Description = "Четвертая книжка",
            Instance = new Instance
            {
                Database = "ISTU",
                Book = "V4/5",
                Number = "W0004"
            },
            Date = OpacUtility.LocalDate (2024, 1, 4),
            Status = Constants.Cancelled
        },

        new ()
        {
            Id = 5,
            Ticket = "126",
            Description = "Пятая книжка",
            Instance = new Instance
            {
                Database = "ISTU",
                Book = "V55/65",
                Number = "X0005",
            },
            Date = OpacUtility.LocalDate (2024, 1, 5),
            Status = Constants.Error
        }
    ];

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MockupHandler
        (
            ILogger logger
        )
    {
        _logger = logger;
    }

    #endregion


    #region Private members

    private int _nextOrderId = 6;

    private int GetNextOrderId() => _nextOrderId++;

    private readonly ILogger _logger;

    [SwaggerOperation ("List all orders")]
    private IResult ListOrders()
    {
        // using var storehouse = GetStorehouse();
        _logger.LogTrace (nameof (ListOrders) + ": OK");

        return Results.Json (Orders);
    }

    /// <summary>
    /// Перечисление заказов с указанным статусом.
    /// </summary>
    [SwaggerOperation ("Find orders with given status")]
    private IResult ListOrdersWithStatus
        (
            [SwaggerParameter ("Status to retrieve")]
            string status
        )
    {
        Sure.NotNullNorEmpty (status);

        var orders = Orders
            .Where (o => o.Status == status)
            .ToArray();

        _logger.LogTrace (nameof (ListOrdersWithStatus) + ": {Status}: OK", status);

        return Results.Json (orders);
    }

    /// <summary>
    /// Перечисление заказов указанного читателя.
    /// </summary>
    [SwaggerOperation ("Find orders of the reader")]
    private IResult ListOrdersOfReader
        (
            [SwaggerParameter ("Ticket number")]
            string ticket
        )
    {
        Sure.NotNullNorEmpty (ticket);

        var orders = Orders
            .Where (o => o.Ticket == ticket)
            .ToArray();

        _logger.LogTrace (nameof (ListOrdersOfReader) + ": {Ticket}: OK", ticket);

        return Results.Json (orders);
    }

    /// <summary>
    /// Перечисление заказов указанного читателя.
    /// </summary>
    [SwaggerOperation ("Find orders of the book exemplar")]
    private IResult ListOrdersOfExemplar
        (
            [SwaggerParameter ("Database name")]
            string database,

            [SwaggerParameter ("Book identifier")]
            string book,

            [SwaggerParameter ("Inventory number")]
            string number
        )
    {
        Sure.NotNullNorEmpty (database);
        Sure.NotNullNorEmpty (book);
        Sure.NotNullNorEmpty (number);

        var orders = Orders
            .Where (o => o.Instance?.Database == database
                && o.Instance.Book == book
                && o.Instance?.Number == number)
            .ToArray();

        _logger.LogTrace (nameof (ListOrdersOfExemplar) + ": {Number}: OK", number);

        return Results.Json (orders);
    }

    /// <summary>
    /// Создание нового заказа.
    /// </summary>
    [SwaggerOperation ("Create an order")]
    private IResult CreateOrder
        (
            [SwaggerParameter ("Order for creation")]
            Order newOrder
        )
    {
        Sure.NotNull (newOrder);

        newOrder.Id = GetNextOrderId();
        Orders.Add (newOrder);

        _logger.LogTrace (nameof (CreateOrder) + ": {Order}: OK", newOrder);

        return Results.Ok();
    }

    /// <summary>
    /// Обновление данных о заказе.
    /// </summary>
    [SwaggerOperation ("Update the order")]
    private IResult UpdateOrder
        (
            [SwaggerParameter ("New values for the order")]
            Order updatedOrder
        )
    {
        Sure.NotNull (updatedOrder);

        var existingOrder = Orders.SingleOrDefault
            (
                o => o.Id == updatedOrder.Id
            );
        if (existingOrder is null)
        {
            _logger.LogTrace (nameof (UpdateOrder)
                              + ": {Order}: not found", updatedOrder);
            return Results.NotFound();
        }

        existingOrder.Status = updatedOrder.Status;
        existingOrder.Instance = updatedOrder.Instance;
        existingOrder.Ticket = updatedOrder.Ticket;
        existingOrder.Date = updatedOrder.Date;

        _logger.LogTrace (nameof (UpdateOrder)
                          + ": {Order}: OK", updatedOrder);

        return Results.Ok();
    }

    /// <summary>
    /// Обновление данных о заказе.
    /// </summary>
    [SwaggerOperation ("Update an order status")]
    private IResult UpdateOrderStatus
        (
            [SwaggerParameter ("Order identifier")]
            int id,

            [SwaggerParameter ("New status value")]
            string status
        )
    {
        Sure.NonNegative (id);
        Sure.NotNullNorEmpty (status);

        var existingOrder = Orders.SingleOrDefault
            (
                o => o.Id == id
            );
        if (existingOrder is null)
        {
            _logger.LogTrace (nameof (UpdateOrderStatus) +
                              ": {OrderId} not found", id);
            return Results.NotFound();
        }

        existingOrder.Status = status;
        _logger.LogTrace (nameof (UpdateOrderStatus) +
                          ": {OrderId}, {Status}: OK", status, status);

        return Results.Json (existingOrder);
    }

    /// <summary>
    /// Удаление указанного заказа.
    /// </summary>
    [SwaggerOperation ("Delete the order")]
    private IResult DeleteOrder
        (
            [SwaggerParameter ("Identifier of the order to delete")]
            int id
        )
    {
        Sure.NonNegative (id);

        var existingOrder = Orders.SingleOrDefault
            (
                o => o.Id == id
            );
        if (existingOrder is null)
        {
            _logger.LogTrace (nameof (DeleteOrder) + ": {OrderId}: not found", id);
            return Results.NotFound();
        }

        Orders.Remove (existingOrder);
        _logger.LogTrace (nameof (DeleteOrder) + ": {Order}: OK", existingOrder);

        return Results.Ok();
    }

    /// <summary>
    /// Получение списка баз данных.
    /// </summary>
    [SwaggerOperation ("List all databases")]
    private IResult ListDatabases()
    {
        // using var storehouse = GetStorehouse();
        _logger.LogTrace (nameof (ListDatabases) + ": OK");

        return Results.Json (Databases);
    }

    /// <summary>
    /// Получение списка сценариев поиска.
    /// </summary>
    [SwaggerOperation ("List all search scenarios")]
    private IResult ListScenarios
        (
            [SwaggerParameter ("Name of the database")]
            string database
        )
    {
        Sure.NotNullNorEmpty (database);

        // using var storehouse = GetStorehouse();
        _logger.LogTrace (nameof (ListScenarios) +
                          ": {Database} : OK", database);

        return Results.Json (Scenarios);
    }

    /// <summary>
    /// Поиск читателя по номеру его билета.
    /// </summary>
    [SwaggerOperation ("Search the reader by ticket number")]
    private IResult GetReader
        (
            [SwaggerParameter ("Ticket number to search by")]
            string ticket
        )
    {
        Sure.NotNullNorEmpty (ticket);

        var reader = Readers.SingleOrDefault
            (
                r => r.Ticket == ticket
            );
        if (reader is null)
        {
            _logger.LogTrace (nameof (GetReader)
                              + ": {Ticket}: not found", ticket);
            return Results.NotFound();
        }

        _logger.LogTrace (nameof (GetReader) + ": OK: {Reader}", reader);

        return Results.Json (reader);
    }

    /// <summary>
    /// Поиск книг по каталогу.
    /// </summary>
    [SwaggerOperation ("Search for the books")]
    private IResult SearchBooks
        (
            [SwaggerParameter ("Database name")]
            string database,

            [SwaggerParameter ("Query text")]
            string query
        )
    {
        Sure.NotNullNorEmpty (database);
        Sure.NotNullNorEmpty (query);

        var books = Books;
        var result = new List<Book>();
        var random = new Random();
        var howMany = random.Next (books.Count);
        var found = new List<int>();
        for (var i = 0; i < howMany; i++)
        {
            while (true)
            {
                var candidate = random.Next (books.Count);
                if (!found.Contains (candidate))
                {
                    found.Add (candidate);
                    result.Add (books[i]);
                    break;
                }
            }
        }

        return Results.Json (result);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Регистрация обработчика.
    /// </summary>
    public void Register
        (
            RouteGroupBuilder api
        )
    {
        Sure.NotNull (api);

        api.MapGet ("/databases", ListDatabases);
        api.MapGet ("/scenarios/{database}", ListScenarios);
        api.MapGet ("/readers/{ticket}", GetReader);
        api.MapGet ("/search/{database}/{query}", SearchBooks);
        api.MapGet ("/orders", ListOrders);
        api.MapGet ("/orders/status/{status}", ListOrdersWithStatus);
        api.MapGet ("/orders/ticket/{ticket}", ListOrdersOfReader);
        api.MapGet ("/orders/number/{database}/{book}/{number}", ListOrdersOfExemplar);
        api.MapPost ("/orders", CreateOrder);
        api.MapPut ("/orders", UpdateOrder);
        api.MapPut ("/orders/status/{id}/{status}", UpdateOrderStatus);
        api.MapDelete ("/orders/{id}", DeleteOrder);
    }

    #endregion
}
