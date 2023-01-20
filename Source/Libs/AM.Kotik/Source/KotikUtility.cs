// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* BarsikUtility.cs -- полезные методы для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Полезные методы для Барсика.
/// </summary>
public static class KotikUtility
{
    #region Public methods

    /// <summary>
    /// Динамический вызов произвольного метода.
    /// </summary>
    /// <param name="context">Контекст интерпретатора.</param>
    /// <param name="target">Класс или экземпляр.</param>
    /// <param name="methodName">Имя метода.</param>
    /// <param name="args">Аргументы (уже подготовленные).</param>
    /// <returns>Результат вызова.</returns>
    public static dynamic? CallAnyMethod
        (
            Context context,
            object? target,
            string methodName,
            params object?[] args
        )
    {
        Sure.NotNull (context);
        Sure.NotNull (target);
        Sure.NotNullNorEmpty (methodName);

        bool staticCall;
        Type targetType;
        if (target is Type type)
        {
            staticCall = true;
            targetType = type;
        }
        else
        {
            staticCall = false;
            targetType = target!.GetType();
        }

        var argTypes = new List<Type>();
        foreach (var o in args)
        {
            var argType = o is null ? typeof (object) : o.GetType();
            argTypes.Add (argType);
        }

        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
        bindingFlags |= staticCall ? BindingFlags.Static : BindingFlags.Instance;
        var method = targetType.GetMethod (methodName, bindingFlags, argTypes.ToArray());
        if (method is null)
        {
            context.Error.WriteLine ($"Can't find method {methodName}");
            return null;
        }

        return method.Invoke (target, args);
    }

