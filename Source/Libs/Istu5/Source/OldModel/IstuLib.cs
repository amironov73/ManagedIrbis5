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
        { "get_storehouse", new FunctionDescriptor ("get_storehouse", GetStorehouse) },
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

        storehouse = Storehouse.GetInstance();
        context.SetDefine (StorehouseDefineName, storehouse);

        return storehouse;
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
