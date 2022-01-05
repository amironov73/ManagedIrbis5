// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* StdLib.cs -- стандартная библиотека
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using static AM.Scripting.Barsik.Builtins;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Стандартная библиотека (включая небезопасные функции).
/// </summary>
public sealed class StdLib
    : IBarsikModule
{
    #region Properties

    /// <summary>
    /// Реестр встроенных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "array", new FunctionDescriptor ("array", Array_) },
        { "eval", new FunctionDescriptor ("eval", Evaluate) },
        { "exec", new FunctionDescriptor ("exec", Execute) },
        { "include", new FunctionDescriptor ("include", Include) },
        { "load", new FunctionDescriptor ("load", LoadAssembly) },
        { "module", new FunctionDescriptor ("module", LoadModule) },
        { "system", new FunctionDescriptor ("system", System) },
        { "type", new FunctionDescriptor ("type", Type) },
        { "use", new FunctionDescriptor ("use", Use) },
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Создание массива.
    /// </summary>
    public static dynamic? Array_
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length == 0)
        {
            return Array.Empty<object>();
        }

        var length = Convert.ToInt32 (Compute (context, args, 0));
        if (length <= 0)
        {
            return Array.Empty<object>();
        }

        var type = typeof (object);
        var typeName = Compute (context, args, 1) as string;
        if (!string.IsNullOrEmpty (typeName))
        {
            type = context.FindType (typeName);
            if (type is null)
            {
                context.Error.WriteLine ($"Can't find type {typeName}");
                return null;
            }
        }

        return Array.CreateInstance (type, length);
    }

    /// <summary>
    /// Вычисление значения выражения.
    /// </summary>
    public static dynamic? Evaluate
        (
            Context context,
            dynamic?[] args
        )
    {
        try
        {
            var sourceCode = ComputeAll (context, args);
            if (string.IsNullOrWhiteSpace (sourceCode))
            {
                return null;
            }

            var expression = Grammar.ParseExpression (sourceCode);
            var result = expression.Compute (context);

            return result;
        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }

        return null;
    }

    /// <summary>
    /// Динамическое исполнение скрипта.
    /// </summary>
    public static dynamic? Execute
        (
            Context context,
            dynamic?[] args
        )
    {
        try
        {
            var sourceCode = ComputeAll (context, args);
            if (string.IsNullOrWhiteSpace (sourceCode))
            {
                return null;
            }

            var program = Interpreter.ParseProgram (sourceCode);
            foreach (var statement in program.Statements)
            {
                statement.Execute (context);
            }
        }
        catch (Exception exception)
        {
            context.Error.WriteLine (exception.Message);
        }

        return null;
    }

    /// <summary>
    /// Загрузка указанного скрипта.
    /// </summary>
    public static dynamic? Include
        (
            Context context,
            dynamic?[] args
        )
    {
        // TODO поиск по путям

        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();

                var sourceCode = File.ReadAllText (name);
                var program = Grammar.ParseProgram (sourceCode);
                program.Execute (context);
            }
        }

        return null;
    }

    /// <summary>
    /// Загрузка указанной сборки.
    /// </summary>
    public static dynamic? LoadAssembly
        (
            Context context,
            dynamic?[] args
        )
    {
        // TODO поиск по путям

        Assembly? loaded = null;
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();

                if (File.Exists (name))
                {
                    var fullPath = Path.GetFullPath (name);
                    loaded = Assembly.LoadFile (fullPath);
                }
                else
                {
                    loaded = Assembly.Load (name);
                }

                context.Output.WriteLine ($"Assembly loaded: {loaded.GetName()}");
            }
        }

        return loaded;
    }

    /// <summary>
    /// Загрузка указанного модуля.
    /// </summary>
    public static dynamic? LoadModule
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                context.LoadModule (name);
            }
        }

        return null;
    }

    /// <summary>
    /// Выполнение внешней программы и получение ее выходного потока.
    /// </summary>
    public static dynamic? System
        (
            Context context,
            dynamic?[] args
        )
    {
        var command = (string?) Compute (context, args, 0);
        if (string.IsNullOrWhiteSpace (command))
        {
            return null;
        }

        command = command.Trim();
        var startInfo = new ProcessStartInfo (command)
        {
            UseShellExecute = false,
            RedirectStandardOutput = true
        };
        for (var i = 1; i < args.Length; i++)
        {
            var value = (string?) Compute (context, args, i);
            if (!string.IsNullOrWhiteSpace (value))
            {
                startInfo.ArgumentList.Add (value.Trim());
            }
        }

        var process = Process.Start (startInfo);
        if (process is not null)
        {
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }

        return null;
    }

    /// <summary>
    /// Выдача типа по его имени.
    /// </summary>
    public static dynamic? Type
        (
            Context context,
            dynamic?[] args
        )
    {
        var typeName = Compute (context, args, 0) as string;
        if (string.IsNullOrEmpty (typeName))
        {
            return null;
        }

        return context.FindType (typeName);
    }

    /// <summary>
    /// Подключение/отключение пространств имен.
    /// </summary>
    public static dynamic Use
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            var name = Compute (context, args, i) as string;
            if (!string.IsNullOrWhiteSpace (name))
            {
                name = name.Trim();
                if (name.StartsWith ('-'))
                {
                    name = name.Substring (1);
                    if (!string.IsNullOrEmpty (name))
                    {
                        context.Namespaces.Remove (name);
                    }
                }
                else
                {
                    context.Namespaces[name] = null;
                }
            }
        }

        return context.Namespaces.Keys;
    }

    #endregion

    #region IBarsikModule members

    /// <inheritdoc cref="IBarsikModule.Description"/>
    public string Description => "StdLib";

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