    /// <summary>
    /// Создание и запуск интерпретатора.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    /// <param name="configure">Опциональная конфигурация интерпретатора.</param>
    /// <param name="exceptionHandler">Опциональный обработчик исключений.</param>
    /// <param name="dumper">Опциональный обработчик дампа переменных.</param>
    /// <returns>Код возврата.</returns>
    public static int CreateAndRunInterpreter
        (
            string[] args,
            Action<Interpreter>? configure = null,
            Action<Interpreter, Exception>? exceptionHandler = null,
            Action<Interpreter>? dumper = null
        )
    {
        var interpreter = new Interpreter();
        configure?.Invoke (interpreter);

        try
        {
            var dump = false;
            var index = 0;

            if (args.Length == 0)
            {
                var result = DoRepl (interpreter);
                if (result.ExitCode != 0)
                {
                    interpreter.Context.Error.WriteLine (result);
                }
            }

            foreach (var fileName in args)
            {
                if (fileName == "-d")
                {
                    dump = true;
                    continue;
                }

                if (fileName == "-r")
                {
                    DoRepl (interpreter);
                    continue;
                }

                if (fileName == "-e")
                {
                    var sourceCode = string.Join (' ', args.Skip (index + 1));
                    interpreter.Execute (sourceCode);
                    break;
                }

                var result = interpreter.ExecuteFile (fileName);
                if (result.ExitCode != 0)
                {
                    interpreter.Context.Error.WriteLine (result);
                }

                if (result.ExitRequested)
                {
                    break;
                }

                index++;
            }

            if (dump)
            {
                dumper?.Invoke (interpreter);
            }
        }
        catch (Exception exception)
        {
            exceptionHandler?.Invoke (interpreter, exception);
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Запуск REPL на указанном интерпретаторе.
    /// </summary>
    public static ExecutionResult DoRepl
        (
            Interpreter interpreter
        )
    {
        Sure.NotNull (interpreter);

        var version = Interpreter.FileVersion;
        interpreter.Context.Output.WriteLine ($"Meow interpreter {version}");
        interpreter.Context.Output.WriteLine ("Press ENTER twice to exit");

        return new Repl (interpreter).Loop();
    }

    /// <summary>
    /// Вывод на печать Expando-объекта.
    /// </summary>
    public static void PrintExpando
        (
            TextWriter output,
            ExpandoObject? expando
        )
    {
        if (expando is null)
        {
            output.Write ("(null)");
            return;
        }

        var dictionary = (IDictionary<string, object?>)expando;
        output.Write ("{");

        var keys = dictionary.Keys;
        var first = true;
        foreach (var key in keys)
        {
            if (!first)
            {
                output.Write (", ");
            }

            PrintObject (output, key);
            output.Write (": ");
            PrintObject (output, dictionary[key]);

            first = false;
        }

        output.Write ("}");
    }

    /// <summary>
    /// Вывод на печать произвольного объекта.
    /// </summary>
    public static void PrintObject
        (
            TextWriter output,
            object? value
        )
    {
        if (value is null)
        {
            output.Write ("(null)");
            return;
        }

        if (value is bool b)
        {
            output.Write (b ? "true" : "false");
            return;
        }

        if (value is string)
        {
            output.Write (value);
            return;
        }

        if (value is ExpandoObject expando)
        {
            PrintExpando (output, expando);
            return;
        }

        if (value is Array array)
        {
            PrintArray (output, array);
            return;
        }

        var type = value.GetType();
        if (type.IsPrimitive)
        {
            if (value is IFormattable formattable)
            {
                output.Write (formattable.ToString (null, CultureInfo.InvariantCulture));
            }
            else
            {
                output.Write (value);
            }

            return;
        }

        switch (value)
        {
            case IDictionary dictionary:
                PrintDictionary (output, dictionary);
                break;

            case IEnumerable sequence:
                PrintSequence (output, sequence);
                break;

            case IFormattable formattable:
                output.Write (formattable.ToString (null, CultureInfo.InvariantCulture));
                break;

            default:
                output.Write (value);
                break;
        }
    }

    /// <summary>
    /// Вывод массива на печать.
    /// </summary>
    public static void PrintArray
        (
            TextWriter output,
            Array? array
        )
    {
        if (array is null)
        {
            output.Write ("(null)");
            return;
        }

        output.Write ("[");
        for (var i = 0; i < array.Length; i++)
        {
            if (i != 0)
            {
                output.Write (", ");
            }

            PrintObject (output, array.GetValue (i));
        }

        output.Write ("]");
    }

    /// <summary>
    /// Вывод на печать словаря.
    /// </summary>
    public static void PrintDictionary
        (
            TextWriter output,
            IDictionary? dictionary
        )
    {
        if (dictionary is null)
        {
            output.Write ("(null)");
            return;
        }

        output.Write ("{");

        var first = true;
        foreach (DictionaryEntry entry in dictionary)
        {
            if (!first)
            {
                output.Write (", ");
            }

            PrintObject (output, entry.Key);
            output.Write (": ");
            PrintObject (output, entry.Value);

            first = false;
        }

        output.Write ("}");
    }

    /// <summary>
    /// Вывод на печать последовательности.
    /// </summary>
    public static void PrintSequence
        (
            TextWriter output,
            IEnumerable? sequence
        )
    {
        if (sequence is null)
        {
            output.Write ("(null)");
            return;
        }

        if (sequence is IDictionary dictionary)
        {
            PrintDictionary (output, dictionary);
            return;
        }

        if (sequence is string)
        {
            output.Write (sequence);
            return;
        }

        var first = true;
        output.Write ("[");
        foreach (var item in sequence)
        {
            if (!first)
            {
                output.Write (", ");
            }

            PrintObject (output, item);
            first = false;
        }

        output.Write ("]");
    }

    /// <summary>
    /// Удаление shebang из исходного кода.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Шебанг  (англ. shebang, sha-bang, hashbang, pound-bang, or hash-pling)
    /// - в программировании последовательность из символов решётки
    /// и восклицательного знака ("#!") в начале файла скрипта.
    /// </para>
    /// <para>
    /// https://ru.wikipedia.org/wiki/%D0%A8%D0%B5%D0%B1%D0%B0%D0%BD%D0%B3_(Unix)
    /// </para>
    /// <para>
    /// Когда скрипт с шебангом выполняется как программа в Unix-подобных
    /// операционных системах, загрузчик программ рассматривает остаток
    /// строки после шебанга как имя файла программы-интерпретатора.
    /// Загрузчик запускает эту программу и передаёт ей в качестве параметра
    /// имя файла скрипта с шебангом.[8] Например, если полное имя файла
    /// скрипта "path/to/script" и первая строка этого файла:
    /// </para>
    /// <code>
    /// #!/bin/sh
    /// </code>
    /// <para>
    /// то загрузчик запускает на выполнение "/bin/sh" (обычно это Bourne shell
    /// или совместимый интерпретатор командной строки) и передаёт
    /// "path/to/script" как первый параметр.
    /// </para>
    /// <para>
    /// Строка с шебангом обычно пропускается интерпретатором, так как символ
    /// "#" является символом начала комментариев во многих скриптовых языках.
    /// Некоторые интерпретаторы, которые не используют символ решётки
    /// для обозначения начала комментариев (такие, как Scheme), могут пропустить
    /// строку шебанга, определив её назначение. Другие решения полагаются
    /// на препроцессор, который обрабатывает и удаляет строку шебанга перед
    /// тем, как остальная часть скрипта передаётся компилятору или интерпретатору.
    /// Так, например, работает InstantFPC, который позволяет запускать программы,
    /// написанные на Free Pascal, как скрипты на некоторых операционных системах.
    /// </para>
    /// </remarks>
    public static string RemoveShebang
        (
            string sourceCode
        )
    {
        Sure.NotNull (sourceCode);

        if (!sourceCode.StartsWith ("#!"))
        {
            return sourceCode;
        }

        var stream = new StringReader (sourceCode);
        stream.ReadLine();
        var result = stream.ReadToEnd();

        return result;
    }

    /// <summary>
    /// Преобразование любого значения в логическое.
    /// </summary>
    public static bool ToBoolean
        (
            object? value
        )
    {
        return value switch
        {
            null => false,
            true => true,
            false => false,
            "true" or "True" => true,
            "false" or "False" => false,
            string text => !string.IsNullOrEmpty (text),
            sbyte sb => sb != 0,
            byte b => b != 0,
            short i16 => i16 != 0,
            ushort u16 => u16 != 0,
            int i32 => i32 != 0,
            uint u32 => u32 != 0,
            long i64 => i64 != 0,
            ulong u64 => u64 != 0,
            float f32 => f32 != 0.0f, //-V3024
            double d64 => d64 != 0.0, //-V3024
            decimal d => d != 0.0m,
            IList list => list.Count != 0,
            IDictionary dictionary => dictionary.Count != 0,
            _ => true
        };
    }

    /// <summary>
    /// Преобразование любого значения в целое со знаком.
    /// </summary>
    public static int ToInt32
        (
            object? value
        )
    {
        return value switch
        {
            null => 0,
            true => 1,
            false => 0,
            string text => text.SafeToInt32(),
            sbyte sb => sb,
            byte b => b,
            short i16 => i16,
            ushort u16 => u16,
            int i32 => i32,
            uint u32 => unchecked ((int)u32),
            long i64 => unchecked ((int)i64),
            ulong u64 => unchecked ((int)u64),
            float f32 => unchecked ((int)f32),
            double d64 => unchecked ((int)d64),
            decimal d => (int)d,
            _ => 0
        };
    }

    /// <summary>
    /// Преобразование значения в строку.
    /// </summary>
    public static string ToString
        (
            object? value
        )
    {
        var builder = StringBuilderPool.Shared.Get();
        var output = new StringWriter (builder);
        PrintObject (output, value);

        return builder.ReturnShared();
    }

    /// <summary>
    /// Вычисление длины объекта.
    /// </summary>
    public static int GetLength
        (
            object? value
        )
    {
        return value switch
        {
            null => 0,
            string text => text.Length,
            Array array => array.Length,
            IList list => list.Count,
            IDictionary dictionary => dictionary.Count,
            _ => 1
        };
    }

    #endregion
}
