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
using System.Dynamic;
using System.Globalization;

using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

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

        return builder.ReturnShared();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка, соответствует ли указанное имя одной из встроенных функций.
    /// </summary>
    public static bool IsBuiltinFunction
        (
            string name
        )
    {
        return Registry.ContainsKey (name);
    }

    /// <summary>
    /// Реестр встроенных функций.
    /// </summary>
    public static readonly Dictionary<string, FunctionDescriptor> Registry = new ()
    {
        { "apply", new FunctionDescriptor ("apply", Apply, false) },
        { "assert", new FunctionDescriptor ("assert", Assert_) },
        { "bind", new FunctionDescriptor ("bind", Bind) },
        { "chr", new FunctionDescriptor ("chr", Chr) },
        { "debug", new FunctionDescriptor ("debug", Debug_) },
        { "define", new FunctionDescriptor ("define", Define, false) },
        { "delete", new FunctionDescriptor ("delete", Delete, false) },
        { "dispose", new FunctionDescriptor ("dispose", Dispose) },
        { "error", new FunctionDescriptor ("error", Error_) },
        { "empty", new FunctionDescriptor ("empty", Empty) },
        { "exit", new FunctionDescriptor ("exit", Exit) },
        { "expando", new FunctionDescriptor ("expando", Expando) },
        { "filter", new FunctionDescriptor ("filter", Filter, false) },
        { "format", new FunctionDescriptor ("format", Format) },
        { "getenv", new FunctionDescriptor ("getenv", GetEnvironment) },
        { "get_env", new FunctionDescriptor ("get_env", GetEnvironment) },
        { "have_var", new FunctionDescriptor ("havevar", HaveVariable, false) },
        { "iif", new FunctionDescriptor ("iif", Iif) },
        { "len", new FunctionDescriptor ("len", Length) },
        { "local", new FunctionDescriptor ("local", Local, false) },
        { "map", new FunctionDescriptor ("map", Map, false) },
        { "max", new FunctionDescriptor ("max", Max) },
        { "min", new FunctionDescriptor ("min", Min) },
        { "now", new FunctionDescriptor ("now", Now) },
        { "print", new FunctionDescriptor ("print", Print) },
        { "println", new FunctionDescriptor ("println", PrintLine) },
        { "quote", new FunctionDescriptor ("quote", Quote) },
        { "readln", new FunctionDescriptor ("readln", ReadLine) },
        { "reduce", new FunctionDescriptor ("reduce", Reduce, false) },
        { "trace", new FunctionDescriptor ("trace", Trace_) },
        { "trim", new FunctionDescriptor ("trim", Trim) },
        { "warn", new FunctionDescriptor ("warn", Warn) },
    };

    /// <summary>
    /// Применение функции.
    /// </summary>
    public static dynamic? Apply
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 1)
        {
            context.Error.WriteLine ("Too few arguments for apply");
            return null;
        }

        FunctionDescriptor? descriptor = null;
        if (args[0] is VariableNode node
            && !context.FindFunction (node.Name, out descriptor))
        {
            return null;
        }

        if (Compute (context, args, 0) is string name2
            && !context.FindFunction (name2, out descriptor))
        {
            return null;
        }

        if (descriptor is null)
        {
            return null;
        }

        if (Compute (context, args, 1) is not IEnumerable source)
        {
            return null;
        }

        var applyArgs = new List<dynamic?>();
        foreach (var arg in source)
        {
            applyArgs.Add (arg);
        }

        return descriptor.CallPoint (context, applyArgs.ToArray());
    }

    /// <summary>
    /// Проверка условия.
    /// </summary>
    public static dynamic? Assert_
        (
            Context context,
            dynamic?[] args
        )
    {
        var message = "Assertion failed";
        var condition = true;

        for (var i = 0; i < args.Length; i++)
        {
            var value = Compute (context, args, i);

            if (value is string stringValue && !string.IsNullOrEmpty (stringValue))
            {
                message = stringValue;
            }

            if (value is bool boolValue)
            {
                condition &= boolValue;
            }
        }

        if (!condition)
        {
            Exit (context, new dynamic?[] { 1, message });
        }

        return null;
    }

    /// <summary>
    /// Метод bind() создаёт новую функцию, для которой (все или часть) аргументы
    /// зафиксированы указанными значениями.
    /// </summary>
    public static dynamic? Bind
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 1)
        {
            context.Error.WriteLine ("Too few arguments for apply");
            return null;
        }

        FunctionDescriptor? descriptor = null;
        if (args[0] is VariableNode node
            && !context.FindFunction (node.Name, out descriptor))
        {
            return null;
        }

        if (Compute (context, args, 0) is string name2
            && !context.FindFunction (name2, out descriptor))
        {
            return null;
        }

        if (descriptor is null)
        {
            return null;
        }

        if (Compute (context, args, 1) is not IEnumerable source)
        {
            return null;
        }

        var applyArgs = new List<dynamic?>();
        foreach (var arg in source)
        {
            applyArgs.Add (arg);
        }

        // TODO реализовать вызов

        return null;
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
    public static dynamic? Debug_
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
    /// Задание дефайна.
    /// В отличие от переменной, дефайн не может быть пере-присвоен.
    /// </summary>
    public static dynamic? Define
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 2)
        {
            return null;
        }

        string? name;
        if (args[0] is VariableNode node)
        {
            name = node.Name;
        }
        else
        {
            name = Compute (context, args, 0) as string;
            if (string.IsNullOrEmpty (name))
            {
                return null;
            }

            name = name.Trim();
            if (string.IsNullOrEmpty (name))
            {
                return null;
            }
        }

        var value = Compute (context, args, 1);
        context.SetDefine (name, value);

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
            var value = args[index];
            if (value is VariableNode node)
            {
                // имя переменной указано без кавычек

                var name = node.Name;
                if (!string.IsNullOrEmpty (name))
                {
                    context.RemoveVariable (name);
                }
            }
            else
            {
                // имя переменной указано в кавычках

                value = Compute (context, args, index);
                if (value is string name && !string.IsNullOrEmpty (name))
                {
                    context.RemoveVariable (value);
                }
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
    public static dynamic? Error_
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
    /// Досрочное завершение скрипта.
    /// </summary>
    public static dynamic Exit
        (
            Context context,
            dynamic?[] args
        )
    {
        var exitCode = 0;
        string? message = null;

        for (var i = 0; i < args.Length; i++)
        {
            var value = Compute (context, args, i);

            if (value is int intValue)
            {
                exitCode = intValue;
            }

            if (value is string stringValue)
            {
                message = stringValue;
            }
        }

        throw new ExitException (exitCode, message);
    }

    /// <summary>
    /// Создание Expando-объекта.
    /// Подходит для хранения всего подряд.
    /// </summary>
    public static dynamic Expando
        (
            Context context,
            dynamic?[] args
        )
    {
        return new ExpandoObject ();
    }

    /// <summary>
    /// Фильтрация коллекции.
    /// </summary>
    public static dynamic? Filter
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 2)
        {
            context.Error.WriteLine ("Too few arguments for filter");
            return null;
        }

        FunctionDescriptor? descriptor = null;
        if (args[0] is VariableNode node
            && !context.FindFunction (node.Name, out descriptor))
        {
            return null;
        }

        if (Compute (context, args, 0) is string name2
            && !context.FindFunction (name2, out descriptor))
        {
            return null;
        }

        if (descriptor is null)
        {
            return null;
        }

        if (Compute (context, args, 1) is not IEnumerable source)
        {
            return null;
        }

        var child = context.CreateChild();
        var index = 0;
        var callArgs = new dynamic?[3];
        var result = new BarsikList();
        foreach (var current in source)
        {
            callArgs[0] = current;
            callArgs[1] = index;
            callArgs[2] = source;
            var retval = BarsikUtility.ToBoolean (descriptor.CallPoint (child, callArgs));
            if (retval)
            {
                result.Add (current);
            }

            index++;
        }

        return result;
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
    /// Возврат первого непустого аргумента
    /// </summary>
    public static dynamic? Iif
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            var value = Compute (context, args, i);
            if (BarsikUtility.ToBoolean (value))
            {
                return value;
            }
        }

        return null;
    }

    /// <summary>
    /// Проверка существования переменной с указанным именем
    /// (в любом контексте).
    /// </summary>
    public static dynamic? HaveVariable
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length == 0)
        {
            return null;
        }

        string? name;
        if (args[0] is VariableNode node)
        {
            name = node.Name;

            return !string.IsNullOrEmpty (name) && context.TryGetVariable (name, out _);
        }

        name = (string?) Compute (context, args, 0);

        return !string.IsNullOrEmpty (name) && context.TryGetVariable (name, out _);
    }

    /// <summary>
    /// Получение переменной окружения.
    /// </summary>
    public static dynamic? GetEnvironment
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length == 0)
        {
            return null;
        }

        var name = (string?) Compute (context, args, 0);
        if (string.IsNullOrEmpty (name))
        {
            return null;
        }

        return Environment.GetEnvironmentVariable (name);
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
    /// Введение локальных переменных.
    /// </summary>
    public static dynamic? Local
        (
            Context context,
            dynamic?[] args
        )
    {
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] is VariableNode node)
            {
                var name = node.Name;
                if (!string.IsNullOrEmpty (name)
                    && !context.Variables.ContainsKey (name))
                {
                    context.Variables[name] = null;
                }
            }
            else
            {
                var name = Compute (context, args, i) as string;
                if (!string.IsNullOrEmpty (name)
                    && !context.Variables.ContainsKey (name))
                {
                    context.Variables[name] = null;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Отображение коллекции.
    /// </summary>
    public static dynamic? Map
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 2)
        {
            context.Error.WriteLine ("Too few arguments for map");
            return null;
        }

        FunctionDescriptor? descriptor = null;
        if (args[0] is VariableNode node
            && !context.FindFunction (node.Name, out descriptor))
        {
            return null;
        }

        if (Compute (context, args, 0) is string name2
            && !context.FindFunction (name2, out descriptor))
        {
            return null;
        }

        if (descriptor is null)
        {
            return null;
        }

        if (Compute (context, args, 1) is not IEnumerable source)
        {
            return null;
        }

        var child = context.CreateChild();
        var index = 0;
        var callArgs = new dynamic?[3];
        var result = new BarsikList();
        foreach (var current in source)
        {
            callArgs[0] = current;
            callArgs[1] = index;
            callArgs[2] = source;
            var retval = descriptor.CallPoint (child, callArgs);
            result.Add (retval);

            index++;
        }

        return result;
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
            // среди перечисленных значений нет ни одного IComparable
            // непонятно, как это все сравнивать, так что выдаем null

            return null;
        }

        index++;
        while (index < args.Length)
        {
            if (args[index] is IComparable comparable
                && result.CompareTo (comparable) < 0)
            {
                result = comparable;
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
            // среди перечисленных значений нет ни одного IComparable
            // непонятно, как это все сравнивать, так что выдаем null

            return null;
        }

        index++;
        while (index < args.Length)
        {
            if (args[index] is IComparable comparable
                && result.CompareTo (comparable) > 0)
            {
                result = comparable;
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
        var result = DateTime.Now;
        var format = Compute (context, args, 0) as string;
        if (!string.IsNullOrEmpty (format))
        {
            return result.ToString (format);
        }

        return result;
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
    /// Закавычивание указанной строки.
    /// </summary>
    public static dynamic? Quote
        (
            Context context,
            dynamic?[] args
        )
    {
        var firstArg = Compute (context, args, 0);
        return firstArg is null
            ? "\"\""
            : "\"" + firstArg + "\"";

    }

    /// <summary>
    /// Чтение строки из стандартного входного потока.
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
    /// Аккумулирование элементов коллекции.
    /// </summary>
    public static dynamic? Reduce
        (
            Context context,
            dynamic?[] args
        )
    {
        if (args.Length < 3)
        {
            context.Error.WriteLine ("Too few arguments for reduce");
            return null;
        }

        FunctionDescriptor? descriptor = null;
        if (args[0] is VariableNode node
            && !context.FindFunction (node.Name, out descriptor))
        {
            return null;
        }

        if (Compute (context, args, 0) is string name2
            && !context.FindFunction (name2, out descriptor))
        {
            return null;
        }

        if (descriptor is null)
        {
            return null;
        }

        if (Compute (context, args, 1) is not IEnumerable source)
        {
            return null;
        }

        var initialValue = Compute (context, args, 2);
        var index = 0;
        dynamic? result = initialValue;
        var child = context.CreateChild();
        var callArgs = new dynamic?[4];
        foreach (var current in source)
        {
            callArgs[0] = result;
            callArgs[1] = current;
            callArgs[2] = index;
            callArgs[3] = source;
            result = descriptor.CallPoint (child, callArgs);

            index++;
        }

        return result;
    }


    /// <summary>
    /// Трассировочное сообщение.
    /// </summary>
    public static dynamic? Trace_
        (
            Context context,
            dynamic?[] args
        )
    {
        var text = ComputeAll (context, args);
        if (!string.IsNullOrEmpty (text))
        {
            Magna.Logger.LogTrace ("Builtin trace: {Text}", text);
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
            Magna.Logger.LogWarning ("Builtin warning: {Text}", text);
        }

        return null;
    }

    #endregion
}
