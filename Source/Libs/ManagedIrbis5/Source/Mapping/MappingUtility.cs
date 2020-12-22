// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable SimplifyConditionalTernaryExpression
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* MappingUtility.cs -- вспомогательные методы для отображения полей/подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Mapping
{
    /// <summary>
    /// Вспомогательные методы для отображения полей/подполей на поля классов.
    /// </summary>
    public static class MappingUtility
    {
        #region Public methods

        /// <summary>
        /// Преобразование в булево значение.
        /// </summary>
        public static bool ToBoolean
            (
                SubField subField
            )
        {
            return !string.IsNullOrEmpty(subField.Value);
        } // method ToBoolean

        /// <summary>
        /// Преобразование в булево значение.
        /// </summary>
        public static bool ToBoolean
            (
                Field field,
                char code
            )
        {
            var subField = field.GetFirstSubField(code);

            return subField is null
                ? false
                : ToBoolean(subField);
        } // method ToBoolean

        /// <summary>
        /// Преобразование в символ.
        /// </summary>
        public static char ToChar
            (
                SubField subField
            )
        {
            var text = subField.Value;

            return string.IsNullOrEmpty(text)
                ? '\0'
                : text[0];
        } // method ToChar

        /// <summary>
        /// Преобразование в символ.
        /// </summary>
        public static char ToChar
            (
                Field field,
                char code
            )
        {
            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? '\0'
                : ToChar(subfield);
        } // method ToChar

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime
            (
                SubField subField
            )
        {
            return IrbisDate.ConvertStringToDate(subField.Value);
        } // method ToDateTime

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime? ToDateTime
            (
                Field field,
                char code
            )
        {
            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? null
                : ToDateTime(subfield);
        } // method ToDateTime

        /// <summary>
        /// Преобразование в число с фиксированной точкой.
        /// </summary>
        public static decimal ToDecimal
            (
                SubField subField
            )
        {
            decimal.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToDecimal

        /// <summary>
        /// Преобразование в число с плавающей точкой
        /// двойной точностью.
        /// </summary>
        public static double ToDouble
            (
                SubField subField
            )
        {
            double.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToDouble

        /// <summary>
        /// Преобразование в число с плавающей точкой
        /// одинарной точности.
        /// </summary>
        public static float ToSingle
            (
                SubField subField
            )
        {
            float.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToSingle

        /// <summary>
        /// Преобразование в 16-битное целое со знаком.
        /// </summary>
        public static short ToInt16
            (
                SubField subField
            )
        {
            short.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        }

        /// <summary>
        /// Преобразование в 32-битное целое со знаком.
        /// </summary>
        public static int ToInt32
            (
                SubField subField
            )
        {
            int.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        }

        /// <summary>
        /// Преобразование в 32-битное целое со знаком.
        /// </summary>
        public static int ToInt32
            (
                Field field,
                char code
            )
        {
            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? 0
                : ToInt32(subfield);
        } // method ToInt32

        /// <summary>
        /// Преобразование в 64-битное целое со знаком.
        /// </summary>
        public static long ToInt64
            (
                SubField subField
            )
        {
            long.TryParse
                (
                    subField.Value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToInt64

        /// <summary>
        /// Преобразование в 64-битное целое со знаком.
        /// </summary>
        public static long ToInt64
            (
                Field field,
                char code
            )
        {
            var subfield = field.GetFirstSubField(code);

            return subfield is null
                ? 0
                : ToInt64(subfield);
        } // method ToInt64

        /// <summary>
        /// Преобразование в строку (тривиальное).
        /// </summary>
        public static string? ToString
            (
                SubField subField
            )
        {
            return subField.Value;
        } // method ToString

        #endregion
    }
}
