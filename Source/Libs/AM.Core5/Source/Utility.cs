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

using AM.Collections;
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

        private static readonly char[] _whitespace = { ' ', '\t', '\r', '\n' };

        #endregion

        #region Public methods

                /// <summary>
        /// Состоит ли строка только из указанного символа.
        /// </summary>
        public static bool ConsistOf
            (
                this string? value,
                char c
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (char c1 in value)
            {
                if (c1 != c)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Состоит ли строка только из указанных символов.
        /// </summary>
        public static bool ConsistOf
            (
                this string? value,
                params char[] array
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (char c in value)
            {
                if (Array.IndexOf(array, c) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Определяет, состоит ли строка только из цифр.
        /// </summary>
        public static bool ConsistOfDigits
            (
                this string? value,
                int startIndex,
                int endIndex
            )
        {
            if (string.IsNullOrEmpty(value)
                || startIndex >= value.Length)
            {
                return false;
            }

            endIndex = Math.Min(endIndex, value.Length);
            for (int i = startIndex; i < endIndex; i++)
            {
                if (!char.IsDigit(value[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Определяет, состоит ли строка только из цифр.
        /// </summary>
        public static bool ConsistOfDigits
            (
                this string? value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Содержит ли строка любой из перечисленных символов.
        /// </summary>
        public static bool ContainsAnySymbol
            (
                this string? text,
                params char[] symbols
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char c in text)
                {
                    if (symbols.Contains(c))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines whether the text contains specified character.
        /// </summary>
        /// <remarks>
        /// For portable library.
        /// </remarks>
        public static bool ContainsCharacter
            (
                this string? text,
                char symbol
            )
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (char c in text)
                {
                    if (c == symbol)
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
        public static bool ContainsWhitespace
            (
                this string? text
            )
        {
            return text.ContainsAnySymbol(_whitespace);
        }

        /// <summary>
        /// Count of given substrings in the text.
        /// </summary>
        public static int CountSubstrings
            (
                this string? text,
                string? substring
            )
        {
            int result = 0;

            if (!string.IsNullOrEmpty(text)
                && !string.IsNullOrEmpty(substring))
            {
                int length = substring.Length;
                int offset = 0;

                while (true)
                {
                    int index = text.IndexOf
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
        public static bool ContainsNoCase
            (
                this IEnumerable<string> lines,
                string line
            )
        {
            foreach (string one in lines)
            {
                if (SameString(one, line))
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
            if (string.IsNullOrEmpty(text))
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

            var result = text.Substring(offset, width);

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

            var result = text.Slice(offset, width);

            return result;
        }

        /// <summary>
        /// Changes the encoding of given string from one to other.
        /// </summary>
        public static string ChangeEncoding
            (
                Encoding fromEncoding,
                Encoding toEncoding,
                string value
            )
        {
            if (fromEncoding.Equals(toEncoding))
            {
                return value;
            }

            byte[] bytes = fromEncoding.GetBytes(value);
            string result = toEncoding.GetString (bytes);

            return result;
        }

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
            {
                Magna.Error
                    (
                        nameof(Utility) + "::" + nameof(ThrowIfEmpty)
                        + ": "
                        + message
                    );

                throw new ArgumentException (message);
            }

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
                Magna.Error
                    (
                        nameof(Utility) + "::" + nameof(ThrowIfEmpty)
                        + ": "
                        + message
                    );

                throw new ArgumentException (message);
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
        /// Бросает исключение, если переданная строка пустая
        /// или равна <c>null</c>.
        /// </summary>
        public static string ThrowIfNullOrEmpty(this string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException();
            }

            return value!;
        }

        /// <summary>
        /// Бросает исключение, если переданная строка пустая
        /// или равна <c>null</c>.
        /// </summary>
        public static ReadOnlySpan<char> ThrowIfNullOrEmpty(this ReadOnlySpan<char> value)
        {
            if (value.IsEmpty)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если переданная строка пробельная
        /// или равна <c>null</c>.
        /// </summary>
        public static string ThrowIfNullOrWhiteSpace
            (
                this string? value
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException();
            }

            return value!;
        }

        /// <summary>
        /// Бросает исключение, если переданная строка пробельная
        /// или равна <c>null</c>.
        /// </summary>
        public static string ThrowIfNullOrWhiteSpace
            (
                this string? value,
                string name
            )
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(name);
            }

            return value!;
        }

        /// <summary>
        /// Бросает исключение, если переданная строка пробельная
        /// или равна <c>null</c>.
        /// </summary>
        public static ReadOnlySpan<char> ThrowIfNullOrWhiteSpace
            (
                this ReadOnlySpan<char> value
            )
        {
            if (value.IsEmpty || value.IsWhiteSpace())
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если переданная строка пробельная
        /// или равна <c>null</c>.
        /// </summary>
        public static ReadOnlySpan<char> ThrowIfNullOrWhiteSpace
            (
                this ReadOnlySpan<char> value,
                string name
            )
        {
            if (value.IsEmpty || value.IsWhiteSpace())
            {
                throw new ArgumentException(name);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static int ThrowIfZero
            (
                this int value
            )
        {
            if (value == 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static int ThrowIfZero
            (
                this int value,
                string message
            )
        {
            if (value == 0)
            {
                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static uint ThrowIfZero
            (
                this uint value
            )
        {
            if (value == 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static uint ThrowIfZero
            (
                this uint value,
                string message
            )
        {
            if (value == 0)
            {
                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static short ThrowIfZero
            (
                this short value
            )
        {
            if (value == 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static short ThrowIfZero
            (
                this short value,
                string message
            )
        {
            if (value == 0)
            {
                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static ushort ThrowIfZero
            (
                this ushort value
            )
        {
            if (value == 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static ushort ThrowIfZero
            (
                this ushort value,
                string message
            )
        {
            if (value == 0)
            {
                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static long ThrowIfZero
            (
                this long value
            )
        {
            if (value == 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static long ThrowIfZero
            (
                this long value,
                string message
            )
        {
            if (value == 0)
            {
                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static ulong ThrowIfZero
            (
                this ulong value
            )
        {
            if (value == 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static ulong ThrowIfZero
            (
                this ulong value,
                string message
            )
        {
            if (value == 0)
            {
                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static double ThrowIfZero
            (
                this double value
            )
        {
            if (value == 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static double ThrowIfZero
            (
                this double value,
                string message
            )
        {
            if (value == 0)
            {
                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static decimal ThrowIfZero
            (
                this decimal value
            )
        {
            if (value == 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число равно нулю.
        /// </summary>
        public static decimal ThrowIfZero
            (
                this decimal value,
                string message
            )
        {
            if (value == 0)
            {
                throw new ArgumentException(message);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static int ThrowIfNegative
            (
                this int value
            )
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static int ThrowIfNegative
            (
                this int value,
                string name
            )
        {
            if (value < 0)
            {
                throw new ArgumentException(name);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static short ThrowIfNegative
            (
                this short value
            )
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static short ThrowIfNegative
            (
                this short value,
                string name
            )
        {
            if (value < 0)
            {
                throw new ArgumentException(name);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static long ThrowIfNegative
            (
                this long value
            )
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static long ThrowIfNegative
            (
                this long value,
                string name
            )
        {
            if (value < 0)
            {
                throw new ArgumentException(name);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static double ThrowIfNegative
            (
                this double value
            )
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static double ThrowIfNegative
            (
                this double value,
                string name
            )
        {
            if (value < 0)
            {
                throw new ArgumentException(name);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static decimal ThrowIfNegative
            (
                this decimal value
            )
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число отрицательное.
        /// </summary>
        public static decimal ThrowIfNegative
            (
                this decimal value,
                string name
            )
        {
            if (value < 0)
            {
                throw new ArgumentException(name);
            }

            return value;
        }

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static int ThrowIfOutOfTheRange
            (
                this int value,
                int minimum,
                int maximum
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException();
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static int ThrowIfOutOfTheRange
            (
                this int value,
                int minimum,
                int maximum,
                string name
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException(name);
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static uint ThrowIfOutOfTheRange
            (
                this uint value,
                uint minimum,
                uint maximum
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException();
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static uint ThrowIfOutOfTheRange
            (
                this uint value,
                uint minimum,
                uint maximum,
                string name
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException(name);
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static short ThrowIfOutOfTheRange
            (
                this short value,
                short minimum,
                short maximum
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException();
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static short ThrowIfOutOfTheRange
            (
                this short value,
                short minimum,
                short maximum,
                string name
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException(name);
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static ushort ThrowIfOutOfTheRange
            (
                this ushort value,
                ushort minimum,
                ushort maximum
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException();
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static ushort ThrowIfOutOfTheRange
            (
                this ushort value,
                ushort minimum,
                ushort maximum,
                string name
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException(name);
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static long ThrowIfOutOfTheRange
            (
                this long value,
                long minimum,
                long maximum
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException();
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static long ThrowIfOutOfTheRange
            (
                this long value,
                long minimum,
                long maximum,
                string name
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException(name);
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static ulong ThrowIfOutOfTheRange
            (
                this ulong value,
                ulong minimum,
                ulong maximum
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException();
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static ulong ThrowIfOutOfTheRange
            (
                this ulong value,
                ulong minimum,
                ulong maximum,
                string name
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException(name);
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static double ThrowIfOutOfTheRange
            (
                this double value,
                double minimum,
                double maximum
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException();
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static double ThrowIfOutOfTheRange
            (
                this double value,
                double minimum,
                double maximum,
                string name
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException(name);
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static decimal ThrowIfOutOfTheRange
            (
                this decimal value,
                decimal minimum,
                decimal maximum
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException();
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Бросает исключение, если число не попадает в указанный интервал.
        /// </summary>
        // ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
        public static decimal ThrowIfOutOfTheRange
            (
                this decimal value,
                decimal minimum,
                decimal maximum,
                string name
            )
        {
            if (value < minimum || value > maximum)
            {
                throw new ArgumentException(name);
            }

            return value;
        }
        // ReSharper restore ParameterOnlyUsedForPreconditionCheck.Global

        /// <summary>
        /// Сравнение двух кусков памяти.
        /// </summary>
        public static int CompareOrdinal
            (
                ReadOnlyMemory<char> first,
                ReadOnlyMemory<char> second
            )
        {
            for (var i = 0; ; i++)
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
        } // method CompareOrdinal

        /// <summary>
        /// Посимвольное сравнение двух кусков памяти.
        /// </summary>
        public static int CompareOrdinal
            (
                ReadOnlySpan<char> first,
                ReadOnlySpan<char> second
            )
        {
            for (var i = 0; ; i++)
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
        } // method CompareOrdinal

        /// <summary>
        /// Превращает объект в видимую строку.
        /// </summary>
        [Pure]
        public static string ToVisibleString<T> (this T? value) where T: class
            => value?.ToString() ?? "(null)";

        /// <summary>
        /// Превращает фрагмент памяти в видимую строку.
        /// </summary>
        [Pure]
        public static string ToVisibleString (this ReadOnlyMemory<char> value)
            => value.IsEmpty ? "(empty)" : value.ToString();

        /// <summary>
        /// Превращает фрагмент памяти в видимую строку.
        /// </summary>
        [Pure]
        public static string ToVisibleString (this ReadOnlySpan<char> value)
            => value.IsEmpty ? "(empty)" : value.ToString();

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
        public static char FirstChar(this ReadOnlyMemory<char> text) =>
            text.Length == 0 ? '\0' : text.Span[0];

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
        public static bool SameString(this ReadOnlyMemory<char> one, string? two) =>
            one.Span.CompareTo(two.AsSpan(), StringComparison.OrdinalIgnoreCase) == 0;

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
        /// <returns>Строки совпадают с точностью до регистра?</returns>
        [Pure]
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
        /// <returns>Строки совпадают?</returns>
        [Pure]
        public static bool SameStringSensitive
            (
                this ReadOnlyMemory<char> one,
                ReadOnlyMemory<char> two
            )
        {
            return one.Span.CompareTo(two.Span, StringComparison.Ordinal) == 0;
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

            if (!int.TryParse(text.Span, out var result))
            {
                result = 0;
            }

            return result;
        } // method SafeToInt32

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

            if (!int.TryParse(text, out var result))
            {
                result = 0;
            }

            return result;
        } // method SafeToInt32

        /// <summary>
        /// Безопасное преобразование строки в целое.
        /// </summary>
        public static long SafeToInt64
            (
                this string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            if (!long.TryParse(text, out var result))
            {
                result = 0;
            }

            return result;
        } // method SafeToInt64

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

            if (!long.TryParse(text.Span, out var result))
            {
                result = 0;
            }

            return result;
        } // method SafeToInt64

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

            if (!long.TryParse(text, out var result))
            {
                result = 0;
            }

            return result;
        } // method SafeToInt64

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
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }

            if (!TryParseDouble(text, out var result))
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

            if (!TryParseDouble(text.Span, out var result))
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

            if (!TryParseDouble(text, out var result))
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
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }

            if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
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

            if (!decimal.TryParse(text.Span, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
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

            if (!decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static short ParseInt16(this string text) =>
            short.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static short ParseInt16(this ReadOnlyMemory<char> text) =>
            short.Parse(text.Span, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static short ParseInt16(this ReadOnlySpan<char> text) =>
            short.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static int ParseInt32(this string text) =>
            int.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static int ParseInt32(this ReadOnlyMemory<char> text) =>
            int.Parse(text.Span, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static int ParseInt32(this ReadOnlySpan<char> text) =>
            int.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static long ParseInt64(this string text) =>
            long.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static long ParseInt64(this ReadOnlyMemory<char> text) =>
            long.Parse(text.Span, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static long ParseInt64(this ReadOnlySpan<char> text) =>
            long.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static ushort ParseUInt16(this string text) =>
            ushort.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static ushort ParseUInt16(this ReadOnlySpan<char> text) =>
            ushort.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static uint ParseUInt32(this string text) =>
            uint.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static uint ParseUInt32(this ReadOnlySpan<char> text) =>
            uint.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static ulong ParseUInt64(this string text) =>
            ulong.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static ulong ParseUInt64(this ReadOnlySpan<char> text) =>
            ulong.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static float ParseSingle(this string text) =>
            float.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static float ParseSingle(this ReadOnlySpan<char> text) =>
            float.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static double ParseDouble(this string text) =>
            double.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static double ParseDouble(this ReadOnlySpan<char> text) =>
            double.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static decimal ParseDecimal(this string text) =>
            decimal.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для Parse.
        /// </summary>
        public static decimal ParseDecimal(this ReadOnlySpan<char> text) =>
            decimal.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseInt16(string? text, out short result) =>
            short.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseInt16(ReadOnlySpan<char> text, out short result) =>
            short.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseInt32(string? text, out int result) =>
            int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseInt32(ReadOnlySpan<char> text, out int result) =>
            int.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseInt64(string? text, out long result) =>
            long.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseInt64(ReadOnlySpan<char> text, out long result) =>
            long.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseUInt16(string? text, out ushort result) =>
            ushort.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseUInt16(ReadOnlySpan<char> text, out ushort result) =>
            ushort.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseUInt32(string? text, out uint result) =>
            uint.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseUInt32(ReadOnlySpan<char> text, out uint result) =>
            uint.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseUInt64(string? text, out ulong result) =>
            ulong.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseUInt64(ReadOnlySpan<char> text, out ulong result) =>
            ulong.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseDouble(string? text, out double result) =>
            double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseDouble(ReadOnlySpan<char> text, out double result) =>
            double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseSingle(string? text, out float result) =>
            float.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseSingle(ReadOnlySpan<char> text, out float result) =>
            float.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseDecimal(string? text, out decimal result) =>
            decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseDecimal(ReadOnlySpan<char> text, out decimal result) =>
            decimal.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseDateTime(string? text, out DateTime result) =>
            DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseDateTime(ReadOnlySpan<char> text, out DateTime result) =>
            DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseDateTime(string? text, string? format, out DateTime result) =>
            DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out result);

        /// <summary>
        /// Сокращение для TryParse.
        /// </summary>
        public static bool TryParseDateTime(ReadOnlySpan<char> text, ReadOnlySpan<char> format, out DateTime result) =>
            DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out result);

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
        /// Определяет, равен ли ли данный объект
        /// любому из перечисленных.
        /// </summary>
        [Pure]
        public static bool IsOneOf
            (
                this ReadOnlyMemory<char> value,
                string? first
            )
            => value.Span.CompareTo(first.AsSpan(), StringComparison.Ordinal) == 0;

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
            => value.Span.CompareTo(first.AsSpan(), StringComparison.Ordinal) == 0
            || value.Span.CompareTo(second.AsSpan(), StringComparison.Ordinal) == 0;

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
            => value.Span.CompareTo(first.AsSpan(), StringComparison.Ordinal) == 0
            || value.Span.CompareTo(second.AsSpan(), StringComparison.Ordinal) == 0
            || value.Span.CompareTo(third.AsSpan(), StringComparison.Ordinal) == 0;

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
                if (value.Span.CompareTo(one.AsSpan(), StringComparison.Ordinal) == 0)
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
                this T?[]? items,
                int index,
                T? defaultValue = default
            )
            => index < 0 || index >= (items?.Length ?? 0) ? defaultValue : items![index];

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
            if (value is bool retval1)
            {
                return retval1;
            }

            if (value is string text)
            {
                if (bool.TryParse(text, out var retval2))
                {
                    return retval2;
                }

                text = text.ToLowerInvariant();

                if (text == "false"
                    || text == "0"
                    || text == "no"
                    || text == "n"
                    || text == "off"
                    || text == "negative"
                    || text == "neg"
                    || text == "disabled"
                    || text == "incorrect"
                    || text == "wrong"
                    || text == "нет"
                )
                {
                    return false;
                }

                if (text == "true"
                    || text == "1"
                    || text == "yes"
                    || text == "y"
                    || text == "on"
                    || text == "positiva"
                    || text == "pos"
                    || text == "enabled"
                    || text == "correct"
                    || text == "right"
                    || text == "да"
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
                    "Bad value: " + value
                );
        }

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise<T> ( this EventHandler<T>? handler, object? sender, T args )
            where T : EventArgs
            => handler?.Invoke(sender, args);

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise<T> ( this EventHandler<T>? handler, object? sender )
            where T : EventArgs
            => handler?.Invoke(sender, null!);

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static void Raise ( this EventHandler? handler, object? sender ) =>
            handler?.Invoke(sender, EventArgs.Empty);

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static Task RaiseAsync(this EventHandler? handler, object? sender, EventArgs args)
        {
            if (handler is null)
            {
                return Task.CompletedTask;
            }

            return Task.Factory.StartNew(() => { handler.Invoke(sender, args); });
        } // method RaiseAsync

        /// <summary>
        /// Raises the specified handler.
        /// </summary>
        public static Task RaiseAsync(this EventHandler? handler, object? sender)
        {
            if (handler is null)
            {
                return Task.CompletedTask;
            }

            return Task.Factory.StartNew(() => { handler.Invoke(sender, EventArgs.Empty); });
        } // method RaiseAsync

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
            if (!string.IsNullOrEmpty(text))
            {
                text = text!.Trim();

                if (text.Length > length)
                {
                    return text.Substring(0, length) + "…";
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
        public static T? GetResultOrDefault<T>(this Task<T?> task) =>
            task.Status == TaskStatus.RanToCompletion ? task.Result : default;

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
        /// Выбирает первый не пустой спан среди перечисленных.
        /// </summary>
        public static ReadOnlyMemory<T> NonEmpty<T>
            (
                ReadOnlyMemory<T> first,
                ReadOnlyMemory<T> second
            )
            => !first.IsEmpty ? first
                : !second.IsEmpty ? second
                : throw new ArgumentOutOfRangeException();

        /// <summary>
        /// Выбирает первый не пустой спан среди перечисленных.
        /// </summary>
        public static ReadOnlyMemory<T> NonEmpty<T>
            (
                ReadOnlyMemory<T> first,
                ReadOnlyMemory<T> second,
                ReadOnlyMemory<T> third
            )
            => !first.IsEmpty ? first
                : !second.IsEmpty ? second
                : !third.IsEmpty ? third
                : throw new ArgumentOutOfRangeException();

        /// <summary>
        /// Выбирает первый не пустой спан среди перечисленных.
        /// </summary>
        public static ReadOnlyMemory<T> NonEmpty<T>
            (
                ReadOnlyMemory<T> first,
                ReadOnlyMemory<T> second,
                ReadOnlyMemory<T> third,
                ReadOnlyMemory<T> fourth
            )
            =>
                !first.IsEmpty ? first
                : !second.IsEmpty ? second
                : !third.IsEmpty ? third
                : !fourth.IsEmpty ? fourth
                : throw new ArgumentOutOfRangeException();

        /// <summary>
        /// Trim lines.
        /// </summary>
        public static IEnumerable<string> TrimLines
            (
                this IEnumerable<string?> lines
            )
        {
            foreach (var line in lines)
            {
                if (!ReferenceEquals(line, null))
                {
                    yield return line.Trim();
                }
            }
        } // method TrimLines

        /// <summary>
        /// Trim lines.
        /// </summary>
        public static IEnumerable<string> TrimLines
            (
                this IEnumerable<string?> lines,
                params char[] characters
            )
        {
            foreach (var line in lines)
            {
                if (!ReferenceEquals(line, null))
                {
                    yield return line.Trim(characters);
                }
            }
        } // method TrimLines

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
                    text = text.Substring(1, length - 2);
                }
            }

            return text;
        } // method Unquote

        /// <summary>
        /// Имя исполняемого процесса (с путями).
        /// </summary>
        public static string ExecutableFileName
        {
            get
            {
                var process = Process.GetCurrentProcess();
                var module = process.MainModule;

                return module?.FileName ?? throw new ApplicationException();
            }
        } // property ExecutableFileName

        private static readonly Random _random = new ();
        private const string RandomChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                            + "abcdefghijklmnopqrstuvwxyz";
        private const string RandomSymbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                            + "abcdefghijklmnopqrstuvwxyz"
                                            + "0123456789"
                                            + "_";

        /// <summary>
        /// Creates random string with given length.
        /// </summary>
        public static string RandomIdentifier
            (
                int length
            )
        {
            var result = new StringBuilder(length);

            if (length > 0)
            {
                result.Append(RandomChars[_random.Next(RandomChars.Length)]);
            }

            for (; length > 1; length--)
            {
                result.Append(RandomSymbols[_random.Next(RandomSymbols.Length)]);
            }

            return result.ToString();
        } // method RandomIdentifier

        /// <summary>
        /// Unwrap the <see cref="AggregateException"/>
        /// (or do nothing if not aggregate).
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
                                Magna.TraceException
                                    (
                                        "Utility::Unwrap",
                                        ex
                                    );

                                return true;
                            }
                    );

                return aggregate.InnerExceptions[0];
            }

            return exception;
        } // method Unwrap

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
                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Универсальное длинное представление даты/времени.
        /// </summary>
        [Pure]
        public static string ToLongUniformString(this DateTime dateTime) =>
            dateTime.ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// Универсальное короткое представление даты.
        /// </summary>
        [Pure]
        public static string ToShortUniformString(this DateTime dateTime) =>
            dateTime.ToString("yyyy-MM-dd");

        /// <summary>
        /// Начало эпохи UNIX.
        /// </summary>
        public static readonly DateTime UnixStart
            = new (1970, 1, 1);

        /// <summary>
        /// Переводит указанную дату в формат Unix.
        /// </summary>
        [Pure]
        public static long ToUnixTime (this DateTime dateTime) =>
            (long)(dateTime - UnixStart).TotalSeconds;

        /// <summary>
        /// Get very last index of substrings.
        /// </summary>
        public static int LastIndexOfAny
            (
                string text,
                string[] fragments
            )
        {
            var result = -1;
            foreach (var fragment in fragments)
            {
                var index = text.LastIndexOf(fragment, StringComparison.InvariantCulture);
                if (index > result)
                {
                    result = index;
                }
            }

            return result;
        } // method LastIndexOfAny

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

        /// <summary>
        /// Получение ссылочного перечислителя.
        /// </summary>
        [Pure]
        public static RefEnumerable<T> AsRefEnumerable<T>(this Span<T> data) => new (data);

        /// <summary>
        /// Получение ссылочного перечислителя.
        /// </summary>
        [Pure]
        public static RefEnumerable<T> AsRefEnumerable<T>(this T[] data) => new (data.AsSpan());

        #endregion

    } // class Utility

} // namespace AM
