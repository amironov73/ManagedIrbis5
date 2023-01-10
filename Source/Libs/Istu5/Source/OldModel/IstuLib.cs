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

/* IstuLib.cs -- библиотека функций для старой книговыдачи ИРНИТУ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Scripting.Barsik;

using static AM.Scripting.Barsik.Builtins;

#endregion

#nullable enable

namespace Istu.OldModel;

/// <summary>
/// Библиотека функций для старой книговыдачи ИРНИТУ.
/// </summary>
public sealed class IstuLib
    : IBarsikModule
{
    #region Constants

    /// <summary>
    /// Имя дефайна, хранящего текущее подключение к базе данных MSSQL.
    /// </summary>
    public const string StorehouseDefineName = "storehouse";

    #endregion

    #region Properties

    // TODO добавить работу с читателями
    // parse_reader

    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "get_storehouse", new FunctionDescriptor ("get_storehouse", GetStorehouse) },
        { "podsob_by_ticket", new FunctionDescriptor ("podsob_by_ticket", PodsobByTicket) },
        { "reader_by_email", new FunctionDescriptor ("reader_by_email", ReaderByEmail) },
        { "reader_by_telegram", new FunctionDescriptor ("reader_by_telegram", ReaderByTelegramId) },
    };

    #endregion

    #region Private members

    /// <summary>
    /// Отыскиваем текущее подключение к серверу.
    /// Ругаемся, если не находим или находим что-то не то.
    /// </summary>
    private static bool TryGetStorehouse
        (
            Context context,
            out Storehouse storehouse,
            bool verbose = true
        )
    {
        storehouse = null!;

        if (!context.TryGetVariable (StorehouseDefineName, out var value))
        {
            if (verbose)
            {
                context.Error.WriteLine ($"Variable {StorehouseDefineName} not found");
            }
            return false;
        }

        if (value is Storehouse storehouse2)
        {
            storehouse = storehouse2;
            return true;
        }

        if (verbose)
        {
            context.Error.WriteLine ($"Bad value of {StorehouseDefineName}: {value}");
        }

        return false;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление разделителя областей описания.
    /// </summary>
    public static dynamic GetStorehouse
        (
            Context context,
            dynamic?[] args
        )
    {
        if (TryGetStorehouse (context, out var storehouse))
        {
            return storehouse;
        }

        var connectionString = Compute (context, args, 0) as string;
        storehouse = Storehouse.GetInstance (connectionString);
        context.SetDefine (StorehouseDefineName, storehouse);

        return storehouse;
    }

    /// <summary>
    /// Отбор книг по читательскому билету.
    /// </summary>
    public static dynamic? PodsobByTicket
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetStorehouse (context, out var storehouse))
        {
            return null;
        }

        var ticket = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (ticket))
        {
            return null;
        }

        var kladovka = storehouse.GetKladovka();
        var podsob = kladovka.GetPodsob();
        var found = podsob
            .Where (book => book.Ticket == ticket)
            .ToArray();

        return found;
    }

    /// <summary>
    /// Поиск читателя по e-mail.
    /// </summary>
    public static dynamic? ReaderByEmail
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetStorehouse (context, out var storehouse))
        {
            return null;
        }

        var email = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (email))
        {
            return null;
        }

        var readerManager = storehouse.CreateReaderManager();

        return readerManager.GetReaderByEmail (email);
    }

    /// <summary>
    /// Поиск читателя по e-mail.
    /// </summary>
    public static dynamic? ReaderByTelegramId
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!TryGetStorehouse (context, out var storehouse))
        {
            return null;
        }

        if (Compute (context, args, 0) is long id and > 0)
        {
            var readerManager = storehouse.CreateReaderManager();

            return readerManager.GetReaderByTelegramId (id);
        }

        return null;
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

        var assembly = typeof (IstuLib).Assembly;
        StdLib.LoadAssembly (context, new dynamic?[] { assembly.GetName().Name });
        StdLib.Use (context, new dynamic?[] { "Istu.OldModel" });

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
