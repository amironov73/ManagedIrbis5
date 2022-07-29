// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TransformHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Imaging;

#endregion

#nullable enable

namespace AM.Windows.Forms.Animation;

/// <summary>
/// Implements image transformations
/// </summary>
public static class TransformHelper
{
    #region Constants

    private const int BytesPerPixel = 4;

    #endregion

    #region Private members

    private static readonly Random _random = new Random();

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public static void DoScale
        (
            TransfromNeededEventArg e,
            Animation animation
        )
    {
        var rect = e.ClientRectangle;
        var center = new PointF (rect.Width / 2f, rect.Height / 2f);
        e.Matrix.Translate (center.X, center.Y);
        var kx = 1f - animation.ScaleCoefficient.X * e.CurrentTime;
        var ky = 1f - animation.ScaleCoefficient.X * e.CurrentTime;
        if (Math.Abs (kx) <= 0.001f)
        {
            kx = 0.001f;
        }

        if (Math.Abs (ky) <= 0.001f)
        {
            ky = 0.001f;
        }

        e.Matrix.Scale (kx, ky);
        e.Matrix.Translate (-center.X, -center.Y);
    }

    /// <summary>
    ///
    /// </summary>
    public static void DoSlide
        (
            TransfromNeededEventArg e,
            Animation animation
        )
    {
        var k = e.CurrentTime;
        e.Matrix.Translate
            (
                -e.ClientRectangle.Width * k * animation.SlideCoefficient.X,
                -e.ClientRectangle.Height * k * animation.SlideCoefficient.Y
            );
    }

