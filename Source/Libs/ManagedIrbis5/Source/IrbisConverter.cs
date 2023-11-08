// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IrbisConverter.cs -- конвертер величин, необходимый для маппинга полей/подполей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using AM;

using JetBrains.Annotations;

#endregion

namespace ManagedIrbis;

/// <summary>
/// Конвертирует произвольные данные с учетом специфики ИРБИС.
/// </summary>
[PublicAPI]
public static class IrbisConverter
{
    #region Public methods

    /// <summary>
    /// Преобразование из строки в указанный тип.
    /// </summary>
    public static TTarget FromString<TTarget>
        (
            string? value
        )
    {
        return Type.GetTypeCode (typeof (TTarget)) switch
        {
            TypeCode.Int32 => (TTarget)(object) value.SafeToInt32(),
            TypeCode.UInt32 => (TTarget)(object) value.SafeToUInt32(),
            TypeCode.Int64 => (TTarget)(object) value.SafeToInt64(),
            TypeCode.UInt64 => (TTarget)(object) value.SafeToUInt64(),
            TypeCode.DateTime =>
                (TTarget)(object) DateTime.ParseExact (value!, "YYYYMMDd", CultureInfo.InvariantCulture),
            _ => (TTarget) Convert.ChangeType (value, typeof (TTarget))!
        };
    }

    /// <summary>
    /// Формирование массива.
    /// </summary>
    public static TTarget[] ArrayFromStrings<TTarget>
        (
            IList<string> values
        )
        => ListFromStrings<TTarget> (values).ToArray();

    /// <summary>
    /// Формирование списка.
    /// </summary>
    public static IList<TTarget> ListFromStrings<TTarget>
        (
            IList<string> values
        )
    {
        var result = new List<TTarget>();
        foreach (var value in values)
        {
            if (!string.IsNullOrEmpty (value))
            {
                var item = FromString<TTarget> (value);
                result.Add (item);
            }
        }

        return result;
    }

    /// <summary>
    /// Формирование списка.
    /// </summary>
    public static IList<TTarget> ListFromFields<TTarget>
        (
            IList<Field> values,
            Func<Field, TTarget, TTarget> converter
        )
        where TTarget: new()
    {
        var result = new List<TTarget>();
        foreach (var value in values)
        {
            var item = converter (value, new TTarget());
            result.Add (item);
        }

        return result;
    }

    /// <summary>
    /// Получение полей из списка.
    /// </summary>
    public static IList<Field> FieldsFromList<TSource>
        (
            IList<TSource> values,
            Func<TSource, Field, Field> converter
        )
    {
        var result = new List<Field>();
        foreach (var value in values)
        {
            var field = converter (value, new Field());
            result.Add (field);
        }

        return result;
    }

    /// <summary>
    /// Преобразование значения в строку.
    /// </summary>
    public static string? ToString
        (
            object? value
        )
    {
        return value switch
        {
            null => null,
            int intValue => intValue.ToInvariantString(),
            uint uintValue => uintValue.ToInvariantString(),
            long longValue => longValue.ToInvariantString(),
            ulong ulongValue => ulongValue.ToInvariantString(),
            DateTime date => date.ToString ("YYYYMMdd", CultureInfo.InvariantCulture),
            IConvertible convertible => convertible.ToString (CultureInfo.InvariantCulture),
            _ => value.ToString()
        };
    }

    /// <summary>
    /// Преобразование нескольких значений в строки.
    /// </summary>
    public static IList<string> ToStrings<TItem>
        (
            IList<TItem> items
        )
    {
        var result = new List<string>();
        foreach (var item in items)
        {
            var line = ToString (item);
            if (!string.IsNullOrEmpty (line))
            {
                result.Add (line);
            }
        }

        return result;
    }

    #endregion
}
