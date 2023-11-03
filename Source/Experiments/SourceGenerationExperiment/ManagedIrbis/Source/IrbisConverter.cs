// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

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

    #endregion
}
