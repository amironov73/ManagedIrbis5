// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* XGraphics.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing.Pdf;
using PdfSharpCore.Pdf.Advanced;

#endregion

#nullable enable

#pragma warning disable 1587

namespace PdfSharpCore.Drawing;

/// <summary>
/// Holds information about the current state of the XGraphics object.
/// </summary>
[Flags]
internal enum InternalGraphicsMode
{
    DrawingGdiGraphics,
    DrawingPdfContent,
    DrawingBitmap,
}

/// <summary>
/// Represents a drawing surface for a fixed size page.
/// </summary>
public sealed class XGraphics : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the XGraphics class for drawing on a PDF page.
    /// </summary>
    private XGraphics
        (
            PdfPage page,
            XGraphicsPdfPageOptions options,
            XGraphicsUnit pageUnit,
            XPageDirection pageDirection
        )
    {
        Sure.NotNull (page);

        _internals = null!;
        _transformer = null!;
        _associatedImage = null!;
        _form = null!;

        if (page.Owner == null)
        {
            throw new ArgumentException ("You cannot draw on a page that is not owned by a PdfDocument object.",
                nameof (page));
        }

        if (page.RenderContent != null)
        {
            throw new InvalidOperationException (
                "An XGraphics object already exists for this page and must be disposed before a new one can be created.");
        }

        if (page.Owner.IsReadOnly)
        {
            throw new InvalidOperationException (
                "Cannot create XGraphics for a page of a document that cannot be modified. Use PdfDocumentOpenMode.Modify.");
        }

        _gsStack = new GraphicsStateStack (this);
        PdfContent? content = null;
        switch (options)
        {
            case XGraphicsPdfPageOptions.Replace:
                page.Contents.Elements.Clear();
                goto case XGraphicsPdfPageOptions.Append;

            case XGraphicsPdfPageOptions.Prepend:
                content = page.Contents.PrependContent();
                break;

            case XGraphicsPdfPageOptions.Append:
                content = page.Contents.AppendContent();
                break;
        }

        page.RenderContent = content;
        _renderer = new XGraphicsPdfRenderer (page, this, options);
        _pageSizePoints = new XSize (page.Width, page.Height);
        switch (pageUnit)
        {
            case XGraphicsUnit.Point:
                _pageSize = new XSize (page.Width, page.Height);
                break;

            case XGraphicsUnit.Inch:
                _pageSize = new XSize (XUnit.FromPoint (page.Width).Inch, XUnit.FromPoint (page.Height).Inch);
                break;

            case XGraphicsUnit.Millimeter:
                _pageSize = new XSize (XUnit.FromPoint (page.Width).Millimeter,
                    XUnit.FromPoint (page.Height).Millimeter);
                break;

            case XGraphicsUnit.Centimeter:
                _pageSize = new XSize (XUnit.FromPoint (page.Width).Centimeter,
                    XUnit.FromPoint (page.Height).Centimeter);
                break;

            case XGraphicsUnit.Presentation:
                _pageSize = new XSize (XUnit.FromPoint (page.Width).Presentation,
                    XUnit.FromPoint (page.Height).Presentation);
                break;

            default:
                throw new NotImplementedException ("unit");
        }

        _pageUnit = pageUnit;
        _pageDirection = pageDirection;

        Initialize();
    }

    private XGraphics
        (
            XSize size,
            XGraphicsUnit pageUnit,
            XPageDirection pageDirection
        )
    {
        _internals = null!;
        _transformer = null!;
        _associatedImage = null!;
        _form = null!;
        _renderer = null!;

        _gsStack = new GraphicsStateStack (this);
        _pageSizePoints = new XSize (size.Width, size.Height);
        switch (pageUnit)
        {
            case XGraphicsUnit.Point:
                _pageSize = new XSize (size.Width, size.Height);
                break;

            case XGraphicsUnit.Inch:
                _pageSize = new XSize (XUnit.FromPoint (size.Width).Inch, XUnit.FromPoint (size.Height).Inch);
                break;

            case XGraphicsUnit.Millimeter:
                _pageSize = new XSize (XUnit.FromPoint (size.Width).Millimeter,
                    XUnit.FromPoint (size.Height).Millimeter);
                break;

            case XGraphicsUnit.Centimeter:
                _pageSize = new XSize (XUnit.FromPoint (size.Width).Centimeter,
                    XUnit.FromPoint (size.Height).Centimeter);
                break;

            case XGraphicsUnit.Presentation:
                _pageSize = new XSize (XUnit.FromPoint (size.Width).Presentation,
                    XUnit.FromPoint (size.Height).Presentation);
                break;

            default:
                throw new NotImplementedException ("unit");
        }

        _pageUnit = pageUnit;
        _pageDirection = pageDirection;

        Initialize();
    }

    private XGraphics
        (
            IXGraphicsRenderer renderer,
            XSize size,
            XGraphicsUnit pageUnit,
            XPageDirection pageDirection
        )
    {
        _internals = null!;
        _transformer = null!;
        _associatedImage = null!;
        _form = null!;
        _renderer = null!;

        _renderer = renderer ?? throw new ArgumentNullException (nameof (renderer));
        _gsStack = new GraphicsStateStack (this);
        _pageSizePoints = new XSize (size.Width, size.Height);
        switch (pageUnit)
        {
            case XGraphicsUnit.Point:
                _pageSize = new XSize (size.Width, size.Height);
                break;

            case XGraphicsUnit.Inch:
                _pageSize = new XSize (XUnit.FromPoint (size.Width).Inch, XUnit.FromPoint (size.Height).Inch);
                break;

            case XGraphicsUnit.Millimeter:
                _pageSize = new XSize (XUnit.FromPoint (size.Width).Millimeter,
                    XUnit.FromPoint (size.Height).Millimeter);
                break;

            case XGraphicsUnit.Centimeter:
                _pageSize = new XSize (XUnit.FromPoint (size.Width).Centimeter,
                    XUnit.FromPoint (size.Height).Centimeter);
                break;

            case XGraphicsUnit.Presentation:
                _pageSize = new XSize (XUnit.FromPoint (size.Width).Presentation,
                    XUnit.FromPoint (size.Height).Presentation);
                break;

            default:
                throw new NotImplementedException ($"{nameof (pageUnit)}: {pageUnit}");
        }

        _pageUnit = pageUnit;
        _pageDirection = pageDirection;

        Initialize();
    }

    /// <summary>
    /// Initializes a new instance of the XGraphics class used for drawing on a form.
    /// </summary>
    private XGraphics (XForm form)
    {
        Sure.NotNull (form);

        _internals = null!;
        _transformer = null!;
        _associatedImage = null!;
        _renderer = null!;

        _form = form;
        form.AssociateGraphics (this);

        _gsStack = new GraphicsStateStack (this);
#if NET5_0
            _drawGraphics = false;
            if (form.Owner != null)
                _renderer = new XGraphicsPdfRenderer(form, this);
            _pageSize = form.Size;
            Initialize();
#endif
    }

    /// <summary>
    /// Creates the measure context. This is a graphics context created only for querying measures of text.
    /// Drawing on a measure context has no effect.
    /// </summary>
    public static XGraphics CreateMeasureContext (XSize size, XGraphicsUnit pageUnit, XPageDirection pageDirection)
    {
        return new XGraphics (size, pageUnit, pageDirection);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="renderer"></param>
    /// <param name="size"></param>
    /// <param name="pageUnit"></param>
    /// <param name="pageDirection"></param>
    /// <returns></returns>
    public static XGraphics FromRenderer
        (
            IXGraphicsRenderer renderer,
            XSize size,
            XGraphicsUnit pageUnit,
            XPageDirection pageDirection
        )
    {
        return new XGraphics (renderer, size, pageUnit, pageDirection);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Pdf.PdfPage object.
    /// </summary>
    public static XGraphics FromPdfPage (PdfPage page)
    {
        return new XGraphics (page, XGraphicsPdfPageOptions.Append, XGraphicsUnit.Point, XPageDirection.Downwards);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Pdf.PdfPage object.
    /// </summary>
    public static XGraphics FromPdfPage (PdfPage page, XGraphicsUnit unit)
    {
        return new XGraphics (page, XGraphicsPdfPageOptions.Append, unit, XPageDirection.Downwards);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Pdf.PdfPage object.
    /// </summary>
    public static XGraphics FromPdfPage (PdfPage page, XPageDirection pageDirection)
    {
        return new XGraphics (page, XGraphicsPdfPageOptions.Append, XGraphicsUnit.Point, pageDirection);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Pdf.PdfPage object.
    /// </summary>
    public static XGraphics FromPdfPage (PdfPage page, XGraphicsPdfPageOptions options)
    {
        return new XGraphics (page, options, XGraphicsUnit.Point, XPageDirection.Downwards);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Pdf.PdfPage object.
    /// </summary>
    public static XGraphics FromPdfPage (PdfPage page, XGraphicsPdfPageOptions options, XPageDirection pageDirection)
    {
        return new XGraphics (page, options, XGraphicsUnit.Point, pageDirection);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Pdf.PdfPage object.
    /// </summary>
    public static XGraphics FromPdfPage (PdfPage page, XGraphicsPdfPageOptions options, XGraphicsUnit unit)
    {
        return new XGraphics (page, options, unit, XPageDirection.Downwards);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Pdf.PdfPage object.
    /// </summary>
    public static XGraphics FromPdfPage (PdfPage page, XGraphicsPdfPageOptions options, XGraphicsUnit unit,
        XPageDirection pageDirection)
    {
        return new XGraphics (page, options, unit, pageDirection);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Drawing.XPdfForm object.
    /// </summary>
    public static XGraphics FromPdfForm (XPdfForm form)
    {
        if (form.Gfx != null!)
        {
            return form.Gfx;
        }

        return new XGraphics (form);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Drawing.XForm object.
    /// </summary>
    public static XGraphics FromForm (XForm form)
    {
        if (form.Gfx != null!)
        {
            return form.Gfx;
        }

        return new XGraphics (form);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Drawing.XForm object.
    /// </summary>
    public static XGraphics? FromImage (XImage image)
    {
        return FromImage (image, XGraphicsUnit.Point);
    }

    /// <summary>
    /// Creates a new instance of the XGraphics class from a PdfSharpCore.Drawing.XImage object.
    /// </summary>
    public static XGraphics? FromImage (XImage image, XGraphicsUnit unit)
    {
        Sure.NotNull (image);

        if (image is XBitmapImage bmImage)
        {
        }

        return null;
    }

    /// <summary>
    /// Internal setup.
    /// </summary>
    private void Initialize()
    {
        _pageOrigin = new XPoint();

        var pageHeight = _pageSize.Height;
        var targetPage = PdfPage;
        var trimOffset = new XPoint();
        if (targetPage != null! && targetPage.TrimMargins.AreSet)
        {
            pageHeight += targetPage.TrimMargins.Top.Point + targetPage.TrimMargins.Bottom.Point;
            trimOffset = new XPoint (targetPage.TrimMargins.Left.Point, targetPage.TrimMargins.Top.Point);
        }

        var matrix = new XMatrix();
        if (_pageDirection != XPageDirection.Downwards)
        {
            matrix.Prepend (new XMatrix (1, 0, 0, -1, 0, pageHeight));
        }

        if (trimOffset != new XPoint())
        {
            matrix.TranslatePrepend (trimOffset.X, -trimOffset.Y);
        }

        DefaultViewMatrix = matrix;
        _transform = new XMatrix();
    }

    /// <summary>
    /// Releases all resources used by this object.
    /// </summary>
    public void Dispose()
    {
        Dispose (true);
    }

    private void Dispose (bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
            if (disposing)
            {
                // Dispose managed resources.
                if (_associatedImage != null!)
                {
                    _associatedImage.DisassociateWithGraphics (this);
                    _associatedImage = null!;
                }
            }

            if (_form != null!)
            {
                _form.Finish();
            }

            _drawGraphics = false;

            if (_renderer != null!)
            {
                _renderer.Close();
                _renderer = null!;
            }
        }
    }

    private bool _disposed;

    /// <summary>
    /// Internal hack for MigraDoc. Will be removed in further releases.
    /// Unicode support requires a global refactoring of MigraDoc and will be done in further releases.
    /// </summary>

    // ReSharper disable once InconsistentNaming
    // ReSharper disable once ConvertToAutoProperty
    public PdfFontEncoding MUH // MigraDoc Unicode Hack...
    {
        get => _muh;
        set => _muh = value;
    }

    private PdfFontEncoding _muh;

    /// <summary>
    /// Gets or sets the unit of measure used for page coordinates.
    /// CURRENTLY ONLY POINT IS IMPLEMENTED.
    /// </summary>
    public XGraphicsUnit PageUnit => _pageUnit;

    //set
    //{
    //  // TODO: other page units
    //  if (value != XGraphicsUnit.Point)
    //    throw new NotImplementedException("PageUnit must be XGraphicsUnit.Point in current implementation.");
    //}
    private readonly XGraphicsUnit _pageUnit;

    /// <summary>
    /// Gets or sets the a value indicating in which direction y-value grow.
    /// </summary>
    public XPageDirection PageDirection
    {
        get => _pageDirection;
        set
        {
            // Is there really anybody who needes the concept of XPageDirection.Upwards?
            if (value != XPageDirection.Downwards)
            {
                throw new NotImplementedException (
                    "PageDirection must be XPageDirection.Downwards in current implementation.");
            }
        }
    }

    private readonly XPageDirection _pageDirection;

    /// <summary>
    /// Gets the current page origin. Setting the origin is not yet implemented.
    /// </summary>
    public XPoint PageOrigin
    {
        get => _pageOrigin;
        set
        {
            // Is there really anybody who needes to set the page origin?
            if (value != new XPoint())
            {
                throw new NotImplementedException ("PageOrigin cannot be modified in current implementation.");
            }
        }
    }

    private XPoint _pageOrigin;

    /// <summary>
    /// Gets the current size of the page.
    /// </summary>
    public XSize PageSize => _pageSize;

    //set
    //{
    //  //TODO
    //  throw new NotImplementedException("PageSize cannot be modified in current implementation.");
    //}
    private XSize _pageSize;
    private XSize _pageSizePoints;

    #region Drawing

    // ----- DrawLine -----------------------------------------------------------------------------

    /// <summary>
    /// Draws a line connecting two XPoint structures.
    /// </summary>
    public void DrawLine (XPen pen, XPoint pt1, XPoint pt2)
    {
        DrawLine (pen, pt1.X, pt1.Y, pt2.X, pt2.Y);
    }

    /// <summary>
    /// Draws a line connecting the two points specified by coordinate pairs.
    /// </summary>
    public void DrawLine (XPen pen, double x1, double y1, double x2, double y2)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (_renderer != null!)
        {
            _renderer.DrawLines (pen, new XPoint[] { new XPoint (x1, y1), new XPoint (x2, y2) });
        }
    }

    // ----- DrawLines ----------------------------------------------------------------------------
    /// <summary>
    /// Draws a series of line segments that connect an array of points.
    /// </summary>
    public void DrawLines (XPen pen, XPoint[] points)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (points == null)
        {
            throw new ArgumentNullException (nameof (points));
        }

        if (points.Length < 2)
        {
            throw new ArgumentException ("points", PSSR.PointArrayAtLeast (2));
        }

        if (_renderer != null!)
        {
            _renderer.DrawLines (pen, points);
        }
    }

    /// <summary>
    /// Draws a series of line segments that connect an array of x and y pairs.
    /// </summary>
    public void DrawLines (XPen pen, double x, double y, params double[] value)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (value == null)
        {
            throw new ArgumentNullException (nameof (value));
        }

        var length = value.Length;
        var points = new XPoint[length / 2 + 1];
        points[0].X = x;
        points[0].Y = y;
        for (var idx = 0; idx < length / 2; idx++)
        {
            points[idx + 1].X = value[2 * idx];
            points[idx + 1].Y = value[2 * idx + 1];
        }

        DrawLines (pen, points);
    }

    // ----- DrawBezier ---------------------------------------------------------------------------
    /// <summary>
    /// Draws a B�zier spline defined by four points.
    /// </summary>
    public void DrawBezier (XPen pen, XPoint pt1, XPoint pt2, XPoint pt3, XPoint pt4)
    {
        DrawBezier (pen, pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y, pt4.X, pt4.Y);
    }

    /// <summary>
    /// Draws a B�zier spline defined by four points.
    /// </summary>
    public void DrawBezier (XPen pen, double x1, double y1, double x2, double y2,
        double x3, double y3, double x4, double y4)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (_renderer != null!)
        {
            _renderer.DrawBeziers (pen,
                new XPoint[] { new XPoint (x1, y1), new XPoint (x2, y2), new XPoint (x3, y3), new XPoint (x4, y4) });
        }
    }

    // ----- DrawBeziers --------------------------------------------------------------------------

    /// <summary>
    /// Draws a series of B�zier splines from an array of points.
    /// </summary>
    public void DrawBeziers (XPen pen, XPoint[] points)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        var count = points.Length;
        if (count == 0)
        {
            return;
        }

        if ((count - 1) % 3 != 0)
        {
            throw new ArgumentException ("Invalid number of points for bezier curves. Number must fulfil 4+3n.",
                nameof (points));
        }

        if (_renderer != null!)
        {
            _renderer.DrawBeziers (pen, points);
        }
    }

    // ----- DrawCurve ----------------------------------------------------------------------------

    /// <summary>
    /// Draws a cardinal spline through a specified array of points.
    /// </summary>
    public void DrawCurve (XPen pen, XPoint[] points)
    {
        DrawCurve (pen, points, 0.5);
    }

    /// <summary>
    /// Draws a cardinal spline through a specified array of point using a specified tension.
    /// The drawing begins offset from the beginning of the array.
    /// </summary>
    public void DrawCurve (XPen pen, XPoint[] points, int offset, int numberOfSegments, double tension)
    {
        var points2 = new XPoint[numberOfSegments];
        Array.Copy (points, offset, points2, 0, numberOfSegments);
        DrawCurve (pen, points2, tension);
    }

    /// <summary>
    /// Draws a cardinal spline through a specified array of points using a specified tension.
    /// </summary>
    public void DrawCurve (XPen pen, XPoint[] points, double tension)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (points == null)
        {
            throw new ArgumentNullException (nameof (points));
        }

        var count = points.Length;
        if (count < 2)
        {
            throw new ArgumentException ("DrawCurve requires two or more points.", nameof (points));
        }

        if (_renderer != null!)
        {
            _renderer.DrawCurve (pen, points, tension);
        }
    }

    // ----- DrawArc ------------------------------------------------------------------------------

    /// <summary>
    /// Draws an arc representing a portion of an ellipse.
    /// </summary>
    public void DrawArc (XPen pen, XRect rect, double startAngle, double sweepAngle)
    {
        DrawArc (pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
    }

    /// <summary>
    /// Draws an arc representing a portion of an ellipse.
    /// </summary>
    public void DrawArc (XPen pen, double x, double y, double width, double height, double startAngle,
        double sweepAngle)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (Math.Abs (sweepAngle) >= 360)
        {
            DrawEllipse (pen, x, y, width, height);
        }
        else
        {
            if (_renderer != null!)
            {
                _renderer.DrawArc (pen, x, y, width, height, startAngle, sweepAngle);
            }
        }
    }

    // ----- DrawRectangle ------------------------------------------------------------------------

    // ----- stroke -----

    /// <summary>
    /// Draws a rectangle.
    /// </summary>
    public void DrawRectangle (XPen pen, XRect rect)
    {
        DrawRectangle (pen, rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Draws a rectangle.
    /// </summary>
    public void DrawRectangle (XPen pen, double x, double y, double width, double height)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (_drawGraphics)
        {
        }

        if (_renderer != null!)
        {
            _renderer.DrawRectangle (pen, null!, x, y, width, height);
        }
    }

    // ----- fill -----

    /// <summary>
    /// Draws a rectangle.
    /// </summary>
    public void DrawRectangle (XBrush brush, XRect rect)
    {
        DrawRectangle (brush, rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Draws a rectangle.
    /// </summary>
    public void DrawRectangle (XBrush brush, double x, double y, double width, double height)
    {
        if (brush == null)
        {
            throw new ArgumentNullException (nameof (brush));
        }

        if (_renderer != null!)
        {
            _renderer.DrawRectangle (null!, brush, x, y, width, height);
        }
    }

    // ----- stroke and fill -----


    /// <summary>
    /// Draws a rectangle.
    /// </summary>
    public void DrawRectangle (XPen pen, XBrush brush, XRect rect)
    {
        DrawRectangle (pen, brush, rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Draws a rectangle.
    /// </summary>
    public void DrawRectangle (XPen pen, XBrush brush, double x, double y, double width, double height)
    {
        if (pen == null && brush == null)
        {
            throw new ArgumentNullException ("pen and brush", PSSR.NeedPenOrBrush);
        }

        if (_renderer != null!)
        {
            _renderer.DrawRectangle (pen!, brush, x, y, width, height);
        }
    }

    // ----- DrawRectangles -----------------------------------------------------------------------

    // ----- stroke -----

    /// <summary>
    /// Draws a series of rectangles.
    /// </summary>
    public void DrawRectangles (XPen pen, XRect[] rectangles)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (rectangles == null)
        {
            throw new ArgumentNullException (nameof (rectangles));
        }

        DrawRectangles (pen, null!, rectangles);
    }

    // ----- fill -----

    /// <summary>
    /// Draws a series of rectangles.
    /// </summary>
    public void DrawRectangles (XBrush brush, XRect[] rectangles)
    {
        if (brush == null)
        {
            throw new ArgumentNullException (nameof (brush));
        }

        if (rectangles == null)
        {
            throw new ArgumentNullException (nameof (rectangles));
        }

        DrawRectangles (null!, brush, rectangles);
    }

    // ----- stroke and fill -----

    /// <summary>
    /// Draws a series of rectangles.
    /// </summary>
    public void DrawRectangles (XPen pen, XBrush brush, XRect[] rectangles)
    {
        if (pen == null && brush == null)
        {
            throw new ArgumentNullException ("pen and brush", PSSR.NeedPenOrBrush);
        }

        if (rectangles == null)
        {
            throw new ArgumentNullException (nameof (rectangles));
        }

        var count = rectangles.Length;

        if (_renderer != null!)
        {
            for (var idx = 0; idx < count; idx++)
            {
                var rect = rectangles[idx];
                _renderer.DrawRectangle (pen!, brush, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }
    }

    // ----- DrawRoundedRectangle -----------------------------------------------------------------

    // ----- stroke -----

    /// <summary>
    /// Draws a rectangles with round corners.
    /// </summary>
    public void DrawRoundedRectangle (XPen pen, XRect rect, XSize ellipseSize)
    {
        DrawRoundedRectangle (pen, rect.X, rect.Y, rect.Width, rect.Height, ellipseSize.Width, ellipseSize.Height);
    }

    /// <summary>
    /// Draws a rectangles with round corners.
    /// </summary>
    public void DrawRoundedRectangle (XPen pen, double x, double y, double width, double height, double ellipseWidth,
        double ellipseHeight)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        DrawRoundedRectangle (pen, null!, x, y, width, height, ellipseWidth, ellipseHeight);
    }

    // ----- fill -----

    /// <summary>
    /// Draws a rectangles with round corners.
    /// </summary>
    public void DrawRoundedRectangle (XBrush brush, XRect rect, XSize ellipseSize)
    {
        DrawRoundedRectangle (brush, rect.X, rect.Y, rect.Width, rect.Height, ellipseSize.Width, ellipseSize.Height);
    }

    /// <summary>
    /// Draws a rectangles with round corners.
    /// </summary>
    public void DrawRoundedRectangle (XBrush brush, double x, double y, double width, double height,
        double ellipseWidth, double ellipseHeight)
    {
        if (brush == null)
        {
            throw new ArgumentNullException (nameof (brush));
        }

        DrawRoundedRectangle (null!, brush, x, y, width, height, ellipseWidth, ellipseHeight);
    }

    // ----- stroke and fill -----

    /// <summary>
    /// Draws a rectangles with round corners.
    /// </summary>
    public void DrawRoundedRectangle (XPen pen, XBrush brush, XRect rect, XSize ellipseSize)
    {
        DrawRoundedRectangle (pen, brush, rect.X, rect.Y, rect.Width, rect.Height, ellipseSize.Width,
            ellipseSize.Height);
    }

    /// <summary>
    /// Draws a rectangles with round corners.
    /// </summary>
    public void DrawRoundedRectangle (XPen pen, XBrush brush, double x, double y, double width, double height,
        double ellipseWidth, double ellipseHeight)
    {
        if (pen == null && brush == null)
        {
            throw new ArgumentNullException ("pen and brush", PSSR.NeedPenOrBrush);
        }

        if (_renderer != null!)
        {
            _renderer.DrawRoundedRectangle (pen!, brush, x, y, width, height, ellipseWidth, ellipseHeight);
        }
    }

    // ----- DrawEllipse --------------------------------------------------------------------------

    // ----- stroke -----

    /// <summary>
    /// Draws an ellipse defined by a bounding rectangle.
    /// </summary>
    public void DrawEllipse (XPen pen, XRect rect)
    {
        DrawEllipse (pen, rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Draws an ellipse defined by a bounding rectangle.
    /// </summary>
    public void DrawEllipse (XPen pen, double x, double y, double width, double height)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        // No DrawArc defined?
        if (_drawGraphics)
        {
        }

        if (_renderer != null!)
        {
            _renderer.DrawEllipse (pen, null!, x, y, width, height);
        }
    }

    // ----- fill -----

    /// <summary>
    /// Draws an ellipse defined by a bounding rectangle.
    /// </summary>
    public void DrawEllipse (XBrush brush, XRect rect)
    {
        DrawEllipse (brush, rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Draws an ellipse defined by a bounding rectangle.
    /// </summary>
    public void DrawEllipse (XBrush brush, double x, double y, double width, double height)
    {
        if (brush == null)
        {
            throw new ArgumentNullException (nameof (brush));
        }

        if (_drawGraphics)
        {
        }

        if (_renderer != null!)
        {
            _renderer.DrawEllipse (null!, brush, x, y, width, height);
        }
    }

    // ----- stroke and fill -----

    /// <summary>
    /// Draws an ellipse defined by a bounding rectangle.
    /// </summary>
    public void DrawEllipse (XPen pen, XBrush brush, XRect rect)
    {
        DrawEllipse (pen, brush, rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Draws an ellipse defined by a bounding rectangle.
    /// </summary>
    public void DrawEllipse (XPen pen, XBrush brush, double x, double y, double width, double height)
    {
        if (pen == null && brush == null)
        {
            throw new ArgumentNullException ("pen and brush", PSSR.NeedPenOrBrush);
        }

        if (_renderer != null!)
        {
            _renderer.DrawEllipse (pen!, brush, x, y, width, height);
        }
    }

    /// <summary>
    /// Draws a polygon defined by an array of points.
    /// </summary>
    public void DrawPolygon (XPen pen, XPoint[] points)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (points == null)
        {
            throw new ArgumentNullException (nameof (points));
        }

        if (points.Length < 2)
        {
            throw new ArgumentException ("points", PSSR.PointArrayAtLeast (2));
        }

        if (_renderer != null!)
        {
            _renderer.DrawPolygon (pen, null!, points, XFillMode.Alternate); // XFillMode is ignored
        }
    }

    // ----- fill -----

    /// <summary>
    /// Draws a polygon defined by an array of points.
    /// </summary>
    public void DrawPolygon (XBrush brush, XPoint[] points, XFillMode fillmode)
    {
        if (brush == null)
        {
            throw new ArgumentNullException (nameof (brush));
        }

        if (points == null)
        {
            throw new ArgumentNullException (nameof (points));
        }

        if (points.Length < 2)
        {
            throw new ArgumentException ("points", PSSR.PointArrayAtLeast (2));
        }

        if (_renderer != null!)
        {
            _renderer.DrawPolygon (null!, brush, points, fillmode);
        }
    }

    // ----- stroke and fill -----

    /// <summary>
    /// Draws a polygon defined by an array of points.
    /// </summary>
    public void DrawPolygon (XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode)
    {
        if (pen == null && brush == null)
        {
            throw new ArgumentNullException ("pen and brush", PSSR.NeedPenOrBrush);
        }

        if (points == null)
        {
            throw new ArgumentNullException (nameof (points));
        }

        if (points.Length < 2)
        {
            throw new ArgumentException ("points", PSSR.PointArrayAtLeast (2));
        }

        if (_renderer != null!)
        {
            _renderer.DrawPolygon (pen!, brush, points, fillmode);
        }
    }

    // ----- DrawPie ------------------------------------------------------------------------------

    // ----- stroke -----


    /// <summary>
    /// Draws a pie defined by an ellipse.
    /// </summary>
    public void DrawPie (XPen pen, XRect rect, double startAngle, double sweepAngle)
    {
        DrawPie (pen, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
    }

    /// <summary>
    /// Draws a pie defined by an ellipse.
    /// </summary>
    public void DrawPie (XPen pen, double x, double y, double width, double height, double startAngle,
        double sweepAngle)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen), PSSR.NeedPenOrBrush);
        }

        if (_renderer != null!)
        {
            _renderer.DrawPie (pen, null!, x, y, width, height, startAngle, sweepAngle);
        }
    }

    // ----- fill -----

    /// <summary>
    /// Draws a pie defined by an ellipse.
    /// </summary>
    public void DrawPie (XBrush brush, XRect rect, double startAngle, double sweepAngle)
    {
        DrawPie (brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
    }

    /// <summary>
    /// Draws a pie defined by an ellipse.
    /// </summary>
    public void DrawPie (XBrush brush, double x, double y, double width, double height, double startAngle,
        double sweepAngle)
    {
        if (brush == null)
        {
            throw new ArgumentNullException (nameof (brush), PSSR.NeedPenOrBrush);
        }

        if (_renderer != null!)
        {
            _renderer.DrawPie (null!, brush, x, y, width, height, startAngle, sweepAngle);
        }
    }

    // ----- stroke and fill -----

    /// <summary>
    /// Draws a pie defined by an ellipse.
    /// </summary>
    public void DrawPie (XPen pen, XBrush brush, XRect rect, double startAngle, double sweepAngle)
    {
        DrawPie (pen, brush, rect.X, rect.Y, rect.Width, rect.Height, startAngle, sweepAngle);
    }

    /// <summary>
    /// Draws a pie defined by an ellipse.
    /// </summary>
    public void DrawPie (XPen pen, XBrush brush, double x, double y, double width, double height, double startAngle,
        double sweepAngle)
    {
        if (pen == null && brush == null)
        {
            throw new ArgumentNullException (nameof (pen), PSSR.NeedPenOrBrush);
        }

        if (_renderer != null!)
        {
            _renderer.DrawPie (pen!, brush, x, y, width, height, startAngle, sweepAngle);
        }
    }

    // ----- DrawClosedCurve ----------------------------------------------------------------------

    // ----- stroke -----

    /// <summary>
    /// Draws a closed cardinal spline defined by an array of points.
    /// </summary>
    public void DrawClosedCurve (XPen pen, XPoint[] points)
    {
        DrawClosedCurve (pen, null!, points, XFillMode.Alternate, 0.5);
    }


    /// <summary>
    /// Draws a closed cardinal spline defined by an array of points.
    /// </summary>
    public void DrawClosedCurve (XPen pen, XPoint[] points, double tension)
    {
        DrawClosedCurve (pen, null!, points, XFillMode.Alternate, tension);
    }

    // ----- fill -----

    /// <summary>
    /// Draws a closed cardinal spline defined by an array of points.
    /// </summary>
    public void DrawClosedCurve (XBrush brush, XPoint[] points)
    {
        DrawClosedCurve (null!, brush, points, XFillMode.Alternate, 0.5);
    }

    /// <summary>
    /// Draws a closed cardinal spline defined by an array of points.
    /// </summary>
    public void DrawClosedCurve (XBrush brush, XPoint[] points, XFillMode fillmode)
    {
        DrawClosedCurve (null!, brush, points, fillmode, 0.5);
    }

    /// <summary>
    /// Draws a closed cardinal spline defined by an array of points.
    /// </summary>
    public void DrawClosedCurve (XBrush brush, XPoint[] points, XFillMode fillmode, double tension)
    {
        DrawClosedCurve (null!, brush, points, fillmode, tension);
    }

    // ----- stroke and fill -----

    /// <summary>
    /// Draws a closed cardinal spline defined by an array of points.
    /// </summary>
    public void DrawClosedCurve (XPen pen, XBrush brush, XPoint[] points)
    {
        DrawClosedCurve (pen, brush, points, XFillMode.Alternate, 0.5);
    }

    /// <summary>
    /// Draws a closed cardinal spline defined by an array of points.
    /// </summary>
    public void DrawClosedCurve (XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode)
    {
        DrawClosedCurve (pen, brush, points, fillmode, 0.5);
    }

    /// <summary>
    /// Draws a closed cardinal spline defined by an array of points.
    /// </summary>
    public void DrawClosedCurve (XPen pen, XBrush brush, XPoint[] points, XFillMode fillmode, double tension)
    {
        if (pen == null && brush == null)
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException ("pen and brush", PSSR.NeedPenOrBrush);
        }

        var count = points.Length;
        if (count == 0)
        {
            return;
        }

        if (count < 2)
        {
            throw new ArgumentException ("Not enough points.", nameof (points));
        }

        if (_renderer != null!)
        {
            _renderer.DrawClosedCurve (pen!, brush, points, tension, fillmode);
        }
    }

    // ----- DrawPath -----------------------------------------------------------------------------

    // ----- stroke -----

    /// <summary>
    /// Draws a graphical path.
    /// </summary>
    public void DrawPath (XPen pen, XGraphicsPath path)
    {
        if (pen == null)
        {
            throw new ArgumentNullException (nameof (pen));
        }

        if (path == null)
        {
            throw new ArgumentNullException (nameof (path));
        }

        if (_renderer != null!)
        {
            _renderer.DrawPath (pen, null!, path);
        }
    }

    // ----- fill -----

    /// <summary>
    /// Draws a graphical path.
    /// </summary>
    public void DrawPath (XBrush brush, XGraphicsPath path)
    {
        if (brush == null)
        {
            throw new ArgumentNullException (nameof (brush));
        }

        if (path == null)
        {
            throw new ArgumentNullException (nameof (path));
        }

        if (_renderer != null!)
        {
            _renderer.DrawPath (null!, brush, path);
        }
    }

    // ----- stroke and fill -----

    /// <summary>
    /// Draws a graphical path.
    /// </summary>
    public void DrawPath (XPen pen, XBrush brush, XGraphicsPath path)
    {
        if (pen == null && brush == null)
        {
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException ("pen and brush", PSSR.NeedPenOrBrush);
        }

        if (path == null)
        {
            throw new ArgumentNullException (nameof (path));
        }

        if (_renderer != null!)
        {
            _renderer.DrawPath (pen!, brush, path);
        }
    }

    // ----- DrawString ---------------------------------------------------------------------------

    /// <summary>
    /// Draws the specified text string.
    /// </summary>
    public void DrawString (string s, XFont font, XBrush brush, XPoint point)
    {
        DrawString (s, font, brush, new XRect (point.X, point.Y, 0, 0), XStringFormats.Default);
    }


    /// <summary>
    /// Draws the specified text string.
    /// </summary>
    public void DrawString (string s, XFont font, XBrush brush, XPoint point, XStringFormat format)
    {
        DrawString (s, font, brush, new XRect (point.X, point.Y, 0, 0), format);
    }

    /// <summary>
    /// Draws the specified text string.
    /// </summary>
    public void DrawString (string s, XFont font, XBrush brush, double x, double y)
    {
        DrawString (s, font, brush, new XRect (x, y, 0, 0), XStringFormats.Default);
    }

    /// <summary>
    /// Draws the specified text string.
    /// </summary>
    public void DrawString (string s, XFont font, XBrush brush, double x, double y, XStringFormat format)
    {
        DrawString (s, font, brush, new XRect (x, y, 0, 0), format);
    }


    /// <summary>
    /// Draws the specified text string.
    /// </summary>
    public void DrawString (string s, XFont font, XBrush brush, XRect layoutRectangle)
    {
        DrawString (s, font, brush, layoutRectangle, XStringFormats.Default);
    }

    /// <summary>
    /// Draws the specified text string.
    /// </summary>
    public void DrawString (string text, XFont font, XBrush brush, XRect layoutRectangle, XStringFormat format)
    {
        if (text == null)
        {
            throw new ArgumentNullException (nameof (text));
        }

        if (font == null)
        {
            throw new ArgumentNullException (nameof (font));
        }

        if (brush == null)
        {
            throw new ArgumentNullException (nameof (brush));
        }

        if (format is { LineAlignment: XLineAlignment.BaseLine } && layoutRectangle.Height != 0)
        {
            throw new InvalidOperationException (
                "DrawString: With XLineAlignment.BaseLine the height of the layout rectangle must be 0.");
        }

        if (text.Length == 0)
        {
            return;
        }

        if (format == null!)
        {
            format = XStringFormats.Default;
        }

        if (_renderer != null!)
        {
            _renderer.DrawString (text, font, brush, layoutRectangle, format);
        }
    }

    // ----- MeasureString ------------------------------------------------------------------------

    /// <summary>
    /// Measures the specified string when drawn with the specified font.
    /// </summary>
    public XSize MeasureString (string text, XFont font, XStringFormat stringFormat)
    {
        if (text == null)
        {
            throw new ArgumentNullException (nameof (text));
        }

        if (font == null)
        {
            throw new ArgumentNullException (nameof (font));
        }

        if (stringFormat == null)
        {
            throw new ArgumentNullException (nameof (stringFormat));
        }

        var size = FontHelper.MeasureString (text, font, XStringFormats.Default);
        return size;
    }

    /// <summary>
    /// Measures the specified string when drawn with the specified font.
    /// </summary>
    public XSize MeasureString (string text, XFont font)
    {
        return MeasureString (text, font, XStringFormats.Default);
    }

    // ----- DrawImage ----------------------------------------------------------------------------

    /// <summary>
    /// Draws the specified image.
    /// </summary>
    public void DrawImage (XImage image, XPoint point)
    {
        DrawImage (image, point.X, point.Y);
    }

    /// <summary>
    /// Draws the specified image.
    /// </summary>
    public void DrawImage (XImage image, double x, double y)
    {
        if (image == null)
        {
            throw new ArgumentNullException (nameof (image));
        }

        CheckXPdfFormConsistence (image);

        var width = image.PointWidth;
        var height = image.PointHeight;

        if (_renderer != null!)
        {
            _renderer.DrawImage (image, x, y, image.PointWidth, image.PointHeight);
        }

        //image.Width * 72 / image.HorizontalResolution,
        //image.Height * 72 / image.HorizontalResolution);
    }

    /// <summary>
    /// Draws the specified image.
    /// </summary>
    public void DrawImage (XImage image, XRect rect)
    {
        DrawImage (image, rect.X, rect.Y, rect.Width, rect.Height);
    }

    /// <summary>
    /// Draws the specified image.
    /// </summary>
    public void DrawImage (XImage image, double x, double y, double width, double height)
    {
        if (image == null)
        {
            throw new ArgumentNullException (nameof (image));
        }

        CheckXPdfFormConsistence (image);

        if (_renderer != null!)
        {
            _renderer.DrawImage (image, x, y, width, height);
        }
    }

    // TODO: calculate destination size
    //public void DrawImage(XImage image, double x, double y, GdiRectF srcRect, XGraphicsUnit srcUnit)
    //public void DrawImage(XImage image, double x, double y, XRect srcRect, XGraphicsUnit srcUnit)

    /// <summary>
    /// Draws the specified image.
    /// </summary>
    public void DrawImage (XImage image, XRect destRect, XRect srcRect, XGraphicsUnit srcUnit)
    {
        if (image == null)
        {
            throw new ArgumentNullException (nameof (image));
        }

        CheckXPdfFormConsistence (image);

        if (_renderer != null!)
        {
            _renderer.DrawImage (image, destRect, srcRect, srcUnit);
        }
    }

    //TODO?
    //public void DrawImage(XImage image, Rectangle destRect, double srcX, double srcY, double srcWidth, double srcHeight, GraphicsUnit srcUnit);
    //public void DrawImage(XImage image, Rectangle destRect, double srcX, double srcY, double srcWidth, double srcHeight, GraphicsUnit srcUnit);

    private void DrawMissingImageRect (XRect rect)
    {
    }

    /// <summary>
    /// Checks whether drawing is allowed and disposes the XGraphics object, if necessary.
    /// </summary>
    private void CheckXPdfFormConsistence (XImage image)
    {
        if (image is XForm xForm)
        {
            // Force disposing of XGraphics that draws the content
            xForm.Finish();

            // ReSharper disable once MergeSequentialChecks
            if (_renderer != null! && (_renderer as XGraphicsPdfRenderer) != null)
            {
                if (xForm.Owner != null && xForm.Owner != ((XGraphicsPdfRenderer)_renderer).Owner)
                {
                    throw new InvalidOperationException (
                        "A XPdfForm object is always bound to the document it was created for and cannot be drawn in the context of another document.");
                }

                if (xForm == ((XGraphicsPdfRenderer)_renderer)._form)
                {
                    throw new InvalidOperationException (
                        "A XPdfForm cannot be drawn on itself.");
                }
            }
        }
    }

    // ----- DrawBarCode --------------------------------------------------------------------------

    /// <summary>
    /// Draws the specified bar code.
    /// </summary>
    public void DrawBarCode (BarCodes.BarCode barcode, XPoint position)
    {
        barcode.Render (this, XBrushes.Black, null!, position);
    }

    /// <summary>
    /// Draws the specified bar code.
    /// </summary>
    public void DrawBarCode (BarCodes.BarCode barcode, XBrush brush, XPoint position)
    {
        barcode.Render (this, brush, null!, position);
    }

    /// <summary>
    /// Draws the specified bar code.
    /// </summary>
    public void DrawBarCode (BarCodes.BarCode barcode, XBrush brush, XFont font, XPoint position)
    {
        barcode.Render (this, brush, font, position);
    }

    // ----- DrawMatrixCode -----------------------------------------------------------------------

    /// <summary>
    /// Draws the specified data matrix code.
    /// </summary>
    public void DrawMatrixCode (BarCodes.MatrixCode matrixcode, XPoint position)
    {
        matrixcode.Render (this, XBrushes.Black, position);
    }

    /// <summary>
    /// Draws the specified data matrix code.
    /// </summary>
    public void DrawMatrixCode (BarCodes.MatrixCode matrixcode, XBrush brush, XPoint position)
    {
        matrixcode.Render (this, brush, position);
    }

    #endregion

    // --------------------------------------------------------------------------------------------

    #region Save and Restore

    /// <summary>
    /// Saves the current state of this XGraphics object and identifies the saved state with the
    /// returned XGraphicsState object.
    /// </summary>
    public XGraphicsState Save()
    {
        var xState = new XGraphicsState();
        var iState = new InternalGraphicsState (this, xState)
        {
            Transform = _transform
        };
        _gsStack.Push (iState);

        if (_renderer != null!)
        {
            _renderer.Save (xState);
        }

        return xState;
    }

    /// <summary>
    /// Restores the state of this XGraphics object to the state represented by the specified
    /// XGraphicsState object.
    /// </summary>
    public void Restore (XGraphicsState state)
    {
        if (state == null)
        {
            throw new ArgumentNullException (nameof (state));
        }

        _gsStack.Restore (state.InternalState);
        _transform = state.InternalState.Transform;

        if (_renderer != null!)
        {
            _renderer.Restore (state);
        }
    }

    /// <summary>
    /// Restores the state of this XGraphics object to the state before the most recently call of Save.
    /// </summary>
    public void Restore()
    {
        if (_gsStack.Count == 0)
        {
            throw new InvalidOperationException ("Cannot restore without preceding save operation.");
        }

        Restore (_gsStack.Current.State!);
    }

    /// <summary>
    /// Saves a graphics container with the current state of this XGraphics and
    /// opens and uses a new graphics container.
    /// </summary>
    public XGraphicsContainer BeginContainer()
    {
        return BeginContainer (new XRect (0, 0, 1, 1), new XRect (0, 0, 1, 1), XGraphicsUnit.Point);
    }

    /// <summary>
    /// Saves a graphics container with the current state of this XGraphics and
    /// opens and uses a new graphics container.
    /// </summary>
    public XGraphicsContainer BeginContainer (XRect dstrect, XRect srcrect, XGraphicsUnit unit)
    {
        // TODO: unit
        if (unit != XGraphicsUnit.Point)
        {
            throw new ArgumentException ("The current implementation supports XGraphicsUnit.Point only.", nameof (unit));
        }

        var xContainer = new XGraphicsContainer();

        var iState = new InternalGraphicsState (this, xContainer)
        {
            Transform = _transform
        };

        _gsStack.Push (iState);

        if (_renderer != null!)
        {
            _renderer.BeginContainer (xContainer, dstrect, srcrect, unit);
        }

        var matrix = new XMatrix();
        var scaleX = dstrect.Width / srcrect.Width;
        var scaleY = dstrect.Height / srcrect.Height;
        matrix.TranslatePrepend (-srcrect.X, -srcrect.Y);
        matrix.ScalePrepend (scaleX, scaleY);
        matrix.TranslatePrepend (dstrect.X / scaleX, dstrect.Y / scaleY);
        AddTransform (matrix, XMatrixOrder.Prepend);

        return xContainer;
    }

    /// <summary>
    /// Closes the current graphics container and restores the state of this XGraphics
    /// to the state saved by a call to the BeginContainer method.
    /// </summary>
    public void EndContainer (XGraphicsContainer container)
    {
        if (container == null)
        {
            throw new ArgumentNullException (nameof (container));
        }

        _gsStack.Restore (container.InternalState);
        _transform = container.InternalState.Transform;

        if (_renderer != null!)
        {
            _renderer.EndContainer (container);
        }
    }

    /// <summary>
    /// Gets the current graphics state level. The default value is 0. Each call of Save or BeginContainer
    /// increased and each call of Restore or EndContainer decreased the value by 1.
    /// </summary>
    public int GraphicsStateLevel => _gsStack.Count;

    #endregion

    // --------------------------------------------------------------------------------------------

    #region Properties

    /// <summary>
    /// Gets or sets the smoothing mode.
    /// </summary>
    /// <value>The smoothing mode.</value>
    public XSmoothingMode SmoothingMode
    {
        get => _smoothingMode;
        set => _smoothingMode = value;
    }

    private XSmoothingMode _smoothingMode;

    #endregion

    // --------------------------------------------------------------------------------------------

    #region Transformation

    /// <summary>
    /// Applies the specified translation operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// </summary>
    public void TranslateTransform (double dx, double dy)
    {
        AddTransform (XMatrix.CreateTranslation (dx, dy), XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Applies the specified translation operation to the transformation matrix of this object
    /// in the specified order.
    /// </summary>
    public void TranslateTransform (double dx, double dy, XMatrixOrder order)
    {
        var matrix = new XMatrix();
        matrix.TranslatePrepend (dx, dy);
        AddTransform (matrix, order);
    }

    /// <summary>
    /// Applies the specified scaling operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// </summary>
    public void ScaleTransform (double scaleX, double scaleY)
    {
        AddTransform (XMatrix.CreateScaling (scaleX, scaleY), XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Applies the specified scaling operation to the transformation matrix of this object
    /// in the specified order.
    /// </summary>
    public void ScaleTransform (double scaleX, double scaleY, XMatrixOrder order)
    {
        var matrix = new XMatrix();
        matrix.ScalePrepend (scaleX, scaleY);
        AddTransform (matrix, order);
    }

    /// <summary>
    /// Applies the specified scaling operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// </summary>

    // ReSharper disable once InconsistentNaming
    public void ScaleTransform (double scaleXY)
    {
        ScaleTransform (scaleXY, scaleXY);
    }

    /// <summary>
    /// Applies the specified scaling operation to the transformation matrix of this object
    /// in the specified order.
    /// </summary>

    // ReSharper disable once InconsistentNaming
    public void ScaleTransform (double scaleXY, XMatrixOrder order)
    {
        ScaleTransform (scaleXY, scaleXY, order);
    }

    /// <summary>
    /// Applies the specified scaling operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// </summary>
    public void ScaleAtTransform (double scaleX, double scaleY, double centerX, double centerY)
    {
        AddTransform (XMatrix.CreateScaling (scaleX, scaleY, centerX, centerY), XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Applies the specified scaling operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// </summary>
    public void ScaleAtTransform (double scaleX, double scaleY, XPoint center)
    {
        AddTransform (XMatrix.CreateScaling (scaleX, scaleY, center.X, center.Y), XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Applies the specified rotation operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// </summary>
    public void RotateTransform (double angle)
    {
        AddTransform (XMatrix.CreateRotationRadians (angle * Const.Deg2Rad), XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Applies the specified rotation operation to the transformation matrix of this object
    /// in the specified order. The angle unit of measure is degree.
    /// </summary>
    public void RotateTransform (double angle, XMatrixOrder order)
    {
        var matrix = new XMatrix();
        matrix.RotatePrepend (angle);
        AddTransform (matrix, order);
    }

    /// <summary>
    /// Applies the specified rotation operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// </summary>
    public void RotateAtTransform (double angle, XPoint point)
    {
        AddTransform (XMatrix.CreateRotationRadians (angle * Const.Deg2Rad, point.X, point.Y), XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Applies the specified rotation operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// </summary>
    public void RotateAtTransform (double angle, XPoint point, XMatrixOrder order)
    {
        AddTransform (XMatrix.CreateRotationRadians (angle * Const.Deg2Rad, point.X, point.Y), order);
    }

    /// <summary>
    /// Applies the specified shearing operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// ShearTransform is a synonym for SkewAtTransform.
    /// Parameter shearX specifies the horizontal skew which is measured in degrees counterclockwise from the y-axis.
    /// Parameter shearY specifies the vertical skew which is measured in degrees counterclockwise from the x-axis.
    /// </summary>
    public void ShearTransform (double shearX, double shearY)
    {
        AddTransform (XMatrix.CreateSkewRadians (shearX * Const.Deg2Rad, shearY * Const.Deg2Rad), XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Applies the specified shearing operation to the transformation matrix of this object
    /// in the specified order.
    /// ShearTransform is a synonym for SkewAtTransform.
    /// Parameter shearX specifies the horizontal skew which is measured in degrees counterclockwise from the y-axis.
    /// Parameter shearY specifies the vertical skew which is measured in degrees counterclockwise from the x-axis.
    /// </summary>
    public void ShearTransform (double shearX, double shearY, XMatrixOrder order)
    {
        AddTransform (XMatrix.CreateSkewRadians (shearX * Const.Deg2Rad, shearY * Const.Deg2Rad), order);
    }

    /// <summary>
    /// Applies the specified shearing operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// ShearTransform is a synonym for SkewAtTransform.
    /// Parameter shearX specifies the horizontal skew which is measured in degrees counterclockwise from the y-axis.
    /// Parameter shearY specifies the vertical skew which is measured in degrees counterclockwise from the x-axis.
    /// </summary>
    public void SkewAtTransform (double shearX, double shearY, double centerX, double centerY)
    {
        AddTransform (XMatrix.CreateSkewRadians (shearX * Const.Deg2Rad, shearY * Const.Deg2Rad, centerX, centerY),
            XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Applies the specified shearing operation to the transformation matrix of this object by
    /// prepending it to the object's transformation matrix.
    /// ShearTransform is a synonym for SkewAtTransform.
    /// Parameter shearX specifies the horizontal skew which is measured in degrees counterclockwise from the y-axis.
    /// Parameter shearY specifies the vertical skew which is measured in degrees counterclockwise from the x-axis.
    /// </summary>
    public void SkewAtTransform (double shearX, double shearY, XPoint center)
    {
        AddTransform (XMatrix.CreateSkewRadians (shearX * Const.Deg2Rad, shearY * Const.Deg2Rad, center.X, center.Y),
            XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Multiplies the transformation matrix of this object and specified matrix.
    /// </summary>
    public void MultiplyTransform (XMatrix matrix)
    {
        AddTransform (matrix, XMatrixOrder.Prepend);
    }

    /// <summary>
    /// Multiplies the transformation matrix of this object and specified matrix in the specified order.
    /// </summary>
    public void MultiplyTransform (XMatrix matrix, XMatrixOrder order)
    {
        AddTransform (matrix, order);
    }

    /// <summary>
    /// Gets the current transformation matrix.
    /// The transformation matrix canot be set. Insted use Save/Restore or BeginContainer/EndContainer to
    /// save the state before Transform is called and later restore to the previous transform.
    /// </summary>
    public XMatrix Transform => _transform;

    /// <summary>
    /// Applies a new transformation to the current transformation matrix.
    /// </summary>
    private void AddTransform (XMatrix transform, XMatrixOrder order)
    {
        var matrix = _transform;
        matrix.Multiply (transform, order);
        _transform = matrix;
        matrix = DefaultViewMatrix;
        matrix.Multiply (_transform, XMatrixOrder.Prepend);
        if (_renderer != null!)
        {
            _renderer.AddTransform (transform, XMatrixOrder.Prepend);
        }
    }

    #endregion

    // --------------------------------------------------------------------------------------------

    #region Clipping

    /// <summary>
    /// Updates the clip region of this XGraphics to the intersection of the
    /// current clip region and the specified rectangle.
    /// </summary>
    public void IntersectClip (XRect rect)
    {
        var path = new XGraphicsPath();
        path.AddRectangle (rect);
        IntersectClip (path);
    }

    /// <summary>
    /// Updates the clip region of this XGraphics to the intersection of the
    /// current clip region and the specified graphical path.
    /// </summary>
    public void IntersectClip (XGraphicsPath path)
    {
        if (path == null)
        {
            throw new ArgumentNullException (nameof (path));
        }

        if (_renderer != null!)
        {
            _renderer.SetClip (path, XCombineMode.Intersect);
        }
    }

    //public void SetClip(Graphics g);
    //public void SetClip(Graphics g, CombineMode combineMode);
    //public void SetClip(GraphicsPath path, CombineMode combineMode);
    //public void SetClip(Rectangle rect, CombineMode combineMode);
    //public void SetClip(GdiRectF rect, CombineMode combineMode);
    //public void SetClip(Region region, CombineMode combineMode);
    //public void IntersectClip(Region region);
    //public void ExcludeClip(Region region);

    #endregion

    // --------------------------------------------------------------------------------------------

    #region Miscellaneous

    /// <summary>
    /// Writes a comment to the output stream. Comments have no effect on the rendering of the output.
    /// They may be useful to mark a position in a content stream of a PDF document.
    /// </summary>
    public void WriteComment (string comment)
    {
        if (comment == null)
        {
            throw new ArgumentNullException (nameof (comment));
        }

        if (_drawGraphics)
        {
            // TODO: Do something if metafile?
        }

        if (_renderer != null!)
        {
            _renderer.WriteComment (comment);
        }
    }

    /// <summary>
    /// Permits access to internal data.
    /// </summary>
    public XGraphicsInternals Internals => _internals ??= new XGraphicsInternals (this);

    private XGraphicsInternals _internals;

    /// <summary>
    /// (Under construction. May change in future versions.)
    /// </summary>
    public SpaceTransformer Transformer => _transformer ?? (_transformer = new SpaceTransformer (this));

    private SpaceTransformer _transformer;

    #endregion

    // --------------------------------------------------------------------------------------------

    internal void DisassociateImage()
    {
        if (_associatedImage == null)
        {
            throw new InvalidOperationException ("No image associated.");
        }

        Dispose();
    }

    internal InternalGraphicsMode InternalGraphicsMode
    {
        get => _internalGraphicsMode;
        set => _internalGraphicsMode = value;
    }

    private InternalGraphicsMode _internalGraphicsMode;

    internal XImage AssociatedImage
    {
        get => _associatedImage;
        set => _associatedImage = value;
    }

    private XImage _associatedImage;

    /// <summary>
    /// The transformation matrix from the XGraphics page space to the Graphics world space.
    /// (The name 'default view matrix' comes from Microsoft OS/2 Presentation Manager. I choose
    /// this name because I have no better one.)
    /// </summary>
    internal XMatrix DefaultViewMatrix;

    /// <summary>
    /// Indicates whether to send drawing operations to _gfx or _dc.
    /// </summary>
    private bool _drawGraphics;

    private readonly XForm _form;

    /// <summary>
    /// Interface to an (optional) renderer. Currently it is the XGraphicsPdfRenderer, if defined.
    /// </summary>
    private IXGraphicsRenderer _renderer;

    /// <summary>
    /// The transformation matrix from XGraphics world space to page unit space.
    /// </summary>
    private XMatrix _transform;

    /// <summary>
    /// The graphics state stack.
    /// </summary>
    private readonly GraphicsStateStack _gsStack;

    /// <summary>
    /// Gets the PDF page that serves as drawing surface if PDF is rendered,
    /// or null, if no such object exists.
    /// </summary>
    public PdfPage? PdfPage
    {
        get
        {
            return _renderer is XGraphicsPdfRenderer renderer ? renderer._page : null;
        }
    }

    /// <summary>
    /// Provides access to internal data structures of the XGraphics class.
    /// </summary>
    public class XGraphicsInternals
    {
        internal XGraphicsInternals (XGraphics gfx)
        {
            _gfx = gfx;
        }

        private readonly XGraphics _gfx;
    }

    /// <summary>
    /// (This class is under construction.)
    /// Currently used in MigraDoc
    /// </summary>
    public class SpaceTransformer
    {
        internal SpaceTransformer (XGraphics gfx)
        {
            _gfx = gfx;
        }

        private readonly XGraphics _gfx;

        /// <summary>
        /// Gets the smallest rectangle in default page space units that completely encloses the specified rect
        /// in world space units.
        /// </summary>
        public XRect WorldToDefaultPage (XRect rect)
        {
            var points = new XPoint[4];
            points[0] = new XPoint (rect.X, rect.Y);
            points[1] = new XPoint (rect.X + rect.Width, rect.Y);
            points[2] = new XPoint (rect.X, rect.Y + rect.Height);
            points[3] = new XPoint (rect.X + rect.Width, rect.Y + rect.Height);

            var matrix = _gfx.Transform;
            matrix.TransformPoints (points);

            var height = _gfx.PageSize.Height;
            points[0].Y = height - points[0].Y;
            points[1].Y = height - points[1].Y;
            points[2].Y = height - points[2].Y;
            points[3].Y = height - points[3].Y;

            var xmin = Math.Min (Math.Min (points[0].X, points[1].X), Math.Min (points[2].X, points[3].X));
            var xmax = Math.Max (Math.Max (points[0].X, points[1].X), Math.Max (points[2].X, points[3].X));
            var ymin = Math.Min (Math.Min (points[0].Y, points[1].Y), Math.Min (points[2].Y, points[3].Y));
            var ymax = Math.Max (Math.Max (points[0].Y, points[1].Y), Math.Max (points[2].Y, points[3].Y));

            return new XRect (xmin, ymin, xmax - xmin, ymax - ymin);
        }
    }
}
