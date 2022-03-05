// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* BeriManager.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;

#endregion

namespace BeriChitai.Data;

public sealed class BeriManager
{
    #region Constants

    /// <summary>
    /// Статус экземпляра.
    /// </summary>
    public const string StatusPrefix = "BERI=";

    /// <summary>
    /// Книга, доступная для заказа.
    /// </summary>
    public const string FreeBook = "0";

    /// <summary>
    /// Заказанная кем-либо книга.
    /// </summary>
    public const string ReservedBook = "1";

    /// <summary>
    /// Отданная читателю книга.
    /// </summary>
    public const string SurrenderedBook = "2";

    /// <summary>
    /// Дата бронирования.
    /// </summary>
    public const string DatePrefix = "DAB=";

    /// <summary>
    /// Читательский билет.
    /// </summary>
    public const string TicletPrefix = "CAB=";

    /// <summary>
    /// Дата выдачи.
    /// </summary>
    public const string IssuePrefix = "DAV=";

    /// <summary>
    /// Населенный пункт.
    /// </summary>
    public const string LocalityPrefix = "NAP=";

    #endregion

    #region Properties

    /// <summary>
    /// Подключение к серверу.
    /// </summary>
    public SyncConnection Connection { get; }

    /// <summary>
    /// Формат библиографического описания.
    /// </summary>
    public string Format { get; }

    #endregion

    #region Construction

    // /// <summary>
    // /// Конструктор.
    // /// </summary>
    // public BeriManager
    //     (
    //         IConfiguration configuration
    //     )
    // {
    //     Sure.NotNull (configuration);
    //
    //     Format = "@";
    //     Connection = ConnectionFactory.Shared.CreateSyncConnection();
    //     var connectionString = configuration["irbis-connection"];
    //     Connection.ParseConnectionString (connectionString);
    // }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BeriManager
        (
            SyncConnection connection
        )
    {
        Sure.NotNull (connection);

        Connection = connection;
        Format = "@";
    }

    #endregion

    #region Private members

