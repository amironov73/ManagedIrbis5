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

/* GblLib.cs -- библиотека функций для ИРБИС для глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Scripting.Barsik;

using ManagedIrbis.PftLite;

#endregion

namespace ManagedIrbis.Scripting.Barsik;

/// <summary>
/// Библиотека функций для ИРБИС для глобальной корректировки.
/// </summary>
public sealed class GblLib
    : IBarsikModule
{
    #region Properties

    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "ADD", new FunctionDescriptor ("ADD", Add) },
        { "CHA", new FunctionDescriptor ("CHA", Cha) },
        { "CHAC", new FunctionDescriptor ("CHAC", Chac) },
        { "DEL", new FunctionDescriptor ("DEL", Del) },
        { "DELR", new FunctionDescriptor ("DELR", DelRecord) },
        { "REP", new FunctionDescriptor ("REP", Replace) },
        { "UNDEL", new FunctionDescriptor ("UNDEL", Undelete) },
        { "UNDOR", new FunctionDescriptor ("UNDOR", Undo) },

    };

    #endregion

    #region Public methods

    /// <summary>
    /// Команда ADD.
    /// </summary>
    public static dynamic? Add
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!IrbisLib.TryGetRecord (context, out var record))
        {
            return null;
        }

        return null;
    }

    /// <summary>
    /// Команда CHA.
    /// </summary>
    public static dynamic? Cha
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!IrbisLib.TryGetRecord (context, out var record))
        {
            return null;
        }

        return null;
    }

    /// <summary>
    /// Команда CHAC.
    /// </summary>
    public static dynamic? Chac
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!IrbisLib.TryGetRecord (context, out var record))
        {
            return null;
        }

        return null;
    }

    /// <summary>
    /// Команда DEL.
    /// </summary>
    public static dynamic? Del
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!IrbisLib.TryGetRecord (context, out var record))
        {
            return null;
        }

        return null;
    }

    /// <summary>
    /// Команда DELR.
    /// </summary>
    public static dynamic? DelRecord
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!IrbisLib.TryGetRecord (context, out var record))
        {
            return null;
        }

        return null;
    }

    /// <summary>
    /// Команда REP.
    /// </summary>
    public static dynamic? Replace
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!IrbisLib.TryGetRecord (context, out var record))
        {
            return null;
        }

        return null;
    }

    /// <summary>
    /// Команда UNDEL.
    /// </summary>
    public static dynamic? Undelete
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!IrbisLib.TryGetRecord (context, out var record))
        {
            return null;
        }

        if ((record.Status & RecordStatus.LogicallyDeleted) == 0)
        {
            return null;
        }

        record.Status &= ~RecordStatus.LogicallyDeleted;

        return null;
    }

    /// <summary>
    /// Команда UNDOR.
    /// </summary>
    public static dynamic? Undo
        (
            Context context,
            dynamic?[] args
        )
    {
        if (!IrbisLib.TryGetRecord (context, out var record))
        {
            return null;
        }

        return null;
    }

    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "GblLib";

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
        if (!context.IsModuleLoaded<IrbisLib>())
        {
            context.AttachModule (new IrbisLib());
        }

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
