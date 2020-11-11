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
using System.Diagnostics;
using System.Diagnostics.Contracts;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Сборник простых вспомогательных методов.
    /// </summary>
    public static class Utility
    {
        #region Public methods

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
            //Sure.NotNull(message, nameof(message));

            if (ReferenceEquals(value, null))
            {
                //Log.Error
                //(
                //    nameof(Utility) + "::" + nameof(ThrowIfNull)
                //    + ": "
                //    + message
                //);

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

        #endregion
    }
}
