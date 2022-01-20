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
using System.Collections.Generic;
using System.Diagnostics;

using AM;
using AM.Collections;
using AM.Scripting.Barsik;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.PftLite;
using ManagedIrbis.Providers;

using static AM.Scripting.Barsik.Builtins;

#endregion

#nullable enable

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

    #endregion

    #region Properties

    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "actualize", new FunctionDescriptor ("actualize", Actualize) },
        { "add_dot", new FunctionDescriptor ("add_dot", AddDot) },
        { "add_separator", new FunctionDescriptor ("add_separator", AddSeparator) },
        { "clear_output", new FunctionDescriptor ("clear_output", ClearOutput) },
        { "connect", new FunctionDescriptor ("connect", Connect) },
        { "create_connection", new FunctionDescriptor ("create_connection", CreateConnection) },
        { "create_database", new FunctionDescriptor ("create_database", CreateDatabase) },
        { "create_dictionary", new FunctionDescriptor ("create_dictionary", CreateDictionary) },
        { "delete_database", new FunctionDescriptor ("delete_database", DeleteDatabase) },
        { "database_info", new FunctionDescriptor ("database_info", DatabaseInfo) },
        { "database_stat", new FunctionDescriptor ("database_stat", Disconnect) },
        { "disconnect", new FunctionDescriptor ("disconnect", Disconnect) },
        { "eat_whitespace", new FunctionDescriptor ("eat_whitespace", EatLastWhitespace) },
        { "error_description", new FunctionDescriptor ("error_description", ErrorDescription) },
        { "flush_output", new FunctionDescriptor ("flush_output", FlushOutput) },
        { "fm", new FunctionDescriptor ("fm", FM) },
        { "fma", new FunctionDescriptor ("fma", FMA) },
        { "format_record", new FunctionDescriptor ("format_record", FormatRecord) },
        { "get_connection_string", new FunctionDescriptor ("get_connection_string", GetConnectionString) },
        { "get_field", new FunctionDescriptor ("get_field", GetField) },
        { "get_fields", new FunctionDescriptor ("get_fields", GetFields) },
        { "have_field", new FunctionDescriptor ("have_field", HaveField) },
        { "have_subfield", new FunctionDescriptor ("have_subfield", HaveSubfield) },
        { "is_connected", new FunctionDescriptor ("is_connected", IsConnected) },
        { "list_files", new FunctionDescriptor ("list_processes", ListFiles) },
        { "list_processes", new FunctionDescriptor ("list_files", ListProcesses) },
        { "list_users", new FunctionDescriptor ("list_users", ListUsers) },
        { "max_mfn", new FunctionDescriptor ("max_mfn", MaxMfn) },
        { "new_line", new FunctionDescriptor ("new_line", NewLine) },
        { "ping", new FunctionDescriptor ("ping", Ping) },
        { "put_p", new FunctionDescriptor ("put_p", PutPrefix) },
        { "put_ps", new FunctionDescriptor ("put_ps", PutPrefixSuffix) },
        { "put_s", new FunctionDescriptor ("put_s", PrintSuffix) },
        { "put", new FunctionDescriptor ("put", Out) },
        { "read_record", new FunctionDescriptor ("read_record", ReadRecord) },
        { "read_terms", new FunctionDescriptor ("read_terms", ReadTerms) },
        { "read_text_file", new FunctionDescriptor ("read_text_file", ReadTextFile) },
        { "search", new FunctionDescriptor ("search", Search) },
        { "search_count", new FunctionDescriptor ("search_count", SearchCount) },
        { "search_format", new FunctionDescriptor ("search_format", SearchFormat) },
        { "search_read", new FunctionDescriptor ("search_read", SearchRead) },
        { "server_stat", new FunctionDescriptor ("server_stat", ServerStat) },
        { "server_version", new FunctionDescriptor ("server_version", ServerVersion) },
        { "sub_field", new FunctionDescriptor ("sub_field", GetSubfield) },
        { "truncate_database", new FunctionDescriptor ("truncate_database", TruncateDatabase) },
        { "unlock_database", new FunctionDescriptor ("unlock_database", UnlockDatabase) },
        { "write_record", new FunctionDescriptor ("write_record", WriteRecord) },
    };

    #endregion

    #region Private members

    /// <summary>
    /// Ограничители, после которых нельзя ставить точку
    /// </summary>
    private static readonly char[] _delimiters = { '!', '?', '.', ',' };

    /// <summary>
    /// Отыскиваем текущее подключение к серверу.
    /// Ругаемся, если не находим или находим что-то не то.
    /// </summary>
    private static bool TryGetConnection
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
                output.Write (". ");
            }
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
                output.Write (lastChar == '.' ? " - " : ". - ");
            }
        }

        return null;
    }

    /// <summary>
    /// Добавление разделителя областей описания.
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
            connection = ConnectionFactory.Shared.CreateSyncConnection();
            context.SetVariable (ConnectionDefineName, connection);
        }

        if (connection.Connected)
        {
            return true;
        }

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
            Magna.Error ($"Can't connect to '{connectionString}': {errorMessage}");

            return false;
        }

        Magna.Trace ($"Connected to: {connectionString}");

        return true;
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
        var connectionString = Compute (context, args, 0) as string;
        var result = ConnectionFactory.Shared.CreateSyncConnection();
        if (!string.IsNullOrEmpty (connectionString))
        {
            result.ParseConnectionString (connectionString);
        }

        context.SetVariable (ConnectionDefineName, result);

        return result;
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
    /// Форматирование записи.
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
        if (!TryGetRecord (context, out var record))
        {
            return null;
        }

        var firstArg = Compute (context, args, 0);
        var secondArg = Compute (context, args, 1);
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
    /// </summary>
    public static dynamic? FMA
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetRecord (context, out var record))
        {
            return null;
        }

        var firstArg = Compute (context, args, 0);
        var secondArg = Compute (context, args, 1);
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
    public static dynamic? GetConnectionString
        (
            Context context,
            dynamic?[] args
        )
    {
        return ConnectionUtility.GetConfiguredConnectionString (Magna.Configuration)
            ?? ConnectionUtility.GetStandardConnectionString();
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
    /// Получение одного (как правило, нулевого) повторения поля.
    /// </summary>
    public static dynamic? GetField
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetRecord (context, out var record))
        {
            return null;
        }

        var firstArg = Compute (context, args, 0);
        if (firstArg is int tag)
        {
            var secondArg = Compute (context, args, 1);
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
        if (!TryGetRecord (context, out var record))
        {
            return Array.Empty<Field>();
        }

        var result = new BarsikList();
        for (var i = 0; i < args.Length; i++)
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
        if (Compute (context, args, 0) is Field field
            && Compute (context, args, 1) is char code)
        {
            return field.GetFirstSubFieldValue (code);
        }

        return null;
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

        return connection.Connected;
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
    /// Простой вывод на печать (проще, чем <code>print</code>).
    /// Отслеживает точки и пробелы.
    /// </summary>
    public static dynamic? Out
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
                output.Write (value);
            }
        }

        return null;
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

        if (!connection.Connected)
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
                BarsikUtility.PrintObject (output.Writer, secondArg);
                BarsikUtility.PrintObject (output.Writer, firstArg);

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
                BarsikUtility.PrintObject (output.Writer, secondArg);

                BarsikUtility.PrintObject (output.Writer, firstArg);

                var thirdArg = Compute (context, args, 2);
                BarsikUtility.PrintObject (output.Writer, thirdArg);

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
                BarsikUtility.PrintObject (output.Writer, firstArg);

                var secondArg = Compute (context, args, 1);
                BarsikUtility.PrintObject (output.Writer, secondArg);

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Чтение записи.
    /// </summary>
    public static dynamic? ReadRecord
        (
            Context context,
            dynamic?[] args
        )
    {
        context.SetDefine (RecordDefineName, null);

        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        if (Compute (context, args, 0) is SearchParameters parameters)
        {
            return connection.Search (parameters);
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
            context.SetDefine (RecordDefineName, record);
            return record;
        }

        if (mfnList.Count > 1)
        {
            var result = connection.ReadRecords (connection.Database!, mfnList);
            if (!result.IsNullOrEmpty())
            {
                context.SetDefine (RecordDefineName, result[0]);
                return result;
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
