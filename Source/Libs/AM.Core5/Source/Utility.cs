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

/* Utility.cs -- сборник простых вспомогательных методов.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AM.PlatformAbstraction;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Сборник простых вспомогательных методов.
    /// </summary>
    public static class Utility
    {
        #region Properties

        /// <summary>
        /// Первый день следующего месяца.
        /// </summary>
        [Pure]
        public static DateTime BeginningOfNextMonth => BeginningOfTheMonth.AddMonths(1);

        /// <summary>
        /// Первый день следующего года.
        /// </summary>
        [Pure]
        public static DateTime BeginningOfNextYear => BeginningOfTheYear.AddYears(1);

        /// <summary>
        /// Первый день предыдущего месяца.
        /// </summary>
        [Pure]
        public static DateTime BeginningOfPreviousMonth => BeginningOfTheMonth.AddMonths(-1);

        /// <summary>
        /// Первый день предыдущего года.
        /// </summary>
        [Pure]
        public static DateTime BeginningOfPreviousYear => BeginningOfTheYear.AddYears(-1);

        /// <summary>
        /// Первый день текущего месяца.
        /// </summary>
        [Pure]
        public static DateTime BeginningOfTheMonth
        {
            [DebuggerStepThrough]
            get
            {
                var today = PlatformAbstractionLayer.Current.Today();

                return new DateTime(today.Year, today.Month, 1);
            }
        }

        /// <summary>
        /// Первый день текущего года.
        /// </summary>
        [Pure]
        public static DateTime BeginningOfTheYear => new
            (
                PlatformAbstractionLayer.Current.Today().Year,
                1,
                1
            );

        /// <summary>
        /// Сегодняшний день.
        /// </summary>
        [Pure]
        public static DateTime Today =>
            PlatformAbstractionLayer.Current.Today();

        /// <summary>
        /// Завтрашний день.
        /// </summary>
        [Pure]
        public static DateTime Tomorrow => Today.AddDays(1.0);

        /// <summary>
        /// Вчерашний день.
        /// </summary>
        [Pure]
        public static DateTime Yesterday => Today.AddDays(-1.0);

        /// <summary>
        /// Длительность: одни сутки.
        /// </summary>
        [Pure]
        public static TimeSpan OneDay => new (1, 0, 0, 0);

        /// <summary>
        /// Длительность: один час.
        /// </summary>
        public static TimeSpan OneHour => new (1, 0, 0);

        /// <summary>
        /// Длительность: одна минута.
        /// </summary>
        public static TimeSpan OneMinute => new (0, 1, 0);

        /// <summary>
        /// Длительность: одна секунда.
        /// </summary>
        [Pure]
        public static TimeSpan OneSecond => new (0, 0, 1);

        /// <summary>
        /// Gets the CP866 (cyrillic) <see cref="Encoding"/>.
        /// </summary>
        public static Encoding Cp866
        {
            [DebuggerStepThrough]
            get
            {
                if (ReferenceEquals(_cp866, null))
                {
                    RegisterEncodingProviders();
                    _cp866 = Encoding.GetEncoding(866);
                }

                return _cp866;
            }
        } // property Cp866

        /// <summary>
        /// Gets the Windows-1251 (cyrillic) <see cref="Encoding"/>.
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

        #endregion

        #region Private members

        private static Encoding? _cp866, _windows1251;

        #endregion

        #region Public methods

        /// <summary>
        /// Is digit from 0 to 9?
        /// </summary>
        [Pure]
        public static bool IsArabicDigit (this char c) =>
            c >= '0' && c <= '9';

        /// <summary>
        /// Is letter from A to Z or a to z?
        /// </summary>
        [Pure]
        public static bool IsLatinLetter (this char c) =>
            c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z';

        /// <summary>
        /// Is digit from 0 to 9
        /// or letter from A to Z or a to z?
        /// </summary>
        [Pure]
        public static bool IsLatinLetterOrArabicDigit (this char c) =>
            c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c >= 'a' && c <= 'z';

        /// <summary>
        /// Is letter from А to Я or а to я?
        /// </summary>
        [Pure]
        public static bool IsRussianLetter ( this char c ) =>
            c >= 'А' && c <= 'я' || c == 'Ё' || c == 'ё';

        /// <summary>
        /// Перенаправление стандартного вывода в файл.
        /// </summary>
        public static void RedirectStandardOutput
            (
                string fileName,
                Encoding encoding
            )
        {
            var stdOutput = new StreamWriter
                (
                    new FileStream
                        (
                            fileName,
                            FileMode.Create
                        ),
                    encoding
                )
            {
                AutoFlush = true
            };

            Console.SetOut(stdOutput);
        } // method RedirectStandardOutput

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
            Console.SetOut(stdOutput);

            var stdError = new StreamWriter
                (
                    Console.OpenStandardError(),
                    encoding
                )
            {
                AutoFlush = true
            };
            Console.SetError(stdError);
        } // method SetOutputCodePage

        /// <summary>
        /// Переключение кодовой страницы вывода консоли.
        /// </summary>
        public static void SetOutputCodePage (int codePage)
            => SetOutputCodePage(Encoding.GetEncoding(codePage));

        /// <summary>
        /// Определение среды исполнения: AppVeyor CI.
        /// </summary>
        [Pure]
        public static bool DetectAppVeyor() =>
            Environment.GetEnvironmentVariable("APPVEYOR").SameString("True");

        /// <summary>
        /// Определение среды исполнения: некий CI сервис вообще.
        /// </summary>
        [Pure]
        public static bool DetectCI() =>
            Environment.GetEnvironmentVariable("CI").SameString("True");

        /// <summary>
        /// Определение среды исполнения: Travis CI.
        /// </summary>
        [Pure]
        public static bool DetectTravis() =>
            Environment.GetEnvironmentVariable("TRAVIS").SameString("True");

        /// <summary>
        /// Определение среды исполнения: Github actions.
        /// </summary>
        [Pure]
        public static bool DetecGithubActions() =>
            Environment.GetEnvironmentVariable("GITHUB_ACTIONS").SameString("True");

        /// <summary>
        /// Бросает исключение, если переданное значение равно <c>null</c>,
        /// иначе просто возвращает его.
        /// </summary>
        [Pure]
        [DebuggerStepThrough]
        public static T ThrowIfNull<T>
            (
                this T? value,
                string message
            )
            where T: class
        {
            Sure.NotNull(message, nameof(message));

            if (ReferenceEquals(value, null))
            {
                Magna.Error
                    (
                        nameof(Utility) + "::" + nameof(ThrowIfNull)
                        + ": "
                        + message
                    );

                throw new ArgumentException (message);
            }

            return value;
        } // method ThrowIfNull

        /// <summary>
        /// Бросает исключение, если переданное значение равно <c>null</c>,
        /// иначе просто возвращает его.
        /// </summary>
        [Pure]
        [DebuggerStepThrough]
        public static T ThrowIfNull<T> (this T? value) where T: class =>
            ThrowIfNull (value, "Null value detected");

        /// <summary>
        /// Превращает объект в видимую строку.
        /// </summary>
        [Pure]
        public static string ToVisibleString<T> (this T? value) where T: class
            => value?.ToString() ?? "(null)";

        /// <summary>
        /// Сравнивает две строки независимо от текущей культуры.
        /// </summary>
        [Pure]
        public static bool SameInvariant (string? left, string? right) =>
            CultureInfo.InvariantCulture.CompareInfo.Compare (left, right) == 0;

        /// <summary>
        /// Безопасное получение первого символа в строке.
        /// </summary>
        [Pure]
        public static char FirstChar (this string? text) =>
            string.IsNullOrEmpty(text) ? '\0' : text[0];

        /// <summary>
        /// Безопасное получение первого символа в строке.
        /// </summary>
        [Pure]
        public static char FirstChar(this ReadOnlySpan<char> text) =>
            text.Length == 0 ? '\0' : text[0];

        /// <summary>
        /// Безопасное получение последнего символа в строке.
        /// </summary>
        [Pure]
        public static char LastChar (this string? text) =>
            string.IsNullOrEmpty(text) ? '\0' : text[^1];

        /// <summary>
        /// Безопасное получение последнего символа в строке.
        /// </summary>
        [Pure]
        public static char LastChar(this ReadOnlySpan<char> text) =>
            text.Length == 0 ? '\0' : text[^1];


        /// <summary>
        /// Сравнивает символы с точностью до регистра
        /// без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первый символ.</param>
        /// <param name="two">Второй символ.</param>
        /// <returns>Символы совпадают с точностью до регистра?</returns>
        [Pure]
        public static bool SameChar (this char one, char two) =>
            char.ToUpperInvariant(one) == char.ToUpperInvariant(two);

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
        [Pure]
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
        [Pure]
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
        [Pure]
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
        /// <param name="four">Четвертая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра?</returns>
        [Pure]
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
        } // method SameString

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
        } // method SameString

        /// <summary>
        /// Сравнивает строки с учетом регистра символов,
        /// но без учета текущей культуры.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <returns>Строки совпадают?</returns>
        [Pure]
        public static bool SameStringSensitive
            (
                this string? one,
                string? two
            )
        {
            return string.Compare
                (
                    one,
                    two,
                    StringComparison.Ordinal
                ) == 0;
        } // method SameStringSensitive

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
            return string.Compare
                (
                    one,
                    two,
                    StringComparison.Ordinal
                ) == 0
            || string.Compare
                (
                    one,
                    three,
                    StringComparison.Ordinal
                ) == 0;
        } // method SameStringSensitive

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
        } // method SameStringSensitive

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
        } // method SameStringSensitive

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
        } // method SameStringSensitive

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        [Pure]
        public static string ToInvariantString (this short value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this short value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        [Pure]
        public static string ToInvariantString (this ushort value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this ushort value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        [Pure]
        public static string ToInvariantString (this int value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this int value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        [Pure]
        public static string ToInvariantString ( this uint value ) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString ( this uint value, string format ) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        [Pure]
        public static string ToInvariantString (this long value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this long value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        [Pure]
        public static string ToInvariantString (this ulong value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this ulong value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this float value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this float value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this double value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this double value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this decimal value) =>
            value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        [Pure]
        public static string ToInvariantString (this decimal value, string format) =>
            value.ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// Register required encoding providers.
        /// </summary>
        public static void RegisterEncodingProviders() =>
            Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

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
            {
                return defaultValue;
            }

            if (!int.TryParse(text, out var result))
            {
                result = defaultValue;
            }

            if (result < minValue
                || result > maxValue)
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
        /// Определяет, равен ли ли данный объект
        /// любому из перечисленных.
        /// </summary>
        [Pure]
        public static bool IsOneOf<T>(this T value, T first, T second)
            where T : IComparable<T> =>
            value.CompareTo(first) == 0
            || value.CompareTo(second) == 0;

        /// <summary>
        /// Определяет, равен ли ли данный объект
        /// любому из перечисленных.
        /// </summary>
        [Pure]
        public static bool IsOneOf<T>(this T value, T first, T second,
            T third)
            where T : IComparable<T> =>
            value.CompareTo(first) == 0
            || value.CompareTo(second) == 0
            || value.CompareTo(third) == 0;

        /// <summary>
        /// Определяет, равен ли ли данный объект
        /// любому из перечисленных.
        /// </summary>
        [Pure]
        public static bool IsOneOf<T>(this T value, T first, T second,
            T third, T fourth)
            where T : IComparable<T> =>
            value.CompareTo(first) == 0
            || value.CompareTo(second) == 0
            || value.CompareTo(third) == 0
            || value.CompareTo(fourth) == 0;

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
            where T: IComparable<T>
        {
            foreach (var one in array)
            {
                if (value.CompareTo(one) == 0)
                {
                    return true;
                }
            }

            return false;
        } // method IsOneOf

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
            => index < 0 || index >= items.Count ? defaultValue : items[index];

        /// <summary>
        /// Безопасный доступ по индексу.
        /// </summary>
        [Pure]
        public static T? SafeAt<T>
            (
                this T?[] items,
                int index,
                T? defaultValue = default
            )
            => index < 0 || index >= items.Length ? defaultValue : items[index];

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
            where T: IComparable<T>
        {
            foreach (var one in items)
            {
                if (value.CompareTo(one) == 0)
                {
                    return true;
                }
            }

            return false;
        } // method IsOneOf

        /// <summary>
        /// Converts empty string to <c>null</c>.
        /// </summary>
        [Pure]
        public static string? EmptyToNull (this string? value) =>
            string.IsNullOrEmpty(value) ? null : value;

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
            if (!ReferenceEquals(value, null))
            {
                var sourceType = value.GetType();
                var targetType = typeof(T);

                if (ReferenceEquals(targetType, sourceType))
                {
                    return true;
                }

                if (targetType.IsAssignableFrom(sourceType))
                {
                    return true;
                }

                var convertible = value as IConvertible;
                if (!ReferenceEquals(convertible, null))
                {
                    return true; // ???
                }

                var converterFrom = TypeDescriptor.GetConverter(value);
                if (converterFrom.CanConvertTo(targetType))
                {
                    return true;
                }

                var converterTo = TypeDescriptor.GetConverter(targetType);
                if (converterTo.CanConvertFrom(sourceType))
                {
                    return true;
                }
            }

            return false;
        } // method CanConvertTo

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
            if (ReferenceEquals(value, null))
            {
                return default!;
            }

            var sourceType = value.GetType();
            var targetType = typeof(T);

            if (targetType == typeof(string))
            {
                return (T)(object)value.ToString()!;
            }

            if (targetType.IsAssignableFrom(sourceType))
            {
                return (T)value;
            }

            if (value is IConvertible)
            {
                return (T)Convert.ChangeType(value, targetType);
            }

            var converterFrom = TypeDescriptor.GetConverter(value);
            if (!ReferenceEquals(converterFrom, null)
                && converterFrom.CanConvertTo(targetType))
            {
                return (T)converterFrom.ConvertTo
                            (
                                value,
                                targetType
                            );
            }

            TypeConverter converterTo = TypeDescriptor.GetConverter(targetType);
            if (!ReferenceEquals(converterTo, null)
                && converterTo.CanConvertFrom(sourceType))
            {
                return (T)converterTo.ConvertFrom(value);
            }

            throw new ArsMagnaException();
        } // method ConvertTo

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
            if (value is bool)
            {
                return (bool)value;
            }

            try
            {
                var result = bool.Parse(value as string ?? false.ToString());

                return result;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                // Pass through
            }

            var svalue = value as string;
            if (!ReferenceEquals(svalue, null))
            {
                svalue = svalue.ToLowerInvariant();

                if (svalue == "false"
                    || svalue == "0"
                    || svalue == "no"
                    || svalue == "n"
                    || svalue == "off"
                    || svalue == "negative"
                    || svalue == "neg"
                    || svalue == "disabled"
                    || svalue == "incorrect"
                    || svalue == "wrong"
                    || svalue == "нет"
                )
                {
                    return false;
                }

                if (svalue == "true"
                    || svalue == "1"
                    || svalue == "yes"
                    || svalue == "y"
                    || svalue == "on"
                    || svalue == "positiva"
                    || svalue == "pos"
                    || svalue == "enabled"
                    || svalue == "correct"
                    || svalue == "right"
                    || svalue == "да"
                )
                {
                    return true;
                }
            }

            if (value is IConvertible)
            {
                return Convert.ToBoolean(value);
            }

            var converterFrom = TypeDescriptor.GetConverter(value);
            if (!ReferenceEquals(converterFrom, null)
                && converterFrom.CanConvertTo(typeof(bool)))
            {
                return (bool)converterFrom.ConvertTo
                    (
                        value,
                        typeof(bool)
                    );
            }

            Magna.Error
                (
                    nameof(Utility) + "::" + nameof(ToBoolean)
                    + "bad value="
                    + value
                );

            throw new FormatException
                (
                    "Bad value " + value
                );
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise
            (
                this EventHandler? handler,
                object? sender,
                EventArgs args
            )
        {
            handler?.Invoke(sender, args);
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
            handler?.Invoke(sender, args);
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
            handler?.Invoke(sender, null!);
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
            handler?.Invoke(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise<T>
            (
                this EventHandler<T>? handler
            )
            where T : EventArgs
        {
            handler?.Invoke(null, null!);
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static Task RaiseAsync (this EventHandler? handler, object? sender,
            EventArgs args) =>
            Task.Factory
                .StartNew (() => { handler?.Invoke(sender, args); });

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static Task RaiseAsync (this EventHandler? handler, object? sender) =>
            Task.Factory
                .StartNew (() => { handler?.Invoke(sender, EventArgs.Empty); });

        /// <summary>
        /// Is zero-length time span?
        /// </summary>
        [Pure]
        public static bool IsZero(this TimeSpan timeSpan)
            => TimeSpan.Compare(timeSpan, TimeSpan.Zero) == 0;

        /// <summary>
        /// Is zero-length or less?
        /// </summary>
        [Pure]
        public static bool IsZeroOrLess(this TimeSpan timeSpan)
            => TimeSpan.Compare(timeSpan, TimeSpan.Zero) <= 0;

        /// <summary>
        /// Is length of the time span less than zero?
        /// </summary>
        [Pure]
        public static bool LessThanZero(this TimeSpan timeSpan)
            => TimeSpan.Compare(timeSpan, TimeSpan.Zero) < 0;

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
                    span.Hours + span.Days * 60,
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
        public static string ToSecondString
            (
                this TimeSpan span
            )
        {
            return span.TotalSeconds.ToString
                (
                    "F2",
                    CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Converts time span using format 's'
        /// </summary>
        [Pure]
        public static string ToWholeSecondsString
            (
                this TimeSpan span
            )
        {
            return span.TotalSeconds.ToString
                (
                    "F0",
                    CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Mangle given text with the escape character.
        /// </summary>
        [Pure]
        public static string? Mangle
            (
                string? text,
                char escape,
                char[] badCharacters
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var result = new StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (badCharacters.Contains(c) || c == escape)
                {
                    result.Append(escape);
                }

                result.Append(c);
            }

            return result.ToString();
        } // method Mangle

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
            for (var i = 0; ; i++)
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
        } // method CompareSpans

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

            text = text.Replace("\r\n", "\n");
            var result = text.Split('\n');

            return result;
        }

        /// <summary>
        /// Содержит ли строка указанную подстроку?
        /// </summary>
        public static bool SafeContains (this string? text, string? subtext) =>
            !string.IsNullOrEmpty(text)
            && !string.IsNullOrEmpty(subtext)
            && text.Contains(subtext);

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
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(subtext1) && text.Contains(subtext1))
            {
                return true;
            }

            return !string.IsNullOrEmpty(subtext2) && text.Contains(subtext2);
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
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(subtext1) && text.Contains(subtext1))
            {
                return true;
            }

            if (!string.IsNullOrEmpty(subtext2) && text.Contains(subtext2))
            {
                return true;
            }

            return !string.IsNullOrEmpty(subtext3) && text.Contains(subtext3);
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
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            foreach (var subtext in subtexts)
            {
                if (!string.IsNullOrEmpty(subtext) && text.Contains(subtext))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Optimal degree of parallelism.
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
        } // property OptmimalParallelism

        /// <summary>
        /// Выбирает первую не пустую среди перечисленных строк.
        /// </summary>
        public static string NonEmpty
            (
                string? first,
                string? second
            )
            => !string.IsNullOrEmpty(first) ? first
                : !string.IsNullOrEmpty(second) ? second
                : throw new ArgumentNullException();

        /// <summary>
        /// Выбирает первую не пустую среди перечисленных строк.
        /// </summary>
        public static string NonEmpty
            (
                string? first,
                string? second,
                string? third
            )
            => !string.IsNullOrEmpty(first) ? first
                : !string.IsNullOrEmpty(second) ? second
                : !string.IsNullOrEmpty(third) ? third
                : throw new ArgumentNullException();

        /// <summary>
        /// Выбирает первую не пустую среди перечисленных строк.
        /// </summary>
        public static string NonEmpty
            (
                string? first,
                string? second,
                string? third,
                string? fourth
            )
            => !string.IsNullOrEmpty(first) ? first
                : !string.IsNullOrEmpty(second) ? second
                : !string.IsNullOrEmpty(third) ? third
                : !string.IsNullOrEmpty(fourth) ? fourth
                : throw new ArgumentNullException();

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
                if (!string.IsNullOrEmpty(one))
                {
                    return one;
                }
            }

            throw new ArgumentNullException();
        }

        /// <summary>
        /// Выбирает первый не пустой спан среди перечисленных.
        /// </summary>
        public static ReadOnlySpan<T> NonEmpty<T>
            (
                ReadOnlySpan<T> first,
                ReadOnlySpan<T> second
            )
            => !first.IsEmpty ? first
                : !second.IsEmpty ? second
                : throw new ArgumentOutOfRangeException();

        /// <summary>
        /// Выбирает первый не пустой спан среди перечисленных.
        /// </summary>
        public static ReadOnlySpan<T> NonEmpty<T>
            (
                ReadOnlySpan<T> first,
                ReadOnlySpan<T> second,
                ReadOnlySpan<T> third
            )
            => !first.IsEmpty ? first
                : !second.IsEmpty ? second
                : !third.IsEmpty ? third
                : throw new ArgumentOutOfRangeException();

        /// <summary>
        /// Выбирает первый не пустой спан среди перечисленных.
        /// </summary>
        public static ReadOnlySpan<T> NonEmpty<T>
            (
                ReadOnlySpan<T> first,
                ReadOnlySpan<T> second,
                ReadOnlySpan<T> third,
                ReadOnlySpan<T> fourth
            )
            =>
                !first.IsEmpty ? first
                : !second.IsEmpty ? second
                : !third.IsEmpty ? third
                : !fourth.IsEmpty ? fourth
                : throw new ArgumentOutOfRangeException();

        /// <summary>
        /// Удаляет в строке начальный и конечный символ кавычек.
        /// </summary>
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
                    text = text.Substring(1, length - 2);
                }
            }

            return text;
        } // method Unquote

        /// <summary>
        /// Упрощенное получение информации о методе.
        /// </summary>
        [Pure]
        public static MethodInfo GetMethodInfo(Expression<Action> expression)
            => ((MethodCallExpression) expression.Body).Method;

        /// <summary>
        /// Упрощенное получение информации о методе.
        /// </summary>
        [Pure]
        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
            => ((MethodCallExpression) expression.Body).Method;

        /// <summary>
        /// Упрощенное получение информации о методе.
        /// </summary>
        [Pure]
        public static MethodInfo GetMethodInfo<T1, T2>(Expression<Action<T1, T2>> expression)
            => ((MethodCallExpression) expression.Body).Method;

        /// <summary>
        /// Упрощенное получение информации о методе.
        /// </summary>
        [Pure]
        public static MethodInfo GetMethodInfo<T1, T2, T3>(Expression<Action<T1, T2, T3>> expression)
            => ((MethodCallExpression) expression.Body).Method;

        /// <summary>
        /// Упрощенное получение информации о методе.
        /// </summary>
        [Pure]
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4>(Expression<Action<T1, T2, T3, T4>> expression)
            => ((MethodCallExpression) expression.Body).Method;

        /// <summary>
        /// Упрощенное получение информации о методе.
        /// </summary>
        [Pure]
        public static MethodInfo GetMethodInfo<T>(Expression<Func<T>> expression)
            => ((MethodCallExpression) expression.Body).Method;

        /// <summary>
        /// Упрощенное получение информации о методе.
        /// </summary>
        [Pure]
        public static MethodInfo GetMethodInfo<T1, T2>(Expression<Func<T1, T2>> expression)
            => ((MethodCallExpression) expression.Body).Method;

        /// <summary>
        /// Упрощенное получение информации о методе.
        /// </summary>
        [Pure]
        public static MethodInfo GetMethodInfo<T1, T2, T3>(Expression<Func<T1, T2, T3>> expression)
            => ((MethodCallExpression) expression.Body).Method;

        /// <summary>
        /// Упрощенное получение информации о методе.
        /// </summary>
        [Pure]
        public static MethodInfo GetMethodInfo<T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4>> expression)
            => ((MethodCallExpression) expression.Body).Method;

        #endregion

    } // class Utility

} // namespace AM
