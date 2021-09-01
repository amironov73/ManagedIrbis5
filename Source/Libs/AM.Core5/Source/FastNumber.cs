// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FastNumber.cs -- быстрые и грязные методы работы со строковым представлением чисел
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.CompilerServices;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Быстрые и грязные методы работы со строковым представлением чисел.
    /// Никак не учитывают текущую локаль. Всегда InvariantCulture.
    /// </summary>
    public static class FastNumber
    {
        #region Public methods

        // ==========================================================

        /// <summary>
        /// Преобразование 32-битного целого числа со знаком в строку.
        /// </summary>
        [SkipLocalsInit]
        public static unsafe string Int32ToString
            (
                int number
            )
        {
            const int Length = 11;

            var flag = number < 0;
            if (flag)
            {
                number = -number;
            }

            var buffer = stackalloc char[Length];
            var offset = Length - 1;
            for ( ; ; offset--)
            {
                number = Math.DivRem(number, 10, out var remainder);
                buffer[offset] = (char) ('0' + remainder);
                if (number == 0)
                {
                    break;
                }
            }

            if (flag)
            {
                buffer[--offset] = '-';
            }

            return new string(buffer, offset, Length - offset);

        } // method Int32ToString

        /// <summary>
        /// Преобразование 32-битного целого числа со знаком в строку.
        /// </summary>
        public static unsafe int Int32ToChars
            (
                int number,
                Span<char> buffer
            )
        {
            var flag = number < 0;
            if (flag)
            {
                number = -number;
            }

            fixed (char* start = buffer)
            {
                var length = 0;
                if (number == 0)
                {
                    *start = '0';
                    return 1;
                }

                var end = start;
                for (; number != 0;)
                {
                    number = Math.DivRem(number, 10, out var rem);
                    *end++ = (char) ('0' + rem);
                    ++length;
                }

                if (flag)
                {
                    *end++ = '-';
                    ++length;
                }

                var ptr1 = start;
                var ptr2 = end - 1;
                while (ptr1 < ptr2)
                {
                    var c = *ptr1;
                    *ptr1++ = *ptr2;
                    *ptr2-- = c;
                }

                return length;
            }

        } // method Int32ToChars

        /// <summary>
        /// Преобразование 32-битного целого числа со знаком в строку.
        /// </summary>
        public static unsafe char* Int32ToChars
            (
                int number,
                char *buffer
            )
        {
            var flag = number < 0;
            if (flag)
            {
                number = -number;
            }

            var start = buffer;
            var length = 0;
            if (number == 0)
            {
                *start = '0';
                return buffer + 1;
            }

            var end = start;
            for (; number != 0;)
            {
                number = Math.DivRem(number, 10, out var remainder);
                *end++ = (char) ('0' + remainder);
                ++length;
            }

            if (flag)
            {
                *end++ = '-';
                ++length;
            }

            var ptr1 = start;
            var ptr2 = end - 1;
            while (ptr1 < ptr2)
            {
                var c = *ptr1;
                *ptr1++ = *ptr2;
                *ptr2-- = c;
            }

            return buffer + length;

        } //hod Int32ToChars

        /// <summary>
        /// Преобразование 32-битного целого числа со знаком в строку.
        /// </summary>
        public static unsafe int Int32ToBytes
            (
                int number,
                Span<byte> buffer
            )
        {
            var flag = number < 0;
            if (flag)
            {
                number = -number;
            }

            fixed (byte* start = buffer)
            {
                var length = 0;
                if (number == 0)
                {
                    *start = (byte)'0';
                    return 1;
                }

                var end = start;
                for (; number != 0;)
                {
                    number = Math.DivRem(number, 10, out var rem);
                    *end++ = (byte) ('0' + rem);
                    ++length;
                }

                if (flag)
                {
                    *end++ = (byte)'-';
                    ++length;
                }

                var ptr1 = start;
                var ptr2 = end - 1;
                while (ptr1 < ptr2)
                {
                    var c = *ptr1;
                    *ptr1++ = *ptr2;
                    *ptr2-- = c;
                }

                return length;
            }

        } // method Int32ToBytes

        /// <summary>
        /// Преобразование целого числа в строку.
        /// </summary>
        public static unsafe byte* Int32ToBytes
            (
                int number,
                byte* buffer
            )
        {
            var flag = number < 0;
            if (flag)
            {
                number = -number;
            }

            var start = buffer;
            var length = 0;
            if (number == 0)
            {
                *start = (byte)'0';
                return buffer + 1;
            }

            var end = start;
            for (; number != 0;)
            {
                number = Math.DivRem(number, 10, out var rem);
                *end++ = (byte) ('0' + rem);
                ++length;
            }

            if (flag)
            {
                *end++ = (byte) '-';
                ++length;
            }

            var ptr1 = start;
            var ptr2 = end - 1;
            while (ptr1 < ptr2)
            {
                var c = *ptr1;
                *ptr1++ = *ptr2;
                *ptr2-- = c;
            }

            return buffer + length;

        } // method Int32ToBytes

        // ==========================================================

        /// <summary>
        /// Преобразование 64-битного целого числа со знаком в строку.
        /// </summary>
        [SkipLocalsInit]
        public static unsafe string Int64ToString
            (
                long number
            )
        {
            const int Length = 21;

            var flag = number < 0;
            if (flag)
            {
                number = -number;
            }

            var buffer = stackalloc char[Length];
            var offset = Length - 1;
            for ( ; ; offset--)
            {
                number = Math.DivRem(number, 10, out long remainder);
                buffer[offset] = (char) ('0' + remainder);
                if (number == 0)
                {
                    break;
                }
            }

            if (flag)
            {
                buffer[--offset] = '-';
            }

            return new string(buffer, offset, Length - offset);

        } // method Int64ToString

        // ==========================================================

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                string text
            )
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
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                string text,
                int offset,
                int length
            )
        {
            var result = 0;
            unchecked
            {
                fixed (char* ptr = text)
                {
                    for (; length > 0; length--, offset++)
                    {
                        result = result * 10 + ptr[offset] - '0';
                    }
                }
            }

            return result;

        } // method ParseInt32

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                char[] text,
                int offset,
                int length
            )
        {
            var result = 0;
            unchecked
            {
                fixed (char* ptr = text)
                {
                    for (; length > 0; length--, offset++)
                    {
                        result = result * 10 + ptr[offset] - '0';
                    }
                }
            }

            return result;

        } // method ParseInt32

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                byte[] text,
                int offset,
                int length
            )
        {
            var result = 0;
            unchecked
            {
                fixed (byte* ptr = text)
                {
                    for (; length > 0; length--, offset++)
                    {
                        result = result * 10 + ptr[offset] - '0';
                    }
                }
            }

            return result;

        } // method ParseInt32

        /// <summary>
        /// Разбор целого 32-битного числа со знаком.
        /// </summary>
        public static unsafe int ParseInt32
            (
                ReadOnlyMemory<char> text
            )
        {
            if (text.IsEmpty)
            {
                return 0;
            }

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
                    {
                        result = result * 10 + ptr[index] - '0';
                    }

                    if (sign)
                    {
                        result = -result;
                    }
                }
            }

            return result;

        } // method ParseInt32

        /// <summary>
        /// Разбор целого 32-битного числа со знаком.
        /// </summary>
        public static unsafe int ParseInt32
            (
                ReadOnlyMemory<byte> text
            )
        {
            if (text.IsEmpty)
            {
                return 0;
            }

            var result = 0;
            var sign = false;
            unchecked
            {
                fixed (byte* ptr = text.Span)
                {
                    var index = 0;
                    while (ptr[index] == '-')
                    {
                        sign = !sign;
                        ++index;
                    }

                    for (; index < text.Length; index++)
                    {
                        result = result * 10 + ptr[index] - '0';
                    }

                    if (sign)
                    {
                        result = -result;
                    }
                }
            }

            return result;

        } // method ParseInt32

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                char* text
            )
        {
            var result = 0;
            unchecked
            {
                char c;
                while ((c = *text) != '\0')
                {
                    result = result * 10 + c - '0';
                    text++;
                }
            }

            return result;

        } // method ParseInt32

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                char* text,
                int length
            )
        {
            var result = 0;
            unchecked
            {
                while (length > 0)
                {
                    char c = *text;
                    result = result * 10 + c - '0';
                    text++;
                    length--;
                }
            }

            return result;

        } // method ParseInt32

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                byte* text
            )
        {
            if (text is null)
            {
                return 0;
            }

            var result = 0;
            var sign = false;
            unchecked
            {
                while (*text == '-')
                {
                    sign = !sign;
                    text++;
                }

                byte c;
                while ((c = *text) != 0)
                {
                    result = result * 10 + c - '0';
                    text++;
                }

                if (sign)
                {
                    result = -result;
                }
            }

            return result;

        } // method ParseInt32

        // ==========================================================

        /// <summary>
        /// Разбор целого 64-битного числа со знаком.
        /// </summary>
        public static long ParseInt64
            (
                string text
            )
        {
            var result = 0L;
            var sign = false;
            unchecked
            {
                foreach (char c in text)
                {
                    if (c == '-')
                    {
                        sign = !sign;
                    }
                    else
                    {
                        result = result * 10 + c - '0';
                    }
                }

                if (sign)
                {
                    result = -result;
                }
            }

            return result;

        } // method ParseInt64

        /// <summary>
        /// Разбор целого 64-битного числа со знаком.
        /// </summary>
        public static long ParseInt64
            (
                string text,
                int offset,
                int length
            )
        {
            var result = 0L;
            unchecked
            {
                for (; length > 0; length--, offset++)
                {
                    result = result * 10 + text[offset] - '0';
                }
            }

            return result;

        } // method ParseInt64

        /// <summary>
        /// Разбор целого 64-битного числа со знаком.
        /// </summary>
        public static long ParseInt64
            (
                char[] text,
                int offset,
                int length
            )
        {
            var result = 0L;
            unchecked
            {
                for (; length > 0; length--, offset++)
                {
                    result = result * 10 + text[offset] - '0';
                }
            }

            return result;

        } // method ParseInt64

        /// <summary>
        /// Разбор целого 64-битного числа со знаком.
        /// </summary>
        public static long ParseInt64
            (
                byte[] text,
                int offset,
                int length
            )
        {
            var result = 0L;
            unchecked
            {
                for (; length > 0; length--, offset++)
                {
                    result = result * 10 + text[offset] - '0';
                }
            }

            return result;

        } // method ParseInt64

        /// <summary>
        /// Разбор целого 64-битного числа со знаком.
        /// </summary>
        public static long ParseInt64
            (
                ReadOnlyMemory<char> text
            )
        {
            var result = 0L;
            var span = text.Span;
            unchecked
            {
                for (var i = 0; i < text.Length; i++)
                {
                    result = result * 10 + span[i] - '0';
                }
            }

            return result;

        } // method ParseInt64

        /// <summary>
        /// Разбор целого 64-битного числа со знаком.
        /// </summary>
        public static long ParseInt64
            (
                ReadOnlyMemory<byte> text
            )
        {
            if (text.IsEmpty)
            {
                return 0L;
            }

            var result = 0L;
            var sign = false;
            var span = text.Span;
            unchecked
            {
                var index = 0;
                while (index < text.Length && span[index] == '-')
                {
                    sign = !sign;
                    index++;
                }

                for (; index < text.Length; index++)
                {
                    result = result * 10 + span[index] - '0';
                }

                if (sign)
                {
                    result = -result;
                }
            }

            return result;

        } // method ParseInt64

        // ==========================================================

        #endregion

    } // class FastNumber

} // namespace AM
