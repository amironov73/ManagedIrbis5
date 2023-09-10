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

/* Private.cs -- полезные, но при этом не публичные методы расширения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#endregion

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
        public static string ToVisibleString<T> (this T? value) where T : class =>
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
            ReferenceEquals (text, null) || text.Length == 0 ? '\0' : text[0];

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
        public static bool IsOneOf<T> (this T value, IEnumerable<T> items) where T : IComparable<T>
        {
            foreach (var one in items)
                if (value.CompareTo (one) == 0)
                    return true;

            return false;
        }

        /// <summary>
        /// Разбивка текста на отдельные строки.
        /// </summary>
        /// <remarks>Пустые строки не удаляются.</remarks>
        public static string[] SplitLines (this string text) =>
            text.Replace ("\r\n", "\n").Split ('\n');

        /// <summary>
        /// Обязательное чтение строки.
        /// </summary>
        public static string RequireLine (this TextReader reader)
        {
            var result = reader.ReadLine();
            if (ReferenceEquals (result, null))
                throw new IrbisException ("Unexpected end of stream");

            return result;
        }

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
        }

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
        }

        /// <summary>
        /// Converts given value to the specified type.
        /// </summary>
        /// <param name="value">The value to be converted.</param>
        /// <returns>Converted value.</returns>
        public static T ConvertTo<T> (object? value)
        {
            if (ReferenceEquals (value, null))
            {
                return default!;
            }

            var sourceType = value.GetType();
            var targetType = typeof (T);

            if (targetType == typeof (string))
                return (T)(object)value.ToString()!;

            if (targetType.IsAssignableFrom (sourceType))
                return (T)value;

            if (value is IConvertible)
                return (T)Convert.ChangeType (value, targetType);

            var converterFrom = TypeDescriptor.GetConverter (value);
            if (converterFrom.CanConvertTo (targetType))
                return (T)converterFrom.ConvertTo (value, targetType)!;

            var converterTo = TypeDescriptor.GetConverter (targetType);
            if (converterTo.CanConvertFrom (sourceType))
                return (T)converterTo.ConvertFrom (value)!;

            throw new IrbisException();
        }

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
        }

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
        }

        /// <summary>
        /// Бросает исключение, если переданное значение равно <c>null</c>,
        /// иначе просто возвращает его.
        /// </summary>
        public static T ThrowIfNull<T>
            (
                this T? value,
                string message
            )
            where T : class
        {
            if (ReferenceEquals (value, null))
            {
                throw new ArgumentException (message);
            }

            return value;
        }

        /// <summary>
        /// Подстановка значения по умолчанию вместо <c>null</c>.
        /// </summary>
        public static T IfNull<T> (this T? value, T defaultValue)
            where T : class
            => value ?? defaultValue;

        /// <summary>
        /// Подстановка значения по умолчания вместо пустой строки.
        /// </summary>
        public static string IfEmpty (this string? value, string defaultValue)
            => ReferenceEquals (value, null) || value.Length == 0
                ? defaultValue.Length == 0
                    ? throw new ArgumentNullException (nameof (defaultValue))
                    : defaultValue
                : value;

        /// <summary>
        /// Бросает исключение, если переданное значение равно <c>null</c>,
        /// иначе просто возвращает его.
        /// </summary>
        public static T ThrowIfNull<T> (this T? value) where T : class =>
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
            if (ReferenceEquals (value, null) || value.Length == 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

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
            if (ReferenceEquals (value, null) || value.Length == 0)
            {
                throw new ArgumentException (argumentName);
            }

            return value;
        }

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
        }

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
                number = Math.DivRem (number, 10, out var rem);
                buffer[length++] = (byte)('0' + rem);
            }

            if (flag)
            {
                buffer[length++] = (byte)'-';
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
        }

        /// <summary>
        /// Проверяет, содержит ли спан указанный символ.
        /// </summary>
        public static bool Contains (ReadOnlySpan<char> span, char chr)
        {
            foreach (var c in span)
                if (c == chr)
                    return true;

            return false;
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
                return defaultValue;

            if (!int.TryParse (text, out var result))
                result = defaultValue;

            if (result < minValue || result > maxValue)
                result = defaultValue;

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
        /// Содержит ли строка указанную подстроку?
        /// </summary>
        public static bool SafeContains (this string? text, string? subtext) =>
            !ReferenceEquals (text, null)
            && !ReferenceEquals (subtext, null)
            && text.Contains (subtext);

        /// <summary>
        /// Содержит ли строка указанный символ?
        /// </summary>
        public static bool SafeContains (this string? text, char symbol) =>
            !ReferenceEquals (text, null)
            && text.Contains (symbol);

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
            if (ReferenceEquals (text, null) || text.Length == 0)
            {
                return false;
            }

            if (!ReferenceEquals (subtext1, null) && text.Contains (subtext1))
            {
                return true;
            }

            return !ReferenceEquals (subtext2, null) && text.Contains (subtext2);
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
            if (ReferenceEquals (text, null) || text.Length == 0)
                return false;

            if (!ReferenceEquals (subtext1, null) && text.Contains (subtext1))
                return true;

            if (!ReferenceEquals (subtext2, null) && text.Contains (subtext2))
                return true;

            return !ReferenceEquals (subtext3, null) && text.Contains (subtext3);
        }

        /// <summary>
        /// Содержит ли данная строка одну из перечисленных подстрок?
        /// </summary>
        public static bool SafeContains
            (
                this string? text,
                params string?[] subtexts
            )
        {
            if (ReferenceEquals (text, null) || text.Length == 0)
                return false;

            foreach (var subtext in subtexts)
                if (!ReferenceEquals (subtext, null) && text.Contains (subtext))
                    return true;

            return false;
        }

        /// <summary>
        /// Сравнивает символы с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первый символ.</param>
        /// <param name="two">Второй символ.</param>
        /// <returns>Символы совпадают с точностью до регистра?</returns>
        public static bool SameChar (this char one, char two) =>
            char.ToUpperInvariant (one) == char.ToUpperInvariant (two);

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
            }

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
            }

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
            }

            return false;
        }

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
        public static bool SameString (this ReadOnlyMemory<char> one, string? two) =>
            one.Span.CompareTo (two.AsSpan(), StringComparison.OrdinalIgnoreCase) == 0;

        /// <summary>
        /// Сравнивает строки с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
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
        }

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
        }

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
        }

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
                if (!ReferenceEquals (item, null))
                {
                    yield return item;
                }
            }
        }

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
                if (!string.IsNullOrEmpty (line))
                {
                    yield return line!;
                }
            }
        }

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
        }

        /// <summary>
        /// Конвертирует пустую строку в <c>null</c>.
        /// </summary>
        public static string? EmptyToNull (this string? value) =>
            string.IsNullOrEmpty (value) ? null : value;

        /// <summary>
        /// Конвертирует пустую строку в <c>null</c>.
        /// </summary>
        public static string? EmptyToNull (this ReadOnlySpan<char> value) =>
            value.IsEmpty ? null : value.ToString();

        /// <summary>
        /// Конвертирует пустую строку в <c>null</c>.
        /// </summary>
        public static string? EmptyToNull (this ReadOnlyMemory<char> value) =>
            value.IsEmpty ? null : value.ToString();

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this short value) =>
            value.ToString (CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this short value, string format) =>
            value.ToString (format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this ushort value) =>
            value.ToString (CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this ushort value, string format) =>
            value.ToString (format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this int value) =>
            value.ToString (CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this int value, string format) =>
            value.ToString (format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this uint value) =>
            value.ToString (CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this uint value, string format) =>
            value.ToString (format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this long value) =>
            value.ToString (CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this long value, string format) =>
            value.ToString (format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString (this ulong value) =>
            value.ToString (CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this ulong value, string format) =>
            value.ToString (format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this float value) =>
            value.ToString (CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        public static string ToInvariantString (this float value, string format) =>
            value.ToString (format, CultureInfo.InvariantCulture);

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
        }

        /// <summary>
        /// Преобразование переводов строк ИРБИС в Windows.
        /// </summary>
        public static string? IrbisToWindows (string? text)
        {
            if (ReferenceEquals (text, null) || text.Length == 0)
                return text;

            if (!text.Contains (Constants.IrbisDelimiter))
                return text;

            var result = text.Replace
                (
                    Constants.IrbisDelimiter,
                    Constants.WindowsDelimiter
                );

            return result;
        }

        /// <summary>
        /// Заменяет переводы строк ИРБИС на Windows.
        /// Замена происходит на месте.
        /// </summary>
        public static void IrbisToWindows
            (
                byte[]? text
            )
        {
            if (ReferenceEquals (text, null) || text.Length == 0)
                return;

            var index = 0;
            while (true)
            {
                index = IndexOf (text, Constants.IrbisDelimiterBytes, index);
                if (index < 0)
                    break;

                Array.Copy (Constants.WindowsDelimiterBytes, 0, text, index, Constants.WindowsDelimiterBytes.Length);
            }
        }

        /// <summary>
        /// Разбивает текст на строки в соответствии с ИРБИС-разделителями.
        /// </summary>
        public static string[] SplitIrbisToLines
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty (text))
            {
                return Array.Empty<string>();
            }

            var provenText = IrbisToWindows (text)!;
            var result = string.IsNullOrEmpty (provenText)
                ? new[] { string.Empty }
                : provenText.Split
                    (
                        Constants.ShortIrbisDelimiterBytes,
                        StringSplitOptions.None
                    );

            return result;
        }

        /// <summary>
        /// Преобразует текст в нижний регистр.
        /// </summary>
        public static string? ToLower
            (
                string? text
            )
        {
            if (ReferenceEquals (text, null) || text.Length == 0)
            {
                return text;
            }

            var result = text.ToLowerInvariant();

            return result;
        }

        /// <summary>
        /// Преобразует текст в верхний регистр.
        /// </summary>
        public static string? ToUpper
            (
                string? text
            )
        {
            if (ReferenceEquals (text, null) || text.Length == 0)
            {
                return text;
            }

            var result = text.ToUpperInvariant();

            return result;
        }

        /// <summary>
        /// Заменяет переводы строк DOS/Windows на ИРБИС.
        /// </summary>
        public static string? WindowsToIrbis
            (
                string? text
            )
        {
            if (ReferenceEquals (text, null) || text.Length == 0)
            {
                return text;
            }

            if (!text.Contains (Constants.WindowsDelimiter))
            {
                return text;
            }

            var result = text.Replace
                (
                    Constants.WindowsDelimiter,
                    Constants.IrbisDelimiter
                );

            return result;
        }

        /// <summary>
        /// Заменяет переводы строк Windows на ИРБИС.
        /// Замена происходит на месте.
        /// </summary>
        public static void WindowsToIrbis
            (
                byte[]? text
            )
        {
            if (ReferenceEquals (text, null) || text.Length == 0)
            {
                return;
            }

            var index = 0;
            while (true)
            {
                index = IndexOf (text, Constants.WindowsDelimiterBytes, index);
                if (index < 0)
                {
                    break;
                }

                Array.Copy (Constants.IrbisDelimiterBytes, 0, text, index, Constants.IrbisDelimiterBytes.Length);
            }
        }

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static bool IsGood (this Response? response) =>
            response is not null && response.CheckReturnCode();

        /// <summary>
        /// Проверяем, хороший ли пришел ответ от сервера.
        /// </summary>
        public static bool IsGood (this Response? response, params int[] goodCodes) =>
            response is not null && response.CheckReturnCode ((goodCodes));

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static T? Transform<T> (this Response? response, Func<Response, T?> transformer) where T : class =>
            response.IsGood() ? transformer (response!) : null;

        /// <summary>
        /// Трансформация запроса во что-нибудь полезное.
        /// </summary>
        public static T? TransformNoCheck<T> (this Response? response, Func<Response, T?> transformer) where T : class
        {
            var result = response is not null ? transformer (response) : null;

            return result;
        }

        /// <summary>
        /// Remove comments from the format.
        /// </summary>
        public static string? RemoveComments
            (
                string? text
            )
        {
            const char zero = '\0';

            if (ReferenceEquals (text, null) || text.Length == 0)
            {
                return text;
            }

            if (!text.Contains ("/*"))
            {
                return text;
            }

            int index = 0, length = text.Length;
            var result = new StringBuilder (length);
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

                        result.Append (c);
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
                                        result.Append (c);
                                        break;
                                    }

                                    index++;
                                }
                            }
                            else
                            {
                                result.Append (c);
                            }
                        }
                        else if (c == '\'' || c == '"' || c == '|')
                        {
                            state = c;
                            result.Append (c);
                        }
                        else
                        {
                            result.Append (c);
                        }

                        break;
                }

                index++;
            }

            return result.ToString();
        }

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
            if (string.IsNullOrEmpty (text))
            {
                return text;
            }

            text = RemoveComments (text);
            if (ReferenceEquals (text, null) || text.Length == 0) //-V3063
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

            var result = new StringBuilder (length);
            for (var i = 0; i < length; i++)
            {
                var c = text[i];
                if (c >= ' ')
                {
                    result.Append (c);
                }
            }

            return result.ToString();
        }

        private static void _AppendIrbisLine
            (
                StringBuilder builder,
                string format,
                params object[] args
            )
        {
            builder.AppendFormat (format, args);
            builder.Append (Constants.IrbisDelimiter);
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
                builder.Append (subField.Value);
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
        }

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

            builder.Append (Constants.IrbisDelimiter);
        }

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
        }

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

                result.Append (c);
            }

            return result.ToString().AsMemory();
        }

        /// <summary>
        /// Разбор одной строки (поля).
        /// </summary>
        public static Field ParseLine
            (
                string line
            )
        {
            var reader = new StringReader (line);
            var result = new Field
            {
                Tag = Private.ParseInt32 (_ReadTo (reader, '#')),
                Value = _ReadTo (reader, '^').EmptyToNull()
            };

            while (true)
            {
                var next = reader.Read();
                if (next < 0)
                {
                    break;
                }

                var code = char.ToLower ((char)next);
                var text = _ReadTo (reader, '^');
                var subField = new SubField
                {
                    Code = code,
                    Value = text.ToString()
                };
                result.Subfields.Add (subField);
            }

            return result;
        }

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
            var regex = new Regex (@"^(-?\d+)\#(\d*)?");
            var match = regex.Match (line1);
            record.Mfn = Math.Abs (Private.ParseInt32 (match.Groups[1].Value));
            if (match.Groups[2].Length > 0)
            {
                record.Status = (RecordStatus)Private.ParseInt32 (match.Groups[2].Value);
            }

            match = regex.Match (line2);
            if (match.Groups[2].Length > 0)
            {
                record.Version = Private.ParseInt32 (match.Groups[2].Value);
            }

            return record;
        }

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
            var split = whole.Split ('\x1E');

            ParseMfnStatusVersion
                (
                    split[0],
                    split[1],
                    record
                );

            for (var i = 2; i < split.Length; i++)
            {
                var line = split[i];
                var field = ParseLine (line);
                if (field.Tag > 0)
                {
                    record.Fields.Add (field);
                }
            }

            return record;
        }

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
                if (ReferenceEquals (item, null))
                {
                    throw new ArgumentNullException (nameof (arrays));
                }

                resultLength += item.Length;
            }

            var result = new T[resultLength];
            var offset = 0;
            for (var i = 0; i < arrays.Length; i++)
            {
                var item = arrays[i]!;
                item.CopyTo (result, offset);
                offset += item.Length;
            }

            return result;
        }

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

            var result = default (T);

            if (length != 0 && occurrence >= 0 && occurrence < length)
                result = array[occurrence];

            return result;
        }
    }
}
