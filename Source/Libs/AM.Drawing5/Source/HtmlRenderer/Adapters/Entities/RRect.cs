// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RRect.cs -- расположение и размер прямоугольника
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters.Entities;

/// <summary>
/// Хранит набор из четырех чисел с плавающей запятой,
/// представляющих расположение и размер прямоугольника.
/// </summary>
public struct RRect
{
    #region Fields and Consts

    /// <summary>
    ///     Represents an instance of the <see cref="RRect" /> class with its members uninitialized.
    /// </summary>
    public static readonly RRect Empty = new ();

    #endregion

    #region Construction

    /// <summary>
    ///     Initializes a new instance of the <see cref="RRect" /> class with the specified location and size.
    /// </summary>
    /// <param name="x">The x-coordinate of the upper-left corner of the rectangle. </param>
    /// <param name="y">The y-coordinate of the upper-left corner of the rectangle. </param>
    /// <param name="width">The width of the rectangle. </param>
    /// <param name="height">The height of the rectangle. </param>
    public RRect
        (
            double x,
            double y,
            double width,
            double height
        )
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RRect" /> class with the specified location and size.
    /// </summary>
    /// <param name="location">A <see cref="RPoint" /> that represents the upper-left corner of the rectangular region.</param>
    /// <param name="size">A <see cref="RSize" /> that represents the width and height of the rectangular region.</param>
    public RRect (RPoint location, RSize size)
    {
        X = location.X;
        Y = location.Y;
        Width = size.Width;
        Height = size.Height;
    }

    #endregion

    /// <summary>
    /// Gets or sets the coordinates of the upper-left corner of this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>A <see cref="RPoint" /> that represents the upper-left corner of this <see cref="RRect" /> structure.</returns>
    public RPoint Location
    {
        get => new (X, Y);
        set
        {
            X = value.X;
            Y = value.Y;
        }
    }

    /// <summary>
    /// Gets or sets the size of this <see cref="RRect" />.
    /// </summary>
    /// <returns>A <see cref="RSize" /> that represents the width and height of this <see cref="RRect" /> structure.</returns>
    public RSize Size
    {
        get => new (Width, Height);
        set
        {
            Width = value.Width;
            Height = value.Height;
        }
    }

    /// <summary>
    ///     Gets or sets the x-coordinate of the upper-left corner of this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     The x-coordinate of the upper-left corner of this <see cref="RRect" /> structure.
    /// </returns>
    public double X { get; set; }

    /// <summary>
    ///     Gets or sets the y-coordinate of the upper-left corner of this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     The y-coordinate of the upper-left corner of this <see cref="RRect" /> structure.
    /// </returns>
    public double Y { get; set; }

    /// <summary>
    ///     Gets or sets the width of this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     The width of this <see cref="RRect" /> structure.
    /// </returns>
    public double Width { get; set; }

    /// <summary>
    ///     Gets or sets the height of this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     The height of this <see cref="RRect" /> structure.
    /// </returns>
    public double Height { get; set; }

    /// <summary>
    ///     Gets the x-coordinate of the left edge of this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     The x-coordinate of the left edge of this <see cref="RRect" /> structure.
    /// </returns>
    public double Left => X;

    /// <summary>
    ///     Gets the y-coordinate of the top edge of this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     The y-coordinate of the top edge of this <see cref="RRect" /> structure.
    /// </returns>
    public double Top => Y;

    /// <summary>
    ///     Gets the x-coordinate that is the sum of <see cref="RRect.X" /> and
    ///     <see
    ///         cref="RRect.Width" />
    ///     of this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     The x-coordinate that is the sum of <see cref="RRect.X" /> and
    ///     <see
    ///         cref="RRect.Width" />
    ///     of this <see cref="RRect" /> structure.
    /// </returns>
    public double Right => X + Width;

