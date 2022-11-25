// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* AvaloniaModule.cs -- Barsik-модуль для Avalonia
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM.Scripting.Barsik;

using static AM.Scripting.Barsik.Builtins;

#endregion

namespace AM.Scripting.WinForms;

/// <summary>
/// Barsik-модуль для Avalonia UI.
/// </summary>
public sealed class AvaloniaModule
    : IBarsikModule
{
    #region Properties

    /// <summary>
    /// Реестр стандартных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "alert", new FunctionDescriptor ("alert", Alert) },
        { "confirm", new FunctionDescriptor ("confirm", Confirm) },
        { "prompt", new FunctionDescriptor ("prompt", Prompt) },
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Выдача простого сообщения.
    /// </summary>
    public static dynamic? Alert
        (
            Context context,
            dynamic?[] args
        )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Простейшее подтверждение действия у пользователя.
    /// </summary>
    public static dynamic? Confirm
        (
            Context context,
            dynamic?[] args
        )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Простейший ввод строки.
    /// </summary>
    public static dynamic? Prompt
        (
            Context context,
            dynamic?[] args
        )
    {
        throw new NotImplementedException();
    }

    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "Avalonia";

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
