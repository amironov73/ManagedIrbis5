// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* Mockup.cs -- заглушка для тестирования
 * Ars Magna project, http://arsmagna.ru
 */

namespace Opac2025;

/// <summary>
/// Заглушка для тестирования.
/// </summary>
internal sealed class Mockup
{
    #region Properties

    /// <summary>
    /// Список книг в фонде.
    /// </summary>
    public readonly List<Book> Books =
    [
        new Book
        {
            Id = "K1/2",
            Description = "Паустовский, Константин. Исаак Левитан : повесть. — М., 1937",
            Exemplars =
            [
                new Exemplar
                {
                    Number = "A6001",
                    Status = "0"
                },
                new Exemplar
                {
                    Number = "A60002",
                    Status = "6"
                }
            ]
        },
        new Book
        {
            Id = "K2/3",
            Description = "Федоров-Давыдов, А. А. И. И. Левитан. Жизнь и творчество. — М., 1960",
            Exemplars =
            [
                new Exemplar
                {
                    Number = "A6003",
                    Status = "0"
                },
                new Exemplar
                {
                    Number = "A60004",
                    Status = "2"
                }
            ]
        },
        new Book
        {
            Id = "K3/4",
            Description = "Левитан М. А. Палеоокеанология Индийского океана в мелунеогене. - М.: Наука. 1992. 244с.",
            Exemplars =
            [
                new Exemplar
                {
                    Number = "A6005",
                    Status = "0"
                },
                new Exemplar
                {
                    Number = "A60006",
                    Status = "0"
                }
            ]
        },
        new Book
        {
            Id = "K3/4",
            Description = "Бродский Б. И. Романтические ведуты: Рассказы об удивительных городах и знаменитых постройках древнего мира. — М.: Советский художник. — (Страницы истории искусств)",
            Exemplars =
            [
                new Exemplar
                {
                    Number = "A6005",
                    Status = "0"
                },
                new Exemplar
                {
                    Number = "A60006",
                    Status = "1"
                }
            ]
        },
        new Book
        {
            Id = "K5/7",
            Description = "Кузнецов Э. Д. Звери и птицы Евгения Чарушина. — М.: Советский художник, 1983. — 160 с. — (Рассказы о художниках)",
            Exemplars =
            [
                new Exemplar
                {
                    Number = "A6007",
                    Status = "0"
                },
                new Exemplar
                {
                    Number = "A60008",
                    Status = "1"
                }
            ]
        },
        new Book
        {
            Id = "L711",
            Description = "Сагалович М. В. По следам Фернана Леже. — М.: Советский художник, 1983. — 352 с. — (Рассказы о художниках)",
            Exemplars =
            [
                new Exemplar
                {
                    Number = "B6102",
                    Status = "0"
                },
                new Exemplar
                {
                    Number = "B6103",
                    Status = "1"
                }
            ]
        },
        new Book
        {
            Id = "M712",
            Description = "Варшавский А. Подвиг художника. — М.: Советский художник, 1965. — 176 с. — (Страницы истории искусств)",
            Exemplars =
            [
                new Exemplar
                {
                    Number = "M1112",
                    Status = "0"
                },
                new Exemplar
                {
                    Number = "M1113",
                    Status = "0"
                }
            ]
        },
    ];

    /// <summary>
    /// Список доступных баз данных.
    /// </summary>
    public readonly List<Database> Databases =
    [
        new Database
        {
            Name = "ISTU",
            Description = "Учебники, монографии и продолжающиеся издания"
        },
        new Database
        {
            Name = "HUDO",
            Description = "Художественная литература"
        },
        new Database
        {
            Name = "NTD",
            Description = "Нормативно-техническая документация"
        },
        new Database
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
        new Reader
        {
            Ticket = "00001",
            Name = "Билибин Иван Яковлевич",
            Mail = "bilibin@mail.ru",
            Loans = []
        },
        new Reader
        {
            Ticket = "00002",
            Name = "Васнецов Юрий Алексеевич",
            Mail = "vasyur@mail.ru",
            Loans = []
        },
        new Reader
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
        new Reader
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
        new Scenario
        {
            Prefix  = "A=",
            Description = "Автор"
        },
        new Scenario
        {
            Prefix  = "T=",
            Description = "Заглавие"
        },
        new Scenario
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
        new Order
        {
            Id = 1,
            Ticket = "123",
            Instance = new Instance
            {
                Database = "ISTU",
                Book = "V1/2",
                Number = "W0001"
            },
            Date = OpacUtility.LocalDate (2024, 1, 1),
            Status = Constants.New
        },

        new Order
        {
            Id = 2,
            Ticket = "124",
            Instance = new Instance
            {
                Database = "ISTU",
                Book = "V2/3",
                Number = "W0002"
            },
            Date = OpacUtility.LocalDate (2024, 1, 2),
            Status = Constants.Done
        },

        new Order
        {
            Id = 3,
            Ticket = "125",
            Instance = new Instance
            {
                Database = "ISTU",
                Book = "V3/4",
                Number = "W0003"
            },
            Date = OpacUtility.LocalDate (2024, 1, 3),
            Status = Constants.Ready
        },

        new Order
        {
            Id = 4,
            Ticket = "126",
            Instance = new Instance
            {
                Database = "ISTU",
                Book = "V4/5",
                Number = "W0004"
            },
            Date = OpacUtility.LocalDate (2024, 1, 4),
            Status = Constants.Cancelled
        },

        new Order
        {
            Id = 5,
            Ticket = "126",
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

    private int _nextOrderId = 6;

    #endregion

    #region Public methods

    public int GetNextOrderId() => _nextOrderId++;

    public Order CreateOrder() => new() { Id = GetNextOrderId() };

    #endregion
}
