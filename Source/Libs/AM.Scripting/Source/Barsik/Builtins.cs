// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Builtins.cs -- встроенные функции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Встроенные функции.
/// </summary>
public static class Builtins
{
    #region Private members

    /// <summary>
    /// Вычисление аргумента по соответствующему индексу.
    /// </summary>
    private static object? Compute
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
    /// Вычисление всех значений в виде одной длинной строки.
    /// </summary>
    private static string? ComputeAll
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length == 0)
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        for (var index = 0; index < args.Length; index++)
        {
            var value = Compute (context, args, index);
            if (value is IFormattable formattable)
            {
                builder.Append (formattable.ToString (null, CultureInfo.InvariantCulture));
            }
            else
            {
                builder.Append (value);
            }
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Реестр встроенных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "bold", new FunctionDescriptor ("bold", Bold) },
        { "cat", new FunctionDescriptor ("cat", Cat) },
        { "chr", new FunctionDescriptor ("chr", Chr) },
        { "debug", new FunctionDescriptor ("debug", Debug) },
        { "delete", new FunctionDescriptor ("delete", Delete, false) },
        { "dispose", new FunctionDescriptor ("dispose", Dispose) },
        { "error", new FunctionDescriptor ("error", Error) },
        { "eval", new FunctionDescriptor ("eval", Evaluate) },
        { "exec", new FunctionDescriptor ("exec", Execute) },
        { "format", new FunctionDescriptor ("format", Format) },
        { "have_var", new FunctionDescriptor ("havevar", HaveVariable) },
        { "italic", new FunctionDescriptor ("italic", Italic) },
        { "len", new FunctionDescriptor ("len", Length) },
        { "max", new FunctionDescriptor ("max", Max) },
        { "min", new FunctionDescriptor ("min", Min) },
        { "now", new FunctionDescriptor ("now", Now) },
        { "open_read", new FunctionDescriptor ("open_read", OpenRead) },
        { "readln", new FunctionDescriptor ("readln", Readln) },
        { "system", new FunctionDescriptor ("system", System) },
        { "to_bool", new FunctionDescriptor ("to_bool", ToBoolean) },
        { "to_byte", new FunctionDescriptor ("to_byte", ToByte) },
        { "to_str", new FunctionDescriptor ("to_str", ToString) },
        { "trace", new FunctionDescriptor ("trace", Trace) },
        { "trim", new FunctionDescriptor ("trim", Trim) },
        { "type", new FunctionDescriptor ("type", Type) },
        { "warn", new FunctionDescriptor ("warn", Warn) },
    };

    /// <summary>
    /// Выделение текста жирным шрифтом.
    /// </summary>
    public static dynamic? Bold
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = Compute (context, args, 0);

        return value is null ? null : "<b>" + value + "</b>";
    }

    /// <summary>
    /// Чтение содержимого файла.
    /// </summary>
    public static dynamic? Cat
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = (string?) Compute (context, args, 0);
        if (string.IsNullOrEmpty (fileName))
        {
            context.Error.WriteLine ("File name is not specified");

            return null;
        }

        if (!File.Exists (fileName))
        {
            context.Error.WriteLine ($"File '{fileName}' doesn't exist");

            return null;
        }

        return File.ReadAllText (fileName);
    }

    /// <summary>
    /// Символ с указанным кодом.
    /// </summary>
    public static dynamic? Chr
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = Compute (context, args, 0);
        if (value is null)
        {
            return null;
        }

        return Convert.ToChar (value);
    }

    /// <summary>
    /// Выдача отладочного сообщения.
    /// </summary>
    public static dynamic? Debug
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = ComputeAll (context, args);
        global::System.Diagnostics.Debug.WriteLine (text);

        return null;
    }

    /// <summary>
    /// Удаление из текущего контекста указанной переменной.
    /// </summary>
    public static dynamic? Delete
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var index = 0; index < args.Length; index++)
        {
            var value = Compute (context, args, index);

            if (value is string name && !string.IsNullOrEmpty (name))
            {
                context.Variables.Remove (name);
            }
        }

        return null;
    }

    /// <summary>
    /// Освобождение ресурса(ов).
    /// </summary>
    public static dynamic? Dispose
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            var value = Compute (context, args, i);
            if (value is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        return null;
    }

    /// <summary>
    /// Выдача сообщения в поток ошибок.
    /// </summary>
    public static dynamic? Error
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = ComputeAll (context, args);
        if (!string.IsNullOrEmpty (text))
        {
            context.Error.WriteLine (text);
        }

        return null;
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
    /// Форматирование.
    /// </summary>
    public static dynamic Format
        (
            Context context,
            dynamic?[] args
        )
    {
        var format = (string?) Compute (context, args, 0);
        if (string.IsNullOrEmpty (format))
        {
            return string.Empty;
        }

        var other = new List<object?> ();
        for (var index = 1; index < args.Length; index++)
        {
            var value = Compute (context, args, index);
            other.Add (value);
        }

        var result = string.Format (CultureInfo.InvariantCulture, format, other.ToArray());

        return result;
    }

    /// <summary>
    /// Проверка существования переменной с указанным именем
    /// (в любом контексте).
    /// </summary>
    public static dynamic HaveVariable
        (
            Context context,
            dynamic?[] args
        )
    {
        var name = (string?) Compute (context, args, 0);
        if (string.IsNullOrEmpty (name))
        {
            return false;
        }

        return context.TryGetVariable (name, out _);
    }

    /// <summary>
    /// Выделение текста курсивом.
    /// </summary>
    public static dynamic? Italic
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = Compute (context, args, 0);

        return value is null ? null : "<i>" + value + "</i>";
    }

    /// <summary>
    /// Вычисление длины объекта.
    /// </summary>
    public static dynamic Length
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = Compute (context, args, 0);

        return BarsikUtility.GetLength (value);
    }

    /// <summary>
    /// Поиск максимального значения среди перечисленных.
    /// </summary>
    public static dynamic? Max
        (
            Context context,
            dynamic?[] args
        )
    {
        IComparable? result = null;
        var index = 0;

        while (index < args.Length)
        {
            if (args[index] is IComparable comparable)
            {
                result = comparable;
                break;
            }

            index++;
        }

        if (result is null)
        {
            return null;
        }

        index++;
        while (index < args.Length)
        {
            if (args[index] is IComparable comparable)
            {
                if (result.CompareTo (comparable) < 0)
                {
                    result = comparable;
                }
            }

            index++;
        }

        return result;
    }

    /// <summary>
    /// Поиск минимального значения среди перечисленных.
    /// </summary>
    public static dynamic? Min
        (
            Context context,
            dynamic?[] args
        )
    {
        IComparable? result = null;
        var index = 0;

        while (index < args.Length)
        {
            if (args[index] is IComparable comparable)
            {
                result = comparable;
                break;
            }

            index++;
        }

        if (result is null)
        {
            return null;
        }

        index++;
        while (index < args.Length)
        {
            if (args[index] is IComparable comparable)
            {
                if (result.CompareTo (comparable) > 0)
                {
                    result = comparable;
                }
            }

            index++;
        }

        return result;
    }

    /// <summary>
    /// Текущие дата и время.
    /// </summary>
    public static dynamic Now
        (
            Context context,
            dynamic?[] args
        )
    {
        return DateTime.Now;
    }

    /// <summary>
    /// Открытие файла только для чтения.
    /// </summary>
    public static dynamic? OpenRead
        (
            Context context,
            dynamic?[] args
        )
    {
        var fileName = (string?) Compute (context, args, 0);
        if (string.IsNullOrWhiteSpace (fileName))
        {
            context.Error.WriteLine ("No file name specified");
            return null;
        }

        if (!File.Exists (fileName))
        {
            context.Error.WriteLine ($"File {fileName} doesn't exist");
            return null;
        }

        return new StreamReader (fileName);
    }

    /// <summary>
    /// Чтение данных из файла.
    /// </summary>
    public static dynamic? Readln
        (
            Context context,
            dynamic?[] args
        )
    {
        return context.Input.ReadLine();
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
    /// Преобразование любого значения в логическое.
    /// </summary>
    public static dynamic ToBoolean
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = Compute (context, args, 0);

        return BarsikUtility.ToBoolean (value);
    }

    /// <summary>
    /// Преобразование любого значения в беззнаковый байт.
    /// </summary>
    public static dynamic ToByte
        (
            Context context,
            dynamic?[] args
        )
    {
        var value = Compute (context, args, 0);

        return Convert.ToByte (value);
    }

    /// <summary>
    /// Преобразование любого значения в строку.
    /// </summary>
    /// <remarks>
    /// Всегда возвращается не <c>null</c>.
    /// Взамен <c>null</c> возвращается <see cref="string.Empty"/>.
    /// </remarks>
    public static dynamic ToString
        (
            Context context,
            dynamic?[] args
        )
    {
        var result = ComputeAll (context, args);

        return result ?? string.Empty;
    }

    /// <summary>
    /// Трассировочное сообщение.
    /// </summary>
    public static dynamic? Trace
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = ComputeAll (context, args);
        if (!string.IsNullOrEmpty (text))
        {
            Magna.Trace (text);
        }

        return null;
    }

    /// <summary>
    /// Обрезка начальных и конечных пробелов в строке.
    /// </summary>
    public static dynamic? Trim
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = ComputeAll (context, args);

        return string.IsNullOrEmpty (text)
            ? text
            : text.Trim();
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
    /// Предупреждающее сообщение.
    /// </summary>
    public static dynamic? Warn
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = ComputeAll (context, args);
        if (!string.IsNullOrEmpty (text))
        {
            Magna.Warning (text);
        }

        return null;
    }

    /// <summary>
    /// Запись данных в файл.
    /// </summary>
    public static dynamic Write
        (
            Context context,
            dynamic?[] args
        )
    {
        throw new NotImplementedException();
    }

    #endregion
}
