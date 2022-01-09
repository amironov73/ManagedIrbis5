// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
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
using AM.Scripting.Barsik;

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
    /// Имя переменной, хранящей текущее подключение.
    /// </summary>
    public const string ConnectionVariableName = "connection";

    #endregion

    #region Properties

    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "actualize", new FunctionDescriptor ("actualize", Actualize) },
        { "connect", new FunctionDescriptor ("connect", Connect) },
        { "create_connection", new FunctionDescriptor ("create_connection", CreateConnection) },
        { "create_database", new FunctionDescriptor ("create_database", CreateDatabase) },
        { "create_dictionary", new FunctionDescriptor ("create_dictionary", CreateDictionary) },
        { "delete_database", new FunctionDescriptor ("delete_database", DeleteDatabase) },
        { "database_info", new FunctionDescriptor ("database_info", DatabaseInfo) },
        { "database_stat", new FunctionDescriptor ("database_stat", Disconnect) },
        { "disconnect", new FunctionDescriptor ("disconnect", Disconnect) },
        { "format_record", new FunctionDescriptor ("format_record", FormatRecord) },
        { "list_files", new FunctionDescriptor ("list_processes", ListProcesses) },
        { "list_processes", new FunctionDescriptor ("list_files", ListFiles) },
        { "list_users", new FunctionDescriptor ("list_users", ListUsers) },
        { "max_mfn", new FunctionDescriptor ("max_mfn", MaxMfn) },
        { "ping", new FunctionDescriptor ("ping", Ping) },
        { "read_record", new FunctionDescriptor ("read_record", ReadRecord) },
        { "read_terms", new FunctionDescriptor ("read_terms", ReadTerms) },
        { "read_text_file", new FunctionDescriptor ("read_text_file", ReadTextFile) },
        { "search", new FunctionDescriptor ("search", Search) },
        { "server_stat", new FunctionDescriptor ("server_stat", ServerStat) },
        { "server_version", new FunctionDescriptor ("server_version", ServerVersion) },
        { "truncate_database", new FunctionDescriptor ("truncate_database", TruncateDatabase) },
        { "unlock_database", new FunctionDescriptor ("unlock_database", UnlockDatabase) },
        { "write_record", new FunctionDescriptor ("write_record", WriteRecord) },
    };

    #endregion

    #region Private members

    private static bool TryGetConnection
        (
            Context context,
            out SyncConnection connection
        )
    {
        connection = null!;

        if (!context.TryGetVariable (ConnectionVariableName, out var value))
        {
            context.Error.WriteLine ($"Variable {ConnectionVariableName} not found");
            return false;
        }

        if (value is SyncConnection syncConnection)
        {
            connection = syncConnection;
            return true;
        }

        context.Error.WriteLine ($"Bad value of {ConnectionVariableName}: {value}");

        return false;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Актуализация.
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
        if (!TryGetConnection (context, out var connection))
        {
            connection = ConnectionFactory.Shared.CreateSyncConnection();
            context.SetVariable (ConnectionVariableName, connection);
        }

        if (connection.Connected)
        {
            return true;
        }

        var connectionString = Compute (context, args, 0) as string;
        if (!string.IsNullOrEmpty (connectionString))
        {
            connection.ParseConnectionString (connectionString);
        }

        if (!connection.Connect())
        {
            var errorMessage = IrbisException.GetErrorDescription (connection.LastError);
            context.Error.WriteLine ($"Can't connect: {errorMessage}");

            return false;
        }

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

        context.SetVariable (ConnectionVariableName, result);

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
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return null;
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

        return null;
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

        return null;
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

        return null;
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
        if (!TryGetConnection (context, out var connection))
        {
            connection.Disconnect();
            context.RemoveVariable (ConnectionVariableName);
        }

        return null;
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

        return null;
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
    /// Чтение записи.
    /// </summary>
    public static dynamic? ReadRecord
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        // TODO implement

        return new Record();
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
        if (!TryGetConnection (context, out var connection))
        {
            return null;
        }

        return null;
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

        // TODO implement

        return new ServerStat();
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

        // TODO implement

        return new ServerVersion();
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

        return null;
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

        return null;
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

        // TODO implement

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
            Context context
        )
    {
        Sure.NotNull (context);

        foreach (var descriptor in Registry)
        {
            context.Functions[descriptor.Key] = descriptor.Value;
        }

        return true;
    }

    /// <inheritdoc cref="IBarsikModule.DetachModule"/>
    public void DetachModule
        (
            Context context
        )
    {
        Sure.NotNull (context);

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
