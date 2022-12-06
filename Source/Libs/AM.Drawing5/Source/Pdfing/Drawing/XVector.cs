// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XVector.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

using PdfSharpCore.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Represents a two-dimensional vector specified by x- and y-coordinates.
/// </summary>
[DebuggerDisplay ("{DebuggerDisplay}")]
[Serializable]
[StructLayout (LayoutKind.Sequential)]
public struct XVector : IFormattable
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public XVector (double x, double y)
    {
        _x = x;
        _y = y;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static bool operator == (XVector vector1, XVector vector2)
    {
        return vector1._x == vector2._x && vector1._y == vector2._y;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static bool operator != (XVector vector1, XVector vector2)
    {
        return vector1._x != vector2._x || vector1._y != vector2._y;
    }

    /// <inheritdoc cref="Equals(PdfSharpCore.Drawing.XVector,PdfSharpCore.Drawing.XVector)"/>
    public static bool Equals (XVector vector1, XVector vector2)
    {
        return vector1.X.Equals (vector2.X) && vector1.Y.Equals (vector2.Y);
    }

    /// <inheritdoc cref="ValueType.Equals(object?)"/>
    public override bool Equals (object? o)
    {
        return o is XVector vector && Equals (this, vector);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Equals (XVector value)
    {
        return Equals (this, value);
    }

    /// <inheritdoc cref="ValueType.GetHashCode"/>
    public override int GetHashCode()
    {
        // ReSharper disable NonReadonlyFieldInGetHashCode

        return _x.GetHashCode() ^ _y.GetHashCode();

        // ReSharper restore NonReadonlyFieldInGetHashCode
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static XVector Parse (string source)
    {
        var helper = new TokenizerHelper (source, CultureInfo.InvariantCulture);
        var str = helper.NextTokenRequired();
        var vector = new XVector (Convert.ToDouble (str, CultureInfo.InvariantCulture),
            Convert.ToDouble (helper.NextTokenRequired(), CultureInfo.InvariantCulture));
        helper.LastTokenRequired();
        return vector;
    }

    /// <summary>
    ///
    /// </summary>
    public double X
    {
        get => _x;
        set => _x = value;
    }

    double _x;

    /// <summary>
    ///
    /// </summary>
    public double Y
    {
        get => _y;
        set => _y = value;
    }

    double _y;

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return ConvertToString (null, null);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="provider"></param>
    /// <returns></returns>
    public string ToString
        (
            IFormatProvider? provider
        )
    {
        return ConvertToString (null, provider);
    }

    string IFormattable.ToString
        (
            string? format,
            IFormatProvider? provider
        )
    {
        return ConvertToString (format, provider);
    }

    internal string ConvertToString
        (
            string? format,
            IFormatProvider? provider
        )
    {
        const char numericListSeparator = ',';
        provider ??= CultureInfo.InvariantCulture;

        // ReSharper disable FormatStringProblem
        return string.Format (provider, "{1:" + format + "}{0}{2:" + format + "}", numericListSeparator, _x, _y);
        // ReSharper restore FormatStringProblem
    }

    /// <summary>
    ///
    /// </summary>
    public double Length => Math.Sqrt (_x * _x + _y * _y);

    /// <summary>
    ///
    /// </summary>
    public double LengthSquared => _x * _x + _y * _y;

    /// <summary>
    ///
    /// </summary>
    public void Normalize()
    {
        this = this / Math.Max (Math.Abs (_x), Math.Abs (_y));
        this = this / Length;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static double CrossProduct (XVector vector1, XVector vector2)
    {
        return vector1._x * vector2._y - vector1._y * vector2._x;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static double AngleBetween (XVector vector1, XVector vector2)
    {
        var y = vector1._x * vector2._y - vector2._x * vector1._y;
        var x = vector1._x * vector2._x + vector1._y * vector2._y;
        return Math.Atan2 (y, x) * 57.295779513082323;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static XVector operator - (XVector vector)
    {
        return new XVector (-vector._x, -vector._y);
    }

    /// <summary>
    ///
    /// </summary>
    public void Negate()
    {
        _x = -_x;
        _y = -_y;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static XVector operator + (XVector vector1, XVector vector2)
    {
        return new XVector (vector1._x + vector2._x, vector1._y + vector2._y);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static XVector Add (XVector vector1, XVector vector2)
    {
        return new XVector (vector1._x + vector2._x, vector1._y + vector2._y);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static XVector operator - (XVector vector1, XVector vector2)
    {
        return new XVector (vector1._x - vector2._x, vector1._y - vector2._y);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static XVector Subtract (XVector vector1, XVector vector2)
    {
        return new XVector (vector1._x - vector2._x, vector1._y - vector2._y);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static XPoint operator + (XVector vector, XPoint point)
    {
        return new XPoint (point.X + vector._x, point.Y + vector._y);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static XPoint Add (XVector vector, XPoint point)
    {
        return new XPoint (point.X + vector._x, point.Y + vector._y);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public static XVector operator * (XVector vector, double scalar)
    {
        return new XVector (vector._x * scalar, vector._y * scalar);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public static XVector Multiply (XVector vector, double scalar)
    {
        return new XVector (vector._x * scalar, vector._y * scalar);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="scalar"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static XVector operator * (double scalar, XVector vector)
    {
        return new XVector (vector._x * scalar, vector._y * scalar);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="scalar"></param>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static XVector Multiply (double scalar, XVector vector)
    {
        return new XVector (vector._x * scalar, vector._y * scalar);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public static XVector operator / (XVector vector, double scalar)
    {
        return vector * (1.0 / scalar);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="scalar"></param>
    /// <returns></returns>
    public static XVector Divide (XVector vector, double scalar)
    {
        return vector * (1.0 / scalar);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static XVector operator * (XVector vector, XMatrix matrix)
    {
        return matrix.Transform (vector);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static XVector Multiply (XVector vector, XMatrix matrix)
    {
        return matrix.Transform (vector);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static double operator * (XVector vector1, XVector vector2)
    {
        return vector1._x * vector2._x + vector1._y * vector2._y;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static double Multiply (XVector vector1, XVector vector2)
    {
        return vector1._x * vector2._x + vector1._y * vector2._y;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    public static double Determinant (XVector vector1, XVector vector2)
    {
        return vector1._x * vector2._y - vector1._y * vector2._x;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static explicit operator XSize (XVector vector)
    {
        return new XSize (Math.Abs (vector._x), Math.Abs (vector._y));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static explicit operator XPoint (XVector vector)
    {
        return new XPoint (vector._x, vector._y);
    }

    /// <summary>
    /// Gets the DebuggerDisplayAttribute text.
    /// </summary>
    /// <value>The debugger display.</value>

    // ReSharper disable UnusedMember.Local
    string DebuggerDisplay

        // ReSharper restore UnusedMember.Local
    {
        get
        {
            const string format = Config.SignificantFigures10;
            return string.Format (CultureInfo.InvariantCulture, "vector=({0:" + format + "}, {1:" + format + "})",
                _x, _y);
        }
    }
}
