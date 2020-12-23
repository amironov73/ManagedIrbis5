// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeNotNullable
// ReSharper disable SimplifyConditionalTernaryExpression
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* Map.cs -- конверсия примитивных типов
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
    /// Конверсия примитивных типов.
    /// </summary>
    public static class Map
    {
        #region Constants

        /// <summary>
        /// Нет кода подполя. Это означает "значение поля до первого разделителя".
        /// </summary>
        private const char NoCode = '\0';

        #endregion

        #region Public methods

        #region Boolean

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromBoolean(bool value) => value ? "1" : null;

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromBoolean(SubField subfield, bool value)
            => subfield.Value = FromBoolean(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromBoolean
            (
                Field field,
                char code,
                bool value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromBoolean(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromBoolean(value);
            }
        } // method FromBoolean

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromBoolean
            (
                Record record,
                int tag,
                char code,
                bool value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromBoolean(field, code, value);
        } // method FromBoolean

        /// <summary>
        /// Преобразование в логическое значение.
        /// </summary>
        public static bool ToBoolean(string? value) => !string.IsNullOrEmpty(value);

        /// <summary>
        /// Преобразование в логическое значение.
        /// </summary>
        public static bool ToBoolean(SubField subfield) => ToBoolean(subfield.Value);

        /// <summary>
        /// Преобразование в логическое значение.
        /// </summary>
        public static bool ToBoolean
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToBoolean(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? false
                : ToBoolean(subField);
        } // method ToBoolean

        /// <summary>
        /// Преобразование в логическое значение.
        /// </summary>
        public static bool ToBoolean
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? false
                : ToBoolean(field, code);
        } // method ToBoolean

        #endregion

        #region Byte

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromByte(byte value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromByte(SubField subfield, byte value)
            => subfield.Value = FromByte(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromByte
            (
                Field field,
                char code,
                byte value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromByte(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromByte(value);
            }
        } // method FromByte

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromByte
            (
                Record record,
                int tag,
                char code,
                byte value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromByte(field, code, value);
        } // method FromByte

        /// <summary>
        /// Преобразование в 8-битное целое без знака.
        /// </summary>
        public static byte ToByte(string? value)
        {
            byte.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToByte

        /// <summary>
        /// Преобразование в 8-битное без знака.
        /// </summary>
        public static byte ToByte(SubField subfield) => ToByte(subfield.Value);

        /// <summary>
        /// Преобразование в 8-битное без знака.
        /// </summary>
        public static byte ToByte
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToByte(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0
                : ToByte(subField);
        } // method ToByte

        #endregion

        #region Char

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromChar(char value) => new (value, 1);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromChar(SubField subfield, char value)
            => subfield.Value = FromChar(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromChar
            (
                Field field,
                char code,
                char value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromChar(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromChar(value);
            }
        } // method FromChar

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromChar
            (
                Record record,
                int tag,
                char code,
                char value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromChar(field, code, value);
        } // method FromChar

        /// <summary>
        /// Преобразование в символ.
        /// </summary>
        public static char ToChar(string? value)
        {
            return string.IsNullOrEmpty(value)
                ? '\0'
                : value[0];
        } // method ToChar

        /// <summary>
        /// Преобразование в символ.
        /// </summary>
        public static char ToChar(SubField subfield) => ToChar(subfield.Value);

        /// <summary>
        /// Преобразование в символ.
        /// </summary>
        public static char ToChar
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToChar(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? '\0'
                : ToChar(subField);
        } // method ToChar

        #endregion

        #region DateTime

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromDateTime(DateTime value)
            => IrbisDate.ConvertDateToString(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromDateTime(SubField subfield, DateTime value)
            => subfield.Value = FromDateTime(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromDateTime
            (
                Field field,
                char code,
                DateTime value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromDateTime(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromDateTime(value);
            }
        } // method FromDateTime

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromDateTime
            (
                Record record,
                int tag,
                char code,
                DateTime value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromDateTime(field, code, value);
        } // method FromDateTime

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime(string? value)
            => IrbisDate.ConvertStringToDate(value);

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime(SubField subfield) => ToDateTime(subfield.Value);

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToDateTime(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? DateTime.MinValue
                : ToDateTime(subField);
        } // method ToDateTime

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? DateTime.MinValue
                : ToDateTime(field, code);
        } // method ToDateTime

        #endregion

        #region Decimal

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromDecimal(decimal value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromDecimal(SubField subfield, decimal value)
            => subfield.Value = FromDecimal(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromDecimal
            (
                Field field,
                char code,
                decimal value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromDecimal(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromDecimal(value);
            }
        } // method FromDecimal

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromDecimal
            (
                Record record,
                int tag,
                char code,
                decimal value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromDecimal(field, code, value);
        } // method FromDecimal

        /// <summary>
        /// Преобразование в денежный тип.
        /// </summary>
        public static decimal ToDecimal(string? value)
        {
            decimal.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToDecimal

        /// <summary>
        /// Преобразование в денежный тип.
        /// </summary>
        public static decimal ToDecimal(SubField subfield) => ToDecimal(subfield.Value);

        /// <summary>
        /// Преобразование в денежный тип.
        /// </summary>
        public static decimal ToDecimal
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToDecimal(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0m
                : ToDecimal(subField);
        } // method ToDecimal

        /// <summary>
        /// Преобразование в денежный тип.
        /// </summary>
        public static decimal ToDecimal
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0m
                : ToDecimal(field, code);
        } // method ToDecimal

        #endregion

        #region Double

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromDouble(double value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromDouble(SubField subfield, double value)
            => subfield.Value = FromDouble(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromDouble
            (
                Field field,
                char code,
                double value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromDouble(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromDouble(value);
            }
        } // method FromDouble

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromDouble
            (
                Record record,
                int tag,
                char code,
                double value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromDouble(field, code, value);
        } // method FromDouble

        /// <summary>
        /// Преобразование в число с плавающей точкой двойной точности.
        /// </summary>
        public static double ToDouble(string? value)
        {
            double.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToDouble

        /// <summary>
        /// Преобразование в число с плавающей точкой двойной точности.
        /// </summary>
        public static double ToDouble(SubField subfield) => ToDouble(subfield.Value);

        /// <summary>
        /// Преобразование в число с плавающей точкой двойной точности.
        /// </summary>
        public static double ToDouble
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToDouble(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0.0
                : ToDouble(subField);
        } // method ToDouble

        /// <summary>
        /// Преобразование в число с плавающей точкой двойной точности.
        /// </summary>
        public static double ToDouble
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0.0
                : ToDouble(field, code);
        } // method ToDouble

        #endregion

        #region Int16

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromInt16(short value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromInt16(SubField subfield, short value)
            => subfield.Value = FromInt16(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromInt16
            (
                Field field,
                char code,
                short value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromInt16(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromInt16(value);
            }
        } // method FromInt16

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromInt16
            (
                Record record,
                int tag,
                char code,
                short value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromInt16(field, code, value);
        } // method FromInt16

        /// <summary>
        /// Преобразование в 16-битное целое число со знаком.
        /// </summary>
        public static short ToInt16(string? value)
        {
            short.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToInt16

        /// <summary>
        /// Преобразование в 16-битное целое число со знаком.
        /// </summary>
        public static short ToInt16(SubField subfield) => ToInt16(subfield.Value);

        /// <summary>
        /// Преобразование в 16-битное целое число со знаком.
        /// </summary>
        public static short ToInt16
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToInt16(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0
                : ToInt16(subField);
        } // method ToInt16

        /// <summary>
        /// Преобразование в 16-битное целое число со знаком.
        /// </summary>
        public static short ToInt16
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0
                : ToInt16(field, code);
        } // method ToInt16

        #endregion

        #region Int32

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromInt32(int value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromInt32(SubField subfield, int value)
            => subfield.Value = FromInt32(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromInt32
            (
                Field field,
                char code,
                int value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromInt32(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromInt32(value);
            }
        } // method FromInt32

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromInt32
            (
                Record record,
                int tag,
                char code,
                int value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromInt32(field, code, value);
        } // method FromInt32

        /// <summary>
        /// Преобразование в 32-битное целое число со знаком.
        /// </summary>
        public static int ToInt32(string? value)
        {
            int.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToInt32

        /// <summary>
        /// Преобразование в 32-битное целое число со знаком.
        /// </summary>
        public static int ToInt32(SubField subfield) => ToInt32(subfield.Value);

        /// <summary>
        /// Преобразование в 32-битное целое число со знаком.
        /// </summary>
        public static int ToInt32
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToInt32(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0
                : ToInt32(subField);
        } // method ToInt32

        /// <summary>
        /// Преобразование в 32-битное целое число со знаком.
        /// </summary>
        public static int ToInt32
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0
                : ToInt32(field, code);
        } // method ToInt32

        #endregion

        #region Int64

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromInt64(long value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromInt64(SubField subfield, long value)
            => subfield.Value = FromInt64(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromInt64
            (
                Field field,
                char code,
                long value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromInt64(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromInt64(value);
            }
        } // method FromInt64

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromInt64
            (
                Record record,
                int tag,
                char code,
                long value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromInt64(field, code, value);
        } // method FromInt64

        /// <summary>
        /// Преобразование в 64-битное целое число со знаком.
        /// </summary>
        public static long ToInt64(string? value)
        {
            long.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToInt64

        /// <summary>
        /// Преобразование в 64-битное целое число со знаком.
        /// </summary>
        public static long ToInt64(SubField subfield) => ToInt64(subfield.Value);

        /// <summary>
        /// Преобразование в 64-битное целое число со знаком.
        /// </summary>
        public static long ToInt64
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToInt64(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0
                : ToInt64(subField);
        } // method ToInt64

        /// <summary>
        /// Преобразование в 64-битное целое число со знаком.
        /// </summary>
        public static long ToInt64
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0
                : ToInt64(field, code);
        } // method ToInt64

        #endregion

        #region SByte

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromSByte(sbyte value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromSByte(SubField subfield, sbyte value)
            => subfield.Value = FromSByte(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromSByte
            (
                Field field,
                char code,
                sbyte value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromSByte(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromSByte(value);
            }
        } // method FromSByte

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromSByte
            (
                Record record,
                int tag,
                char code,
                sbyte value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromSByte(field, code, value);
        } // method FromSByte


        /// <summary>
        /// Преобразование в 8-битное целое со знаком.
        /// </summary>
        public static sbyte ToSByte(string? value)
        {
            sbyte.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToByte

        /// <summary>
        /// Преобразование в 8-битное со знаком.
        /// </summary>
        public static sbyte ToSByte(SubField subfield) => ToSByte(subfield.Value);

        /// <summary>
        /// Преобразование в 8-битное со знаком.
        /// </summary>
        public static sbyte ToSByte
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToSByte(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0
                : ToSByte(subField);
        } // method ToSByte

        #endregion

        #region Single

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromSingle(float value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromSingle(SubField subfield, float value)
            => subfield.Value = FromDouble(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromSingle
            (
                Field field,
                char code,
                float value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromSingle(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromSingle(value);
            }
        } // method FromSingle

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromSingle
            (
                Record record,
                int tag,
                char code,
                short value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromSingle(field, code, value);
        } // method FromSingle

        /// <summary>
        /// Преобразование в число с плавающей точкой одинарной точности.
        /// </summary>
        public static float ToSingle(string? value)
        {
            float.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToSingle

        /// <summary>
        /// Преобразование в число с плавающей точкой одинарной точности.
        /// </summary>
        public static float ToSingle(SubField subfield) => ToSingle(subfield.Value);

        /// <summary>
        /// Преобразование в число с плавающей точкой одинарной точности.
        /// </summary>
        public static float ToSingle
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToSingle(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0.0f
                : ToSingle(subField);
        } // method ToSingle

        /// <summary>
        /// Преобразование в число с плавающей точкой одинарной точности.
        /// </summary>
        public static float ToSingle
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0.0f
                : ToSingle(field, code);
        } // method ToSingle

        #endregion

        #region String

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromString(SubField subfield, string? value)
            => subfield.Value = value;

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromString
            (
                Field field,
                char code,
                string? value
            )
        {
            if (code == NoCode)
            {
                field.Value = value;
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = value;
            }
        } // method FromString

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromString
            (
                Record record,
                int tag,
                char code,
                string? value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromString(field, code, value);
        } // method FromString

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? ToString(SubField subfield) => subfield.Value;

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? ToString
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return field.Value;
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? null
                : ToString(subField);
        } // method ToString

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? ToString
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? null
                : ToString(field, code);
        } // method ToString

        #endregion

        #region UInt16

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromUInt16(ushort value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromUInt16(SubField subfield, ushort value)
            => subfield.Value = FromUInt16(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromUInt16
            (
                Field field,
                char code,
                ushort value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromUInt16(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromUInt16(value);
            }
        } // method FromUInt16

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromUInt16
            (
                Record record,
                int tag,
                char code,
                ushort value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromUInt16(field, code, value);
        } // method FromUInt16

        /// <summary>
        /// Преобразование в 16-битное целое число без знака.
        /// </summary>
        public static ushort ToUInt16(string? value)
        {
            ushort.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToUInt16

        /// <summary>
        /// Преобразование в 16-битное целое число без знака.
        /// </summary>
        public static ushort ToUInt16(SubField subfield) => ToUInt16(subfield.Value);

        /// <summary>
        /// Преобразование в 16-битное целое число без знака.
        /// </summary>
        public static ushort ToUInt16
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToUInt16(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0
                : ToUInt16(subField);
        } // method ToUInt16

        /// <summary>
        /// Преобразование в 16-битное целое число без знака.
        /// </summary>
        public static ushort ToUInt16
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0
                : ToUInt16(field, code);
        } // method ToUInt16

        #endregion

        #region UInt32

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromUInt32(uint value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromUInt32(SubField subfield, uint value)
            => subfield.Value = FromUInt32(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromUInt32
            (
                Field field,
                char code,
                uint value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromUInt32(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromUInt32(value);
            }
        } // method FromUInt32

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromUInt32
            (
                Record record,
                int tag,
                char code,
                uint value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromUInt32(field, code, value);
        } // method FromUInt32

        /// <summary>
        /// Преобразование в 32-битное целое число без знака.
        /// </summary>
        public static uint ToUInt32(string? value)
        {
            uint.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToUInt32

        /// <summary>
        /// Преобразование в 32-битное целое число без знака.
        /// </summary>
        public static uint ToUInt32(SubField subfield) => ToUInt32(subfield.Value);

        /// <summary>
        /// Преобразование в 32-битное целое число без знака.
        /// </summary>
        public static uint ToUInt32
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToUInt32(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0
                : ToUInt32(subField);
        } // method ToUInt32

        /// <summary>
        /// Преобразование в 32-битное целое число без знака.
        /// </summary>
        public static uint ToUInt32
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0
                : ToUInt32(field, code);
        } // method ToUInt32

        #endregion

        #region UInt64

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? FromUInt64(ulong value)
            => value.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromUInt64(SubField subfield, ulong value)
            => subfield.Value = FromUInt64(value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromUInt64
            (
                Field field,
                char code,
                ulong value
            )
        {
            if (code == NoCode)
            {
                field.Value = FromUInt64(value);
                return;
            }

            var subfield = field.GetFirstSubField(code);
            if (subfield is not null)
            {
                subfield.Value = FromUInt64(value);
            }
        } // method FromUInt64

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromUInt64
            (
                Record record,
                int tag,
                char code,
                ulong value
            )
        {
            var field = record.GetFirstField(tag) ?? record.Add(tag);
            FromUInt64(field, code, value);
        } // method FromInt16

        /// <summary>
        /// Преобразование в 64-битное целое число без знака.
        /// </summary>
        public static ulong ToUInt64(string? value)
        {
            ulong.TryParse
                (
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToUInt64

        /// <summary>
        /// Преобразование в 64-битное целое число без знака.
        /// </summary>
        public static ulong ToUInt64(SubField subfield) => ToUInt64(subfield.Value);

        /// <summary>
        /// Преобразование в 64-битное целое число без знака.
        /// </summary>
        public static ulong ToUInt64
            (
                Field field,
                char code
            )
        {
            if (code == NoCode)
            {
                return ToUInt64(field.Value);
            }

            var subField = field.GetFirstSubField(code);

            return subField is null
                ? 0
                : ToUInt64(subField);
        } // method ToUInt64

        /// <summary>
        /// Преобразование в 64-битное целое число без знака.
        /// </summary>
        public static ulong ToUInt64
            (
                Record record,
                int tag,
                char code
            )
        {
            var field = record.GetFirstField(tag);

            return field is null
                ? 0
                : ToUInt64(field, code);
        } // method ToUInt64

        #endregion

        #endregion

    } // class Map

} // namespace ManagedIrbis.Mapping
