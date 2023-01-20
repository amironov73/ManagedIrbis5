// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* OmnipotentComparer.cs -- сравнитель всего со всем
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Сравнитель всего со всем.
/// </summary>
public sealed class OmnipotentComparer
    : IComparer
{
    #region Properties

    /// <summary>
    /// Сравнитель по умолчанию.
    /// </summary>
    public static IComparer Default { get; } = new OmnipotentComparer();

    #endregion

    #region IComparer members

    /// <inheritdoc cref="IComparer.Compare"/>
    public int Compare
        (
            object? x,
            object? y
        )
    {
        if (x is null)
        {
            return y is null ? 0 : -1;
        }

        if (y is null)
        {
            return 1;
        }

        if (x is bool boolX)
        {
            unchecked
            {
                var tempX = Convert.ToInt32 (boolX);
                return y switch
                {
                    bool boolY => Math.Sign (tempX - Convert.ToInt32 (boolY)),
                    byte byteY => Math.Sign (tempX - byteY),
                    sbyte sbyteY => Math.Sign (tempX - sbyteY),
                    short shortY => Math.Sign (tempX - shortY),
                    ushort ushortY => Math.Sign (tempX - ushortY),
                    int intY => Math.Sign (tempX - intY),
                    uint uintY => Math.Sign (tempX - uintY),
                    long longY => Math.Sign (tempX - longY),
                    ulong ulongY => Math.Sign (((double) tempX) - ulongY),
                    float floatY => Math.Sign (tempX - floatY),
                    double doubleY => Math.Sign (tempX - doubleY),
                    decimal decimalY => Math.Sign (tempX - decimalY),
                    nint nintY => Math.Sign (tempX - Convert.ToInt32 (nintY)),
                    nuint nuintY => Math.Sign (tempX - Convert.ToInt32 (nuintY)),
                    string s => Math.Sign (tempX - s.SafeToInt32()),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is byte byteX)
        {
            unchecked
            {
                return y switch
                {
                    bool boolY => Math.Sign (byteX - Convert.ToByte (boolY)),
                    byte byteY => Math.Sign (byteX - byteY),
                    sbyte sbyteY => Math.Sign (byteX - sbyteY),
                    short shortY => Math.Sign (byteX - shortY),
                    ushort ushortY => Math.Sign (byteX - ushortY),
                    int intY => Math.Sign (byteX - intY),
                    uint uintY => Math.Sign (byteX - uintY),
                    long longY => Math.Sign (byteX - longY),
                    ulong ulongY => Math.Sign (((double) byteX) - ulongY),
                    float floatY => Math.Sign (byteX - floatY),
                    double doubleY => Math.Sign (byteX - doubleY),
                    decimal decimalY => Math.Sign (byteX - decimalY),
                    nint nintY => Math.Sign (byteX - Convert.ToInt32 (nintY)),
                    nuint nuintY => Math.Sign (byteX - Convert.ToInt32 (nuintY)),
                    string s => Math.Sign (byteX - s.SafeToInt32()),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is sbyte sbyteX)
        {
            unchecked
            {
                return y switch
                {
                    bool boolY => Math.Sign (sbyteX - Convert.ToSByte (boolY)),
                    byte byteY => Math.Sign (sbyteX - byteY),
                    sbyte sbyteY => Math.Sign (sbyteX - sbyteY),
                    short shortY => Math.Sign (sbyteX - shortY),
                    ushort ushortY => Math.Sign (sbyteX - ushortY),
                    int intY => Math.Sign (sbyteX - intY),
                    uint uintY => Math.Sign (sbyteX - uintY),
                    long longY => Math.Sign (sbyteX - longY),
                    ulong ulongY => Math.Sign (((double) sbyteX) - ulongY),
                    float floatY => Math.Sign (sbyteX - floatY),
                    double doubleY => Math.Sign (sbyteX - doubleY),
                    decimal decimalY => Math.Sign (sbyteX - decimalY),
                    nint nintY => Math.Sign (sbyteX - Convert.ToInt32 (nintY)),
                    nuint nuintY => Math.Sign (sbyteX - Convert.ToInt32 (nuintY)),
                    string s => Math.Sign (sbyteX - s.SafeToInt32()),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is short shortX)
        {
            unchecked
            {
                return y switch
                {
                    bool boolY => Math.Sign (shortX - Convert.ToInt16 (boolY)),
                    byte byteY => Math.Sign (shortX - byteY),
                    sbyte sbyteY => Math.Sign (shortX - sbyteY),
                    short shortY => Math.Sign (shortX - shortY),
                    ushort ushortY => Math.Sign (shortX - ushortY),
                    int intY => Math.Sign (shortX - intY),
                    uint uintY => Math.Sign (shortX - uintY),
                    long longY => Math.Sign (shortX - longY),
                    ulong ulongY => Math.Sign (((double) shortX) - ulongY),
                    float floatY => Math.Sign (shortX - floatY),
                    double doubleY => Math.Sign (shortX - doubleY),
                    decimal decimalY => Math.Sign (shortX - decimalY),
                    nint nintY => Math.Sign (shortX - Convert.ToInt32 (nintY)),
                    nuint nuintY => Math.Sign (shortX - Convert.ToInt32 (nuintY)),
                    string s => Math.Sign (shortX - s.SafeToInt32()),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is ushort ushortX)
        {
            unchecked
            {
                return y switch
                {
                    bool boolY => Math.Sign (ushortX - Convert.ToUInt16 (boolY)),
                    byte byteY => Math.Sign (ushortX - byteY),
                    sbyte sbyteY => Math.Sign (ushortX - sbyteY),
                    short shortY => Math.Sign (ushortX - shortY),
                    ushort ushortY => Math.Sign (ushortX - ushortY),
                    int intY => Math.Sign (ushortX - intY),
                    uint uintY => Math.Sign (ushortX - uintY),
                    long longY => Math.Sign (ushortX - longY),
                    ulong ulongY => Math.Sign (((double) ushortX) - ulongY),
                    float floatY => Math.Sign (ushortX - floatY),
                    double doubleY => Math.Sign (ushortX - doubleY),
                    decimal decimalY => Math.Sign (ushortX - decimalY),
                    nint nintY => Math.Sign (ushortX - Convert.ToInt32 (nintY)),
                    nuint nuintY => Math.Sign (ushortX - Convert.ToInt32 (nuintY)),
                    string s => Math.Sign (ushortX - s.SafeToInt32()),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is int intX)
        {
            unchecked
            {
                return y switch
                {
                    bool boolY => Math.Sign (intX - Convert.ToInt32 (boolY)),
                    byte byteY => Math.Sign (intX - byteY),
                    sbyte sbyteY => Math.Sign (intX - sbyteY),
                    short shortY => Math.Sign (intX - shortY),
                    ushort ushortY => Math.Sign (intX - ushortY),
                    int intY => Math.Sign (intX - intY),
                    uint uintY => Math.Sign (intX - uintY),
                    long longY => Math.Sign (intX - longY),
                    ulong ulongY => Math.Sign (((double) intX) - ulongY),
                    float floatY => Math.Sign (intX - floatY),
                    double doubleY => Math.Sign (intX - doubleY),
                    decimal decimalY => Math.Sign (intX - decimalY),
                    nint nintY => Math.Sign (intX - Convert.ToInt32 (nintY)),
                    nuint nuintY => Math.Sign (intX - Convert.ToInt32 (nuintY)),
                    string s => Math.Sign (intX - s.SafeToInt32()),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is uint uintX)
        {
            unchecked
            {
                return y switch
                {
                    bool boolY => Math.Sign ((int)(uintX - Convert.ToUInt32 (boolY))),
                    byte byteY => Math.Sign ((int)(uintX - byteY)),
                    sbyte sbyteY => Math.Sign ((int)(uintX - sbyteY)),
                    short shortY => Math.Sign ((int)(uintX - shortY)),
                    ushort ushortY => Math.Sign ((int)(uintX - ushortY)),
                    int intY => Math.Sign ((int)(uintX - intY)),
                    uint uintY => Math.Sign (uintX - uintY),
                    long longY => Math.Sign (uintX - longY),
                    ulong ulongY => Math.Sign (((double) uintX) - ulongY),
                    float floatY => Math.Sign (uintX - floatY),
                    double doubleY => Math.Sign (uintX - doubleY),
                    decimal decimalY => Math.Sign (uintX - decimalY),
                    nint nintY => Math.Sign ((int)(uintX - Convert.ToUInt32 (nintY))),
                    nuint nuintY => Math.Sign ((int)(uintX - Convert.ToUInt32 (nuintY))),
                    string s => Math.Sign ((int)(uintX - (uint)s.SafeToInt32())),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is long longX)
        {
            unchecked
            {
                return y switch
                {
                    bool boolY => Math.Sign (longX - Convert.ToInt64 (boolY)),
                    byte byteY => Math.Sign (longX - byteY),
                    sbyte sbyteY => Math.Sign (longX - sbyteY),
                    short shortY => Math.Sign (longX - shortY),
                    ushort ushortY => Math.Sign (longX - ushortY),
                    int intY => Math.Sign (longX - intY),
                    uint uintY => Math.Sign (longX - uintY),
                    long longY => Math.Sign (longX - longY),
                    ulong ulongY => Math.Sign (((double) longX) - ulongY),
                    float floatY => Math.Sign (longX - floatY),
                    double doubleY => Math.Sign (longX - doubleY),
                    decimal decimalY => Math.Sign (longX - decimalY),
                    nint nintY => Math.Sign (longX - Convert.ToInt64 (nintY)),
                    nuint nuintY => Math.Sign (longX - Convert.ToInt64 (nuintY)),
                    string s => Math.Sign (longX - s.SafeToInt64()),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is ulong ulongX)
        {
            unchecked
            {
                return y switch
                {
                    bool boolY => Math.Sign ((long) (ulongX - Convert.ToUInt64 (boolY))),
                    byte byteY => Math.Sign ((long) (ulongX - byteY)),
                    sbyte sbyteY => Math.Sign ((double) ulongX - sbyteY),
                    short shortY => Math.Sign ((double) ulongX - shortY),
                    ushort ushortY => Math.Sign ((double) ulongX - ushortY),
                    int intY => Math.Sign ((double) ulongX - intY),
                    uint uintY => Math.Sign ((double) ulongX - uintY),
                    long longY => Math.Sign ((double) ulongX - longY),
                    ulong ulongY => Math.Sign (((double) ulongX) - ulongY),
                    float floatY => Math.Sign (ulongX - floatY),
                    double doubleY => Math.Sign (ulongX - doubleY),
                    decimal decimalY => Math.Sign (ulongX - decimalY),
                    nint nintY => Math.Sign ((long) (ulongX - Convert.ToUInt64 (nintY))),
                    nuint nuintY => Math.Sign ((long) (ulongX - Convert.ToUInt64 (nuintY))),
                    string s => Math.Sign ((long) (ulongX - (ulong) s.SafeToInt64())),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is float floatX)
        {
            return y switch
            {
                bool boolY => Math.Sign (floatX - Convert.ToSingle (boolY)),
                byte byteY => Math.Sign (floatX - byteY),
                sbyte sbyteY => Math.Sign (floatX - sbyteY),
                short shortY => Math.Sign (floatX - shortY),
                ushort ushortY => Math.Sign (floatX - ushortY),
                int intY => Math.Sign (floatX - intY),
                uint uintY => Math.Sign (floatX - uintY),
                long longY => Math.Sign (floatX - longY),
                ulong ulongY => Math.Sign (((double) floatX) - ulongY),
                float floatY => Math.Sign (floatX - floatY),
                double doubleY => Math.Sign (floatX - doubleY),
                decimal decimalY => Math.Sign (floatX - (double) decimalY),
                nint nintY => Math.Sign (floatX - Convert.ToSingle (nintY)),
                nuint nuintY => Math.Sign (floatX - Convert.ToSingle (nuintY)),
                string s => Math.Sign (floatX - s.SafeToDouble()),
                _ => throw new InvalidOperationException()
            };
        }

        if (x is double doubleX)
        {
            return y switch
            {
                bool boolY => Math.Sign (doubleX - Convert.ToDouble (boolY)),
                byte byteY => Math.Sign (doubleX - byteY),
                sbyte sbyteY => Math.Sign (doubleX - sbyteY),
                short shortY => Math.Sign (doubleX - shortY),
                ushort ushortY => Math.Sign (doubleX - ushortY),
                int intY => Math.Sign (doubleX - intY),
                uint uintY => Math.Sign (doubleX - uintY),
                long longY => Math.Sign (doubleX - longY),
                ulong ulongY => Math.Sign (doubleX - ulongY),
                float floatY => Math.Sign (doubleX - floatY),
                double doubleY => Math.Sign (doubleX - doubleY),
                decimal decimalY => Math.Sign (doubleX - (double) decimalY),
                nint nintY => Math.Sign (doubleX - Convert.ToDouble (nintY)),
                nuint nuintY => Math.Sign (doubleX - Convert.ToDouble (nuintY)),
                string s => Math.Sign (doubleX - s.SafeToDouble()),
                _ => throw new InvalidOperationException()
            };
        }

        if (x is decimal decimalX)
        {
            return y switch
            {
                bool boolY => Math.Sign (decimalX - Convert.ToDecimal (boolY)),
                byte byteY => Math.Sign (decimalX - byteY),
                sbyte sbyteY => Math.Sign (decimalX - sbyteY),
                short shortY => Math.Sign (decimalX - shortY),
                ushort ushortY => Math.Sign (decimalX - ushortY),
                int intY => Math.Sign (decimalX - intY),
                uint uintY => Math.Sign (decimalX - uintY),
                long longY => Math.Sign (decimalX - longY),
                ulong ulongY => Math.Sign (((double) decimalX) - ulongY),
                float floatY => Math.Sign ((double) decimalX - floatY),
                double doubleY => Math.Sign ((double) decimalX - doubleY),
                decimal decimalY => Math.Sign (decimalX - decimalY),
                nint nintY => Math.Sign (decimalX - Convert.ToDecimal (nintY)),
                nuint nuintY => Math.Sign (decimalX - Convert.ToDecimal (nuintY)),
                string s => Math.Sign (decimalX - s.SafeToDecimal()),
                _ => throw new InvalidOperationException()
            };
        }

        if (x is string stringX)
        {
            unchecked
            {
                return y switch
                {
                    bool boolY => Math.Sign (Convert.ToInt32 (Convert.ToBoolean (stringX)) - Convert.ToInt32 (boolY)),
                    byte byteY => Math.Sign (stringX.SafeToInt32() - Convert.ToInt32 (byteY)),
                    sbyte sbyteY => Math.Sign (stringX.SafeToInt32() - Convert.ToInt32 (sbyteY)),
                    short shortY => Math.Sign (stringX.SafeToInt32() - Convert.ToInt32 (shortY)),
                    ushort ushortY => Math.Sign (stringX.SafeToInt32() - Convert.ToInt32 (ushortY)),
                    int intY => Math.Sign (stringX.SafeToInt32() - intY),
                    uint uintY => Math.Sign (Convert.ToUInt32 (stringX) - uintY),
                    long longY => Math.Sign (stringX.SafeToInt64() - Convert.ToInt64 (longY)),
                    ulong ulongY => Math.Sign ((int)(Convert.ToUInt64 (stringX) - Convert.ToUInt64 (ulongY))),
                    float floatY => Math.Sign (stringX.SafeToDouble() - floatY),
                    double doubleY => Math.Sign (stringX.SafeToDouble() - doubleY),
                    decimal decimalY => Math.Sign (stringX.SafeToDecimal() - decimalY),
                    nint nintY => Math.Sign (stringX.SafeToInt32() - Convert.ToInt32 (nintY)),
                    nuint nuintY => Math.Sign (Convert.ToUInt32 (stringX) - Convert.ToUInt32 (nuintY)),
                    string s => Math.Sign (string.CompareOrdinal (stringX, s)),
                    _ => throw new InvalidOperationException()
                };
            }
        }

        if (x is IComparable comparable)
        {
            return comparable.CompareTo (y);
        }

        var xType = x.GetType();
        var yType = y.GetType();

        throw new InvalidOperationException ($"Can't compare {xType} with {yType}");
    }

    #endregion
}
