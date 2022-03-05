// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* BeriService.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Collections;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

#endregion

namespace BeriChitai.Data;

public sealed class BeriService
{
    #region Constants

    public const string StatusPrefix = "BERI=";
    public const string FreeBook = "0";

    #endregion

    #region Properties

    public SyncConnection Connection { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BeriService
        (
            IConfiguration configuration
        )
    {
        Sure.NotNull (configuration);

        Connection = ConnectionFactory.Shared.CreateSyncConnection();
        var connectionString = configuration["irbis-connection"];
        Connection.ParseConnectionString (connectionString);
    }

    #endregion

    #region Public methods

    public ReaderInfo? FindReader
        (
            string ticket,
            string email
        )
    {
        try
        {
            Connection.PushDatabase ("RDR");
            var manager = new ReaderManager (Connection);
            var result = manager.GetReader (ticket);
            if (result is null)
            {
                return null;
            }

            if (!email.SameString (result.Email)
                && !email.SameString (result.HomePhone))
            {
                return null;
            }

            return result;
        }
        finally
        {
            Connection.PopDatabase();
        }
    }

    private bool ContainsBook
        (
            int mfn
        )
    {
        var expression = $"\"I={mfn}\"";

        //Connection.Connect();
        var result = Connection.SearchCount (expression) == 1;

        //Connection.Disconnect();

        return result;
    }

    public OrderResult OrderBooks
        (
            string ticket,
            string email,
            int[] mfns
        )
    {
        if (string.IsNullOrEmpty (ticket))
        {
            return new OrderResult
            {
                Ok = false,
                Message = "Не задан читательский билет"
            };
        }

        if (string.IsNullOrEmpty (email))
        {
            return new OrderResult
            {
                Ok = false,
                Message = "Не задан e-mail или телефон"
            };
        }

        if (mfns.IsNullOrEmpty())
        {
            return new OrderResult
            {
                Ok = false,
                Message = "Не заданы книги для заказа"
            };
        }


        mfns = mfns.Where (item => ContainsBook (item)).ToArray();
        if (mfns.Length == 0)
        {
            return new OrderResult
            {
                Ok = false,
                Message = "Все MFN вне пределов базы"
            };
        }

        var reader = FindReader (ticket, email);
        if (ReferenceEquals (reader, null))
        {
            return new OrderResult
            {
                Ok = false,
                Message = "Неверные учётные данные"
            };
        }

        // Тот, кто умеет размещать заказы
        var manager = new BeriManager (Connection);

        // Читатель может быть лишен права пользования библиотекой
        if (!manager.IsReaderEnabled (reader))
        {
            return new OrderResult
            {
                Ok = false,
                Message = "Читатель лишён права пользования библиотекой"
            };
        }

        // Собственно заказ книг
        foreach (var mfn in mfns)
        {
            if (!manager.CreateBooking (mfn, reader))
            {
                return new OrderResult
                {
                    Ok = false,
                    Message = $"Ошибка при заказе книги с MFN {mfn}"
                };
            }
        } // foreach

        return new OrderResult
        {
            Ok = true,
            Message = "Заказ успешно размещён"
        };
    }

    public BookInfo[] AllBooks()
    {
        var parameters = new SearchParameters()
        {
            Database = Connection.EnsureDatabase(),
            Expression = StatusPrefix + FreeBook,
            Format = "@brief"
        };
        var found = Connection.Search (parameters);
        if (found is null)
        {
            return Array.Empty<BookInfo>();
        }

        var books = found
            .Select (book => new BookInfo
                {
                    Mfn = book.Mfn,
                    Description = book.Text,
                    Selected = false
                })
            .Where (book => book.Mfn != 0)
            .ToArray();
        return CorrectBooks (books);
    }

    public BookInfo[] ReadBooks
        (
            int[] mfns
        )
    {
        if (mfns.Length == 0)
        {
            return Array.Empty<BookInfo>();
        }

        mfns = mfns.Where (item => ContainsBook (item)).ToArray();
        if (mfns.Length == 0)
        {
            return Array.Empty<BookInfo>();
        }

        var parameters = new FormatRecordParameters()
        {
            Database = Connection.EnsureDatabase(),
            Format = "@brief",
            Mfns = mfns
        };
        var success = Connection.FormatRecords (parameters);
        if (!success)
        {
            return Array.Empty<BookInfo>();
        }
        var result = parameters.Result.AsArray()
            .Zip (mfns, (first, second) => new BookInfo
            {
                Selected = false,
                Mfn = second,
                Description = first
            })
            .ToArray();

        return CorrectBooks (result);
    }

    public BookInfo[] CorrectBooks
        (
            BookInfo[] books
        )
    {
        var mfns = books.Select (book => book.Mfn).ToArray();
        var parameters = new FormatRecordParameters()
        {
            Database = Connection.EnsureDatabase(),
            Format = "v903",
            Mfns = mfns
        };
        if (!Connection.FormatRecords (parameters))
        {
            return Array.Empty<BookInfo>();
        }

        var indexes = parameters.Result.AsArray();
        for (var i = 0; i < indexes.Length; i++)
        {
            books[i].Mfn = indexes[i].SafeToInt32();
        }

        return books;
    }

    public BookInfo[] RandomBooks()
    {
        var parameters = new SearchParameters()
        {
            Database = Connection.EnsureDatabase(),
            Expression = StatusPrefix + FreeBook,
            Format = "@brief",
            NumberOfRecords = 100
        };
        var result = new List<BookInfo>();
        var found =
        (
            Connection.Search (parameters)
            ?? Array.Empty<FoundItem>()
        ).ToList();
        var random = new Random();
        for (var i = 0; i < 10 && found.Count != 0; i++)
        {
            var index = random.Next (found.Count);
            var one = found[index];
            found.RemoveAt (index);
            var record = Connection.ReadRecord (one.Mfn);
            var book = new BookInfo
            {
                Selected = false,
                Mfn = one.Mfn,
                Description = one.Text,
                Cover = record?.FM (951, 'a')
            };
            result.Add (book);
        }

        return CorrectBooks (result.ToArray());
    }

    public List<BookInfo> SearchBooks
        (
            string keyword
        )
    {
        var result = new List<BookInfo>();
        var parameters = new SearchParameters()
        {
            Database = Connection.EnsureDatabase(),
            Expression = $"\"K={keyword}$\"",
            Format = "@brief",
            NumberOfRecords = 100
        };
        var found = Connection.Search (parameters);
        if (found.IsNullOrEmpty())
        {
            return result;
        }

        foreach (var item in found)
        {
            var record = Connection.ReadRecord (item.Mfn);
            if (record is not null)
            {
                var book = new BookInfo()
                {
                    Selected = false,
                    Mfn = record.FM (903).SafeToInt32(),
                    Cover = record.FM (951, 'a'),
                    Description = item.Text
                };
                result.Add (book);
            }
        }

        return result;
    }

    public int CountBooks()
    {
        return Connection.SearchCount (StatusPrefix + FreeBook);
    }

    public bool Connect()
    {
        return Connection.Connect();
    }

    public bool Disconnect()
    {
        return Connection.Connect();
    }

    #endregion
}
