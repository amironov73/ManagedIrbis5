// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Utility.cs -- сборник простых вспомогательных методов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace AvaloniaApp;

/// <summary>
/// Сборник простых вспомогательных методов.
/// </summary>
public static class Utility
{
    #region Properties

    /// <summary>
    /// Переменная не используется.
    /// </summary>
    [DebuggerStepThrough]
    public static void NotUsed<T>
        (
            this T variable,
            [CallerArgumentExpression ("variable")]
            string? variableName = null,
            [CallerMemberName] string? member = null,
            [CallerFilePath] string? file = null,
            [CallerLineNumber] int line = 0
        )
    {
        Debug.WriteLine ($"variable {variableName} on {file}: {line} not used");
    }

    /// <summary>
    /// Требование, чтобы аргументы <paramref name="argument"/>
    /// имел тип <typeparamref name="TType"/>.
    /// </summary>
    public static TType SureOfType<TType>
        (
            this object? argument,
            [CallerArgumentExpression (nameof (argument))]
            string? argumentName = null
        )
    {
        if (argument is TType value)
        {
            return value;
        }

        if (!string.IsNullOrEmpty (argumentName))
        {
            // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
            throw new ArgumentException (argumentName);
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Очистка с последующим обнулением переменной.
    /// </summary>
    public static void DisposeAndNull<T>
        (
            ref T? obj
        )
        where T: class, IDisposable
    {
        if (obj is not null)
        {
            obj.Dispose();
            obj = null;
        }
    }

    /// <summary>
    /// Очистка объекта после получения результата вычисления.
    /// </summary>
    public static TOutput DisposeAfter<TInput, TOutput>
        (
            this TInput disposable,
            Func<TInput, TOutput> action
        )
        where TInput: IDisposable
    {
        using (disposable)
        {
            return action (disposable);
        }
    }

    /// <summary>
    /// Очистка объекта после получения результата вычисления.
    /// </summary>
    public static TOutput DisposeAfter<TInput, TOutput>
        (
            this TInput disposable,
            Func<TOutput> action
        )
        where TInput: IDisposable
    {
        using (disposable)
        {
            return action();
        }
    }

    /// <summary>
    /// Очистка объекта после выполнения процедуры.
    /// </summary>
    public static void DisposeAfter<T>
        (
            this T disposable,
            Action<T> action
        )
        where T: IDisposable
    {
        using (disposable)
        {
            action(disposable);
        }
    }

    /// <summary>
    /// Очистка объекта после выполнения процедуры.
    /// </summary>
    public static void DisposeAfter<T>
        (
            this T disposable,
            Action action
        )
        where T: IDisposable
    {
        using (disposable)
        {
            action();
        }
    }

    /// <summary>
    /// Первый день следующего месяца.
    /// </summary>
    [Pure]
    public static DateTime BeginningOfNextMonth => BeginningOfTheMonth.AddMonths (1);

    /// <summary>
    /// Первый день следующего года.
    /// </summary>
    [Pure]
    public static DateTime BeginningOfNextYear => BeginningOfTheYear.AddYears (1);

    /// <summary>
    /// Первый день предыдущего месяца.
    /// </summary>
    [Pure]
    public static DateTime BeginningOfPreviousMonth => BeginningOfTheMonth.AddMonths (-1);

    /// <summary>
    /// Первый день предыдущего года.
    /// </summary>
    [Pure]
    public static DateTime BeginningOfPreviousYear => BeginningOfTheYear.AddYears (-1);

    /// <summary>
    /// Первый день текущего месяца.
    /// </summary>
    [Pure]
    public static DateTime BeginningOfTheMonth
    {
        [DebuggerStepThrough]
        get
        {
            var today = Today;

            return new DateTime (today.Year, today.Month, 1);
        }
    }

    /// <summary>
    /// Первый день текущего года.
    /// </summary>
    [Pure]
    public static DateTime BeginningOfTheYear => new
        (
            DateTime.Today.Year,
            1,
            1
        );

    /// <summary>
    /// Сегодняшний день.
    /// </summary>
    [Pure]
    public static DateTime Today => DateTime.Today;

    /// <summary>
    /// Завтрашний день.
    /// </summary>
    [Pure]
    public static DateTime Tomorrow => Today.AddDays (1.0);

    /// <summary>
    /// Вчерашний день.
    /// </summary>
    [Pure]
    public static DateTime Yesterday => Today.AddDays (-1.0);

    /// <summary>
    /// Длительность: одни сутки.
    /// </summary>
    [Pure]
    public static TimeSpan OneDay => new (1, 0, 0, 0);

    /// <summary>
    /// Длительность: один час.
    /// </summary>
    [Pure]
    public static TimeSpan OneHour => new (1, 0, 0);

    /// <summary>
    /// Длительность: одна минута.
    /// </summary>
    [Pure]
    public static TimeSpan OneMinute => new (0, 1, 0);

    /// <summary>
    /// Длительность: одна секунда.
    /// </summary>
    [Pure]
    public static TimeSpan OneSecond => new (0, 0, 1);

    /// <summary>
    /// Кодировка CP866 (кириллическая) <see cref="Encoding"/>.
    /// </summary>
    public static Encoding Cp866
    {
        [DebuggerStepThrough]
        get
        {
            if (_cp866 is null)
            {
                RegisterEncodingProviders();
                _cp866 = Encoding.GetEncoding (866);
            }

            return _cp866;
        }
    }

    /// <summary>
    /// Кодировка Windows-1251 (кириллическая) <see cref="Encoding"/>.
    /// </summary>
    public static Encoding Windows1251
    {
        [DebuggerStepThrough]
        get
        {
            if (_windows1251 is null)
            {
                RegisterEncodingProviders();
                _windows1251 = Encoding.GetEncoding (1251);
            }

            return _windows1251;
        }
    }

    #endregion

    #region Private members

    private static Encoding? _cp866, _windows1251;

    private static readonly char[] _whitespace = { ' ', '\t', '\r', '\n' };

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка, состоит ли строка только из указанного символа.
    /// </summary>
    /// <remarks>
    /// Пустая строка считается НЕ состоящей из символов.
    /// </remarks>
    [Pure]
    public static bool ConsistOf
        (
            this string? value,
            char c
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return false;
        }

        foreach (var c1 in value)
        {
            if (c1 != c)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Проверка, состоит ли строка только из указанных символов.
    /// </summary>
    /// <remarks>
    /// Пустая строка считается НЕ состоящей из символов.
    /// </remarks>
    [Pure]
    public static bool ConsistOf
        (
            this string? value,
            params char[] array
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return false;
        }

        foreach (var c in value)
        {
            if (Array.IndexOf (array, c) < 0)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Проверка, состоит ли строка только из указанных символов.
    /// </summary>
    /// <remarks>
    /// Пустая строка считается НЕ состоящей из символов.
    /// </remarks>
    [Pure]
    public static bool ConsistOf
        (
            this string? value,
            ReadOnlySpan<char> symbols
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return false;
        }

        foreach (var c in value)
        {
            if (!symbols.Contains (c))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Проверка, состоит ли строка только из цифр.
    /// </summary>
    [Pure]
    public static bool ConsistOfDigits
        (
            this string? value,
            int startIndex,
            int endIndex
        )
    {
        if (string.IsNullOrEmpty (value)
            || startIndex >= value.Length)
        {
            return false;
        }

        endIndex = Math.Min (endIndex, value.Length);
        for (var i = startIndex; i < endIndex; i++)
        {
            if (!char.IsDigit (value[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Проверка, состоит ли строка только из цифр.
    /// </summary>
    [Pure]
    public static bool ConsistOfDigits
        (
            this string? value
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return false;
        }

        foreach (var c in value)
        {
            if (!char.IsDigit (c))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Проверка, содержит ли строка любой из перечисленных символов.
    /// </summary>
    [Pure]
    public static bool ContainsAnySymbol
        (
            this string? text,
            params char[] symbols
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            foreach (var c in text)
            {
                if (symbols.Contains (c))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Проверка, содержит ли строка любой из перечисленных символов.
    /// </summary>
    [Pure]
    public static bool ContainsAnySymbol
        (
            this string? text,
            ReadOnlySpan<char> symbols
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            foreach (var c in text)
            {
                if (symbols.Contains (c))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Строка содержит пробельные символы?
    /// </summary>
    [Pure]
    public static bool ContainsWhitespace
        (
            this string? text
        )
    {
        return text.ContainsAnySymbol (_whitespace);
    }

    /// <summary>
    /// Подсчет вхождений указанной строки в тексте.
    /// </summary>
    [Pure]
    public static int CountSubstrings
        (
            this string? text,
            string? substring
        )
    {
        var result = 0;

        if (!string.IsNullOrEmpty (text)
            && !string.IsNullOrEmpty (substring))
        {
            var length = substring.Length;
            var offset = 0;

            while (true)
            {
                var index = text.IndexOf
                    (
                        substring,
                        offset,
                        StringComparison.OrdinalIgnoreCase
                    );
                if (index < 0)
                {
                    break;
                }

                result++;
                offset = index + length;
            }
        }

        return result;
    }

    /// <summary>
    /// Содержит ли перечень строк указанную строку
    /// (с точностью до регистра символов)?
    /// </summary>
    [Pure]
    public static bool ContainsNoCase
        (
            this IEnumerable<string> lines,
            string line
        )
    {
        foreach (var one in lines)
        {
            if (SameString (one, line))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Безопасное извлечение подстроки (не должно бросаться
    /// исключениями, разве что при нехватке памяти).
    /// </summary>
    public static string? SafeSubstring
        (
            this string? text,
            int offset,
            int width
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var length = text.Length;
        if (offset < 0
            || offset >= length
            || width <= 0)
        {
            return string.Empty;
        }

        if (offset + width > length)
        {
            width = length - offset;
        }

        if (width <= 0)
        {
            return string.Empty;
        }

        var result = text.Substring (offset, width);

        return result;
    }

    /// <summary>
    /// Безопасное извлечение подстроки (не должно бросаться
    /// исключениями вообще никогда).
    /// </summary>
    public static ReadOnlySpan<char> SafeSubSpan
        (
            this ReadOnlySpan<char> text,
            int offset,
            int width
        )
    {
        if (text.IsEmpty)
        {
            return text;
        }

        var length = text.Length;
        if (offset < 0
            || offset >= length
            || width <= 0)
        {
            return string.Empty;
        }

        if (offset + width > length)
        {
            width = length - offset;
        }

        if (width <= 0)
        {
            return string.Empty;
        }

        var result = text.Slice (offset, width);

        return result;
    }

    /// <summary>
    /// Исправление кодировки символов в строке.
    /// </summary>
    public static string ChangeEncoding
        (
            Encoding fromEncoding,
            Encoding toEncoding,
            string value
        )
    {
        if (fromEncoding.Equals (toEncoding))
        {
            return value;
        }

        var bytes = fromEncoding.GetBytes (value);
        var result = toEncoding.GetString (bytes);

        return result;
    }

    /// <summary>
    /// Проверка, является ли указанный символ безопасным с точки
    /// зрения помещения его в URL.
    /// </summary>
    /// <remarks>Set of safe chars, from RFC 1738.4 minus '+'</remarks>
    [Pure]
    public static bool IsUrlSafeChar
        (
            char ch
        )
    {
        if (ch is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9')
        {
            return true;
        }

        switch (ch)
        {
            case '-':
            case '_':
            case '.':
            case '!':
            case '*':
            case '(':
            case ')':
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Кодирование строки в формат URL.
    /// </summary>
    public static string? UrlEncode
        (
            string? text,
            Encoding encoding
        )
    {
        char _IntToHex (int n) => n <= 9 ? (char)(n + '0') : (char)(n - 10 + 'A');

        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var bytes = encoding.GetBytes (text);
        var builder = new StringBuilder();
        foreach (var b in bytes)
        {
            var c = (char) b;
            if (IsUrlSafeChar (c))
            {
                builder.Append (c);
            }
            else if (c == ' ')
            {
                builder.Append ('+');
            }
            else
            {
                builder.Append ('%');
                builder.Append (_IntToHex ((b >> 4) & 0x0F));
                builder.Append (_IntToHex (b & 0x0F));
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// Проверка строго на цифры от <c>0</c> до <c>9</c> включительно,
    /// различные "расширенные цифры" не рассматриваются.
    /// Is digit from 0 to 9?
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool IsArabicDigit
        (
            this char c
        )
    {
        return c is >= '0' and <= '9';
    }

    /// <summary>
    /// Проверка строго на символы от <c>A</c> до <c>Z</c> включительно
    /// и от <c>a</c>  до <c>z</c> включительно, различные "расширенные латиницы"
    /// и прочие символы не рассматриваются.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool IsLatinLetter
        (
            this char c
        )
    {
        return c is >= 'A' and <= 'Z' or >= 'a' and <= 'z';
    }

    /// <summary>
    /// Проверка строго на символы от <c>A</c> до <c>Z</c> включительно
    /// и от <c>a</c>  до <c>z</c> включительно и цифры от <c>0</c> до <c>9</c>
    /// включительно, прочие символы не рассматриваются.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool IsLatinLetterOrArabicDigit
        (
            this char c
        )
    {
        return c is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z';
    }

    /// <summary>
    /// Проверка, является ли указанный символ русской буквой от А до Я (или от а до я).
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool IsRussianLetter
        (
            this char c
        )
    {
        return c is >= 'А' and <= 'я' or 'Ё' or 'ё';
    }

    /// <summary>
    /// Перенаправление стандартного вывода в файл.
    /// </summary>
    public static void RedirectStandardOutput
        (
            string fileName,
            Encoding encoding
        )
    {
        var stdOutput = new StreamWriter (new FileStream (fileName, FileMode.Create), encoding)
        {
            AutoFlush = true
        };

        Console.SetOut (stdOutput);
    }

    /// <summary>
    /// Переключение кодовой страницы вывода консоли.
    /// </summary>
    public static void SetOutputCodePage
        (
            Encoding encoding
        )
    {
        var stdOutput = new StreamWriter
            (
                Console.OpenStandardOutput(),
                encoding
            )
            {
                AutoFlush = true
            };
        Console.SetOut (stdOutput);

        var stdError = new StreamWriter
            (
                Console.OpenStandardError(),
                encoding
            )
            {
                AutoFlush = true
            };
        Console.SetError (stdError);
    }

    /// <summary>
    /// Переключение кодовой страницы вывода консоли.
    /// </summary>
    public static void SetOutputCodePage
        (
            int codePage
        )
    {
        SetOutputCodePage (Encoding.GetEncoding (codePage));
    }

    /// <summary>
    /// Определение среды исполнения: AppVeyor CI.
    /// </summary>
    [Pure]
    public static bool DetectAppVeyor()
    {
        return Environment.GetEnvironmentVariable ("APPVEYOR").SameString ("True");
    }

    /// <summary>
    /// Определение среды исполнения: некий CI сервис вообще.
    /// </summary>
    [Pure]
    public static bool DetectCI()
    {
        return Environment.GetEnvironmentVariable ("CI").SameString ("True");
    }

    /// <summary>
    /// Определение среды исполнения: Travis CI.
    /// </summary>
    [Pure]
    public static bool DetectTravis()
    {
        return Environment.GetEnvironmentVariable ("TRAVIS").SameString ("True");
    }

    /// <summary>
    /// Определение среды исполнения: Github actions.
    /// </summary>
    [Pure]
    public static bool DetectGithubActions()
    {
        return Environment.GetEnvironmentVariable ("GITHUB_ACTIONS").SameString ("True");
    }

    /// <summary>
    /// Бросает исключение, если переданное значение пустое,
    /// иначе просто возвращает его.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ReadOnlyMemory<T> ThrowIfEmpty<T>
        (
            this ReadOnlyMemory<T> memory,
            [CallerArgumentExpression ("memory")] string? message = null
        )
    {
        if (memory.IsEmpty)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                Debug.WriteLine (nameof (Utility) + "::" + nameof (ThrowIfEmpty) + ": " + message);

                throw new ArgumentException (message);
            }

            Debug.WriteLine (nameof (Utility) + "::" + nameof (ThrowIfEmpty));

            throw new ArgumentException();
        }

        return memory;
    }

    /// <summary>
    /// Бросает исключение, если переданное значение пустое,
    /// иначе просто возвращает его.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ReadOnlySpan<T> ThrowIfEmpty<T>
        (
            this ReadOnlySpan<T> memory,
            [CallerArgumentExpression ("memory")] string? message = null
        )
    {
        if (memory.IsEmpty)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                Debug.WriteLine (nameof (Utility) + "::" + nameof (ThrowIfEmpty) + ": " + message);

                throw new ArgumentException (message);
            }

            Debug.WriteLine (nameof (Utility) + "::" + nameof (ThrowIfEmpty));

            throw new ArgumentException();
        }

        return memory;
    }

    /// <summary>
    /// Бросает исключение, если переданное значение равно <c>null</c>,
    /// иначе просто возвращает его.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static T ThrowIfNull<T>
        (
            this T? value,
            [CallerArgumentExpression ("value")] string? message = null
        )
        where T: class
    {
        if (value is null)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                Debug.WriteLine (nameof (Utility) + "::" + nameof (ThrowIfNull) + ": " + message);

                throw new ArgumentException (message);
            }

            Debug.WriteLine (nameof (Utility) + "::" + nameof (ThrowIfNull));

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Подстановка значения по умолчанию вместо <c>null</c>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static T IfNull<T>
        (
            this T? value,
            T defaultValue
        )
        where T: class
    {
        return value ?? defaultValue;
    }

    /// <summary>
    /// Подстановка значения по умолчания вместо пустой строки.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static string IfEmpty
        (
            this string? value,
            string defaultValue
        )
    {
        return string.IsNullOrEmpty (value)
            ? string.IsNullOrEmpty (defaultValue)
                ? throw new ArgumentNullException (nameof (defaultValue))
                : defaultValue
            : value;
    }

    /// <summary>
    /// Бросает исключение, если переданная строка пустая
    /// или равна <c>null</c>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static string ThrowIfNullOrEmpty
        (
            this string? value,
            [CallerArgumentExpression ("value")] string? argumentName = null
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            if (!string.IsNullOrEmpty (argumentName))
            {
                // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                throw new ArgumentException (argumentName);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если переданная строка пустая
    /// или равна <c>null</c>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ReadOnlySpan<char> ThrowIfNullOrEmpty
        (
            this ReadOnlySpan<char> value,
            [CallerArgumentExpression ("value")] string? argumentName = null
        )
    {
        if (value.IsEmpty)
        {
            if (!string.IsNullOrEmpty (argumentName))
            {
                // .NET 5 SDK подставляет в argumentName значение null, .NET 6 делает по-человечески
                throw new ArgumentException (argumentName);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если переданная строка пробельная
    /// или равна <c>null</c>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static string ThrowIfNullOrWhiteSpace
        (
            this string? value,
            [CallerArgumentExpression ("value")] string? name = null
        )
    {
        if (string.IsNullOrWhiteSpace (value))
        {
            if (!string.IsNullOrEmpty (name))
            {
                // .NET 5 SDK подставляет в name значение null, .NET 6 делает по-человечески
                throw new ArgumentException (name);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если переданная строка пробельная
    /// или равна <c>null</c>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ReadOnlySpan<char> ThrowIfNullOrWhiteSpace
        (
            this ReadOnlySpan<char> value,
            [CallerArgumentExpression ("value")] string? name = null
        )
    {
        if (value.IsEmpty || value.IsWhiteSpace())
        {
            if (!string.IsNullOrEmpty (name))
            {
                // .NET 5 SDK подставляет в name значение null, .NET 6 делает по-человечески
                throw new ArgumentException (name);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число равно нулю.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static int ThrowIfZero
        (
            this int value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value is 0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число равно нулю.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static uint ThrowIfZero
        (
            this uint value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value is 0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число равно нулю.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static short ThrowIfZero
        (
            this short value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value is 0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число равно нулю.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ushort ThrowIfZero
        (
            this ushort value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value is 0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число равно нулю.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static long ThrowIfZero
        (
            this long value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        return value == 0 ? throw new ArgumentException (message) : value;
    }

    /// <summary>
    /// Бросает исключение, если число равно нулю.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ulong ThrowIfZero
        (
            this ulong value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value is 0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число равно нулю.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static double ThrowIfZero
        (
            this double value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value is 0.0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число равно нулю.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static decimal ThrowIfZero
        (
            this decimal value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value is 0.0m)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число отрицательное.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static int ThrowIfNegative
        (
            this int value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < 0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число отрицательное.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static short ThrowIfNegative
        (
            this short value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < 0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число отрицательное.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static long ThrowIfNegative
        (
            this long value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < 0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число отрицательное.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static double ThrowIfNegative
        (
            this double value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < 0.0)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число отрицательное.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static decimal ThrowIfNegative
        (
            this decimal value,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < 0.0m)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число не попадает в указанный интервал.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static int ThrowIfOutOfTheRange
        (
            this int value,
            int minimum,
            int maximum,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < minimum || value > maximum)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число не попадает в указанный интервал.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static uint ThrowIfOutOfTheRange
        (
            this uint value,
            uint minimum,
            uint maximum,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < minimum || value > maximum)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число не попадает в указанный интервал.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static short ThrowIfOutOfTheRange
        (
            this short value,
            short minimum,
            short maximum,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < minimum || value > maximum)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число не попадает в указанный интервал.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ushort ThrowIfOutOfTheRange
        (
            this ushort value,
            ushort minimum,
            ushort maximum,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < minimum || value > maximum)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число не попадает в указанный интервал.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static long ThrowIfOutOfTheRange
        (
            this long value,
            long minimum,
            long maximum,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < minimum || value > maximum)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число не попадает в указанный интервал.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ulong ThrowIfOutOfTheRange
        (
            this ulong value,
            ulong minimum,
            ulong maximum,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < minimum || value > maximum)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число не попадает в указанный интервал.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static double ThrowIfOutOfTheRange
        (
            this double value,
            double minimum,
            double maximum,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < minimum || value > maximum)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Бросает исключение, если число не попадает в указанный интервал.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static decimal ThrowIfOutOfTheRange
        (
            this decimal value,
            decimal minimum,
            decimal maximum,
            [CallerArgumentExpression ("value")] string? message = null
        )
    {
        if (value < minimum || value > maximum)
        {
            if (!string.IsNullOrEmpty (message))
            {
                // .NET 5 SDK подставляет в message значение null, .NET 6 делает по-человечески
                throw new ArgumentException (message);
            }

            throw new ArgumentException();
        }

        return value;
    }

    /// <summary>
    /// Склеивание только непустых строк с разделителем.
    /// </summary>
    public static string JoinNonEmpty
        (
            string separator,
            string? first,
            string? second
        )
    {
        return string.IsNullOrEmpty (first)
            ? string.IsNullOrEmpty (second)
                ? string.Empty
                : second
            : string.IsNullOrEmpty (second)
                ? first
                : first + separator + second;
    }

    /// <summary>
    /// Склеивание только непустых строк с разделителем.
    /// </summary>
    public static string JoinNonEmpty
        (
            string separator,
            string? first,
            string? second,
            string? third
        )
    {
        var builder = new StringBuilder();

        builder
            .AppendWithDelimiter (first, separator)
            .AppendWithDelimiter (second, separator)
            .AppendWithDelimiter (third, separator);

        return builder.ToString();
    }

    /// <summary>
    /// Склеивание только непустых строк с разделителем.
    /// </summary>
    public static string JoinNonEmpty
        (
            string separator,
            string? first,
            string? second,
            string? third,
            string? fourth
        )
    {
        var builder = new StringBuilder();

        builder
            .AppendWithDelimiter (first, separator)
            .AppendWithDelimiter (second, separator)
            .AppendWithDelimiter (third, separator)
            .AppendWithDelimiter (fourth, separator);

        return builder.ToString();
    }

    /// <summary>
    /// Склеивание только непустых строк с разделителем.
    /// </summary>
    public static string JoinNonEmpty
        (
            string separator,
            IEnumerable<string> strings
        )
    {
        var builder = new StringBuilder();

        foreach (var t in strings)
        {
            builder.AppendWithDelimiter (t, separator);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Склеивание только непустых строк с разделителем.
    /// </summary>
    public static string JoinNonEmpty
        (
            string separator,
            params string?[] strings
        )
    {
        var builder = new StringBuilder();

        foreach (var t in strings)
        {
            builder.AppendWithDelimiter (t, separator);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Сравнение двух блоков памяти
    /// (интерпретируемых как символы Unicode).
    /// </summary>
    [Pure]
    public static int CompareOrdinal
        (
            ReadOnlyMemory<char> first,
            ReadOnlyMemory<char> second
        )
    {
        for (var i = 0;; i++)
        {
            if (i == first.Length)
            {
                return i == second.Length ? 0 : -1;
            }

            if (i == second.Length)
            {
                return 1;
            }

            var result = first.Span[i] - second.Span[i];
            if (result != 0)
            {
                return result;
            }
        }
    }

    /// <summary>
    /// Посимвольное сравнение двух блоков памяти
    /// (интерпретируемых как символы Unicode).
    /// </summary>
    [Pure]
    public static int CompareOrdinal
        (
            ReadOnlySpan<char> first,
            ReadOnlySpan<char> second
        )
    {
        for (var i = 0;; i++)
        {
            if (i == first.Length)
            {
                return i == second.Length ? 0 : -1;
            }

            if (i == second.Length)
            {
                return 1;
            }

            var result = first[i] - second[i];
            if (result != 0)
            {
                return result;
            }
        }
    }

    /// <summary>
    /// Ищет первое вхождение паттерна в данных.
    /// </summary>
    /// <returns>
    /// Индекс первого вхождения или -1.
    /// </returns>
    [Pure]
    public static int IndexOf
        (
            byte[] data,
            byte[] pattern,
            int start = 0
        )
    {
        var patternLength = pattern.Length;
        var dataLength = data.Length - patternLength;
        if (patternLength == 0 || dataLength < 0)
        {
            return -1;
        }

        for (var i = start; i <= dataLength; i++)
        {
            var found = true;
            for (var j = 0; j < patternLength; j++)
            {
                if (data[i + j] != pattern[j])
                {
                    found = false;
                    break;
                }
            }

            if (found)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Превращает объект в видимую строку.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static string ToVisibleString<T>
        (
            this T? value
        )
        where T : class
    {
        return value?.ToString() ?? "(null)";
    }

    /// <summary>
    /// Превращает блок памяти в видимую строку.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static string ToVisibleString
        (
            this ReadOnlyMemory<char> value
        )
    {
        return value.IsEmpty ? "(empty)" : value.ToString();
    }

    /// <summary>
    /// Превращает блок памяти в видимую строку.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static string ToVisibleString
        (
            this ReadOnlySpan<char> value
        )
    {
        return value.IsEmpty ? "(empty)" : value.ToString();
    }

    /// <summary>
    /// Сравнивает две строки независимо от текущей культуры.
    /// </summary>
    [Pure]
    public static bool SameInvariant
        (
            string? left,
            string? right
        )
    {
        return CultureInfo.InvariantCulture.CompareInfo.Compare (left, right) == 0;
    }

    /// <summary>
    /// Безопасное получение первого символа в строке.
    /// </summary>
    [Pure]
    public static char FirstChar (this string? text) =>
        string.IsNullOrEmpty (text) ? '\0' : text[0];

    /// <summary>
    /// Безопасное получение первого символа в строке.
    /// </summary>
    [Pure]
    public static char FirstChar (this ReadOnlyMemory<char> text) =>
        text.Length == 0 ? '\0' : text.Span[0];

    /// <summary>
    /// Безопасное получение первого символа в строке.
    /// </summary>
    [Pure]
    public static char FirstChar (this ReadOnlySpan<char> text) =>
        text.Length == 0 ? '\0' : text[0];

    /// <summary>
    /// Безопасное получение последнего символа в строке.
    /// </summary>
    [Pure]
    public static char LastChar
        (
            this string? text
        )
    {
        return string.IsNullOrEmpty (text) ? '\0' : text[^1];
    }

    /// <summary>
    /// Безопасное получение последнего символа в строке.
    /// </summary>
    [Pure]
    public static char LastChar
        (
            this ReadOnlySpan<char> text
        )
    {
        return text.Length == 0 ? '\0' : text[^1];
    }


    /// <summary>
    /// Сравнивает символы с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первый символ.</param>
    /// <param name="two">Второй символ.</param>
    /// <returns>Символы совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameChar (this char one, char two)
    {
        // общая рекомендация: для сравнения без учета регистра символов
        // преобразовывать к верхнему регистру
        return char.ToUpperInvariant (one) == char.ToUpperInvariant (two);
    }

    /// <summary>
    /// Сравнивает символы с точностью до регистра.
    /// </summary>
    /// <param name="one">Первый символ.</param>
    /// <param name="two">Второй символ.</param>
    /// <param name="three">Третий символ.</param>
    /// <returns>Символы совпадают с точностью до регистра.</returns>
    [Pure]
    public static bool SameChar
        (
            this char one,
            char two,
            char three
        )
    {
        // общая рекомендация: для сравнения без учета регистра символов
        // преобразовывать к верхнему регистру
        one = char.ToUpperInvariant (one);

        return one == char.ToUpperInvariant (two)
               || one == char.ToUpperInvariant (three);
    }

    /// <summary>
    /// Сравнивает символы с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первый символ.</param>
    /// <param name="two">Второй символ.</param>
    /// <param name="three">Третий символ.</param>
    /// <param name="four">Четвертый символ.</param>
    /// <returns>Символы совпадают с точностью до регистра.</returns>
    [Pure]
    public static bool SameChar
        (
            this char one,
            char two,
            char three,
            char four
        )
    {
        one = char.ToUpperInvariant (one);

        return one == char.ToUpperInvariant (two)
               || one == char.ToUpperInvariant (three)
               || one == char.ToUpperInvariant (four);
    }

    /// <summary>
    /// Сравнивает символы с точностью до регистра
    /// без учета текущей кульутры.
    /// </summary>
    /// <param name="one">Левая часть.</param>
    /// <param name="array">Правая часть.</param>
    /// <returns>Результат поиска <paramref name="one"/> среди
    /// элементов <paramref name="array"/>.</returns>
    [Pure]
    public static bool SameChar
        (
            this char one,
            params char[] array
        )
    {
        foreach (var two in array)
        {
            if (char.ToUpperInvariant (one) == char.ToUpperInvariant (two))
            {
                return true;
            }
        } // foreach

        return false;
    }

    /// <summary>
    /// Сравнивает символы с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Левая часть.</param>
    /// <param name="text">Правая часть.</param>
    /// <returns>Результат поиска <paramref name="one"/> среди
    /// элементов <paramref name="text"/>.</returns>
    [Pure]
    public static bool SameChar
        (
            this char one,
            IEnumerable<char> text
        )
    {
        foreach (var two in text)
        {
            if (char.ToUpperInvariant (one) == char.ToUpperInvariant (two))
            {
                return true;
            }
        } // foreach

        return false;
    }

    /// <summary>
    /// Сравнивает символы с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Левая часть.</param>
    /// <param name="text">Правая часть.</param>
    /// <returns>Результат поиска <paramref name="one"/> среди
    /// элементов <paramref name="text"/>.</returns>
    [Pure]
    public static bool SameChar
        (
            this char one,
            ReadOnlySpan<char> text
        )
    {
        foreach (var two in text)
        {
            if (char.ToUpperInvariant (one) == char.ToUpperInvariant (two))
            {
                return true;
            }
        } // foreach

        return false;
    }

    /// <summary>
    /// Сравнивает строки с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <returns>Строки совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameString (this string? one, string? two) =>
        string.Compare (one, two, StringComparison.OrdinalIgnoreCase) == 0;

    /// <summary>
    /// Сравнивает строки с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <returns>Строки совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameString (this ReadOnlyMemory<char> one, string? two) =>
        one.Span.CompareTo (two.AsSpan(), StringComparison.OrdinalIgnoreCase) == 0;

    /// <summary>
    /// Сравнивает строки с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <returns>Строки совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameString (this ReadOnlySpan<char> one, ReadOnlySpan<char> two) =>
        one.CompareTo (two, StringComparison.OrdinalIgnoreCase) == 0;

    /// <summary>
    /// Сравнивает строки с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <param name="three">Третья строка.</param>
    /// <returns>Строки совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameString (this string? one, string? two, string? three) =>
        string.Compare (one, two, StringComparison.OrdinalIgnoreCase) == 0
        || string.Compare (one, three, StringComparison.OrdinalIgnoreCase) == 0;

    /// <summary>
    /// Сравнивает строки с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <param name="three">Третья строка.</param>
    /// <returns>Строки совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameString (this ReadOnlyMemory<char> one, string? two, string? three) =>
        one.Span.CompareTo (two.AsSpan(), StringComparison.OrdinalIgnoreCase) == 0
        || one.Span.CompareTo (three.AsSpan(), StringComparison.OrdinalIgnoreCase) == 0;

    /// <summary>
    /// Сравнивает строки с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <param name="three">Третья строка.</param>
    /// <param name="four">Четвертая строка.</param>
    /// <returns>Строки совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameString
        (
            this string? one,
            string? two,
            string? three,
            string? four
        )
    {
        return string.Compare (one, two, StringComparison.OrdinalIgnoreCase) == 0
               || string.Compare (one, three, StringComparison.OrdinalIgnoreCase) == 0
               || string.Compare (one, four, StringComparison.OrdinalIgnoreCase) == 0;
    }

    /// <summary>
    /// Сравнивает строки с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="array">Строки для сопоставления.</param>
    /// <returns>Строки совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameString
        (
            this string? one,
            params string?[] array
        )
    {
        foreach (var two in array)
        {
            if (string.Compare
                    (
                        one,
                        two,
                        StringComparison.OrdinalIgnoreCase
                    ) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Сравнивает строки с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="strings">Строки для сопоставления.</param>
    /// <returns>Строки совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameString
        (
            this string? one,
            IEnumerable<string?> strings
        )
    {
        foreach (var two in strings)
        {
            if (string.Compare
                    (
                        one,
                        two,
                        StringComparison.OrdinalIgnoreCase
                    ) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Сравнивает строки с точностью до регистра
    /// без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="strings">Строки для сопоставления.</param>
    /// <returns>Строки совпадают с точностью до регистра?</returns>
    [Pure]
    public static bool SameString
        (
            this ReadOnlyMemory<char> one,
            IEnumerable<string?> strings
        )
    {
        foreach (var two in strings)
        {
            if (one.Span.CompareTo
                    (
                        two.AsSpan(),
                        StringComparison.OrdinalIgnoreCase
                    )
                == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Сравнивает строки с учетом регистра символов,
    /// но без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <returns>Строки совпадают?</returns>
    [Pure]
    public static bool SameStringSensitive (this string? one, string? two) =>
        string.Compare (one, two, StringComparison.Ordinal) == 0;

    /// <summary>
    /// Сравнивает строки с учетом регистра символов,
    /// но без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <returns>Строки совпадают?</returns>
    [Pure]
    public static bool SameStringSensitive (this ReadOnlyMemory<char> one, ReadOnlyMemory<char> two) =>
        one.Span.CompareTo (two.Span, StringComparison.Ordinal) == 0;

    /// <summary>
    /// Сравнивает строки с учетом регистра символов,
    /// но без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <param name="three">Третья строка.</param>
    /// <returns>Строки совпадают?</returns>
    [Pure]
    public static bool SameStringSensitive
        (
            this string? one,
            string? two,
            string? three
        )
    {
        return string.Compare (one, two, StringComparison.Ordinal) == 0
               || string.Compare (one, three, StringComparison.Ordinal) == 0;
    }

    /// <summary>
    /// Сравнивает строки с учетом регистра символов,
    /// но без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="two">Вторая строка.</param>
    /// <param name="three">Третья строка.</param>
    /// <param name="four">Четвертая строка.</param>
    /// <returns>Строки совпадают?</returns>
    [Pure]
    public static bool SameStringSensitive
        (
            this string? one,
            string? two,
            string? three,
            string? four
        )
    {
        return string.Compare
                   (
                       one,
                       two,
                       StringComparison.OrdinalIgnoreCase
                   ) == 0
               || string.Compare
                   (
                       one,
                       three,
                       StringComparison.Ordinal
                   ) == 0
               || string.Compare
                   (
                       one,
                       four,
                       StringComparison.Ordinal
                   ) == 0;
    }

    /// <summary>
    /// Сравнивает строки с учетом регистра символов,
    /// но без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="array">Строки для сопоставления.</param>
    /// <returns>Строки совпадают?</returns>
    [Pure]
    public static bool SameStringSensitive
        (
            this string? one,
            params string?[] array
        )
    {
        foreach (var two in array)
        {
            if (string.Compare
                    (
                        one,
                        two,
                        StringComparison.Ordinal
                    ) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Сравнивает строки с учетом регистра символов,
    /// но без учета текущей культуры.
    /// </summary>
    /// <param name="one">Первая строка.</param>
    /// <param name="strings">Строки для сопоставления.</param>
    /// <returns>Строки совпадают?</returns>
    [Pure]
    public static bool SameStringSensitive
        (
            this string? one,
            IEnumerable<string?> strings
        )
    {
        foreach (var two in strings)
        {
            if (string.Compare
                    (
                        one,
                        two,
                        StringComparison.Ordinal
                    ) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Сравнение двух строк.
    /// </summary>
    [Pure]
    public static int SameCompare
        (
            this string? first,
            string? second
        )
    {
        if (first is null)
        {
            return second is null ? 0 : -1;
        }

        if (second is null)
        {
            return 1;
        }

        return string.Compare (first, second, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// Format a double using the local culture currency settings.
    /// </summary>
    /// <param name="value">The double to be formatted.</param>
    /// <param name="culture">(Optional) If specified, uses the specified
    /// cultures currency format. Otherwise uses machines current culture.</param>
    /// <returns>The double formatted based on the local culture currency settings.</returns>
    [Pure]
    public static string ToLocalCurrencyString
        (
            this double value,
            CultureInfo? culture = null
        )
    {
        culture ??= CultureInfo.CurrentCulture;

        return value.ToString ("c", culture);
    }

    /// <summary>
    /// Format a decimal using the local culture currency settings.
    /// </summary>
    /// <param name="value">The double to be formatted.</param>
    /// <param name="culture">(Optional) If specified, uses the specified
    /// cultures currency format. Otherwise uses machines current culture.</param>
    /// <returns>The double formatted based on the local culture currency settings.</returns>
    [Pure]
    public static string ToLocalCurrencyString
        (
            this decimal value,
            CultureInfo? culture = null
        )
    {
        culture ??= CultureInfo.CurrentCulture;

        return value.ToString ("c", culture);
    }

    /// <summary>
    /// 	Ensures that a string starts with a given prefix.
    /// </summary>
    /// <param name = "value">The string value to check.</param>
    /// <param name = "prefix">The prefix value to check for.</param>
    /// <returns>The string value including the prefix</returns>
    /// <example>
    /// 	<code>
    /// 		var extension = "txt";
    /// 		var fileName = string.Concat(file.Name, extension.EnsureStartsWith("."));
    /// 	</code>
    /// </example>
    [Pure]
    public static string EnsureStartsWith
        (
            this string value,
            string prefix
        )
    {
        return value.StartsWith (prefix) ? value : value.Insert(0, prefix);
    }

    /// <summary>
    /// 	Ensures that a string ends with a given suffix.
    /// </summary>
    /// <param name = "value">The string value to check.</param>
    /// <param name = "suffix">The suffix value to check for.</param>
    /// <returns>The string value including the suffix</returns>
    /// <example>
    /// 	<code>
    /// 		var url = "http://www.pgk.de";
    /// 		url = url.EnsureEndsWith("/"));
    /// 	</code>
    /// </example>
    [Pure]
    public static string EnsureEndsWith
        (
            this string value,
            string suffix
        )
    {
        return value.EndsWith(suffix) ? value : value.Insert(value.Length, suffix);
    }

    /// <summary>
    /// Returns characters from left of specified length
    /// </summary>
    /// <param name="value">String value</param>
    /// <param name="length">Max number of charaters to return</param>
    /// <returns>Returns string from left</returns>
    [Pure]
    public static string? Left
        (
            this string? value,
            int length
        )
    {
        return value is not null && value.Length > length
            ? value.Substring (0, length)
            : value;
    }

    /// <summary>
    /// Returns characters from right of specified length
    /// </summary>
    /// <param name="value">String value</param>
    /// <param name="length">Max number of charaters to return</param>
    /// <returns>Returns string from right</returns>
    [Pure]
    public static string? Right
        (
            this string? value,
            int length
        )
    {
        return value is not null && value.Length > length
            ? value.Substring (value.Length - length)
            : value;
    }

    /// <summary>
    /// Преобразование произвольного значения в строку
    /// по правилам инвариантной культуры.
    /// </summary>
    public static string ToInvariantString
        (
            this object? value
        )
    {
        return value switch
        {
            bool val => val.ToInvariantString(),
            double val => val.ToInvariantString(),
            int val => val.ToInvariantString(),
            long val => val.ToInvariantString(),
            TimeSpan val => val.ToInvariantString(),
            _ => Convert.ToString (value, CultureInfo.InvariantCulture)!,
        };
    }

    /// <summary>
    /// Преобразование промежутка времени в строку по правилам
    /// инвариантной культуры.
    /// </summary>
    public static string ToInvariantString
        (
            this TimeSpan value
        )
    {
        return value.ToString ("c", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование логического значения в "true" в "false".
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this bool value
        )
    {
        return value ? "true" : "false";
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    /// <param name="value">Число для преобразования.</param>
    /// <returns>Строковое представление числа.</returns>
    [Pure]
    public static string ToInvariantString
        (
            this short value
        )
    {
        return value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this short value,
            string format
        )
    {
        return value.ToString (format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    /// <param name="value">Число для преобразования.</param>
    /// <returns>Строковое представление числа.</returns>
    [Pure]
    public static string ToInvariantString
        (
            this ushort value
        )
    {
        return value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this ushort value,
            string format
        )
    {
        return value.ToString (format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    /// <param name="value">Число для преобразования.</param>
    /// <returns>Строковое представление числа.</returns>
    [Pure]
    public static string ToInvariantString
        (
            this int value
        )
    {
        return value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this int value,
            string format
        )
    {
        return value.ToString (format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    /// <param name="value">Число для преобразования.</param>
    /// <returns>Строковое представление числа.</returns>
    [Pure]
    public static string ToInvariantString
        (
            this uint value
        )
    {
        return value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this uint value,
            string format
        )
    {
        return value.ToString (format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    /// <param name="value">Число для преобразования.</param>
    /// <returns>Строковое представление числа.</returns>
    [Pure]
    public static string ToInvariantString
        (
            this long value
        )
    {
        return value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this long value,
            string format
        )
    {
        return value.ToString (format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    /// <param name="value">Число для преобразования.</param>
    /// <returns>Строковое представление числа.</returns>
    [Pure]
    public static string ToInvariantString
        (
            this ulong value
        )
    {
        return value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this ulong value,
            string format
        )
    {
        return value.ToString (format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this float value
        )
    {
        return value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this float value,
            string format
        )
    {
        return value.ToString (format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this double value
        )
    {
        return value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this double value,
            string format
        )
    {
        return value.ToString (format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this decimal value
        )
    {
        return value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Преобразование числа в строку по правилам инвариантной
    /// (не зависящей от региона) культуры.
    /// </summary>
    [Pure]
    public static string ToInvariantString
        (
            this decimal value,
            string format
        )
    {
        return value.ToString (format, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Convert a <see cref="IEnumerable{T}"/> to a <see cref="ObservableCollection{T}"/>
    /// and can be used in XAML projects.
    /// </summary>
    public static ObservableCollection<T> ToObservableCollection<T>
        (
            this IEnumerable<T> enumerable
        )
    {
        var result = new ObservableCollection<T>();
        foreach (var item in enumerable)
        {
            result.Add(item);
        }

        return result;
    }

    /// <summary>
    /// Register required encoding providers.
    /// </summary>
    public static void RegisterEncodingProviders()
    {
        Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);
    }

    /// <summary>
    /// Безопасное преобразование строки в целое.
    /// </summary>
    public static int SafeToInt32
        (
            this string? text,
            int defaultValue,
            int minValue,
            int maxValue
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return defaultValue;
        }

        if (!int.TryParse (text, out var result))
        {
            result = defaultValue;
        }

        if (result < minValue
            || result > maxValue)
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в целое.
    /// </summary>
    public static int SafeToInt32
        (
            this string? text,
            int defaultValue
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return defaultValue;
        }

        if (!int.TryParse (text, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в целое.
    /// </summary>
    public static int SafeToInt32
        (
            this string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return 0;
        }

        if (!int.TryParse (text, out var result))
        {
            result = 0;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в целое.
    /// </summary>
    public static int SafeToInt32
        (
            this ReadOnlyMemory<char> text
        )
    {
        if (text.IsEmpty)
        {
            return 0;
        }

        if (!int.TryParse (text.Span, out var result))
        {
            result = 0;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в целое.
    /// </summary>
    public static int SafeToInt32
        (
            this ReadOnlySpan<char> text
        )
    {
        if (text.IsEmpty)
        {
            return 0;
        }

        if (!int.TryParse (text, out var result))
        {
            result = 0;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в целое.
    /// </summary>
    public static long SafeToInt64
        (
            this string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return 0;
        }

        if (!long.TryParse (text, out var result))
        {
            result = 0;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в целое.
    /// </summary>
    public static long SafeToInt64
        (
            this ReadOnlyMemory<char> text
        )
    {
        if (text.IsEmpty)
        {
            return 0;
        }

        if (!long.TryParse (text.Span, out var result))
        {
            result = 0;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в целое.
    /// </summary>
    public static long SafeToInt64
        (
            this ReadOnlySpan<char> text
        )
    {
        if (text.IsEmpty)
        {
            return 0;
        }

        if (!long.TryParse (text, out var result))
        {
            result = 0;
        }

        return result;
    }

    /// <summary>
    /// Перегрузка для Барсика.
    /// </summary>
    public static double StringSafeToDouble
        (
            this string? text
        )
    {
        return SafeToDouble (text);
    }

    /// <summary>
    /// Безопасное преобразование строки
    /// в число с плавающей точкой.
    /// </summary>
    public static double SafeToDouble
        (
            this string? text,
            double defaultValue = default
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return defaultValue;
        }

        if (!TryParseDouble (text, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки
    /// в число с плавающей точкой.
    /// </summary>
    public static double SafeToDouble
        (
            this ReadOnlyMemory<char> text,
            double defaultValue = default
        )
    {
        if (text.IsEmpty)
        {
            return defaultValue;
        }

        if (!TryParseDouble (text.Span, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки
    /// в число с плавающей точкой.
    /// </summary>
    public static double SafeToDouble
        (
            this ReadOnlySpan<char> text,
            double defaultValue = default
        )
    {
        if (text.IsEmpty)
        {
            return defaultValue;
        }

        if (!TryParseDouble (text, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в денежный тип.
    /// </summary>
    public static decimal SafeToDecimal
        (
            this string? text,
            decimal defaultValue = default
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return defaultValue;
        }

        if (!decimal.TryParse (text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в денежный тип.
    /// </summary>
    public static decimal SafeToDecimal
        (
            this ReadOnlyMemory<char> text,
            decimal defaultValue = default
        )
    {
        if (text.IsEmpty)
        {
            return defaultValue;
        }

        if (!decimal.TryParse (text.Span, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Безопасное преобразование строки в денежный тип.
    /// </summary>
    public static decimal SafeToDecimal
        (
            this ReadOnlySpan<char> text,
            decimal defaultValue = default
        )
    {
        if (text.IsEmpty)
        {
            return defaultValue;
        }

        if (!decimal.TryParse (text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static short ParseInt16
        (
            this string text
        )
    {
        return short.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static short ParseInt16
        (
            this ReadOnlyMemory<char> text
        )
    {
        return short.Parse
            (
                text.Span,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static short ParseInt16
        (
            this ReadOnlySpan<char> text
        )
    {
        return short.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static int ParseInt32
        (
            this string text
        )
    {
        return int.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static int ParseInt32
        (
            this ReadOnlyMemory<char> text
        )
    {
        return int.Parse
            (
                text.Span,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static int ParseInt32
        (
            this ReadOnlySpan<char> text
        )
    {
        return int.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static long ParseInt64
        (
            this string text
        )
    {
        return long.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static long ParseInt64
        (
            this ReadOnlyMemory<char> text
        )
    {
        return long.Parse
            (
                text.Span,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static long ParseInt64
        (
            this ReadOnlySpan<char> text
        )
    {
        return long.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ushort ParseUInt16
        (
            this string text
        )
    {
        return ushort.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ushort ParseUInt16
        (
            this ReadOnlySpan<char> text
        )
    {
        return ushort.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static uint ParseUInt32
        (
            this string text
        )
    {
        return uint.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static uint ParseUInt32
        (
            this ReadOnlySpan<char> text
        )
    {
        return uint.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ulong ParseUInt64
        (
            this string text
        )
    {
        return ulong.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static ulong ParseUInt64
        (
            this ReadOnlySpan<char> text
        )
    {
        return ulong.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static float ParseSingle
        (
            this string text
        )
    {
        return float.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static float ParseSingle
        (
            this ReadOnlySpan<char> text
        )
    {
        return float.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static double ParseDouble
        (
            this string text
        )
    {
        return double.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static double ParseDouble
        (
            this ReadOnlySpan<char> text
        )
    {
        return double.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static decimal ParseDecimal
        (
            this string text
        )
    {
        return decimal.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для Parse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static decimal ParseDecimal
        (
            this ReadOnlySpan<char> text
        )
    {
        return decimal.Parse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseInt16
        (
            string? text,
            out short result
        )
    {
        return short.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseInt16
        (
            ReadOnlySpan<char> text,
            out short result
        )
    {
        return short.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseInt32
        (
            string? text,
            out int result
        )
    {
        return int.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseInt32
        (
            ReadOnlySpan<char> text,
            out int result
        )
    {
        return int.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseInt64
        (
            string? text,
            out long result
        )
    {
        return long.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseInt64
        (
            ReadOnlySpan<char> text,
            out long result
        )
    {
        return long.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseUInt16
        (
            string? text,
            out ushort result
        )
    {
        return ushort.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseUInt16
        (
            ReadOnlySpan<char> text,
            out ushort result
        )
    {
        return ushort.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseUInt32
        (
            string? text,
            out uint result
        )
    {
        return uint.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseUInt32
        (
            ReadOnlySpan<char> text,
            out uint result
        )
    {
        return uint.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseUInt64
        (
            string? text,
            out ulong result
        )
    {
        return ulong.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseUInt64
        (
            ReadOnlySpan<char> text,
            out ulong result
        )
    {
        return ulong.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseDouble
        (
            string? text,
            out double result
        )
    {
        return double.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseDouble
        (
            ReadOnlySpan<char> text,
            out double result
        )
    {
        return double.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseSingle
        (
            string? text,
            out float result
        )
    {
        return float.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool TryParseSingle
        (
            ReadOnlySpan<char> text,
            out float result
        )
    {
        return float.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static bool TryParseDecimal
        (
            string? text,
            out decimal result
        )
    {
        return decimal.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static bool TryParseDecimal
        (
            ReadOnlySpan<char> text,
            out decimal result
        )
    {
        return decimal.TryParse
            (
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static bool TryParseDateTime
        (
            string? text,
            out DateTime result
        )
    {
        return DateTime.TryParse
            (
                text,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static bool TryParseDateTime
        (
            ReadOnlySpan<char> text,
            out DateTime result
        )
    {
        return DateTime.TryParse
            (
                text,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static bool TryParseDateTime
        (
            string? text,
            string? format,
            out DateTime result
        )
    {
        return DateTime.TryParseExact
            (
                text,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out result
            );
    }

    /// <summary>
    /// Сокращение для TryParse с <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    public static bool TryParseDateTime
        (
            ReadOnlySpan<char> text,
            ReadOnlySpan<char> format,
            out DateTime result
        )
    {
        return DateTime.TryParseExact
            (
                text,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AllowWhiteSpaces,
                out result
            );
    }

    /// <summary>
    /// Определяет, равен ли ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf
        (
            this string? value,
            string? first,
            string? second
        )
    {
        return !string.IsNullOrEmpty (value) &&
               (
                    string.CompareOrdinal (value, first) == 0
                   || string.CompareOrdinal (value, second) == 0
               );
    }

    /// <summary>
    /// Определяет, равен ли ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf<T>
        (
            this T value,
            T first,
            T second
        )
        where T : IComparable<T>
    {
        return value.CompareTo (first) == 0
               || value.CompareTo (second) == 0;
    }

    /// <summary>
    /// Определяет, равен ли ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf
        (
            this string? value,
            string? first,
            string? second,
            string? third
        )
    {
        return !string.IsNullOrEmpty (value) &&
               (
                   string.CompareOrdinal (value, first) == 0
                   || string.CompareOrdinal (value, second) == 0
                   || string.CompareOrdinal (value, third) == 0
               );
    }

    /// <summary>
    /// Определяет, равен ли ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf<T> (this T value, T first, T second,
        T third)
        where T : IComparable<T> =>
        value.CompareTo (first) == 0
        || value.CompareTo (second) == 0
        || value.CompareTo (third) == 0;

    /// <summary>
    /// Определяет, равен ли ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf
        (
            this string? value,
            string? first,
            string? second,
            string? third,
            string? fourth
        )
    {
        return !string.IsNullOrEmpty (value)
               && (string.CompareOrdinal (value, first) == 0
                   || string.CompareOrdinal (value, second) == 0
                   || string.CompareOrdinal (value, third) == 0
                   || string.CompareOrdinal (value, fourth) == 0);
    }

    /// <summary>
    /// Определяет, равен ли ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf<T>
        (
            this T value,
            T first,
            T second,
            T third,
            T fourth
        )
        where T : IComparable<T>
    {
        return value.CompareTo (first) == 0
               || value.CompareTo (second) == 0
               || value.CompareTo (third) == 0
               || value.CompareTo (fourth) == 0;
    }

    /// <summary>
    /// Определяет, равен ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf
        (
            this string? value,
            params string?[] array
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return false;
        }

        foreach (var one in array)
        {
            if (string.CompareOrdinal (value, one) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Определяет, равен ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf<T>
        (
            this T value,
            params T[] array
        )
        where T : IComparable<T>
    {
        foreach (var one in array)
        {
            if (value.CompareTo (one) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Определяет, равен ли ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf
        (
            this ReadOnlyMemory<char> value,
            string? first
        )
    {
        return value.Span.CompareTo (first.AsSpan(), StringComparison.Ordinal) == 0;
    }

    /// <summary>
    /// Определяет, равен ли ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf
        (
            this ReadOnlyMemory<char> value,
            string? first,
            string? second
        )
    {
        return value.Span.CompareTo (first.AsSpan(), StringComparison.Ordinal) == 0
               || value.Span.CompareTo (second.AsSpan(), StringComparison.Ordinal) == 0;
    }

    /// <summary>
    /// Определяет, равен ли ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf
        (
            this ReadOnlyMemory<char> value,
            string? first,
            string? second,
            string? third
        )
    {
        return value.Span.CompareTo (first.AsSpan(), StringComparison.Ordinal) == 0
               || value.Span.CompareTo (second.AsSpan(), StringComparison.Ordinal) == 0
               || value.Span.CompareTo (third.AsSpan(), StringComparison.Ordinal) == 0;
    }

    /// <summary>
    /// Определяет, равен ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf
        (
            this ReadOnlyMemory<char> value,
            params string?[] array
        )
    {
        foreach (var one in array)
        {
            if (value.Span.CompareTo (one.AsSpan(), StringComparison.Ordinal) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Безопасный доступ по индексу.
    /// </summary>
    [Pure]
    public static T? SafeAt<T>
        (
            this IList<T?> items,
            int index,
            T? defaultValue = default
        )
    {
        return index < 0 || index >= items.Count ? defaultValue : items[index];
    }

    /// <summary>
    /// Безопасный доступ по индексу.
    /// </summary>
    [Pure]
    public static T? SafeAt<T>
        (
            this T?[]? items,
            int index,
            T? defaultValue = default
        )
    {
        if (items is null || index < 0 || index >= items.Length)
        {
            return defaultValue;
        }

        return items[index];
    }

    /// <summary>
    /// Определяет, равен ли данный объект
    /// любому из перечисленных.
    /// </summary>
    [Pure]
    public static bool IsOneOf<T>
        (
            this T value,
            IEnumerable<T> items
        )
        where T : IComparable<T>
    {
        foreach (var one in items)
        {
            if (value.CompareTo (one) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Converts empty string to <c>null</c>.
    /// </summary>
    [Pure]
    public static string? EmptyToNull
        (
            this string? value
        )
    {
        return string.IsNullOrEmpty (value) ? null : value;
    }

    /// <summary>
    /// Converts empty string to <c>null</c>.
    /// </summary>
    [Pure]
    public static string? EmptyToNull
        (
            this ReadOnlySpan<char> value
        )
    {
        return value.IsEmpty ? null : value.ToString();
    }

    /// <summary>
    /// Converts empty string to <c>null</c>.
    /// </summary>
    [Pure]
    public static string? EmptyToNull
        (
            this ReadOnlyMemory<char> value
        )
    {
        return value.IsEmpty ? null : value.ToString();
    }

    /// <summary>
    /// Добавление списка по принципу "первый, второй и последний".
    /// </summary>
    public static StringBuilder AppendList
        (
            this StringBuilder builder,
            IList? list,
            string? separator = ", ",
            string? union = " and "
        )
    {
        Sure.NotNull (builder);

        if (list is null)
        {
            return builder;
        }

        var count = list.Count;
        for (var index = 0; index < count; index++)
        {
            if (index == count - 1 && count > 1)
            {
                builder.Append (union);
            }
            else if (index != 0)
            {
                builder.Append (separator);
            }

            builder.Append (list[index]);
        }

        return builder;
    }

    /// <summary>
    /// Добавление списка по принципу "первый, второй и последний".
    /// </summary>
    public static StringBuilder AppendList<T>
        (
            this StringBuilder builder,
            IList<T>? list,
            Func<T, string> transformator,
            string? separator = ", ",
            string? union = " and "
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (transformator);

        if (list is null)
        {
            return builder;
        }

        var count = list.Count;
        for (var index = 0; index < count; index++)
        {
            if (index == count - 1 && count > 1)
            {
                builder.Append (union);
            }
            else if (index != 0)
            {
                builder.Append (separator);
            }

            var transformed = transformator (list[index]);
            builder.Append (transformed);
        }

        return builder;
    }

    /// <summary>
    /// Добавление объекта-перечисления.
    /// </summary>
    public static StringBuilder AppendEnumerable
        (
            this StringBuilder builder,
            IEnumerable? obj,
            string? delimiter = ", ",
            string? open = "[",
            string? close = "]"
        )
    {
        if (obj is null)
        {
            builder.Append ("(null)");
            return builder;
        }

        var first = true;
        builder.Append (open);
        foreach (var item in obj)
        {
            if (!first)
            {
                builder.Append (delimiter);
            }

            builder.Append (item.ToVisibleString());
        }

        builder.Append (close);

        return builder;
    }

    /// <summary>
    /// Добавление объекта, предваренного разделителем.
    /// </summary>
    public static StringBuilder AppendWithDelimiter
        (
            this StringBuilder builder,
            object? obj,
            string? delimiter = ", "
        )
    {
        if (obj is not null)
        {
            if (builder.Length != 0)
            {
                builder.Append (delimiter);
            }

            if (obj is IFormattable formattable)
            {
                builder.Append (formattable.ToString (null, CultureInfo.InvariantCulture));
            }
            else
            {
                builder.Append (obj);
            }
        }

        return builder;
    }

    /// <summary>
    /// Добавление разделенных между собой элементов.
    /// </summary>
    public static StringBuilder AppendWithSeparator
        (
            this StringBuilder builder,
            string? separator,
            string? item1,
            string? item2
        )
    {
        var separate = false;

        if (!string.IsNullOrEmpty (item1))
        {
            builder.Append (item1);
            separate = true;
        }

        if (!string.IsNullOrEmpty (item2))
        {
            if (separate)
            {
                builder.Append (separator);
            }

            builder.Append (item2);
        }

        return builder;
    }

    /// <summary>
    /// Добавление разделенных между собой элементов.
    /// </summary>
    public static StringBuilder AppendWithSeparator
        (
            this StringBuilder builder,
            string? separator,
            string? item1,
            string? item2,
            string? item3
        )
    {
        var separate = false;

        if (!string.IsNullOrEmpty (item1))
        {
            builder.Append (item1);
            separate = true;
        }

        if (!string.IsNullOrEmpty (item2))
        {
            if (separate)
            {
                builder.Append (separator);
            }

            builder.Append (item2);
            separate = true;
        }

        if (!string.IsNullOrEmpty (item3))
        {
            if (separate)
            {
                builder.Append (separator);
            }

            builder.Append (item3);
        }

        return builder;
    }

    /// <summary>
    /// Добавление разделенных между собой элементов.
    /// </summary>
    public static StringBuilder AppendWithSeparator
        (
            this StringBuilder builder,
            string? separator,
            IEnumerable<string?> items
        )
    {
        var separate = false;

        foreach (var item in items)
        {
            if (separate)
            {
                builder.Append (separator);
            }

            if (!string.IsNullOrEmpty (item))
            {
                builder.Append (item);
                separate = true;
            }
        }

        return builder;
    }

    /// <summary>
    /// Добавление объекта, заключенного в скобки.
    /// </summary>
    public static StringBuilder AppendWithBrackets
        (
            this StringBuilder builder,
            object? obj,
            string? open = " (",
            string? close = ")"
        )
    {
        if (obj is not null)
        {
            builder.Append (open);

            if (obj is IFormattable formattable)
            {
                builder.Append (formattable.ToString (null, CultureInfo.InvariantCulture));
            }
            else
            {
                builder.Append (obj);
            }

            builder.Append (close);
        }

        return builder;
    }

    /// <summary>
    /// Добавление текста с префиксом.
    /// </summary>
    public static StringBuilder AppendWithPrefix
        (
            this StringBuilder builder,
            string? text,
            string? prefix
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.Append (prefix);
            builder.Append (text);
        }

        return builder;
    }

    /// <summary>
    /// Добавление текста с суффиксом.
    /// </summary>
    public static StringBuilder AppendWithSuffix
        (
            this StringBuilder builder,
            string? text,
            string? suffix
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.Append (text);
            builder.Append (suffix);
        }

        return builder;
    }

    /// <summary>
    /// Добавление текста с префиксом и суффиксом.
    /// </summary>
    public static StringBuilder AppendWithPrefixAndSuffix
        (
            this StringBuilder builder,
            string? text,
            string? prefix,
            string? suffix
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.Append (prefix);
            builder.Append (text);
            builder.Append (suffix);
        }

        return builder;
    }

    /// <summary>
    /// Determines whether given value can be converted to
    /// the specified type.
    /// </summary>
    /// <param name="value">Value to be converted.</param>
    /// <returns>
    /// <c>true</c> if value can be converted;
    /// otherwise, <c>false</c>.
    /// </returns>
    public static bool CanConvertTo<T>
        (
            object? value
        )
    {
        if (!ReferenceEquals (value, null))
        {
            var sourceType = value.GetType();
            var targetType = typeof (T);

            if (ReferenceEquals (targetType, sourceType))
            {
                return true;
            }

            if (targetType.IsAssignableFrom (sourceType))
            {
                return true;
            }

            var convertible = value as IConvertible;
            if (!ReferenceEquals (convertible, null))
            {
                return true; // ???
            }

            var converterFrom = TypeDescriptor.GetConverter (value);
            if (converterFrom.CanConvertTo (targetType))
            {
                return true;
            }

            var converterTo = TypeDescriptor.GetConverter (targetType);
            if (converterTo.CanConvertFrom (sourceType))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Converts given value to the specified type.
    /// </summary>
    /// <param name="value">The value to be converted.</param>
    /// <returns>Converted value.</returns>
    public static T ConvertTo<T>
        (
            object? value
        )
    {
        if (ReferenceEquals (value, null))
        {
            return default!;
        }

        var sourceType = value.GetType();
        var targetType = typeof (T);

        if (targetType == typeof (string))
        {
            return (T)(object)value.ToString()!;
        }

        if (targetType.IsAssignableFrom (sourceType))
        {
            return (T)value;
        }

        if (value is IConvertible)
        {
            return (T)Convert.ChangeType (value, targetType);
        }

        var converterFrom = TypeDescriptor.GetConverter (value);
        if (converterFrom.CanConvertTo (targetType))
        {
            return (T)converterFrom.ConvertTo
                (
                    value,
                    targetType
                )!;
        }

        var converterTo = TypeDescriptor.GetConverter (targetType);
        if (converterTo.CanConvertFrom (sourceType))
        {
            return (T)converterTo.ConvertFrom (value)!;
        }

        throw new Exception();
    }

    /// <summary>
    /// Converts given object to boolean value.
    /// </summary>
    /// <param name="value">Object to be converted.</param>
    /// <returns>Converted value.</returns>
    /// <exception cref="FormatException">
    /// Value can't be converted.
    /// </exception>
    public static bool ToBoolean
        (
            object value
        )
    {
        if (value is bool retval1)
        {
            return retval1;
        }

        if (value is string text)
        {
            if (bool.TryParse (text, out var retval2))
            {
                return retval2;
            }

            text = text.ToLowerInvariant();

            switch (text)
            {
                case "false" or "0" or "no" or "n" or "off"
                    or "negative" or "neg" or "disabled" or "incorrect"
                    or "wrong" or "нет":
                    return false;

                case "true" or "1" or "yes" or "y" or "on"
                    or "positiva" or "pos" or "enabled" or "correct"
                    or "right" or "да":
                    return true;
            }
        }

        if (value is IConvertible)
        {
            return Convert.ToBoolean (value);
        }

        var converterFrom = TypeDescriptor.GetConverter (value);
        if (converterFrom.CanConvertTo (typeof (bool)))
        {
            var converted = converterFrom.ConvertTo
                (
                    value,
                    typeof (bool)
                );

            return converted is not null && (bool)converted;
        }

        Debug.WriteLine
            (
                nameof (Utility) + "::" + nameof (ToBoolean)
                + "bad value=" + value
            );

        throw new FormatException
            (
                "Bad value: " + value
            );
    }

    /// <summary>
    /// Raises the specified handler.
    /// </summary>
    public static void Raise<T>
        (
            this EventHandler<T>? handler,
            object? sender,
            T args
        )
        where T : EventArgs
    {
        handler?.Invoke (sender, args);
    }

    /// <summary>
    /// Raises the specified handler.
    /// </summary>
    public static void Raise<T>
        (
            this EventHandler<T>? handler,
            object? sender
        )
        where T : EventArgs
    {
        handler?.Invoke (sender, null!);
    }

    /// <summary>
    /// Raises the specified handler.
    /// </summary>
    public static void Raise
        (
            this EventHandler? handler,
            object? sender
        )
    {
        handler?.Invoke (sender, EventArgs.Empty);
    }

    /// <summary>
    /// Raises the specified handler.
    /// </summary>
    public static Task RaiseAsync
        (
            this EventHandler? handler,
            object? sender,
            EventArgs args
        )
    {
        return handler is null
            ? Task.CompletedTask
            : Task.Factory.StartNew (() => { handler.Invoke (sender, args); });
    }

    /// <summary>
    /// Raises the specified handler.
    /// </summary>
    public static Task RaiseAsync
        (
            this EventHandler? handler,
            object? sender
        )
    {
        if (handler is null)
        {
            return Task.CompletedTask;
        }

        return Task.Factory.StartNew (() => { handler.Invoke (sender, EventArgs.Empty); });
    }

    /// <summary>
    /// Is zero-length time span?
    /// </summary>
    [Pure]
    public static bool IsZero
        (
            this TimeSpan timeSpan
        )
    {
        return TimeSpan.Compare (timeSpan, TimeSpan.Zero) == 0;
    }

    /// <summary>
    /// Is zero-length or less?
    /// </summary>
    [Pure]
    public static bool IsZeroOrLess
        (
            this TimeSpan timeSpan
        )
    {
        return TimeSpan.Compare (timeSpan, TimeSpan.Zero) <= 0;
    }

    /// <summary>
    /// Is length of the time span less than zero?
    /// </summary>
    [Pure]
    public static bool LessThanZero
        (
            this TimeSpan timeSpan
        )
    {
        return TimeSpan.Compare (timeSpan, TimeSpan.Zero) < 0;
    }

    /// <summary>
    /// Converts time span to string
    /// automatically selecting format
    /// according duration of the span.
    /// </summary>
    [Pure]
    public static string ToAutoString
        (
            this TimeSpan span
        )
    {
        if (span >= OneDay)
        {
            return span.ToDayString();
        }

        if (span >= OneHour)
        {
            return span.ToHourString();
        }

        if (span >= OneMinute)
        {
            return span.ToMinuteString();
        }

        return span.ToSecondString();
    }

    /// <summary>
    /// Converts time span using format 'dd:hh:mm:ss'
    /// </summary>
    [Pure]
    public static string ToDayString
        (
            this TimeSpan span
        )
    {
        return string.Format
            (
                CultureInfo.InvariantCulture,
                "{0:00} d {1:00} h {2:00} m {3:00} s",
                span.Days,
                span.Hours,
                span.Minutes,
                span.Seconds
            );
    }

    /// <summary>
    /// Converts time span using format 'hh:mm:ss'
    /// </summary>
    [Pure]
    public static string ToHourString
        (
            this TimeSpan span
        )
    {
        return string.Format
            (
                CultureInfo.InvariantCulture,
                "{0:00}:{1:00}:{2:00}",
                span.Hours + span.Days * 24,
                span.Minutes,
                span.Seconds
            );
    }

    /// <summary>
    /// Converts time span using format 'mm:ss'
    /// </summary>
    [Pure]
    public static string ToMinuteString
        (
            this TimeSpan span
        )
    {
        var totalMinutes = span.TotalMinutes;
        var minutes = (int)totalMinutes;
        var seconds = (int)((totalMinutes - minutes) * 60.0);

        return string.Format
            (
                CultureInfo.InvariantCulture,
                "{0:00}:{1:00}",
                minutes,
                seconds
            );
    }

    /// <summary>
    /// Converts time span using format 's.ff'
    /// </summary>
    [Pure]
    public static string ToSecondString (this TimeSpan span) =>
        span.TotalSeconds.ToString ("F2", CultureInfo.InvariantCulture);

    /// <summary>
    /// Converts time span using format 's'
    /// </summary>
    [Pure]
    public static string ToWholeSecondsString (this TimeSpan span) =>
        span.TotalSeconds.ToString ("F0", CultureInfo.InvariantCulture);

    /// <summary>
    /// Замена управляющих символов в тексте на указанный
    /// (по умолчанию - пробел).
    /// </summary>
    public static string? ReplaceControlCharacters
        (
            string? text,
            char substitute = ' '
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var needReplace = false;
        foreach (var c in text)
        {
            if (c < ' ')
            {
                needReplace = true;
                break;
            }
        }

        if (!needReplace)
        {
            return text;
        }

        var builder = new StringBuilder();
        builder.EnsureCapacity (text.Length);

        foreach (var c in text)
        {
            builder.Append
                (
                    c < ' ' ? substitute : c
                );
        }

        return builder.ToString();
    }

    /// <summary>
    /// Экранирование символов в тексте с помощью escape-символа.
    /// </summary>
    [Pure]
    public static string? Mangle
        (
            string? text,
            char escape,
            ReadOnlySpan<char> badCharacters
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var found = 0;
        foreach (var c in text)
        {
            // если текст не содержит недопустимых символов,
            // незачем городить весь огород

            if (c == escape || badCharacters.Contains (c))
            {
                ++found;
            }
        }

        if (found == 0)
        {
            return text;
        }

        var builder = new StringBuilder();
        builder.EnsureCapacity (text.Length + found);
        foreach (var c in text)
        {
            if (badCharacters.Contains (c) || c == escape)
            {
                builder.Append (escape);
            }

            builder.Append (c);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Сравнение двух фрагментов.
    /// </summary>
    [Pure]
    public static int CompareSpans
        (
            ReadOnlySpan<byte> first,
            ReadOnlySpan<byte> second
        )
    {
        for (var i = 0;; i++)
        {
            if (i == first.Length)
            {
                if (i == second.Length)
                {
                    return 0;
                }

                return -1;
            }

            if (i == second.Length)
            {
                return 1;
            }

            var result = first[i] - second[i];
            if (result != 0)
            {
                return result;
            }
        }
    }

    /// <summary>
    /// Сравнение двух фрагментов.
    /// </summary>
    [Pure]
    public static int CompareSpans
        (
            ReadOnlySpan<char> first,
            ReadOnlySpan<char> second
        )
    {
        for (var i = 0;; i++)
        {
            if (i == first.Length)
            {
                if (i == second.Length)
                {
                    return 0;
                }

                return -1;
            }

            if (i == second.Length)
            {
                return 1;
            }

            var result = first[i] - second[i];
            if (result != 0)
            {
                return result;
            }
        }
    }

    /// <summary>
    /// Разбивка текста на отдельные строки.
    /// </summary>
    /// <remarks>Пустые строки не удаляются.</remarks>
    /// <param name="text">Текст для разбиения.</param>
    /// <returns>Массив строк.</returns>
    [Pure]
    public static string[] SplitLines
        (
            this string text
        )
    {
        // TODO реализовать эффективно

        text = text.Replace ("\r\n", "\n");
        var result = text.Split ('\n');

        return result;
    }

    /// <summary>
    /// Содержит ли строка указанную подстроку?
    /// </summary>
    public static bool SafeContains
        (
            this string? text,
            string? subtext
        )
    {
        return !string.IsNullOrEmpty (text)
               && !string.IsNullOrEmpty (subtext)
               && text.Contains (subtext);
    }

    /// <summary>
    /// Содержит ли строка указанный символ?
    /// </summary>
    public static bool SafeContains
        (
            this string? text,
            char symbol
        )
    {
        return !string.IsNullOrEmpty (text)
               && text.Contains (symbol);
    }

    /// <summary>
    /// Содержит ли данная строка одну из перечисленных подстрок?
    /// </summary>
    public static bool SafeContains
        (
            this string? text,
            string? subtext1,
            string? subtext2
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return false;
        }

        if (!string.IsNullOrEmpty (subtext1) && text.Contains (subtext1))
        {
            return true;
        }

        return !string.IsNullOrEmpty (subtext2) && text.Contains (subtext2);
    }

    /// <summary>
    /// Содержит ли данная строка одну из перечисленных подстрок?
    /// </summary>
    public static bool SafeContains
        (
            this string? text,
            string? subtext1,
            string? subtext2,
            string? subtext3
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return false;
        }

        if (!string.IsNullOrEmpty (subtext1) && text.Contains (subtext1))
        {
            return true;
        }

        if (!string.IsNullOrEmpty (subtext2) && text.Contains (subtext2))
        {
            return true;
        }

        return !string.IsNullOrEmpty (subtext3) && text.Contains (subtext3);
    }

    /// <summary>
    /// Содержит ли данная строка одну из перечисленных подстрок?
    /// </summary>
    [Pure]
    public static bool SafeContains
        (
            this string? text,
            params string?[] subtexts
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return false;
        }

        foreach (var subtext in subtexts)
        {
            if (!string.IsNullOrEmpty (subtext) && text.Contains (subtext))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Безопасное вычисление длины строки.
    /// </summary>
    [Pure]
    public static int SafeLength
        (
            this string? text
        )
    {
        return text?.Length ?? 0;
    }

    /// <summary>
    /// Содержит ли данная строка заданную подстроку (без учета регистра символов)?
    /// </summary>
    [Pure]
    public static bool SafeContainsNoCase
        (
            this string? text,
            string? subtext
        )
    {
        return !string.IsNullOrEmpty (text) && !string.IsNullOrEmpty (subtext) &&
               text.ToUpperInvariant().Contains (subtext.ToUpperInvariant());
    }

    /// <summary>
    /// Безопасный триминг строки.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static string? SafeTrim
        (
            this string? text
        )
    {
        return string.IsNullOrEmpty (text) ? text : text.Trim();
    }

    /// <summary>
    /// Конвертирует слеши в принятые в текущей операционной системе.
    /// </summary>
    public static string ConvertSlashes
        (
            this string path
        )
    {
        return OperatingSystem.IsWindows()
            ? path
            : path.Replace ('\\', '/');
    }

    /// <summary>
    /// Усекает строку до указанной длины, добавляя при необходимости
    /// многоточие в конце строки.
    /// </summary>
    /// <param name="text">Текст для усечения.</param>
    /// <param name="length">Максимальная длина (без учета многоточия).
    /// </param>
    /// <returns>Результирующая строка.</returns>
    [Pure]
    public static string TruncateWithEllipsis
        (
            string? text,
            int length
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            text = text.Trim();

            if (text.Length > length)
            {
                return text.Substring (0, length) + "…";
            }
        }

        return text ?? string.Empty;
    }

    /// <summary>
    /// Получает результат выполнения задачи, если задача завершена,
    /// иначе выдает значение по умолчанию.
    /// </summary>
    /// <param name="task">Проверяемая задача.</param>
    [Pure]
    [DebuggerStepThrough]
    public static T? GetResultOrDefault<T>
        (
            this Task<T?> task
        )
    {
        return task.Status == TaskStatus.RanToCompletion
            ? task.Result
            : default;
    }

    /// <summary>
    /// Оптимальное число параллельных потоков для машины,
    /// на которой выполняется код.
    /// </summary>
    [Pure]
    public static int OptimalParallelism
    {
        get
        {
            var result = Math.Min
                (
                    Math.Max
                        (
                            Environment.ProcessorCount - 1,
                            1
                        ),
                    8 // TODO choose good number
                );

            return result;
        }
    }

    /// <summary>
    /// Выбор первой не пустой среди перечисленных строк.
    /// </summary>
    [Pure]
    public static string NonEmpty
        (
            string? first,
            string? second
        )
    {
        return !string.IsNullOrEmpty (first) ? first
            : !string.IsNullOrEmpty (second) ? second
            : throw new ArgumentNullException();
    }

    /// <summary>
    /// Выбор первой не пустой среди перечисленных строк.
    /// </summary>
    [Pure]
    public static string NonEmpty
        (
            string? first,
            string? second,
            string? third
        )
    {
        return !string.IsNullOrEmpty (first) ? first
            : !string.IsNullOrEmpty (second) ? second
            : !string.IsNullOrEmpty (third) ? third
            : throw new ArgumentNullException();
    }

    /// <summary>
    /// Выбор первой не пустой среди перечисленных строк.
    /// </summary>
    [Pure]
    public static string NonEmpty
        (
            string? first,
            string? second,
            string? third,
            string? fourth
        )
    {
        return !string.IsNullOrEmpty (first) ? first
            : !string.IsNullOrEmpty (second) ? second
            : !string.IsNullOrEmpty (third) ? third
            : !string.IsNullOrEmpty (fourth) ? fourth
            : throw new ArgumentNullException();
    }

    /// <summary>
    /// Выбирает первую не пустую среди перечисленных строк.
    /// </summary>
    public static string NonEmpty
        (
            IEnumerable<string?> strings
        )
    {
        foreach (var one in strings)
        {
            if (!string.IsNullOrEmpty (one))
            {
                return one;
            }
        }

        throw new ArgumentNullException();
    }

    /// <summary>
    /// Выбирает первую не пустую среди перечисленных строк,
    /// либо возвращает значение по умолчанию.
    /// </summary>
    [Pure]
    public static string NonEmptyOrDefault
        (
            string?[]? array,
            string defaultValue = "(none)"
        )
    {
        if (array is null || array.Length == 0)
        {
            return defaultValue;
        }

        foreach (var contentType in array)
        {
            if (!string.IsNullOrEmpty (contentType))
            {
                return contentType;
            }
        }

        return defaultValue;
    }

    /// <summary>
    /// Выбирает первый не пустой спан среди перечисленных.
    /// </summary>
    [Pure]
    public static ReadOnlySpan<T> NonEmpty<T>
        (
            ReadOnlySpan<T> first,
            ReadOnlySpan<T> second
        )
    {
        return !first.IsEmpty ? first
            : !second.IsEmpty ? second
            : throw new ArgumentOutOfRangeException();
    }

    /// <summary>
    /// Выбирает первый не пустой спан среди перечисленных.
    /// </summary>
    [Pure]
    public static ReadOnlySpan<T> NonEmpty<T>
        (
            ReadOnlySpan<T> first,
            ReadOnlySpan<T> second,
            ReadOnlySpan<T> third
        )
    {
        return !first.IsEmpty ? first
            : !second.IsEmpty ? second
            : !third.IsEmpty ? third
            : throw new ArgumentOutOfRangeException();
    }

    /// <summary>
    /// Выбор первого не пустого спана среди перечисленных.
    /// </summary>
    [Pure]
    public static ReadOnlySpan<T> NonEmpty<T>
        (
            ReadOnlySpan<T> first,
            ReadOnlySpan<T> second,
            ReadOnlySpan<T> third,
            ReadOnlySpan<T> fourth
        )
    {
        return !first.IsEmpty ? first
            : !second.IsEmpty ? second
            : !third.IsEmpty ? third
            : !fourth.IsEmpty ? fourth
            : throw new ArgumentOutOfRangeException();
    }

    /// <summary>
    /// Выбор первого не пустго спана среди перечисленных.
    /// </summary>
    [Pure]
    public static ReadOnlyMemory<T> NonEmpty<T>
        (
            ReadOnlyMemory<T> first,
            ReadOnlyMemory<T> second
        )
    {
        return !first.IsEmpty ? first
            : !second.IsEmpty ? second
            : throw new ArgumentOutOfRangeException();
    }

    /// <summary>
    /// Выбор первого не пустого спана среди перечисленных.
    /// </summary>
    [Pure]
    public static ReadOnlyMemory<T> NonEmpty<T>
        (
            ReadOnlyMemory<T> first,
            ReadOnlyMemory<T> second,
            ReadOnlyMemory<T> third
        )
    {
        return !first.IsEmpty ? first
            : !second.IsEmpty ? second
            : !third.IsEmpty ? third
            : throw new ArgumentOutOfRangeException();
    }

    /// <summary>
    /// Выбор первого не пустого спана среди перечисленных.
    /// </summary>
    [Pure]
    public static ReadOnlyMemory<T> NonEmpty<T>
        (
            ReadOnlyMemory<T> first,
            ReadOnlyMemory<T> second,
            ReadOnlyMemory<T> third,
            ReadOnlyMemory<T> fourth
        )
    {
        return !first.IsEmpty ? first
            : !second.IsEmpty ? second
            : !third.IsEmpty ? third
            : !fourth.IsEmpty ? fourth
            : throw new ArgumentOutOfRangeException();
    }

    /// <summary>
    /// Обрезка начальных и конечных пробелов в строках.
    /// </summary>
    public static IEnumerable<string> TrimLines
        (
            this IEnumerable<string?> lines
        )
    {
        foreach (var line in lines)
        {
            if (!ReferenceEquals (line, null))
            {
                yield return line.Trim();
            }
        }
    }

    /// <summary>
    /// Обрезка начальных и конечных пробелов в строках.
    /// </summary>
    public static IEnumerable<string> TrimLines
        (
            this IEnumerable<string?> lines,
            params char[] characters
        )
    {
        foreach (var line in lines)
        {
            if (!ReferenceEquals (line, null))
            {
                yield return line.Trim (characters);
            }
        }
    }

    /// <summary>
    /// Удаляет в строке начальный и конечный символ кавычек.
    /// </summary>
    [Pure]
    public static string Unquote
        (
            this string text,
            char quoteChar = '"'
        )
    {
        var length = text.Length;
        if (length > 1)
        {
            if (text[0] == quoteChar
                && text[length - 1] == quoteChar)
            {
                text = text.Substring (1, length - 2);
            }
        }

        return text;
    }

    /// <summary>
    /// Получение имени исполняемого процесса (с путями).
    /// </summary>
    public static string ExecutableFileName
    {
        get
        {
            var process = Process.GetCurrentProcess();
            var module = process.MainModule;

            return module?.FileName
                   ?? throw new ApplicationException ("Can't obtain executable file name");
        }
    }

    private static readonly Random _random = new ();
    private const string RandomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    private const string RandomSymbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";

    /// <summary>
    /// Создание случайной строки указанной длины
    /// </summary>
    public static string RandomIdentifier
        (
            int length
        )
    {
        var builder = new StringBuilder();
        builder.EnsureCapacity (length);

        if (length > 0)
        {
            builder.Append (RandomChars[_random.Next (RandomChars.Length)]);
        }

        for (; length > 1; length--)
        {
            builder.Append (RandomSymbols[_random.Next (RandomSymbols.Length)]);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Извлечение внутреннего исключения из <see cref="AggregateException"/>
    /// (если таковое имеется, иначе ничего не происходит).
    /// </summary>
    public static Exception Unwrap
        (
            Exception exception
        )
    {
        if (exception is AggregateException aggregate)
        {
            aggregate = aggregate.Flatten();
            aggregate.Handle
                (
                    ex =>
                    {
                        Debug.WriteLine
                            (
                                nameof (Utility) + "::" + nameof (Unwrap) 
                                + ": " + ex.Message
                            );

                        return true;
                    }
                );

            return aggregate.InnerExceptions[0];
        }

        return exception;
    }

    /// <summary>
    /// Преобразование последовательности объектов в массив строк.
    /// </summary>
    public static string[] ToStringArray<T>
        (
            this IEnumerable<T> items
        )
    {
        var result = new List<string>();
        foreach (var item in items)
        {
            var line = item is null
                ? string.Empty
                : item.ToString() ?? string.Empty;
            result.Add (line);
        }

        return result.ToArray();
    }

    /// <summary>
    /// Сокращение для <see cref="string.IsNullOrEmpty"/>
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static bool IsEmpty
        (
            [NotNullWhen (false)] this string? text
        )
    {
        return string.IsNullOrEmpty (text);
    }

    /// <summary>
    /// Универсальное длинное представление даты/времени.
    /// </summary>
    [Pure]
    public static string ToLongUniformString
        (
            this DateTime dateTime
        )
    {
        return dateTime.ToString ("yyyy-MM-dd HH:mm:ss");
    }

    /// <summary>
    /// Универсальное короткое представление даты.
    /// </summary>
    [Pure]
    public static string ToShortUniformString
        (
            this DateTime dateTime
        )
    {
        return dateTime.ToString ("yyyy-MM-dd");
    }

    /// <summary>
    /// Начало эпохи UNIX.
    /// </summary>
    public static readonly DateTime UnixStart = new (1970, 1, 1);

    /// <summary>
    /// Переводит указанную дату в формат Unix.
    /// </summary>
    [Pure]
    public static long ToUnixTime
        (
            this DateTime dateTime
        )
    {
        return (long)(dateTime - UnixStart).TotalSeconds;
    }

    /// <summary>
    /// Получение индекса последнего вхождения любой из перечисленных строк.
    /// </summary>
    [Pure]
    public static int LastIndexOfAny
        (
            string text,
            IEnumerable<string> fragments
        )
    {
        var result = -1;
        foreach (var fragment in fragments)
        {
            var index = text.LastIndexOf (fragment, StringComparison.InvariantCulture);
            if (index > result)
            {
                result = index;
            }
        }

        return result;
    }

    /// <summary>
    /// Чтение структуры по указанному смещению.
    /// </summary>
    [Pure]
    public static T Read<T>
        (
            this ReadOnlySpan<byte> span,
            int offset
        )
        where T : struct
    {
        return MemoryMarshal.Read<T> (span[offset..]);
    }

    /// <summary>
    /// Чтение 16-битного целого в хостовой раскладке по указанному смещению.
    /// </summary>
    [Pure]
    public static short ReadHostInt16
        (
            this ReadOnlySpan<byte> span,
            int offset
        )
    {
        return MemoryMarshal.Read<short> (span[offset..]);
    }

    /// <summary>
    /// Чтение 32-битного целого в сетевой раскладке по указанному смещению.
    /// </summary>
    [Pure]
    public static short ReadNetworkInt16
        (
            this ReadOnlySpan<byte> span,
            int offset
        )
    {
        return IPAddress.NetworkToHostOrder (MemoryMarshal.Read<short> (span[offset..]));
    }

    /// <summary>
    /// Чтение 32-битного целого в хостовой раскладке по указанному смещению.
    /// </summary>
    [Pure]
    public static int ReadHostInt32
        (
            this ReadOnlySpan<byte> span,
            int offset
        )
    {
        return MemoryMarshal.Read<int> (span[offset..]);
    }

    /// <summary>
    /// Чтение 32-битного целого в сетевой раскладке по указанному смещению.
    /// </summary>
    [Pure]
    public static int ReadNetworkInt32
        (
            this ReadOnlySpan<byte> span,
            int offset
        )
    {
        return IPAddress.NetworkToHostOrder (MemoryMarshal.Read<int> (span[offset..]));
    }

    /// <summary>
    /// Чтение 64-битного целого в хостовой раскладке по указанному смещению.
    /// </summary>
    [Pure]
    public static long ReadHostInt64
        (
            this ReadOnlySpan<byte> span,
            int offset
        )
    {
        return MemoryMarshal.Read<long> (span[offset..]);
    }

    /// <summary>
    /// Запись структуры по указанному смещению.
    /// Метод предназначен для мелких структур, например, System.Int32.
    /// </summary>
    public static void Write<T>
        (
            this Span<byte> span,
            int offset,
            T value
        )
        where T : struct
    {
        MemoryMarshal.Write (span[offset..], ref value);
    }

    /// <summary>
    /// Запись 16-битного целого по указанному адресу в хостовой раскладке.
    /// </summary>
    public static void WriteHostInt16
        (
            this Span<byte> span,
            int offset,
            short value
        )
    {
        Write (span, offset, value);
    }

    /// <summary>
    /// Запись 16-битного целого по указанному адресу в сетевой раскладке.
    /// </summary>
    public static void WriteNetworkInt16
        (
            this Span<byte> span,
            int offset,
            short value
        )
    {
        Write (span, offset, IPAddress.HostToNetworkOrder (value));
    }

    /// <summary>
    /// Запись 32-битного целого по указанному адресу в хостовой раскладке.
    /// </summary>
    public static void WriteHostInt32
        (
            this Span<byte> span,
            int offset,
            int value
        )
    {
        Write (span, offset, value);
    }

    /// <summary>
    /// Запись 32-битного целого по указанному адресу в сетевой раскладке.
    /// </summary>
    public static void WriteNetworkInt32
        (
            this Span<byte> span,
            int offset,
            int value
        )
    {
        Write (span, offset, IPAddress.HostToNetworkOrder (value));
    }

    /// <summary>
    /// Запись 64-битного целого по указанному адресу в хостовой раскладке.
    /// </summary>
    public static void WriteHostInt64
        (
            this Span<byte> span,
            int offset,
            long value
        )
    {
        Write (span, offset, value);
    }

    /// <summary>
    /// Упрощенное получение информации о методе.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static MethodInfo GetMethodInfo
        (
            Expression<Action> expression
        )
    {
        return ((MethodCallExpression) expression.Body).Method;
    }

    /// <summary>
    /// Упрощенное получение информации о методе.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static MethodInfo GetMethodInfo<T>
        (
            Expression<Action<T>> expression
        )
    {
        return ((MethodCallExpression) expression.Body).Method;
    }

    /// <summary>
    /// Упрощенное получение информации о методе.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static MethodInfo GetMethodInfo<T1, T2>
        (
            Expression<Action<T1, T2>> expression
        )
    {
        return ((MethodCallExpression) expression.Body).Method;
    }

    /// <summary>
    /// Упрощенное получение информации о методе.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static MethodInfo GetMethodInfo<T1, T2, T3>
        (
            Expression<Action<T1, T2, T3>> expression
        )
    {
        return ((MethodCallExpression) expression.Body).Method;
    }

    /// <summary>
    /// Упрощенное получение информации о методе.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4>
        (
            Expression<Action<T1, T2, T3, T4>> expression
        )
    {
        return ((MethodCallExpression) expression.Body).Method;
    }

    /// <summary>
    /// Упрощенное получение информации о методе.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static MethodInfo GetMethodInfo<T>
        (
            Expression<Func<T>> expression
        )
    {
        return ((MethodCallExpression) expression.Body).Method;
    }

    /// <summary>
    /// Упрощенное получение информации о методе.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static MethodInfo GetMethodInfo<T1, T2>
        (
            Expression<Func<T1, T2>> expression
        )
    {
        return ((MethodCallExpression) expression.Body).Method;
    }

    /// <summary>
    /// Упрощенное получение информации о методе.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static MethodInfo GetMethodInfo<T1, T2, T3>
        (
            Expression<Func<T1, T2, T3>> expression
        )
    {
        return ((MethodCallExpression) expression.Body).Method;
    }

    /// <summary>
    /// Упрощенное получение информации о методе.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static MethodInfo GetMethodInfo<T1, T2, T3, T4>
        (
            Expression<Func<T1, T2, T3, T4>> expression
        )
    {
        return ((MethodCallExpression) expression.Body).Method;
    }

    /// <summary>
    /// "Запустить и забыть".
    /// </summary>
    /// <remarks>
    /// https://www.meziantou.net/fire-and-forget-a-task-in-dotnet.htm
    /// </remarks>
    public static void Forget
        (
            this Task task
        )
    {
        // Only care about tasks that may fault or are faulted,
        // so fast-path for SuccessfullyCompleted and Canceled tasks
        if (!task.IsCompleted || task.IsFaulted)
        {
            _ = ForgetAwaited (task);
        }

        static async Task ForgetAwaited (Task task)
        {
            try
            {
                // No need to resume on the original SynchronizationContext
                await task.ConfigureAwait (false);
            }
            catch
            {
                // Nothing to do here
            }
        }
    }

    /// <summary>
    /// Соединение последовательности в строку.
    /// </summary>
    [Pure]
    [DebuggerStepThrough]
    public static string JoinText<T>
        (
            this IEnumerable<T> sequence,
            string separator = ", "
        )
    {
        return string.Join (separator, sequence.ToArray());
    }

    /// <summary>
    /// Ожидание завершения задачи с таймаутом или отменой по токену.
    /// </summary>
    public static async Task<TResult> WithTimeout<TResult>
        (
            this Task<TResult> task,
            TimeSpan timeout,
            CancellationToken cToken
        )
    {
        Sure.NotNull (task);

        using var cts = CancellationTokenSource.CreateLinkedTokenSource (cToken);
        var completedTask = await Task.WhenAny (task, Task.Delay (timeout, cts.Token));
        if (completedTask == task)
        {
            cts.Cancel(); // Cancel to stop timer
            return await task;
        }

        throw new TimeoutException();
    }

    /// <summary>
    /// Попытка получить <see cref="ReadOnlySpan{T}"/> от коллекции.
    /// </summary>
    /// <remarks>
    /// Метрод выдран из исходников .NET 7.
    /// </remarks>
    public static bool TryGetSpan<TSource>
        (
            IEnumerable<TSource> source,
            out ReadOnlySpan<TSource> span
        )
        where TSource : struct
    {
        Sure.NotNull (source);

        // Use `GetType() == typeof(...)` rather than `is` to avoid cast helpers.  This is measurably cheaper
        // but does mean we could end up missing some rare cases where we could get a span but don't (e.g. a uint[]
        // masquerading as an int[]).  That's an acceptable tradeoff.  The Unsafe usage is only after we've
        // validated the exact type; this could be changed to a cast in the future if the JIT starts to recognize it.
        // We only pay the comparison/branching costs here for super common types we expect to be used frequently
        // with LINQ methods.

        var result = true;
        if (source.GetType() == typeof (TSource[]))
        {
            span = Unsafe.As<TSource[]> (source);
        }
        else if (source.GetType() == typeof (List<TSource>))
        {
            span = CollectionsMarshal.AsSpan (Unsafe.As<List<TSource>> (source));
        }
        else
        {
            span = default;
            result = false;
        }

        return result;
    }

    #endregion
}
