// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Utility.cs -- сборник простых вспомогательных методов.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
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
        /// Gets the date of next month first day.
        /// </summary>
        /// <value>Next month first day.</value>
        public static DateTime NextMonth => ThisMonth.AddMonths(1);

        /// <summary>
        /// Gets the date of next year first day.
        /// </summary>
        /// <value>Next year first day.</value>
        public static DateTime NextYear => ThisYear.AddYears(1);

        /// <summary>
        /// Gets the date of previous month first day.
        /// </summary>
        /// <value>Previous month first day.</value>
        public static DateTime PreviousMonth => ThisMonth.AddMonths(-1);

        /// <summary>
        /// Gets the date of previous year first day.
        /// </summary>
        /// <value>Previous year first day.</value>
        public static DateTime PreviousYear => ThisYear.AddYears(-1);

        /// <summary>
        /// Gets the date of current month first day.
        /// </summary>
        /// <value>Current month first day.</value>
        public static DateTime ThisMonth
        {
            get
            {
                var today = PlatformAbstractionLayer.Current.Today();

                return new DateTime(today.Year, today.Month, 1);
            }
        }

        /// <summary>
        /// Gets the date of current year first day.
        /// </summary>
        /// <value>Current year first day.</value>
        public static DateTime ThisYear => new DateTime
            (
                PlatformAbstractionLayer.Current.Today().Year,
                1,
                1
            );

        /// <summary>
        /// Gets the date for tomorrow.
        /// </summary>
        /// <value>Tomorrow date.</value>
        public static DateTime Tomorrow => PlatformAbstractionLayer.Current.Today().AddDays(1.0);

        /// <summary>
        /// Gets the for yesterday.
        /// </summary>
        /// <value>Yesterday date.</value>
        public static DateTime Yesterday => PlatformAbstractionLayer.Current.Today().AddDays(-1.0);

        /// <summary>
        /// One day.
        /// </summary>
        public static TimeSpan OneDay => new TimeSpan(1, 0, 0, 0);

        /// <summary>
        /// One hour.
        /// </summary>
        public static TimeSpan OneHour => new TimeSpan(1, 0, 0);

        /// <summary>
        /// One minute.
        /// </summary>
        public static TimeSpan OneMinute => new TimeSpan(0, 1, 0);

        /// <summary>
        /// One second.
        /// </summary>
        public static TimeSpan OneSecond => new TimeSpan(0, 0, 1);

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
        }

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
        }

        #endregion

        #region Private members

        private static Encoding? _cp866, _windows1251;

        #endregion

        #region Public methods

        /// <summary>
        /// Is digit from 0 to 9?
        /// </summary>
        public static bool IsArabicDigit
            (
                this char c
            )
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        /// Is letter from A to Z or a to z?
        /// </summary>
        public static bool IsLatinLetter
            (
                this char c
            )
        {
            return c >= 'A' && c <= 'Z'
                   || c >= 'a' && c <= 'z';
        }

        /// <summary>
        /// Is digit from 0 to 9
        /// or letter from A to Z or a to z?
        /// </summary>
        public static bool IsLatinLetterOrArabicDigit
            (
                this char c
            )
        {
            return c >= '0' && c <= '9'
                   || c >= 'A' && c <= 'Z'
                   || c >= 'a' && c <= 'z';
        }

        /// <summary>
        /// Is letter from А to Я or а to я?
        /// </summary>
        public static bool IsRussianLetter
            (
                this char c
            )
        {
            return c >= 'А' && c <= 'я'
                   || c == 'Ё' || c == 'ё';
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
            //Code.NotNullNorEmpty(fileName, "fileName");
            //Code.NotNull(encoding, "encoding");

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
        }

        /// <summary>
        /// Переключение кодовой страницы вывода консоли.
        /// </summary>
        public static void SetOutputCodePage
            (
                Encoding encoding
            )
        {
            // Code.NotNull(encoding, "encoding");

            StreamWriter stdOutput = new StreamWriter
            (
                Console.OpenStandardOutput(),
                encoding
            )
            {
                AutoFlush = true
            };
            Console.SetOut(stdOutput);

            StreamWriter stdError = new StreamWriter
            (
                Console.OpenStandardError(),
                encoding
            )
            {
                AutoFlush = true
            };
            Console.SetError(stdError);
        }

        /// <summary>
        /// Переключение кодовой страницы вывода консоли.
        /// </summary>
        public static void SetOutputCodePage
            (
                int codePage
            )
        {
            SetOutputCodePage
            (
                Encoding.GetEncoding(codePage)
            );
        }

        /// <summary>
        /// Detect AppVeyor CI environment.
        /// </summary>
        public static bool DetectAppVeyor()
        {
            return Environment.GetEnvironmentVariable("APPVEYOR") == "True";
        }

        /// <summary>
        /// Detect generic CI environment.
        /// </summary>
        public static bool DetectCI()
        {
            return Environment.GetEnvironmentVariable("CI") == "True";
        }

        /// <summary>
        /// Detect generic Travis CI environment.
        /// </summary>
        public static bool DetectTravis()
        {
            return Environment.GetEnvironmentVariable("TRAVIS") == "True";
        }

        /// <summary>
        /// Бросает исключение, если переданное значение равно <c>null</c>,
        /// иначе просто возвращает его.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [Pure]
        [DebuggerStepThrough]
        public static T ThrowIfNull<T>
            (
                this T? value,
                string message
            )
            where T : class
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
        }

        /// <summary>
        /// Бросает исключение, если переданное значение равно <c>null</c>,
        /// иначе просто возвращает его.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        [DebuggerStepThrough]
        public static T ThrowIfNull<T>
            (
                this T? value
            )
            where T : class
        {
            return ThrowIfNull<T>
                (
                    value,
                    "Null value detected"
                );
        }

        /// <summary>
        /// Превращает объект в видимую строку.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public static string ToVisibleString<T>
            (
                this T? value
            )
            where T : class
        {
            var result = value?.ToString();

            if (ReferenceEquals(result, null))
            {
                return "(null)";
            }

            return result;
        }

        /// <summary>
        /// Сравнивает две строки независимо от текущей культуры.
        /// </summary>
        public static bool EqualInvariant
            (
                string? left,
                string? right
            )
        {
            return CultureInfo.InvariantCulture.CompareInfo.Compare
                (
                    left,
                    right
                ) == 0;
        }

        /// <summary>
        /// Gets the first char of the text.
        /// </summary>
        public static char FirstChar
            (
                this string? text
            )
        {
            return string.IsNullOrEmpty(text)
                ? '\0'
                : text[0];
        }

        /// <summary>
        /// Gets the last char of the text.
        /// </summary>
        public static char LastChar
            (
                this string? text
            )
        {
            return string.IsNullOrEmpty(text)
                ? '\0'
                : text[^1];
        }

        /// <summary>
        /// Сравнивает символы с точностью до регистра.
        /// </summary>
        /// <param name="one">Первый символ.</param>
        /// <param name="two">Второй символ.</param>
        /// <returns>Символы совпадают с точностью до регистра.</returns>
        public static bool SameChar
            (
                this char one,
                char two
            )
        {
            return char.ToUpperInvariant(one) == char.ToUpperInvariant(two);
        }

        /// <summary>
        /// Сравнивает строки с точностью до регистра.
        /// </summary>
        /// <param name="one">Первая строка.</param>
        /// <param name="two">Вторая строка.</param>
        /// <returns>Строки совпадают с точностью до регистра.</returns>
        public static bool SameString
            (
                this string? one,
                string? two
            )
        {
            return string.Compare
                (
                    one,
                    two,
                    StringComparison.OrdinalIgnoreCase
                ) == 0;
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString
            (
                this short value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString
            (
                this ushort value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString
            (
                this int value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString
            (
                this uint value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString
            (
                this long value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Преобразование числа в строку по правилам инвариантной
        /// (не зависящей от региона) культуры.
        /// </summary>
        /// <param name="value">Число для преобразования.</param>
        /// <returns>Строковое представление числа.</returns>
        public static string ToInvariantString
            (
                this ulong value
            )
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Register required encoding providers.
        /// </summary>
        public static void RegisterEncodingProviders()
        {
            Encoding.RegisterProvider
                (
                    CodePagesEncodingProvider.Instance
                );
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
            if (string.IsNullOrEmpty(text))
            {
                return defaultValue;
            }

            if (!int.TryParse(text, out var result))
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
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            if (!int.TryParse(text, out var result))
            {
                result = 0;
            }

            return result;
        }

        /// <summary>
        /// Determines whether is one of the specified values.
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
                if (value.CompareTo(one) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts empty string to <c>null</c>.
        /// </summary>
        public static string? EmptyToNull
            (
                this string? value
            )
        {
            return string.IsNullOrEmpty(value)
                ? null
                : value;
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
            if (!ReferenceEquals(value, null))
            {
                Type sourceType = value.GetType();
                Type targetType = typeof(T);

                if (ReferenceEquals(targetType, sourceType))
                {
                    return true;
                }

                if (targetType.IsAssignableFrom(sourceType))
                {
                    return true;
                }

                IConvertible? convertible = value as IConvertible;
                if (!ReferenceEquals(convertible, null))
                {
                    return true; // ???
                }

                TypeConverter converterFrom = TypeDescriptor.GetConverter(value);
                if (converterFrom.CanConvertTo(targetType))
                {
                    return true;
                }

                TypeConverter converterTo = TypeDescriptor.GetConverter(targetType);
                if (converterTo.CanConvertFrom(sourceType))
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
        public static Task RaiseAsync
            (
                this EventHandler? handler,
                object? sender,
                EventArgs args
            )
        {
            Task result = Task.Factory.StartNew
                (
                    () =>
                    {
                        handler?.Invoke(sender, args);
                    }
                );

            return result;
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
            Task result = Task.Factory.StartNew
                (
                    () =>
                    {
                        handler?.Invoke(sender, EventArgs.Empty);
                    }
                );

            return result;
        }

        /// <summary>
        /// Is zero-length time span?
        /// </summary>
        public static bool IsZero(this TimeSpan timeSpan)
            => TimeSpan.Compare(timeSpan, TimeSpan.Zero) == 0;

        /// <summary>
        /// Is zero-length or less?
        /// </summary>
        public static bool IsZeroOrLess(this TimeSpan timeSpan)
            => TimeSpan.Compare(timeSpan, TimeSpan.Zero) <= 0;

        /// <summary>
        /// Is length of the time span less than zero?
        /// </summary>
        public static bool LessThanZero(this TimeSpan timeSpan)
            => TimeSpan.Compare(timeSpan, TimeSpan.Zero) < 0;

        /// <summary>
        /// Converts time span to string
        /// automatically selecting format
        /// according duration of the span.
        /// </summary>
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

        #endregion
    }
}
