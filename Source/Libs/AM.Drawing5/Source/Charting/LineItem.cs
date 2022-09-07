// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* LineItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Runtime.Serialization;

#endregion

#nullable enable

namespace AM.Drawing.Charting;

/// <summary>
/// Encapsulates a curve type that is displayed as a line and/or a set of
/// symbols at each point.
/// </summary>
[Serializable]
public class LineItem
    : CurveItem, ICloneable
{
    #region Fields

    /// <summary>
    /// Private field that stores a reference to the <see cref="Charting.Line"/>
    /// class defined for this <see cref="LineItem"/>.  Use the public
    /// property <see cref="Line"/> to access this value.
    /// </summary>
    protected Line _line;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the <see cref="Charting.Symbol"/> class instance defined
    /// for this <see cref="LineItem"/>.
    /// </summary>
    public Symbol Symbol { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Charting.Line"/> class instance defined
    /// for this <see cref="LineItem"/>.
    /// </summary>
    public Line Line
    {
        get { return _line; }
        set { _line = value; }
    }

    /// <inheritdoc cref="CurveItem.IsZIncluded"/>
    internal override bool IsZIncluded (GraphPane pane)
    {
        return false;
    }

    /// <inheritdoc cref="CurveItem.IsXIndependent"/>
    internal override bool IsXIndependent (GraphPane pane)
    {
        return true;
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Create a new <see cref="LineItem"/>, specifying only the legend <see cref="CurveItem.Label" />.
    /// </summary>
    /// <param name="label">The _label that will appear in the legend.</param>
    public LineItem (string label)
        : base (label)
    {
        Symbol = new Symbol();
        _line = new Line();
    }

    /// <summary>
    /// Create a new <see cref="LineItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The _label that will appear in the legend.</param>
    /// <param name="x">An array of double precision values that define
    /// the independent (X axis) values for this curve</param>
    /// <param name="y">An array of double precision values that define
    /// the dependent (Y axis) values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> and <see cref="Symbol"/> properties.
    /// </param>
    /// <param name="symbolType">A <see cref="SymbolType"/> enum specifying the
    /// type of symbol to use for this <see cref="LineItem"/>.  Use <see cref="SymbolType.None"/>
    /// to hide the symbols.</param>
    /// <param name="lineWidth">The width (in points) to be used for the <see cref="Line"/>.  This
    /// width is scaled based on <see cref="PaneBase.CalcScaleFactor"/>.  Use a value of zero to
    /// hide the line (see <see cref="LineBase.IsVisible"/>).</param>
    public LineItem
        (
            string label,
            double[] x,
            double[] y,
            Color color,
            SymbolType symbolType,
            float lineWidth
        )
        : this (label, new PointPairList (x, y), color, symbolType, lineWidth)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Create a new <see cref="LineItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The _label that will appear in the legend.</param>
    /// <param name="x">An array of double precision values that define
    /// the independent (X axis) values for this curve</param>
    /// <param name="y">An array of double precision values that define
    /// the dependent (Y axis) values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> and <see cref="Symbol"/> properties.
    /// </param>
    /// <param name="symbolType">A <see cref="SymbolType"/> enum specifying the
    /// type of symbol to use for this <see cref="LineItem"/>.  Use <see cref="SymbolType.None"/>
    /// to hide the symbols.</param>
    public LineItem
        (
            string label,
            double[] x,
            double[] y,
            Color color,
            SymbolType symbolType
        )
        : this (label, new PointPairList (x, y), color, symbolType)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Create a new <see cref="LineItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The _label that will appear in the legend.</param>
    /// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
    /// the X and Y values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> and <see cref="Symbol"/> properties.
    /// </param>
    /// <param name="symbolType">A <see cref="SymbolType"/> enum specifying the
    /// type of symbol to use for this <see cref="LineItem"/>.  Use <see cref="SymbolType.None"/>
    /// to hide the symbols.</param>
    /// <param name="lineWidth">The width (in points) to be used for the <see cref="Line"/>.  This
    /// width is scaled based on <see cref="PaneBase.CalcScaleFactor"/>.  Use a value of zero to
    /// hide the line (see <see cref="LineBase.IsVisible"/>).</param>
    public LineItem
        (
            string label,
            IPointList points,
            Color color,
            SymbolType symbolType,
            float lineWidth
        )
        : base (label, points)
    {
        _line = new Line (color);
        if (lineWidth == 0)
        {
            _line.IsVisible = false;
        }
        else
        {
            _line.Width = lineWidth;
        }

        Symbol = new Symbol (symbolType, color);
    }

    /// <summary>
    /// Create a new <see cref="LineItem"/> using the specified properties.
    /// </summary>
    /// <param name="label">The _label that will appear in the legend.</param>
    /// <param name="points">A <see cref="IPointList"/> of double precision value pairs that define
    /// the X and Y values for this curve</param>
    /// <param name="color">A <see cref="Color"/> value that will be applied to
    /// the <see cref="Line"/> and <see cref="Symbol"/> properties.
    /// </param>
    /// <param name="symbolType">A <see cref="SymbolType"/> enum specifying the
    /// type of symbol to use for this <see cref="LineItem"/>.  Use <see cref="SymbolType.None"/>
    /// to hide the symbols.</param>
    public LineItem
        (
            string label,
            IPointList points,
            Color color,
            SymbolType symbolType
        )
        : this (label, points, color, symbolType, LineBase.Default.Width)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="LineItem"/> object from which to copy</param>
    public LineItem
        (
            LineItem rhs
        )
        : base (rhs)
    {
        Symbol = new Symbol (rhs.Symbol);
        _line = new Line (rhs.Line);
    }

    #endregion

    /// <inheritdoc cref="ICloneable.Clone"/>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    public LineItem Clone()
    {
        return new LineItem (this);
    }

    #region Serialization

    /// <summary>
    /// Current schema value that defines the version of the serialized file
    /// </summary>
    public const int schema2 = 10;

    /// <summary>
    /// Constructor for deserializing objects
    /// </summary>
    /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
    /// </param>
    /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
    /// </param>
    protected LineItem
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema2").NotUsed();

        Symbol = (Symbol)info.GetValue ("symbol", typeof (Symbol));
        _line = (Line)info.GetValue ("line", typeof (Line));
    }

    /// <inheritdoc cref="ISerializable.GetObjectData"/>
    public override void GetObjectData
        (
            SerializationInfo info,
            StreamingContext context
        )
    {
        base.GetObjectData (info, context);
        info.AddValue ("schema2", schema2);
        info.AddValue ("symbol", Symbol);
        info.AddValue ("line", _line);
    }

    #endregion

    #region Methods

    /// <inheritdoc cref="CurveItem.Draw"/>
    public override void Draw
        (
            Graphics graphics,
            GraphPane pane,
            int pos,
            float scaleFactor
        )
    {
        if (IsVisible)
        {
            Line.Draw (graphics, pane, this, scaleFactor);

            Symbol.Draw (graphics, pane, this, scaleFactor, IsSelected);
        }
    }

    /// <inheritdoc cref="CurveItem.DrawLegendKey"/>
    public override void DrawLegendKey
        (
            Graphics graphics,
            GraphPane pane,
            RectangleF rect,
            float scaleFactor
        )
    {
        // Draw a sample curve to the left of the label text
        var xMid = (int)(rect.Left + rect.Width / 2.0F);
        var yMid = (int)(rect.Top + rect.Height / 2.0F);

        //RectangleF rect2 = rect;
        //rect2.Y = yMid;
        //rect2.Height = rect.Height / 2.0f;

        _line.Fill.Draw (graphics, rect);

        _line.DrawSegment (graphics, pane, rect.Left, yMid, rect.Right, yMid, scaleFactor);

        // Draw a sample symbol to the left of the label text
        Symbol.DrawSymbol (graphics, pane, xMid, yMid, scaleFactor, false, null);
    }

    /// <inheritdoc cref="CurveItem.MakeUnique(AM.Drawing.Charting.ColorSymbolRotator)"/>
    public override void MakeUnique
        (
            ColorSymbolRotator rotator
        )
    {
        Color = rotator.NextColor;
        Symbol.Type = rotator.NextSymbol;
    }

    /// <inheritdoc cref="CurveItem.GetCoords"/>
    public override bool GetCoords
        (
            GraphPane pane,
            int i,
            out string coords
        )
    {
        coords = string.Empty;

        if (i < 0 || i >= Points.Count)
        {
            return false;
        }

        var pt = Points[i];
        if (pt.IsInvalid)
        {
            return false;
        }

        double x, y, z;
        var valueHandler = new ValueHandler (pane, false);
        valueHandler.GetValues (this, i, out x, out z, out y);

        var yAxis = GetYAxis (pane);
        var xAxis = GetXAxis (pane);

        var pixPt = new PointF (xAxis.Scale.Transform (IsOverrideOrdinal, i, x),
            yAxis.Scale.Transform (IsOverrideOrdinal, i, y));

        if (!pane.Chart.Rect.Contains (pixPt))
        {
            return false;
        }

        var halfSize = Symbol.Size * pane.CalcScaleFactor();

        coords = string.Format ("{0:f0},{1:f0},{2:f0},{3:f0}",
            pixPt.X - halfSize, pixPt.Y - halfSize,
            pixPt.X + halfSize, pixPt.Y + halfSize);

        return true;
    }

    #endregion
}
