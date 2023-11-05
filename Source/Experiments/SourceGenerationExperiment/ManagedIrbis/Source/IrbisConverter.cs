// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;

#endregion

namespace ManagedIrbis;

/// <summary>
/// Конвертирует произвольные данные с учетом специфики ИРБИС.
/// </summary>
public static class IrbisConverter
{
    #region Public methods

    public static TTarget FromString<TTarget>
        (
            string? value
        )
    {
        return Type.GetTypeCode (typeof (TTarget)) switch
        {
            TypeCode.DateTime => (TTarget) (object) DateTime.ParseExact (value!, "YYYYMMDd", CultureInfo.InvariantCulture),
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

    public static ICollection<TTarget> CollectionFromStrings<TTarget>
        (
            IList<string> values
        )
    {
        var result = new Collection<TTarget>();
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

    public static string? ToString
        (
            object? value
        )
    {
        return value switch
        {
            null => null,
            DateTime date => date.ToString ("YYYYMMdd", CultureInfo.InvariantCulture),
            IConvertible convertible => convertible.ToString (CultureInfo.InvariantCulture),
            _ => value.ToString()
        };
    }

    public static IList<string> StringsToList
        (
            IList values
        )
    {
        var result = new List<string>();
        foreach (var value in values)
        {
            var line = ToString (value);
            if (!string.IsNullOrEmpty (line))
            {
                result.Add (line);
            }
        }

        return result;
    }

    #endregion
}