    /// <summary>
    ///     Gets the y-coordinate that is the sum of <see cref="RRect.Y" /> and
    ///     <see
    ///         cref="RRect.Height" />
    ///     of this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     The y-coordinate that is the sum of <see cref="RRect.Y" /> and
    ///     <see
    ///         cref="RRect.Height" />
    ///     of this <see cref="RRect" /> structure.
    /// </returns>
    public double Bottom => Y + Height;

    /// <summary>
    ///     Tests whether the <see cref="RRect.Width" /> or
    ///     <see
    ///         cref="RRect.Height" />
    ///     property of this <see cref="RRect" /> has a value of zero.
    /// </summary>
    /// <returns>
    ///     This property returns true if the <see cref="RRect.Width" /> or
    ///     <see
    ///         cref="RRect.Height" />
    ///     property of this <see cref="RRect" /> has a value of zero; otherwise, false.
    /// </returns>
    public bool IsEmpty => Width <= 0.0 || Height <= 0.0;

    /// <summary>
    ///     Tests whether two <see cref="RRect" /> structures have equal location and size.
    /// </summary>
    /// <returns>
    ///     This operator returns true if the two specified <see cref="RRect" /> structures have equal
    ///     <see cref="RRect.X" />, <see cref="RRect.Y" />, <see cref="RRect.Width" />, and <see cref="RRect.Height" /> properties.
    /// </returns>
    /// <param name="left">
    ///     The <see cref="RRect" /> structure that is to the left of the equality operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="RRect" /> structure that is to the right of the equality operator.
    /// </param>
    public static bool operator == (RRect left, RRect right)
    {
        if (Math.Abs (left.X - right.X) < 0.001 && Math.Abs (left.Y - right.Y) < 0.001 &&
            Math.Abs (left.Width - right.Width) < 0.001)
        {
            return Math.Abs (left.Height - right.Height) < 0.001;
        }

        return false;
    }

    /// <summary>
    ///     Tests whether two <see cref="RRect" /> structures differ in location or size.
    /// </summary>
    /// <returns>
    ///     This operator returns true if any of the <see cref="RRect.X" /> ,
    ///     <see cref="RRect.Y" />, <see cref="RRect.Width" />, or <see cref="RRect.Height" />
    ///     properties of the two <see cref="RRect" /> structures are unequal; otherwise false.
    /// </returns>
    /// <param name="left">
    ///     The <see cref="RRect" /> structure that is to the left of the inequality operator.
    /// </param>
    /// <param name="right">
    ///     The <see cref="RRect" /> structure that is to the right of the inequality operator.
    /// </param>
    public static bool operator != (RRect left, RRect right)
    {
        return !(left == right);
    }

    /// <summary>
    ///     Creates a <see cref="RRect" /> structure with upper-left corner and lower-right corner at the specified locations.
    /// </summary>
    /// <returns>
    ///     The new <see cref="RRect" /> that this method creates.
    /// </returns>
    /// <param name="left">The x-coordinate of the upper-left corner of the rectangular region. </param>
    /// <param name="top">The y-coordinate of the upper-left corner of the rectangular region. </param>
    /// <param name="right">The x-coordinate of the lower-right corner of the rectangular region. </param>
    /// <param name="bottom">The y-coordinate of the lower-right corner of the rectangular region. </param>
    public static RRect FromLTRB (double left, double top, double right, double bottom)
    {
        return new RRect (left, top, right - left, bottom - top);
    }

    /// <summary>
    ///     Tests whether <paramref name="obj" /> is a <see cref="RRect" /> with the same location and size of this
    ///     <see cref="RRect" />.
    /// </summary>
    /// <returns>
    ///     This method returns true if <paramref name="obj" /> is a <see cref="RRect" /> and its X, Y, Width, and Height properties are equal to the corresponding properties of this
    ///     <see cref="RRect" />; otherwise, false.
    /// </returns>
    /// <param name="obj">
    ///     The <see cref="T:System.Object" /> to test.
    /// </param>
    public override bool Equals (object? obj)
    {
        if (!(obj is RRect))
        {
            return false;
        }

        var rectangleF = (RRect)obj;
        if (Math.Abs (rectangleF.X - X) < 0.001 && Math.Abs (rectangleF.Y - Y) < 0.001 &&
            Math.Abs (rectangleF.Width - Width) < 0.001)
        {
            return Math.Abs (rectangleF.Height - Height) < 0.001;
        }

        return false;
    }

