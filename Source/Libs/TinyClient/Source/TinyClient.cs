// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TinyClient.cs -- сокращенная версия клиента для ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Полезные, при этом не публичные методы расширения.
    /// </summary>
    [DebuggerStepThrough]
    internal static class Private
    {
        /// <summary>
        /// Превращает объект в видимую строку.
        /// </summary>
        public static string ToVisibleString<T> (this T? value) where T: class =>
            value?.ToString() ?? "(null)";

        /// <summary>
        /// Безопасный доступ по индексу.
        /// </summary>
        public static T? SafeAt<T> (this IList<T?> items, int index, T? defaultValue = default) =>
            index < 0 || index >= items.Count ? defaultValue : items[index];

        /// <summary>
        /// Безопасный доступ по индексу.
        /// </summary>
        public static T? SafeAt<T> (this T?[]? items, int index, T? defaultValue = default) =>
            items is null || index < 0 || index >= items.Length ? defaultValue : items[index];

        /// <summary>
        /// Безопасное получение первого символа в строке.
        /// </summary>
        public static char FirstChar (this string? text) =>
            ReferenceEquals(text, null) || text.Length == 0 ? '\0' : text[0];

        /// <summary>
        /// Безопасное получение первого символа в строке.
        /// </summary>
        public static char FirstChar (this ReadOnlyMemory<char> text) =>
            text.Length == 0 ? '\0' : text.Span[0];

        /// <summary>
        /// Безопасное получение первого символа в строке.
        /// </summary>
        public static char FirstChar (this ReadOnlySpan<char> text) =>
            text.Length == 0 ? '\0' : text[0];

        /// <summary>
        /// Определяет, равен ли данный объект
        /// любому из перечисленных.
        /// </summary>
        public static bool IsOneOf<T> (this T value, IEnumerable<T> items) where T: IComparable<T>
        {
            foreach (var one in items)
                if (value.CompareTo(one) == 0)
                    return true;

            return false;

        } // method IsOneOf

        /// <summary>
        /// Разбивка текста на отдельные строки.
        /// </summary>
        /// <remarks>Пустые строки не удаляются.</remarks>
        public static string[] SplitLines (this string text) =>
            text.Replace("\r\n", "\n").Split('\n');

        /// <summary>
        /// Обязательное чтение строки.
        /// </summary>
        public static string RequireLine (this TextReader reader)
        {
            var result = reader.ReadLine();
            if (ReferenceEquals(result, null))
                throw new IrbisException ("Unexpected end of stream");

            return result;

        } // method RequireLine

        /// <summary>
        /// Создает текстовый файл в указанной кодировке.
        /// </summary>
        public static StreamWriter CreateTextFile (string fileName, Encoding encoding) =>
            new (new FileStream (fileName, FileMode.Create), encoding);

        /// <summary>
        /// Открывает текстовый файл в указанной кодировке.
        /// </summary>
        public static StreamReader OpenRead (string fileName, Encoding encoding) =>
            new (File.OpenRead (fileName), encoding);

        /// <summary>
        /// Разбор целого 32-битного числа со знаком.
        /// </summary>
        public static unsafe int ParseInt32 (ReadOnlyMemory<char> text)
        {
            if (text.IsEmpty)
                return 0;

            var result = 0;
            var sign = false;
            unchecked
            {
                fixed (char* ptr = text.Span)
                {
                    var index = 0;
                    while (ptr[index] == '-')
                    {
                        sign = !sign;
                        ++index;
                    }

                    for (; index < text.Length; index++)
                        result = result * 10 + ptr[index] - '0';

                    if (sign)
                        result = -result;
                }
            }

            return result;

        } // method ParseInt32

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32 (string text)
        {
            var result = 0;
            unchecked
            {
                fixed (char* ptr = text)
                {
                    var length = text.Length;
                    for (var i = 0; i < length; i++)
                    {
                        result = result * 10 + ptr[i] - '0';
                    }
                }
            }

            return result;

        } // method ParseInt32

        /// <summary>
        /// Converts given value to the specified type.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <returns>Converted value.</returns>
        public static T ConvertTo<T> (object? value)
        {
            if (ReferenceEquals(value, null))
            {
                return default!;
            }

            var sourceType = value.GetType();
            var targetType = typeof(T);

            if (targetType == typeof(string))
                return (T)(object) value.ToString()!;

            if (targetType.IsAssignableFrom (sourceType))
                return (T)value;

            if (value is IConvertible)
                return (T)Convert.ChangeType (value, targetType);

            var converterFrom = TypeDescriptor.GetConverter (value);
            if (converterFrom.CanConvertTo (targetType))
                return (T) converterFrom.ConvertTo (value, targetType)!;

            var converterTo = TypeDescriptor.GetConverter (targetType);
            if (converterTo.CanConvertFrom (sourceType))
                return (T) converterTo.ConvertFrom(value)!;

            throw new IrbisException();

        } // method ConvertTo

        /// <summary>
        /// Бросает исключение, если переданное значение пустое,
        /// иначе просто возвращает его.
        /// </summary>
        public static ReadOnlyMemory<T> ThrowIfEmpty<T>
            (
                this ReadOnlyMemory<T> memory,
                string message = "Empty value detected"
            )
        {
            if (memory.IsEmpty)
                throw new ArgumentException (message);

            return memory;

        } // method ThrowIfEmpty

        /// <summary>
        /// Бросает исключение, если переданное значение пустое,
        /// иначе просто возвращает его.
        /// </summary>
        public static ReadOnlySpan<T> ThrowIfEmpty<T>
            (
                this ReadOnlySpan<T> memory,
                string message = "Empty value detected"
            )
        {
            if (memory.IsEmpty)
            {
                throw new ArgumentException (message);
            }

            return memory;

        } // method ThrowIfEmpty

        /// <summary>
        /// Бросает исключение, если переданное значение равно <c>null</c>,
        /// иначе просто возвращает его.
        /// </summary>
        public static T ThrowIfNull<T>
            (
                this T? value,
                string message
            )
            where T: class
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentException (message);
            }

            return value;

        } // method ThrowIfNull

        /// <summary>
        /// Подстановка значения по умолчанию вместо <c>null</c>.
        /// </summary>
        public static T IfNull<T>(this T? value, T defaultValue)
            where T: class
            => value ?? defaultValue;

        /// <summary>
        /// Подстановка значения по умолчания вместо пустой строки.
        /// </summary>
        public static string IfEmpty(this string? value, string defaultValue)
            => ReferenceEquals(value, null) || value.Length == 0
                ? defaultValue.Length == 0
                    ? throw new ArgumentNullException(nameof(defaultValue))
                    : defaultValue
                : value;

        /// <summary>
        /// Бросает исключение, если переданное значение равно <c>null</c>,
        /// иначе просто возвращает его.
        /// </summary>
        public static T ThrowIfNull<T> (this T? value) where T: class =>
            ThrowIfNull (value, "Null value detected");

        /// <summary>
        /// Бросает исключение, если переданная строка пустая
        /// или равна <c>null</c>.
        /// </summary>
        public static string ThrowIfNullOrEmpty
            (
                this string? value
            )
        {
            if (ReferenceEquals(value, null) || value.Length == 0)
            {
                throw new ArgumentException();
            }

            return value;

        } // method ThrowIfNullOrEmpty

        /// <summary>
        /// Бросает исключение, если переданная строка пустая
        /// или равна <c>null</c>.
        /// </summary>
        public static string ThrowIfNullOrEmpty
            (
                this string? value,
                string argumentName
            )
        {
            if (ReferenceEquals(value, null) || value.Length == 0)
            {
                throw new ArgumentException(argumentName);
            }

            return value;

        } // method ThrowIfNullOrEmpty

        /// <summary>
        /// Бросает исключение, если переданная строка пустая
        /// или равна <c>null</c>.
        /// </summary>
        public static ReadOnlySpan<char> ThrowIfNullOrEmpty
            (
                this ReadOnlySpan<char> value
            )
        {
            if (value.IsEmpty)
            {
                throw new ArgumentException();
            }

            return value;

        } // method ThrowIfNullOrEmpty

        /// <summary>
        /// Преобразование целого числа в строку.
        /// </summary>
        public static int Int32ToBytes
            (
                int number,
                byte[] buffer
            )
        {
            var flag = number < 0;
            if (flag)
            {
                number = -number;
            }

            if (number == 0)
            {
                buffer[0] = (byte)'0';
                return 1;
            }

            var length = 0;
            for (; number != 0;)
            {
                number = Math.DivRem(number, 10, out var rem);
                buffer[length++] = (byte) ('0' + rem);
            }

            if (flag)
            {
                buffer[length++] = (byte) '-';
            }

            var i1 = 0;
            var i2 = length - 1;
            while (i1 < i2)
            {
                var c = buffer[i1];
                buffer[i1++] = buffer[i2];
                buffer[i2--] = c;
            }

            return length;

        } // method Int32ToBytes

        /// <summary>
        /// Проверяет, содержит ли спан указанный символ.
        /// </summary>
        public static bool Contains (ReadOnlySpan<char> span, char chr)
        {
            foreach (var c in span)
                if (c == chr)
                    return true;

            return false;

        } // method Contains

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
            if (string.IsNullOrEmpty(text))
                return defaultValue;

            if (!int.TryParse(text, out var result))
                result = defaultValue;

            if (result < minValue || result > maxValue)
                result = defaultValue;

            return result;

        } // method SafeToInt32

        /// <summary>
        /// Безопасное преобразование строки в целое.
        /// </summary>
        public static int SafeToInt32
            (
                this string? text,
                int defaultValue
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }

            if (!int.TryParse(text, out var result))
            {
                result = defaultValue;
            }

            return result;

        } // method SafeToInt32

        /// <summary>
        /// Безопасное преобразование строки в целое.
        /// </summary>
        public static int SafeToInt32
            (
                this string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            if (!int.TryParse(text, out var result))
            {
                result = 0;
            }

            return result;

        } // method SafeToInt32

        /// <summary>
        /// Содержит ли строка указанную подстроку?
        /// </summary>
        public static bool SafeContains (this string? text, string? subtext) =>
            !ReferenceEquals(text, null)
            && !ReferenceEquals(subtext, null)
            && text.Contains(subtext);

        /// <summary>
        /// Содержит ли строка указанный символ?
        /// </summary>
        public static bool SafeContains (this string? text, char symbol) =>
            !ReferenceEquals(text, null)
            && text.Contains(symbol);

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
            if (ReferenceEquals(text, null) || text.Length == 0)
            {
                return false;
            }

            if (!ReferenceEquals(subtext1, null) && text.Contains(subtext1))
            {
                return true;
            }

            return !ReferenceEquals(subtext2, null) && text.Contains(subtext2);

        } // method SafeContains

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
            if (ReferenceEquals(text, null) || text.Length == 0)
                return false;

            if (!ReferenceEquals(subtext1, null) && text.Contains(subtext1))
                return true;

            if (!ReferenceEquals(subtext2, null) && text.Contains(subtext2))
                return true;

            return !ReferenceEquals(subtext3, null) && text.Contains(subtext3);

        } // method SafeContains

        /// <summary>
        /// Содержит ли данная строка одну из перечисленных подстрок?
        /// </summary>
        public static bool SafeContains
            (
                this string? text,
                params string?[] subtexts
            )
        {
            if (ReferenceEquals(text, null) || text.Length == 0)
                return false;

            foreach (var subtext in subtexts)
                if (!ReferenceEquals(subtext, null) && text.Contains(subtext))
                    return true;

            return false;

        } // method SafeContains

        /// <summary>
        /// Сравнивает символы с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первый символ.</param>
        /// <param name="two">Второй символ.</param>
        /// <returns>Символы совпадают с точностью до регистра?</returns>
        public static bool SameChar (this char one, char two) =>
            char.ToUpperInvariant(one) == char.ToUpperInvariant(two);

        /// <summary>
        /// Сравнивает символы с точностью до регистра.
        /// </summary>
        /// <param name="one">Первый символ.</param>
        /// <param name="two">Второй символ.</param>
        /// <param name="three">Третий символ.</param>
        /// <returns>Символы совпадают с точностью до регистра.</returns>
        public static bool SameChar
            (
                this char one,
                char two,
                char three
            )
        {
            one = char.ToUpperInvariant(one);

            return one == char.ToUpperInvariant(two)
                || one == char.ToUpperInvariant(three);

        } // method SameChar

        /// <summary>
        /// Сравнивает символы с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первый символ.</param>
        /// <param name="two">Второй символ.</param>
        /// <param name="three">Третий символ.</param>
        /// <param name="four">Четвертый символ.</param>
        /// <returns>Символы совпадают с точностью до регистра.</returns>
        public static bool SameChar
            (
                this char one,
                char two,
                char three,
                char four
            )
        {
            one = char.ToUpperInvariant(one);

            return one == char.ToUpperInvariant(two)
                || one == char.ToUpperInvariant(three)
                || one == char.ToUpperInvariant(four);

        } // method SameChar

        /// <summary>
        /// Сравнивает символы с точностью до регистра
        /// без учета текущей кульутры.
        /// </summary>
        /// <param name="one">Левая часть.</param>
        /// <param name="array">Правая часть.</param>
        /// <returns>Результат поиска <paramref name="one"/> среди
        /// элементов <paramref name="array"/>.</returns>
        public static bool SameChar
            (
                this char one,
                params char[] array
            )
        {
            foreach (var two in array)
            {
                if (char.ToUpperInvariant(one) == char.ToUpperInvariant(two))
                {
                    return true;
                }
            }

            return false;

        } // method SameChar

        /// <summary>
        /// Сравнивает символы с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Левая часть.</param>
        /// <param name="text">Правая часть.</param>
        /// <returns>Результат поиска <paramref name="one"/> среди
        /// элементов <paramref name="text"/>.</returns>
        public static bool SameChar
            (
                this char one,
                IEnumerable<char> text
            )
        {
            foreach (var two in text)
            {
                if (char.ToUpperInvariant(one) == char.ToUpperInvariant(two))
                {
                    return true;
                }
            }

            return false;

        } // method SameChar

        /// <summary>
        /// Сравнивает символы с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Левая часть.</param>
        /// <param name="text">Правая часть.</param>
        /// <returns>Результат поиска <paramref name="one"/> среди
        /// элементов <paramref name="text"/>.</returns>
        public static bool SameChar
            (
                this char one,
                ReadOnlySpan<char> text
            )
        {
            foreach (var two in text)
            {
                if (char.ToUpperInvariant(one) == char.ToUpperInvariant(two))
                {
                    return true;
                }
            }

            return false;

        } // method SameChar

        /// <summary>
        /// Сравнивает строки с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
        public static bool SameString (this string? one, string? two) =>
            string.Compare (one, two, StringComparison.OrdinalIgnoreCase) == 0;

        /// <summary>
        /// Сравнивает строки с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
        public static bool SameString(this ReadOnlyMemory<char> one, string? two) =>
            one.Span.CompareTo(two.AsSpan(), StringComparison.OrdinalIgnoreCase) == 0;

        /// <summary>
        /// Сравнивает строки с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
        public static bool SameString(this ReadOnlySpan<char> one, ReadOnlySpan<char> two) =>
            one.CompareTo(two, StringComparison.OrdinalIgnoreCase) == 0;

        /// <summary>
        /// Сравнивает строки с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <param name="three">Третья строка.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
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
        public static bool SameString(this ReadOnlyMemory<char> one, string? two, string? three) =>
            one.Span.CompareTo(two.AsSpan(), StringComparison.OrdinalIgnoreCase) == 0
            || one.Span.CompareTo(three.AsSpan(), StringComparison.OrdinalIgnoreCase) == 0;

        /// <summary>
        /// Сравнивает строки с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <param name="three">Третья строка.</param>
        /// <param name="four">Четвертая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
        public static bool SameString (this string? one, string? two,
            string? three, string? four) =>
            string.Compare (one, two, StringComparison.OrdinalIgnoreCase) == 0
            || string.Compare (one, three, StringComparison.OrdinalIgnoreCase) == 0
            || string.Compare (one, four, StringComparison.OrdinalIgnoreCase) == 0;

        /// <summary>
        /// Сравнивает строки с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="array">Строки для сопоставления.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
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

        } // method SameString

        /// <summary>
        /// Сравнивает строки с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="strings">Строки для сопоставления.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
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

        } // method SameString

        /// <summary>
        /// Сравнивает строки с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="strings">Строки для сопоставления.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
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

        } // method SameString

        /// <summary>
        /// Отбирает из последовательности только
        /// ненулевые элементы.
        /// </summary>
        public static IEnumerable<T> NonNullItems<T>
            (
                this IEnumerable<T?> sequence
            )
            where T : class
        {
            foreach (var item in sequence)
            {
                if (!ReferenceEquals(item, null))
                {
                    yield return item;
                }
            }

        } // method NonNullItems

        /// <summary>
        /// Отбирает из последовательности только непустые строки.
        /// </summary>
        public static IEnumerable<string> NonEmptyLines
            (
                this IEnumerable<string?> sequence
            )
        {
            foreach (var line in sequence)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    yield return line!;
                }
            }

        } // method NonEmptyLines

        /// <summary>
        /// Отбирает из последовательности только непустые строки.
        /// </summary>
        public static IEnumerable<ReadOnlyMemory<char>> NonEmptyLines
            (
                this IEnumerable<ReadOnlyMemory<char>> sequence
            )
        {
            foreach (var line in sequence)
            {
                if (!line.IsEmpty)
                {
                    yield return line;
                }
            }

        } // method NonEmptyLines

        /// <summary>
        /// Конвертирует пустую строку в <c>null</c>.
        /// </summary>
        public static string? EmptyToNull (this string? value) =>
            string.IsNullOrEmpty(value) ? null : value;

        /// <summary>
        /// Конвертирует пустую строку в <c>null</c>.
        /// </summary>
        public static string? EmptyToNull(this ReadOnlySpan<char> value) =>
            value.IsEmpty ? null : value.ToString();

        /// <summary>
        /// Конвертирует пустую строку в <c>null</c>.
        /// </summary>
        public static string? EmptyToNull(this ReadOnlyMemory<char> value) =>
            value.IsEmpty ? null : value.ToString();

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this short value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this short value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this ushort value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this ushort value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this int value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this int value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString ( this uint value ) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString ( this uint value, string format ) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this long value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this long value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this ulong value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this ulong value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this float value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this float value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this double value) =>
            value.ToString (CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this double value, string format) =>
            value.ToString (format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this decimal value) =>
            value.ToString (CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this decimal value, string format) =>
            value.ToString (format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Is the list is <c>null</c> or empty?
        /// </summary>
        public static bool IsNullOrEmpty<T> (this IList<T>? list) =>
            ReferenceEquals (list, null) || list.Count == 0;

        /// <summary>
        /// Throw <see cref="ArgumentNullException"/>
        /// if the list is <c>null</c> or empty.
        /// </summary>
        public static IList<T> ThrowIfNullOrEmpty<T> (this IList<T>? list) =>
            ReferenceEquals (list, null) || list.Count == 0 ? throw new IrbisException() : list;

        /// <summary>
        /// Ищет первое вхождение паттерна в данных.
        /// </summary>
        /// <returns>
        /// Индекс первого вхождения или -1.
        /// </returns>
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
                return -1;

            for (var i = start; i <= dataLength; i++)
            {
                var found = true;
                for (var j = 0; j < patternLength; j++)
                    if (data[i + j] != pattern[j])
                    {
                        found = false;
                        break;
                    }

                if (found)
                    return i;
            }

            return -1;

        } // method IndexOf

        /// <summary>
        /// Преобразование переводов строк ИРБИС в Windows.
        /// </summary>
        public static string? IrbisToWindows (string? text)
        {
            if (ReferenceEquals(text, null) || text.Length == 0)
                return text;

            if (!text.Contains(Constants.IrbisDelimiter))
                return text;

            var result = text.Replace
                (
                    Constants.IrbisDelimiter,
                    Constants.WindowsDelimiter
                );

            return result;

        } // method IrbisToWindows

        /// <summary>
        /// Заменяет переводы строк ИРБИС на Windows.
        /// Замена происходит на месте.
        /// </summary>
        public static void IrbisToWindows
            (
                byte[]? text
            )
        {
            if (ReferenceEquals(text, null) || text.Length == 0)
                return;

            var index = 0;
            while (true)
            {
                index = IndexOf(text, Constants.IrbisDelimiterBytes, index);
                if (index < 0)
                    break;

                Array.Copy(Constants.WindowsDelimiterBytes, 0, text, index, Constants.WindowsDelimiterBytes.Length);
            }

        } // method IrbisToWindows

        /// <summary>
        /// Разбивает текст на строки в соответствии с ИРБИС-разделителями.
        /// </summary>
        public static string[] SplitIrbisToLines
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return Array.Empty<string>();
            }

            var provenText = IrbisToWindows(text)!;
            var result = string.IsNullOrEmpty(provenText)
                ? new[] { string.Empty }
                : provenText.Split
                    (
                        Constants.ShortIrbisDelimiterBytes,
                        StringSplitOptions.None
                    );

            return result;

        } // method SplitIrbisToLines

        /// <summary>
        /// Преобразует текст в нижний регистр.
        /// </summary>
        public static string? ToLower
            (
                string? text
            )
        {
            if (ReferenceEquals(text, null) || text.Length == 0)
            {
                return text;
            }

            var result = text.ToLowerInvariant();

            return result;

        } // method ToLower

        /// <summary>
        /// Преобразует текст в верхний регистр.
        /// </summary>
        public static string? ToUpper
            (
                string? text
            )
        {
            if (ReferenceEquals(text, null) || text.Length == 0)
            {
                return text;
            }

            var result = text.ToUpperInvariant();

            return result;

        } // method ToUpper

        /// <summary>
        /// Заменяет переводы строк DOS/Windows на ИРБИС.
        /// </summary>
        public static string? WindowsToIrbis
            (
                string? text
            )
        {
            if (ReferenceEquals(text, null) || text.Length == 0)
            {
                return text;
            }

            if (!text.Contains(Constants.WindowsDelimiter))
            {
                return text;
            }

            var result = text.Replace
                (
                    Constants.WindowsDelimiter,
                    Constants.IrbisDelimiter
                );

            return result;

        } // method WindowsToIrbis

        /// <summary>
        /// Заменяет переводы строк Windows на ИРБИС.
        /// Замена происходит на месте.
        /// </summary>
        public static void WindowsToIrbis
            (
                byte[]? text
            )
        {
            if (ReferenceEquals(text, null) || text.Length == 0)
            {
                return;
            }

            var index = 0;
            while (true)
            {
                index = IndexOf(text, Constants.WindowsDelimiterBytes, index);
                if (index < 0)
                {
                    break;
                }

                Array.Copy(Constants.IrbisDelimiterBytes, 0, text, index, Constants.IrbisDelimiterBytes.Length);
            }

        } // method WindowsToIrbis

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static bool IsGood (this Response? response) =>
            response is not null && response.CheckReturnCode();

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static bool IsGood (this Response? response, params int[] goodCodes) =>
            response is not null && response.CheckReturnCode((goodCodes));

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static T? Transform<T> (this Response? response, Func<Response, T?> transformer) where T : class =>
            response.IsGood() ? transformer(response!) : null;

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static T? TransformNoCheck<T> (this Response? response, Func<Response, T?> transformer)  where T : class
        {
            var result = response is not null ? transformer(response) : null;

            return result;

        } // method TransformNoCheck

        /// <summary>
        /// Remove comments from the format.
        /// </summary>
        public static string? RemoveComments
            (
                string? text
            )
        {
            const char zero = '\0';

            if (ReferenceEquals(text, null) || text.Length == 0)
            {
                return text;
            }

            if (!text.Contains("/*"))
            {
                return text;
            }

            int index = 0, length = text.Length;
            var result = new StringBuilder(length);
            var state = zero;

            while (index < length)
            {
                var c = text[index];

                switch (state)
                {
                    case '\'':
                    case '"':
                    case '|':
                        if (c == state)
                        {
                            state = zero;
                        }
                        result.Append(c);
                        break;

                    default:
                        if (c == '/')
                        {
                            if (index + 1 < length && text[index + 1] == '*')
                            {
                                while (index < length)
                                {
                                    c = text[index];
                                    if (c == '\r' || c == '\n')
                                    {
                                        result.Append(c);
                                        break;
                                    }

                                    index++;
                                }
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        else if (c == '\'' || c == '"' || c == '|')
                        {
                            state = c;
                            result.Append(c);
                        }
                        else
                        {
                            result.Append(c);
                        }
                        break;
                }

                index++;
            } // while

            return result.ToString();

        } // method RemoveComments

        /// <summary>
        /// Prepare the dynamic format string.
        /// </summary>
        /// <remarks>Dynamic format string
        /// mustn't contains comments and
        /// string delimiters (no matter
        /// real or IRBIS).
        /// </remarks>
        public static string? PrepareFormat
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = RemoveComments(text);
            if (ReferenceEquals(text, null) || text.Length == 0) //-V3063
            {
                return text;
            }

            var length = text.Length;
            var flag = false;
            for (var i = 0; i < length; i++)
            {
                if (text[i] < ' ')
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                return text;
            }

            var result = new StringBuilder(length);
            for (var i = 0; i < length; i++)
            {
                var c = text[i];
                if (c >= ' ')
                {
                    result.Append(c);
                }
            }

            return result.ToString();

        } // method PrepareFormat

        private static void _AppendIrbisLine
            (
                StringBuilder builder,
                string format,
                params object[] args
            )
        {
            builder.AppendFormat(format, args);
            builder.Append(Constants.IrbisDelimiter);
        }

        /// <summary>
        /// Кодирование подполя.
        /// </summary>
        public static void EncodeSubField
            (
                StringBuilder builder,
                SubField subField
            )
        {
            if (subField.RepresentsValue)
            {
                builder.Append(subField.Value);
            }
            else
            {
                builder.AppendFormat
                    (
                        "{0}{1}{2}",
                        SubField.Delimiter,
                        subField.Code,
                        subField.Value
                    );
            }

        } // method EncodeSubField

        /// <summary>
        /// Кодирование одного поля.
        /// </summary>
        public static void EncodeField
            (
                StringBuilder builder,
                Field field
            )
        {
            builder.AppendFormat
                (
                    "{0}#",
                    field.Tag
                );

            foreach (var subField in field.Subfields)
            {
                EncodeSubField
                    (
                        builder,
                        subField
                    );
            }

            builder.Append(Constants.IrbisDelimiter);

        } // method EncodeField

        /// <summary>
        /// Кодирование записи в клиентское представление.
        /// </summary>
        /// <param name="record">Запись для кодирования.</param>
        /// <returns>
        /// Закодированная запись.
        /// </returns>
        public static string EncodeRecord
            (
                Record record
            )
        {
            var result = new StringBuilder();

            _AppendIrbisLine
                (
                    result,
                    "{0}#{1}",
                    record.Mfn,
                    (int)record.Status
                );
            _AppendIrbisLine
                (
                    result,
                    "0#{0}",
                    record.Version
                );

            foreach (var field in record.Fields)
            {
                EncodeField
                (
                    result,
                    field
                );
            }

            return result.ToString();

        } // method EncodeRecord

        private static ReadOnlyMemory<char> _ReadTo
            (
                StringReader reader,
                char delimiter
            )
        {
            var result = new StringBuilder();

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                var c = (char)next;
                if (c == delimiter)
                {
                    break;
                }
                result.Append(c);
            }

            return result.ToString().AsMemory();

        } // method _ReadTo

        /// <summary>
        /// Разбор одной строки (поля).
        /// </summary>
        public static Field ParseLine
            (
                string line
            )
        {
            var reader = new StringReader(line);
            var result = new Field
            {
                Tag = Private.ParseInt32(_ReadTo(reader, '#')),
                Value = _ReadTo(reader, '^').EmptyToNull()
            };

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }
                var code = char.ToLower((char)next);
                var text = _ReadTo(reader, '^');
                var subField = new SubField
                {
                    Code = code,
                    Value = text.ToString()
                };
                result.Subfields.Add(subField);
            }

            return result;

        } // method ParseLine

        /// <summary>
        /// Parse MFN, status and version of the record
        /// </summary>
        public static Record ParseMfnStatusVersion
            (
                string line1,
                string line2,
                Record record
            )
        {
            var regex = new Regex(@"^(-?\d+)\#(\d*)?");
            var match = regex.Match(line1);
            record.Mfn = Math.Abs(Private.ParseInt32(match.Groups[1].Value));
            if (match.Groups[2].Length > 0)
            {
                record.Status = (RecordStatus) Private.ParseInt32(match.Groups[2].Value);
            }
            match = regex.Match(line2);
            if (match.Groups[2].Length > 0)
            {
                record.Version = Private.ParseInt32(match.Groups[2].Value);
            }

            return record;

        } // method ParseMfnStatusVersion

        /// <summary>
        /// Parse server response for WriteRecordsCommand.
        /// </summary>
        public static Record ParseResponseForWriteRecords
            (
                Response response,
                Record record
            )
        {
            record.Fields.Clear();

            var whole = response.RequireUtf();
            var split = whole.Split('\x1E');

            ParseMfnStatusVersion
                (
                    split[0],
                    split[1],
                    record
                );

            for (var i = 2; i < split.Length; i++)
            {
                var line = split[i];
                var field = ParseLine(line);
                if (field.Tag > 0)
                {
                    record.Fields.Add(field);
                }
            }

            return record;

        } // method ParseResponseForWriteRecords

        /// <summary>
        /// Merges the specified arrays.
        /// </summary>
        /// <param name="arrays">Arrays to merge.</param>
        /// <returns>Array that consists of all <paramref name="arrays"/>
        /// items.</returns>
        /// <typeparam name="T">Type of array item.</typeparam>
        /// <exception cref="ArgumentNullException">
        /// At least one of <paramref name="arrays"/> is <c>null</c>.
        /// </exception>
        public static T[] Merge<T>
            (
                params T[]?[] arrays
            )
        {
            var resultLength = 0;
            for (var i = 0; i < arrays.Length; i++)
            {
                var item = arrays[i];
                if (ReferenceEquals(item, null))
                {
                    throw new ArgumentNullException(nameof(arrays));
                }

                resultLength += item.Length;
            }

            var result = new T[resultLength];
            var offset = 0;
            for (var i = 0; i < arrays.Length; i++)
            {
                var item = arrays[i]!;
                item.CopyTo(result, offset);
                offset += item.Length;
            }

            return result;

        } // method Merge

        /// <summary>
        /// Выборка элемента из массива.
        /// </summary>
        /// <remarks>
        /// Возможна отрицательная нумерация
        /// (означает индекс с конца массива).
        /// При выходе за границы массива
        /// выдаётся значение по умолчанию.
        /// </remarks>
        public static T? GetOccurrence<T> (this T[] array, int occurrence)
        {
            var length = array.Length;

            occurrence = occurrence >= 0 ? occurrence : length + occurrence;

            var result = default(T);

            if (length != 0 && occurrence >= 0 && occurrence < length)
                result = array[occurrence];

            return result;

        } // method GetOccurrence

    } // class Private

    /// <summary>
    /// Разнообразные полезные методы расширения.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Однобайтовая кодировка по умолчанию.
        /// Как правило, кодовая страница 1251.
        /// </summary>
        public static Encoding Ansi => Windows1251;

        /// <summary>
        /// Регистрация кодировок, основанных на кодовых страницах.
        /// </summary>
        public static void RegisterEncodingProviders() =>
            Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

        private static Encoding? _windows1251;

        /// <summary>
        /// Получение кодировки Windows-1251 (cyrillic) <see cref="Encoding"/>.
        /// </summary>
        public static Encoding Windows1251
        {
            [DebuggerStepThrough]
            get
            {
                if (ReferenceEquals(_windows1251, null))
                {
                    RegisterEncodingProviders();
                    _windows1251 = Encoding.GetEncoding(1251);
                }

                return _windows1251;
            }

        } // property Windows1251

        /// <summary>
        /// Проверка: валиден ли код подполя.
        /// </summary>
        public static bool IsValidSubFieldCode(char code) =>
            code is >= Constants.FirstCode and <= Constants.LastCode and not '^';

        /// <summary>
        /// Верификация кода подполя с выбросом исключения.
        /// </summary>
        public static bool VerifySubFieldCode(char code, bool trhowOnError = true) =>
            IsValidSubFieldCode(code) || (trhowOnError ? throw new IrbisException() : false);

        /// <summary>
        /// Нормализация кода подполя.
        /// </summary>
        public static char NormalizeCode (char code) => char.ToLowerInvariant(code);

        /// <summary>
        /// Проверка: валидно ли значение подполя.
        /// </summary>
        public static bool IsValidSubFieldValue
            (
                ReadOnlySpan<char> value
            )
        {
            foreach (var c in value)
            {
                if (c == SubField.Delimiter)
                {
                    return false;
                }
            }

            return true;

        } // method IsValidSubFieldValue

        /// <summary>
        /// Верификация значения подполя с выбромо исключения.
        /// </summary>
        public static bool VerifySubFieldValue(ReadOnlySpan<char> value, bool throwOnError = true) =>
            IsValidSubFieldValue(value) || (throwOnError ? throw new IrbisException() : false);

        /// <summary>
        /// Нормализация значения подполя.
        /// </summary>
        public static string? NormalizeSubFieldValue
            (
                string? value
            )
        {
            if (ReferenceEquals(value, null) || value.Length == 0)
            {
                return value;
            }

            var result = value.Trim();

            return result;

        } // method Normalize

        /// <summary>
        /// Первое вхождение подполя с указанным кодом.
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this IEnumerable<SubField> subFields,
                char code
            )
        {
            foreach (var subField in subFields)
            {
                if (subField.Code.SameChar(code))
                {
                    return subField;
                }
            }

            return null;
        } // method GetFirstSubField

        /// <summary>
        /// Первое вхождение подполя с одним из указанных кодов.
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {
            foreach (var subField in subFields)
            {
                foreach (var code in codes)
                {
                    if (subField.Code.SameChar(code))
                    {
                        return subField;
                    }
                }
            }

            return null;

        } // method GetFirstSubField

        /// <summary>
        /// Первое вхождение подполя с указанными кодом
        /// и значением (с учётом регистра символов).
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this IEnumerable<SubField> subFields,
                char code,
                string? value
            )
        {
            foreach (var subField in subFields)
            {
                if (subField.Code.SameChar(code)
                    && subField.Value.SameString(value))
                {
                    return subField;
                }
            }

            return null;

        } // method GetFirstSubField

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<SubField> subFields,
                char code
            )
        {
            List<SubField>? result = null;
            foreach (var subField in subFields)
            {
                if (subField.Code.SameChar(code))
                {
                    result ??= new List<SubField>();
                    result.Add(subField);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        } // method GetSubField

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {
            List<SubField>? result = null;
            foreach (var subField in subFields)
            {
                if (subField.Code.SameChar(codes))
                {
                    result ??= new();
                    result.Add(subField);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        } // method GetSubField

        /// <summary>
        /// Выполнение неких действий над подполями.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<SubField> subFields,
                Action<SubField>? action
            )
        {
            var result = subFields.ToArray();

            if (!ReferenceEquals(action, null))
            {
                foreach (var subField in result)
                {
                    action(subField);
                }
            }

            return result;
        } // method GetSubField

        /// <summary>
        /// Фильтрация подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<Field> fields,
                Func<Field, bool> fieldPredicate,
                Func<SubField, bool> subPredicate
            )
        {
            List<SubField>? result = null;
            foreach (var field in fields)
            {
                if (fieldPredicate(field))
                {
                    foreach (SubField subField in field.Subfields)
                    {
                        if (subPredicate(subField))
                        {
                            result ??= new List<SubField>();
                            result.Add(subField);
                        }
                    }
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        } // method GetSubField

        /// <summary>
        /// Получение значения подполя.
        /// </summary>
        public static string[] GetSubFieldValue
            (
                this IEnumerable<SubField> subFields
            )
        {
            List<string>? result = null;
            foreach (var subField in subFields.NonNullItems())
            {
                var value = subField.Value;
                if (!ReferenceEquals(value, null) && value.Length != 0)
                {
                    result ??= new List<string>();
                    result.Add(value);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<string>()
                : result.ToArray();

        } // method GetSubFieldValue

        /// <summary>
        /// Добавление подполя, при условии, что оно не пустое.
        /// </summary>
        public static Field AddNonEmptySubField
            (
                this Field field,
                char code,
                string? value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                field.Add(code, value);
            }

            return field;

        } // method AddNonEmptySubField

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        public static Field AddNonEmptySubField
            (
                this Field field,
                char code,
                bool flag,
                string? value
            )
        {
            if (flag && !string.IsNullOrEmpty(value))
            {
                field.Add(code, value);
            }

            return field;
        }

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        public static Field AddNonEmptySubField
            (
                this Field field,
                char code,
                int? value
            )
        {
            if (value.HasValue)
            {
                field.Add(code, value.Value);
            }

            return field;
        }

        /// <summary>
        /// Добавление подполя.
        /// </summary>
        public static Field AddNonEmptySubField
            (
                this Field field,
                char code,
                long? value
            )
        {
            if (value.HasValue)
            {
                field.Add(code, value.Value);
            }

            return field;
        }

        /// <summary>
        /// Добавление подполей.
        /// </summary>
        public static Field AddSubFields
            (
                this Field field,
                IEnumerable<SubField>? subFields
            )
        {
            if (!ReferenceEquals(subFields, null))
            {
                foreach (var subField in subFields)
                {
                    field.Subfields.Add(subField);
                }
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Все подполя.
        /// </summary>
        public static SubField[] AllSubFields
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .SelectMany(field => field.Subfields)
                .NonNullItems()
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Apply subfield value.
        /// </summary>
        public static Field ApplySubField
            (
                this Field field,
                char code,
                object? value
            )
        {
            if (code == SubField.NoCode)
                return field;

            if (ReferenceEquals(value, null))
            {
                field.RemoveSubField(code);
            }
            else
            {
                var subField = field.GetFirstSubField(code);
                if (ReferenceEquals(subField, null))
                {
                    subField = new SubField { Code = code };
                    field.Subfields.Add(subField);
                }
                subField.Value = value.ToString();
            }

            return field;
        }

        /// <summary>
        /// Apply subfield value.
        /// </summary>
        public static Field ApplySubField
            (
                this Field field,
                char code,
                bool value,
                string text
            )
        {
            if (code == SubField.NoCode)
            {
                return field;
            }

            if (value == false)
            {
                field.RemoveSubField(code);
            }
            else
            {
                var subField = field.GetFirstSubField(code);
                if (ReferenceEquals(subField, null))
                {
                    subField = new SubField { Code = code };
                    field.Subfields.Add(subField);
                }
                subField.Value = text;
            }

            return field;
        }

        /// <summary>
        /// Apply subfield value.
        /// </summary>
        public static Field ApplySubField
            (
                this Field field,
                char code,
                string? value
            )
        {
            if (code == SubField.NoCode)
            {
                return field;
            }

            if (string.IsNullOrEmpty(value))
            {
                field.RemoveSubField(code);
            }
            else
            {
                var subField = field.GetFirstSubField(code);
                if (ReferenceEquals(subField, null))
                {
                    subField = new SubField { Code = code };
                    field.Subfields.Add(subField);
                }
                subField.Value = value;
            }

            return field;
        }

        // ==========================================================

        /// <summary>
        /// Отбор подполей с указанными кодами.
        /// </summary>
        public static SubField[] FilterSubFields
            (
                this IEnumerable<SubField> subFields,
                params char[] codes
            )
        {

            return subFields
                .Where
                    (
                        subField => subField.Code.IsOneOf(codes)
                    )
                .ToArray();
        }

        /// <summary>
        /// Отбор подполей с указанными кодами.
        /// </summary>
        public static SubField[] FilterSubFields
            (
                this Field field,
                params char[] codes
            )
        {
            return field.Subfields
                .FilterSubFields(codes);
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                int tag
            )
        {
            return fields
                .Where(field => field.Tag == tag)
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetField
            (
                this IEnumerable<Field> fields,
                int tag,
                int occurrence
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (occurrence == 0)
                    {
                        return field;
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                int[] tags
            )
        {
            return fields
                .Where(field => field.Tag.IsOneOf(tags))
                .ToArray();
        }

        ///// <summary>
        ///// Фильтрация полей.
        ///// </summary>
        //[NotNull]
        //[ItemNotNull]
        //public static RecordField[] GetField
        //    (
        //        this RecordFieldCollection fields,
        //        params int[] tags
        //    )
        //{
        //    Code.NotNull(fields, "fields");

        //    List<RecordField> result = null;
        //    int count = fields.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (fields[i].Tag.OneOf(tags))
        //        {
        //            if (ReferenceEquals(result, null))
        //            {
        //                result = new List<RecordField>();
        //            }
        //            result.Add(fields[i]);
        //        }
        //    }

        //    return ReferenceEquals(result, null)
        //        ? EmptyArray
        //        : result.ToArray();
        //}

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetField
            (
                this IEnumerable<Field> fields,
                int[] tags,
                int occurrence
            )
        {
            return fields
                .GetField(tags)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Func<Field, bool> predicate
            )
        {
            return fields
                .NonNullItems()
                .Where(predicate)
                .ToArray();
        }

        /// <summary>
        /// Выполнение неких действий над полями.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Action<Field>? action
            )
        {
            var result = fields.ToArray();
            if (!ReferenceEquals(action, null))
            {
                foreach (var field in result)
                {
                    action(field);
                }
            }

            return result;
        } // method GetField

        /// <summary>
        /// Выполнение неких действий над полями и подполями.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Action<Field>? fieldAction,
                Action<SubField>? subFieldAction
            )
        {
            var result = fields.ToArray();
            if (!ReferenceEquals(fieldAction, null)
                || !ReferenceEquals(subFieldAction, null))
            {
                foreach (var field in result)
                {
                    fieldAction?.Invoke(field);

                    if (!ReferenceEquals(subFieldAction, null))
                    {
                        foreach (var subField in field.Subfields)
                        {
                            subFieldAction(subField);
                        }
                    }
                }
            } // method GetField

            return result;
        }

        /// <summary>
        /// Выполнение неких действий над подполями.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Action<SubField>? action
            )
        {
            var result = fields.ToArray();
            if (!ReferenceEquals(action, null))
            {
                foreach (var field in result)
                {
                    foreach (var subField in field.Subfields)
                    {
                        action(subField);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Func<SubField, bool> predicate
            )
        {
            return fields
                .Where(field => field.Subfields.Any(predicate))
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                char[] codes,
                Func<SubField, bool> predicate
            )
        {
            return fields
                .NonNullItems()
                .Where
                    (
                        field => field.Subfields
                            .NonNullItems()
                            .Any
                                (
                                    sub => sub.Code.SameChar(codes)
                                        && predicate(sub)
                                )
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                char[] codes,
                params string[] values
            )
        {
            return fields
                .Where
                    (
                        field => field.Subfields
                            .Any
                                (
                                    sub => sub.Code.SameChar(codes)
                                        && sub.Value.SameString(values)
                                )
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                char code,
                string? value
            )
        {
            return fields
                .Where
                    (
                        field => field.Subfields
                            .Any
                                (
                                    sub => sub.Code.SameChar(code)
                                        && sub.Value.SameString(value)
                                )
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                int[] tags,
                char[] codes,
                string[] values
            )
        {
            return fields
                .Where(field => field.Tag.IsOneOf(tags))
                .Where
                    (
                        field => field.Subfields
                            .Any
                                (
                                    sub => sub.Code.SameChar(codes)
                                        && sub.Value.SameString(values)
                                )
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetField
            (
                this IEnumerable<Field> fields,
                Func<Field, bool> fieldPredicate,
                Func<SubField, bool> subPredicate
            )
        {
            return fields
                .Where(fieldPredicate)
                .Where(field => field.Subfields.Any(subPredicate))
                .ToArray();
        }

        // ==========================================================

        /// <summary>
        /// Количество повторений поля.
        /// </summary>
        public static int GetFieldCount
            (
                this IEnumerable<Field> fields,
                int tag
            )
        {
            var result = 0;

            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    result++;
                }
            }

            return result;
        } // method GetFieldCount

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetFieldRegex
            (
                this IEnumerable<Field> fields,
                string tagRegex
            )
        {
            return fields
                .Where
                    (
                        field =>
                        {
                            var tag = field.Tag.ToInvariantString();

                            return !string.IsNullOrEmpty(tag)
                                && Regex.IsMatch
                                   (
                                       tag,
                                       tagRegex
                                   );
                        }
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetFieldRegex
            (
                this IEnumerable<Field> fields,
                string tagRegex,
                int occurrence
            )
        {
            return fields
                .GetFieldRegex(tagRegex)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetFieldRegex
            (
                this IEnumerable<Field> fields,
                int[] tags,
                string textRegex
            )
        {
            return fields
                .GetField(tags)
                .Where
                    (
                        field =>
                        {
                            var value = field.Value;
                            return !ReferenceEquals(value, null) && value.Length != 0
                                   && Regex.IsMatch(value, textRegex);
                        })
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetFieldRegex
            (
                this IEnumerable<Field> fields,
                int[] tags,
                string textRegex,
                int occurrence
            )
        {
            return fields
                .GetFieldRegex(tags, textRegex)
                .GetOccurrence(occurrence);
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] GetFieldRegex
            (
                this IEnumerable<Field> fields,
                int[] tags,
                char[] codes,
                string textRegex
            )
        {
            var regex = new Regex(textRegex);
            return fields
                .GetField(tags)
                .Where(field => field.FilterSubFields(codes)
                    .Any(sub =>
                    {
                        var value = sub.Value;

                        return !ReferenceEquals(value, null) && value.Length != 0
                            && regex.IsMatch(value);
                    }))
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field? GetFieldRegex
            (
                this IEnumerable<Field> fields,
                int[] tags,
                char[] codes,
                string textRegex,
                int occurrence
            )
        {
            return fields
                .GetFieldRegex(tags, codes, textRegex)
                .GetOccurrence(occurrence);
        }

        // ==========================================================

        /// <summary>
        /// Получение значения поля.
        /// </summary>
        public static string[] GetFieldValue
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .Select (field => field.Value!)
                .Where(line => !ReferenceEquals(line, null) && line.Length != 0)
                .ToArray();
        }

        /// <summary>
        /// Непустые значения полей с указанным тегом.
        /// </summary>
        public static string[] GetFieldValue
            (
                this IEnumerable<Field> fields,
                int tag
            )
        {
            var result = new List<string>();
            foreach (var field in fields.NonNullItems())
                if (field.Tag == tag && !ReferenceEquals(field.Value, null) && field.Value.Length != 0)
                    result.Add(field.Value);

            return result.ToArray();
        }

        /// <summary>
        /// Первое вхождение поля с указанным тегом.
        /// </summary>
        public static Field? GetFirstField
            (
                this IEnumerable<Field> fields,
                int tag
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    return field;
                }
            }

            return null;
        } // method GetFirstField

        /// <summary>
        /// Первое вхождение поля с указанным тегом.
        /// </summary>
        public static Field? GetFirstField
            (
                this IEnumerable<Field> fields,
                int tag1,
                int tag2
            )
        {
            foreach (var field in fields)
            {
                var tag = field.Tag;
                if (tag == tag1 || tag == tag2)
                {
                    return field;
                }
            }

            return null;
        } // method GetFirstField

        /// <summary>
        /// Первое вхождение поля с указанным тегом.
        /// </summary>
        public static Field? GetFirstField
            (
                this IEnumerable<Field> fields,
                int tag1,
                int tag2,
                int tag3
            )
        {
            foreach (var field in fields)
            {
                var tag = field.Tag;
                if (tag == tag1 || tag == tag2 || tag == tag3)
                {
                    return field;
                }
            }

            return null;
        } // method GetFirstField

        /// <summary>
        /// Первое вхождение поля с любым из перечисленных тегов.
        /// </summary>
        public static Field? GetFirstField
            (
                this IEnumerable<Field> fields,
                params int[] tags
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag.IsOneOf(tags))
                {
                    return field;
                }
            }

            return null;
        }

        // ==========================================================

        ///// <summary>
        ///// Значение первого поля с указанным тегом или <c>null</c>.
        ///// </summary>
        //[CanBeNull]
        //public static string GetFirstFieldValue
        //    (
        //        this IEnumerable<RecordField> fields,
        //        int tag
        //    )
        //{
        //    Code.NotNull(fields, "fields");

        //    foreach (RecordField field in fields)
        //    {
        //        if (field.Tag == tag)
        //        {
        //            return field.Value;
        //        }
        //    }

        //    return null;
        //}

        // ==========================================================

        /// <summary>
        /// Gets the first subfield.
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this Field field,
                char code
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return subFields[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Первое вхождение подполя, соответствующего указанным
        /// критериям.
        /// </summary>
        public static SubField? GetFirstSubField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    var subFields = field.Subfields;
                    var count = subFields.Count;
                    for (var i = 0; i < count; i++)
                    {
                        if (subFields[i].Code.SameChar(code))
                        {
                            return subFields[i];
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получение текста указанного подполя
        /// </summary>
        public static string? GetFirstSubFieldValue
            (
                this Field field,
                char code
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return subFields[i].Value;
                }
            }

            return default;
        }

        /// <summary>
        /// Значение первого подполя с указанными тегом и кодом
        /// или <c>null</c>.
        /// </summary>
        public static string? GetFirstSubFieldValue
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    foreach (var subField in field.Subfields)
                    {
                        if (subField.Code.SameChar(code))
                        {
                            return subField.Value;
                        }
                    }
                }
            }

            return default;
        }

        // ==========================================================

        /// <summary>
        /// Перечень подполей с указанным кодом.
        /// </summary>
        public static SubField[] GetSubField
            (
                this Field field,
                char code
            )
        {
            List<SubField>? result = null;
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    result ??= new List<SubField>();
                    result.Add(subFields[i]);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Указанное повторение подполя с данным кодом.
        /// </summary>
        public static SubField? GetSubField
            (
                this Field field,
                char code,
                int occurrence
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    if (occurrence == 0)
                    {
                        return subFields[i];
                    }
                    occurrence--;
                }
            }

            return null;
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<Field> fields,
                char code
            )
        {
            List<SubField>? result = null;
            foreach (var field in fields)
            {
                var subFields = field.Subfields;
                var count = subFields.Count;
                for (var i = 0; i < count; i++)
                {
                    if (subFields[i].Code.SameChar(code))
                    {
                        result ??= new List<SubField>();
                        result.Add(subFields[i]);
                    }
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<Field> fields,
                params char[] codes
            )
        {
            List<SubField>? result = null;
            foreach (var field in fields)
            {
                var subFields = field.Subfields;
                var count = subFields.Count;
                for (var i = 0; i < count; i++)
                {
                    if (subFields[i].Code.IsOneOf(codes))
                    {
                        result ??= new List<SubField>();
                        result.Add(subFields[i]);
                    }
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Получение подполей.
        /// </summary>
        public static SubField[] GetSubField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            List<SubField>? result = null;
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    foreach (var subField in field.Subfields)
                    {
                        if (subField.Code.SameChar(code))
                        {
                            result ??= new List<SubField>();
                            result.Add(subField);
                        }
                    }
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Получение подполя.
        /// </summary>
        public static SubField? GetSubField
            (
                this IEnumerable<Field> fields,
                int tag,
                int fieldOccurrence,
                char code,
                int subOccurrence
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (fieldOccurrence == 0)
                    {
                        var subFields = field.Subfields;
                        var subCount = subFields.Count;
                        for (var j = 0; j < subCount; j++)
                        {
                            if (subFields[j].Code.SameChar(code))
                            {
                                if (subOccurrence == 0)
                                {
                                    return subFields[j];
                                }
                                subOccurrence--;
                            }
                        }

                        return null;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получение подполя.
        /// </summary>
        public static SubField? GetSubField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code,
                int occurrence
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    var subFields = field.Subfields;
                    var subCount = subFields.Count;
                    for (var j = 0; j < subCount; j++)
                    {
                        if (subFields[j].Code.SameChar(code))
                        {
                            if (occurrence == 0)
                            {
                                return subFields[j];
                            }
                            occurrence--;
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Получение текста указанного подполя.
        /// </summary>
        public static string? GetSubFieldValue
            (
                this Field field,
                char code,
                int occurrence
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    if (occurrence == 0)
                    {
                        return subFields[i].Value;
                    }
                    occurrence--;
                }
            }

            return default;
        }

        // ==========================================================

        /// <summary>
        /// Непустые значения подполей с указанными тегом и кодом.
        /// </summary>
        public static string[] GetSubFieldValue
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            var result = new List<string>();
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    foreach (var subField in field.Subfields)
                    {
                        if (subField.Code.SameChar(code)
                            && !ReferenceEquals(subField.Value, null) && subField.Value.Length != 0)
                        {
                            result.Add(subField.Value);
                        }
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Есть хотя бы одно подполе с указанным кодом?
        /// </summary>
        public static bool HaveSubField
            (
                this Field field,
                char code
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Есть хотя бы одно подполе с указанным кодом?
        /// </summary>
        public static bool HaveSubField
            (
                this Field field,
                char code,
                string value
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code)
                    && subFields[i].Value.SameString(value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Есть хотя бы одно поле с любым из указанных кодов?
        /// </summary>
        public static bool HaveSubField
            (
                this Field field,
                params char[] codes
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(codes))
                {
                    return true;
                }
            }

            return false;
        }

        // ==========================================================

        /// <summary>
        /// Нет ни одного подполя с указанным кодом?
        /// </summary>
        public static bool HaveNotSubField
            (
                this Field field,
                char code
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(code))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Нет ни одного подполя с указанными кодами?
        /// </summary>
        public static bool HaveNotSubField
            (
                this Field field,
                params char[] codes
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                if (subFields[i].Code.SameChar(codes))
                {
                    return false;
                }
            }

            return true;
        }

        // ==========================================================

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] NotNullTag
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .Where
                    (
                        field => field.Tag != 0
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] NotNullValue
            (
                this IEnumerable<Field> fields
            )
        {
            return fields .Where(field => !ReferenceEquals(field.Value, null) && field.Value.Length != 0)
                .ToArray();
        }

        /// <summary>
        /// Меняем значение подполя.
        /// </summary>
        public static Field ReplaceSubField
            (
                this Field field,
                char code,
                string oldValue,
                string newValue
            )
        {
            var subFields = field.Subfields;
            var count = subFields.Count;
            for (var i = 0; i < count; i++)
            {
                var subField = subFields[i];
                if (subField.Code.SameChar(code))
                {
                    if (subField.Value == oldValue)
                    {
                        subField.Value = newValue;
                    }
                }
            }

            return field;
        }

        /// <summary>
        /// Меняем значение подполя.
        /// </summary>
        public static Field ReplaceSubField
            (
                this Field field,
                char code,
                string newValue,
                bool ignoreCase
            )
        {
            var oldValue = field.GetSubFieldValue
                (
                    code
                );
            var changed = string.CompareOrdinal(oldValue, newValue);

            if (changed != 0)
            {
                field.SetSubFieldValue(code, newValue);
            }

            return field;

        }

        /// <summary>
        /// Get unknown subfields.
        /// </summary>
        public static SubField[] GetUnknownSubFields
            (
                this IEnumerable<SubField> subFields,
                ReadOnlySpan<char> knownCodes
            )
        {
            List<SubField>? result = null;
            foreach (var subField in subFields)
            {
                if (subField.Code != '\0'
                    && !subField.Code.SameChar(knownCodes))
                {
                    result ??= new List<SubField>();
                    result.Add(subField);
                }
            }

            return ReferenceEquals(result, null)
                ? Array.Empty<SubField>()
                : result.ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithNullTag
            (
                this IEnumerable<Field> fields
            )
        {
            return fields
                .Where
                    (
                        field => field.Tag == 0
                    )
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithNullValue
            (
                this IEnumerable<Field> fields
            )
        {
            return fields .Where(field => ReferenceEquals(field.Value, null) || field.Value.Length == 0)
                .ToArray();
        }

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithoutSubFields (this IEnumerable<Field> fields) =>
            fields.Where (field => field.Subfields.Count == 0).ToArray();

        /// <summary>
        /// Есть ли в поле подполя с кодами?
        /// </summary>
        public static bool HaveSubFields (this Field field)
        {
            foreach (var subfield in field.Subfields)
                if (subfield.Code != SubField.NoCode)
                    return true;

            return false;

        } // method HaveSubFields

        /// <summary>
        /// Фильтрация полей.
        /// </summary>
        public static Field[] WithSubFields (this IEnumerable<Field> fields) =>
            fields.Where (field => field.HaveSubFields()).ToArray();

        /// <summary>
        /// Поиск поля, которое обязательно должно быть.
        /// </summary>
        public static Field RequireField
            (
                this IEnumerable<Field> fields,
                int tag,
                int occurrence = default
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (occurrence == 0)
                    {
                        return field;
                    }
                    occurrence--;
                }
            }

            throw new KeyNotFoundException($"Tag={tag}");

        } // method RequireField

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        public static IEnumerable<Field> EnumerateField (this IEnumerable<Field> fields, int tag)
        {
            foreach (var field in fields)
                if (field.Tag == tag)
                    yield return field;

        } // method EnumerateField

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        public static IEnumerable<Field> EnumerateField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (field.HaveSubField(code))
                    {
                        yield return field;
                    }
                }
            }
        } // method EnumerateField

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        public static IEnumerable<Field> EnumerateField
            (
                this IEnumerable<Field> fields,
                int tag,
                char code,
                string value
            )
        {
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    if (field.HaveSubField(code, value))
                    {
                        yield return field;
                    }
                }
            }
        } // method EnumerateField

    } // class Utility

    /// <summary>
    /// Общие для ИРБИС64 константы.
    /// </summary>
    public static class Constants
    {
        #region Коды команд для сервера ИРБИС64

        /// <summary>
        /// Получение признака монопольной блокировки базы данных.
        /// </summary>
        public const string ExclusiveDatabaseLock = "#";

        /// <summary>
        /// Новый полнотекстовый поиск для ИРБИС64+.
        /// </summary>
        public const string NewFulltextSearch = "&";

        /// <summary>
        /// Фасеты по результатам поиска.
        /// </summary>
        public const string SearchCell = "$";

        /// <summary>
        /// Получение списка удаленных, неактуализированных
        /// и заблокированных записей.
        /// </summary>
        public const string RecordList = "0";

        /// <summary>
        /// Получение версии сервера.
        /// </summary>
        public const string ServerInfo = "1";

        /// <summary>
        /// Получение статистики по базе данных.
        /// </summary>
        public const string DatabaseStat = "2";

        /// <summary>
        /// IRBIS_FORMAT_ISO_GROUP.
        /// </summary>
        public const string FormatIsoGroup = "3";

        /// <summary>
        /// Сбросить запущенную задачу только на сервере.
        /// </summary>
        public const string StopClientProcess = "4";

        /// <summary>
        /// Глобальная корректировка.
        /// </summary>
        /// <remarks>IRBIS_GBL</remarks>
        public const string GlobalCorrection = "5";

        /// <summary>
        /// Сохранение группы записей.
        /// </summary>
        public const string SaveRecordGroup = "6";

        /// <summary>
        /// Печать.
        /// </summary>
        public const string Print = "7";

        /// <summary>
        /// Запись параметров в ini-файл, расположенный на сервере.
        /// </summary>
        public const string UpdateIniFile = "8";

        /// <summary>
        /// IRBIS_IMPORT_ISO.
        /// </summary>
        public const string ImportIso = "9";

        /// <summary>
        /// Регистрация клиента на сервере.
        /// </summary>
        /// <remarks>IRBIS_REG</remarks>
        public const string RegisterClient = "A";

        /// <summary>
        /// Разрегистрация клиента.
        /// </summary>
        /// <remarks>IRBIS_UNREG</remarks>
        public const string UnregisterClient = "B";

        /// <summary>
        /// Чтение записи, ее расформатирование.
        /// </summary>
        /// <remarks>IRBIS_READ</remarks>
        public const string ReadRecord = "C";

        /// <summary>
        /// Сохранение записи.
        /// </summary>
        /// <remarks>IRBIS_UPDATE</remarks>
        public const string UpdateRecord = "D";

        /// <summary>
        /// Разблокировка записи.
        /// </summary>
        /// <remarks>IRBIS_RUNLOCK</remarks>
        public const string UnlockRecord = "E";

        /// <summary>
        /// Актуализация записи.
        /// </summary>
        /// <remarks>IRBIS_RECIFUPDATE</remarks>
        public const string ActualizeRecord = "F";

        /// <summary>
        /// Форматирование записи или группы записей.
        /// </summary>
        /// <remarks>IRBIS_SVR_FORMAT</remarks>
        public const string FormatRecord = "G";

        /// <summary>
        /// Получение терминов и ссылок словаря, форматирование записей
        /// </summary>
        /// <remarks>IRBIS_TRM_READ</remarks>
        public const string ReadTerms = "H";

        /// <summary>
        /// Получение ссылок для термина (списка терминов).
        /// </summary>
        /// <remarks>IRBIS_POSTING</remarks>
        public const string ReadPostings = "I";

        /// <summary>
        /// Глобальная корректировка виртуальной записи.
        /// </summary>
        /// <remarks>IRBIS_GBL_RECORD</remarks>
        public const string CorrectVirtualRecord = "J";

        /// <summary>
        /// Поиск записей с опциональным форматированием
        /// (также последовательный поиск).
        /// </summary>
        /// <remarks>IRBIS_SEARCH</remarks>
        public const string Search = "K";

        /// <summary>
        /// Получение/сохранение текстового файла, расположенного
        /// на сервере (группы текстовых файлов).
        /// </summary>
        public const string ReadDocument = "L";

        /// <summary>
        /// IRBIS_BACKUP.
        /// </summary>
        public const string Backup = "M";

        /// <summary>
        /// Пустая операция. Периодическое подтверждение
        /// соединения с сервером.
        /// </summary>
        /// <remarks>IRBIS_NOOP</remarks>
        public const string Nop = "N";

        /// <summary>
        /// Получение максимального MFN для базы данных.
        /// </summary>
        /// <remarks>IRBIS_MAXMFN</remarks>
        public const string GetMaxMfn = "O";

        /// <summary>
        /// Получение терминов и ссылок словаря в обратном порядке.
        /// </summary>
        public const string ReadTermsReverse = "P";

        /// <summary>
        /// Разблокирование записей.
        /// </summary>
        public const string UnlockRecords = "Q";

        /// <summary>
        /// Полнотекстовый поиск.
        /// </summary>
        /// <remarks>IRBIS_FULLTEXT_SEARCH</remarks>
        public const string FullTextSearch = "R";

        /// <summary>
        /// Опустошение базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_EMPTY</remarks>
        public const string EmptyDatabase = "S";

        /// <summary>
        /// Создание базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_NEW</remarks>
        public const string CreateDatabase = "T";

        /// <summary>
        /// Разблокирование базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_UNLOCK</remarks>
        public const string UnlockDatabase = "U";

        /// <summary>
        /// Чтение ссылок для заданного MFN.
        /// </summary>
        /// <remarks>IRBIS_MFN_POSTINGS</remarks>
        public const string GetRecordPostings = "V";

        /// <summary>
        /// Удаление базы данных.
        /// </summary>
        /// <remarks>IRBIS_DB_DELETE</remarks>
        public const string DeleteDatabase = "W";

        /// <summary>
        /// Реорганизация мастер-файла.
        /// </summary>
        /// <remarks>IRBIS_RELOAD_MASTER</remarks>
        public const string ReloadMasterFile = "X";

        /// <summary>
        /// Реорганизация словаря.
        /// </summary>
        /// <remarks>IRBIS_RELOAD_DICT</remarks>
        public const string ReloadDictionary = "Y";

        /// <summary>
        /// Создание поискового словаря заново.
        /// </summary>
        /// <remarks>IRBIS_CREATE_DICT</remarks>
        public const string CreateDictionary = "Z";

        /// <summary>
        /// Получение статистики работы сервера.
        /// </summary>
        /// <remarks>IRBIS_STAT</remarks>
        public const string GetServerStat = "+1";

        /// <summary>
        /// Список запущенных потоков.
        /// </summary>
        public const string GetThreadList = "+2";

        /// <summary>
        /// Получение списка запущенных процессов.
        /// </summary>
        public const string GetProcessList = "+3";

        /// <summary>
        /// Сбросить запущенную задачу.
        /// </summary>
        public const string StopProcess = "+4";

        /// <summary>
        /// Сбросить запущенный поток.
        /// </summary>
        public const string StopThread = "+5";

        /// <summary>
        /// Сбросить зарегистрированного клиента.
        /// </summary>
        public const string StopClient = "+6";

        /// <summary>
        /// Сохранение списка пользователей.
        /// </summary>
        public const string SetUserList = "+7";

        /// <summary>
        /// Перезапуск сервера.
        /// </summary>
        public const string RestartServer = "+8";

        /// <summary>
        /// Получение списка пользователей.
        /// </summary>
        public const string GetUserList = "+9";

        /// <summary>
        /// Получение списка файлов на сервере.
        /// </summary>
        public const string ListFiles = "!";

        #endregion

        #region Прочие константы

        /// <summary>
        /// Начало диапазона валидных кодов подполей.
        /// </summary>
        public const char FirstCode = '!';

        /// <summary>
        /// Конец диапазона валидных кодов подполей (включая!).
        /// </summary>
        public const char LastCode = '~';

        /// <summary>
        /// Имя файла, содержащего список баз данных для администратора.
        /// </summary>
        public const string AdministratorDatabaseList = "dbnam1.mnu";

        /// <summary>
        /// Имя файла, содержащего список баз данных для каталогизатора.
        /// </summary>
        public const string CatalogerDatabaseList = "dbnam2.mnu";

        /// <summary>
        /// Максимальная длина (размер полки) - ограничение формата.
        /// </summary>
        public const int MaxRecord = 32000;

        /// <summary>
        /// Максимальное количество постингов в пакете.
        /// </summary>
        public const int MaxPostings = 32758;

        /// <summary>
        /// Имя файла, содержащего список баз данных для читателя.
        /// </summary>
        public const string ReaderDatabaseList = "dbnam3.mnu";

        /// <summary>
        /// Стандартный разделитель строк ИРБИС.
        /// </summary>
        public const string IrbisDelimiter = "\x001F\x001E";

        /// <summary>
        /// Короткий разделитель строк в ИРБИС.
        /// </summary>
        public static readonly char[] ShortIrbisDelimiterBytes = { '\x1F' };

        /// <summary>
        /// Стандартный разделитель строк ИРБИС.
        /// </summary>
        public static readonly byte[] IrbisDelimiterBytes = { 0x1F, 0x1E };

        /// <summary>
        /// Стандартный разделитель строк в DOS/Windows.
        /// </summary>
        public const string WindowsDelimiter = "\r\n";

        /// <summary>
        /// Стандартный разделитель строк в DOS/Windows.
        /// </summary>
        public static readonly byte[] WindowsDelimiterBytes = { 13, 10 };

        /// <summary>
        /// Точка с запятой.
        /// </summary>
        public static readonly char[] Semicolon = { ';' };

        /// <summary>
        /// Знак равенства.
        /// </summary>
        public static readonly char[] EqualSign = { '=' };

        /// <summary>
        /// Решетка.
        /// </summary>
        public static readonly char[] NumberSign = { '#' };

        /// <summary>
        /// Преамбула для двоичных файлов.
        /// IRBIS_BINARY_DATA
        /// </summary>
        public static readonly byte[] Preamble =
        {
            73, 82, 66, 73, 83, 95, 66, 73, 78, 65, 82, 89, 95, 68,
            65, 84, 65
        };

        /// <summary>
        /// Допустимые коды для чтения записей с сервера.
        /// </summary>
        public static readonly int[] GoodCodesForReadRecord = { -201, -600, -602, -603 };

        /// <summary>
        /// Допустимые коды для чтения терминов с сервера.
        /// </summary>
        public static readonly int[] GoodCodesForReadTerms = { -202, -203, -204 };

        #endregion

    } // class Constants

    /// <summary>
    /// Коды возврата, используемые при общении с сервером ИРБИС64.
    /// </summary>
    public enum ReturnCode
    {
        /// <summary>
        /// Успешное завершение, нет ошибки.
        /// </summary>
        NoError = 0,

        /// <summary>
        /// Успешное завершение, нет ошибки.
        /// </summary>
        Zero = 0,

        /// <summary>
        /// Прервано пользователем или общая ошибка.
        /// </summary>
        UserError = -1,

        /// <summary>
        /// Не завершена обработка предыдущего запроса.
        /// </summary>
        Busy = -2,

        /// <summary>
        /// Неизвестная ошибка.
        /// </summary>
        Unknown = -3,

        /// <summary>
        /// Выходной буфер мал.
        /// </summary>
        BadBufferSize = -4,

        /// <summary>
        /// Ошибка выделения памяти.
        /// </summary>
        MemoryAllocationError = -100,

        /// <summary>
        /// Размер полки меньше размера записи.
        /// </summary>
        ShelfSizeError = -101,

        /// <summary>
        /// Номер полки больше числа полок.
        /// </summary>
        ShelfNumberError = -102,

        /// <summary>
        /// Заданный MFN вне пределов БД.
        /// </summary>
        WrongMfn = -140,

        /// <summary>
        /// Ошибка чтения записи, она требует физического удаления.
        /// </summary>
        ReadRecordError = -141,

        /// <summary>
        /// Заданного поля нет.
        /// isisfldrep irbisfldadd = пустое поле.
        /// </summary>
        FieldNotExist = -200,

        /// <summary>
        /// Нет предыдущей версии записи.
        /// </summary>
        PreviousVersionNotExist = -201,

        /// <summary>
        /// Нет запрошенного значения в поисковом индексе.
        /// </summary>
        TermNotExist = -202,

        /// <summary>
        /// Была считана последняя запись в поисковом индексе.
        /// </summary>
        LastTermInList = -203,

        /// <summary>
        /// Возвращена первая подходящая запись в поисковом
        /// индексе вместо запрошенного значения.
        /// </summary>
        FirstTermInList = -204,

        /// <summary>
        /// Монопольная блокировка БД.
        /// </summary>
        DatabaseLocked = -300,

        /// <summary>
        /// Блокировка ввода - не используется в IRBIS64.
        /// </summary>
        DatabaseLockedForEdit = -301,

        /// <summary>
        /// Ошибка при открытии файла MST или XRF.
        /// </summary>
        OpenMstError = -400,

        /// <summary>
        /// Ошибка при открытии файлов поискового индекса.
        /// </summary>
        OpenIndexError = -401,

        /// <summary>
        /// Ошибка при записи в файл.
        /// </summary>
        WriteError = -402,

        /// <summary>
        /// Ошибка при актуализации.
        /// </summary>
        ActualizationError = -403,

        /// <summary>
        /// Запись логически удалена.
        /// </summary>
        LogicallyDeleted1 = -600,

        /// <summary>
        /// Запись физически удалена.
        /// </summary>
        PhysicallyDeleted1 = -601,

        /// <summary>
        /// Запись заблокирована на ввод.
        /// </summary>
        RecordLocked = -602,

        /// <summary>
        /// Запись логически удалена.
        /// </summary>
        RecordDeleted = -603,

        /// <summary>
        /// Запись физически удалена.
        /// </summary>
        PhysicallyDeleted = -605,

        /// <summary>
        /// Ошибка в Autoin.gbl.
        /// </summary>
        AutoinError = -607,

        /// <summary>
        /// При записи обнаружено несоответствие версий.
        /// </summary>
        VersionError = -608,

        /// <summary>
        /// Ошибка в GUID. Появилась в IRBIS64+.
        /// </summary>
        GuidError = -609,

        /// <summary>
        /// Ошибка при создании страховой копии.
        /// </summary>
        BackupCreationError = -700,

        /// <summary>
        /// Ошибка при восстановлении из страховой копии.
        /// </summary>
        BackupRestoreError = -701,

        /// <summary>
        /// Ошибка при сортировке.
        /// </summary>
        ErrorWhileSorting = -702,

        /// <summary>
        /// Ошибка при отборе терминов словаря.
        /// </summary>
        TermCreationError = -703,

        /// <summary>
        /// Ошибка при разгрузке словаря.
        /// </summary>
        LinkCreationError = -704,

        /// <summary>
        /// Ошибка при загрузке словаря.
        /// </summary>
        LinkLoadError = -705,

        /// <summary>
        /// Количество параметров GBL не число.
        /// </summary>
        GblParameterError = -800,

        /// <summary>
        /// Повторение задано не числом.
        /// </summary>
        GblOccurrenceError = -801,

        /// <summary>
        /// Метка задана не числом.
        /// </summary>
        GblTagError = -802,

        /// <summary>
        /// Ошибка в клиентском файле формата.
        /// </summary>
        ClientFormatError = -999,

        /// <summary>
        /// Ошибка выполнения на сервере.
        /// </summary>
        ServerExecutionError = -1111,

        /// <summary>
        /// Несоответствие полученной и реальной длины.
        /// </summary>
        AnswerLengthError = -1112,

        /// <summary>
        /// Неверный протокол.
        /// </summary>
        WrongProtocol = -2222,

        /// <summary>
        /// Незарегистрированный клиент.
        /// </summary>
        ClientNotInList = -3333,

        /// <summary>
        /// Клиент не выполнил регистрацию.
        /// </summary>
        ClientNotInUse = -3334,

        /// <summary>
        /// Неправльный идентификатор клиента.
        /// </summary>
        WrongClientIdentifier = -3335,

        /// <summary>
        /// Зарегистрировано максимально допустимое
        /// количество клиентов.
        /// </summary>
        ClientListOverload = -3336,

        /// <summary>
        /// Клиент уже зарегистрирован.
        /// </summary>
        ClientAlreadyExist = -3337,

        /// <summary>
        /// Нет доступа к командам АРМ.
        /// </summary>
        ClientNotAllowed = -3338,

        /// <summary>
        /// Неверный пароль.
        /// </summary>
        WrongPassword = -4444,

        /// <summary>
        /// Файл не существует.
        /// </summary>
        FileNotExist = -5555,

        /// <summary>
        /// Сервер перегружен: достигнуто максимальное число
        /// потоков обработки.
        /// </summary>
        ServerOverload = -6666,

        /// <summary>
        /// Не удалось запустить или прервать поток или процесс.
        /// </summary>
        ProcessError = -7777,

        /// <summary>
        /// Обрушение при выполнении глобальной корректировки.
        /// </summary>
        GlobalError = -8888,

        /// <summary>
        /// Операция была отменена.
        /// </summary>
        Cancelled = -100_000,

        /// <summary>
        /// Ошибка создания сокета.
        /// </summary>
        SocketCreationError = -100_001,

        /// <summary>
        /// Сбой сети.
        /// </summary>
        NetworkFailure = -100_002,

        /// <summary>
        /// Не подключен к серверу.
        /// </summary>
        NotConnected = -100_003,

    } // enum ReturnCode

    /// <summary>
    /// Путь к файлам на сервере ИРБИС64.
    /// </summary>
    public enum IrbisPath
    {
        /// <summary>
        /// Общесистемный путь.
        /// </summary>
        System = 0,

        /// <summary>
        /// Путь размещения сведений о базах данных сервера ИРБИС64.
        /// </summary>
        Data = 1,

        /// <summary>
        /// Путь на мастер-файл базы данных.
        /// </summary>
        MasterFile = 2,

        /// <summary>
        /// Путь на словарь базы данных (как правило, совпадает с
        /// путем на мастер-файл).
        /// </summary>
        InvertedFile = 3,

        /// <summary>
        /// Путь на параметрию базы данных.
        /// </summary>
        ParameterFile = 10,

        /// <summary>
        /// Путь к полным текстам (как правило, полные тексты
        /// вынесены в отдельную папку или даже на отдельный диск).
        /// </summary>
        FullText = 11,

        /// <summary>
        /// Внутренний ресурс.
        /// </summary>
        InternalResource = 12

    } // enum IrbisPath

    /// <summary>
    /// Базовое исключение для всех ситуаций,
    /// специфичных для ИРБИС64.
    /// </summary>
    [DebuggerDisplay("Code={" + nameof(ErrorCode) + "}, Message={" + nameof(Message) + "}")]
    public class IrbisException
        : ApplicationException
    {
        #region Properties

        /// <summary>
        /// Код ошибки, возвращенный сервером.
        /// Значения, меньшие нуля, свидетельствуют об ошибке.
        /// Ноль и больше -- нормальное завершение операции.
        /// </summary>
        public int ErrorCode { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public IrbisException()
        {
        } // constructor

        /// <summary>
        /// Конструктор с кодом возврата.
        /// </summary>
        public IrbisException
            (
                int returnCode
            )
            : base(GetErrorDescription(returnCode))
        {
            ErrorCode = returnCode;

        } // constructor

        /// <summary>
        /// Конструктор с сообщением об ошибке.
        /// </summary>
        public IrbisException
            (
                string message
            )
            : base(message)
        {
        } // constructor

        /// <summary>
        /// Конструктор с сообщением об ошибке и вложенным исключением.
        /// </summary>
        public IrbisException
            (
                string message,
                Exception innerException
            )
            : base(message, innerException)
        {
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Get text description of the error.
        /// </summary>
        public static string GetErrorDescription
            (
                IrbisException exception
            )
        {
            return string.IsNullOrEmpty(exception.Message)
                ? GetErrorDescription(exception.ErrorCode)
                : exception.Message;
        }

        /// <summary>
        /// Get text description ot the error.
        /// </summary>
        public static string GetErrorDescription
            (
                int code
            )
        {
            var result = "Unknown error";

            if (code > 0)
            {
                result = "No error";
            }
            else
            {
                switch (code)
                {
                    case 0:
                        result = "Normal return";
                        break;

                    case -100:
                        result = "Given MFN is outside the database range";
                        break;

                    case -101:
                        result = "Bad shelf size";
                        break;

                    case -102:
                        result = "Bad shelf number";
                        break;

                    case -140:
                        result = "MFN outside the database range";
                        break;

                    case -141:
                        result = "Read error";
                        break;

                    case -200:
                        result = "The field is absent";
                        break;

                    case -201:
                        result = "No previous version of the record";
                        break;

                    case -202:
                        result = "Term doesn't exist";
                        break;

                    case -203:
                        result = "Last term in the list";
                        break;

                    case -204:
                        result = "First term in the list";
                        break;

                    case -300:
                        result = "Database is exclusively locked";
                        break;

                    case -301:
                        result = "Database is exclusively locked";
                        break;

                    case -400:
                        result = "Master file error";
                        break;

                    case -401:
                        result = "Index file error";
                        break;

                    case -402:
                        result = "Write error";
                        break;

                    case -403:
                        result = "Error during record actualization";
                        break;

                    case -600:
                        result = "The record is logically deleted";
                        break;

                    case -601:
                        result = "The record is physically deleted";
                        break;

                    case -602:
                        result = "The record is blocked";
                        break;

                    case -603:
                        result = "The record is logically deleted";
                        break;

                    case -605:
                        result = "The record is physically deleted";
                        break;

                    case -607:
                        result = "Error during autoin.gbl processing";
                        break;

                    case -608:
                        result = "Record version mismatch";
                        break;

                    case -700:
                        result = "Backup creation error";
                        break;

                    case -701:
                        result = "Backup restore error";
                        break;

                    case -702:
                        result = "Error during sorting";
                        break;

                    case -703:
                        result = "Bad term";
                        break;

                    case -704:
                        result = "Dictionary creation error";
                        break;

                    case -705:
                        result = "Dictionary loading error";
                        break;

                    case -800:
                        result = "Global correction parameter error";
                        break;

                    case -801:
                        result = "Global correction: bad rep tag";
                        break;

                    case -802:
                        result = "Global correction: bad tag";
                        break;

                    case -1111:
                        result = "Server execution error";
                        break;

                    case -2222:
                        result = "Protocol error";
                        break;

                    case -3333:
                        result = "Client not registered";
                        break;

                    case -3334:
                        result = "Client doesn't in use";
                        break;

                    case -3335:
                        result = "Bad client identifier";
                        break;

                    case -3336:
                        result = "Workstation access denied";
                        break;

                    case -3337:
                        result = "The client is already registered";
                        break;

                    case -3338:
                        result = "Client not allowed";
                        break;

                    case -4444:
                        result = "Bad password";
                        break;

                    case -5555:
                        result = "File not exist";
                        break;

                    case -6666:
                        result = "Server is overloaded";
                        break;

                    case -7777:
                        result = "Administrator thread error";
                        break;

                    case -8888:
                        result = "General error";
                        break;
                }
            }

            return result;
       }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => string.Format
            (
                "ErrorCode: {2}{1}Description: {3}{1}{0}",
                base.ToString(),
                Environment.NewLine,
                ErrorCode,
                Message
            );

        #endregion

    } // class IrbisException

    /// <summary>
    /// Подполе библиографической записи.
    /// </summary>
    public sealed class SubField
    {
        #region Constants

        /// <summary>
        /// Нет кода подполя, т. е. код пока не задан.
        /// Также используется для обозначения, что подполе
        /// используется для хранения значения поля
        /// до первого разделителя.
        /// </summary>
        public const char NoCode = '\0';

        /// <summary>
        /// Разделитель подполей.
        /// </summary>
        public const char Delimiter = '^';

        #endregion

        #region Properties

        /// <summary>
        /// Код подполя.
        /// </summary>
        public char Code { get; set; } = NoCode;

        /// <summary>
        /// Значение подполя.
        /// </summary>
        public string? Value
        {
            get => _value;
            set => SetValue(value);
        }

        /// <summary>
        /// Подполе хранит значение поля до первого разделителя.
        /// </summary>
        public bool RepresentsValue => Code == NoCode;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public SubField()
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        public SubField
            (
                char code,
                ReadOnlyMemory<char> value = default
            )
        {
            Utility.VerifySubFieldCode(code);
            Code = code;
            Utility.VerifySubFieldValue(value.Span);
            Value = value.ToString();

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        public SubField
            (
                char code,
                string? value
            )
        {
            Utility.VerifySubFieldCode(code);
            Code = code;
            Utility.VerifySubFieldValue(value.AsSpan());
            Value = value;

        } // constructor

        #endregion

        #region Private members

        private string? _value;

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование подполя.
        /// </summary>
        public SubField Clone() => (SubField) MemberwiseClone();

        /// <summary>
        /// Сравнение двух подполей.
        /// </summary>
        public static int Compare
            (
                SubField subField1,
                SubField subField2
            )
        {
            // сравниваем коды подполей с точностью до регистра символов
            var result = char.ToUpperInvariant(subField1.Code)
                .CompareTo(char.ToUpperInvariant(subField2.Code));
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal(subField1.Value, subField2.Value);

            return result;

        } // method Compare

        /// <summary>
        /// Декодирование строки.
        /// </summary>
        public void Decode
            (
                ReadOnlySpan<char> text
            )
        {
            if (!text.IsEmpty)
            {
                var code = char.ToLowerInvariant(text[0]);
                Utility.VerifySubFieldCode(code);
                Code = code;
                var value = text.Slice(1);
                Utility.VerifySubFieldValue(value);
                Value = value.EmptyToNull();
            }
        } // method Decode

        /// <summary>
        /// Установка нового значения подполя.
        /// </summary>
        public void SetValue
            (
                ReadOnlySpan<char> value
            )
        {
            Utility.VerifySubFieldValue(value);
            _value = value.ToString();

        } // method SetValue

        /// <summary>
        /// Установка нового значения подполя.
        /// </summary>
        public void SetValue
            (
                string? value
            )
        {
            Utility.VerifySubFieldValue(value.AsSpan());
            _value = value;

        } // method SetValue

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Code == NoCode
                ? Value ?? string.Empty
                : "^" + char.ToLowerInvariant(Code) + Value;

        #endregion

    } // class SubField

    /// <summary>
    /// Поле библиографической записи.
    /// </summary>
    public class Field
        : IEnumerable<SubField>
    {
        #region Constants

        /// <summary>
        /// Специальный код, зарезервированный для
        /// значения поля до первого разделителя.
        /// </summary>
        private const char ValueCode = '\0';

        /// <summary>
        /// Нет тега, т. е. тег ещё не присвоен.
        /// </summary>
        public const int NoTag = 0;

        /// <summary>
        /// Разделитель подполей.
        /// </summary>
        public const char Delimiter = '^';

        /// <summary>
        /// Количество индикаторов поля.
        /// </summary>
        public const int IndicatorCount = 2;

        #endregion

        #region Properties

        /// <summary>
        /// Метка поля.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Значение поля до первого разделителя.
        /// </summary>
        /// <remarks>
        /// Значение имитируется с помощью первого подполя,
        /// код которого должен быть равен '\0'.
        /// </remarks>
        public string? Value
        {
            get => GetValueSubField()?.Value ?? default;
            set
            {
                Clear();
                if (value.SafeContains(Delimiter))
                {
                    DecodeBody(value!);
                }
                else
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        CreateValueSubField().Value = value;
                    }
                }
            } // set
        } // property Value

        /// <summary>
        /// Список подполей.
        /// </summary>
        public List<SubField> Subfields { get; } = new ();

        /// <summary>
        /// Номер повторения поля.
        /// </summary>
        /// <remarks>
        /// Формируется автоматически.
        /// </remarks>
        public int Repeat { get; internal set; }

        /// <summary>
        /// Запись, которой принадлежит поле.
        /// </summary>
        public Record? Record { get; internal set; }

        /// <summary>
        /// Пустое ли поле?
        /// </summary>
        public bool IsEmpty => Subfields.Count == 0;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public Field()
        {
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="value">Значение поля до первого разделителя
        /// (опционально).</param>
        public Field
            (
                int tag,
                string? value
            )
        {
            Tag = tag;
            Value = value;
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="subfield1"></param>
        public Field
            (
                int tag,
                SubField subfield1
            )
        {
            Tag = tag;
            Subfields.Add(subfield1);
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="subfield1"></param>
        /// <param name="subfield2"></param>
        public Field
            (
                int tag,
                SubField subfield1,
                SubField subfield2
            )
        {
            Tag = tag;
            Subfields.Add(subfield1);
            Subfields.Add(subfield2);

        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="subfield1"></param>
        /// <param name="subfield2"></param>
        /// <param name="subfield3"></param>
        public Field
            (
                int tag,
                SubField subfield1,
                SubField subfield2,
                SubField subfield3
            )
        {
            Tag = tag;
            Subfields.Add(subfield1);
            Subfields.Add(subfield2);
            Subfields.Add(subfield3);
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="subfields">Подполя.</param>
        public Field
            (
                int tag,
                params SubField[] subfields
            )
        {
            Tag = tag;
            Subfields.AddRange(subfields);
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя (опционально).</param>
        public Field
            (
                int tag,
                char code1,
                ReadOnlyMemory<char> value1 = default
            )
        {
            Tag = tag;
            Subfields.Add(new SubField(code1, value1));
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя.</param>
        /// <param name="code2">Код подполя.</param>
        /// <param name="value2">Значение подполя (опционально).</param>
        public Field
            (
                int tag,
                char code1,
                string? value1,
                char code2,
                string? value2 = default
            )
        {
            Tag = tag;
            Subfields.Add(new SubField(code1, value1));
            Subfields.Add(new SubField(code2, value2));
        } // constructor

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя.</param>
        /// <param name="code2">Код подполя.</param>
        /// <param name="value2">Значение подполя.</param>
        /// <param name="code3">Код подполя.</param>
        /// <param name="value3">Значение подполя (опционально).</param>
        public Field
            (
                int tag,
                char code1,
                string? value1,
                char code2,
                string? value2,
                char code3,
                string? value3 = default
            )
        {
            Tag = tag;
            Subfields.Add(new SubField(code1, value1));
            Subfields.Add(new SubField(code2, value2));
            Subfields.Add(new SubField(code3, value3));

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Поле с подполями.
        /// </summary>
        public static Field WithSubFields
            (
                int tag,
                params string[] subfields
            )
        {
            var result = new Field(tag);
            for (var i = 0; i < subfields.Length; i += 2)
            {
                var code = subfields[i][0];
                var value = subfields[i + 1];
                result.Subfields.Add(new SubField(code, value));
            }

            return result;
        }

        /// <summary>
        /// Добавление подполя в конец списка подполей.
        /// </summary>
        /// <param name="subfield">Добавляемое подполе.</param>
        /// <returns>this</returns>
        public Field Add
            (
                SubField subfield
            )
        {
            Subfields.Add(subfield);
            return this;
        } // method Add

        /// <summary>
        /// Добавление подполя в конец списка подполей.
        /// </summary>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Field Add
            (
                char code,
                ReadOnlyMemory<char> value
            )
        {
            Subfields.Add(new SubField(code, value));
            return this;
        } // method Add

        /// <summary>
        /// Добавление подполя в конец списка подполей.
        /// </summary>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Field Add
            (
                char code,
                string? value = default
            )
        {
            Subfields.Add(new SubField(code, value));
            return this;
        } // method Add

        /// <summary>
        /// Assign the field from another.
        /// </summary>
        public Field AssignFrom
            (
                Field source
            )
        {
            Value = source.Value;
            Subfields.Clear();
            foreach (var subField in source.Subfields)
            {
                Subfields.Add(subField.Clone());
            }

            return this;
        } // method AssignFrom

        /// <summary>
        /// Compares the specified fields.
        /// </summary>
        public static int Compare
            (
                Field field1,
                Field field2
            )
        {
            var result = field1.Tag - field2.Tag;
            if (result != 0)
            {
                return result;
            }

            result = string.CompareOrdinal
                (
                    field1.Value,
                    field2.Value
                );
            if (result != 0)
            {
                return result;
            }

            result = field1.Subfields.Count - field2.Subfields.Count;
            if (result != 0)
            {
                return result;
            }

            for (int i = 0; i < field1.Subfields.Count; i++)
            {
                var subField1 = field1.Subfields[i];
                var subField2 = field2.Subfields[i];

                result = SubField.Compare
                    (
                        subField1,
                        subField2
                    );
                if (result != 0)
                {
                    return result;
                }
            }

            return result;

        } // method Compare

        /// <summary>
        /// Если нет подполя, выделенного для хранения
        /// значения поля до первого разделителя,
        /// создаем его (оно должно быть первым в списке подполей).
        /// </summary>
        public SubField CreateValueSubField()
        {
            SubField result;

            if (Subfields.Count == 0)
            {
                result = new SubField { Code = ValueCode };
                Subfields.Add(result);
                return result;

            }

            result = Subfields[0];
            if (result.Code != ValueCode)
            {
                result = new SubField { Code = ValueCode };
                Subfields.Insert(0, result);
            }

            return result;
        } // method CreateValueSubField

        /// <summary>
        /// Получаем подполе, выделенное для хранения
        /// значения поля до первого разделителя.
        /// </summary>
        public SubField? GetValueSubField()
        {
            if (Subfields.Count == 0)
            {
                return null;
            }

            var result = Subfields[0];
            if (result.Code == ValueCode)
            {
                return result;
            }

            return null;
        } // method GetValueSubField

        /// <summary>
        /// Добавление подполя в конец списка подполей.
        /// </summary>
        public Field Add
            (
                char code,
                object? value
            )
        {
            if (code == ValueCode)
            {
                Value = value?.ToString();
                return this;
            }

            var text = value?.ToString();
            var subfield = new SubField { Code = code, Value = text };
            Subfields.Add(subfield);

            return this;

        } // method Add

        /// <summary>
        /// Добавление поля, если переданное значение не равно 0.
        /// </summary>
        public Field AddNonEmpty
            (
                char code,
                int value
            )
        {
            if (value is not 0)
            {
                Add(code, value.ToInvariantString());
            }

            return this;

        } // method AddNonEmpty

        /// <summary>
        /// Добавление поля, если переданное значение не равно 0.
        /// </summary>
        public Field AddNonEmpty
            (
                char code,
                long value
            )
        {
            if (value is not 0)
            {
                Add(code, value.ToInvariantString());
            }

            return this;

        } // method AddNonEmpty

        /// <summary>
        /// Добавление подполя в конец списка подполей
        /// при условии, что значение поля не пустое.
        /// </summary>
        public Field AddNonEmpty
            (
                char code,
                object? value
            )
        {
            if (value is not null)
            {
                if (code == ValueCode)
                {
                    Value = value.ToString();
                    return this;
                }

                var text = value.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    var subfield = new SubField { Code = code, Value = text };
                    Subfields.Add(subfield);
                }
            }

            return this;
        } // method AddNonEmpty

        /// <summary>
        /// Очистка подполей.
        /// </summary>
        public Field Clear()
        {
            Subfields.Clear();

            return this;
        } // method Clear

        /// <summary>
        /// Клонирование поля.
        /// </summary>
        public Field Clone()
        {
            var result = (Field) MemberwiseClone();

            for (var i = 0; i < Subfields.Count; i++)
            {
                Subfields[i] = Subfields[i].Clone();
            }

            return result;
        } // method Clone

        /// <summary>
        /// Декодирование строки.
        /// </summary>
        public void Decode
            (
                string line
            )
        {
            var index = line.IndexOf('#');
            Tag = line.Substring(0, index).SafeToInt32();
            line = line.Substring(index + 1);
            DecodeBody(line);

        } // method Decode

        /// <summary>
        /// Декодирование тела поля.
        /// </summary>
        public void DecodeBody
            (
                string line
            )
        {
            var index = line.IndexOf('^');
            if (index < 0)
            {
                Value = line;

                return;
            }

            if (index != 0)
            {
                Value = line.Substring(0, index);
            }

            line = line.Substring(index + 1);

            while (true)
            {
                index = line.IndexOf('^');
                if (index < 0)
                {
                    Add(line[0], line.Substring(1));
                    return;
                }

                Add(line[0], line.Substring(1, index - 1));
                line = line.Substring(index + 1);
            }
        } // method DecodeBody

        /// <summary>
        /// Получение первого подполя с указанным кодом.
        /// </summary>
        public SubField? GetFirstSubField
            (
                char code
            )
        {
            if (code == ValueCode)
            {
                var firstSubfield = Subfields.FirstOrDefault();
                if (firstSubfield?.Code == ValueCode)
                {
                    return firstSubfield;
                }

                return null;
            }

            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    return subfield;
                }
            }

            return null;
        } // method GetFirstSubField

        /// <summary>
        /// Перечисление подполей с указанным кодом.
        /// </summary>
        public IEnumerable<SubField> EnumerateSubFields
            (
                char code
            )
        {
            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    yield return subfield;
                }
            }
        } // method EnumerateSubFields

        /// <summary>
        /// Получение всех подполей с указанным кодом.
        /// </summary>
        public SubField[] GetSubFields
            (
                char code
            )
        {
            var result = new List<SubField>();

            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    result.Add(subfield);
                }
            }

            return result.ToArray();
        } // method GetSubFields

        /// <summary>
        /// Получение первого подполя с указанным кодом
        /// либо создание нового подполя, если таковое отсуствует.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <returns>Найденное или созданное подполе.</returns>
        public SubField GetOrAddSubField
            (
                char code
            )
        {
            if (code == '\0')
            {

            }

            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    return subfield;
                }
            }

            var result = new SubField { Code = code };
            Subfields.Add(result);

            return result;
        } // method GetOrAddSubField

        /// <summary>
        /// Указанное повторение подполя с данным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="occurrence">Номер повторения.
        /// Нумерация начинается с нуля.
        /// Отрицательные индексы отсчитываются с конца массива.</param>
        /// <returns>Найденное подполе или <c>null</c>.</returns>
        public SubField? GetSubField
            (
                char code,
                int occurrence = 0
            )
        {
            if (code == ValueCode)
            {
                if (occurrence != 0)
                {
                    return null;
                }

                return GetValueSubField();
            }

            if (occurrence < 0)
            {
                // отрицательные индексы отсчитываются от конца
                occurrence = Subfields.Count(sf => sf.Code.SameChar(code)) + occurrence;
                if (occurrence < 0)
                {
                    return null;
                }
            }

            foreach (var subfield in Subfields)
            {
                if (subfield.Code.SameChar(code))
                {
                    if (occurrence == 0)
                    {
                        return subfield;
                    }

                    --occurrence;
                }
            }

            return null;
        } // method GetSubField

        /// <summary>
        /// Получение текста указанного подполя.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="occurrence">Номер повторения.
        /// Нумерация начинается с нуля.
        /// Отрицательные индексы отсчитываются с конца массива.</param>
        /// <returns>Текст найденного подполя или <c>null</c>.</returns>
        public string? GetSubFieldValue
            (
                char code,
                int occurrence = 0
            )
            => GetSubField(code, occurrence)?.Value ?? default;

        /// <summary>
        /// For * specification.
        /// </summary>
        public string? GetValueOrFirstSubField()
            => Subfields.FirstOrDefault()?.Value ?? default;

        /// <summary>
        /// Установка значения подполя.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <param name="value">Новое значение подполя.</param>
        /// <returns>this</returns>
        public Field SetSubFieldValue
            (
                char code,
                string? value
            )
        {
            if (code == ValueCode)
            {
                Value = value;
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    RemoveSubField(code);
                }
                else
                {
                    GetOrAddSubField(code).Value = value;
                }
            }

            return this;
        } // method SetSubFieldValue

        /// <summary>
        /// Удаление подполей с указанным кодом.
        /// </summary>
        /// <param name="code">Искомый код подполя.</param>
        /// <returns>this</returns>
        public Field RemoveSubField
            (
                char code
            )
        {
            SubField? subfield;

            while ((subfield = GetFirstSubField(code)) is not null)
            {
                Subfields.Remove(subfield);
            }

            return this;
        } // method RemoveSubField

        /// <summary>
        /// Текстовое представление только значимой части поля.
        /// Метка поля не выводится.
        /// </summary>
        public string ToText()
        {
            var length = Subfields.Sum
                (
                    sf => (sf.Value!.Length)
                          + (sf.Code == ValueCode ? 1 : 2)
                );
            var result = new StringBuilder (length);

            // if (!string.IsNullOrEmpty(Value))
            // {
            //     result.Append(Value);
            // }

            foreach (var subField in Subfields)
            {
                var subText = subField.ToString();
                result.Append(subText);
            }

            return result.ToString();
        } // method ToText

        #endregion

        #region IEnumerable<SubField> members

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<SubField> GetEnumerator() => Subfields.GetEnumerator();

        #endregion


        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var length = 4 + Subfields.Sum
                (
                    sf => (sf.Value!.Length)
                    + (sf.Code == ValueCode ? 1 : 2)
                );
            var result = new StringBuilder (length);
            result.Append(Tag.ToInvariantString())
                .Append('#');
            foreach (var subfield in Subfields)
            {
                result.Append(subfield);
            }

            return result.ToString();
        } // method ToString

        #endregion

    } // class Field

    /// <summary>
    /// Статус библиографической записи (флаги).
    /// </summary>
    [Flags]
    public enum RecordStatus
    {
        /// <summary>
        /// Нет статуса -- запись только что создана.
        /// </summary>
        None = 0,

        /// <summary>
        /// Запись логически удалена.
        /// </summary>
        LogicallyDeleted = 1,

        /// <summary>
        /// Запись физически удалена.
        /// </summary>
        PhysicallyDeleted = 2,

        /// <summary>
        /// Запись отсутствует.
        /// </summary>
        Absent = 4,

        /// <summary>
        /// Запись не актуализирована.
        /// </summary>
        NonActualized = 8,

        /// <summary>
        /// Первый экземпляр записи.
        /// </summary>
        NewRecord = 16,

        /// <summary>
        /// Последний экземпляр записи.
        /// </summary>
        Last = 32,

        /// <summary>
        /// Запись заблокирована.
        /// </summary>
        Locked = 64,

        /// <summary>
        /// Ошибка в Autoin.gbl.
        /// </summary>
        AutoinError = 128,

        /// <summary>
        /// Полный текст не актуализирован.
        /// </summary>
        FullTextNotActualized = 256

    } // enum RecordStatus

    /// <summary>
    /// Библиографическая запись. Состоит из произвольного количества полей.
    /// </summary>
    [DebuggerDisplay("[{" + nameof(Database) +
        "}] MFN={" + nameof(Mfn) + "} ({" + nameof(Version) + "})")]
    public sealed class Record
        : IEnumerable<Field>
    {
        #region Constants

        /// <summary>
        /// Запись удалена любым способом (логически или физически).
        /// </summary>
        private const RecordStatus IsDeleted = RecordStatus.LogicallyDeleted | RecordStatus.PhysicallyDeleted;

        #endregion

        #region Properties

        /// <summary>
        /// База данных, в которой хранится запись.
        /// Для вновь созданных записей -- <c>null</c>.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN (порядковый номер в базе данных) записи.
        /// Для вновь созданных записей равен <c>0</c>.
        /// Для хранящихся в базе записей нумерация начинается
        /// с <c>1</c>.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Версия записи. Для вновь созданных записей равна <c>0</c>.
        /// Для хранящихся в базе записей нумерация версий начинается
        /// с <c>1</c>.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Статус записи. Для вновь созданных записей <c>None</c>.
        /// </summary>
        public RecordStatus Status { get; set; }

        /// <summary>
        /// Признак -- запись помечена как логически удаленная.
        /// </summary>
        public bool Deleted => (Status & IsDeleted) != 0;

        /// <summary>
        /// Список полей.
        /// </summary>
        public List<Field> Fields { get; } = new ();

        /// <summary>
        /// Описание в произвольной форме (опциональное).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Признак того, что запись модифицирована.
        /// </summary>
        public bool Modified { get; internal set; }

        /// <summary>
        /// Индекс документа (поле 920).
        /// </summary>
        public string? Index { get; set; }

        /// <summary>
        /// Ключ для сортировки записей.
        /// </summary>
        public string? SortKey { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// Данное свойство используется, например,
        /// при построении отчета.
        /// </summary>
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Установка статуса записи.
        /// </summary>
        /// <param name="status">Новый статус записи.</param>
        /// <returns>this</returns>
        public Record Add
            (
                RecordStatus status
            )
        {
            Status = status;
            return this;
        }

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <returns>
        /// Свежедобавленное поле (для цепочечных вызовов).
        /// </returns>
        public Record Add
            (
                int tag,
                string? value = default
            )
        {
            var field = new Field(tag);
            if (!ReferenceEquals(value, null))
            {
                field.DecodeBody(value);
            }
            Fields.Add(field);

            return this;

        } // method Add

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <returns>
        /// this (для цепочечных вызовов).
        /// </returns>
        public Record Add
            (
                int tag,
                SubField subfield1
            )
        {
            var field = new Field(tag) { subfield1 };
            Fields.Add(field);

            return this;

        } // method Add

        /// <summary>
        /// Добавление полей в конец записи.
        /// </summary>
        /// <returns>
        /// this (для цепочечных вызовов).
        /// </returns>
        public Record Add
            (
                int tag,
                SubField subfield1,
                SubField subfield2
            )
        {
            var field = new Field(tag) { subfield1, subfield2 };
            Fields.Add(field);

            return this;

        } // method Add

        /// <summary>
        /// Добавление полей в конец записи.
        /// </summary>
        /// <returns>
        /// this (для цепочечных вызовов).
        /// </returns>
        public Record Add
            (
                int tag,
                SubField subfield1,
                SubField subfield2,
                SubField subfield3
            )
        {
            var field = new Field(tag) { subfield1, subfield2, subfield3 };
            Fields.Add(field);

            return this;

        } // method Add

        /// <summary>
        /// Добавление полей в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="subfields">Подполя.</param>
        /// <returns>this (для цепочечных вызовов).</returns>
        public Record Add
            (
                int tag,
                params SubField[] subfields
            )
        {
            var field = new Field(tag);
            field.Subfields.AddRange(subfields);
            Fields.Add(field);

            return this;

        } // method Add

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code">Код подполя.</param>
        /// <param name="value">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Record Add
            (
                int tag,
                char code,
                string? value = default
            )
        {
            var field = new Field(tag);
            field.Subfields.Add(new SubField(code, value));
            Fields.Add(field);

            return this;
        } // method Add

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя.</param>
        /// <param name="code2">Код подполя.</param>
        /// <param name="value2">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Record Add
            (
                int tag,
                char code1,
                string? value1,
                char code2,
                string? value2 = default
            )
        {
            var field = new Field(tag);
            field.Subfields.Add(new SubField(code1, value1));
            field.Subfields.Add(new SubField(code2, value2));
            Fields.Add(field);

            return this;
        } // method Add

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="code1">Код подполя.</param>
        /// <param name="value1">Значение подполя.</param>
        /// <param name="code2">Код подполя.</param>
        /// <param name="value2">Значение подполя.</param>
        /// <param name="code3">Код подполя.</param>
        /// <param name="value3">Значение подполя (опционально).</param>
        /// <returns>this</returns>
        public Record Add
            (
                int tag,
                char code1,
                string? value1,
                char code2,
                string? value2,
                char code3,
                string? value3 = default
            )
        {
            var field = new Field(tag);
            field.Subfields.Add(new SubField(code1, value1));
            field.Subfields.Add(new SubField(code2, value2));
            field.Subfields.Add(new SubField(code3, value3));
            Fields.Add(field);

            return this;

        } // method Add

        /// <summary>
        /// Добавление поля в конец записи.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <param name="subfields">Коды и значения подполей.</param>
        /// <returns>this (для цепочечных вызовов)</returns>
        public Record Add
            (
                int tag,
                string[] subfields
            )
        {
            var field = Field.WithSubFields(tag, subfields);
            Fields.Add(field);

            return this;

        } // method Add

        /// <summary>
        /// Добавление в запись непустого поля.
        /// </summary>
        public Record AddNonEmptyField
            (
                int tag,
                string? value
            )
        {
            if (!ReferenceEquals(value, null))
            {
                var field = new Field { Tag = tag };
                field.DecodeBody(value);
                Fields.Add(field);
            }

            return this;

        } // method AddNonEmptyField

        /// <summary>
        /// Очистка записи (удаление всех полей).
        /// </summary>
        /// <returns>
        /// Ту же самую, но очищенную запись.
        /// </returns>
        public Record Clear()
        {
            Fields.Clear();

            return this;

        } // method Clear

        /// <summary>
        /// Создание глубокой копии записи.
        /// </summary>
        public Record Clone()
        {
            var result = (Record) MemberwiseClone();

            for (var i = 0; i < result.Fields.Count; i++)
            {
                result.Fields[i] = result.Fields[i].Clone();
            }

            return result;

        } // method Clone

        /// <summary>
        /// Декодирование ответа сервера.
        /// </summary>
        public void Decode
            (
                Response response
            )
        {
            try
            {
                var line = response.ReadUtf();

                var first = line.Split('#');
                Mfn = int.Parse(first[0]);
                Status = first.Length == 1
                    ? RecordStatus.None
                    : (RecordStatus) first[1].SafeToInt32();

                line = response.ReadUtf();
                var second = line.Split('#');
                Version = second.Length == 1
                    ? 0
                    : int.Parse(second[1]);

                while (!response.EOT)
                {
                    line = response.ReadUtf();
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    }

                    var field = new Field();
                    field.Decode(line);
                    Fields.Add(field);
                }
            }
            catch (Exception exception)
            {
                throw new IrbisException
                    (
                        nameof(Record) + "::" + nameof(Decode),
                        exception
                    );
            }

        } // method Decode

        /// <summary>
        /// Декодирование ответа сервера.
        /// </summary>
        public void Decode
            (
                string[] lines
            )
        {
            try
            {
                var line = lines[0];

                var first = line.Split('#');
                Mfn = int.Parse(first[0]);
                Status = first.Length == 1
                    ? RecordStatus.None
                    : (RecordStatus) first[1].SafeToInt32();

                line = lines[1];
                var second = line.Split('#');
                Version = second.Length == 1
                    ? 0
                    : int.Parse(second[1]);

                for (var i = 2; i < lines.Length; i++)
                {
                    line = lines[i];
                    if (string.IsNullOrEmpty(line))
                    {
                        break;
                    }

                    var field = new Field();
                    field.Decode(line);
                    Fields.Add(field);
                }
            }
            catch (Exception exception)
            {
                throw new IrbisException
                    (
                        nameof(Record) + "::" + nameof(Decode),
                        exception
                    );
            }

        } // method Decode

        /// <summary>
        /// Кодирование записи.
        /// </summary>
        public string Encode
            (
                string? delimiter = Constants.IrbisDelimiter
            )
        {
            var result = new StringBuilder(512);

            result.Append(Mfn.ToInvariantString())
                .Append('#')
                .Append(((int) Status).ToInvariantString())
                .Append(delimiter)
                .Append("0#")
                .Append(Version.ToInvariantString())
                .Append(delimiter);

            foreach (var field in Fields)
            {
                result.Append(field).Append(delimiter);
            }

            return result.ToString();

        } // method Encode

        /// <summary>
        /// Получить текст поля до разделителей подполей
        /// первого повторения поля с указанной меткой.
        /// </summary>
        /// <param name="tag">Метка поля.</param>
        /// <returns>Значение поля или <c>null</c>.</returns>
        public string? FM ( int tag ) => GetField(tag)?.Value;

        /// <summary>
        /// Текст первого подполя с указанным тегом и кодом.
        /// </summary>
        public string? FM
            (
                int tag,
                char code
            )
        {
            var field = GetField(tag);

            if (!ReferenceEquals(field, null))
            {
                return code == '*'
                    ? field.GetValueOrFirstSubField()
                    : field.GetSubFieldValue(code);
            }

            return default;

        } // method FM

        /// <summary>
        /// Текст всех полей с указанным тегом.
        /// </summary>
        public string[] FMA
            (
                int tag
            )
        {
            var result = new List<string>();

            foreach (var field in Fields)
            {
                if (field.Tag == tag
                    && !ReferenceEquals(field.Value, null))
                {
                    result.Add(field.Value);
                }
            }

            return result.ToArray();

        } // method FMA

        /// <summary>
        /// Текст всех подполей с указанным тегом и кодом.
        /// </summary>
        public string[] FMA
            (
                int tag,
                char code
            )
        {
            var result = new List<string>();

            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    var value = code == '*'
                        ? field.GetValueOrFirstSubField()
                        : field.GetSubFieldValue(code);
                    if (!ReferenceEquals(value, null))
                    {
                        result.Add(value);
                    }
                }
            }

            return result.ToArray();

        } // method FMA

        /// <summary>
        /// Получение заданного повторения поля с указанной меткой.
        /// </summary>
        public Field? GetField
            (
                int tag,
                int occurrence = 0
            )
        {
            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    if (occurrence == 0)
                    {
                        return field;
                    }

                    --occurrence;
                }
            }

            return null;

        } // method GetField

        /// <summary>
        /// Перечисление полей с указанной меткой.
        /// </summary>
        /// <param name="tag">Искомая метка поля.</param>
        public IEnumerable<Field> EnumerateField
            (
                int tag
            )
        {
            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    yield return field;
                }
            }

        } // method EnumerateField

        /// <summary>
        /// Получение поля с указанной меткой
        /// либо создание нового поля, если таковое отсутствует.
        /// </summary>
        /// <param name="tag">Искомая метка поля.</param>
        /// <returns>Найденное либо созданное поле.</returns>
        public Field GetOrAddField
            (
                int tag
            )
        {
            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    return field;
                }
            }

            var result = new Field { Tag = tag };
            Fields.Add(result);

            return result;

        } // method GetOrAddField

        /// <summary>
        /// Проверка, есть ли в записи поле с указанной меткой.
        /// </summary>
        public bool HaveField
            (
                int tag
            )
        {
            foreach (var field in Fields)
            {
                if (field.Tag == tag)
                {
                    return true;
                }
            }

            return false;

        } // method HaveField

        /// <summary>
        /// Удаление из записи поля с указанной меткой.
        /// </summary>
        /// <param name="tag">Искомая метка.</param>
        /// <returns>this.</returns>
        public Record RemoveField
            (
                int tag
            )
        {
            Field? field;
            while ((field = GetField(tag)) is not null)
            {
                Fields.Remove(field);
            }

            return this;

        } // method RemoveField

        /// <summary>
        /// Формирует плоское текстовое представление записи.
        /// </summary>
        public string ToPlainText()
        {
            var result = new StringBuilder();

            foreach (var field in Fields)
            {
                result.AppendFormat("{0}#", field.Tag);
                foreach (var subField in field.Subfields)
                {
                    if (subField.Code != SubField.NoCode)
                    {
                        result.Append('^');
                        result.Append(subField.Code);
                    }
                    result.Append(subField.Value);
                }
                result.AppendLine();
            }

            return result.ToString();

        }

        #endregion

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<Field> GetEnumerator() => Fields.GetEnumerator();

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Encode("\n");

    } // class Record

    /// <summary>
    /// Ответ сервера ИРБИС64.
    /// </summary>
    public sealed class Response
    {
        #region Properties

        /// <summary>
        /// Код команды.
        /// </summary>
        public string? Command { get; private set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int ClientId { get; private set; }

        /// <summary>
        /// Порядковый номер запроса.
        /// </summary>
        public int QueryId { get; private set; }

        /// <summary>
        /// Код возврвата (не для всех запросов).
        /// </summary>
        public int ReturnCode { get; private set; }

        /// <summary>
        /// Размер запроса в байтах
        /// (не всегда присылвается сервером).
        /// </summary>
        public int AnswerSize { get; private set; }

        /// <summary>
        /// Версия сервера (присылается в ответ на запрос A,
        /// т. е. регистрацию клиента).
        /// </summary>
        public string? ServerVersion { get; private set; }

        /// <summary>
        /// Достигнут конец текста?
        /// </summary>
        public bool EOT { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Response()
        {
            _memory = new List<ArraySegment<byte>>();
        }

        #endregion

        #region Private members

        private readonly List<ArraySegment<byte>> _memory;
        private ArraySegment<byte> _currentChunk;
        private int _currentIndex, _currentOffset;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление сегмента данных.
        /// </summary>
        public void Add
            (
                ArraySegment<byte> chunk
            )
        {
            _memory.Add(chunk);
        }

        /// <summary>
        /// Проверка кода возврата.
        /// </summary>
        public bool CheckReturnCode() => GetReturnCode() >= 0;

        /// <summary>
        /// Проверка кода возврата.
        /// </summary>
        public bool CheckReturnCode
            (
                params int[] goodCodes
            )
        {
            if (GetReturnCode() < 0)
            {
                if (Array.IndexOf(goodCodes, ReturnCode) < 0)
                {
                    // throw new IrbisException(ReturnCode);
                    return false;
                }
            }

            return true;

        } // method CheckReturnCode

        /// <summary>
        /// Ищем преамбулу сырых бинарных данных.
        /// </summary>
        public bool FindPreamble()
        {
            var preamble = Constants.Preamble;
            var preambleLength = preamble.Length;

            while (!EOT)
            {
                var found = true;
                for (var i = 0; i < preambleLength; i++)
                {
                    var b = ReadByte();
                    if (b != preamble[i])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return true;
                }
            }

            return false;

        } // method FindPreamble

        /// <summary>
        /// Начальный разбор ответа сервера.
        /// </summary>
        public void Parse()
        {
            if (_memory.Count == 0)
            {
                EOT = true;
            }
            else
            {
                EOT = false;
                _currentChunk = _memory.First();
                _currentIndex = 0;
                _currentOffset = 0;

                Command = ReadAnsi();
                ClientId = ReadInteger();
                QueryId = ReadInteger();
                AnswerSize = ReadInteger();
                ServerVersion = ReadAnsi();
                ReadAnsi();
                ReadAnsi();
                ReadAnsi();
                ReadAnsi();
                ReadAnsi();
            }

        } // method Parse

        /// <summary>
        ///
        /// </summary>
        public byte Peek()
        {
            if (EOT)
            {
                return 0;
            }

            if (_currentOffset >= _currentChunk.Count)
            {
                _currentOffset = 0;
                _currentIndex++;
                if (_currentIndex >= _memory.Count)
                {
                    EOT = true;
                    return 0;
                }

                _currentChunk = _memory[_currentIndex];
            }

            return _currentChunk.Array![_currentChunk.Offset + _currentOffset];

        } // method Peek

        /// <summary>
        ///
        /// </summary>
        public byte ReadByte()
        {
            if (EOT)
            {
                return 0;
            }

            if (_currentOffset >= _currentChunk.Count)
            {
                _currentOffset = 0;
                _currentIndex++;
                if (_currentIndex >= _memory.Count)
                {
                    EOT = true;
                    return 0;
                }

                _currentChunk = _memory[_currentIndex];
            }

            var result = _currentChunk.Array![_currentChunk.Offset + _currentOffset];
            _currentOffset++;

            if (_currentOffset > _currentChunk.Count)
            {
                _currentOffset = 0;
                _currentIndex++;
                if (_currentIndex >= _memory.Count)
                {
                    EOT = true;
                }
                else
                {
                    _currentChunk = _memory[_currentIndex];
                }
            }

            return result;

        } // method ReadByte

        /// <summary>
        ///
        /// </summary>
        public byte[] ReadLine()
        {
            using var result = new MemoryStream();
            while (true)
            {
                var one = ReadByte();
                if (one == 0)
                {
                    break;
                }

                if (one == 13)
                {
                    if (Peek() == 10)
                    {
                        ReadByte();
                    }

                    break;
                }

                if (one == 10)
                {
                    break;
                }

                result.WriteByte(one);
            }

            return result.ToArray();

        } // method ReadLine

        /// <summary>
        ///
        /// </summary>
        public string ReadLine
            (
                Encoding encoding
            )
        {
            var bytes = ReadLine();
            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            return encoding.GetString(bytes);

        } // method ReadLine

        /// <summary>
        ///
        /// </summary>
        public byte[] RemainingBytes()
        {
            if (EOT)
            {
                return Array.Empty<byte>();
            }

            var length = _currentChunk.Count - _currentOffset;

            for (var i = _currentIndex + 1; i < _memory.Count; i++)
            {
                length += _memory[i].Count;
            }

            if (length == 0)
            {
                EOT = true;

                return Array.Empty<byte>();
            }

            var result = new byte[length];
            var offset = 0;
            Array.Copy
                (
                    _currentChunk.Array!,
                    _currentChunk.Offset + _currentOffset,
                    result,
                    0,
                    _currentChunk.Count - _currentOffset
                );
            offset += _currentChunk.Count - _currentOffset;
            for (var i = _currentIndex + 1; i < _memory.Count; i++)
            {
                var chunk = _memory[i];
                Array.Copy(_memory[i].Array!, 0, result, offset, _memory[i].Count);
                offset += chunk.Count;
            }

            return result;

        } // method RemainingBytes

        /// <summary>
        ///
        /// </summary>
        public string RemainingText
            (
                Encoding encoding
            )
        {
            var bytes = RemainingBytes();
            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            return encoding.GetString(bytes);

        } // method RemainingText

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void Debug
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            foreach (var memory in _memory)
            {
                foreach (var b in memory)
                {
                    writer.Write($" {b:X2}");
                }
            }

        } // method Debug

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void Debug
            (
                string fileName
            )
        {
            using var writer = File.CreateText(fileName);
            Debug(writer);

        } // method Debug

        /// <summary>
        /// Debug dump.
        /// </summary>
        public void DebugUtf
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            foreach (var memory in _memory)
            {
                writer.Write(Encoding.UTF8.GetString(memory.Array!, memory.Offset, memory.Count));
            }

        } // method DebugUtf

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugUtf
            (
                string fileName
            )
        {
            using var writer = File.CreateText(fileName);
            DebugUtf(writer);

        } // method DebugUtf

        /// <summary>
        /// Debug dump.
        /// </summary>
        public void DebugAnsi
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            foreach (var memory in _memory)
            {
                writer.Write(Utility.Ansi.GetString(memory.Array!, memory.Offset, memory.Count));
            }

        } // method DebugAnsi

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugAnsi
            (
                string fileName
            )
        {
            using var writer = File.CreateText(fileName);
            Debug(writer);

        } // method DebugAnsi

        /// <summary>
        ///
        /// </summary>
        public int GetReturnCode()
        {
            ReturnCode = ReadInteger();

            return ReturnCode;

        } // method GetReturnCode

        /// <summary>
        ///
        /// </summary>
        public string ReadAnsi() => ReadLine(Utility.Ansi);

        /// <summary>
        ///
        /// </summary>
        public int ReadInteger() => ReadLine(Utility.Ansi).SafeToInt32();

        /// <summary>
        ///
        /// </summary>
        public string[]? ReadAnsiStrings
            (
                int count
            )
        {
            var result = new List<string>(count);
            for (var i = 0; i < count; i++)
            {
                var line = ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }

                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get array of ANSI strings.
        /// </summary>
        /// <returns><c>null</c>if there is no lines in
        /// the server response, otherwise missing lines will
        /// be added (as empty lines).</returns>
        public string[]? ReadAnsiStringsPlus
            (
                int count
            )
        {
            var result = new List<string>(count);
            var index = 0;
            string line;
            for (; index < 1; index++)
            {
                line = ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }
                result.Add(line);
            }
            for (; index < count; index++)
            {
                line = ReadAnsi();
                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Require ANSI-encoded line.
        /// </summary>
        public string RequireAnsi()
        {
            var result = ReadAnsi();
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException();
            }

            return result;
        }

        /// <summary>
        /// Require UTF8-encoded line.
        /// </summary>
        public string RequireUtf()
        {
            var result = ReadUtf();
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException();
            }

            return result;
        }

        /// <summary>
        /// Require integer value.
        /// </summary>
        public int RequireInteger()
        {
            var line = ReadAnsi();
            var result = int.Parse(line);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> EnumRemainingAnsiLines()
        {
            while (!EOT)
            {
                string line;
                try
                {
                    line = ReadAnsi();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> EnumRemainingNonNullAnsiLines()
        {
            while (!EOT)
            {
                string line;
                try
                {
                    line = ReadAnsi();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> EnumRemainingUtfLines()
        {
            while (!EOT)
            {
                string line;
                try
                {
                    line = ReadUtf();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> EnumRemainingNonNullUtfLines()
        {
            while (!EOT)
            {
                string line;
                try
                {
                    line = ReadUtf();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<byte[]> EnumRemainingBinaryLines()
        {
            while (!EOT)
            {
                byte[] line;
                try
                {
                    line = ReadLine();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        /// Чтение нескольких строк в кодировке ANSI.
        /// </summary>
        public string[]? GetAnsiStrings
            (
                int lineCount
            )
        {
            var result = new List<string>();

            for (var i = 0; i < lineCount; i++)
            {
                if (EOT)
                {
                    return null;
                }

                var line = ReadAnsi();
                result.Add(line);
            }

            return result.ToArray();

        } // method GetAnsiStrings

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string[] ReadRemainingAnsiLines()
        {
            var result = new List<string>();

            while (!EOT)
            {
                var line = ReadAnsi();
                result.Add(line);
            }

            return result.ToArray();

        } // method ReadRemainingAnsiLines

        /// <summary>
        ///
        /// </summary>
        public string ReadRemainingAnsiText() => RemainingText(Utility.Ansi);

        /// <summary>
        ///
        /// </summary>
        public string[] ReadRemainingUtfLines()
        {
            var result = new List<string>();

            while (!EOT)
            {
                string line = ReadLine(Encoding.UTF8);
                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        public string ReadRemainingUtfText() => RemainingText(Encoding.UTF8);

        /// <summary>
        /// Чтение строки в кодировке UTF-8.
        /// </summary>
        public string ReadUtf() => ReadLine(Encoding.UTF8);

        /// <summary>
        /// Чтение целого числа.
        /// </summary>
        public int RequireInt32()
        {
            if (EOT)
            {
                throw new IrbisException("Unexpected end of response");
            }

            var line = ReadLine(Utility.Ansi);

            return int.Parse(line, CultureInfo.InvariantCulture);

        } // method RequireInt32

        #endregion

    } // class Response

    /// <summary>
    /// Клиентский запрос к серверу ИРБИС64
    /// (для синхронного сценария).
    /// </summary>
    public readonly struct SyncQuery
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SyncQuery
            (
                SyncConnection connection,
                string commandCode
            )
            : this()
        {
            _writer = new MemoryStream(2048);

            // Заголовок запроса
            AddAnsi(commandCode);
            AddAnsi(connection.Workstation);
            AddAnsi(commandCode);
            Add(connection.ClientId);
            Add(connection.QueryId);
            AddAnsi(connection.Password);
            AddAnsi(connection.Username);
            NewLine();
            NewLine();
            NewLine();

        } // constructor

        #endregion

        #region Private members

        private readonly MemoryStream _writer;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление строки с целым числом (плюс перевод строки).
        /// </summary>
        public void Add
            (
                int value
            )
        {
            var buffer = new byte[12];
            var length = Private.Int32ToBytes(value, buffer);
            _writer.Write(buffer, 0, length);
            NewLine();

        } // method Add

        /// <summary>
        /// Добавление строки с флагом "да-нет".
        /// </summary>
        public void Add(bool value) => Add(value ? 1 : 0);

        /// <summary>
        /// Добавление строки в кодировке ANSI (плюс перевод строки).
        /// </summary>
        public void AddAnsi
            (
                string? value
            )
        {
            if (value is not null)
            {
                var bytes = Utility.Ansi.GetBytes(value);
                _writer.Write(bytes, 0, bytes.Length);
            }

            NewLine();

        } // method AddAnsi

        /// <summary>
        /// Добавление строки в кодировке UTF-8 (плюс перевод строки).
        /// </summary>
        public void AddUtf
            (
                string? value
            )
        {
            if (value is not null)
            {
                var bytes = Encoding.UTF8.GetBytes(value);
                _writer.Write(bytes, 0, bytes.Length);
            }

            NewLine();

        } // method AddUtf

        /// <summary>
        /// Добавление формата.
        /// </summary>
        public void AddFormat
            (
                string? format
            )
        {
            if (ReferenceEquals(format, null) || format.Length == 0)
            {
                NewLine();
            }
            else
            {
                format = format.Trim();
                if (string.IsNullOrEmpty(format))
                {
                    NewLine();
                }
                else
                {
                    if (format.StartsWith("@"))
                    {
                        AddAnsi(format);
                    }
                    else
                    {
                        var prepared = Private.PrepareFormat(format);
                        AddUtf("!" + prepared);
                    }
                }
            }

        } // method AddFormat

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void Debug
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            var span = GetBody();
            foreach (var b in span)
            {
                writer.Write($" {b:X2}");
            }

        } // method Debug

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugAnsi
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            writer.WriteLine (Utility.Ansi.GetString (_writer.ToArray()));

        } // method DebugUtf

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugUtf
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            writer.WriteLine (Encoding.UTF8.GetString (_writer.ToArray()));

        } // method DebugUtf

        /// <summary>
        /// Получение массива байтов, из которых состоит
        /// клиентский запрос.
        /// </summary>
        public byte[] GetBody() => _writer.ToArray();

        /// <summary>
        /// Подсчет общей длины запроса (в байтах).
        /// </summary>
        public int GetLength() => unchecked((int)_writer.Length);

        /// <summary>
        /// Добавление одного перевода строки.
        /// </summary>
        public void NewLine() => _writer.WriteByte(10);

        #endregion

    } // struct SyncQuery

    /// <summary>
    /// Навигатор по тексту, оформленный как структура.
    /// </summary>
    public ref struct ValueTextNavigator
    {
        #region Constants

        /// <summary>
        /// Признак конца текста.
        /// </summary>
        public const char EOF = '\0';

        #endregion

        #region Properties

        /// <summary>
        /// Текст закончился?
        /// </summary>
        public bool IsEOF => _position >= _text.Length;

        /// <summary>
        /// Общая длина текста в символах.
        /// </summary>
        public int Length => _text.Length;

        /// <summary>
        /// Текущая позиция в тексте.
        /// </summary>
        public int Position => _position;

        /// <summary>
        /// Текст, хранимый в навигаторе.
        /// </summary>
        public string Text => _text.ToString();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="text">Текст</param>
        public ValueTextNavigator
            (
                ReadOnlySpan<char> text
            )
        {
            _text = text;
            _position = 0;
        } // constructor

        #endregion

        #region Private members

        private readonly ReadOnlySpan<char> _text;
        private int _position;

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование навигатора (включая текущую позицию в тексте).
        /// </summary>
        public ValueTextNavigator Clone()
        {
            var result = new ValueTextNavigator(_text)
            {
                _position = _position
            };

            return result;

        } // method Clone

        /// <summary>
        /// Навигатор по текстовому файлу.
        /// </summary>
        public static ValueTextNavigator FromFile
            (
                string fileName,
                Encoding? encoding = default
            )
        {
            encoding ??= Encoding.UTF8;
            var text = File.ReadAllText(fileName, encoding);
            var result = new ValueTextNavigator(text.AsSpan());

            return result;

        } // method FromFile

        /// <summary>
        /// Выдать остаток текста.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> GetRemainingText() => IsEOF
            ? new ReadOnlySpan<char>()
            : _text.Slice(_position);

        /// <summary>
        /// Текущий символ - управляющий?
        /// </summary>
        public bool IsControl() => char.IsControl(PeekChar());

        /// <summary>
        /// Текущий символ - цифра?
        /// </summary>
        public bool IsDigit() => char.IsDigit(PeekChar());

        /// <summary>
        /// Текущий символ - буква?
        /// </summary>
        public bool IsLetter() => char.IsLetter(PeekChar());

        /// <summary>
        /// Текущий символ - буква или цифра?
        /// </summary>
        public bool IsLetterOrDigit() => char.IsLetterOrDigit(PeekChar());

        /// <summary>
        /// Текущий символ - часть числа?
        /// </summary>
        public bool IsNumber() => char.IsNumber(PeekChar());

        /// <summary>
        /// Текущий символ - знак пунктуации?
        /// </summary>
        public bool IsPunctuation() => char.IsPunctuation(PeekChar());

        /// <summary>
        /// Текущий символ - разделитель?
        /// </summary>
        public bool IsSeparator() => char.IsSeparator(PeekChar());

        /// <summary>
        /// Текущий символ принадлежит одной
        /// из Unicode категорий: MathSymbol,
        /// CurrencySymbol, ModifierSymbol либо OtherSymbol?
        /// </summary>
        public bool IsSymbol() => char.IsSymbol(PeekChar());

        /// <summary>
        /// Текущий символ - пробельный?
        /// </summary>
        public bool IsWhiteSpace() => char.IsWhiteSpace(PeekChar());

        /// <summary>
        /// Заглядывание вперёд на одну позицию.
        /// Движения по тексту не происходит.
        /// </summary>
        /// <remarks>Это на одну позицию дальше,
        /// чем <see cref="PeekChar()"/>
        /// </remarks>
        public char LookAhead()
        {
            var ahead = _position + 1;
            return ahead >= _text.Length
                ? EOF
                : _text[ahead];

        } // method LookAhead

        /// <summary>
        /// Заглядывание вперёд на указанное количество символов.
        /// Движения по тексту не происходит.
        /// </summary>
        public char LookAhead
            (
                int distance
            )
        {
            var ahead = _position + distance;
            return ahead >= _text.Length
                ? EOF
                : _text[ahead];

        } // method LookAhead

        /// <summary>
        /// Заглядывание назад на одну позицию.
        /// Движения по тексту не происходит.
        /// </summary>
        public char LookBehind() => _position == 0 ? EOF : _text[_position - 1];

        /// <summary>
        /// Заглядывание назад на указанное число позиций.
        /// Движения по тексту не происходит.
        /// </summary>
        /// <param name="distance">Дистанция, на которую
        /// предполагается заглянуть - положительное число,
        /// означающее количество символов, на которые
        /// нужно "отмотать назад".</param>
        public char LookBehind (int distance) => _position < distance ? EOF : _text[_position - distance];

        /// <summary>
        /// Относительное перемещение указателя на указанную дистанцию.
        /// </summary>
        /// <remarks>
        /// Переместить указатель за пределы текста не получится,
        /// он остановится в крайней (начальной или конечной) позиции.
        /// </remarks>
        public void Move (int distance) =>
            _position = Math.Max (0, Math.Min(_position + distance, _text.Length));

        /// <summary>
        /// Подглядывание текущего символа (т. е. символа в текущей позиции).
        /// </summary>
        public char PeekChar() => _position >= _text.Length
                ? EOF
                : _text[_position];

        /// <summary>
        /// Подглядывание строки вплоть до указанной длины.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        /// <remarks>
        /// Символы перевода строки в данном методе
        /// считаются обычными символами и включаются в результат.
        /// </remarks>
        public ReadOnlySpan<char> PeekString
            (
                int length
            )
        {
            if (IsEOF)
            {
                return ReadOnlySpan<char>.Empty;
            }

            var start = _position;
            for (var i = 0; i < length; i++)
            {
                var c = ReadChar();
                if (c == EOF)
                {
                    break;
                }
            }

            var result = _text.Slice(start, _position - start);
            _position = start;

            return result;

        } // method PeekString

        /// <summary>
        /// Подглядывание вплоть до указанного символа
        /// (включая его).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> PeekTo
            (
                char stopChar
            )
        {
            var position = _position;
            var result = ReadTo(stopChar);
            _position = position;

            return result;

        } // method PeekTo

        /// <summary>
        /// Подглядывание вплоть до указанных символов
        /// (включая их).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> PeekTo
            (
                params char[] stopChars
            )
        {
            var position = _position;
            var result = ReadTo(stopChars);
            _position = position;

            return result;

        } // method PeekTo

        /// <summary>
        /// Подглядывание вплоть до указанного символа
        /// (не включая его).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> PeekUntil
            (
                char stopChar
            )
        {
            var position = _position;
            var result = ReadUntil(stopChar);
            _position = position;

            return result;

        } // method PeekUntil

        /// <summary>
        /// Подглядывание вплоть до указанных символов
        /// (не включая их).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> PeekUntil
            (
                params char[] stopChars
            )
        {
            var position = _position;
            var result = ReadUntil(stopChars);
            _position = position;

            return result;

        } // metdho PeekUntil

        /// <summary>
        /// Считывание одного символа.
        /// Если достигнут конец текста, возвращается
        /// <see cref="EOF"/>.
        /// </summary>
        public char ReadChar()
        {
            if (_position >= _text.Length)
            {
                return EOF;
            }

            return _text[_position++];

        } // method ReadChar

        /// <summary>
        /// Считывание экранированной строки вплоть до разделителя
        /// (не включая его).
        /// </summary>
        /// <param name="escapeChar">Экранирующий символ,
        /// как правило, '\\'.</param>
        /// <param name="stopChar">Стоп-символ (разделитель).</param>
        /// <returns><c>null</c>, если достигнут конец текста.
        /// </returns>
        /// <exception cref="FormatException">Нарушен формат (есть символ
        /// экранирования, но за ним строка обрывается).
        /// </exception>
        public string? ReadEscapedUntil
            (
                char escapeChar,
                char stopChar
            )
        {
            if (IsEOF)
            {
                return null;
            }

            var result = new StringBuilder();
            while (true)
            {
                var c = ReadChar();
                if (c == EOF)
                {
                    break;
                }

                if (c == escapeChar)
                {
                    c = ReadChar();
                    if (c == EOF)
                    {
                        throw new FormatException();
                    }
                    result.Append(c);
                }
                else if (c == stopChar)
                {
                    break;
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();

        } // method ReadEscapedUntil

        /// <summary>
        /// Считывание начиная с открывающего символа
        /// до закрывающего (включая их).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или нет открывающего либо закрывающего символа.
        /// </returns>
        public ReadOnlySpan<char> ReadFrom
            (
                char openChar,
                char closeChar
            )
        {
            if (IsEOF)
            {
                return ReadOnlySpan<char>.Empty;
            }

            var start = _position;
            if (PeekChar() != openChar)
            {
                return ReadOnlySpan<char>.Empty;
            }
            ReadChar();

            while (true)
            {
                var c = ReadChar();
                if (c == EOF)
                {
                    _position = start;
                    return ReadOnlySpan<char>.Empty;
                }

                if (c == closeChar)
                {
                    break;
                }
            }

            return _text.Slice
                (
                    start,
                    _position - start
                );

        } // method ReadFrom

        /// <summary>
        /// Считывание начиная с открывающего символа
        /// до закрывающего (включая их).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или если нет открывающего либо закрывающего символа.
        /// </returns>
        public ReadOnlySpan<char> ReadFrom
            (
                ReadOnlySpan<char> openChars,
                ReadOnlySpan<char> closeChars
            )
        {
            if (IsEOF)
            {
                return ReadOnlySpan<char>.Empty;
            }

            var start = _position;
            if (!Private.Contains(openChars, PeekChar()))
            {
                return ReadOnlySpan<char>.Empty;
            }
            ReadChar();

            while (true)
            {
                var c = ReadChar();
                if (c == EOF)
                {
                    _position = start;
                    return ReadOnlySpan<char>.Empty;
                }
                if (Private.Contains(closeChars, c))
                {
                    break;
                }
            }

            return _text.Slice
                (
                    start,
                    _position - start
                );

        } // metdhod ReadFrom

        /// <summary>
        /// Чтение беззнакового целого.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ не цифровой.</returns>
        public ReadOnlySpan<char> ReadInteger()
        {
            if (!IsDigit())
            {
                return ReadOnlySpan<char>.Empty;
            }

            var startPosition = _position;
            while (IsDigit())
            {
                ReadChar();
            }

            return _text.Slice
                (
                    startPosition,
                    _position - startPosition
                );

        } // method ReadInteger

        /// <summary>
        /// Чтение до конца строки.
        /// </summary>
        public ReadOnlySpan<char> ReadLine()
        {
            var startPosition = _position;
            while (!IsEOF)
            {
                var c = PeekChar();
                if (c == '\r' || c == '\n')
                {
                    break;
                }
                ReadChar();
            }

            var stopPosition = _position;
            if (!IsEOF)
            {
                var c = PeekChar();
                if (c == '\r')
                {
                    ReadChar();
                    c = PeekChar();
                }

                if (c == '\n')
                {
                    ReadChar();
                }
            }

            return _text.Slice
                (
                    startPosition,
                    stopPosition - startPosition
                );

        } // method ReadLine

        /// <summary>
        /// Чтение строки вплоть до указанной длины.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> ReadString
            (
                int length
            )
        {
            var startPosition = _position;
            for (var i = 0; i < length; i++)
            {
                var c = ReadChar();
                if (c == EOF)
                {
                    break;
                }
            }

            return Substring
                (
                    startPosition,
                    _position - startPosition
                );

        } // method ReadString

        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (включая его).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> ReadTo
            (
                char stopChar
            )
        {
            var startPosition = _position;
            while (true)
            {
                var c = ReadChar();
                if (c == EOF || c == stopChar)
                {
                    break;
                }
            }

            return Substring
                (
                    startPosition,
                    length: _position - startPosition
                );

        } // method ReadTo

        /// <summary>
        /// Считывание вплоть до указанного разделителя
        /// (разделитель не помещается в возвращаемое значение,
        /// однако, считывается).
        /// </summary>
        /// <remarks><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </remarks>
        public ReadOnlySpan<char> ReadToString
            (
                ReadOnlySpan<char> stopString
            )
        {
            // Sure.NotNullNorEmpty(stopString, nameof(stopString));

            var savePosition = _position;
            var length = 0;
            while (true)
            {
                AGAIN:
                var c = ReadChar();
                if (c == EOF)
                {
                    _position = savePosition;
                    return ReadOnlySpan<char>.Empty;
                }

                length++;
                if (length >= stopString.Length)
                {
                    var start = _position - stopString.Length;
                    for (var i = 0; i < stopString.Length; i++)
                    {
                        if (_text[start + i] != stopString[i])
                        {
                            goto AGAIN;
                        }
                    }
                    break;
                }
            }

            return Substring
                (
                    savePosition,
                    _position - savePosition - stopString.Length
                );

        } // method ReadTo

        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (включая один из них).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> ReadTo
            (
                params char[] stopChars
            )
        {
            var start = _position;
            while (true)
            {
                var c = ReadChar();
                if (c == EOF
                    || Array.IndexOf(stopChars, c) >= 0)
                {
                    break;
                }
            }

            return _text.Slice
                (
                    start: start,
                    length: _position - start
                );

        } // method ReadTo

        /// <summary>
        /// Считывание вплоть до указанного символа
        /// (не включая его).
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </returns>
        public ReadOnlySpan<char> ReadUntil
            (
                char stopChar
            )
        {
            var start = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF || c == stopChar)
                {
                    break;
                }
                ReadChar();
            }

            return _text.Slice
                (
                    start,
                    _position - start
                );

        } // method ReadUntil

        /// <summary>
        /// Считывание вплоть до указанного разделителя
        /// (разделитель не помещается в возвращаемое значение
        /// и не считывается).
        /// </summary>
        /// <remarks><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </remarks>
        public ReadOnlySpan<char> ReadUntilString
            (
                ReadOnlySpan<char> stopString
            )
        {
            // Sure.NotNullNorEmpty(stopString, nameof(stopString));

            var position = _position;
            var length = 0;
            while (true)
            {
                AGAIN:
                var c = ReadChar();
                if (c == EOF)
                {
                    _position = position;
                    return ReadOnlySpan<char>.Empty;
                }

                length++;
                if (length >= stopString.Length)
                {
                    var start = _position - stopString.Length;
                    for (var i = 0; i < stopString.Length; i++)
                    {
                        if (_text[start + i] != stopString[i])
                        {
                            goto AGAIN;
                        }
                    }
                    break;
                }
            }

            var result = _text.Slice
                (
                    position,
                    _position - position - stopString.Length
                );
            _position -= stopString.Length;

            return result;

        } // method ReadUntil

        /// <summary>
        /// Считывание вплоть до указанных символов
        /// (не включая их).
        /// </summary>
        /// <remarks><c>Пустой фрагмент</c>, если достигнут конец текста.
        /// </remarks>
        public ReadOnlySpan<char> ReadUntil
            (
                params char[] stopChars
            )
        {
            var savePosition = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF
                    || Array.IndexOf(stopChars, c) >= 0)
                {
                    break;
                }
                ReadChar();
            }

            return _text.Slice
                (
                    savePosition,
                    _position - savePosition
                );

        } // method ReadUntil

        /// <summary>
        /// Считывание вплоть до указанных символов
        /// (не включая их).
        /// </summary>
        /// <remarks><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ не открывающий.
        /// </remarks>
        public ReadOnlySpan<char> ReadUntil
            (
                ReadOnlySpan<char> openChars,
                ReadOnlySpan<char> closeChars,
                ReadOnlySpan<char> stopChars
            )
        {
            var start = _position;
            var level = 0;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF)
                {
                    _position = start;
                    return ReadOnlySpan<char>.Empty;
                }

                if (Private.Contains(openChars, c))
                {
                    level++;
                }
                else if (Private.Contains(closeChars, c))
                {
                    if (level == 0
                        && Private.Contains(stopChars, c))
                    {
                        break;
                    }
                    level--;
                }
                else if (Private.Contains(stopChars, c))
                {
                    if (level == 0)
                    {
                        break;
                    }
                }
                ReadChar();
            }

            return _text.Slice
                (
                    start,
                    _position - start
                );

        } // method ReadUntil

        /// <summary>
        /// Считывание, пока встречается указанный символ.
        /// </summary>
        /// <returns><c>Простой фрагмент</c>, если достигнут конец текста
        /// или текущий символ не совпадает с указанным.
        /// </returns>
        public ReadOnlySpan<char> ReadWhile
            (
                char goodChar
            )
        {
            var startPosition = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF || c != goodChar)
                {
                    break;
                }
                ReadChar();
            }

            return _text.Slice
                (
                    startPosition,
                    _position - startPosition
                );

        } // method ReadWhile

        /// <summary>
        /// Считывание, пока встречается указанные символы.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ не входит в число "хороших".
        /// </returns>
        public ReadOnlySpan<char> ReadWhile
            (
                params char[] goodChars
            )
        {
            var start = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF
                    || Array.IndexOf(goodChars, c) < 0)
                {
                    break;
                }
                ReadChar();
            }

            return _text.Slice
                (
                    start,
                    _position - start
                );

        } // method ReadWhile

        /// <summary>
        /// Считываем слово, начиная с текущей позиции.
        /// </summary>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ "не-словесный".
        /// </returns>
        public ReadOnlySpan<char> ReadWord()
        {
            var startPosition = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF
                    || !char.IsLetterOrDigit(c))
                {
                    break;
                }
                ReadChar();
            }

            return _text.Slice
                (
                    startPosition,
                    _position - startPosition
                );

        } // metdhod ReadWord

        /// <summary>
        /// Считываем слово под курсором.
        /// </summary>
        /// <param name="additionalWordCharacters">Дополнительные символы,
        /// которые мы полагаем "словесными".</param>
        /// <returns><c>Пустой фрагмент</c>, если достигнут конец текста
        /// или текущий символ "не-словесный".
        /// </returns>
        public ReadOnlySpan<char> ReadWord
            (
                params char[] additionalWordCharacters
            )
        {
            var savePosition = _position;
            while (true)
            {
                var c = PeekChar();
                if (c == EOF
                    || !char.IsLetterOrDigit(c)
                        && Array.IndexOf(additionalWordCharacters, c) < 0)
                {
                    break;
                }
                ReadChar();
            }

            return _text.Slice
                (
                    savePosition,
                    _position - savePosition
                );

        } // method ReadWord

        /// <summary>
        /// Получаем недавно считанный текст указанной длины.
        /// </summary>
        /// <param name="length">Желаемая длина текста в символах
        /// (положительное целое).</param>
        public ReadOnlySpan<char> RecentText
            (
                int length
            )
        {
            var start = _position - length;
            if (start < 0)
            {
                length += start;
                start = 0;
            }

            if (start >= _text.Length)
            {
                length = 0;
                start = _text.Length - 1;
            }

            if (length < 0)
            {
                length = 0;
            }

            return Substring (start, length);

        } // method RecentText

        /// <summary>
        /// Пропускает один символ, если он совпадает с указанным.
        /// </summary>
        /// <returns><c>true</c>, если символ был съеден успешно.
        /// </returns>
        public bool SkipChar
            (
                char c
            )
        {
            if (PeekChar() == c)
            {
                ReadChar();

                return true;
            }

            return false;

        } // method SkipChar

        /// <summary>
        /// Пропускает указанное число символов.
        /// </summary>
        /// <returns><c>true</c>, если ещё остались непрочитанные символы.
        /// </returns>
        public bool SkipChar
            (
                int n
            )
        {
            for (var i = 0; i < n; i++)
            {
                ReadChar();
            }

            return !IsEOF;

        } // method SkipChar

        /// <summary>
        /// Пропускает один символ, если он совпадает с любым
        /// из указанных.
        /// </summary>
        /// <returns><c>true</c>, если символ был съеден успешно
        /// </returns>
        public bool SkipChar
            (
                params char[] allowed
            )
        {
            if (Array.IndexOf(allowed, PeekChar()) >= 0)
            {
                ReadChar();
                return true;
            }

            return false;

        } // method SkipChar

        /// <summary>
        /// Пропускаем управляющие символы.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
        public bool SkipControl()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                if (IsControl())
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkipControl

        /// <summary>
        /// Пропускаем пунктуацию.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
        public bool SkipPunctuation()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                if (IsPunctuation())
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkipPunctuation

        /// <summary>
        /// Пропускаем "не-словесные" символы.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
        public bool SkipNonWord()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                var c = PeekChar();
                if (!char.IsLetterOrDigit(c))
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkipNonWord

        /// <summary>
        /// Пропускаем "не-словесные" символы.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
        public bool SkipNonWord
            (
                params char[] additionalWordCharacters
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                var c = PeekChar();
                if (!char.IsLetterOrDigit(c)
                    && Array.LastIndexOf(additionalWordCharacters, c) < 0)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkipNonWord

        /// <summary>
        /// Пропускаем произвольное количество символов
        /// из указанного диапазона (например, от 'A' до 'Z').
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
        public bool SkipRange
            (
                char fromChar,
                char toChar
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                var c = PeekChar();
                if (c >= fromChar && c <= toChar)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkipRange

        /// <summary>
        /// Пропустить указанный символ.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
        public bool SkipWhile
            (
                char skipChar
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                var c = PeekChar();
                if (c == skipChar)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkipWhile

        /// <summary>
        /// Пропустить указанные символы.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
        public bool SkipWhile
            (
                params char[] skipChars
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                var c = PeekChar();
                if (Array.IndexOf(skipChars, c) >= 0)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkipWhile

        /// <summary>
        /// Пропустить, пока не встретится указанный стоп-символ.
        /// Сам стоп-символ не считывается.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
        public bool SkipTo
            (
                char stopChar
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                var c = PeekChar();
                if (c == stopChar)
                {
                    return true;
                }

                ReadChar();
            }

        } // method SkipTo

        /// <summary>
        /// Пропустить, пока не встретятся указанные символы.
        /// </summary>
        /// <returns><c>false</c>, если достигнут конец текста.
        /// </returns>
        public bool SkipWhileNot
            (
                params char[] goodChars
            )
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                var c = PeekChar();
                if (Array.IndexOf(goodChars, c) < 0)
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkipWhileNot

        /// <summary>
        /// Пропускаем пробельные символы.
        /// </summary>
        public bool SkipWhitespace()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                if (IsWhiteSpace())
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkiWhitespace

        /// <summary>
        /// Пропускаем пробельные символы и пунктуацию.
        /// </summary>
        public bool SkipWhitespaceAndPunctuation()
        {
            while (true)
            {
                if (IsEOF)
                {
                    return false;
                }

                if (IsWhiteSpace() || IsPunctuation())
                {
                    ReadChar();
                }
                else
                {
                    return true;
                }
            }

        } // method SkipWhitespaceAndPunctuation

        /// <summary>
        /// Извлечение подстроки, начиная с указанного смещения.
        /// </summary>
        public ReadOnlySpan<char> Substring
            (
                int offset
            )
        {
            return offset < 0
                || offset >= _text.Length
                ? ReadOnlySpan<char>.Empty
                : _text.Slice(offset);
        } // method Substring

        /// <summary>
        /// Извлечение подстроки.
        /// </summary>
        /// <remarks>Если параметры заданы неверно,
        /// метод может выбросить исключение
        /// <see cref="ArgumentOutOfRangeException"/>.</remarks>
        /// <param name="offset">Смещение до начала подстроки в символах.</param>
        /// <param name="length">Длина подстроки в символах.</param>
        public ReadOnlySpan<char> Substring
            (
                int offset,
                int length
            )
        {
            return offset < 0
                || offset >= _text.Length
                || length <= 0
                ? ReadOnlySpan<char>.Empty
                : _text.Slice(offset, length);

        } // method Substring

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"Position={Position}";
        } // method ToString

        #endregion

    } // struct ValueTextNavigator

    /// <summary>
    /// Спецификация пути к файлу на сервере ИРБИС64.
    /// </summary>
    public sealed class FileSpecification
    {
        #region Properties

        /// <summary>
        /// Is the file binary or text?
        /// </summary>
        public bool BinaryFile { get; set; }

        /// <summary>
        /// Path.
        /// </summary>
        public IrbisPath Path { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// File content (when we want write the file).
        /// </summary>
        public string? Content { get; set; }

        #endregion

        #region Private members

        private static bool _CompareDatabases
            (
                string? first,
                string? second
            )
        {
            if (string.IsNullOrEmpty(first) && string.IsNullOrEmpty(second))
            {
                return true;
            }

            return first.SameString(second);

        } // method CompareDatabases

        #endregion

         #region Public methods

         /// <summary>
         /// Построение спецификации файла по ее компонентам.
         /// </summary>
         public static string Build
             (
                IrbisPath path,
                string database,
                string fileName
             )
         {
             return ((int) path).ToInvariantString()
                    + "."
                    + database
                    + "."
                    + fileName;
         }

        /// <summary>
        /// Parse the text specification.
        /// </summary>
        public static FileSpecification Parse
            (
                string text
            )
        {
            var navigator = new ValueTextNavigator(text.AsSpan());
            var path = int.Parse
                (
                    navigator.ReadTo('.').ToString(),
                    CultureInfo.InvariantCulture
                );
            var database = navigator.ReadTo('.').ToString().EmptyToNull();
            var fileName = navigator.GetRemainingText().ToString();
            var binaryFile = fileName.StartsWith("@");
            if (binaryFile)
            {
                fileName = fileName.Substring(1);
            }

            string? content = null;
            var position = fileName.IndexOf("&", StringComparison.InvariantCulture);
            if (position >= 0)
            {
                content = fileName.Substring(position + 1);
                fileName = fileName.Substring(0, position);
            }
            var result = new FileSpecification
            {
                BinaryFile = binaryFile,
                Path = (IrbisPath)path,
                Database = database,
                FileName = fileName,
                Content = content
            };

            return result;

        } // method Parse

        #endregion

        #region Object members

        /// <summary>
        /// Compare with other <see cref="FileSpecification"/>
        /// instance.
        /// </summary>
        public bool Equals
            (
                FileSpecification? other
            )
        {
            if (ReferenceEquals(other, null))
            {
                throw new ArgumentNullException();
            }

            return Path == other.Path
                   && _CompareDatabases(Database, other.Database)
                   && FileName.SameString(other.FileName);
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object? obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is FileSpecification other && Equals(other);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        // ReSharper disable NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Path;
                hashCode = (hashCode * 397)
                           ^ (Database != null ? Database.GetHashCode() : 0);
                hashCode = (hashCode * 397)
                    ^ (FileName != null ? FileName.GetHashCode() : 0);

                return hashCode;
            }

        } // method GetHashCode

        // ReSharper restore NonReadonlyMemberInGetHashCode

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var fileName = FileName;
            if (BinaryFile)
            {
                fileName = "@" + fileName;
            }
            else
            {
                if (!ReferenceEquals(Content, null))
                {
                    fileName = "&" + fileName;
                }
            }

            string result;

            switch (Path)
            {
                case IrbisPath.System:
                case IrbisPath.Data:
                    result = $"{(int) Path}..{fileName}";
                    break;

                default:
                    result = $"{(int) Path}.{Database}.{fileName}";
                    break;
            }

            if (!ReferenceEquals(Content, null))
            {
                result = result + "&" + Private.WindowsToIrbis(Content);
            }

            return result;

        } // method ToString

        #endregion

   } // class FileSpecification

    /// <summary>
    /// Работа с INI-файлами.
    /// </summary>
    public class IniFile
        : IEnumerable<IniFile.Section>,
        IDisposable
    {
        #region Nested classes

        /// <summary>
        /// Line (element) of the INI-file.
        /// </summary>
        [DebuggerDisplay("{Key}={Value}")]
        public sealed class Line
        {
            #region Properties

            /// <summary>
            /// Key (name) of the element.
            /// </summary>
            public string Key { get; private set; }

            /// <summary>
            /// Value of the element.
            /// </summary>
            public string? Value
            {
                get => _value;
                set
                {
                    _value = value;
                    Modified = true;
                }
            }

            /// <summary>
            /// Modification flag.
            /// </summary>
            public bool Modified { get; set; }

            #endregion

            #region Construction

            /// <summary>
            /// Default constructor.
            /// </summary>
            public Line()
            {
                Key = string.Empty;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Line
                (
                    string key,
                    string? value
                )
            {
                CheckKeyName(key);

                Key = key;
                _value = value;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            public Line
                (
                    string key,
                    string? value,
                    bool modified
                )
            {
                CheckKeyName(key);

                Key = key;
                _value = value;
                Modified = modified;
            }

            #endregion

            #region Private members

            private string? _value;

            #endregion

            #region Public methods

            /// <summary>
            /// Write the line to the stream.
            /// </summary>
            public void Write
                (
                    TextWriter writer
                )
            {
                if (string.IsNullOrEmpty(Value))
                {
                    writer.WriteLine(Key);
                }
                else
                {
                    writer.WriteLine
                        (
                            "{0}={1}",
                            Key, Value
                        );
                }
            }

            #endregion

            #region Object members

            /// <inheritdoc cref="object.ToString" />
            public override string ToString() => $"{Key}={Value}";

            #endregion
        }

        // =========================================================

        /// <summary>
        /// INI-file section.
        /// </summary>
        public sealed class Section
            : IEnumerable<Line>
        {
            #region Properties

            /// <summary>
            /// Count of lines.
            /// </summary>
            public int Count => _lines.Count;

            /// <summary>
            /// All the keys of the section.
            /// </summary>
            public IEnumerable<string> Keys
            {
                get
                {
                    foreach (var line in _lines)
                    {
                        yield return line.Key;
                    }
                }
            }

            /// <summary>
            /// Section is modified?
            /// </summary>
            public bool Modified { get; set; }

            /// <summary>
            /// Section name.
            /// </summary>
            public string? Name
            {
                get => _name;
                set => SetName(value.ThrowIfNull("value"));
            }

            /// <summary>
            /// INI-file.
            /// </summary>
            public IniFile Owner { get; private set; }

            /// <summary>
            /// Indexer.
            /// </summary>
            public string? this[string key]
            {
                get => GetValue(key, null);
                set => SetValue(key, value);
            }

            #endregion

            #region Construction

            internal Section
                (
                    IniFile owner,
                    string? name
                )
            {
                Owner = owner;
                _name = name;
                _lines = new List<Line>();
            }

            #endregion

            #region Private members

            private string? _name;

            private readonly List<Line> _lines;

            #endregion

            #region Public methods

            /// <summary>
            /// Add new item to the section.
            /// </summary>
            public void Add
                (
                    string key,
                    string? value
                )
            {
                var line = new Line(key, value);
                Add(line);

            } // method Add

            /// <summary>
            /// Add new line to the section.
            /// </summary>
            public void Add
                (
                    Line line
                )
            {
                CheckKeyName(line.Key);
                if (ContainsKey(line.Key))
                {
                    throw new ArgumentException("duplicate key " + line.Key);
                }

                _lines.Add(line);

            } // method Add

            /// <summary>
            /// Apply to other section.
            /// </summary>
            public void ApplyTo
                (
                    Section section
                )
            {
                foreach (var line in this)
                {
                    section[line.Key] = line.Value;
                }

            } // method ApplyTo

            /// <summary>
            /// Clear the section.
            /// </summary>
            public void Clear()
            {
                _lines.Clear();
                Modified = true;
                Owner.Modified = true;

            } // method Clear

            /// <summary>
            /// Whether the section have line with given key?
            /// </summary>
            public bool ContainsKey
                (
                    string key
                )
            {
                foreach (var line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        return true;
                    }
                }

                return false;

            } // method ContainsKey

            /// <summary>
            /// Get value associated with specified key.
            /// </summary>
            public string? GetValue
                (
                    string key,
                    string? defaultValue
                )
            {
                CheckKeyName(key);

                foreach (var line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        return line.Value;
                    }
                }

                return defaultValue;

            } // method GetValues

            /// <summary>
            /// Get value associated with given key.
            /// </summary>
            public T? GetValue<T>
                (
                    string key,
                    T? defaultValue
                )
            {
                var value = GetValue(key, null);
                if (string.IsNullOrEmpty(value))
                {
                    return defaultValue;
                }

                var result = Private.ConvertTo<T>(value);

                return result;
            }

            /// <summary>
            /// Remove specified key.
            /// </summary>
            public Section Remove
                (
                    string key
                )
            {
                CheckKeyName(key);

                foreach (var line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        _lines.Remove(line);
                        Modified = true;
                        Owner.Modified = true;
                        break;
                    }
                }

                return this;
            }

            /// <summary>
            /// Set name of the section.
            /// </summary>
            public void SetName
                (
                    string name
                )
            {
                _name = name;
                Modified = true;
                Owner.Modified = true;
            }

            /// <summary>
            /// Set value associated with given key.
            /// </summary>
            public Section SetValue
                (
                    string key,
                    string? value
                )
            {
                CheckKeyName(key);

                Line? target = null;
                foreach (var line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        target = line;
                        break;
                    }
                }

                if (ReferenceEquals(target, null))
                {
                    target = new Line(key, value);
                    _lines.Add(target);
                }

                target.Value = value;

                return this;
            }

            /// <summary>
            /// Set value associate with given key.
            /// </summary>
            public Section SetValue<T>
                (
                    string key,
                    T value
                )
            {
                CheckKeyName(key);

                if (ReferenceEquals(value, null))
                {
                    Remove(key);
                }
                else
                {
                    var text = value.ToString();
                    SetValue(key, text);
                }

                return this;
            }

            /// <summary>
            /// Try to get value for given key.
            /// </summary>
            public bool TryGetValue
                (
                    string key,
                    out string? value
                )
            {
                CheckKeyName(key);

                foreach (var line in _lines)
                {
                    if (line.Key.SameString(key))
                    {
                        value = line.Value;
                        return true;
                    }
                }

                value = null;

                return false;
            }

            #endregion

            #region IEnumerable<Line> members

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
            public IEnumerator<Line> GetEnumerator() => _lines.GetEnumerator();

            #endregion

            #region Object members

            /// <inheritdoc cref="object.ToString" />
            public override string ToString()
            {
                var result = new StringBuilder();
                result
                    .AppendFormat("[{0}]", Name)
                    .AppendLine();

                foreach (var line in _lines)
                {
                    result.AppendLine(line.ToString());
                }

                return result.ToString();
            }

            #endregion
        }

        #endregion

        // =========================================================

        #region Properties

        /// <summary>
        /// Encoding.
        /// </summary>
        public Encoding? Encoding { get; set; }

        /// <summary>
        /// Name of the file.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Modified?
        /// </summary>
        public bool Modified { get; set; }

        /// <summary>
        /// Section indexer.
        /// </summary>
        public Section? this[string sectionName] => GetSection(sectionName);

        /// <summary>
        /// Value indexer.
        /// </summary>
        public string? this
            [
                string sectionName,
                string keyName
            ]
        {
            get => GetValue(sectionName, keyName, null);
            set => SetValue(sectionName, keyName, value);
        }

        /// <summary>
        /// Writable?
        /// </summary>
        public bool Writable { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IniFile()
        {
            _sections = new List<Section>();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public IniFile
            (
                string fileName,
                Encoding? encoding = null,
                bool writable = false
            )
            : this()
        {
            FileName = fileName;
            Encoding = encoding;
            Writable = writable;

            Read();
        }

        #endregion

        #region Private members

        private readonly List<Section> _sections;

        internal static void CheckKeyName
            (
                string keyName
            )
        {
            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentException(nameof(keyName));
            }

            if (keyName.Contains("="))
            {
                throw new ArgumentException(nameof(keyName));
            }

        } // method CheckKeyName

        private static void _SaveSection
            (
                TextWriter writer,
                Section section
            )
        {
            if (!string.IsNullOrEmpty(section.Name))
            {
                writer.WriteLine
                    (
                        "[{0}]", section.Name
                    );
            }

            foreach (var line in section)
            {
                line.Write(writer);
            }

        } // method _SaveSection

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the INI-file.
        /// </summary>
        public void ApplyTo
            (
                IniFile iniFile
            )
        {
            foreach (var thisSection in this)
            {
                var name = thisSection.Name;
                if (!ReferenceEquals(name, null) && name.Length != 0)
                {
                    var otherSection = iniFile.GetOrCreateSection(name);
                    thisSection.ApplyTo(otherSection);
                }
            }
        }

        /// <summary>
        /// Clear the INI-file.
        /// </summary>
        public IniFile Clear()
        {
            _sections.Clear();

            return this;
        }

        /// <summary>
        /// Clear modification flag in all sections and lines.
        /// </summary>
        public void ClearModification()
        {
            Modified = false;

            foreach (var section in _sections)
            {
                section.Modified = false;
                foreach (var line in section)
                {
                    line.Modified = false;
                }
            }
        }

        /// <summary>
        /// Contains section with given name?
        /// </summary>
        public bool ContainsSection
            (
                string name
            )
        {
            CheckKeyName(name);

            foreach (var section in _sections)
            {
                if (section.Name.SameString(name))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create section with specified name.
        /// </summary>
        public Section CreateSection
            (
                string name
            )
        {
            CheckKeyName(name);

            if (ContainsSection(name))
            {
                throw new ArgumentException("duplicate name " + name);
            }

            var result = new Section(this, name);
            _sections.Add(result);

            return result;

        } // method CreateSection

        /// <summary>
        /// Get or create (if not exist) section with given name.
        /// </summary>
        public Section GetOrCreateSection
            (
                string name
            )
        {
            CheckKeyName(name);

            var result = GetSection(name)
                         ?? CreateSection(name);

            return result;
        }

        /// <summary>
        /// Get section with given name.
        /// </summary>
        public Section? GetSection
            (
                string name
            )
        {
            CheckKeyName(name);

            foreach (var section in _sections)
            {
                if (section.Name.SameString(name))
                {
                    return section;
                }
            }

            return null;

        } // method GetSection

        /// <summary>
        /// Get all the sections.
        /// </summary>
        public Section[] GetSections() => _sections.ToArray();

        /// <summary>
        /// Get value from the given section and key.
        /// </summary>
        public string? GetValue
            (
                string sectionName,
                string keyName,
                string? defaultValue
            )
        {
            var section = GetSection(sectionName);
            var result = ReferenceEquals(section, null)
                ? defaultValue
                : section.GetValue(keyName, defaultValue);

            return result;
        }

        /// <summary>
        /// Get value from the given section and key.
        /// </summary>
        public T? GetValue<T>
            (
                string sectionName,
                string keyName,
                T? defaultValue
            )
        {
            var section = GetSection(sectionName);
            var result = ReferenceEquals(section, null)
                ? defaultValue
                : section.GetValue(keyName, defaultValue);

            return result;

        } // method GetValue

        /// <summary>
        /// Merge the section.
        /// </summary>
        public void MergeSection
            (
                Section section
            )
        {
            var sectionName = section.Name;
            if (sectionName is null)
            {
                // TODO: слить с безымянной секцией
                return;
            }

            var found = GetSection(sectionName);
            if (ReferenceEquals(found, null))
            {
                _sections.Add(section);
            }
            else
            {
                foreach (var key in section.Keys)
                {
                    if (!found.ContainsKey(key))
                    {
                        found[key] = section[key];
                    }
                }
            }

        } // method MergeSection

        /// <summary>
        /// Remove specified section.
        /// </summary>
        public IniFile RemoveSection
            (
                string name
            )
        {
            CheckKeyName(name);

            foreach (var section in _sections)
            {
                if (section.Name.SameString(name))
                {
                    _sections.Remove(section);
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Remove specified value.
        /// </summary>
        public IniFile RemoveValue
            (
                string sectionName,
                string keyName
            )
        {
            var section = GetSection(sectionName);
            section?.Remove(keyName);

            return this;
        }

        /// <summary>
        /// Reread the <see cref="IniFile"/> from the file.
        /// </summary>
        public void Read()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                return;
            }

            var encoding = Encoding ?? Encoding.Default;

            Read(FileName.ThrowIfNull(nameof(FileName)), encoding);
        }

        /// <summary>
        /// Reread from the file.
        /// </summary>
        public void Read
            (
                string fileName,
                Encoding encoding
            )
        {
            using var reader = Private.OpenRead (fileName, encoding);
            Read(reader);
        }

        /// <summary>
        /// Reread from the stream.
        /// </summary>
        public void Read
            (
                TextReader reader
            )
        {
            char[] separators = { '=' };
            _sections.Clear();
            Section? section = null;

            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                if (line.StartsWith("["))
                {
                    if (!line.EndsWith("]"))
                    {
                        throw new FormatException();
                    }

                    var name = line.Substring(1, line.Length - 2);
                    section = CreateSection(name);
                }
                else
                {
                    if (section == null)
                    {
                        section = new Section(this, null);
                        _sections.Add(section);
                    }

                    var parts = line.Split(separators, 2);

                    var key = parts[0];
                    if (!string.IsNullOrEmpty(key))
                    {
                        var value = parts.Length == 2
                            ? parts[1]
                            : null;
                        section.SetValue(key, value);
                    }
                }
            }

            ClearModification();
        }

        /// <summary>
        /// Write INI-file into the stream.
        /// </summary>
        public void Save
            (
                TextWriter writer
            )
        {
            var first = true;
            foreach (var section in _sections)
            {
                if (!first)
                {
                    writer.WriteLine();
                }

                _SaveSection
                    (
                        writer,
                        section
                    );

                first = false;
            }

            Modified = false;

        } // method Save

        /// <summary>
        /// Save the INI-file to specified file.
        /// </summary>
        public void Save
            (
                string fileName
            )
        {
            var encoding = Encoding ?? Encoding.Default;

            using var writer = Private.CreateTextFile
                (
                    fileName,
                    encoding
                );
            Save(writer);

        } // method Save

        /// <summary>
        /// Set value for specified section and key.
        /// </summary>
        public IniFile SetValue
            (
                string sectionName,
                string keyName,
                string? value
            )
        {
            var section = GetOrCreateSection(sectionName);
            section.SetValue(keyName, value);

            return this;
        }

        /// <summary>
        /// Set value for specified section and key.
        /// </summary>
        public IniFile SetValue<T>
            (
                string sectionName,
                string keyName,
                T value
            )
        {
            var section = GetOrCreateSection(sectionName);
            section.SetValue(keyName, value);

            return this;
        }

        /// <summary>
        /// Write modified values to the stream.
        /// </summary>
        public void WriteModifiedValues
            (
                TextWriter writer
            )
        {
            var first = true;
            foreach (var section in _sections)
            {
                var lines = section
                    .Where(line => line.Modified)
                    .ToArray();

                if (lines.Length != 0)
                {
                    if (!first)
                    {
                        writer.WriteLine();
                    }

                    if (!string.IsNullOrEmpty(section.Name))
                    {
                        writer.WriteLine
                            (
                                "[{0}]",
                                section.Name
                            );
                    }

                    foreach (var line in lines)
                    {
                        line.Write(writer);
                    }

                    first = false;
                }
                else if (section.Modified)
                {
                    if (!first)
                    {
                        writer.WriteLine();
                    }
                    _SaveSection(writer, section);
                    first = false;
                }
            }
        }

        #endregion


        #region IEnumerable<Section> members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<Section> GetEnumerator() => _sections.GetEnumerator();

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose" />
        public void Dispose()
        {
            if (Writable
                && Modified
                && !string.IsNullOrEmpty(FileName))
            {
                Save(FileName.ThrowIfNull(nameof(FileName)));
            }
        }

        #endregion

    } // class IniFile

    /// <summary>
    /// Signature for Stat command.
    /// </summary>
    public sealed class StatDefinition
    {
        #region Nested classes

        /// <summary>
        /// Sort method.
        /// </summary>
        public enum SortMethod
        {
            /// <summary>
            /// Don't sort.
            /// </summary>
            None = 0,

            /// <summary>
            /// Ascending sort.
            /// </summary>
            Ascending = 1,

            /// <summary>
            /// Descending sort.
            /// </summary>
            Descending = 2
        }

        /// <summary>
        /// Stat item.
        /// </summary>
        public sealed class Item
        {
            #region Properties

            /// <summary>
            /// Field (possibly with subfield) specification.
            /// </summary>
            public string? Field { get; set; }

            /// <summary>
            /// Maximum length of the value (truncation).
            /// </summary>
            public int Length { get; set; }

            /// <summary>
            /// Count of items to take.
            /// </summary>
            public int Count { get; set; }

            /// <summary>
            /// How to sort result.
            /// </summary>
            public SortMethod Sort { get; set; }

            #endregion

            #region Object members

            /// <inheritdoc cref="object.ToString"/>
            public override string ToString() => $"{Field},{Length},{Count},{(int)Sort}";

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public string? DatabaseName { get; set; }

        /// <summary>
        /// Items.
        /// </summary>
        public List<Item> Items { get; } = new();

        /// <summary>
        /// Search query specification.
        /// </summary>
        public string? SearchQuery { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Optional query for sequential search.
        /// </summary>
        public string? SequentialQuery { get; set; }

        /// <summary>
        /// List of MFN.
        /// </summary>
        public List<int> MfnList { get; } = new();

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование в пользовательский запрос.
        /// </summary>
        public void Encode
            (
                SyncConnection connection,
                SyncQuery query
            )
        {
            // "2"               STAT
            // "IBIS"            database
            // "v200^a,10,100,1" field
            // "T=A$"            search
            // "0"               min
            // "0"               max
            // ""                sequential
            // ""                mfn list

            var items = string.Join(Constants.IrbisDelimiter, Items);
            var mfns = string.Join(",", MfnList);
            query.AddAnsi(connection.EnsureDatabase(DatabaseName));
            query.AddAnsi(items);
            query.AddUtf(SearchQuery);
            query.Add(MinMfn);
            query.Add(MaxMfn);
            query.AddUtf(SequentialQuery);

            // TODO: реализовать список MFN
            query.AddAnsi(mfns);

        } // method Encode

        #endregion

    } // class StatDefinition

    /// <summary>
    /// Параметры актуализации записи на ИРБИС-сервере.
    /// </summary>
    public sealed class ActualizeRecordParameters
    {
        #region Properties

        /// <summary>
        /// Имя базы данных (опционально).
        /// Если не указано, используется текущая база данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN актуализируемой записи (обязательно).
        /// 0 означает "актуализировать всю базу данных".
        /// </summary>
        public int Mfn { get; set; }

        #endregion

    } // class ActualizeRecordParameters

    /// <summary>
    /// Параметры создания базы данных на ИРБИС-сервере
    /// </summary>
    public sealed class CreateDatabaseParameters
    {
        #region Properties

        /// <summary>
        /// Имя создаваемой базы данных (обязательно).
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Описание создаваемой базы данных в произвольной форме
        /// (опционально).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Сделать базу данных видимой для АРМ "Читатель"?
        /// </summary>
        public bool ReaderAccess { get; set; }

        /// <summary>
        /// Имя базы данных-шаблона (опционально).
        /// </summary>
        public string? Template { get; set; }

        #endregion

    } // class CreateDatabaseParameters

    /// <summary>
    /// Виды сортировки ИРБИС-меню.
    /// </summary>
    public enum MenuSort
    {
        /// <summary>
        /// Без сортировки.
        /// </summary>
        None,

        /// <summary>
        /// Сортировка по кодам.
        /// </summary>
        ByCode,

        /// <summary>
        /// Сортировка по комментариям (значениям).
        /// </summary>
        ByComment

    } // enum MenuSort

    /// <summary>
    /// Пара строк в MNU-файле: код и соответствующее значение
    /// (либо комментарий).
    /// </summary>
    [DebuggerDisplay("{" + nameof(Code) + "} = {" + nameof(Comment) + "}")]
    public sealed class MenuEntry
    {
        #region Properties

        /// <summary>
        /// Первая строка - код.
        /// Коды могут повторяться в рамках одного MNU-файла.
        /// </summary>
        public string? Code { get; set; }

        /// <summary>
        /// Вторая строка - значение либо комментарий.
        /// Часто бывает пустой.
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Ссылка на другую пару строк, применяется при построении дерева
        /// (TRE-файла).
        /// </summary>
        public MenuEntry? OtherEntry { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => string.IsNullOrEmpty(Comment)
            ? Code.ToVisibleString()
            : $"{Code} - {Comment}";

        #endregion

    } // class MenuEntry

    /// <summary>
    /// Работа с MNU-файлами.
    /// </summary>
    public sealed class MenuFile
    {
        #region Constants

        /// <summary>
        /// End of menu marker.
        /// </summary>
        public const string StopMarker = "*****";

        #endregion

        #region Properties

        /// <summary>
        /// Name of menu file -- for identification
        /// purposes only.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        public List<MenuEntry> Entries { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuFile()
        {
            Entries = new List<MenuEntry>();
        } // constructor

        /// <summary>
        /// Internal constructor.
        /// </summary>
        internal MenuFile
            (
                List<MenuEntry> entries
            )
        {
            Entries = entries;
        } // constructor

        #endregion

        #region Private members

        /// <summary>
        /// Separators for the menu entries.
        /// </summary>
        public static readonly char[] MenuSeparators = { ' ', '-', '=', ':' };

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the specified code and comment.
        /// </summary>
        public MenuFile Add
            (
                string code,
                string? comment
            )
        {
            var entry = new MenuEntry
            {
                Code = code,
                Comment = comment
            };
            Entries.Add(entry);

            return this;
        } // method Add

        /// <summary>
        /// Trims the code.
        /// </summary>
        public static string TrimCode
            (
                string code
            )
        {
            code = code.Trim();
            var parts = code.Split(MenuSeparators);
            if (parts.Length != 0)
            {
                code = parts[0];
            }

            return code;
        } // method TrimCode

        /// <summary>
        /// Finds the entry.
        /// </summary>
        public MenuEntry? FindEntry (string code) =>
            Entries.FirstOrDefault (entry => entry.Code.SameString(code));

        /// <summary>
        /// Finds the entry (case sensitive).
        /// </summary>
        public MenuEntry? FindEntrySensitive (string code) =>
            Entries.FirstOrDefault(entry => string.CompareOrdinal (entry.Code, code) == 0);

        /// <summary>
        /// Finds the entry.
        /// </summary>
        public MenuEntry? GetEntry
            (
                string code
            )
        {
            var result = FindEntry(code);
            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            code = code.Trim();
            result = FindEntry(code);
            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            code = TrimCode(code);
            result = FindEntry(code);

            return result;
        } // method GetEntry

        /// <summary>
        /// Finds the entry (case sensitive).
        /// </summary>
        public MenuEntry? GetEntrySensitive
            (
                string code
            )
        {
            var result = FindEntrySensitive(code);
            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            code = code.Trim();
            result = FindEntrySensitive(code);
            if (!ReferenceEquals(result, null))
            {
                return result;
            }

            code = TrimCode(code);
            result = FindEntrySensitive(code);

            return result;
        } // method GetEntrySensitive

        /// <summary>
        /// Finds comment by the code.
        /// </summary>
        public string? GetString
            (
                string code,
                string? defaultValue = null
            )
        {
            var found = FindEntry(code);

            return ReferenceEquals(found, null)
                ? defaultValue
                : found.Comment;
        } // method GetString

        /// <summary>
        /// Finds comment by the code (case sensitive).
        /// </summary>
        public string? GetStringSensitive
            (
                string code,
                string? defaultValue = null
            )
        {
            var found = FindEntrySensitive(code);

            return ReferenceEquals(found, null)
                ? defaultValue
                : found.Comment;
        } // method GetStringSensitive

        /// <summary>
        /// Parses the specified stream.
        /// </summary>
        public static MenuFile ParseStream
            (
                TextReader reader
            )
        {
            var result = new MenuFile();

            while (true)
            {
                var code = reader.ReadLine();
                if (ReferenceEquals(code, null))
                {
                    break;
                }

                if (code.StartsWith(StopMarker))
                {
                    break;
                }

                var comment = reader.RequireLine();
                var entry = new MenuEntry
                {
                    Code = code,
                    Comment = comment
                };
                result.Entries.Add(entry);
            }

            return result;

        } // method ParseStream

        /// <summary>
        /// Parses the local file.
        /// </summary>
        public static MenuFile ParseLocalFile
            (
                string fileName,
                Encoding encoding
            )
        {
            using var reader = Private.OpenRead
                (
                    fileName,
                    encoding
                );
            var result = ParseStream(reader);
            result.FileName = Path.GetFileName(fileName);

            return result;
        } // method ParseLocalFile

        /// <summary>
        /// Parses the local file.
        /// </summary>
        public static MenuFile ParseLocalFile ( string fileName ) =>
            ParseLocalFile(fileName, Utility.Ansi);

        /// <summary>
        /// Parse server response.
        /// </summary>
        public static MenuFile ParseServerResponse
            (
                string response
            )
        {
            var reader = new StringReader(response);
            var result = ParseStream(reader);

            return result;
        } // method Parse

        /// <summary>
        /// Read <see cref="MenuFile"/> from server.
        /// </summary>
        public static MenuFile? ReadFromServer
            (
                SyncConnection connection,
                FileSpecification fileSpecification
            )
        {
            var response = connection.ReadTextFile(fileSpecification);
            if (ReferenceEquals(response, null))
            {
                return null;
            }

            var result = ParseServerResponse(response);

            return result;
        } // method ReadFromServer

        /// <summary>
        /// Sorts the entries.
        /// </summary>
        public MenuEntry[] SortEntries
            (
                MenuSort sortBy
            )
        {
            var copy = new List<MenuEntry>(Entries);
            switch (sortBy)
            {
                case MenuSort.None:
                    // Nothing to do
                    break;

                case MenuSort.ByCode:
                    copy = copy.OrderBy(entry => entry.Code).ToList();
                    break;

                case MenuSort.ByComment:
                    copy = copy.OrderBy(entry => entry.Comment).ToList();
                    break;

                default:
                    throw new IrbisException("Unexpected sortBy=" + sortBy);
            }

            return copy.ToArray();
        } // method SortEntries

        /// <summary>
        /// Builds text representation.
        /// </summary>
        public string ToText()
        {
            var result = new StringBuilder();

            foreach (var entry in Entries)
            {
                result.AppendLine(entry.Code);
                result.AppendLine(entry.Comment);
            }
            result.AppendLine(StopMarker);

            return result.ToString();
        } // method ToText

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => FileName.ToVisibleString();

        #endregion

    } // class MenuFile

    /// <summary>
    /// Коды АРМ ИРБИС64.
    /// Незнакомые серверу ИРБИС64 коды АРМ
    /// приводят к ошибке при регистрации на сервере.
    /// </summary>
    public enum Workstation
        : byte
    {
        /// <summary>
        /// Администратор.
        /// </summary>
        Administrator = (byte)'A',

        /// <summary>
        /// Каталогизатор.
        /// </summary>
        Cataloger = (byte)'C',

        /// <summary>
        /// Комплектатор.
        /// </summary>
        Acquisitions = (byte)'M',

        /// <summary>
        /// Читатель.
        /// </summary>
        Reader = (byte)'R',

        /// <summary>
        /// Книговыдача.
        /// </summary>
        Circulation = (byte)'B',

        /// <summary>
        /// Тоже книговыдача.
        /// </summary>
        Bookland = (byte)'B',

        /// <summary>
        /// Книгообеспеченность.
        /// </summary>
        Provision = (byte)'K',

        /// <summary>
        /// Java апплет.
        /// </summary>
        JavaApplet = (byte)'J',

        /// <summary>
        /// Не задан.
        /// </summary>
        None = 0

    } // enum Workstation

    /// <summary>
    /// Информация о зарегистрированном пользователе системы
    /// (по данным client_m.mnu).
    /// </summary>
    [DebuggerDisplay("{" + nameof(Name) + "}")]
    public sealed class UserInfo
    {
        #region Properties

        /// <summary>
        /// Номер по порядку.
        /// </summary>
        [Browsable(false)]
        public string? Number { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Доступность АРМ Каталогизатор.
        /// </summary>
        public string? Cataloger { get; set; }

        /// <summary>
        /// АРМ Читатель.
        /// </summary>
        public string? Reader { get; set; }

        /// <summary>
        /// АРМ Книговыдача.
        /// </summary>
        public string? Circulation { get; set; }

        /// <summary>
        /// АРМ Комплектатор.
        /// </summary>
        public string? Acquisitions { get; set; }

        /// <summary>
        /// АРМ Книгообеспеченность.
        /// </summary>
        public string? Provision { get; set; }

        /// <summary>
        /// АРМ Администратор.
        /// </summary>
        public string? Administrator { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Private members

        private static void _DecodePair
            (
                UserInfo user,
                MenuFile clientIni,
                char code,
                string? value
            )
        {
            value ??= GetStandardIni(clientIni, code);

            value = value.EmptyToNull();

            switch (code)
            {
                case 'C':
                    user.Cataloger = value;
                    break;

                case 'R':
                    user.Reader = value;
                    break;

                case 'B':
                    user.Circulation = value;
                    break;

                case 'M':
                    user.Acquisitions = value;
                    break;

                case 'K':
                    user.Provision = value;
                    break;

                case 'A':
                    user.Administrator = value;
                    break;
            }

        } // method _DecodePair

        private static void _DecodeLine
            (
                UserInfo user,
                MenuFile clientIni,
                string line
            )
        {
            var pairs = line.Split(Constants.Semicolon);
            var dictionary = new Dictionary<char, string>();
            foreach (var pair in pairs)
            {
                var parts = pair.Split(Constants.EqualSign, 2);
                if (parts.Length != 2 || parts[0].Length != 1)
                {
                    continue;
                }

                dictionary[char.ToUpper(parts[0][0])] = parts[1];
            }

            char[] codes = { 'C', 'R', 'B', 'M', 'K', 'A' };
            foreach (var code in codes)
            {
                dictionary.TryGetValue(code, out var value);
                _DecodePair(user, clientIni, code, value);
            }

        } // method _DecodeLine

        private string _FormatPair ( string prefix, string? value, string defaultValue ) =>
            value.SameString(defaultValue) ? string.Empty : $"{prefix}={value};";

        #endregion

        #region Public methods

        /// <summary>
        /// Encode.
        /// </summary>
        // ReSharper disable UseStringInterpolation
        public string Encode()
        {
            return string.Format
                (
                    "{0}\r\n{1}\r\n{2}{3}{4}{5}{6}{7}",
                    Name,
                    Password,
                    _FormatPair("C", Cataloger, "irbisc.ini"),
                    _FormatPair("R", Reader, "irbisr.ini"),
                    _FormatPair("B", Circulation, "irbisb.ini"),
                    _FormatPair("M", Acquisitions, "irbisp.ini"),
                    _FormatPair("K", Provision, "irbisk.ini"),
                    _FormatPair("A", Administrator, "irbisa.ini")
                );
        } // method Encode

        // ReSharper restore UseStringInterpolation

        /// <summary>
        /// Get standard INI-file name from client_ini.mnu
        /// for the workstation code.
        /// </summary>
        public static string GetStandardIni
            (
                MenuFile clientIni,
                char workstation
            )
        {
            var entries = (IList<MenuEntry?>) clientIni.Entries.ThrowIfNull(nameof(clientIni.Entries));
            var code = (Workstation) char.ToUpper(workstation);
            var result = code switch
            {
                Workstation.Cataloger => entries.SafeAt(0),
                Workstation.Reader => entries.SafeAt(1),
                Workstation.Circulation => entries.SafeAt(2),
                Workstation.Acquisitions => entries.SafeAt(3),
                Workstation.Provision => entries.SafeAt(4),
                Workstation.Administrator => entries.SafeAt(5),
                _ => throw new ArgumentOutOfRangeException()
            };

            return result.ThrowIfNull().Code.ThrowIfNull();

        } // method GetStandardIni

        /// <summary>
        /// Парсинг текстового представления.
        /// </summary>
        public static UserInfo[] Parse
            (
                string text
            )
        {
            var lines = text.SplitLines().Skip(2).ToArray();
            var result = new List<UserInfo>();
            while (true)
            {
                var current = lines.Take(9).ToArray();
                if (current.Length != 9)
                {
                    break;
                }

                var user = new UserInfo
                {
                    Number = current[0].EmptyToNull(),
                    Name = current[1].EmptyToNull(),
                    Password = current[2].EmptyToNull(),
                    Cataloger = current[3].EmptyToNull(),
                    Reader = current[4].EmptyToNull(),
                    Circulation = current[5].EmptyToNull(),
                    Acquisitions = current[6].EmptyToNull(),
                    Provision = current[7].EmptyToNull(),
                    Administrator = current[8].EmptyToNull()
                };
                result.Add(user);

                lines = lines.Skip(9).ToArray();
            }

            return result.ToArray();

        } // method Parse

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static UserInfo[] Parse
            (
                Response response
            )
        {
            var result = new List<UserInfo>();
            response.ReadAnsiStrings(2);
            while (true)
            {
                var lines = response.ReadAnsiStringsPlus(9);
                if (ReferenceEquals(lines, null))
                {
                    break;
                }

                var user = new UserInfo
                {
                    Number = lines[0].EmptyToNull(),
                    Name = lines[1].EmptyToNull(),
                    Password = lines[2].EmptyToNull(),
                    Cataloger = lines[3].EmptyToNull(),
                    Reader = lines[4].EmptyToNull(),
                    Circulation = lines[5].EmptyToNull(),
                    Acquisitions = lines[6].EmptyToNull(),
                    Provision = lines[7].EmptyToNull(),
                    Administrator = lines[8].EmptyToNull()
                };
                result.Add(user);
            }

            return result.ToArray();

        } // method Parse

        /// <summary>
        /// Parse the MNU-file from the stream.
        /// </summary>
        public static UserInfo[] ParseStream
            (
                TextReader reader,
                MenuFile clientIni
            )
        {
            var result = new List<UserInfo>();
            while (true)
            {
                var line1 = reader.ReadLine();
                if (ReferenceEquals(line1, null) || line1.StartsWith("***"))
                {
                    break;
                }

                var line2 = reader.ReadLine();
                var line3 = reader.ReadLine();
                if (ReferenceEquals(line2, null) || ReferenceEquals(line3, null))
                {
                    break;
                }

                var user = new UserInfo
                {
                    Name = line1,
                    Password = line2
                };
                _DecodeLine(user, clientIni, line3);
                result.Add(user);
            }

            return result.ToArray();

        } // method ParseStream

        /// <summary>
        /// Parse the MNU-file.
        /// </summary>
        public static UserInfo[] ParseFile
            (
                string fileName,
                MenuFile clientIni
            )
        {
            using var reader = Private.OpenRead(fileName, Utility.Ansi);

            return ParseStream(reader, clientIni);

        } // method ParseFile

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        // ReSharper disable UseStringInterpolation
        public override string ToString() => string.Format
            (
                "Number: {0}, Name: {1}, Password: {2}, "
                + "Cataloger: {3}, Reader: {4}, Circulation: {5}, "
                + "Acquisitions: {6}, Provision: {7}, "
                + "Administrator: {8}",
                Number.ToVisibleString(),
                Name.ToVisibleString(),
                Password.ToVisibleString(),
                Cataloger.ToVisibleString(),
                Reader.ToVisibleString(),
                Circulation.ToVisibleString(),
                Acquisitions.ToVisibleString(),
                Provision.ToVisibleString(),
                Administrator.ToVisibleString()
            );

        // ReSharper restore UseStringInterpolation

        #endregion

    } // class UserInfo

    /// <summary>
    /// Содержит 0, 1 или много экземпляров ссылочного типа.
    /// </summary>
    public readonly struct SomeValues<T>
        : IList<T>
        where T: class
    {
        #region Construction

        /// <summary>
        /// Конструктор: одно значение.
        /// </summary>
        public SomeValues
            (
                T value
            )
            : this()
        {
            _values = value;
        } // constructor

        /// <summary>
        /// Конструктор: не одно значение: либо 0 (в т. ч. <c>null</c>), либо много.
        /// </summary>
        public SomeValues
            (
                T[]? values
            )
            : this()
        {
            if (values is not null && values.Length == 1)
            {
                _values = values[0];
            }
            else
            {
                _values = values;
            }
        } // constructor

        #endregion

        #region Private members

        private readonly object? _values;

        #endregion

        #region Public methods

        /// <summary>
        /// Выдать значение как единственное.
        /// </summary>
        public T? AsSingle()
        {
            // Take local copy of _values so type checks remain
            // valid even if the StringValues is overwritten in memory
            var value = _values;

            if (value is T result1)
            {
                return result1;
            }

            if (value is null)
            {
                return null;
            }

            // Not T, not null, can only be T[]
            var result2 = Unsafe.As<T[]>(value)!;

            return result2.Length == 0 ? null : result2[0];

        } // method AsSingle

        /// <summary>
        /// Выдать значение как массив.
        /// </summary>
        public T[] AsArray()
        {
            // Take local copy of _values so type checks remain
            // valid even if the SomeValues is overwritten in memory
            var value = _values;

            if (value is T result1)
            {
                return new[] { result1 };
            }

            if (value is null)
            {
                return Array.Empty<T>();
            }

            // Not T, not null, can only be T[]
            return Unsafe.As<T[]>(value)!;

        } // method AsArray

        /// <summary>
        /// Контейнер пуст?
        /// </summary>
        public bool IsNullOrEmpty()
        {
            // Take local copy of _values so type checks remain
            // valid even if the SomeValues is overwritten in memory
            var value = _values;

            if (value is T)
            {
                return false;
            }

            if (value is null)
            {
                return true;
            }

            // Not T, not null, can only be T[]
            var array = Unsafe.As<T[]>(value)!;

            return array.Length == 0;

        } // method IsNullOrEmpty

        /// <summary>
        /// Оператор неявного преобразования.
        /// </summary>
        public static implicit operator SomeValues<T> (T value)
            => new(value);

        /// <summary>
        /// Оператор неявного преобразования.
        /// </summary>
        public static implicit operator SomeValues<T> (T[] values)
            => new(values);

        /// <summary>
        /// Оператор неявного преобразования.
        /// </summary>
        public static implicit operator T? (SomeValues<T> values)
            => values.AsSingle();

        /// <summary>
        /// Оператор неявного преобразования.
        /// </summary>
        public static implicit operator T[](SomeValues<T> values)
            => values.AsArray();

        #endregion

        #region IList<T> members

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator()
        {
            // Take local copy of _values so type checks remain
            // valid even if the SomeValues is overwritten in memory
            var value = _values;

            if (value is T result1)
            {
                yield return result1;
            }
            else if (value is not null)
            {
                // Not T, not null, can only be T[]
                var array = Unsafe.As<T[]>(value)!;

                foreach (var one in array)
                {
                    yield return one;
                }
            }
        } // method GetEnumerator

        /// <summary>
        /// Not implemented.
        /// </summary>
        public void Add(T item) => throw new NotImplementedException();

        /// <summary>
        /// Not implemented.
        /// </summary>
        public void Clear() => throw new NotImplementedException();

        /// <inheritdoc cref="ICollection{T}.Contains"/>
        public bool Contains(T item)
        {
            // Take local copy of _values so type checks remain
            // valid even if the SomeValues is overwritten in memory
            var value = _values;
            var comparer = EqualityComparer<T>.Default;

            if (value is T one)
            {
                return comparer.Equals(item, one);
            }

            if (value is null)
            {
                return false;
            }

            // Not T, not null, can only be T[]
            var array = Unsafe.As<T[]>(value)!;
            foreach (var other in array)
            {
                if (comparer.Equals(item, other))
                {
                    return true;
                }
            }

            return false;
        } // method Contains

        /// <inheritdoc cref="ICollection{T}.CopyTo"/>
        public void CopyTo
            (
                T[] array,
                int arrayIndex
            )
        {
            // Take local copy of _values so type checks remain
            // valid even if the SomeValues is overwritten in memory
            var value = _values;

            if (value is T one)
            {
                array[arrayIndex] = one;
            }
            else if (value is not null)
            {
                // Not T, not null, can only be T[]
                var many = Unsafe.As<T[]>(value)!;
                Array.Copy(many, 0, array, arrayIndex, many.Length);
            }
        } // method CopyTo

        /// <summary>
        /// Not implemented.
        /// </summary>
        public bool Remove(T item) => throw new NotImplementedException();

        /// <summary>
        /// <inheritdoc cref="ICollection{T}.Count"/>
        /// </summary>
        public int Count
        {
            get
            {
                // Take local copy of _values so type checks remain
                // valid even if the SomeValues is overwritten in memory
                var value = _values;

                if (value is T)
                {
                    return 1;
                }

                if (value is null)
                {
                    return 0;
                }

                // Not T, not null, can only be T[]
                return Unsafe.As<T[]>(value)!.Length;
            }
        }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
        public bool IsReadOnly => true;

        /// <inheritdoc cref="IList{T}.IndexOf"/>
        public int IndexOf
            (
                T item
            )
        {
            // Take local copy of _values so type checks remain
            // valid even if the SomeValues is overwritten in memory
            var value = _values;
            var comparer = EqualityComparer<T>.Default;

            if (value is T one)
            {
                return comparer.Equals(item, one) ? 0 : -1;
            }

            if (value is null)
            {
                return -1;
            }

            // Not T, not null, can only be T[]
            var array = Unsafe.As<T[]>(value)!;
            for (var index = 0; index < array.Length; index++)
            {
                if (comparer.Equals(item, array[index]))
                {
                    return index;
                }
            }

            return -1;

        } // method IndexOf

        /// <summary>
        /// Not implemented.
        /// </summary>
        public void Insert(int index, T item) => throw new NotImplementedException();

        /// <summary>
        /// Not implemented
        /// </summary>
        public void RemoveAt(int index) => throw new NotImplementedException();

        /// <inheritdoc cref="IList{T}.this"/>
        public T this[int index]
        {
            get
            {
                // Take local copy of _values so type checks remain
                // valid even if the SomeValues is overwritten in memory
                var value = _values;

                if (value is T value1)
                {
                    return index == 0
                        ? value1
                        : throw new IndexOutOfRangeException();
                }

                if (value is null)
                {
                    throw new IndexOutOfRangeException();
                }

                // Not T, not null, can only be T[]
                return Unsafe.As<T[]>(value)! [index];
            }
            set => throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
            => AsSingle()?.ToString() ?? string.Empty;

        #endregion

    } // struct SomeValues

    /// <summary>
    /// Параметры форматирования записи на ИРБИС-сервере.
    /// </summary>
    public sealed class FormatRecordParameters
    {
        #region Properties

        /// <summary>
        /// Сюда помещается результат.
        /// </summary>
        public SomeValues<string> Result { get; set; }

        /// <summary>
        /// Имя базы данных (опционально).
        /// Если не указано, используется текущая база данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Спецификация формата (обязательно).
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// MFN записи, подлежащей форматированию.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// MFN записей, подлежащих форматированию.
        /// </summary>
        public int[]? Mfns { get; set; }

        /// <summary>
        /// Запись, подлежащая форматированию.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Записи подлежащие форматированию.
        /// </summary>
        public Record[]? Records { get; set; }

        #endregion

    } // class FormatRecordParameters

    /// <summary>
    /// Один результат полнотекстового поиска.
    /// </summary>
    public sealed class FoundPages
    {
        #region Properties

        /// <summary>
        /// MFN найденной записи.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Номера страниц найденной записи.
        /// </summary>
        public int[]? Pages { get; set; }

        /// <summary>
        /// Результат расформатирования.
        /// </summary>
        public string? Formatted { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор текстовой строки.
        /// </summary>
        public void Decode
            (
                string line
            )
        {
            var parts = line.Split(Constants.NumberSign, 3);
            var pages = new List<int>();
            Mfn = int.Parse(parts[0]);
            if (parts.Length == 3)
            {
                Formatted = parts[2];
            }

            if (parts.Length > 1)
            {
                parts = parts[1].Split('\x1F');
                foreach (var part in parts)
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        var page = int.Parse(part);
                        pages.Add(page);
                    }
                }
            }

            Pages = pages.ToArray();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static FoundPages[] Decode
            (
                Response response
            )
        {
            // response.Debug(Console.Out);
            // response.DebugUtf(Console.Out);

            var number = response.ReadInteger(); // количество найденных записей
            var result = new List<FoundPages>(number);
            for (var i = 0; i < number; i++)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var one = new FoundPages();
                one.Decode(line);
                result.Add(one);
            }

            return result.ToArray();
        }

        #endregion

    } // class FoundPages

    /// <summary>
    /// Один найденный фасет в результатах поиска.
    /// </summary>
    public sealed class FoundFacet
    {
        #region Properties

        /// <summary>
        /// Число записей с данным термином.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Термин с префиксом.
        /// </summary>
        public string? Term { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор строки ответа сервера.
        /// </summary>
        /// <param name="line">Строка из ответа сервера.</param>
        public void Decode
            (
                ReadOnlySpan<char> line
            )
        {
            throw new NotImplementedException();

        } // method Decode

        #endregion

    } // class FoundFacet

    /// <summary>
    /// Результат полнотекстового поиска.
    /// </summary>
    public sealed class FullTextResult
    {
        #region Properties

        /// <summary>
        /// Найденные страницы
        /// </summary>
        public FoundPages[]? Pages { get; set; }

        /// <summary>
        /// Фасеты.
        /// </summary>
        public FoundFacet[]? Facets { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public void Decode
        (
            Response response
        )
        {
            Pages = FoundPages.Decode(response);
        }

        #endregion

    } // class FullTextResult

    /// <summary>
    /// Параметры поискового запроса.
    /// </summary>
    public sealed class SearchParameters
    {
        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Смещение первой записи, которую необходимо вернуть.
        /// Нумерация с 1.
        /// По умолчанию 1.
        /// </summary>
        public int FirstRecord { get; set; } = 1;

        /// <summary>
        /// Опциональная спецификация формата.
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// Максимальный MFN.
        /// По умолчанию 0 - без ограничения по MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Минимальный MFN.
        /// По умолчанию 0 - без ограничения по MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Количество записей, которые необходимо вернуть.
        /// По умолчанию 0 - максимально возможное
        /// (ограничение текущей реализации MAX_PACKET).
        /// </summary>
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Выражение для поиска по словарю.
        /// </summary>
        public string? Expression { get; set; }

        /// <summary>
        /// Опциональное выражение для последовательного поиска.
        /// </summary>
        public string? Sequential { get; set; }

        /// <summary>
        /// Опциональная спецификация для фильтрации на клиенте.
        /// </summary>
        public string? Filter { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование параметров поиска.
        /// </summary>
        public SearchParameters Clone()
        {
            return (SearchParameters) MemberwiseClone();
        }

        /// <summary>
        /// Кодирование параметров поиска для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode
            (
                SyncConnection connection,
                SyncQuery query
            )
        {
            var database = Database.ThrowIfNull(nameof(Database));

            query.AddAnsi(database);
            query.AddUtf(Expression);
            query.Add(NumberOfRecords);
            query.Add(FirstRecord);
            query.AddFormat(Format);
            query.Add(MinMfn);
            query.Add(MaxMfn);
            query.AddAnsi(Sequential);

        } // method Encode

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() =>
            (Expression ?? Sequential).ToVisibleString();

        #endregion

    } // class SearchParameters

    /// <summary>
    /// Параметры полнотекстового поиска для ИРБИС64+.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Request) + "}")]
    public sealed class TextParameters
    {
        #region Constants

        /// <summary>
        /// Разделитель между элементами запроса.
        /// </summary>
        public const char Delimiter = '\x1F';

        #endregion

        #region Properties

        /// <summary>
        /// Поисковый запрос на естественном языке.
        /// </summary>
        public string? Request { get; set; }

        /// <summary>
        /// Искать эти слова в найденном по Request.
        /// </summary>
        public string[]? Words { get; set; }

        /// <summary>
        /// Использовать морфологию? По умолчанию используется стемминг
        /// и поиск по совпадению с стеммированным словом.
        /// </summary>
        public bool Morphology { get; set; }

        /// <summary>
        /// Префикс в словаре. По умолчанию "KT=".
        /// </summary>
        public string Prefix { get; set; } = "KT=";

        /// <summary>
        /// Учитывать максимальное расстояние между словами.
        /// По умолчанию = -1, что означает "не учитывать".
        /// </summary>
        public int MaxDistanse { get; set; } = -1;

        /// <summary>
        /// Тематические индексы через запятую.
        /// </summary>
        public string? Context { get; set; }

        /// <summary>
        /// Максимальное число ответов.
        /// По умолчанию 100.
        /// </summary>
        public int MaxCount { get; set; } = 100;

        /// <summary>
        /// Тип фасета (префикс).
        /// По умолчанию отсутсвует.
        /// </summary>
        public string? CellType { get; set; }

        /// <summary>
        /// Глубина выдачи.
        /// По умолчанию 5 верхних фасетов.
        /// </summary>
        public int CellCount { get; set; } = 5;

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование параметров в строку.
        /// </summary>
        public string Encode()
        {
            var builder = new StringBuilder();
            builder.Append(Request);
            builder.Append(Delimiter);
            builder.Append
                (
                    Words is not null
                        ? string.Join(" ", Words)
                        : string.Empty
                );
            builder.Append(Delimiter);
            builder.Append(Morphology ? "1" : "0");
            builder.Append(Delimiter);
            builder.Append(Prefix);
            builder.Append(Delimiter);
            builder.Append(MaxDistanse.ToInvariantString());
            builder.Append(Delimiter);
            builder.Append(Context);
            builder.Append(Delimiter);
            builder.Append(MaxCount.ToInvariantString());
            builder.Append(Delimiter);
            builder.Append(CellType);
            builder.Append(Delimiter);
            builder.Append(CellCount.ToInvariantString());

            return builder.ToString();

        } // method Encode

        /// <summary>
        /// Кодирование в пользовательский запрос.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="query">Пользовательский запрос.</param>
        public void Encode
            (
                SyncConnection connection,
                SyncQuery query
            )
        {
            query.AddUtf(Encode());

        } // method Encode

        #endregion

    } // class TextParameters

    /// <summary>
    /// Одна строка в ответе сервера на поисковый запрос.
    /// </summary>
    [DebuggerDisplay("{Mfn} {Text}")]
    public sealed class FoundItem
        : IEquatable<FoundItem>
    {
        #region Properties

        /// <summary>
        /// Текстовая часть (может отсутствовать,
        /// если не запрашивалось форматирование).
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// MFN найденной записи.
        /// </summary>
        public int Mfn { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Выбирает только MFN из найденных записей.
        /// </summary>
        public static int[] ToMfn
            (
                FoundItem[]? found
            )
        {
            if (found is null || found.Length == 0)
            {
                return Array.Empty<int>();
            }

            var result = new int[found.Length];
            for (var i = 0; i < found.Length; i++)
            {
                result[i] = found[i].Mfn;
            }

            return result;
        } // method ToMfn

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static FoundItem[] Parse
            (
                Response response
            )
        {
            var expected = response.ReadInteger();
            var result = new List<FoundItem>(expected);
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split(Constants.NumberSign, 2);
                var item = new FoundItem
                {
                    Mfn = Private.ParseInt32(parts[0]),
                    Text = parts.Length == 2 ? parts[1] : string.Empty
                };
                result.Add(item);
            }

            return result.ToArray();

        } // method Parse

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static int[] ParseMfn
            (
                Response response
            )
        {
            var expected = response.ReadInteger();
            var result = new List<int>(expected);
            while (!response.EOT)
            {
                var line = response.ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split(Constants.NumberSign, 2);
                var mfn = int.Parse(parts[0]);
                result.Add(mfn);
            }

            return result.ToArray();

        } // method ParseMfn

        /// <summary>
        /// Загружаем найденные записи с сервера.
        /// </summary>
        public static FoundItem[] Read
            (
                SyncConnection connection,
                string format,
                IEnumerable<int> found
            )
        {
            var array = found.ToArray();
            var length = array.Length;
            var result = new FoundItem[length];
            var formatted = connection.FormatRecords(array, format);
            if (formatted is null)
            {
                return Array.Empty<FoundItem>();
            }

            for (var i = 0; i < length; i++)
            {
                var item = new FoundItem
                {
                    Mfn = array[i],
                    Text = formatted[i]
                };
                result[i] = item;
            }

            return result;

        } // method Read

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals(FoundItem? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Text == other.Text && Mfn == other.Mfn;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object)"/>
        public override bool Equals(object? obj) =>
            ReferenceEquals(this, obj) || obj is FoundItem other && Equals(other);

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode() =>
            unchecked(((Text != null ? Text.GetHashCode() : 0) * 397) ^ Mfn);

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"[{Mfn}] {Text.ToVisibleString()}";

        #endregion

    } // class FoundItem

    /// <summary>
    /// Параметры чтения записи с ИРБИС-сервера.
    /// </summary>
    public sealed class ReadRecordParameters
    {
        #region Properties

        /// <summary>
        /// Результат помещается сюда.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Имя базы данных (опционально).
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN записи (обязательно).
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Оставить запись заблокированной?
        /// </summary>
        public bool Lock { get; set; }

        /// <summary>
        /// Номер версии (опционально).
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Формат (опционально).
        /// </summary>
        public string? Format { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"MFN={Mfn}";

        #endregion

    } // class ReadRecordParameters

    /// <summary>
    /// Термин в поисковом словаре.
    /// </summary>
    [DebuggerDisplay("{Count} {Text}")]
    public class Term
        : IEquatable<Term>
    {
        #region Properties

        /// <summary>
        /// Количество ссылок.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Поисковый термин.
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the <see cref="Term"/>.
        /// </summary>
        public Term Clone() => (Term) MemberwiseClone();

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static Term[] Parse
            (
                Response response
            )
        {
            var result = new List<Term>();
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split(Constants.NumberSign, 2);
                var item = new Term
                {
                    Count = int.Parse(parts[0]),
                    Text = parts.Length == 2 ? parts[1] : string.Empty
                };
                result.Add(item);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Удаляет префиксы с терминов.
        /// </summary>
        public static Term[] TrimPrefix
            (
                ICollection<Term> terms,
                string prefix
            )
        {
            var prefixLength = prefix.Length;
            var result = new List<Term>(terms.Count);
            if (prefixLength == 0)
            {
                foreach (var term in terms)
                {
                    result.Add(term.Clone());
                }
            }
            else
            {
                foreach (var term in terms)
                {
                    var item = term.Text;
                    if (!ReferenceEquals(item, null) && item.StartsWith(prefix))
                    {
                        item = item.Substring(prefixLength);
                    }
                    var clone = term.Clone();
                    clone.Text = item;
                    result.Add(clone);
                }
            }

            return result.ToArray();

        } // method TrimPrefix

        #endregion

        #region IEquatable<T> members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
        public bool Equals(Term? other)
            => Text?.Equals(other?.Text) ?? false;

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString"/>
        public override string ToString() => $"{Count}#{Text.ToVisibleString()}";

        #endregion

    } // class Term

    /// <summary>
    /// Параметры запроса терминов.
    /// </summary>
    public sealed class TermParameters
    {
        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Количество терминов, которое необходимо вернуть.
        /// По умолчанию 0 - максимально возможное.
        /// Ограничение текущей реализации MAX_PACKET.
        /// </summary>
        public int NumberOfTerms { get; set; }

        /// <summary>
        /// Термины в обратном порядке?
        /// </summary>
        public bool ReverseOrder { get; set; }

        /// <summary>
        /// Стартовый термин.
        /// </summary>
        public string? StartTerm { get; set; }

        /// <summary>
        /// Опциональная спецификация формата.
        /// </summary>
        public string? Format { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование параметров.
        /// </summary>
        public TermParameters Clone()
        {
            return (TermParameters) MemberwiseClone();
        }

        /// <summary>
        /// Кодирование параметров постингов для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode
            (
                SyncConnection connection,
                SyncQuery query
            )
        {
            var database = Database.ThrowIfNull(nameof(Database));

            query.AddAnsi(database);
            query.AddUtf(StartTerm);
            query.Add(NumberOfTerms);
            query.AddFormat(Format);

        } // method Encode

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => StartTerm.ToVisibleString();

        #endregion

    } // class TermParameters

    /// <summary>
    /// Информация о процессе на сервере ИРБИС64.
    /// </summary>
    public sealed class ProcessInfo
        : IEquatable<ProcessInfo>
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? IpAddress { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Workstation { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Started { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? LastCommand { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? CommandNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? ProcessId { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? State { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static ProcessInfo[] Parse
            (
                Response response
            )
        {
            var lines = response.ReadRemainingAnsiLines();
            var processCount = int.Parse(lines[0]);
            var linesPerProcess = int.Parse(lines[1]);
            var result = new List<ProcessInfo>(processCount);
            if (processCount == 0 || linesPerProcess == 0)
            {
                return result.ToArray();
            }

            for (int i = 2; i < lines.Length; i += linesPerProcess + 1)
            {
                if ((i + linesPerProcess) > lines.Length)
                {
                    break;
                }

                var process = new ProcessInfo
                {
                    Number = lines[i + 0],
                    IpAddress = lines[i + 1],
                    Name = lines[i + 2],
                    ClientId = lines[i + 3],
                    Workstation = lines[i + 4],
                    Started = lines[i + 5],
                    LastCommand = lines[i + 6],
                    CommandNumber = lines[i + 7],
                    ProcessId = lines[i + 8],
                    State = lines[i + 9]
                };
                result.Add(process);
            }

            return result.ToArray();

        } // method Parse

        #endregion

        #region IEquatable members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(ProcessInfo? other) => Number?.Equals(other?.Number) ?? false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => $"{Number} {IpAddress} {Name} {Workstation}";

        #endregion

    } // class ProcessInfo

    /// <summary>
    /// Постинг термина.
    /// </summary>
    [DebuggerDisplay("[{Mfn}] {Tag} {Occurrence} {Count} {Text}")]
    public sealed class TermPosting
        : IEquatable<TermPosting>
    {
        #region Properties

        /// <summary>
        /// MFN записи с искомым термом.
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Тег поля с искомым термом.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        public int Occurrence { get; set; }

        /// <summary>
        /// Количество повторений.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Результат форматирования.
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the <see cref="TermPosting"/>.
        /// </summary>
        public TermPosting Clone()
        {
            return (TermPosting) MemberwiseClone();
        }

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static TermPosting[] Parse
            (
                Response response
            )
        {
            var result = new List<TermPosting>();
            while (!response.EOT)
            {
                var line = response.ReadUtf();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var parts = line.Split(Constants.NumberSign, 5);
                if (parts.Length < 4)
                {
                    break;
                }

                var item = new TermPosting
                {
                    Mfn = int.Parse(parts[0]),
                    Tag = int.Parse(parts[1]),
                    Occurrence = int.Parse(parts[2]),
                    Count = int.Parse(parts[3]),
                    Text = parts.Length == 5 ? parts[4] : string.Empty
                };
                result.Add(item);
            }

            return result.ToArray();
        }

        #endregion

        #region IEquatable members

        /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
        public bool Equals(TermPosting? other)
        {
            if (other is not null)
            {
                return Mfn == other.Mfn
                       && Tag == other.Tag
                       && Occurrence == other.Occurrence
                       && Count == other.Count
                       && Text == other.Text;
            }

            return false;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString() => $"{Mfn}#{Tag}#{Occurrence}#{Count}#{Text}";

        #endregion

    } // class TermPosting

    /// <summary>
    /// Параметры запроса постингов.
    /// </summary>
    public sealed class PostingParameters
    {
        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Номер первого постинга, который необходимо вернуть.
        /// Нумерация с 1.
        /// По умолчанию 1.
        /// </summary>
        public int FirstPosting { get; set; } = 1;

        /// <summary>
        /// Опциональный формат.
        /// </summary>
        public string? Format { get; set; }

        /// <summary>
        /// Количество постингов, которые необходимо вернуть.
        /// По умолчанию 0 - все.
        /// </summary>
        public int NumberOfPostings { get; set; }

        /// <summary>
        /// Массив терминов, для которых нужны постинги.
        /// </summary>
        public string[]? Terms { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone the parameters.
        /// </summary>
        public PostingParameters Clone()
        {
            var result = (PostingParameters) MemberwiseClone();
            if (Terms is not null)
            {
                result.Terms = (string[]?) Terms.Clone();
            }

            return result;

        } // method Clone

        /// <summary>
        /// Кодирование параметров постингов для клиентского запроса.
        /// </summary>
        /// <param name="connection">Ссылка на подключение к серверу.</param>
        /// <param name="query">Клиентский запрос.</param>
        public void Encode
            (
                SyncConnection connection,
                SyncQuery query
            )
        {
            var database = connection.EnsureDatabase(Database);
            query.AddAnsi(database);
            query.Add(NumberOfPostings);
            query.Add(FirstPosting);
            query.AddFormat(Format);

            foreach (var term in Terms.ThrowIfNull())
            {
                query.AddUtf(term);
            }

        } // method Encode

        #endregion

    } // class PostingParameters

    /// <summary>
    /// Signature for Table command.
    /// </summary>
    public sealed class TableDefinition
    {
        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public string? DatabaseName { get; set; }

        /// <summary>
        /// Table name.
        /// </summary>
        public string? Table { get; set; }

        /// <summary>
        /// Table headers.
        /// </summary>
        public List<string> Headers { get; } = new ();

        /// <summary>
        /// Mode.
        /// </summary>
        public string? Mode { get; set; }

        /// <summary>
        /// Search query.
        /// </summary>
        public string? SearchQuery { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Optional sequential query.
        /// </summary>
        public string? SequentialQuery { get; set; }

        /// <summary>
        /// List of MFN.
        /// </summary>
        public List<int> MfnList { get; } = new ();

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование запроса.
        /// </summary>
        public void Encode
            (
                SyncQuery query
            )
        {
            query.AddAnsi(Table);
            query.NewLine(); // вместо заголовков
            query.AddAnsi(Mode);
            query.AddUtf(SearchQuery);
            query.Add(MinMfn);
            query.Add(MaxMfn);
            query.AddUtf(SequentialQuery);
            query.NewLine(); // вместо списка MFN

        } // method Encode

        #endregion

    } // class TableDefinition

    /// <summary>
    /// Строка в протоколе выполнения глобальной корректировки.
    /// </summary>
    public sealed class GblProtocolLine
    {
        #region Properties

        /// <summary>
        /// Общий признак успеха.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN записи
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Результат Autoin.gbl
        /// </summary>
        public string? Autoin { get; set; }

        /// <summary>
        /// UPDATE=
        /// </summary>
        public string? Update { get; set; }

        /// <summary>
        /// STATUS=
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Код ошибки, если есть
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// UPDUF=
        /// </summary>
        public string? UpdUf { get; set; }

        /// <summary>
        /// Исходный текст (до парсинга)
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse one text line.
        /// </summary>
        public void Decode
            (
                string line
            )
        {
            Text = line;
            Success = true;
            var parts = line.Split('#');
            foreach (var part in parts)
            {
                var p = part.Split('=');
                if (p.Length > 0)
                {
                    var name = p[0].ToUpper();
                    var value = string.Empty;
                    if (p.Length > 1)
                    {
                        value = p[1];
                    }

                    switch (name)
                    {
                        case "DBN":
                            Database = value;
                            break;

                        case "MFN":
                            Mfn = value.SafeToInt32();
                            break;

                        case "AUTOIN":
                            Autoin = value;
                            break;

                        case "UPDATE":
                            Update = value;
                            break;

                        case "STATUS":
                            Status = value;
                            break;

                        case "UPDUF":
                            UpdUf = value;
                            break;

                        case "GBL_ERROR":
                            Error = value;
                            Success = false;
                            break;
                    } // switch
                } // if
            } // foreach
        } // method Decode

        /// <summary>
        /// Parse server response.
        /// </summary>
        public static GblProtocolLine[] Decode
            (
                Response response
            )
        {
            var result = new List<GblProtocolLine>();

            while (true)
            {
                var line = response.ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var item = new GblProtocolLine();
                item.Decode(line);
                result.Add(item);
            } // while

            return result.ToArray();
        } // method Decode

        #endregion

        #region Object members


        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Text.ToVisibleString();
        } // method ToString

        #endregion

    } // class GblProtocolLine

    /// <summary>
    /// Результат исполнения глобальной корректировки.
    /// </summary>
    public sealed class GblResult
    {
        #region Properties

        /// <summary>
        /// Момент начала обработки.
        /// </summary>
        public DateTime TimeStarted { get; set; }

        /// <summary>
        /// Всего времени затрачено (с момента начала обработки).
        /// </summary>
        public TimeSpan TimeElapsed { get; set; }

        /// <summary>
        /// Отменено пользователем.
        /// </summary>
        public bool Canceled { get; set; }

        /// <summary>
        /// Исключение (если возникло).
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// Предполагалось обработать записей.
        /// </summary>
        public int RecordsSupposed { get; set; }

        /// <summary>
        /// Обработано записей.
        /// </summary>
        public int RecordsProcessed { get; set; }

        /// <summary>
        /// Успешно обработано записей.
        /// </summary>
        public int RecordsSucceeded { get; set; }

        /// <summary>
        /// Ошибок при обработке записей.
        /// </summary>
        public int RecordsFailed { get; set; }

        /// <summary>
        /// Результаты для каждой записи.
        /// </summary>
        public GblProtocolLine[]? Protocol { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Get empty result.
        /// </summary>
        public static GblResult GetEmptyResult()
        {
            var result = new GblResult
            {
                TimeStarted = DateTime.Now,
                TimeElapsed = new TimeSpan(0)
            };

            return result;
        } // method GetEmptyResult

        /// <summary>
        /// Merge result.
        /// </summary>
        public void MergeResult
            (
                GblResult intermediateResult
            )
        {
            if (intermediateResult.Canceled)
            {
                Canceled = intermediateResult.Canceled;
            }

            if (!ReferenceEquals(intermediateResult.Exception, null))
            {
                Exception = intermediateResult.Exception;
            }

            RecordsProcessed += intermediateResult.RecordsProcessed;
            RecordsFailed += intermediateResult.RecordsFailed;
            RecordsSucceeded += intermediateResult.RecordsSucceeded;
            Protocol ??= Array.Empty<GblProtocolLine>();
            var otherLines
                = intermediateResult.Protocol ?? Array.Empty<GblProtocolLine>();
            Protocol = Private.Merge (Protocol, otherLines);
        } // method MergeResult

        /// <summary>
        /// Parse server response.
        /// </summary>
        public void Parse
            (
                Response response
            )
        {
            Protocol = GblProtocolLine.Decode(response);
            RecordsProcessed = Protocol.Length;
            RecordsSucceeded = Protocol.Count(line => line.Success);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return $"Records processed: {RecordsProcessed}, Canceled: {Canceled}";
        } // method ToString

        #endregion

    } // class GblResult

    /// <summary>
    /// Параметры сохранения записи/записей на ИРБИС-сервере.
    /// </summary>
    public sealed class WriteRecordParameters
    {
        #region Properties

        /// <summary>
        /// Запись (обязательно, если записываем одну).
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Много записей (обязательно, если записываем несколько).
        /// </summary>
        public Record[]? Records { get; set; }

        /// <summary>
        /// Оставить запись заблокированной?
        /// </summary>
        public bool Lock { get; set; }

        /// <summary>
        /// Актуализировать поисковый индекс?
        /// </summary>
        public bool Actualize { get; set; }

        /// <summary>
        /// Новое значение MaxMfn (устанавливает сервер).
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Не разбирать ответ сервера.
        /// </summary>
        public bool DontParse { get; set; }

        #endregion

    } // class WriteRecordParameters

    /// <summary>
    /// Оператор глобальной корректировки со всеми относящимися
    /// к нему данными.
    /// </summary>
    [DebuggerDisplay("{Command} {Parameter1} {Parameter2}")]
    public sealed class GblStatement
    {
        #region Constants

        /// <summary>
        /// Разделитель элементов
        /// </summary>
        public const string Delimiter = "\x1F\x1E";

        #endregion

        #region Properties

        /// <summary>
        /// Команда (оператор), например, ADD или DEL.
        /// </summary>
        public string? Command { get; set; }

        /// <summary>
        /// Первый параметр, как правило, спецификация поля/подполя.
        /// </summary>
        public string? Parameter1 { get; set; }

        /// <summary>
        /// Второй параметр, как правило, спецификация повторения.
        /// </summary>
        public string? Parameter2 { get; set; }

        /// <summary>
        /// Первый формат, например, выражение для замены.
        /// </summary>
        public string? Format1 { get; set; }

        /// <summary>
        /// Второй формат, например, заменяющее выражение.
        /// </summary>
        public string? Format2 { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode for protocol.
        /// </summary>
        public string EncodeForProtocol()
        {
            var result = new StringBuilder();

            result.Append(Command);
            result.Append(Delimiter);
            result.Append(Parameter1);
            result.Append(Delimiter);
            result.Append(Parameter2);
            result.Append(Delimiter);
            result.Append(Format1);
            result.Append(Delimiter);
            result.Append(Format2);
            result.Append(Delimiter);

            return result.ToString();

        } // method EncodeForProtocol

        /// <summary>
        /// Parse the stream.
        /// </summary>
        public static GblStatement? ParseStream
            (
                TextReader reader
            )
        {
            var command = reader.ReadLine();
            if (ReferenceEquals(command, null) || command.Length == 0)
            {
                return null;
            }

            var result = new GblStatement
            {
                Command = command.Trim(),
                Parameter1 = reader.RequireLine(),
                Parameter2 = reader.RequireLine(),
                Format1 = reader.RequireLine(),
                Format2 = reader.RequireLine()
            };

            return result;

        } // method ParseStream

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Command: {0},{5}"
                    + "Parameter1: {1},{5}"
                    + "Parameter2: {2},{5}"
                    + "Format1: {3},{5}"
                    + "Format2: {4}",
                    Command,
                    Parameter1,
                    Parameter2,
                    Format1,
                    Format2,
                    Environment.NewLine
                );
        } // method ToString

        #endregion

    } // class GblStatement

    /// <summary>
    /// Настройки для глобальной корректировки.
    /// </summary>
    public sealed class GblSettings
    {
        #region Constants

        /// <summary>
        /// Разделитель элементов в строке.
        /// </summary>
        private const string Delimiter = "\x001F\x001E";

        #endregion

        #region Properties

        /// <summary>
        /// Actualize records after processing.
        /// </summary>
        public bool Actualize { get; set; } = true;

        /// <summary>
        /// Process 'autoin.gbl'.
        /// </summary>
        public bool Autoin { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// First record MFN.
        /// </summary>
        public int FirstRecord { get; set; } = 1;

        /// <summary>
        /// Provide formal control.
        /// </summary>
        public bool FormalControl { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        /// <remarks>0 means 'all records in the database'.
        /// </remarks>
        public int MaxMfn { get; set; }

        /// <summary>
        /// List of MFN to process.
        /// </summary>
        public int[]? MfnList { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        /// <remarks>0 means 'all records in the database'.
        /// </remarks>
        public int MinMfn { get; set; }

        /// <summary>
        /// Number of records to process.
        /// </summary>
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Search expression.
        /// </summary>
        public string? SearchExpression { get; set; }

        /// <summary>
        /// Statements.
        /// </summary>
        public List<GblStatement> Statements { get; private set; } = new ();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public GblSettings()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="connection">Настроенное подключение.</param>
        public GblSettings (SyncConnection connection) =>
            Database = connection.Database;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="connection">Настроенное подключение.</param>
        /// <param name="statements">Операторы ГК.</param>
        public GblSettings
            (
                SyncConnection connection,
                IEnumerable<GblStatement> statements
            )
            : this(connection) => Statements.AddRange(statements);

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование пользовательского запроса.
        /// </summary>
        public void Encode
            (
                SyncQuery query
            )
        {
            query.Add(Actualize ? 1 : 0);
            if (!string.IsNullOrEmpty(FileName))
            {
                query.AddAnsi("@" + FileName);
            }
            else
            {
                var builder = new StringBuilder();
                // "!" здесь означает, что передавать будем в UTF-8
                builder.Append('!');
                // не знаю, что тут означает "0"
                builder.Append('0');
                builder.Append(Delimiter);
                foreach (var statement in Statements)
                {
                    // TODO: сделать подстановку параметров
                    // $encoded .=  $settings->substituteParameters(strval($statement));
                    builder.Append(statement.EncodeForProtocol());
                    builder.Append(Delimiter);
                }

                builder.Append(Delimiter);
                query.AddUtf(builder.ToString());
            }

            // отбор записей на основе поиска
            query.AddUtf(SearchExpression); // поиск по словарю
            query.Add(MinMfn); // нижняя граница MFN
            query.Add(MaxMfn); // верхняя граница MFN
            //query.AddUtf(SequentialExpression); // последовательный поиск

            // TODO поддержка режима "кроме отмеченных"
            if (MfnList is { Length: not 0 })
            {
                query.Add(MfnList.Length);
                foreach (var mfn in MfnList)
                {
                    query.Add(mfn);
                }
            }
            else
            {
                var count = MaxMfn - MinMfn + 1;
                query.Add(count);
                for (var mfn = 0; mfn <= MaxMfn; mfn++)
                {
                    query.Add(mfn);
                }
            }

            if (!FormalControl)
            {
                query.AddAnsi("*");
            }

            if (!Autoin)
            {
                query.AddAnsi("&");
            }

        } // method Encode

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given interval of MFN.
        /// </summary>
        public static GblSettings ForInterval
            (
                SyncConnection connection,
                int minMfn,
                int maxMfn,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings(connection, statements)
            {
                MinMfn = minMfn,
                MaxMfn = maxMfn
            };

            return result;

        } // method ForInterval

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given interval of MFN.
        /// </summary>
        public static GblSettings ForInterval
            (
                SyncConnection connection,
                string database,
                int minMfn,
                int maxMfn,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings(connection, statements)
            {
                Database = database,
                MinMfn = minMfn,
                MaxMfn = maxMfn
            };

            return result;

        } // method ForInterval

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given list of MFN.
        /// </summary>
        public static GblSettings ForList
            (
                SyncConnection connection,
                IEnumerable<int> mfnList,
                IEnumerable<GblStatement> statements
            )
        {

            var result = new GblSettings(connection, statements)
            {
                MfnList = mfnList.ToArray()
            };

            return result;

        } // method ForList

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given list of MFN.
        /// </summary>
        public static GblSettings ForList
            (
                SyncConnection connection,
                string database,
                IEnumerable<int> mfnList,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings(connection, statements)
            {
                Database = database,
                MfnList = mfnList.ToArray()
            };

            return result;
        } // method ForList

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given list of MFN.
        /// </summary>
        public static GblSettings ForList
            (
                SyncConnection connection,
                string database,
                IEnumerable<int> mfnList
            )
        {
            var result = new GblSettings(connection)
            {
                Database = database,
                MfnList = mfnList.ToArray()
            };

            return result;
        } // method ForList

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given searchExpression.
        /// </summary>
        public static GblSettings ForSearchExpression
            (
                SyncConnection connection,
                string searchExpression,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings(connection, statements)
            {
                SearchExpression = searchExpression
            };

            return result;
        } // method ForSearchExpression

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given searchExpression.
        /// </summary>
        public static GblSettings ForSearchExpression
            (
                SyncConnection connection,
                string database,
                string searchExpression,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings(connection, statements)
            {
                Database = database,
                SearchExpression = searchExpression
            };

            return result;
        } // method ForSearchExpression

        /// <summary>
        /// Set (server) file name.
        /// </summary>
        public GblSettings SetFileName
            (
                string fileName
            )
        {
            FileName = fileName;

            return this;
        } // method SetFileName

        /// <summary>
        /// Set first record and number of records
        /// to process.
        /// </summary>
        public GblSettings SetRange
            (
                int firstRecord,
                int numberOfRecords
            )
        {
            FirstRecord = firstRecord;
            NumberOfRecords = numberOfRecords;

            return this;

        } // method SetRange

        /// <summary>
        /// Set search expression.
        /// </summary>
        public GblSettings SetSearchExpression
            (
                string searchExpression
            )
        {
            SearchExpression = searchExpression;

            return this;

        } // method SetSearchExpression

        #endregion

    } // class GblSettings

    /// <summary>
    /// Информация о базе данных ИРБИС.
    /// </summary>
    public sealed class DatabaseInfo
    {
        #region Constants

        /// <summary>
        /// Разделитель элементов
        /// </summary>
        public const char ItemDelimiter = (char)0x1E;

        #endregion

        #region Properties

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Описание базы данных
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Максимальный MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Список логически удаленных записей.
        /// </summary>
        public int[]? LogicallyDeletedRecords { get; set; }

        /// <summary>
        /// Список физически удаленных записей.
        /// </summary>
        public int[]? PhysicallyDeletedRecords { get; set; }

        /// <summary>
        /// Список неактуализированных записей.
        /// </summary>
        public int[]? NonActualizedRecords { get; set; }

        /// <summary>
        /// Список заблокированных записей.
        /// </summary>
        public int[]? LockedRecords { get; set; }

        /// <summary>
        /// Флаг монопольной блокировки базы данных.
        /// </summary>
        public bool DatabaseLocked { get; set; }

        /// <summary>
        /// База данных только для чтения.
        /// </summary>
        public bool ReadOnly { get; set; }

        #endregion

        #region Private members

        private static void _Write
            (
                TextWriter writer,
                string name,
                int[]? mfns
            )
        {
            writer.Write($"{name}: ");
            writer.WriteLine
                (
                    mfns is null or { Length: 0 }
                    ? "None"
                    : string.Join(", ", mfns)
                );
        }

        private static int[] _ParseLine
            (
                string? text
            )
        {
            if (ReferenceEquals(text, null) || text.Length == 0)
            {
                return Array.Empty<int>();
            }

            var items = text.Split(ItemDelimiter);
            var result = new int[items.Length];
            for (var i = 0; i < items.Length; i++)
            {
                result[i] = int.Parse(items[i]);
            }

            Array.Sort(result);

            return result;

        } // method ParseLine

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static DatabaseInfo Parse
            (
                string? name,
                Response response
            )
        {
            var result = new DatabaseInfo
            {
                Name = name,
                LogicallyDeletedRecords = _ParseLine(response.ReadAnsi()),
                PhysicallyDeletedRecords = _ParseLine(response.ReadAnsi()),
                NonActualizedRecords = _ParseLine(response.ReadAnsi()),
                LockedRecords = _ParseLine(response.ReadAnsi()),
                MaxMfn = _ParseLine(response.ReadAnsi())[0],
                DatabaseLocked = _ParseLine(response.ReadAnsi())[0] != 0
            };

            return result;

        } // method Parse

        /// <summary>
        /// Разбор меню со списком баз данных.
        /// </summary>
        public static DatabaseInfo[] ParseMenu
            (
                MenuFile menu
            )
        {
            var result = new List<DatabaseInfo>();

            foreach (var entry in menu.Entries)
            {
                var readOnly = false;
                var name = entry.Code;
                if (!ReferenceEquals(name, null) && name.Length != 0)
                {
                    if (name.FirstChar() == '-')
                    {
                        readOnly = true;
                        name = name.Substring(1);
                    }

                    var database = new DatabaseInfo
                    {
                        Name = name,
                        Description = entry.Comment,
                        ReadOnly = readOnly
                    };
                    result.Add(database);
                }
            }

            return result.ToArray();

        } // method ParseMenu

        /// <summary>
        /// Вывод сведений о базе данных.
        /// </summary>
        public void Write
            (
                TextWriter writer
            )
        {
            writer.WriteLine($"Database: {Name}");
            writer.WriteLine($"Max MFN={MaxMfn}");
            writer.WriteLine($"Locked={DatabaseLocked}");
            _Write(writer, "Logically deleted records", LogicallyDeletedRecords);
            _Write(writer, "Physically deleted records", PhysicallyDeletedRecords);
            _Write(writer, "Non-actualized records", NonActualizedRecords);
            _Write(writer, "Locked records", LockedRecords);

        } // method Write

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return Name.ToVisibleString();
            }

            return $"{Name} - {Description}";

        } // method ToString

        #endregion

    } // class DatabaseInfo

    /// <summary>
    /// Информация о клиенте, подключенном к серверу ИРБИС
    /// (не обязательно о текущем).
    /// </summary>
    [DebuggerDisplay("{IPAddress} {Name} {Workstation}")]
    public sealed class ClientInfo
    {
        #region Properties

        /// <summary>
        /// Номер
        /// </summary>
        public string? Number { get; set; }

        /// <summary>
        /// Адрес клиента
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string? IPAddress { get; set; }

        /// <summary>
        /// Порт клиента
        /// </summary>
        public string? Port { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Идентификатор клиентской программы
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string? ID { get; set; }

        /// <summary>
        /// Клиентский АРМ
        /// </summary>
        public string? Workstation { get; set; }

        /// <summary>
        /// Время подключения к серверу
        /// </summary>
        public string? Registered { get; set; }

        /// <summary>
        /// Последнее подтверждение
        /// </summary>
        public string? Acknowledged { get; set; }

        /// <summary>
        /// Последняя команда
        /// </summary>
        public string? LastCommand { get; set; }

        /// <summary>
        /// Номер последней команды
        /// </summary>
        public string? CommandNumber { get; set; }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() =>
            $"IP: {IPAddress}, ID: {ID}, {Workstation}";

        #endregion

    } // class ClientInfo

    /// <summary>
    /// Статистика работы сервера ИРБИС64.
    /// </summary>
    public sealed class ServerStat
    {
        #region Properties

        /// <summary>
        /// List of running client.
        /// </summary>
        public ClientInfo[]? RunningClients { get; set; }

        /// <summary>
        /// Current client count.
        /// </summary>
        public int ClientCount { get; set; }

        /// <summary>
        /// Total commands executed since server start.
        /// </summary>
        public int TotalCommandCount { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse server response.
        /// </summary>
        public static ServerStat Parse
            (
                Response response
            )
        {
            var result = new ServerStat
            {
                TotalCommandCount = response.RequireInt32(),
                ClientCount = response.RequireInt32(),
            };

            var linesPerClient = response.RequireInt32();
            var clients = new List<ClientInfo>();

            for(var i = 0; i < result.ClientCount; i++)
            {
                var lines = response.GetAnsiStrings(linesPerClient + 1);
                if (ReferenceEquals(lines, null))
                {
                    break;
                }

                var client = new ClientInfo();
                if (lines.Length != 0)
                {
                    client.Number = lines[0].EmptyToNull();
                }

                if (lines.Length > 1)
                {
                    client.IPAddress = lines[1].EmptyToNull();
                }

                if (lines.Length > 2)
                {
                    client.Port = lines[2].EmptyToNull();
                }

                if (lines.Length > 3)
                {
                    client.Name = lines[3].EmptyToNull();
                }

                if (lines.Length > 4)
                {
                    client.ID = lines[4].EmptyToNull();
                }

                if (lines.Length > 5)
                {
                    client.Workstation = lines[5].EmptyToNull();
                }

                if (lines.Length > 6)
                {
                    client.Registered = lines[6].EmptyToNull();
                }

                if (lines.Length > 7)
                {
                    client.Acknowledged = lines[7].EmptyToNull();
                }

                if (lines.Length > 8)
                {
                    client.LastCommand = lines[8].EmptyToNull();
                }

                if (lines.Length > 9)
                {
                    client.CommandNumber = lines[9].EmptyToNull();
                }

                clients.Add(client);
            }

            result.RunningClients = clients.ToArray();

            return result;

        } // method Parse

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            var result = new StringBuilder();

            result.AppendFormat("Command executed: {0}", TotalCommandCount);
            result.AppendLine();
            result.AppendFormat("Running clients: {0}", ClientCount);
            result.AppendLine();
            if (!ReferenceEquals(RunningClients, null))
            {
                foreach (ClientInfo client in RunningClients)
                {
                    result.AppendLine(client.ToString());
                }

            }

            return result.ToString();

        } // method ToString

        #endregion

    } // class ServerStat

    /// <summary>
    /// Информация о версии сервера ИРБИС64.
    /// </summary>
    public sealed class ServerVersion
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        public string? Organization { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int MaxClients { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ConnectedClients { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        public static ServerVersion Parse
            (
                Response response
            )
        {
            var lines = response.ReadRemainingAnsiLines();
            var result = new ServerVersion();

            if (lines.Length >= 4)
            {
                result.Organization = lines[0];
                result.Version = lines[1];
                result.ConnectedClients = lines[2].SafeToInt32();
                result.MaxClients = lines[3].SafeToInt32();
            }
            else
            {
                result.Version = lines[0];
                result.ConnectedClients = lines[1].SafeToInt32();
                result.MaxClients = lines[2].SafeToInt32();
            }

            return result;

        } // method Parse

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        // ReSharper disable UseStringInterpolation
        public override string ToString() => string.Format
            (
                "Version: {0}, MaxClients: {1}, "
                + "ConnectedClients: {2}, Organization: {3}",
                Version.ToVisibleString(),
                MaxClients,
                ConnectedClients,
                Organization.ToVisibleString()
            );
        // ReSharper restore UseStringInterpolation

        #endregion

    } // class ServerVersion

    /// <summary>
    /// Синхронное подключение к серверу ИРБИС64.
    /// </summary>
    public class SyncConnection
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Адрес или имя хоста сервера ИРБИС64.
        /// </summary>
        /// <remarks>Значение по умолчанию "127.0.0.1".</remarks>
        public string? Host { get; set; } = "127.0.0.1";

        /// <summary>
        /// Номер порта, на котором сервер ИРБИС64 принимает клиентские подключения.
        /// </summary>
        /// <remarks>Значение по умолчанию 6666.</remarks>
        public ushort Port { get; set; } = 6666;

        /// <summary>
        /// Имя (логин) пользователя системы ИРБИС64.
        /// </summary>
        /// <remarks>Значение по умолчанию <c>null</c>,
        /// с таким значением подключение не может быть установлено.</remarks>
        public string? Username { get; set; } = string.Empty;

        /// <summary>
        /// Пароль пользователя системы ИРБИС64.
        /// </summary>
        /// <remarks>Значение по умолчанию <c>null</c>,
        /// с таким значением подключение не может быть установлено.</remarks>
        public string? Password { get; set; } = string.Empty;

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        /// <remarks>Значение по умолчанию <c>"IBIS"</c>.
        /// </remarks>
        public string? Database { get; set; } = "IBIS";

        /// <summary>
        /// Код типа приложения.
        /// </summary>
        /// <remarks>Значение по умолчанию <c>null</c>.
        /// </remarks>
        public string? Workstation { get; set; } = "C";

        /// <summary>
        /// Уникальный идентификатор клиента.
        /// </summary>
        public int ClientId { get; protected internal set; }

        /// <summary>
        /// Порядковый номер команды.
        /// </summary>
        public int QueryId
        {
            get => _queryId; // переменная нужна для Interlocked.Increment
            protected internal set => _queryId = value;
        }

        /// <summary>
        /// Признак активного подключения к серверу.
        /// </summary>
        public bool Connected { get; protected internal set; }

        /// <summary>
        /// Код ошибки, установленный последней командой.
        /// </summary>
        public int LastError { get; protected internal set; }

        /// <summary>
        /// Токен для отмены длительных операций.
        /// </summary>
        public CancellationToken Cancellation { get; protected set; }

        /// <summary>
        /// Версия сервера. Берется из ответа на регистрацию клиента.
        /// Сервер может прислать и пустую строку, надо быть
        /// к этому готовым.
        /// </summary>
        public string? ServerVersion { get; protected internal set; }

        /// <summary>
        /// INI-файл, присылвемый сервером в ответ на регистрацию клиента.
        /// </summary>
        public IniFile? IniFile { get; protected set; }

        /// <summary>
        /// Интервал подтверждения на сервере, минуты.
        /// Берется из ответа сервера при регистрации клиента.
        /// Сервер может прислать и пустую строку, к этому надо
        /// быть готовым.
        /// </summary>
        public int Interval { get; protected set; }

        #endregion

        #region Private members

        /// <summary>
        /// Номер запроса.
        /// </summary>
        protected int _queryId;

        #endregion

        #region Public methods

        /// <summary>
        /// Подстановка имени текущей базы данных, если она не задана явно.
        /// </summary>
        public string EnsureDatabase(string? database = null) =>
            ReferenceEquals(database, null) || database.Length == 0
                ? ReferenceEquals(Database, null) || Database.Length == 0
                    ? throw new ArgumentException(nameof(Database))
                    : Database
                : database;

        /// <summary>
        /// Проверка состояния подключения.
        /// </summary>
        public bool CheckProviderState()
        {
            if (!Connected)
            {
                LastError = -100_500;
            }

            return Connected;

        } // method CheckProviderState

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="args">Опциональные параметры команды
        /// (в кодировке ANSI).</param>
        /// <returns>Ответ сервера.</returns>
        public Response? ExecuteSync
            (
                string command,
                params object[] args
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, command);
            foreach (var arg in args)
            {
                query.AddAnsi(arg.ToString());
            }

            var result = ExecuteSync(query);

            return result;

        } // method ExecuteSync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <returns>Ответ сервера.</returns>
        public Response? ExecuteSync
            (
                string command
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, command);
            var result = ExecuteSync(query);

            return result;

        } // method ExecuteSync

        /// <summary>
        /// Отправка запроса на сервер по упрощённой схеме.
        /// </summary>
        /// <param name="command">Код команды.</param>
        /// <param name="arg1">Первый и единственный параметр команды.</param>
        /// <returns>Ответ сервера.</returns>
        public Response? ExecuteSync
            (
                string command,
                object arg1
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, command);
            query.AddAnsi(arg1.ToString());

            var result = ExecuteSync(query);

            return result;

        } // method ExecuteSync

        /// <summary>
        /// Выполнение обмена с сервером.
        /// </summary>
        public Response? TransactSync
            (
                SyncQuery query
            )
        {
            using var client = new TcpClient(AddressFamily.InterNetwork);
            try
            {
                var host = Host.ThrowIfNull(nameof(Host));
                client.Connect(host, Port);
            }
            catch (Exception)
            {
                LastError = -100_002;

                return default;
            }

            var socket = client.Client;
            var length = query.GetLength();
            var prefix = new byte[12];
            length = Private.Int32ToBytes(length, prefix);
            prefix[length] = 10; // перевод строки
            var body = query.GetBody();

            try
            {
                socket.Send(prefix, 0, length + 1, SocketFlags.None);
                socket.Send(body, SocketFlags.None);
                socket.Shutdown(SocketShutdown.Send);
            }
            catch (Exception)
            {
                LastError = -100_002;

                return default;
            }

            var result = new Response();
            try
            {
                while (true)
                {
                    var buffer = new byte[2048];
                    var read = socket.Receive(buffer, SocketFlags.None);
                    if (read <= 0)
                    {
                        break;
                    }

                    var chunk = new ArraySegment<byte>(buffer, 0, read);
                    result.Add(chunk);
                }
            }
            catch (Exception)
            {
                LastError = -100_002;

                return default;
            }

            return result;

        } // method TransactSync

        #endregion

        #region ISyncConnection members

        /// <summary>
        /// Отправка клиентского запроса на сервер
        /// и получение ответа от него.
        /// </summary>
        /// <param name="query">Клиентский запрос.</param>
        /// <returns>Ответ от сервера.</returns>
        public Response? ExecuteSync
            (
                SyncQuery query
            )
        {
            Response? result;
            try
            {

                result = TransactSync(query);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                return null;
            }

            if (result is null)
            {
                return null;
            }

            result.Parse();
            Interlocked.Increment(ref _queryId);

            return result;

        } // method ExecuteSync

        /// <summary>
        /// Получение статистики по базе данных.
        /// </summary>
        public string? GetDatabaseStat
            (
                StatDefinition definition
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, Constants.DatabaseStat);
            definition.Encode(this, query);

            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = "{\\rtf1 "
                + response!.ReadRemainingUtfText()
                + "}";

            return result;

        } // method GetDatabaseStat

        /// <summary>
        /// Переподключение к серверу.
        /// </summary>
        public bool Reconnect()
        {
            if (Connected)
            {
                Disconnect();
            }

            IniFile?.Dispose();
            IniFile = null;

            return Connect();

        } // method Reconnect

        /// <summary>
        /// Разблокирование указанной записи (альтернативный вариант).
        /// </summary>
        public bool UnlockRecordAlt(int mfn) =>
            ExecuteSync("E", EnsureDatabase(), mfn).IsGood();

        #endregion

        #region ISyncProvider members

        /// <summary>
        /// Актуализация всех неактуализированных записей
        /// в указанной базе данных.
        /// </summary>
        public bool ActualizeDatabase (string? database = default) =>
            ActualizeRecord ( new() { Database = database, Mfn = 0 } );

        /// <summary>
        /// Актуализация записи.
        /// </summary>
        public bool ActualizeRecord (ActualizeRecordParameters parameters) => ExecuteSync
                (
                    Constants.ActualizeRecord,
                    EnsureDatabase(parameters.Database),
                    parameters.Mfn
                ).IsGood();

        /// <summary>
        /// Подключение к серверу.
        /// </summary>
        public bool Connect()
        {
            if (Connected)
            {
                return true;
            }

            AGAIN:
            LastError = 0;
            QueryId = 1;
            ClientId = new Random().Next(100000, 999999);

            // нельзя использовать using response из-за goto
            var query = new SyncQuery(this, Constants.RegisterClient);
            query.AddAnsi(Username);
            query.AddAnsi(Password);

            var response = ExecuteSync(query);
            if (response is null)
            {
                LastError = -100_500;
                return false;
            }

            if (response.GetReturnCode() == -3337)
            {
                goto AGAIN;
            }

            if (response.ReturnCode < 0)
            {
                LastError = response.ReturnCode;
                return false;
            }

            ServerVersion = response.ServerVersion;
            Interval = response.ReadInteger();

            IniFile = new IniFile();
            var remainingText = response.RemainingText(Utility.Ansi);
            var reader = new StringReader(remainingText);
            IniFile.Read(reader);
            Connected = true;

            return true;

        } // method Connect

        /// <summary>
        /// Создание базы данных.
        /// </summary>
        public bool CreateDatabase
            (
                CreateDatabaseParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery(this, Constants.CreateDatabase);
            query.AddAnsi(EnsureDatabase(parameters.Database));
            query.AddAnsi(parameters.Description);
            query.Add(parameters.ReaderAccess);
            var response = ExecuteSync(query);

            return response.IsGood();

        } // method CreateDatabase

        /// <summary>
        /// Создание словаря в указанной базе данных.
        /// </summary>
        public bool CreateDictionary (string? databaseName = default) =>
            ExecuteSync(Constants.CreateDictionary,
                EnsureDatabase(databaseName)).IsGood();

        /// <summary>
        /// Удаление базы данных на сервере.
        /// </summary>
        public bool DeleteDatabase (string? databaseName = default) =>
            ExecuteSync(Constants.DeleteDatabase,
                EnsureDatabase(databaseName)).IsGood();

        /// <summary>
        /// Удаление записи с указанным MFN.
        /// </summary>
        public bool DeleteRecord
            (
                int mfn
            )
        {
            var record = ReadRecord(mfn);
            if (record is null)
            {
                return false;
            }

            if (record.Deleted)
            {
                return true;
            }

            record.Status |= RecordStatus.LogicallyDeleted;

            return WriteRecord(record, dontParse: true);

        } // method DeleteRecord

        /// <summary>
        /// Отключение от сервера.
        /// </summary>
        public bool Disconnect()
        {
            if (Connected)
            {
                try
                {
                    var _ = ExecuteSync(Constants.UnregisterClient);
                }
                catch (Exception)
                {
                    Debug.WriteLine(nameof(SyncConnection) + "::" + nameof(Disconnect) + ": error");
                }

                Connected = false;
            }

            return true;

        } // method Disconnect

        /// <summary>
        /// Файл существует?
        /// </summary>
        public bool FileExist(FileSpecification specification) =>
            !string.IsNullOrEmpty(ReadTextFile(specification));

        /// <summary>
        /// Форматирование указанной записи по ее MFN.
        /// </summary>
        public string? FormatRecord
            (
                string format,
                int mfn
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, Constants.FormatRecord);
            query.AddAnsi(Database);
            query.AddFormat(format);
            query.Add(1);
            query.Add(mfn);
            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = response!.ReadRemainingUtfText().TrimEnd();

            return result;

        } // method FormatRecord

        /// <summary>
        /// Форматирование записи.
        /// </summary>
        public bool FormatRecords
            (
                FormatRecordParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            // TODO: обнаруживать Records

            if (parameters.Mfns.IsNullOrEmpty())
            {
                parameters.Result = FormatRecord(parameters.Format!, parameters.Mfn)!;
                return true;
            }

            var query = new SyncQuery(this, Constants.FormatRecord);
            query.AddAnsi(EnsureDatabase(parameters.Database));
            query.AddFormat(parameters.Format);
            query.Add(parameters.Mfns!.Length);
            foreach (var mfn in parameters.Mfns)
            {
                query.Add(mfn);
            }

            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return false;
            }

            var lines = response!.ReadRemainingUtfLines();
            var result = new List<string>(lines.Length);
            if (parameters.Mfns.Length == 1)
            {
                result.Add(lines[0]);
            }
            else
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    var parts = line.Split(Constants.NumberSign, 2);
                    if (parts.Length > 1)
                    {
                        result.Add(parts[1]);
                    }
                }
            }

            parameters.Result = result.ToArray();

            return true;

        } // method FormatRecords

        /// <summary>
        /// Форматирование записи в клиентском представлении.
        /// </summary>
        public string? FormatRecord
            (
                string format,
                Record record
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }

            var query = new SyncQuery(this, Constants.FormatRecord);
            query.AddAnsi(EnsureDatabase(string.Empty));
            query.AddFormat(format);
            query.Add(-2);
            query.AddUtf(record.Encode());
            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = response!.ReadRemainingUtfText().TrimEnd();

            return result;

        } // method FormatRecord

        /// <summary>
        /// Форматирование записей по их MFN.
        /// </summary>
        public string[]? FormatRecords
            (
                int[] mfns,
                string format
            )
        {
            var parameters = new FormatRecordParameters
            {
                Database = EnsureDatabase(),
                Mfns = mfns,
                Format = format
            };

            return FormatRecords(parameters)
                ? parameters.Result.AsArray()
                : null;

        } // method FormatRecords

        /// <summary>
        /// Полнотекстовый поиск.
        /// </summary>
        public FullTextResult? FullTextSearch
            (
                SearchParameters searchParameters,
                TextParameters textParameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, Constants.NewFulltextSearch);
            searchParameters.Encode(this, query);
            textParameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = new FullTextResult();
            result.Decode(response!);

            return result;

        } // method FullTextSearch

        /// <summary>
        /// Получение информации о базе данных.
        /// </summary>
        public DatabaseInfo? GetDatabaseInfo(string? databaseName = default) =>
            ExecuteSync(Constants.RecordList, EnsureDatabase(databaseName))
                .Transform (resp => DatabaseInfo.Parse (EnsureDatabase(databaseName), resp));

        /// <summary>
        /// Получение максимального MFN для указанной базы данных.
        /// </summary>
        public int GetMaxMfn
            (
                string? databaseName = default
            )
        {
            var response = ExecuteSync(Constants.GetMaxMfn, EnsureDatabase(databaseName));

            return response.IsGood() ? response!.ReturnCode : 0;

        } // method GetMaxMfn

        /// <summary>
        /// Получение статистики работы сервера ИРБИС64.
        /// </summary>
        public ServerStat? GetServerStat() =>
            ExecuteSync(Constants.GetServerStat).Transform(ServerStat.Parse);

        /// <summary>
        /// Получение информации о версии сервера.
        /// </summary>
        public ServerVersion? GetServerVersion() =>
            ExecuteSync(Constants.ServerInfo).Transform(ManagedIrbis.ServerVersion.Parse);

        /// <summary>
        /// Глобальная корректировка.
        /// </summary>
        public GblResult? GlobalCorrection
            (
                GblSettings settings
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var database = EnsureDatabase(settings.Database);
            var query = new SyncQuery(this, Constants.GlobalCorrection);
            query.AddAnsi(database);
            settings.Encode(query);

            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return null;
            }

            var result = new GblResult();
            result.Parse(response!);

            return result;

        } // method GlobalCorrection

        /// <summary>
        /// Получение списка баз данных.
        /// </summary>
        public DatabaseInfo[] ListDatabases
            (
                string listFile = "dbnam3.mnu"
            )
        {
            var specification = new FileSpecification
            {
                Path = IrbisPath.Data,
                FileName = listFile
            };
            var menu = RequireMenuFile(specification);

            return DatabaseInfo.ParseMenu(menu);

        } // method ListDatabases

        /// <summary>
        /// Получение списка файлов из ответа сервера.
        /// </summary>
        public static string[]? ListFiles
            (
                Response? response
            )
        {
            if (response is null)
            {
                return null;
            }

            var lines = response.ReadRemainingAnsiLines();
            var result = new List<string>();
            var delimiters = new [] { Constants.WindowsDelimiter };
            foreach (var line in lines)
            {
                var files = Private.SplitIrbisToLines(line);
                foreach (var file1 in files)
                {
                    if (!string.IsNullOrEmpty(file1))
                    {
                        foreach (var file2 in file1.Split(delimiters, StringSplitOptions.None))
                        {
                            if (!string.IsNullOrEmpty(file2))
                            {
                                result.Add(file2);
                            }
                        }
                    }
                }
            }

            return result.ToArray();

        } // method ListFiles

        /// <summary>
        /// Получение списка файлов.
        /// </summary>
        public string[]? ListFiles
            (
                params FileSpecification[] specifications
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            if (specifications.Length == 0)
            {
                return Array.Empty<string>();
            }

            var query = new SyncQuery(this, Constants.ListFiles);
            foreach (var specification in specifications)
            {
                query.AddAnsi(specification.ToString());
            }

            var response = ExecuteSync(query);

            return ListFiles(response);

        } // method ListFiles

        /// <summary>
        /// Получение списка файлов, соответствующих спецификации.
        /// </summary>
        public string[]? ListFiles
            (
                string specification
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            if (string.IsNullOrEmpty(specification))
            {
                return Array.Empty<string>();
            }

            var response = ExecuteSync(Constants.ListFiles, specification);

            return ListFiles(response);

        } // method ListFiles

        /// <summary>
        /// Список серверных процессов.
        /// </summary>
        public ProcessInfo[]? ListProcesses() =>
            ExecuteSync(Constants.GetProcessList).Transform(ProcessInfo.Parse);

        /// <summary>
        /// Список пользователей, зарегистрированных в системе.
        /// </summary>
        public UserInfo[]? ListUsers() =>
            ExecuteSync(Constants.GetUserList).Transform(UserInfo.Parse);

        /// <summary>
        /// Блокирование указанной записи.
        /// </summary>
        public bool LockRecord
            (
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Mfn = mfn,
                Lock = true
            };

            return ReadRecord (parameters) is not null;

        } // method LockRecord

        /// <summary>
        /// Пустая операция - подтверждение регистрации.
        /// </summary>
        public bool NoOperation() => ExecuteSync(Constants.Nop).IsGood();

        /// <summary>
        /// Построение таблицы.
        /// </summary>
        public string? PrintTable (TableDefinition definition)
        {
            var query = new SyncQuery(this, Constants.Print);
            query.AddAnsi(EnsureDatabase(definition.DatabaseName));
            definition.Encode(query);

            var response = ExecuteSync(query);

            return response?.ReadRemainingUtfText();

        } // method PrintTable

        /// <summary>
        /// Чтение двоичного файла с сервера.
        /// </summary>
        public byte[]? ReadBinaryFile
            (
                FileSpecification specification
            )
        {
            specification.BinaryFile = true;
            var response = ExecuteSync(Constants.ReadDocument, specification.ToString());
            if (response is null || !response.FindPreamble())
            {
                return null;
            }

            return response.RemainingBytes();

        } // method ReadBinaryFile

        /// <summary>
        /// Чтение INI-файла как текстового.
        /// </summary>
        public IniFile? ReadIniFile (FileSpecification specification)
        {
            var content = ReadTextFile(specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader(content);
            var result = new IniFile { FileName = specification.FileName };
            result.Read(reader);

            return result;

        } // method ReadIniFile

        /// <summary>
        /// Чтение меню как текстового файла.
        /// </summary>
        public MenuFile? ReadMenuFile
            (
                FileSpecification specification
            )
        {
            var content = ReadTextFile(specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader(content);
            var result = MenuFile.ParseStream(reader);
            result.FileName = specification.FileName;

            return result;

        } // method ReadMenuFile

        /// <summary>
        /// Чтение постингов термина.
        /// </summary>
        public TermPosting[]? ReadPostings
            (
                PostingParameters parameters
            )
        {
            var query = new SyncQuery(this, Constants.ReadPostings);
            parameters.Encode(this, query);

            var response = ExecuteSync(query);
            if (!response.IsGood(Constants.GoodCodesForReadTerms))
            {
                return null;
            }

            return TermPosting.Parse(response!);

        } // method ReadPosting

        /// <summary>
        /// Чтение библиографической записи.
        /// </summary>
        public Record? ReadRecord
            (
                ReadRecordParameters parameters
            )
        {
            Record? result;

            try
            {
                var database = EnsureDatabase(parameters.Database);
                var query = new SyncQuery(this, Constants.ReadRecord);
                query.AddAnsi(database);
                query.Add(parameters.Mfn);
                if (parameters.Version != 0)
                {
                    query.Add(parameters.Version);
                }
                else
                {
                    query.Add(parameters.Lock);
                }

                query.AddFormat(parameters.Format);

                var response = ExecuteSync(query);
                if (!response.IsGood(Constants.GoodCodesForReadRecord))
                {
                    return null;
                }

                result = new Record
                {
                    Database = Database
                };

                switch ((ReturnCode) response!.ReturnCode)
                {
                    case ReturnCode.PreviousVersionNotExist:
                        result.Status |= RecordStatus.Absent;
                        break;

                    case ReturnCode.PhysicallyDeleted:
                    case ReturnCode.PhysicallyDeleted1:
                        result.Status |= RecordStatus.PhysicallyDeleted;
                        break;

                    default:
                        result.Decode(response);
                        break;
                }

                if (parameters.Version != 0)
                {
                    UnlockRecords(new [] { parameters.Mfn });
                }
            }
            catch (Exception exception)
            {
                throw new IrbisException
                (
                    nameof(ReadRecord) + " " + parameters,
                    exception
                );
            }

            return result;

        } // method ReadRecord

        /// <summary>
        /// Чтение библиографической записи.
        /// </summary>
        public Record? ReadRecord
            (
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Database = Database,
                Mfn = mfn
            };

            return ReadRecord(parameters);

        } // method ReadRecord

        /// <summary>
        /// Чтение постингов, относящихся к указанной записи.
        /// </summary>
        public TermPosting[]? ReadRecordPostings
            (
                ReadRecordParameters parameters,
                string prefix
            )
        {
            if (!CheckProviderState() || string.IsNullOrEmpty(prefix))
            {
                return null;
            }

            var query = new SyncQuery(this, Constants.GetRecordPostings);
            query.AddAnsi(EnsureDatabase(parameters.Database));
            query.Add(parameters.Mfn);
            query.AddUtf(prefix);

            return ExecuteSync(query).Transform(TermPosting.Parse);

        } // method ReadRecordPostings

        /// <summary>
        /// Чтение терминов поискового словаря.
        /// </summary>
        public Term[]? ReadTerms
            (
                TermParameters parameters
            )
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var command = parameters.ReverseOrder
                ? Constants.ReadTermsReverse
                : Constants.ReadTerms;
            var query = new SyncQuery(this, command);
            parameters.Encode(this, query);
            var response = ExecuteSync(query);

            return !response.IsGood(Constants.GoodCodesForReadTerms) ? null : Term.Parse(response!);

        } // method ReadTerms

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="startTerm">Параметры терминов.</param>
        /// <param name="numberOfTerms">Максимальное число терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public Term[]? ReadTerms
            (
                string startTerm,
                int numberOfTerms
            )
        {
            var parameters = new TermParameters
            {
                Database = EnsureDatabase(),
                StartTerm = startTerm,
                NumberOfTerms = numberOfTerms
            };

            return ReadTerms(parameters);

        } // method ReadTerms

        /// <summary>
        /// Чтение текстового файла с сервера.
        /// </summary>
        public string? ReadTextFile (FileSpecification specification) =>
            ExecuteSync(Constants.ReadDocument, specification.ToString())
                .TransformNoCheck (resp => Private.IrbisToWindows(resp.ReadAnsi()));

        /// <summary>
        /// Чтение несколькних текстовых файлов с сервера.
        /// </summary>
        public string[]? ReadTextFiles (FileSpecification[] specifications)
        {
            var query = new SyncQuery(this, Constants.ReadDocument);
            foreach (var specification in specifications)
            {
                query.AddAnsi(specification.ToString());
            }

            var response = ExecuteSync(query);

            return response.IsGood() ? response!.ReadRemainingAnsiLines() : null;

        } // method ReadTextFiles

        /// <summary>
        /// Перезагрузка словаря для указанной базы данных.
        /// </summary>
        public bool ReloadDictionary (string? databaseName = default) =>
            ExecuteSync(Constants.ReloadDictionary, EnsureDatabase(databaseName)).IsGood();

        /// <summary>
        /// Перезагрузка файла документов в указанной базе данных.
        /// </summary>
        public bool ReloadMasterFile (string? databaseName = default) =>
            ExecuteSync(Constants.ReloadMasterFile,
                databaseName ?? Database.ThrowIfNull(nameof(Database))).IsGood();

        /// <summary>
        /// Чтение с сервера записи, которая обязательно должна быть.
        /// </summary>
        /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
        public Record RequireRecord (int mfn) => ReadRecord(mfn)
            ?? throw new IrbisException($"Record not found: MFN={mfn}");

        /// <summary>
        /// Чтение с сервера записи, которая обязательно должна быть.
        /// </summary>
        /// <exception cref="IrbisException">Запись отсутствует или другая ошибка при чтении.</exception>
        public Record RequireRecord (string expression) => SearchReadOneRecord(expression)
            ?? throw new IrbisException($"Record not found: expression={expression}");

        /// <summary>
        /// Чтение с сервера текстового файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="FileNotFoundException">Файл отсутствует или другая ошибка при чтении.</exception>
        public string RequireTextFile (FileSpecification specification) => ReadTextFile(specification)
            ?? throw new IrbisException($"File not found: {specification}");

        /// <summary>
        /// Чтение с сервера INI-файла, который обязательно должен быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public IniFile RequireIniFile (FileSpecification specification) => ReadIniFile(specification)
            ?? throw new IrbisException($"INI not found: {specification}");

        /// <summary>
        /// Чтение с сервера файла меню, которое обязательно должно быть.
        /// </summary>
        /// <exception cref="IrbisException">Файл отсутствует или другая ошибка при чтении.</exception>
        public MenuFile RequireMenuFile (FileSpecification specification) => ReadMenuFile(specification)
            ?? throw new IrbisException($"Menu not found: {specification}");

        /// <summary>
        /// Перезапуск сервера.
        /// </summary>
        public bool RestartServer() => ExecuteSync(Constants.RestartServer).IsGood();

        /// <summary>
        /// Поиск записей.
        /// </summary>
        public FoundItem[]? Search (SearchParameters parameters)
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var query = new SyncQuery(this, Constants.Search);
            parameters.Encode(this, query);

            return ExecuteSync(query).Transform(FoundItem.Parse);

        } // method Search

        /// <summary>
        /// Упрощенный поиск.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Массив MFN найденных записей.</returns>
        public int[] Search (string expression)
        {
            if (!CheckProviderState())
            {
                return Array.Empty<int>();
            }

            var query = new SyncQuery(this, Constants.Search);
            var parameters = new SearchParameters
            {
                Database = Database,
                Expression = expression
            };
            parameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return Array.Empty<int>();
            }

            return FoundItem.ParseMfn(response!);

        } // method Search

        /// <summary>
        /// Определение количества записей, удовлетворяющих
        /// заданному запросу.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
        public int SearchCount (string expression)
        {
            if (!CheckProviderState())
            {
                return -1;
            }

            var query = new SyncQuery(this, Constants.Search);
            var parameters = new SearchParameters
            {
                Database = Database,
                Expression = expression,
                FirstRecord = 0
            };
            parameters.Encode(this, query);
            var response = ExecuteSync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return -1;
            }

            return response.ReadInteger();

        } // method SearchCount

        /// <summary>
        /// Поиск с последующим чтением записей.
        /// </summary>
        public Record[]? SearchRead (string expression)
        {
            if (!CheckProviderState())
            {
                return null;
            }

            var found = Search(expression);
            var result = new List<Record>(found.Length);
            foreach (var mfn in found)
            {
                var record = ReadRecord(mfn);
                if (record is not null)
                {
                    result.Add(record);
                }
            }

            return result.ToArray();

        } // method SearchRead

        /// <summary>
        /// Поиск с последующим чтением одной записи.
        /// </summary>
        public Record? SearchReadOneRecord
            (
                string expression
            )
        {
            var parameters = new SearchParameters
            {
                Expression = expression,
                NumberOfRecords = 1
            };
            var found = Search(parameters);

            return found is { Length: 1 }
                ? ReadRecord(found[0].Mfn)
                : default;

        } // method SearchReadOneRecord

        /// <summary>
        /// Очистка базы данных (до нулевой длины).
        /// </summary>
        public bool TruncateDatabase (string? databaseName = default) =>
            ExecuteSync(Constants.EmptyDatabase, EnsureDatabase(databaseName)).IsGood();

        /// <summary>
        /// Разблокирование базы данных.
        /// </summary>
        public bool UnlockDatabase (string? databaseName = default) =>
            ExecuteSync(Constants.UnlockDatabase, EnsureDatabase(databaseName)).IsGood();

        /// <summary>
        /// Разблокирование записей.
        /// </summary>
        public bool UnlockRecords
            (
                IEnumerable<int> mfnList,
                string? databaseName = default
            )
        {
            var query = new SyncQuery(this, Constants.UnlockRecords);
            query.AddAnsi(EnsureDatabase(databaseName));
            var counter = 0;
            foreach (var mfn in mfnList)
            {
                query.Add(mfn);
                ++counter;
            }

            // Если список MFN пуст, считаем операцию успешной
            if (counter == 0)
            {
                return true;
            }

            return ExecuteSync(query).IsGood();

        } // method UnlockRecords

        /// <summary>
        /// Обновление сервеного INI-файла.
        /// </summary>
        public bool UpdateIniFile (IEnumerable<string> lines)
        {
            var query = new SyncQuery(this, Constants.UpdateIniFile);
            var counter = 0;
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    query.AddAnsi(line);
                    ++counter;
                }
            }

            // Если список обновляемых строк пуст, считаем операцию успешной
            if (counter == 0)
            {
                return true;
            }

            return ExecuteSync(query).IsGood();

        } // method UpdateIniFile

        /// <summary>
        /// Обновление списка зарегистрированных в системе пользователей.
        /// </summary>
        public bool UpdateUserList (IEnumerable<UserInfo> users)
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery(this, Constants.SetUserList);
            var counter = 0;
            foreach (var user in users)
            {
                query.AddAnsi(user.Encode());
                ++counter;
            }

            // Если список обновляемых пользователей пуст, считаем операцию неуспешной
            if (counter == 0)
            {
                return false;
            }

            return ExecuteSync(query).IsGood();

        } // method UpdateUserList

        /// <summary>
        /// Сохранение записи на сервере.
        /// </summary>
        public bool WriteRecord
            (
                WriteRecordParameters parameters
            )
        {
            var record = parameters.Record;
            if (record is not null)
            {
                var database = EnsureDatabase(record.Database);
                var query = new SyncQuery(this, Constants.UpdateRecord);
                query.AddAnsi(database);
                query.Add(parameters.Lock);
                query.Add(parameters.Actualize);
                query.AddUtf(record.Encode());

                var response = ExecuteSync(query);
                if (!response.IsGood())
                {
                    return false;
                }

                var result = response!.ReturnCode;
                if (!parameters.DontParse)
                {
                    record.Database ??= database;
                    record.Decode(response);
                }

                parameters.MaxMfn = result;

                return true;

            }

            var records = parameters.Records.ThrowIfNull(nameof(parameters.Records));
            if (records.Length == 0)
            {
                return true;
            }

            if (records.Length == 1)
            {
                parameters.Record = records[0];
                parameters.Records = null;
                var result2 = WriteRecord(parameters);
                parameters.Record = null;
                parameters.Records = records;

                return result2;
            }

            return WriteRecords
                (
                    records,
                    parameters.Lock,
                    parameters.Actualize,
                    parameters.DontParse
                );

        } // method WriteRecord

        /// <summary>
        /// Сохранение/обновление записи в базе данных.
        /// </summary>
        public bool WriteRecord
            (
                Record record,
                bool actualize = true,
                bool lockRecord = false,
                bool dontParse = false
            )
        {
            var parameters = new WriteRecordParameters
            {
                Record = record,
                Actualize = actualize,
                Lock = lockRecord,
                DontParse = dontParse
            };

            return WriteRecord(parameters);

        } // method WriteRecord

        /// <summary>
        /// Сохранение записей на сервере.
        /// </summary>
        public bool WriteRecords
            (
                IEnumerable<Record> records,
                bool lockFlag = false,
                bool actualize = true,
                bool dontParse = true
            )
        {
            if (!CheckProviderState())
            {
                return false;
            }

            var query = new SyncQuery(this, Constants.SaveRecordGroup);
            query.Add(lockFlag);
            query.Add(actualize);
            var recordList = new List<Record>();
            foreach (var record in records)
            {
                var line = EnsureDatabase(record.Database)
                           + Constants.IrbisDelimiter
                           + Private.EncodeRecord(record);
                query.AddUtf(line);
                recordList.Add(record);
            }

            if (recordList.Count == 0)
            {
                return true;
            }

            var response = ExecuteSync(query);
            if (!response.IsGood())
            {
                return false;
            }

            if (!dontParse)
            {
                foreach (var record in recordList)
                {
                    Private.ParseResponseForWriteRecords(response!, record);
                }
            }

            return true;

        } // method WriteRecords

        /// <summary>
        /// Сохранение на сервере текстового файла.
        /// </summary>
        public bool WriteTextFile(FileSpecification specification) =>
            ExecuteSync(Constants.ReadDocument, specification.ToString()).IsGood();

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Disconnect();

        } // method Dispose

        #endregion

    } // class SyncConnection

} // namespace ManagedIrbis
