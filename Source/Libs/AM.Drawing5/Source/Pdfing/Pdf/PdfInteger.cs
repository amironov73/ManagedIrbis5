// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* PdfInteger.cs -- представляет прямое целочисленное значение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using AM;

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Представляет прямое целочисленное значение.
/// </summary>
[DebuggerDisplay ("({Value})")]
public sealed class PdfInteger
    : PdfNumber, IConvertible, IFormattable
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfInteger"/> class.
    /// </summary>
    public PdfInteger()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfInteger"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    public PdfInteger
        (
            int value
        )
    {
        Value = value;
    }

    /// <summary>
    /// Gets the value as integer.
    /// </summary>
    /// <remarks>
    /// This class must behave like a value type.
    /// Therefore it cannot be changed.
    /// </remarks>
    public int Value { get; }

    #endregion

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Value.ToInvariantString();
    }

    /// <summary>
    /// Writes the integer as string.
    /// </summary>
    internal override void WriteObject (PdfWriter writer)
    {
        writer.Write (this);
    }

    #region IConvertible Members

    sbyte IConvertible.ToSByte (IFormatProvider? provider)
    {
        throw new InvalidCastException();
    }

    ulong IConvertible.ToUInt64 (IFormatProvider? provider)
    {
        return Convert.ToUInt64 (Value);
    }

    double IConvertible.ToDouble (IFormatProvider? provider)
    {
        return Value;
    }

    DateTime IConvertible.ToDateTime (IFormatProvider? provider)
    {
        // TODO:  Add PdfInteger.ToDateTime implementation
        return new DateTime();
    }

    float IConvertible.ToSingle (IFormatProvider? provider)
    {
        return Value;
    }

    bool IConvertible.ToBoolean (IFormatProvider? provider)
    {
        return Convert.ToBoolean (Value);
    }

    int IConvertible.ToInt32 (IFormatProvider? provider)
    {
        return Value;
    }

    ushort IConvertible.ToUInt16 (IFormatProvider? provider)
    {
        return Convert.ToUInt16 (Value);
    }

    short IConvertible.ToInt16 (IFormatProvider? provider)
    {
        return Convert.ToInt16 (Value);
    }

    string IConvertible.ToString (IFormatProvider? provider)
    {
        return Value.ToString (provider);
    }

    byte IConvertible.ToByte (IFormatProvider? provider)
    {
        return Convert.ToByte (Value);
    }

    char IConvertible.ToChar (IFormatProvider? provider)
    {
        return Convert.ToChar (Value);
    }

    long IConvertible.ToInt64 (IFormatProvider? provider)
    {
        return Value;
    }

    /// <summary>
    /// Returns TypeCode for 32-bit integers.
    /// </summary>
    public TypeCode GetTypeCode()
    {
        return TypeCode.Int32;
    }

    decimal IConvertible.ToDecimal (IFormatProvider? provider)
    {
        return Value;
    }

    object IConvertible.ToType (Type conversionType, IFormatProvider? provider)
    {
        // TODO:  Add PdfInteger.ToType implementation
        return null!;
    }

    uint IConvertible.ToUInt32 (IFormatProvider? provider)
    {
        return Convert.ToUInt32 (Value);
    }

    #endregion

    #region IFormattable Members

    string IFormattable.ToString (string? _value, IFormatProvider? provider)
    {
        return _value.ToVisibleString();
    }

    #endregion
}
