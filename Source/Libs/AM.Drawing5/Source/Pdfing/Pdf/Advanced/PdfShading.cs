// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Pdf;
using PdfSharpCore.Pdf.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Represents a shading dictionary.
/// </summary>
public sealed class PdfShading : PdfDictionary
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PdfShading"/> class.
    /// </summary>
    public PdfShading (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    internal void SetupFromBrush
        (
            XBaseGradientBrush brush,
            XGraphicsPdfRenderer renderer
        )
    {
        if (brush is XRadialGradientBrush radialBrush)
        {
            SetupFromBrush (radialBrush, renderer);
        }
        else if (brush is XLinearGradientBrush linearBrush)
        {
            SetupFromBrush (linearBrush, renderer);
        }
        else
        {
            throw new ArgumentException ("Unsupoorted XGradientBrush: " + brush, nameof (brush));
        }
    }

    internal void SetupFromBrush (XRadialGradientBrush brush, XGraphicsPdfRenderer renderer)
    {
        if (brush == null)
        {
            throw new ArgumentNullException ("brush");
        }

        var colorMode = _document.Options.ColorMode;
        var color1 = ColorSpaceHelper.EnsureColorMode (colorMode, brush._color1);
        var color2 = ColorSpaceHelper.EnsureColorMode (colorMode, brush._color2);

        var function = new PdfDictionary();

        Elements[Keys.ShadingType] = new PdfInteger (3);
        if (colorMode != PdfColorMode.Cmyk)
        {
            Elements[Keys.ColorSpace] = new PdfName ("/DeviceRGB");
        }
        else
        {
            Elements[Keys.ColorSpace] = new PdfName ("/DeviceCMYK");
        }

        var p1 = renderer.WorldToView (brush._center1);
        var p2 = renderer.WorldToView (brush._center2);

        var rv1 = renderer.WorldToView (new XPoint (brush._r1 + brush._center1.X, brush._center1.Y));
        var rv2 = renderer.WorldToView (new XPoint (brush._r2 + brush._center2.X, brush._center2.Y));

        var dx1 = rv1.X - p1.X;
        var dy1 = rv1.Y - p1.Y;
        var dx2 = rv2.X - p2.X;
        var dy2 = rv2.Y - p2.Y;

        var r1 = Math.Sqrt (dx1 * dx1 + dy1 * dy1);
        var r2 = Math.Sqrt (dx2 * dx2 + dy2 * dy2);

        const string format = Config.SignificantFigures3;
        Elements[Keys.Coords] =
            new PdfLiteral (
                "[{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "} {4:" + format + "} {5:" +
                format + "}]", p1.X, p1.Y, r1, p2.X, p2.Y, r2);

        //Elements[Keys.Background] = new PdfRawItem("[0 1 1]");
        //Elements[Keys.Domain] =
        Elements[Keys.Function] = function;

        //Elements[Keys.Extend] = new PdfRawItem("[true true]");

        var clr1 = "[" + PdfEncoders.ToString (color1, colorMode, true) + "]";
        var clr2 = "[" + PdfEncoders.ToString (color2, colorMode, true) + "]";

        function.Elements["/FunctionType"] = new PdfInteger (2);
        function.Elements["/C0"] = new PdfLiteral (clr1);
        function.Elements["/C1"] = new PdfLiteral (clr2);
        function.Elements["/Domain"] = new PdfLiteral ("[0 1]");
        function.Elements["/N"] = new PdfInteger (1);
    }

    /// <summary>
    /// Setups the shading from the specified brush.
    /// </summary>
    internal void SetupFromBrush
        (
            XLinearGradientBrush brush,
            XGraphicsPdfRenderer renderer
        )
    {
        Sure.NotNull (brush);

        var colorMode = _document.Options.ColorMode;
        var color1 = ColorSpaceHelper.EnsureColorMode (colorMode, brush._color1);
        var color2 = ColorSpaceHelper.EnsureColorMode (colorMode, brush._color2);

        var function = new PdfDictionary();

        Elements[Keys.ShadingType] = new PdfInteger (2);
        if (colorMode != PdfColorMode.Cmyk)
        {
            Elements[Keys.ColorSpace] = new PdfName ("/DeviceRGB");
        }
        else
        {
            Elements[Keys.ColorSpace] = new PdfName ("/DeviceCMYK");
        }

        double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
        if (brush._useRect)
        {
            var pt1 = renderer.WorldToView (brush._rect.TopLeft);
            var pt2 = renderer.WorldToView (brush._rect.BottomRight);

            switch (brush._linearGradientMode)
            {
                case XLinearGradientMode.Horizontal:
                    x1 = pt1.X;
                    y1 = pt1.Y;
                    x2 = pt2.X;
                    y2 = pt1.Y;
                    break;

                case XLinearGradientMode.Vertical:
                    x1 = pt1.X;
                    y1 = pt1.Y;
                    x2 = pt1.X;
                    y2 = pt2.Y;
                    break;

                case XLinearGradientMode.ForwardDiagonal:
                    x1 = pt1.X;
                    y1 = pt1.Y;
                    x2 = pt2.X;
                    y2 = pt2.Y;
                    break;

                case XLinearGradientMode.BackwardDiagonal:
                    x1 = pt2.X;
                    y1 = pt1.Y;
                    x2 = pt1.X;
                    y2 = pt2.Y;
                    break;
            }
        }
        else
        {
            var pt1 = renderer.WorldToView (brush._point1);
            var pt2 = renderer.WorldToView (brush._point2);

            x1 = pt1.X;
            y1 = pt1.Y;
            x2 = pt2.X;
            y2 = pt2.Y;
        }

        const string format = Config.SignificantFigures3;
        Elements[Keys.Coords] =
            new PdfLiteral ("[{0:" + format + "} {1:" + format + "} {2:" + format + "} {3:" + format + "}]", x1, y1, x2,
                y2);

        //Elements[Keys.Background] = new PdfRawItem("[0 1 1]");
        //Elements[Keys.Domain] =
        Elements[Keys.Function] = function;

        //Elements[Keys.Extend] = new PdfRawItem("[true true]");

        var clr1 = "[" + PdfEncoders.ToString (color1, colorMode, true) + "]";
        var clr2 = "[" + PdfEncoders.ToString (color2, colorMode, true) + "]";

        function.Elements["/FunctionType"] = new PdfInteger (2);
        function.Elements["/C0"] = new PdfLiteral (clr1);
        function.Elements["/C1"] = new PdfLiteral (clr2);
        function.Elements["/Domain"] = new PdfLiteral ("[0 1]");
        function.Elements["/N"] = new PdfInteger (1);
    }

    /// <summary>
    /// Common keys for all streams.
    /// </summary>
    internal sealed class Keys
        : KeysBase
    {
        /// <summary>
        /// (Required) The shading type:
        /// 1 Function-based shading
        /// 2 Axial shading
        /// 3 Radial shading
        /// 4 Free-form Gouraud-shaded triangle mesh
        /// 5 Lattice-form Gouraud-shaded triangle mesh
        /// 6 Coons patch mesh
        /// 7 Tensor-product patch mesh
        /// </summary>
        [KeyInfo (KeyType.Integer | KeyType.Required)]
        public const string ShadingType = "/ShadingType";

        /// <summary>
        /// (Required) The color space in which color values are expressed. This may be any device,
        /// CIE-based, or special color space except a Pattern space.
        /// </summary>
        [KeyInfo (KeyType.NameOrArray | KeyType.Required)]
        public const string ColorSpace = "/ColorSpace";

        /// <summary>
        /// (Optional) An array of color components appropriate to the color space, specifying
        /// a single background color value. If present, this color is used, before any painting
        /// operation involving the shading, to fill those portions of the area to be painted
        /// that lie outside the bounds of the shading object. In the opaque imaging model,
        /// the effect is as if the painting operation were performed twice: first with the
        /// background color and then with the shading.
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Optional)]
        public const string Background = "/Background";

        /// <summary>
        /// (Optional) An array of four numbers giving the left, bottom, right, and top coordinates,
        /// respectively, of the shading's bounding box. The coordinates are interpreted in the
        /// shading's target coordinate space. If present, this bounding box is applied as a temporary
        /// clipping boundary when the shading is painted, in addition to the current clipping path
        /// and any other clipping boundaries in effect at that time.
        /// </summary>
        [KeyInfo (KeyType.Rectangle | KeyType.Optional)]
        public const string BBox = "/BBox";

        /// <summary>
        /// (Optional) A flag indicating whether to filter the shading function to prevent aliasing
        /// artifacts. The shading operators sample shading functions at a rate determined by the
        /// resolution of the output device. Aliasing can occur if the function is not smooth - that
        /// is, if it has a high spatial frequency relative to the sampling rate. Anti-aliasing can
        /// be computationally expensive and is usually unnecessary, since most shading functions
        /// are smooth enough or are sampled at a high enough frequency to avoid aliasing effects.
        /// Anti-aliasing may not be implemented on some output devices, in which case this flag
        /// is ignored.
        /// Default value: false.
        /// </summary>
        [KeyInfo (KeyType.Boolean | KeyType.Optional)]
        public const string AntiAlias = "/AntiAlias";

        // ---- Type 2 ----------------------------------------------------------

        /// <summary>
        /// (Required) An array of four numbers [x0 y0 x1 y1] specifying the starting and
        /// ending coordinates of the axis, expressed in the shading's target coordinate space.
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Required)]
        public const string Coords = "/Coords";

        /// <summary>
        /// (Optional) An array of two numbers [t0 t1] specifying the limiting values of a
        /// parametric variable t. The variable is considered to vary linearly between these
        /// two values as the color gradient varies between the starting and ending points of
        /// the axis. The variable t becomes the input argument to the color function(s).
        /// Default value: [0.0 1.0].
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Optional)]
        public const string Domain = "/Domain";

        /// <summary>
        /// (Required) A 1-in, n-out function or an array of n 1-in, 1-out functions (where n
        /// is the number of color components in the shading dictionary's color space). The
        /// function(s) are called with values of the parametric variable t in the domain defined
        /// by the Domain entry. Each function's domain must be a superset of that of the shading
        /// dictionary. If the value returned by the function for a given color component is out
        /// of range, it is adjusted to the nearest valid value.
        /// </summary>
        [KeyInfo (KeyType.Function | KeyType.Required)]
        public const string Function = "/Function";

        /// <summary>
        /// (Optional) An array of two boolean values specifying whether to extend the shading
        /// beyond the starting and ending points of the axis, respectively.
        /// Default value: [false false].
        /// </summary>
        [KeyInfo (KeyType.Array | KeyType.Optional)]
        public const string Extend = "/Extend";

        /// <summary>
        /// Gets the KeysMeta for these keys.
        /// </summary>
        internal static DictionaryMeta Meta => _meta ??= CreateMeta (typeof (Keys));

        private static DictionaryMeta? _meta;
    }

    /// <summary>
    /// Gets the KeysMeta of this dictionary type.
    /// </summary>
    internal override DictionaryMeta Meta => Keys.Meta;
}
