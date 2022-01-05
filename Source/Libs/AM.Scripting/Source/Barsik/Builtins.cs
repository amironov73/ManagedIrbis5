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
using System.Collections;
using System.Collections.Generic;
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
    /// Вычисление всех значений в виде одной длинной строки.
    /// </summary>
    public static string? ComputeAll
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
        { "cat", new FunctionDescriptor ("cat", Cat) },
        { "chr", new FunctionDescriptor ("chr", Chr) },
        { "debug", new FunctionDescriptor ("debug", Debug) },
        { "delete", new FunctionDescriptor ("delete", Delete, false) },
        { "dispose", new FunctionDescriptor ("dispose", Dispose) },
        { "error", new FunctionDescriptor ("error", Error) },
        { "empty", new FunctionDescriptor ("empty", Empty) },
        { "format", new FunctionDescriptor ("format", Format) },
        { "have_var", new FunctionDescriptor ("havevar", HaveVariable) },
        { "len", new FunctionDescriptor ("len", Length) },
        { "max", new FunctionDescriptor ("max", Max) },
        { "min", new FunctionDescriptor ("min", Min) },
        { "now", new FunctionDescriptor ("now", Now) },
        { "open_read", new FunctionDescriptor ("open_read", OpenRead) },
        { "print", new FunctionDescriptor ("print", Print) },
        { "println", new FunctionDescriptor ("println", PrintLine) },
        { "readln", new FunctionDescriptor ("readln", ReadLine) },
        { "trace", new FunctionDescriptor ("trace", Trace) },
        { "trim", new FunctionDescriptor ("trim", Trim) },
        { "warn", new FunctionDescriptor ("warn", Warn) },
    };

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
        System.Diagnostics.Debug.WriteLine (text);

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
    /// Определение, не пустая ли строка/массив/список/словарь.
    /// </summary>
    public static dynamic? Empty
        (
            Context context,
            dynamic?[] args
        )
    {
        var obj = Compute (context, args, 0);

        return obj switch
        {
            null => true,
            string str => string.IsNullOrEmpty (str),
            IList list => list.Count == 0,
            _ => false
        };
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
    /// Вывод данных в стандартный поток.
    /// </summary>
    public static dynamic? Print
        (
            Context context,
            dynamic?[] args
        )
    {
        foreach (var node in args)
        {
            BarsikUtility.PrintObject (context.Output, node);
        }

        return null;
    }

    /// <summary>
    /// Вывод данных в стандартный поток
    /// с добавлением перевода строки.
    /// </summary>
    public static dynamic? PrintLine
        (
            Context context,
            dynamic?[] args
        )
    {
        Print (context, args);
        context.Output.WriteLine();

        return null;
    }

    /// <summary>
    /// Чтение данных из файла.
    /// </summary>
    public static dynamic? ReadLine
        (
            Context context,
            dynamic?[] args
        )
    {
        return context.Input.ReadLine();
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