    private static string? PrepareDescription
        (
            string? description
        )
    {
        if (string.IsNullOrEmpty (description))
        {
            return description;
        }

        var result = description.Replace
            (
                "</><dd> (Нет сведений об экземплярах)<br>",
                string.Empty
            );
        result = result.Replace
            (
                "<b> Нет сведений об экземплярах</b>",
                string.Empty
            );

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Расширение информации о книгах.
    /// </summary>
    public void ExtendInfo
        (
            IEnumerable<BeriInfo> books
        )
    {
        Sure.NotNull ((object)books);

        Connection.Connect();
        var array = books.ToArray();

        if (!string.IsNullOrEmpty (Format))
        {
            foreach (var book in array)
            {
                if (string.IsNullOrEmpty (book.Description)
                    && !ReferenceEquals (book.Record, null))
                {
                    var description = Connection.FormatRecord
                        (
                            Format,
                            book.Record.Mfn
                        );
                    description = PrepareDescription (description);
                    book.Description = description;
                }
            }
        }

        Connection.PushDatabase (StandardDatabases.Readers);
        try
        {
            var readerManager = new ReaderManager (Connection);
            foreach (var book in array)
            {
                var ticket = book.Ticket;
                if (ReferenceEquals (book.Reader, null)
                    && !string.IsNullOrEmpty (ticket))
                {
                    var reader = readerManager.GetReader (ticket);
                    book.Reader = reader;
                    if (!ReferenceEquals (reader, null))
                    {
                        reader.Description = Connection.FormatRecord ("@", reader.Mfn);
                    }
                }
            }
        }
        finally
        {
            Connection.PopDatabase();
        }

        Connection.Disconnect();
    }

    /// <summary>
    /// Получение списка книг с указанным статусом.
    /// </summary>
    public BeriInfo[] GetBooksWithStatus
        (
            string status
        )
    {
        Sure.NotNullNorEmpty (status);

        Connection.Connect();
        var expression = $"\"{StatusPrefix}{status}\"";

        var records = BatchRecordReader.Search
            (
                Connection,
                expression,
                Connection.EnsureDatabase()
            ).ToArray();

        var result = records
            .SelectMany (record => BeriInfo.Parse (record))
            .ToArray();
        Connection.Disconnect();

        return result;
    }

    /// <summary>
    /// Получение списка доступных для заказа книг.
    /// </summary>
    public BeriInfo[] GetFreeBooks()
    {
        return GetBooksWithStatus (FreeBook);
    }

    /// <summary>
    /// Получение списка заказанных книг.
    /// </summary>
    public BeriInfo[] GetReservedBooks()
    {
        var result = GetBooksWithStatus (ReservedBook);
        ExtendInfo (result);

        return result;
    }

    /// <summary>
    /// Получение списка выданных читателю книг.
    /// </summary>
    public BeriInfo[] GetSurrenderedBooks()
    {
        var result = GetBooksWithStatus (SurrenderedBook);
        ExtendInfo (result);

        return result;
    }

    /// <summary>
    /// Может ли читатель получать книги.
    /// </summary>
    public bool IsReaderEnabled
        (
            ReaderInfo reader
        )
    {
        Sure.NotNull (reader);

        return string.IsNullOrEmpty (reader.Rights);
    }

    /// <summary>
    /// Создание заказа на книгу.
    /// </summary>
    public bool CreateBooking
        (
            int mfn,
            ReaderInfo reader
        )
    {
        Sure.Positive (mfn);
        Sure.NotNull (reader);

        Connection.Connect();
        var record = Connection.SearchReadOneRecord ($"\"I={mfn}\"");
        if (record is null)
        {
            Connection.Disconnect();
            return false;
        }

        var already = BeriInfo.Parse (record);
        if (already.Length != 0)
        {
            Connection.Disconnect();
            return false;
        }

        var ticket = reader.Ticket;
        if (string.IsNullOrEmpty (ticket))
        {
            Connection.Disconnect();
            return false;
        }

        var field = new Field (BeriInfo.BeriTag)
            .Add ('a', IrbisDate.TodayText)
            .Add ('b', ticket);
        record.Add (field);

        Connection.WriteRecord (record, false, true);
        Connection.Disconnect();

        return true;
    }

    /// <summary>
    /// Находим книгу по указанному индексу.
    /// </summary>
    public BeriInfo[]? GetBook
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        Connection.Connect();
        var record = Connection.SearchReadOneRecord ($"\"I={index}\"");
        Connection.Disconnect();
        if (record is null)
        {
            return null;
        }

        var result = BeriInfo.Parse (record);

        return result;
    }

    /// <summary>
    /// Регистрация выдачи книги.
    /// </summary>
    public bool GiveBook
        (
            BeriInfo book
        )
    {
        Sure.NotNull (book);

        var record = book.Record.ThrowIfNull();
        var field = book.Field.ThrowIfNull();
        if (!ReferenceEquals (field.Record, record))
        {
            throw new IrbisException ("the field doesn't belong the record");
        }

        field.SetSubFieldValue ('c', IrbisDate.TodayText);

        Connection.Connect();
        Connection.WriteRecord (record);
        Connection.Disconnect();

        return true;
    }

    /// <summary>
    /// Отменить заказ на книгу.
    /// </summary>
    public bool CancelBooking
        (
            BeriInfo book
        )
    {
        Sure.NotNull (book);

        // var reader = book.Reader.ThrowIfNull();
        var record = book.Record.ThrowIfNull();
        var field = book.Field.ThrowIfNull();
        if (!ReferenceEquals (field.Record, record))
        {
            throw new IrbisException ("the field doesn't belong the record");
        }

        record.Fields.Remove (field);

        Connection.Connect();
        Connection.WriteRecord (record);
        Connection.Disconnect();

        return true;
    }

    #endregion
}
