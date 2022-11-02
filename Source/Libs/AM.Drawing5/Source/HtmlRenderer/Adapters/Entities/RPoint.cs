// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable InconsistentNaming

/* RPoint.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters.Entities;

/// <summary>
///     Represents an ordered pair of floating-point x- and y-coordinates that defines a point in a two-dimensional plane.
/// </summary>
public struct RPoint
{
    #region Properties

    /// <summary>
    ///     Represents a new instance of the <see cref="RPoint" /> class with member data left uninitialized.
    /// </summary>
    /// <filterpriority>1</filterpriority>
    public static readonly RPoint Empty = new ();

    /// <summary>
    ///     Gets a value indicating whether this <see cref="RPoint" /> is empty.
    /// </summary>
    /// <returns>
    ///     true if both <see cref="RPoint.X" /> and
    ///     <see
    ///         cref="RPoint.Y" />
    ///     are 0; otherwise, false.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public bool IsEmpty
    {
        get
        {
            if (Math.Abs (X - 0.0) < 0.001)
            {
                return Math.Abs (Y - 0.0) < 0.001;
            }

            return false;
        }
    }

    /// <summary>
    ///     Gets or sets the x-coordinate of this <see cref="RPoint" />.
    /// </summary>
    /// <returns>
    ///     The x-coordinate of this <see cref="RPoint" />.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public double X { get; set; }

    /// <summary>
    ///     Gets or sets the y-coordinate of this <see cref="RPoint" />.
    /// </summary>
    /// <returns>
    ///     The y-coordinate of this <see cref="RPoint" />.
    /// </returns>
    /// <filterpriority>1</filterpriority>
    public double Y { get; set; }

    #endregion

    #region Construction

    /// <summary>
    ///     Initializes a new instance of the <see cref="RPoint" /> class with the specified coordinates.
    /// </summary>
    /// <param name="x">The horizontal position of the point. </param>
    /// <param name="y">The vertical position of the point. </param>
    public RPoint (double x, double y)
    {
        X = x;
        Y = y;
    }

    #endregion

    #region Operators

    /// <summary>
    ///     Translates the <see cref="RPoint" /> by the specified
    ///     <see
    ///         cref="T:System.Drawing.SizeF" />
    ///     .
    /// </summary>
    /// <returns>
    ///     The translated <see cref="RPoint" />.
    /// </returns>
    /// <param name="pt">
    ///     The <see cref="RPoint" /> to translate.
    /// </param>
    /// <param name="sz">
    ///     The <see cref="T:System.Drawing.SizeF" /> that specifies the numbers to add to the x- and y-coordinates of the
    ///     <see
    ///         cref="RPoint" />
    ///     .
    /// </param>
    public static RPoint operator + (RPoint pt, RSize sz)
    {
        return Add (pt, sz);
    }

    /// <summary>
    ///     Translates a <see cref="RPoint" /> by the negative of a specified
    ///     <see
    ///         cref="T:System.Drawing.SizeF" />
    ///     .
    /// </summary>
    /// <returns>
    ///     The translated <see cref="RPoint" />.
    /// </returns>
    /// <param name="pt">
    ///     The <see cref="RPoint" /> to translate.
    /// </param>
    /// <param name="sz">
    ///     The <see cref="T:System.Drawing.SizeF" /> that specifies the numbers to subtract from the coordinates of
    ///     <paramref
    ///         name="pt" />
    ///     .
    /// </param>
    public static RPoint operator - (RPoint pt, RSize sz)
    {
        return Subtract (pt, sz);
    }

    /// <summary>
    ///     Compares two <see cref="RPoint" /> structures. The result specifies whether the values of the
    ///     <see
    ///         cref="RPoint.X" />
    ///     and <see cref="RPoint.Y" /> properties of the two
    ///     <see
    ///         cref="RPoint" />
    ///     structures are equal.
    /// </summary>
    /// <returns>
    ///     true if the <see cref="RPoint.X" /> and
    ///     <see
    ///         cref="RPoint.Y" />
    ///     values of the left and right
    ///     <see
    ///         cref="RPoint" />
    ///     structures are equal; otherwise, false.
    /// </returns>
    /// <param name="left">
    ///     A <see cref="RPoint" /> to compare.
    /// </param>
    /// <param name="right">
    ///     A <see cref="RPoint" /> to compare.
    /// </param>
    /// <filterpriority>3</filterpriority>
    public static bool operator == (RPoint left, RPoint right)
    {
        return left.X == right.X && left.Y == right.Y;
    }

    /// <summary>
    ///     Determines whether the coordinates of the specified points are not equal.
    /// </summary>
    /// <returns>
    ///     true to indicate the <see cref="RPoint.X" /> and
    ///     <see
    ///         cref="RPoint.Y" />
    ///     values of <paramref name="left" /> and
    ///     <paramref
    ///         name="right" />
    ///     are not equal; otherwise, false.
    /// </returns>
    /// <param name="left">
    ///     A <see cref="RPoint" /> to compare.
    /// </param>
    /// <param name="right">
    ///     A <see cref="RPoint" /> to compare.
    /// </param>
    /// <filterpriority>3</filterpriority>
    public static bool operator != (RPoint left, RPoint right)
    {
        return !(left == right);
    }

    #endregion

    #region Public methods

    /// <summary>
    ///     Translates a given <see cref="RPoint" /> by a specified
    ///     <see
    ///         cref="T:System.Drawing.SizeF" />
    ///     .
    /// </summary>
    /// <returns>
    ///     The translated <see cref="RPoint" />.
    /// </returns>
    /// <param name="pt">
    ///     The <see cref="RPoint" /> to translate.
    /// </param>
    /// <param name="sz">
    ///     The <see cref="T:System.Drawing.SizeF" /> that specifies the numbers to add to the coordinates of
    ///     <paramref
    ///         name="pt" />
    ///     .
    /// </param>
    public static RPoint Add (RPoint pt, RSize sz)
    {
        return new RPoint (pt.X + sz.Width, pt.Y + sz.Height);
    }

    /// <summary>
    ///     Translates a <see cref="RPoint" /> by the negative of a specified size.
    /// </summary>
    /// <returns>
    ///     The translated <see cref="RPoint" />.
    /// </returns>
    /// <param name="pt">
    ///     The <see cref="RPoint" /> to translate.
    /// </param>
    /// <param name="sz">
    ///     The <see cref="T:System.Drawing.SizeF" /> that specifies the numbers to subtract from the coordinates of
    ///     <paramref
    ///         name="pt" />
    ///     .
    /// </param>
    public static RPoint Subtract (RPoint pt, RSize sz)
    {
        return new RPoint (pt.X - sz.Width, pt.Y - sz.Height);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="ValueType.Equals(object?)"/>
    public override bool Equals (object? obj)
    {
        if (obj is not RPoint pointF)
        {
            return false;
        }

        return pointF.X == X && pointF.Y == Y;
    }

    /// <inheritdoc cref="ValueType.GetHashCode"/>
    public override int GetHashCode()
    {
        // ReSharper disable BaseObjectGetHashCodeCallInGetHashCode

        return base.GetHashCode();

        // ReSharper restore BaseObjectGetHashCodeCallInGetHashCode
    }

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return $"{{X={X}, Y={Y}}}";
    }

    #endregion
}