    /// <summary>
    ///     Determines if the specified point is contained within this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     This method returns true if the point defined by <paramref name="x" /> and <paramref name="y" /> is contained within this
    ///     <see cref="RRect" />
    ///     structure; otherwise false.
    /// </returns>
    /// <param name="x">The x-coordinate of the point to test. </param>
    /// <param name="y">The y-coordinate of the point to test. </param>
    public bool Contains (double x, double y)
    {
        if (X <= x && x < X + Width && Y <= y)
        {
            return y < Y + Height;
        }

        return false;
    }

    /// <summary>
    ///     Determines if the specified point is contained within this <see cref="RRect" /> structure.
    /// </summary>
    /// <returns>
    ///     This method returns true if the point represented by the <paramref name="pt" /> parameter is contained within this
    ///     <see cref="RRect" />
    ///     structure; otherwise false.
    /// </returns>
    /// <param name="pt">The <see cref="RPoint" /> to test.</param>
    public bool Contains (RPoint pt)
    {
        return Contains (pt.X, pt.Y);
    }

    /// <summary>
    ///     Determines if the rectangular region represented by <paramref name="rect" /> is entirely contained within this
    ///     <see cref="RRect" />
    ///     structure.
    /// </summary>
    /// <returns>
    ///     This method returns true if the rectangular region represented by <paramref name="rect" /> is entirely contained within the rectangular region represented by this
    ///     <see cref="RRect" />
    ///     ; otherwise false.
    /// </returns>
    /// <param name="rect">
    ///     The <see cref="RRect" /> to test.
    /// </param>
    public bool Contains (RRect rect)
    {
        if (X <= rect.X && rect.X + rect.Width <= X + Width && Y <= rect.Y)
        {
            return rect.Y + rect.Height <= Y + Height;
        }

        return false;
    }

    /// <summary>
    ///     Inflates this <see cref="RRect" /> structure by the specified amount.
    /// </summary>
    /// <param name="x">
    ///     The amount to inflate this <see cref="RRect" /> structure horizontally.
    /// </param>
    /// <param name="y">
    ///     The amount to inflate this <see cref="RRect" /> structure vertically.
    /// </param>
    public void Inflate (double x, double y)
    {
        X -= x;
        Y -= y;
        Width += 2f * x;
        Height += 2f * y;
    }

    /// <summary>
    ///     Inflates this <see cref="RRect" /> by the specified amount.
    /// </summary>
    /// <param name="size">The amount to inflate this rectangle. </param>
    public void Inflate (RSize size)
    {
        Inflate (size.Width, size.Height);
    }

    /// <summary>
    ///     Creates and returns an inflated copy of the specified <see cref="RRect" /> structure. The copy is inflated by the specified amount. The original rectangle remains unmodified.
    /// </summary>
    /// <returns>
    ///     The inflated <see cref="RRect" />.
    /// </returns>
    /// <param name="rect">
    ///     The <see cref="RRect" /> to be copied. This rectangle is not modified.
    /// </param>
    /// <param name="x">The amount to inflate the copy of the rectangle horizontally. </param>
    /// <param name="y">The amount to inflate the copy of the rectangle vertically. </param>
    public static RRect Inflate (RRect rect, double x, double y)
    {
        var rectangleF = rect;
        rectangleF.Inflate (x, y);
        return rectangleF;
    }

