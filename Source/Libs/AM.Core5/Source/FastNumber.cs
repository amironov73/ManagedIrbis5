// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FastNumber.cs -- быстрые и грязные методы работы с числами
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
    /// Fast routines for integer numbers.
    /// </summary>
    public static class FastNumber
    {
        #region Public methods

        // ==========================================================

        /// <summary>
        /// Convert integer to string.
        /// </summary>
        [SkipLocalsInit]
        public static unsafe string Int32ToString
            (
                int number
            )
        {
            var buffer = stackalloc char[10];
            var offset = 9;
            if (number == 0)
            {
                buffer[offset] = '0';
                offset--;
            }
            else
            {
                for (; number != 0; offset--)
                {
                    number = Math.DivRem(number, 10, out int rem);
                    buffer[offset] = (char) ('0' + rem);
                }
            }

            return new string(buffer, offset + 1, 9 - offset);
        }

        // ==========================================================

        /// <summary>
        /// Convert integer to string.
        /// </summary>
        [SkipLocalsInit]
        public static unsafe string Int64ToString
            (
                long number
            )
        {
            var buffer = stackalloc char[20];
            var offset = 19;
            if (number == 0)
            {
                buffer[offset] = '0';
                offset--;
            }
            else
            {
                for (; number != 0; offset--)
                {
                    number = Math.DivRem(number, 10, out long rem);
                    buffer[offset] = (char) ('0' + rem);
                }
            }

            return new string(buffer, offset + 1, 19 - offset);
        }

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
        }

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
        }

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
        }

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
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                ReadOnlyMemory<char> text
            )
        {
            var result = 0;
            unchecked
            {
                fixed (char* ptr = text.Span)
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        result = result * 10 + ptr[i] - '0';
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                ReadOnlyMemory<byte> text
            )
        {
            var result = 0;
            unchecked
            {
                fixed (byte* ptr = text.Span)
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        result = result * 10 + ptr[i] - '0';
                    }
                }
            }

            return result;
        }

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
        }

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
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static unsafe int ParseInt32
            (
                byte* text
            )
        {
            var result = 0;
            unchecked
            {
                byte c;
                while ((c = *text) != 0)
                {
                    result = result * 10 + c - '0';
                    text++;
                }
            }

            return result;
        }

        // ==========================================================

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static long ParseInt64
            (
                string text
            )
        {
            var result = 0L;
            unchecked
            {
                foreach (char c in text)
                {
                    result = result * 10 + c - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
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
        }

        /// <summary>
        /// Fast number parsing.
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
        }

        /// <summary>
        /// Fast number parsing.
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
        }

        /// <summary>
        /// Fast number parsing.
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
                for (int i = 0; i < text.Length; i++)
                {
                    result = result * 10 + span[i] - '0';
                }
            }

            return result;
        }

        /// <summary>
        /// Fast number parsing.
        /// </summary>
        public static long ParseInt64
            (
                ReadOnlyMemory<byte> text
            )
        {
            var result = 0L;
            var span = text.Span;
            unchecked
            {
                for (int i = 0; i < text.Length; i++)
                {
                    result = result * 10 + span[i] - '0';
                }
            }

            return result;
        }

        // ==========================================================

        #endregion
    }
}
