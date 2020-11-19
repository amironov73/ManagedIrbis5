// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* InvariantFormat.cs -- форматирование текста, не зависящее от культуры
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

#endregion

#nullable enable

namespace AM.Text
{
    /// <summary>
    /// Форматирование текста, не зависящее от культуы.
    /// </summary>
    public static class InvariantFormat
    {
        #region Private members

        private static readonly IFormatProvider FormatProvider;

        static InvariantFormat()
        {
            FormatProvider = CultureInfo.InvariantCulture;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Format string.
        /// </summary>
        public static string Format
            (
                string format,
                object? arg0
            )
        {
            var result = string.Format
                (
                    FormatProvider,
                    format,
                    arg0
                );

            return result;
        }

        /// <summary>
        /// Format string.
        /// </summary>
        public static string Format
            (
                string format,
                object? arg0,
                object? arg1
            )
        {
            var result = string.Format
                (
                    FormatProvider,
                    format,
                    arg0,
                    arg1
                );

            return result;
        }

        /// <summary>
        /// Format string.
        /// </summary>
        public static string Format
            (
                string format,
                object? arg0,
                object? arg1,
                object? arg2
            )
        {
            var result = string.Format
                (
                    FormatProvider,
                    format,
                    arg0,
                    arg1,
                    arg2
                );

            return result;
        }

        /// <summary>
        /// Format string.
        /// </summary>
        public static string Format
            (
                string format,
                params object[] args
            )
        {
            var result = string.Format
                (
                    FormatProvider,
                    format,
                    args
                );

            return result;
        }

        /// <summary>
        /// Format integer.
        /// </summary>
        public static string Format
            (
                string format,
                int arg0
            )
        {
            var result = string.Format
                (
                    FormatProvider,
                    format,
                    arg0
                );

            return result;
        }

        /// <summary>
        /// Format double.
        /// </summary>
        public static string Format
            (
                string format,
                double arg0
            )
        {
            var result = string.Format
                (
                    FormatProvider,
                    format,
                    arg0
                );

            return result;
        }

        /// <summary>
        /// Format integer.
        /// </summary>
        public static string Format
            (
                int arg0
            )
        {
            var result = arg0.ToString(FormatProvider);

            return result;
        }

        /// <summary>
        /// Format double.
        /// </summary>
        public static string Format
            (
                double arg0
            )
        {
            var result = arg0.ToString(FormatProvider);

            return result;
        }

        /// <summary>
        /// Format decimal.
        /// </summary>
        public static string Format
            (
                decimal arg0
            )
        {
            var result = arg0.ToString(FormatProvider);

            return result;
        }

        #endregion
    }
}