    /// <summary>
    ///     Replaces this <see cref="RRect" /> structure with the intersection of itself and the specified
    ///     <see
    ///         cref="RRect" />
    ///     structure.
    /// </summary>
    /// <param name="rect">The rectangle to intersect. </param>
    public void Intersect (RRect rect)
    {
        var rectangleF = Intersect (rect, this);
        X = rectangleF.X;
        Y = rectangleF.Y;
        Width = rectangleF.Width;
        Height = rectangleF.Height;
    }

    /// <summary>
    ///     Returns a <see cref="RRect" /> structure that represents the intersection of two rectangles. If there is no intersection, and empty
    ///     <see
    ///         cref="RRect" />
    ///     is returned.
    /// </summary>
    /// <returns>
    ///     A third <see cref="RRect" /> structure the size of which represents the overlapped area of the two specified rectangles.
    /// </returns>
    /// <param name="a">A rectangle to intersect. </param>
    /// <param name="b">A rectangle to intersect. </param>
    public static RRect Intersect (RRect a, RRect b)
    {
        var x = Math.Max (a.X, b.X);
        var num1 = Math.Min (a.X + a.Width, b.X + b.Width);
        var y = Math.Max (a.Y, b.Y);
        var num2 = Math.Min (a.Y + a.Height, b.Y + b.Height);
        if (num1 >= x && num2 >= y)
        {
            return new RRect (x, y, num1 - x, num2 - y);
        }

        return Empty;
    }

    /// <summary>
    ///     Determines if this rectangle intersects with <paramref name="rect" />.
    /// </summary>
    /// <returns>
    ///     This method returns true if there is any intersection.
    /// </returns>
    /// <param name="rect">The rectangle to test. </param>
    public bool IntersectsWith (RRect rect)
    {
        if (rect.X < X + Width && X < rect.X + rect.Width && rect.Y < Y + Height)
        {
            return Y < rect.Y + rect.Height;
        }

        return false;
    }

    /// <summary>
    ///     Creates the smallest possible third rectangle that can contain both of two rectangles that form a union.
    /// </summary>
    /// <returns>
    ///     A third <see cref="RRect" /> structure that contains both of the two rectangles that form the union.
    /// </returns>
    /// <param name="a">A rectangle to union. </param>
    /// <param name="b">A rectangle to union. </param>
    public static RRect Union (RRect a, RRect b)
    {
        var x = Math.Min (a.X, b.X);
        var num1 = Math.Max (a.X + a.Width, b.X + b.Width);
        var y = Math.Min (a.Y, b.Y);
        var num2 = Math.Max (a.Y + a.Height, b.Y + b.Height);
        return new RRect (x, y, num1 - x, num2 - y);
    }

    /// <summary>
    ///     Adjusts the location of this rectangle by the specified amount.
    /// </summary>
    /// <param name="pos">The amount to offset the location. </param>
    public void Offset (RPoint pos)
    {
        Offset (pos.X, pos.Y);
    }

    /// <summary>
    ///     Adjusts the location of this rectangle by the specified amount.
    /// </summary>
    /// <param name="x">The amount to offset the location horizontally. </param>
    /// <param name="y">The amount to offset the location vertically. </param>
    public void Offset (double x, double y)
    {
        X += x;
        Y += y;
    }

    #region Object members

    /// <inheritdoc cref="ValueType.GetHashCode"/>
    public override int GetHashCode()
    {
        return (int)(uint)X ^ ((int)(uint)Y << 13 | (int)((uint)Y >> 19)) ^
               ((int)(uint)Width << 26 | (int)((uint)Width >> 6)) ^
               ((int)(uint)Height << 7 | (int)((uint)Height >> 25));
    }

    /// <inheritdoc cref="ValueType.ToString"/>
    public override string ToString()
    {
        return "{X=" + X + ",Y=" + Y + ",Width=" + Width + ",Height=" + Height + "}";
    }

    #endregion
}
