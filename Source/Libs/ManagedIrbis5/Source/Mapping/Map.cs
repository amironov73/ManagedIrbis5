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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

using AM;

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
        public static ReadOnlyMemory<char> FromBoolean(bool value) =>
            value ? "1".AsMemory() : default;

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
            => field.SetSubFieldValue(code, FromBoolean(value));

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
            => FromBoolean(record.GetOrAddField(tag), code, value);

        /// <summary>
        /// Преобразование в логическое значение.
        /// </summary>
        public static bool ToBoolean(string? value) => !string.IsNullOrEmpty(value);

        /// <summary>
        /// Преобразование в логическое значение.
        /// </summary>
        public static bool ToBoolean(ReadOnlyMemory<char> value) => !value.IsEmpty;

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
            var field = record.GetField(tag);

            return field is null
                ? false
                : ToBoolean(field, code);
        } // method ToBoolean

        #endregion

        #region Byte

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromByte(byte value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromByte(value));

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
            => FromByte(record.GetOrAddField(tag), code, value);

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
        /// Преобразование в 8-битное целое без знака.
        /// </summary>
        public static byte ToByte(ReadOnlyMemory<char> value)
        {
            byte.TryParse
                (
                    value.Span,
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
                ? (byte) 0
                : ToByte(subField);
        } // method ToByte

        #endregion

        #region Char

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromChar(char value) =>
            new string(value, 1).AsMemory();

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
            => field.SetSubFieldValue(code, FromChar(value));

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
            => FromChar(record.GetOrAddField(tag), code, value);

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
        public static char ToChar(ReadOnlyMemory<char> value)
        {
            return value.IsEmpty
                ? '\0'
                : value.Span[0];
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
        public static ReadOnlyMemory<char> FromDateTime(DateTime value)
            => IrbisDate.ConvertDateToString(value).AsMemory();

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
            => field.SetSubFieldValue(code, FromDateTime(value));

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
            => FromDateTime(record.GetOrAddField(tag), code, value);

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime(string? value)
            => IrbisDate.ConvertStringToDate(value);

        /// <summary>
        /// Преобразование в дату.
        /// </summary>
        public static DateTime ToDateTime(ReadOnlyMemory<char> value)
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
            var field = record.GetField(tag);

            return field is null
                ? DateTime.MinValue
                : ToDateTime(field, code);
        } // method ToDateTime

        #endregion

        #region Decimal

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromDecimal(decimal value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromDecimal(value));

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
            => FromDecimal(record.GetOrAddField(tag), code, value);

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
        public static decimal ToDecimal(ReadOnlyMemory<char> value)
        {
            decimal.TryParse
                (
                    value.Span,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToDecimal

        /// <summary>
        /// Преобразование в денежный тип.
        /// </summary>
        public static decimal ToDecimal(SubField subfield) =>
            ToDecimal(subfield.Value);

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
            var field = record.GetField(tag);

            return field is null
                ? 0m
                : ToDecimal(field, code);
        } // method ToDecimal

        #endregion

        #region Double

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromDouble(double value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromDouble(value));

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
            => FromDouble(record.GetOrAddField(tag), code, value);

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
        public static double ToDouble(ReadOnlyMemory<char> value)
        {
            // TODO: сделать через Utility
            double.TryParse
                (
                    value.Span,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out var result
                );

            return result;
        } // method ToDouble

        /// <summary>
        /// Преобразование в число с плавающей точкой двойной точности.
        /// </summary>
        public static double ToDouble(SubField subfield) =>
            ToDouble(subfield.Value);

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
            var field = record.GetField(tag);

            return field is null
                ? 0.0
                : ToDouble(field, code);
        } // method ToDouble

        #endregion

        #region Int16

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromInt16(short value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromInt16(value));

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
            => FromInt16(record.GetOrAddField(tag), code, value);

        /// <summary>
        /// Преобразование в 16-битное целое число со знаком.
        /// </summary>
        public static short ToInt16(string? value)
        {
            // TODO: сделать через Utility
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
        public static short ToInt16(ReadOnlyMemory<char> value)
        {
            // TODO: сделать через Utility
            short.TryParse
                (
                    value.Span,
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
                ? (short) 0
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
            var field = record.GetField(tag);

            return field is null
                ? (short) 0
                : ToInt16(field, code);
        } // method ToInt16

        #endregion

        #region Int32

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromInt32(int value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromInt32(value));

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
            => FromInt32(record.GetOrAddField(tag), code, value);

        /// <summary>
        /// Преобразование в 32-битное целое число со знаком.
        /// </summary>
        public static int ToInt32(string? value)
            => value.ParseInt32();

        /// <summary>
        /// Преобразование в 32-битное целое число со знаком.
        /// </summary>
        public static int ToInt32(ReadOnlyMemory<char> value)
            => value.ParseInt32();

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
            var field = record.GetField(tag);

            return field is null
                ? 0
                : ToInt32(field, code);
        } // method ToInt32

        #endregion

        #region Int64

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromInt64(long value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromInt64(value));

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
            => FromInt64(record.GetOrAddField(tag), code, value);

        /// <summary>
        /// Преобразование в 64-битное целое число со знаком.
        /// </summary>
        public static long ToInt64(string? value)
            => value.ParseInt64();

        /// <summary>
        /// Преобразование в 64-битное целое число со знаком.
        /// </summary>
        public static long ToInt64(ReadOnlyMemory<char> value)
            => value.ParseInt64();

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
            var field = record.GetField(tag);

            return field is null
                ? 0
                : ToInt64(field, code);
        } // method ToInt64

        #endregion

        #region SByte

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromSByte(sbyte value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromSByte(value));

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
            => FromSByte(record.GetOrAddField(tag), code, value);

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

        public static sbyte ToSByte(ReadOnlyMemory<char> value)
        {
            sbyte.TryParse
                (
                    value.Span,
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
                ? (sbyte) 0
                : ToSByte(subField);
        } // method ToSByte

        #endregion

        #region Single

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromSingle(float value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromSingle(value));

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
            => FromSingle(record.GetOrAddField(tag), code, value);

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
        public static float ToSingle(ReadOnlyMemory<char> value)
        {
            float.TryParse
                (
                    value.Span,
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
            var field = record.GetField(tag);

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
            => subfield.Value = value.AsMemory();

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static void FromString
            (
                Field field,
                char code,
                string? value
            )
            => field.SetSubFieldValue(code, value.AsMemory());

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
            => FromString(record.GetOrAddField(tag), code, value);

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static string? ToString(SubField subfield) => subfield.Value.ToString();

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
                return field.Value.ToString();
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
            var field = record.GetField(tag);

            return field is null
                ? null
                : ToString(field, code);
        } // method ToString

        #endregion

        #region UInt16

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromUInt16(ushort value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromUInt16(value));

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
            => FromUInt16(record.GetOrAddField(tag), code, value);

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
        public static ushort ToUInt16(ReadOnlyMemory<char> value)
        {
            ushort.TryParse
                (
                    value.Span,
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
                ? (ushort) 0
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
            var field = record.GetField(tag);

            return field is null
                ? (ushort) 0
                : ToUInt16(field, code);
        } // method ToUInt16

        #endregion

        #region UInt32

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromUInt32(uint value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromUInt32(value));

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
            => FromUInt32(record.GetOrAddField(tag), code, value);

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
        public static uint ToUInt32(ReadOnlyMemory<char> value)
        {
            uint.TryParse
                (
                    value.Span,
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
            var field = record.GetField(tag);

            return field is null
                ? 0
                : ToUInt32(field, code);
        } // method ToUInt32

        #endregion

        #region UInt64

        /// <summary>
        /// Преобразование в строку.
        /// </summary>
        public static ReadOnlyMemory<char> FromUInt64(ulong value)
            => value.ToString(CultureInfo.InvariantCulture).AsMemory();

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
            => field.SetSubFieldValue(code, FromUInt64(value));

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
            => FromUInt64(record.GetOrAddField(tag), code, value);
        // method FromInt16

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
        public static ulong ToUInt64(ReadOnlyMemory<char> value)
        {
            ulong.TryParse
                (
                    value.Span,
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
            var field = record.GetField(tag);

            return field is null
                ? 0
                : ToUInt64(field, code);
        } // method ToUInt64

        #endregion

        #region Object

        public static void FromObject
            (
                Field field,
                Type type,
                object source
            )
        {
            var mapper = MapperCache.GetFieldMapper(type);
            mapper.ToField(field, source);
        } // method FromObject

        public static void FromObject<T>(Field field, object source)
            where T : class
            => FromObject(field, typeof(T), source);

        public static void FromObject
            (
                Record record,
                int tag,
                Type type,
                IEnumerable source
            )
        {
            var mapper = MapperCache.GetFieldMapper(type);
            record.RemoveField(tag);

            foreach (var item in source)
            {
                var notnull = item.ThrowIfNull("item");
                var field = new Field { Tag = tag };
                mapper.ToField(field, notnull);
                record.Fields.Add(field);
            }
        } // method FromObject

        public static void FromObject<T>(Record record, int tag, IEnumerable source)
            where T : class
            => FromObject(record, tag, typeof(T), source);

        public static void ToObject
            (
                Field field,
                Type type,
                object target
            )
        {
            var mapper = MapperCache.GetFieldMapper(type);
            mapper.FromField(field, target);
        } // method ToObject

        public static void ToObject<T>(Field field, object target)
            where T : class
            => ToObject(field, typeof(T), target);

        public static void ToObject
            (
                Record record,
                int tag,
                object target
            )
        {
            var field = record.GetField(tag);
            if (field is not null)
            {
                var mapper = MapperCache.GetFieldMapper(target.GetType());
                mapper.FromField(field, target);
            }
        } // method ToObject

        public static void ToObject
            (
                Record record,
                int tag,
                Type type,
                IList target
            )
        {
            var mapper = MapperCache.GetFieldMapper(type);
            foreach (var field in record.Fields)
            {
                if (field.Tag == tag)
                {
                    var item = Activator.CreateInstance(type)
                        .ThrowIfNull("item");
                    mapper.FromField(field, item);
                    target.Add(item);
                }
            }
        } // method ToObject

        public static void ToObject<T>(Record record, int tag, IList target)
            where T : class, new()
            => ToObject(record, tag, typeof(T), target);

        #endregion

        #endregion

    } // class Map

} // namespace ManagedIrbis.Mapping
