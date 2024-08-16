// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* IrbisLib.cs -- библиотека функций для ИРБИС
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AM;
using AM.Collections;
using AM.Configuration;
using AM.Scripting.Barsik;
using AM.Text;

using ManagedIrbis.AppServices;
using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.PftLite;
using ManagedIrbis.Providers;
using ManagedIrbis.Readers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Logging;

using static AM.Scripting.Barsik.Builtins;

#endregion

namespace ManagedIrbis.Scripting.Barsik;

/// <summary>
/// Библиотека функций для ИРБИС.
/// </summary>
public sealed class IrbisLib
    : IBarsikModule
{
    #region Constants

    /// <summary>
    /// Имя дефайна, хранящего текущее подключение.
    /// </summary>
    public const string ConnectionDefineName = "connection";

    /// <summary>
    /// Имя дефайна, хранящего текущую запись.
    /// </summary>
    public const string RecordDefineName = "record";

    /// <summary>
    /// Имя дефайна, хранящего выходной буфер.
    /// </summary>
    public const string OutputDefineName = "$output";

    /// <summary>
    /// Имя дефайна, отслеживающего вывод в группе.
    /// </summary>
    public const string MarkDefineName = "$mark";

    #endregion

    #region Properties

    // TODO добавить аналоги операторов глобальной корректировки
    // Оператор ADD
    // Оператор REP
    // Оператор CHA/CHAC
    // Оператор DEL
    // Оператор UNDOR (откат)

    // TODO добавить работу с читателями
    // parse_reader

    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "actualize", new FunctionDescriptor ("actualize", Actualize) },
        { "add_dot", new FunctionDescriptor ("add_dot", AddDot) },
        { "add_field", new FunctionDescriptor ("add_field", AddField) },
        { "add_separator", new FunctionDescriptor ("add_separator", AddSeparator) },
        { "batch_format", new FunctionDescriptor ("batch_format", BatchFormat) },
        { "batch_read", new FunctionDescriptor ("batch_read", BatchRead) },
        { "batch_search", new FunctionDescriptor ("batch_search", BatchSearch) },
        { "batch_write", new FunctionDescriptor ("batch_write", BatchWrite) },
        { "book_author", new FunctionDescriptor ("book_author", BookAuthor) },
        { "book_authors", new FunctionDescriptor ("book_authors", BookAuthors) },
        { "book_classification", new FunctionDescriptor ("book_classification", BookClassification) },
        { "book_country", new FunctionDescriptor ("book_country", BookCountry) },
        { "book_exemplars", new FunctionDescriptor ("book_exemplars", BookExemplars) },
        { "book_holder", new FunctionDescriptor ("book_holder", BookHolder) },
        { "book_image", new FunctionDescriptor ("book_image", BookImage) },
        { "book_index", new FunctionDescriptor ("book_index", BookIndex) },
        { "book_info", new FunctionDescriptor ("book_info", BookInfo_) },
        { "book_isbn", new FunctionDescriptor ("book_isbn", BookIsbn) },
        { "book_issn", new FunctionDescriptor ("book_issn", BookIssn) },
        { "book_keywords", new FunctionDescriptor ("book_keywords", BookKeywords) },
        { "book_knowledge", new FunctionDescriptor ("book_knowledge", BookKnowledge) },
        { "book_language", new FunctionDescriptor ("book_language", BookLanguage) },
        { "book_price", new FunctionDescriptor ("book_price", BookPrice) },
        { "book_rental_count", new FunctionDescriptor ("book_rental_count", BookRentalCount) },
        { "book_type", new FunctionDescriptor ("book_type", BookType) },
        { "book_worksheet", new FunctionDescriptor ("book_price", BookWorksheet) },
        { "book_year", new FunctionDescriptor ("book_year", BookYear) },
        { "carriage_return", new FunctionDescriptor ("carriage_return", NewLine) },
        { "clear_output", new FunctionDescriptor ("clear_output", ClearOutput) },
        { "connect", new FunctionDescriptor ("connect", Connect) },
        { "create_connection", new FunctionDescriptor ("create_connection", CreateConnection) },
        { "create_counter", new FunctionDescriptor ("create_counter", CreateCounter) },
        { "create_database", new FunctionDescriptor ("create_database", CreateDatabase) },
        { "create_dictionary", new FunctionDescriptor ("create_dictionary", CreateDictionary) },
        { "create_record", new FunctionDescriptor ("create_record", CreateRecord) },
        { "delete_database", new FunctionDescriptor ("delete_database", DeleteDatabase) },
        { "database_info", new FunctionDescriptor ("database_info", DatabaseInfo) },
        { "database_stat", new FunctionDescriptor ("database_stat", DatabaseStat) },
        { "decrypt", new FunctionDescriptor ("decrypt", Decrypt) },
        { "disconnect", new FunctionDescriptor ("disconnect", Disconnect) },
        { "eat_whitespace", new FunctionDescriptor ("eat_whitespace", EatLastWhitespace) },
        { "encrypt", new FunctionDescriptor ("encrypt", Encrypt) },
        { "error_description", new FunctionDescriptor ("error_description", ErrorDescription) },
        { "flush_output", new FunctionDescriptor ("flush_output", FlushOutput) },
        { "fm", new FunctionDescriptor ("fm", FM) },
        { "fma", new FunctionDescriptor ("fma", FMA) },
        { "format_record", new FunctionDescriptor ("format_record", FormatRecord) },
        { "get_connection_string", new FunctionDescriptor ("get_connection_string", GetConnectionString) },
        { "get_exemplars", new FunctionDescriptor ("get_exemplars", GetExemplars) },
        { "get_field", new FunctionDescriptor ("get_field", GetField) },
        { "get_fields", new FunctionDescriptor ("get_fields", GetFields) },
        { "get_mark", new FunctionDescriptor ("get_mark", GetMark) },
        { "have_field", new FunctionDescriptor ("have_field", HaveField) },
        { "have_subfield", new FunctionDescriptor ("have_subfield", HaveSubfield) },
        { "is_article", new FunctionDescriptor ("is_article", IsArticle) },
        { "is_book", new FunctionDescriptor ("is_book", IsBook) },
        { "is_connected", new FunctionDescriptor ("is_connected", IsConnected) },
        { "is_electronic", new FunctionDescriptor ("is_electronic", IsElectronic) },
        { "is_issue", new FunctionDescriptor ("is_issue", IsIssue) },
        { "is_magazine", new FunctionDescriptor ("is_magazine", IsMagazine) },
        { "is_text", new FunctionDescriptor ("is_text", IsText) },
        { "list_files", new FunctionDescriptor ("list_processes", ListFiles) },
        { "list_processes", new FunctionDescriptor ("list_files", ListProcesses) },
        { "list_users", new FunctionDescriptor ("list_users", ListUsers) },
        { "load_record", new FunctionDescriptor ("load_record", LoadRecord) },
        { "max_mfn", new FunctionDescriptor ("max_mfn", MaxMfn) },
        { "normalize_text", new FunctionDescriptor ("normalize_text", NormalizeText) },
        { "parse_book", new FunctionDescriptor ("parse_book", ParseBook) },
        { "ping", new FunctionDescriptor ("ping", Ping) },
        { "protect", new FunctionDescriptor ("protect", Protect) },
        { "put_d", new FunctionDescriptor ("put_d", PutDelimited) },
        { "put_p", new FunctionDescriptor ("put_p", PutPrefix) },
        { "put_ps", new FunctionDescriptor ("put_ps", PutPrefixSuffix) },
        { "put_s", new FunctionDescriptor ("put_s", PrintSuffix) },
        { "put", new FunctionDescriptor ("put", Put) },
        { "read_record", new FunctionDescriptor ("read_record", ReadRecord) },
        { "read_terms", new FunctionDescriptor ("read_terms", ReadTerms) },
        { "read_text_file", new FunctionDescriptor ("read_text_file", ReadTextFile) },
        { "reader_id", new FunctionDescriptor ("reader_id", ReaderId) },
        { "reader_info", new FunctionDescriptor ("reader_info", ReaderInfo_) },
        { "reader_ticket", new FunctionDescriptor ("reader_ticket", ReaderTicket) },
        { "relax_utf8", new FunctionDescriptor ("relax_utf8", RelaxUtf8) },
        { "search", new FunctionDescriptor ("search", Search) },
        { "search_all", new FunctionDescriptor ("search_all", SearchAll) },
        { "search_count", new FunctionDescriptor ("search_count", SearchCount) },
        { "search_format", new FunctionDescriptor ("search_format", SearchFormat) },
        { "search_read", new FunctionDescriptor ("search_read", SearchRead) },
        { "search_read_one", new FunctionDescriptor ("search_read_one", SearchReadOneRecord) },
        { "server_stat", new FunctionDescriptor ("server_stat", ServerStat) },
        { "server_version", new FunctionDescriptor ("server_version", ServerVersion) },
        { "set_mark", new FunctionDescriptor ("set_mark", SetMark) },
        { "sort", new FunctionDescriptor ("sort", Sort) },
        { "sub_field", new FunctionDescriptor ("sub_field", GetSubfield) },
        { "truncate_database", new FunctionDescriptor ("truncate_database", TruncateDatabase) },
        { "unprotect", new FunctionDescriptor ("unprotect", Unprotect) },
        { "unlock_database", new FunctionDescriptor ("unlock_database", UnlockDatabase) },
        { "url_encode", new FunctionDescriptor ("url_encode", UrlEncode) },
        { "write_record", new FunctionDescriptor ("write_record", WriteRecord) },
    };

    #endregion

    #region Private members

    /// <summary>
    /// Ограничители, после которых нельзя ставить точку
    /// </summary>
    private static readonly char[] _delimiters = ['!', '?', '.', ','];

    /// <summary>
    /// Отыскиваем текущее подключение к серверу.
    /// Ругаемся, если не находим или находим что-то не то.
    /// </summary>
    internal static bool TryGetConnection
        (
            Context context,
            out SyncConnection connection,
            bool verbose = true
        )
    {
        connection = null!;

        if (!context.TryGetVariable (ConnectionDefineName, out var value))
        {
            if (verbose)
            {
                context.Error.WriteLine ($"Variable {ConnectionDefineName} not found");
            }

            return false;
        }

        if (value is SyncConnection syncConnection)
        {
            connection = syncConnection;
            return true;
        }

        if (verbose)
        {
            context.Error.WriteLine ($"Bad value of {ConnectionDefineName}: {value}");
        }

        return false;
    }

    /// <summary>
    /// Отыскиваем текущую запись.
    /// Ругаемся, если не находим или находим что-то не то.
    /// </summary>
    internal static bool TryGetRecord
        (
            Context context,
            out Record record,
            bool verbose = true
        )
    {
        record = null!;

        if (!context.TryGetVariable (RecordDefineName, out var value))
        {
            if (verbose)
            {
                context.Error.WriteLine ($"Variable {RecordDefineName} not found");
            }
            return false;
        }

        if (value is Record rec)
        {
            record = rec;
            return true;
        }

        if (verbose)
        {
            context.Error.WriteLine ($"Bad value of {RecordDefineName}: {value}");
        }

        return false;
    }

    /// <summary>
    /// Отыскиваем выходной буфер.
    /// Ругаемся, если не находим или находим что-то не то.
    /// </summary>
    internal static bool TryGetOutput
        (
            Context context,
            out OutputBuffer buffer
        )
    {
        buffer = null!;

        if (!context.TryGetVariable (OutputDefineName, out var value))
        {
            var newBuffer = new OutputBuffer();
            context.SetDefine (OutputDefineName, newBuffer);
            buffer = newBuffer;
            return true;
        }

        if (value is OutputBuffer outputBuffer)
        {
            buffer = outputBuffer;
            return true;
        }

        context.Error.WriteLine ($"Bad value of {OutputDefineName}: {value}");

        return false;
    }

    /// <summary>
    /// Вывод с поглощением лишних пробелов.
    /// </summary>
    private static void _Put
        (
            Context context,
            OutputBuffer output,
            object? value
        )
    {
        var lastChar = output.GetLastChar();

        if (char.IsWhiteSpace (lastChar))
        {
            var text = value?.ToString();
            if (!string.IsNullOrEmpty (text))
            {
                context.SetDefine (MarkDefineName, true);
                output.Write (text.TrimStart());
            }
        }
        else
        {
            context.SetDefine (MarkDefineName, true);
            output.Write (value);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Актуализация записей.
    /// </summary>
    public static dynamic? Actualize
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var parameters = new ActualizeRecordParameters();
        for (var i = 0; i < args.Length; i++)
        {
            var value = Compute (context, args, i);
            if (value is int mfn)
            {
                parameters.Mfn = mfn;
                connection.ActualizeRecord (parameters);
            }
        }

        return null;
    }

    /// <summary>
    /// Добавление точки.
    /// </summary>
    public static dynamic? AddDot
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            var lastChar = output.GetLastChar();
            if (Array.IndexOf (_delimiters, lastChar) < 0)
            {
                _Put (context, output, ". ");
            }
        }

        return null;
    }

    /// <summary>
    /// Добавление поля в текущую библиографическую запись.
    /// </summary>
    public static dynamic? AddField
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetRecord (context, out var record))
        {
            return null;
        }

        if (Compute (context, args, 0) is int tag and > 0
            && Compute (context, args, 1) is string value)
        {
            var result = new Field (tag, value);
            record.Add (result);

            return result;
        }

        return null;
    }

    /// <summary>
    /// Добавление разделителя областей описания.
    /// </summary>
    public static dynamic? AddSeparator
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            var lastChar = output.GetLastChar();
            if (lastChar != '-')
            {
                var needDot = Array.IndexOf (_delimiters, lastChar) < 0;
                _Put (context, output, needDot ? ". - " : " - ");
            }
        }

        return null;
    }

    /// <summary>
    /// Пакетное форматирование записей.
    /// </summary>
    public static dynamic? BatchFormat
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var database = connection.EnsureDatabase();
        var format = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (format))
        {
            return null;
        }

        var secondArg = Compute (context, args, 1);
        if (secondArg is IEnumerable<int> range)
        {
            return new BatchRecordFormatter (connection, database, format, 500, range);
        }

        if (secondArg is string expression
            && !string.IsNullOrEmpty (expression))
        {
            return BatchRecordFormatter.Search (connection, database, format, expression, 500);
        }

        if (secondArg is true)
        {
            return BatchRecordFormatter.WholeDatabase (connection, database, format, 500);
        }

        return null;
    }

    /// <summary>
    /// Пакетное чтение записей.
    /// </summary>
    public static dynamic? BatchRead
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var firstArg = Compute (context, args, 0);
        if (firstArg is IEnumerable<int> range)
        {
            return new BatchRecordReader (connection, range, database: connection.Database);
        }

        if (firstArg is true)
        {
            return BatchRecordReader.WholeDatabase (connection, connection.Database);
        }

        if (firstArg is string searchExpression)
        {
            var found = connection.SearchAll (searchExpression);
            return new BatchRecordReader (connection, found, database: connection.Database);
            // return BatchRecordReader.Search (connection, searchExpression, database: connection.Database);
        }

        if (firstArg is int fromMfn)
        {
            if (Compute (context, args, 1) is int toMfn)
            {
                return BatchRecordReader.Interval (connection, fromMfn, toMfn);
            }

            return BatchRecordReader.Interval (connection, fromMfn);
        }

        return null;
    }

    /// <summary>
    /// Пакетный поиск с загрузкой найденных записей.
    /// </summary>
    public static dynamic? BatchSearch
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var firstArg = Compute (context, args, 0);
        if (firstArg is string expression
            && !string.IsNullOrEmpty (expression))
        {
            return BatchRecordReader.Search (connection, expression, connection.Database);
        }

        if (firstArg is IEnumerable<int> range)
        {
            return new BatchRecordReader (connection, range, database: connection.Database);
        }

        if (firstArg is true)
        {
            return BatchRecordReader.WholeDatabase (connection, connection.Database);
        }

        return null;
    }

    /// <summary>
    /// Пакетное сохранение записей на сервере.
    /// </summary>
    public static dynamic? BatchWrite
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var database = connection.EnsureDatabase();
        var arg0 = Compute (context, args, 0);
        var arg1 = Compute (context, args, 1);
        if (arg0 is string database2
            && !string.IsNullOrEmpty (database2))
        {
            database = database2;
        }

        var capacity = 500;
        if (arg1 is int capacity2 and > 0)
        {
            capacity = capacity2;
        }

        if (arg0 is IEnumerable<Record> records)
        {
            using var writer = new BatchRecordWriter (connection, database, capacity);
            writer.AddRange (records);

            return null;
        }

        if (arg0 is BarsikList list)
        {
            var records2 = list.OfType<Record>().ToArray();
            using var writer2 = new BatchRecordWriter (connection, database, capacity);
            writer2.AddRange (records2);

            return null;
        }

        return new BatchRecordWriter (connection, database, capacity);
    }

    /// <summary>
    /// Получение массива ФИО авторов для указанной записи.
    /// </summary>
    public static dynamic? BookAuthor
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return ChimeraUtility.GetAuthor (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return ChimeraUtility.GetAuthors (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение массива ФИО авторов для указанной записи.
    /// </summary>
    public static dynamic? BookAuthors
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return ChimeraUtility.GetAuthors (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return ChimeraUtility.GetAuthors (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение классификационного индекса для указанной записи.
    /// </summary>
    public static dynamic? BookClassification
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetClassification (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetClassification (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение страны происхождения для указанной записи.
    /// </summary>
    public static dynamic? BookCountry
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetCountryCode (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetCountryCode (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение экземпляров для указанной записи.
    /// </summary>
    public static dynamic? BookExemplars
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetExemplars (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetExemplars (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение поля с держателем экземпляра для указанной записи.
    /// </summary>
    public static dynamic? BookHolder
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetHolderField (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetHolderField (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение поля с графическими данными для указанной записи.
    /// </summary>
    public static dynamic? BookImage
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetImageField (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetImageField (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение шифра документа для указанной записи.
    /// </summary>
    public static dynamic? BookIndex
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetIndex (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetIndex (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение всей информации о книге для указанной записи.
    /// </summary>
    public static dynamic? BookInfo_
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            if (TryGetConnection (context, out var connection))
            {
                return new BookInfo (connection, record);
            }
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return new BookInfo (connection, record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение информации об ISBN для указанной записи.
    /// </summary>
    public static dynamic? BookIsbn
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetIsbn (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetIsbn (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение информации об ISSN для указанной записи.
    /// </summary>
    public static dynamic? BookIssn
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetIssn (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetIssn (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение массива ключевых слов из указанной записи.
    /// </summary>
    public static dynamic? BookKeywords
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return record.FMA (610);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return record2.FMA (610);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение раздела знаний для указанной записи.
    /// </summary>
    public static dynamic? BookKnowledge
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetKnowledgeSection (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetKnowledgeSection (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение кода языка для указанной записи.
    /// </summary>
    public static dynamic? BookLanguage
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetLanguageCode (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetLanguageCode (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение цены экземпляра для указанной записи.
    /// </summary>
    public static dynamic? BookPrice
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetPrice (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetPrice (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение количества выдач для указанной записи.
    /// </summary>
    public static dynamic? BookRentalCount
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetRentalCount (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetRentalCount (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение типа документа для указанной записи.
    /// </summary>
    public static dynamic? BookType
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetDocumentType (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetDocumentType (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение рабочего листа для указанной записи.
    /// </summary>
    public static dynamic? BookWorksheet
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetWorksheet (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetWorksheet (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Получение года издания для указанной записи.
    /// </summary>
    public static dynamic? BookYear
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().GetYear (record);
        }

        if (firstArg is int mfn)
        {
            if (TryGetConnection (context, out var connection))
            {
                var record2 = connection.ReadRecord (mfn);
                if (record2 is not null)
                {
                    return RecordConfiguration.GetDefault().GetYear (record2);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Очистка вывовода.
    /// </summary>
    public static dynamic? ClearOutput
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            output.Clear();
        }

        return null;
    }

    /// <summary>
    /// Подключение к серверу.
    /// При необходимости создает подключение.
    /// </summary>
    public static dynamic Connect
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection, false))
        {
            // если соединения ещё нет, создаем его
            connection = (SyncConnection) _CreateConnection (context);
        }

        if (connection.IsConnected)
        {
            return true;
        }

        // TODO предусмотреть более сложную логику получения строки
        var connectionString = Compute (context, args, 0) as string
            ?? GetConnectionString (context, args) as string;
        if (!string.IsNullOrEmpty (connectionString))
        {
            connection.ParseConnectionString (connectionString);
        }

        if (!connection.Connect())
        {
            var errorMessage = IrbisException.GetErrorDescription (connection.LastError);
            context.Error.WriteLine ($"Can't connect: {errorMessage}");
            Magna.Logger.LogError ("Can't connect to {ConnectionString}", connectionString);

            return false;
        }

        Magna.Logger.LogTrace ("Connected to: {ConnectionString}", connectionString);

        return true;
    }

    private static dynamic _CreateConnection
        (
            Context context
        )
    {
        var result = ConnectionFactory.Shared.CreateSyncConnection();
        context.SetVariable (ConnectionDefineName, result);

        return result;
    }

    /// <summary>
    /// Создание синхронного подключения.
    /// </summary>
    public static dynamic CreateConnection
        (
            Context context,
            dynamic?[] args
        )
    {
        var result = (SyncConnection) _CreateConnection (context);
        if (Compute (context, args, 0) is string connectionString)
        {
            result.ParseConnectionString (connectionString);
        }

        return result;
    }

    /// <summary>
    /// Создание простого счетчика (костыль!).
    /// </summary>
    public static dynamic CreateCounter
        (
            Context context,
            dynamic?[] args
        )
    {
        return new DictionaryCounter<object, int>();
    }

    /// <summary>
    /// Создание базы данных.
    /// </summary>
    public static dynamic? CreateDatabase
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 1
            || !TryGetConnection (context, out var connection))
        {
            return null;
        }

        var parameters = new CreateDatabaseParameters();
        var databaseName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (databaseName))
        {
            return false;
        }

        parameters.Database = databaseName;
        parameters.Description = Compute (context, args, 1) as string;
        parameters.ReaderAccess = BarsikUtility.ToBoolean (Compute (context, args, 2));

        return connection.CreateDatabase (parameters);
    }

    /// <summary>
    /// Создание словаря.
    /// </summary>
    public static dynamic? CreateDictionary
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var databaseName = Compute (context, args, 0) as string;

        return connection.CreateDictionary (databaseName);
    }

    /// <summary>
    /// Создание новой библиографической записи.
    /// </summary>
    public static dynamic CreateRecord
        (
            Context context,
            dynamic?[] args
        )
    {
        var result = new Record();
        context.SetDefine (RecordDefineName, result);

        return result;
    }

    /// <summary>
    /// Информация о базе данных.
    /// </summary>
    public static dynamic? DatabaseInfo
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var databaseName = Compute (context, args, 0) as string;

        return connection.GetDatabaseInfo (databaseName);
    }

    /// <summary>
    /// Статистика по базе данных.
    /// </summary>
    public static dynamic? DatabaseStat
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        if (Compute (context, args, 0) is StatDefinition definition)
        {
            return connection.GetDatabaseStat (definition);
        }

        return null;
    }

    /// <summary>
    /// Раскодирование строки подключения либо пароля.
    /// </summary>
    public static dynamic? Decrypt
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is string { Length: > 0 } encrypted )
        {
            var password = Compute (context, args, 1) as string;
            return IrbisUtility.Decrypt (encrypted, password);
        }

        return null;
    }

    /// <summary>
    /// Удаление базы данных.
    /// </summary>
    public static dynamic? DeleteDatabase
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var databaseName = Compute (context, args, 0) as string;

        return connection.DeleteDatabase (databaseName);
    }

    /// <summary>
    /// Отключение от сервера.
    /// </summary>
    public static dynamic? Disconnect
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetConnection (context, out var connection))
        {
            connection.Disconnect();
            context.RemoveDefine (ConnectionDefineName);
        }

        return null;
    }

    /// <summary>
    /// Поедание конечных пробелов в выходном буфере.
    /// </summary>
    public static dynamic? EatLastWhitespace
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            output.EatLastWhitespace();
        }

        return null;
    }

    /// <summary>
    /// Шифрование строки подключения.
    /// </summary>
    public static dynamic? Encrypt
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is string { Length: > 0 } plainText)
        {
            var password = Compute (context, args, 1) as string;
            return IrbisUtility.Encrypt (plainText, password);
        }

        return null;
    }

    /// <summary>
    /// Описание ошибки по ее коду.
    /// </summary>
    public static dynamic? ErrorDescription
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return IrbisException.GetErrorDescription (connection.LastError);
    }

    /// <summary>
    /// Форматирование записи/записей.
    /// </summary>
    public static dynamic? FormatRecord
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        if (Compute (context, args, 0) is FormatRecordParameters parameters)
        {
            return connection.FormatRecords (parameters);
        }

        parameters = new FormatRecordParameters();
        for (var i = 0; i < args.Length; i++)
        {
            var value = Compute (context, args, i);
            if (value is int mfn)
            {
                parameters.Mfn = mfn;
            }
            else if (value is Record record)
            {
                parameters.Record = record;
            }
            else if (value is string format)
            {
                parameters.Format = format;
            }
        }

        if (connection.FormatRecords (parameters))
        {
            // TODO AsMany
            return parameters.Result.AsSingle();
        }

        return null;
    }

    /// <summary>
    /// Простой доступ к полям записи.
    /// </summary>
    public static dynamic? FlushOutput
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            // TODO очистка от двойных точек и разделителей подполей
            context.Output.Write (output.ToString());
            output.Clear();
        }

        return null;
    }

    /// <summary>
    /// Простой доступ к полям записи.
    /// </summary>
    public static dynamic? FM
        (
            Context context,
            dynamic?[] args
        )
    {
        Record? record;
        var firstArg = Compute (context, args, 0);
        var index = 1;

        if (firstArg is Record record2)
        {
            record = record2;
            firstArg = Compute (context, args, 1);
            index = 2;
        }
        else
        {
            if (!TryGetRecord (context, out record))
            {
                return null;
            }
        }

        var secondArg = Compute (context, args, index);
        if (firstArg is int tag)
        {
            if (secondArg is char code)
            {
                return record.FM (tag, code);
            }
            return record.FM (tag);
        }

        return null;
    }

    /// <summary>
    /// Простой доступ к полям записи.
    /// Можно вызывать так: <code>fma(910)</code>
    /// или так: <code>fma(910, 'a')</code>.
    /// </summary>
    public static dynamic? FMA
        (
            Context context,
            dynamic?[] args
        )
    {
        Record? record;
        var firstArg = Compute (context, args, 0);
        var index = 1;
        if (firstArg is Record record2)
        {
            record = record2;
            firstArg = Compute (context, args, 1);
            index = 2;
        }
        else
        {
            if (!TryGetRecord (context, out record))
            {
                return null;
            }
        }

        var secondArg = Compute (context, args, index);
        if (firstArg is int tag)
        {
            if (secondArg is char code)
            {
                return record.FMA (tag, code);
            }
            return record.FMA (tag);
        }

        return null;
    }

    /// <summary>
    /// Простой доступ к полям записи.
    /// </summary>
    public static dynamic GetConnectionString
        (
            Context context,
            dynamic?[] args
        )
    {
        var application = new IrbisApplication (Magna.Args);
        application.BuildConnectionSettings();

        return application.Settings.ToString();
    }

    /// <summary>
    /// Получение отметки о выводе в группе.
    /// </summary>
    public static dynamic? GetMark
        (
            Context context,
            dynamic?[] args
        )
    {
        context.TryGetVariable (MarkDefineName, out var result);

        return result;
    }

    /// <summary>
    /// Проверка наличия поля.
    /// </summary>
    public static dynamic HaveField
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetRecord (context, out var record))
        {
            return false;
        }

        for (var i = 0; i < args.Length; i++)
        {
            var value = Compute (context, args, i);
            if (value is int tag)
            {
                if (record.HaveField (tag))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Проверка наличия подполя.
    /// </summary>
    public static dynamic HaveSubfield
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is Field field)
        {
            for (var i = 1; i < args.Length; i++)
            {
                if (Compute (context, args, i) is char code
                    && field.HaveSubField (code))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Получение экземпляров для текущей или указанной записи.
    /// </summary>
    public static dynamic? GetExemplars
        (
            Context context,
            dynamic?[] args
        )
    {
        Record? record = null;
        if (Compute (context, args, 0) is Record record1)
        {
            record = record1;
        }
        else if (TryGetRecord (context, out Record record2))
        {
            record = record2;
        }

        if (record is null)
        {
            return null;
        }

        return ExemplarInfo.ParseRecord (record);
    }

    /// <summary>
    /// Получение одного (как правило, нулевого) повторения поля.
    /// </summary>
    public static dynamic? GetField
        (
            Context context,
            dynamic?[] args
        )
    {
        Record? record;
        var firstArg = Compute (context, args, 0);
        var index = 1;

        if (firstArg is Record record2)
        {
            record = record2;
            firstArg = Compute (context, args, 1);
            index = 2;
        }
        else
        {
            if (!TryGetRecord (context, out record))
            {
                return null;
            }
        }

        if (firstArg is int tag)
        {
            var secondArg = Compute (context, args, index);
            if (secondArg is int occurrence)
            {
                return record.GetField (tag, occurrence);
            }

            return record.GetField (tag);
        }

        return null;
    }

    /// <summary>
    /// Получение всех повторений перечисленных полей.
    /// </summary>
    public static dynamic GetFields
        (
            Context context,
            dynamic?[] args
        )
    {
        Record? record;
        var firstArg = Compute (context, args, 0);
        var index = 1;

        if (firstArg is Record record2)
        {
            record = record2;
        }
        else
        {
            if (!TryGetRecord (context, out record))
            {
                return Array.Empty<Field>();
            }
        }

        var result = new BarsikList();
        for (var i = index; i < args.Length; i++)
        {
            var value = Compute (context, args, i);
            if (value is int tag)
            {
                result.AddRange (record.Fields.GetField (tag));
            }
        }

        return result;
    }

    /// <summary>
    /// Получение указанного подполя.
    /// </summary>
    public static dynamic? GetSubfield
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is Field field)
        {
            for (var i = 1; i < args.Length; i++)
            {
                if (Compute (context, args, i) is char code)
                {
                    var result = field.GetFirstSubFieldValue (code);

                    if (!string.IsNullOrEmpty (result))
                    {
                        return result;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Проверка, представляет ли запись статью из журнала.
    /// </summary>
    public static dynamic IsArticle
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().IsArticle (record);
        }

        return false;
    }

    /// <summary>
    /// Проверка, представляет ли запись книгу.
    /// </summary>
    public static dynamic IsBook
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().IsBook (record);
        }

        return false;
    }

    /// <summary>
    /// Мы подключены к серверу.
    /// </summary>
    public static dynamic IsConnected
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return false;
        }

        return connection.IsConnected;
    }

    /// <summary>
    /// Проверка, представляет ли запись электронный документ.
    /// </summary>
    public static dynamic IsElectronic
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().IsElectronic (record);
        }

        return false;
    }

    /// <summary>
    /// Проверка, представляет ли запись выпуск журнала.
    /// </summary>
    public static dynamic IsIssue
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().IsMagazineIssue (record);
        }

        return false;
    }

    /// <summary>
    /// Проверка, представляет ли запись сводную по журналу.
    /// </summary>
    public static dynamic IsMagazine
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().IsMagazineSummary (record);
        }

        return false;
    }

    /// <summary>
    /// Проверка, представляет ли запись текстовый документ.
    /// </summary>
    public static dynamic IsText
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return RecordConfiguration.GetDefault().IsText (record);
        }

        return false;
    }

    /// <summary>
    /// Получение списка файлов.
    /// </summary>
    public static dynamic? ListFiles
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var specifications = new List<FileSpecification>();
        for (var i = 0; i < args.Length; i++)
        {
            var item = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (item))
            {
                var spec = FileSpecification.Parse (item);
                specifications.Add (spec);
            }
        }

        if (!specifications.IsNullOrEmpty())
        {
            return connection.ListFiles (specifications.ToArray());
        }

        return null;
    }

    /// <summary>
    /// Получение списка процессов.
    /// </summary>
    public static dynamic? ListProcesses
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.ListProcesses();
    }

    /// <summary>
    /// Получение списка пользователей системы.
    /// </summary>
    public static dynamic? ListUsers
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.ListUsers();
    }

    /// <summary>
    /// Чтение записи.
    /// Запись НЕ помещается в <code>record</code>.
    /// </summary>
    public static dynamic? LoadRecord
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var mfnList = new List<int>();
        for (var i = 0; i < args.Length; i++)
        {
            var value = Compute (context, args, i);
            if (value is int mfn)
            {
                mfnList.Add (mfn);
            }
        }

        if (mfnList.Count == 1)
        {
            var record = connection.ReadRecord (mfnList[0]);
            return record;
        }

        if (mfnList.Count > 1)
        {
            var result = connection.ReadRecords (connection.Database!, mfnList);
            if (!result.IsNullOrEmpty())
            {
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// Получение максимального MFN.
    /// </summary>
    public static dynamic? MaxMfn
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var databaseName = Compute (context, args, 0) as string;

        return connection.GetMaxMfn (databaseName);
    }

    /// <summary>
    /// Переход на новую строку.
    /// </summary>
    public static dynamic? NewLine
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            output.EatLastWhitespace();
            output.WriteLine();
        }

        return null;
    }

    /// <summary>
    /// Нормализация текста: убираем лишние пробелы.
    /// Первый символ прописной, остальные - строчные.
    /// </summary>
    public static dynamic? NormalizeText
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var result = new Sparcer().SparceText (text);
        if (!string.IsNullOrEmpty (result))
        {
            Span<char> span = stackalloc char[result.Length];
            span[0] = char.ToUpperInvariant (result[0]);
            for (var i = 1; i < result.Length; i++)
            {
                span[i] = char.ToLowerInvariant (result[i]);
            }

            result = span.ToString();
        }

        return result;
    }

    /// <summary>
    /// Разбор книги по косточкам.
    /// </summary>
    public static dynamic? ParseBook
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        Record? record = null;
        if (Compute (context, args, 0) is Record record2)
        {
            record = record2;
        }
        else if (TryGetRecord (context, out var record3))
        {
            record = record3;
        }

        if (record is null)
        {
            return null;
        }

        return new BookInfo (connection, record);
    }

    /// <summary>
    /// Проверка подключения к серверу.
    /// Напоминание серверу о себе.
    /// </summary>
    public static dynamic Ping
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return false;
        }

        if (!connection.IsConnected)
        {
            return false;
        }

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        if (connection.NoOperation())
        {
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        return false;
    }

    /// <summary>
    /// Простой вывод на печать (проще, чем <code>print</code>).
    /// Отслеживает точки и пробелы.
    /// </summary>
    public static dynamic? Put
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetOutput (context, out var output))
        {
            return null;
        }

        for (var i = 0; i < args.Length; i++)
        {
            var value = Compute (context, args, i);
            if (value is not null)
            {
                _Put(context, output, value);
            }
        }

        return null;
    }

    /// <summary>
    /// Вывод на печать с разделителями.
    /// </summary>
    public static dynamic PutDelimited
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            var delimeter = Compute (context, args, 0) as string;
            var first = true;
            for (var i = 1; i < args.Length; i++)
            {
                var value = Compute (context, args, i);
                if (BarsikUtility.ToBoolean (value))
                {
                    if (!first)
                    {
                        output.Write (delimeter);
                    }

                    output.Write (value);
                    first = false;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Вывод на печать с префиксом.
    /// </summary>
    public static dynamic PutPrefix
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            var firstArg = Compute (context, args, 0);
            if (BarsikUtility.ToBoolean (firstArg))
            {
                var secondArg = Compute (context, args, 1);
                _Put (context, output, secondArg);
                _Put (context, output, firstArg);

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Вывод на печать с префиксом и суффиксом.
    /// </summary>
    public static dynamic PutPrefixSuffix
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            var firstArg = Compute (context, args, 0);
            if (BarsikUtility.ToBoolean (firstArg))
            {
                var secondArg = Compute (context, args, 1);
                _Put (context, output, secondArg);

                _Put (context, output, firstArg);

                var thirdArg = Compute (context, args, 2);
                _Put (context, output, thirdArg);

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Вывод на печать с суффиксом.
    /// </summary>
    public static dynamic PrintSuffix
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetOutput (context, out var output))
        {
            var firstArg = Compute (context, args, 0);
            if (BarsikUtility.ToBoolean (firstArg))
            {
                _Put (context, output, firstArg);

                var secondArg = Compute (context, args, 1);
                _Put (context, output, secondArg);

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Простейшая защита строки подключения.
    /// </summary>
    public static dynamic? Protect
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is string { Length: > 0 } plainText)
        {
            return ConfigurationUtility.Protect (plainText);
        }

        return Compute (context, args, 0);
    }

    /// <summary>
    /// Чтение записи.
    /// Запись помещается в <code>record</code>.
    /// </summary>
    public static dynamic? ReadRecord
        (
            Context context,
            dynamic?[] args
        )
    {
        context.SetDefine (RecordDefineName, null);

        var result = LoadRecord (context, args);
        if (result is Record record)
        {
            context.SetDefine (RecordDefineName, record);
        }
        else if (result is IList list)
        {
            if (list.Count != 0)
            {
                context.SetDefine (RecordDefineName, list[0]);
            }
        }

        return null;
    }

    /// <summary>
    /// Чтение терминов поискового словаря.
    /// </summary>
    public static dynamic? ReadTerms
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 1)
        {
            return null;
        }

        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var firstArg = Compute (context, args, 0);
        var parameters = firstArg as TermParameters;
        if (parameters is null)
        {
            parameters = new TermParameters();
            var startTerm = Compute (context, args, 0) as string;
            if (startTerm is null)
            {
                return null;
            }

            parameters.StartTerm = startTerm;
            var secondArg = Compute (context, args, 1);
            if (secondArg is int number)
            {
                parameters.NumberOfTerms = number;
            }
        }

        return connection.ReadTerms (parameters);
    }

    /// <summary>
    /// Чтение текстового файла.
    /// </summary>
    public static dynamic? ReadTextFile
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var specifications = new List<FileSpecification>();
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                specifications.Add (FileSpecification.Parse (name.Trim()));
            }
        }

        if (!specifications.IsNullOrEmpty())
        {
            return connection.ReadTextFiles (specifications.ToArray());
        }

        return null;
    }

    /// <summary>
    /// Получение идентификатора читателя.
    /// </summary>
    public static dynamic? ReaderId
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return ReaderConfiguration.GetDefault().GetReaderId (record);
        }

        return null;
    }

    /// <summary>
    /// Разбор записи, содержащей информацию о читателе.
    /// </summary>
    public static dynamic? ReaderInfo_
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return ReaderInfo.Parse (record);
        }

        return null;
    }

    /// <summary>
    /// Получение номера читательского билета.
    /// </summary>
    public static dynamic? ReaderTicket
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        if (firstArg is Record record)
        {
            return ReaderConfiguration.GetDefault().GetTicket (record);
        }

        return null;
    }

    /// <summary>
    /// Расслабляем UTF-8, чтобы не бросалась исключениями.
    /// </summary>
    public static dynamic? RelaxUtf8
        (
            Context context,
            dynamic?[] args
        )
    {
        IrbisEncoding.RelaxUtf8();

        return null;
    }

    /// <summary>
    /// Поиск по словарю.
    /// </summary>
    public static dynamic? Search
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var firstArg = Compute (context, args, 0);
        if (firstArg is SearchParameters parameters)
        {
            return connection.Search (parameters);
        }

        var expression = firstArg as string;
        if (!string.IsNullOrEmpty (expression))
        {
            return connection.Search (expression);
        }

        return null;
    }

    /// <summary>
    /// Поиск по словарю - выдача всех записей.
    /// </summary>
    public static dynamic? SearchAll
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var firstArg = Compute (context, args, 0);
        if (firstArg is SearchParameters parameters)
        {
            return connection.Search (parameters);
        }

        var expression = firstArg as string;
        if (!string.IsNullOrEmpty (expression))
        {
            return connection.SearchAll (expression);
        }

        return null;
    }

    /// <summary>
    /// Поиск по словарю с выдачей количества найденных записей.
    /// </summary>
    public static dynamic? SearchCount
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        if (Compute (context, args, 0) is string expression
            && !string.IsNullOrEmpty (expression))
        {
            return connection.SearchCount (expression);
        }

        return null;
    }

    /// <summary>
    /// Поиск по словарю с последующим расформатированием записей.
    /// </summary>
    public static dynamic? SearchFormat
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        if (Compute (context, args, 0) is string expression
            && !string.IsNullOrEmpty (expression)
            && Compute (context, args, 1) is string format
            && !string.IsNullOrEmpty (format))
        {
            var parameters = new SearchParameters()
            {
                Expression = expression,
                Format = format
            };
            var result = connection.Search (parameters);
            if (result is not null)
            {
                return FoundItem.ToText (result);
            }
        }

        return null;
    }

    /// <summary>
    /// Поиск по словарю с последующей загрузкой записей.
    /// </summary>
    public static dynamic? SearchRead
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        if (Compute (context, args, 0) is string expression
            && !string.IsNullOrEmpty (expression))
        {
            return connection.SearchRead (expression);
        }

        return null;
    }

    /// <summary>
    /// Поиск по словарю с последующей загрузкой записей (именно одной!).
    /// Запись НЕ помещается в <code>record</code>.
    /// </summary>
    public static dynamic? SearchReadOneRecord
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        if (Compute (context, args, 0) is string expression
            && !string.IsNullOrEmpty (expression))
        {
            return connection.SearchReadOneRecord (expression);
        }

        return null;
    }

    /// <summary>
    /// Статистика сервера.
    /// </summary>
    public static dynamic? ServerStat
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.GetServerStat();
    }

    /// <summary>
    /// Версия сервера.
    /// </summary>
    public static dynamic? ServerVersion
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return connection.GetServerVersion();
    }

    /// <summary>
    /// Установка метки о начале вывода в группе.
    /// </summary>
    public static dynamic? SetMark
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = BarsikUtility.ToBoolean (Compute (context, args, 0));
        context.SetDefine (MarkDefineName, value);

        return null;
    }

    /// <summary>
    /// Сортировка.
    /// </summary>
    public static dynamic? Sort
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);

        if (firstArg is Array array1)
        {
            Array.Sort (array1);
            return array1;
        }

        if (firstArg is IEnumerable enumerable)
        {
            var array2 = enumerable.Cast<object>().ToArray();
            Array.Sort (array2);
            return array2;
        }

        return firstArg;
    }

    /// <summary>
    /// Опустошение базы данных.
    /// </summary>
    public static dynamic? TruncateDatabase
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var databaseName = Compute (context, args, 0) as string;

        return connection.TruncateDatabase (databaseName);
    }

    /// <summary>
    /// Разблокирование базы данных.
    /// </summary>
    public static dynamic? UnlockDatabase
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        var databaseName = Compute (context, args, 0) as string;

        return connection.UnlockDatabase (databaseName);
    }

    /// <summary>
    /// Снятие защиты со строки подключения.
    /// </summary>
    public static dynamic? Unprotect
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is string { Length: > 0 } protectedText)
        {
            return ConfigurationUtility.Unprotect (protectedText);
        }

        return Compute (context, args, 0);
    }

    /// <summary>
    /// Кодирование текста как URL.
    /// </summary>
    public static dynamic? UrlEncode
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = Compute (context, args, 0) as string;
        if (!string.IsNullOrEmpty (text))
        {
            return Utility.UrlEncode (text, IrbisEncoding.Utf8);
        }

        return text;
    }

    /// <summary>
    /// Сохранение записи.
    /// </summary>
    public static dynamic? WriteRecord
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        if (Compute (context, args, 0) is Record record)
        {
            return connection.WriteRecord (record);
        }

        return false;
    }

    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "IrbisLib";

    /// <inheritdoc cref="IBarsikModule.Version"/>
    public Version Version { get; } = new (1, 0);

    /// <inheritdoc cref="IBarsikModule.AttachModule"/>
    public bool AttachModule
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        var context = interpreter.Context.ThrowIfNull();
        foreach (var descriptor in Registry)
        {
            context.Functions[descriptor.Key] = descriptor.Value;
        }

        var assembly = typeof (IrbisLib).Assembly;
        StdLib.LoadAssembly (context, new dynamic?[] { assembly.GetName().Name });
        StdLib.Use (context, new dynamic?[] { "ManagedIrbis" });
        interpreter.ExternalCodeHandler = LiteHandler.ExternalCodeHandler;

        return true;
    }

    /// <inheritdoc cref="IBarsikModule.DetachModule"/>
    public void DetachModule
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        var context = interpreter.Context.ThrowIfNull();
        interpreter.ExternalCodeHandler = null;
        foreach (var descriptor in Registry)
        {
            context.Functions.Remove (descriptor.Key);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Description;
    }

    #endregion
}