    /// <summary>
    ///
    /// </summary>
    public static void DoBlind
        (
            NonLinearTransfromNeededEventArg e,
            Animation animation
        )
    {
        if (animation.BlindCoefficient == PointF.Empty)
        {
            return;
        }

        var pixels = e.Pixels;
        var sx = e.ClientRectangle.Width;
        var sy = e.ClientRectangle.Height;
        var s = e.Stride;
        var kx = animation.BlindCoefficient.X;
        var ky = animation.BlindCoefficient.Y;
        var a = (int)((sx * kx + sy * ky) * (1 - e.CurrentTime));

        for (var x = 0; x < sx; x++)
        {
            for (var y = 0; y < sy; y++)
            {
                var i = y * s + x * BytesPerPixel;
                var d = x * kx + y * ky - a;
                if (d >= 0)
                {
                    pixels[i + 3] = 0;
                }
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static void DoMosaic
        (
            NonLinearTransfromNeededEventArg e,
            Animation animation,
            ref Point[] buffer,
            ref byte[] pixelsBuffer
        )
    {
        if (animation.MosaicCoefficient == PointF.Empty || animation.MosaicSize == 0)
        {
            return;
        }

        var pixels = e.Pixels;
        var sx = e.ClientRectangle.Width;
        var sy = e.ClientRectangle.Height;
        var s = e.Stride;
        var a = e.CurrentTime;
        var count = pixels.Length;
        var opacity = 1 - e.CurrentTime;
        if (opacity < 0f)
        {
            opacity = 0f;
        }

        if (opacity > 1f)
        {
            opacity = 1f;
        }

        var mkx = animation.MosaicCoefficient.X;
        var mky = animation.MosaicCoefficient.Y;

        if (buffer == null)
        {
            buffer = new Point[pixels.Length];
            for (var i = 0; i < pixels.Length; i++)
                buffer[i] = new Point ((int)(mkx * (_random.NextDouble() - 0.5)),
                    (int)(mky * (_random.NextDouble() - 0.5)));
        }

        if (pixelsBuffer == null)
        {
            pixelsBuffer = (byte[])pixels.Clone();
        }

        for (var i = 0; i < count; i += BytesPerPixel)
        {
            pixels[i + 0] = 255;
            pixels[i + 1] = 255;
            pixels[i + 2] = 255;
            pixels[i + 3] = 0;
        }

        var ms = animation.MosaicSize;
        var msx = animation.MosaicShift.X;
        var msy = animation.MosaicShift.Y;

        for (var y = 0; y < sy; y++)
        for (var x = 0; x < sx; x++)
        {
            var yi = (y / ms);
            var xi = (x / ms);
            var i = y * s + x * BytesPerPixel;
            var j = yi * s + xi * BytesPerPixel;

            var newX = x + (int)(a * (buffer[j].X + xi * msx));
            var newY = y + (int)(a * (buffer[j].Y + yi * msy));

            if (newX >= 0 && newX < sx)
            {
                if (newY >= 0 && newY < sy)
                {
                    var newI = newY * s + newX * BytesPerPixel;
                    pixels[newI + 0] = pixelsBuffer[i + 0];
                    pixels[newI + 1] = pixelsBuffer[i + 1];
                    pixels[newI + 2] = pixelsBuffer[i + 2];
                    pixels[newI + 3] = (byte)(pixelsBuffer[i + 3] * opacity);
                }
            }
        }
    }


    /// <summary>
    ///
    /// </summary>
    public static void DoLeaf
        (
            NonLinearTransfromNeededEventArg e,
            Animation animation
        )
    {
        if (animation.LeafCoefficient == 0f)
        {
            return;
        }

        var pixels = e.Pixels;
        var sx = e.ClientRectangle.Width;
        var sy = e.ClientRectangle.Height;
        var s = e.Stride;
        var a = (int)((sx + sy) * (1 - e.CurrentTime * e.CurrentTime));

        var count = pixels.Length;

        for (var x = 0; x < sx; x++)
        for (var y = 0; y < sy; y++)
        {
            var i = y * s + x * BytesPerPixel;
            if (x + y >= a)
            {
                var newX = a - y;
                var newY = a - x;
                var d = a - x - y;
                if (d < -20)
                {
                    d = -20;
                }

                var newI = newY * s + newX * BytesPerPixel;
                if (newX >= 0 && newY >= 0)
                {
                    if (newI >= 0 && newI < count)
                    {
                        if (pixels[i + 3] > 0)
                        {
                            pixels[newI + 0] = (byte)Math.Min (255, d + 250 + pixels[i + 0] / 10);
                            pixels[newI + 1] = (byte)Math.Min (255, d + 250 + pixels[i + 1] / 10);
                            pixels[newI + 2] = (byte)Math.Min (255, d + 250 + pixels[i + 2] / 10);
                            pixels[newI + 3] = 230;
                        }
                    }
                }

                pixels[i + 3] = (byte)(0);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static void DoTransparent
        (
            NonLinearTransfromNeededEventArg e,
            Animation animation
        )
    {
        if (animation.TransparencyCoefficient == 0f)
        {
            return;
        }

        var opacity = 1f - animation.TransparencyCoefficient * e.CurrentTime;
        if (opacity < 0f)
        {
            opacity = 0f;
        }

        if (opacity > 1f)
        {
            opacity = 1f;
        }

        var pixels = e.Pixels;
        for (var counter = 0; counter < pixels.Length; counter += BytesPerPixel)
        {
            pixels[counter + 3] = (byte)(pixels[counter + 3] * opacity);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static void CalcDifference
        (
            Bitmap bmp1,
            Bitmap bmp2
        )
    {
        var pxf = PixelFormat.Format32bppArgb;
        var rect = new Rectangle (0, 0, bmp1.Width, bmp1.Height);

        var bmpData1 = bmp1.LockBits (rect, ImageLockMode.ReadWrite, pxf);
        var ptr1 = bmpData1.Scan0;

        var bmpData2 = bmp2.LockBits (rect, ImageLockMode.ReadOnly, pxf);
        var ptr2 = bmpData2.Scan0;

        var numBytes = bmp1.Width * bmp1.Height * BytesPerPixel;
        var pixels1 = new byte[numBytes];
        var pixels2 = new byte[numBytes];

        System.Runtime.InteropServices.Marshal.Copy (ptr1, pixels1, 0, numBytes);
        System.Runtime.InteropServices.Marshal.Copy (ptr2, pixels2, 0, numBytes);

        for (var i = 0; i < numBytes; i += BytesPerPixel)
        {
            if (pixels1[i + 0] == pixels2[i + 0] &&
                pixels1[i + 1] == pixels2[i + 1] &&
                pixels1[i + 2] == pixels2[i + 2])
            {
                pixels1[i + 0] = 255;
                pixels1[i + 1] = 255;
                pixels1[i + 2] = 255;
                pixels1[i + 3] = 0;
            }
        }

        System.Runtime.InteropServices.Marshal.Copy (pixels1, 0, ptr1, numBytes);
        bmp1.UnlockBits (bmpData1);
        bmp2.UnlockBits (bmpData2);
    }

    /// <summary>
    ///
    /// </summary>
    public static void DoRotate
        (
            TransfromNeededEventArg e,
            Animation animation
        )
    {
        var rect = e.ClientRectangle;
        var center = new PointF (rect.Width / 2, rect.Height / 2);

        e.Matrix.Translate (center.X, center.Y);
        if (e.CurrentTime > animation.RotateLimit)
        {
            e.Matrix.Rotate (360 * (e.CurrentTime - animation.RotateLimit) * animation.RotateCoefficient);
        }

        e.Matrix.Translate (-center.X, -center.Y);
    }

    /// <summary>
    ///
    /// </summary>
    public static void DoBottomMirror
        (
            NonLinearTransfromNeededEventArg e
        )
    {
        var source = e.SourcePixels;
        var output = e.Pixels;

        var s = e.Stride;
        var dy = 1;
        var beginY = e.SourceClientRectangle.Bottom + dy;
        var sy = e.ClientRectangle.Height;
        var beginX = e.SourceClientRectangle.Left;
        var endX = e.SourceClientRectangle.Right;
        var d = sy - beginY;

        for (var x = beginX; x < endX; x++)
        for (var y = beginY; y < sy; y++)
        {
            var sourceY = (int)(beginY - 1 - dy - (y - beginY));
            if (sourceY < 0)
            {
                break;
            }

            var sourceX = x;
            var sourceI = sourceY * s + sourceX * BytesPerPixel;
            var outI = y * s + x * BytesPerPixel;
            output[outI + 0] = source[sourceI + 0];
            output[outI + 1] = source[sourceI + 1];
            output[outI + 2] = source[sourceI + 2];
            output[outI + 3] = (byte)((1 - 1f * (y - beginY) / d) * 90);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static void DoBlur
        (
            NonLinearTransfromNeededEventArg e,
            int radius
        )
    {
        var output = e.Pixels;
        var source = e.SourcePixels;

        var s = e.Stride;
        var sy = e.ClientRectangle.Height;
        var sx = e.ClientRectangle.Width;
        var maxI = source.Length - BytesPerPixel;

        for (var x = radius; x < sx - radius; x++)
        for (var y = radius; y < sy - radius; y++)
        {
            var outI = y * s + x * BytesPerPixel;

            var r = 0;
            var g = 0;
            var b = 0;
            var a = 0;
            var counter = 0;
            for (var xx = x - radius; xx < x + radius; xx++)
            {
                for (var yy = y - radius; yy < y + radius; yy++)
                {
                    var srcI = yy * s + xx * BytesPerPixel;
                    if (srcI >= 0 && srcI < maxI && source[srcI + 3] > 0)
                    {
                        b += source[srcI + 0];
                        g += source[srcI + 1];
                        r += source[srcI + 2];
                        a += source[srcI + 3];
                        counter++;
                    }
                }
            }

            if (outI < maxI && counter > 5)
            {
                output[outI + 0] = (byte)(b / counter);
                output[outI + 1] = (byte)(g / counter);
                output[outI + 2] = (byte)(r / counter);
                output[outI + 3] = (byte)(a / counter);
            }
        }
    }

    #endregion
}
