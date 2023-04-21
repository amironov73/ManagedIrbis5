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

using AM.Kotik.Barsik;
using AM.Kotik.Barsik.Ast;

using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Kotik.Avalonia;

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
        { "getBrush", new FunctionDescriptor ("getBrush", GetBrush) },
        { "prompt", new FunctionDescriptor ("prompt", Prompt) },
        { "stretchHorizontally", new FunctionDescriptor ("stretchHorizontally", StretchHorizontally) },
        { "stretchVertically", new FunctionDescriptor ("stretchVertically", StretchVertically) },
        { "runDesktopApplication", new FunctionDescriptor ("runDesktopApplication", RunDesktopApplication) },
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
    /// Вычисление аргумента по соответствующему индексу.
    /// </summary>
    public static object? Compute
        (
            Context context,
            dynamic?[] args,
            int index
        )
    {
        if (index >= args.Length)
        {
            return null;
        }

        var arg = args[index];
        if (arg is null)
        {
            return null;
        }

        if (arg is AtomNode atom)
        {
            var value = atom.Compute (context);
            return value;
        }

        return arg;
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
    /// Получение кисти указанного цвета.
    /// </summary>
    public static dynamic? GetBrush
        (
            Context context,
            dynamic?[] args
        )
    {
        return Brushes.Bisque;
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

    /// <summary>
    /// Запуск десктопного приложения.
    /// </summary>
    public static dynamic? RunDesktopApplication
        (
            Context context,
            dynamic?[] args
        )
    {
        Sure.NotNull (context);

        if (Compute (context, args, 0) is FunctionDescriptor descriptor)
        {
            var mainWindowCreator = () =>
                (Window) descriptor.CallPoint.Invoke (context, args)!;
            BarsikApplication.RunDesktopApplication (mainWindowCreator);
        }

        return null;
    }

    /// <summary>
    /// Растяжение по горизонтали.
    /// </summary>
    public static dynamic? StretchHorizontally
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is Control control)
        {
            control.HorizontalAlignment = HorizontalAlignment.Stretch;

            return control;
        }

        return null;
    }

    /// <summary>
    /// Растяжение по вертикали.
    /// </summary>
    public static dynamic? StretchVertically
        (
            Context context,
            dynamic?[] args
        )
    {
        if (Compute (context, args, 0) is Control control)
        {
            control.VerticalAlignment = VerticalAlignment.Stretch;

            return control;
        }

        return null;
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
