// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* TextObj.cs --
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
/// A class that represents a text object on the graph.  A list of
/// <see cref="GraphObj"/> objects is maintained by the
/// <see cref="GraphObjList"/> collection class.
/// </summary>
[Serializable]
public class TextObj
    : GraphObj, ICloneable
{
    #region Fields

    /// <summary> Private field to store the actual text string for this
    /// <see cref="TextObj"/>.  Use the public property <see cref="TextObj.Text"/>
    /// to access this value.
    /// </summary>
    private string _text;

    /// <summary>
    /// Private field to store the <see cref="FontSpec"/> class used to render
    /// this <see cref="TextObj"/>.  Use the public property <see cref="FontSpec"/>
    /// to access this class.
    /// </summary>
    private FontSpec _fontSpec;

    /*
    /// <summary>
    /// Private field to indicate whether this <see cref="TextObj"/> is to be
    /// wrapped when rendered.  Wrapping is to be done within <see cref="TextObj.wrappedRect"/>.
    /// Use the public property <see cref="TextObj.IsWrapped"/>
    /// to access this value.
    /// </summary>
    private bool isWrapped;
    */

    /// <summary>
    /// Private field holding the SizeF into which this <see cref="TextObj"/>
    /// should be rendered. Use the public property <see cref="TextObj.LayoutArea"/>
    /// to access this value.
    /// </summary>
    private SizeF _layoutArea;

    #endregion

    #region Defaults

    /// <summary>
    /// A simple struct that defines the
    /// default property values for the <see cref="TextObj"/> class.
    /// </summary>
    public new struct Default
    {
        /*
        /// <summary>
        /// The default wrapped flag for rendering this <see cref="TextObj,Text"/>.
        /// </summary>
        public static bool IsWrapped = false ;
        /// <summary>
        /// The default RectangleF for rendering this <see cref="TextObj.Text"/>
        /// </summary>
        public static SizeF WrappedSize = new SizeF( 0,0 );
        */

        /// <summary>
        /// The default font family for the <see cref="TextObj"/> text
        /// (<see cref="FontSpec.Family"/> property).
        /// </summary>
        public static string FontFamily = "Arial";

        /// <summary>
        /// The default font size for the <see cref="TextObj"/> text
        /// (<see cref="FontSpec.Size"/> property).  Units are
        /// in points (1/72 inch).
        /// </summary>
        public static float FontSize = 12.0F;

        /// <summary>
        /// The default font color for the <see cref="TextObj"/> text
        /// (<see cref="FontSpec.FontColor"/> property).
        /// </summary>
        public static Color FontColor = Color.Black;

        /// <summary>
        /// The default font bold mode for the <see cref="TextObj"/> text
        /// (<see cref="FontSpec.IsBold"/> property). true
        /// for a bold typeface, false otherwise.
        /// </summary>
        public static bool FontBold = false;

        /// <summary>
        /// The default font underline mode for the <see cref="TextObj"/> text
        /// (<see cref="FontSpec.IsUnderline"/> property). true
        /// for an underlined typeface, false otherwise.
        /// </summary>
        public static bool FontUnderline = false;

        /// <summary>
        /// The default font italic mode for the <see cref="TextObj"/> text
        /// (<see cref="FontSpec.IsItalic"/> property). true
        /// for an italic typeface, false otherwise.
        /// </summary>
        public static bool FontItalic = false;
    }

    #endregion

    #region Properties

    /*
    /// <summary>
    ///
    /// </summary>
    internal bool IsWrapped
    {
        get { return (this.isWrapped); }
        set { this.isWrapped = value; }
    }
    */

    /// <summary>
    ///
    /// </summary>
    public SizeF LayoutArea
    {
        get => _layoutArea;
        set => _layoutArea = value;
    }


    /// <summary>
    /// The <see cref="TextObj"/> to be displayed.  This text can be multi-line by
    /// including newline ('\n') characters between the lines.
    /// </summary>
    public string Text
    {
        get => _text;
        set => _text = value;
    }

    /// <summary>
    /// Gets a reference to the <see cref="FontSpec"/> class used to render
    /// this <see cref="TextObj"/>
    /// </summary>
    /// <seealso cref="Default.FontColor"/>
    /// <seealso cref="Default.FontBold"/>
    /// <seealso cref="Default.FontItalic"/>
    /// <seealso cref="Default.FontUnderline"/>
    /// <seealso cref="Default.FontFamily"/>
    /// <seealso cref="Default.FontSize"/>
    public FontSpec FontSpec
    {
        get => _fontSpec;
        set
        {
            Sure.NotNull (value);

            _fontSpec = value;
        }
    }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor that sets all <see cref="TextObj"/> properties to default
    /// values as defined in the <see cref="Default"/> class.
    /// </summary>
    /// <param name="text">The text to be displayed.</param>
    /// <param name="x">The x position of the text.  The units
    /// of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.  The text will be
    /// aligned to this position based on the <see cref="AlignH"/>
    /// property.</param>
    /// <param name="y">The y position of the text.  The units
    /// of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.  The text will be
    /// aligned to this position based on the
    /// <see cref="AlignV"/> property.</param>
    public TextObj (string text, double x, double y)
        : base (x, y)
    {
        Init (text);
    }

    private void Init (string text)
    {
        if (text != null)
        {
            _text = text;
        }
        else
        {
            text = "Text";
        }

        _fontSpec = new FontSpec (
            Default.FontFamily, Default.FontSize,
            Default.FontColor, Default.FontBold,
            Default.FontItalic, Default.FontUnderline);

        //this.isWrapped = Default.IsWrapped ;
        _layoutArea = new SizeF (0, 0);
    }

    /// <summary>
    /// Constructor that sets all <see cref="TextObj"/> properties to default
    /// values as defined in the <see cref="Default"/> class.
    /// </summary>
    /// <param name="text">The text to be displayed.</param>
    /// <param name="x">The x position of the text.  The units
    /// of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.  The text will be
    /// aligned to this position based on the <see cref="AlignH"/>
    /// property.</param>
    /// <param name="y">The y position of the text.  The units
    /// of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.  The text will be
    /// aligned to this position based on the
    /// <see cref="AlignV"/> property.</param>
    /// <param name="coordType">The <see cref="CoordType"/> enum value that
    /// indicates what type of coordinate system the x and y parameters are
    /// referenced to.</param>
    public TextObj (string text, double x, double y, CoordType coordType)
        : base (x, y, coordType)
    {
        Init (text);
    }

    /// <summary>
    /// Constructor that sets all <see cref="TextObj"/> properties to default
    /// values as defined in the <see cref="Default"/> class.
    /// </summary>
    /// <param name="text">The text to be displayed.</param>
    /// <param name="x">The x position of the text.  The units
    /// of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.  The text will be
    /// aligned to this position based on the <see cref="AlignH"/>
    /// property.</param>
    /// <param name="y">The y position of the text.  The units
    /// of this position are specified by the
    /// <see cref="Location.CoordinateFrame"/> property.  The text will be
    /// aligned to this position based on the
    /// <see cref="AlignV"/> property.</param>
    /// <param name="coordType">The <see cref="CoordType"/> enum value that
    /// indicates what type of coordinate system the x and y parameters are
    /// referenced to.</param>
    /// <param name="alignH">The <see cref="AlignH"/> enum that specifies
    /// the horizontal alignment of the object with respect to the (x,y) location</param>
    /// <param name="alignV">The <see cref="AlignV"/> enum that specifies
    /// the vertical alignment of the object with respect to the (x,y) location</param>
    public TextObj (string text, double x, double y, CoordType coordType, AlignH alignH, AlignV alignV)
        : base (x, y, coordType, alignH, alignV)
    {
        Init (text);
    }

    /// <summary>
    /// Parameterless constructor that initializes a new <see cref="TextObj"/>.
    /// </summary>
    public TextObj() : base (0, 0)
    {
        Init ("");
    }

    /// <summary>
    /// The Copy Constructor
    /// </summary>
    /// <param name="rhs">The <see cref="TextObj"/> object from which to copy</param>
    public TextObj (TextObj rhs) : base (rhs)
    {
        _text = rhs.Text;
        _fontSpec = new FontSpec (rhs.FontSpec);
    }

    /// <summary>
    /// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
    /// calling the typed version of <see cref="Clone" />
    /// </summary>
    /// <returns>A deep copy of this object</returns>
    object ICloneable.Clone()
    {
        return Clone();
    }

    /// <summary>
    /// Typesafe, deep-copy clone method.
    /// </summary>
    /// <returns>A new, independent copy of this class</returns>
    public TextObj Clone()
    {
        return new TextObj (this);
    }

    #endregion

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
    protected TextObj
        (
            SerializationInfo info,
            StreamingContext context
        )
        : base (info, context)
    {
        // The schema value is just a file version parameter.  You can use it to make future versions
        // backwards compatible as new member variables are added to classes
        info.GetInt32 ("schema2").NotUsed();

        _text = info.GetString ("text");
        _fontSpec = (FontSpec)info.GetValue ("fontSpec", typeof (FontSpec));

        //isWrapped = info.GetBoolean ("isWrapped") ;
        _layoutArea = (SizeF)info.GetValue ("layoutArea", typeof (SizeF));
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
        info.AddValue ("text", _text);
        info.AddValue ("fontSpec", _fontSpec);

        //info.AddValue( "isWrapped", isWrapped );
        info.AddValue ("layoutArea", _layoutArea);
    }

    #endregion

    #region Rendering Methods

    /// <inheritdoc cref="GraphObj.Draw"/>
    public override void Draw
        (
            Graphics graphics,
            PaneBase pane,
            float scaleFactor
        )
    {
        // transform the x,y location from the user-defined
        // coordinate frame to the screen pixel location
        var pix = _location.Transform (pane);

        // Draw the text on the screen, including any frame and background
        // fill elements
        if (pix.X > -100000 && pix.X < 100000 && pix.Y > -100000 && pix.Y < 100000)
        {
            //if ( this.layoutSize.IsEmpty )
            //	this.FontSpec.Draw( g, pane.IsPenWidthScaled, this.text, pix.X, pix.Y,
            //		this.location.AlignH, this.location.AlignV, scaleFactor );
            //else
            FontSpec.Draw (graphics, pane, _text, pix.X, pix.Y,
                _location.AlignH, _location.AlignV, scaleFactor, _layoutArea);
        }
    }

    /// <inheritdoc cref="GraphObj.PointInBox"/>
    public override bool PointInBox
        (
            PointF point,
            PaneBase pane,
            Graphics graphics,
            float scaleFactor
        )
    {
        if (!base.PointInBox (point, pane, graphics, scaleFactor))
        {
            return false;
        }

        // transform the x,y location from the user-defined
        // coordinate frame to the screen pixel location
        var pix = _location.Transform (pane);

        return _fontSpec.PointInBox (point, graphics, _text, pix.X, pix.Y,
            _location.AlignH, _location.AlignV, scaleFactor, LayoutArea);
    }

    /// <inheritdoc cref="GraphObj.GetCoords"/>
    public override void GetCoords
        (
            PaneBase pane,
            Graphics graphics,
            float scaleFactor,
            out string shape,
            out string coords
        )
    {
        // transform the x,y location from the user-defined
        // coordinate frame to the screen pixel location
        var pix = _location.Transform (pane);

        var pts = _fontSpec.GetBox (graphics, _text, pix.X, pix.Y, _location.AlignH,
            _location.AlignV, scaleFactor, new SizeF());

        shape = "poly";
        coords = string.Format ("{0:f0},{1:f0},{2:f0},{3:f0},{4:f0},{5:f0},{6:f0},{7:f0},",
            pts[0].X, pts[0].Y, pts[1].X, pts[1].Y,
            pts[2].X, pts[2].Y, pts[3].X, pts[3].Y);
    }

    #endregion
}
