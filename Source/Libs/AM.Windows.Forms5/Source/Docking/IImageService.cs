// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* IImageService.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Imaging;

#endregion

#nullable enable

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public interface IImageService
{
    /// <summary>
    ///
    /// </summary>
    Bitmap Dockindicator_PaneDiamond { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap Dockindicator_PaneDiamond_Fill { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap Dockindicator_PaneDiamond_Hotspot { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockIndicator_PaneDiamond_HotspotIndex { get; }

    /// <summary>
    ///
    /// </summary>
    Image DockIndicator_PanelBottom { get; }

    /// <summary>
    ///
    /// </summary>
    Image DockIndicator_PanelFill { get; }

    /// <summary>
    ///
    /// </summary>
    Image DockIndicator_PanelLeft { get; }

    /// <summary>
    ///
    /// </summary>
    Image DockIndicator_PanelRight { get; }

    /// <summary>
    ///
    /// </summary>
    Image DockIndicator_PanelTop { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPane_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPane_List { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPane_Dock { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneActive_AutoHide { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPane_Option { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPane_OptionOverflow { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneActive_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneActive_Dock { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneActive_Option { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneHover_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneHover_List { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneHover_Dock { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneActiveHover_AutoHide { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneHover_Option { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneHover_OptionOverflow { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPanePress_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPanePress_List { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPanePress_Dock { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPanePress_AutoHide { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPanePress_Option { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPanePress_OptionOverflow { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneActiveHover_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneActiveHover_Dock { get; }

    /// <summary>
    ///
    /// </summary>
    Bitmap DockPaneActiveHover_Option { get; }

    /// <summary>
    ///
    /// </summary>
    Image TabActive_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Image TabInactive_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Image TabLostFocus_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Image TabHoverActive_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Image TabHoverInactive_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Image TabHoverLostFocus_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Image TabPressActive_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Image TabPressInactive_Close { get; }

    /// <summary>
    ///
    /// </summary>
    Image TabPressLostFocus_Close { get; }
}

/// <summary>
///
/// </summary>
public static class ImageServiceHelper
{
    /// <summary>
    /// Gets images for tabs and captions.
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="glyph"></param>
    /// <param name="background"></param>
    /// <param name="border"></param>
    /// <returns></returns>
    public static Bitmap GetImage
        (
            Bitmap mask,
            Color glyph,
            Color background,
            Color? border = null
        )
    {
        var width = mask.Width;
        var height = mask.Height;
        var input = new Bitmap (width, height);
        using (var graphics = Graphics.FromImage (input))
        {
            var brush = new SolidBrush (glyph);
            graphics.FillRectangle (brush, 0, 0, width, height);
        }

        var output = new Bitmap (input.Width, input.Height, PixelFormat.Format32bppArgb);
        var rect = new Rectangle (0, 0, input.Width, input.Height);
        var bitsMask = mask.LockBits (rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var bitsInput = input.LockBits (rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var bitsOutput = output.LockBits (rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        unsafe
        {
            for (var y = 0; y < input.Height; y++)
            {
                var ptrMask = (byte*)bitsMask.Scan0 + y * bitsMask.Stride;
                var ptrInput = (byte*)bitsInput.Scan0 + y * bitsInput.Stride;
                var ptrOutput = (byte*)bitsOutput.Scan0 + y * bitsOutput.Stride;
                for (var x = 0; x < input.Width; x++)
                {
                    ptrOutput[4 * x] = ptrInput[4 * x]; // blue
                    ptrOutput[4 * x + 1] = ptrInput[4 * x + 1]; // green
                    ptrOutput[4 * x + 2] = ptrInput[4 * x + 2]; // red
                    ptrOutput[4 * x + 3] = ptrMask[4 * x]; // alpha
                }
            }
        }

        mask.UnlockBits (bitsMask);
        input.UnlockBits (bitsInput);
        output.UnlockBits (bitsOutput);
        input.Dispose();

        border ??= background;
        var back = new Bitmap (width, height);
        using (var graphics = Graphics.FromImage (back))
        {
            var brush = new SolidBrush (background);
            var brush2 = new SolidBrush (border.Value);
            graphics.FillRectangle (brush2, 0, 0, width, height);
            if (background != border.Value)
            {
                graphics.FillRectangle (brush, 1, 1, width - 2, height - 2);
            }

            graphics.DrawImageUnscaled (output, 0, 0);
        }

        output.Dispose();
        return back;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="innerBorder"></param>
    /// <param name="outerBorder"></param>
    /// <param name="width"></param>
    /// <param name="painting"></param>
    /// <returns></returns>
    public static Bitmap GetBackground
        (
            Color innerBorder,
            Color outerBorder,
            int width,
            IPaintingService painting
        )
    {
        var back = new Bitmap (width, width);
        using (var graphics = Graphics.FromImage (back))
        {
            var brush = painting.GetBrush (innerBorder);
            var brush2 = painting.GetBrush (outerBorder);
            graphics.FillRectangle (brush2, 0, 0, width, width);
            graphics.FillRectangle (brush, 1, 1, width - 2, width - 2);
        }

        return back;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="color"></param>
    /// <param name="width"></param>
    /// <param name="painting"></param>
    /// <returns></returns>
    public static Bitmap GetLayerImage
        (
            Color color,
            int width,
            IPaintingService painting
        )
    {
        var back = new Bitmap (width, width);
        using (var graphics = Graphics.FromImage (back))
        {
            var brush = painting.GetBrush (color);
            graphics.FillRectangle (brush, 0, 0, width, width);
        }

        return back;
    }

    /// <summary>
    /// Gets images for docking indicators.
    /// </summary>
    /// <returns></returns>
    public static Bitmap GetDockIcon
        (
            Bitmap? maskArrow,
            Bitmap layerArrow,
            Bitmap maskWindow,
            Bitmap layerWindow,
            Bitmap maskBack,
            Color background,
            IPaintingService painting,
            Bitmap? maskCore = null,
            Bitmap? layerCore = null,
            Color? separator = null
        )
    {
        var width = maskBack.Width;
        var height = maskBack.Height;
        // var rect = new Rectangle (0, 0, width, height);
        Bitmap? arrowOut = null;

        if (maskArrow != null)
        {
            var input = layerArrow;
            arrowOut = MaskImages (input, maskArrow);
        }

        var windowIn = layerWindow;
        var windowOut = MaskImages (windowIn, maskWindow);

        Bitmap? coreOut = null;
        if (layerCore != null)
        {
            var coreIn = layerCore;
            coreOut = MaskImages (coreIn, maskCore);
        }

        var backIn = new Bitmap (width, height);
        using (var graphics = Graphics.FromImage (backIn))
        {
            var brush = painting.GetBrush (background);
            graphics.FillRectangle (brush, 0, 0, width, height);
            graphics.DrawImageUnscaled (windowOut, 0, 0);
            windowOut.Dispose();
            if (layerCore != null)
            {
                graphics.DrawImageUnscaled (coreOut!, 0, 0);
                coreOut!.Dispose();
            }

            if (separator != null)
            {
                var sep = painting.GetPen (separator.Value);
                graphics.DrawRectangle (sep, 0, 0, width - 1, height - 1);
            }
        }

        var backOut = MaskImages (backIn, maskBack);
        backIn.Dispose();

        using (var gfx = Graphics.FromImage (backOut))
        {
            if (arrowOut != null)
            {
                gfx.DrawImageUnscaled (arrowOut, 0, 0);
                arrowOut.Dispose();
            }
        }

        return backOut;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="input"></param>
    /// <param name="maskArrow"></param>
    /// <returns></returns>
    public static Bitmap MaskImages
        (
            Bitmap input,
            Bitmap? maskArrow
        )
    {
        var width = input.Width;
        var height = input.Height;
        var rect = new Rectangle (0, 0, width, height);
        var arrowOut = new Bitmap (width, height, PixelFormat.Format32bppArgb);
        var bitsMask = maskArrow!.LockBits (rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var bitsInput = input.LockBits (rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        var bitsOutput = arrowOut.LockBits (rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        unsafe
        {
            for (var y = 0; y < height; y++)
            {
                var ptrMask = (byte*)bitsMask.Scan0 + y * bitsMask.Stride;
                var ptrInput = (byte*)bitsInput.Scan0 + y * bitsInput.Stride;
                var ptrOutput = (byte*)bitsOutput.Scan0 + y * bitsOutput.Stride;
                for (var x = 0; x < width; x++)
                {
                    ptrOutput[4 * x] = ptrInput[4 * x]; // blue
                    ptrOutput[4 * x + 1] = ptrInput[4 * x + 1]; // green
                    ptrOutput[4 * x + 2] = ptrInput[4 * x + 2]; // red
                    ptrOutput[4 * x + 3] = ptrMask[4 * x]; // alpha
                }
            }
        }

        maskArrow.UnlockBits (bitsMask);
        input.UnlockBits (bitsInput);
        arrowOut.UnlockBits (bitsOutput);
        return arrowOut;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="icon"></param>
    /// <param name="background"></param>
    /// <returns></returns>
    public static Bitmap GetDockImage
        (
            Bitmap icon,
            Bitmap background
        )
    {
        var result = new Bitmap (background);
        var offset = (background.Width - icon.Width) / 2;
        using (var graphics = Graphics.FromImage (result))
        {
            graphics.DrawImage (icon, offset, offset);
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="five"></param>
    /// <param name="bottom"></param>
    /// <param name="center"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="top"></param>
    /// <returns></returns>
    public static Bitmap CombineFive
        (
            Bitmap five,
            Bitmap bottom,
            Bitmap center,
            Bitmap left,
            Bitmap right,
            Bitmap top
        )
    {
        var result = new Bitmap (five);
        var cell = (result.Width - bottom.Width) / 2;
        var offset = (cell - bottom.Width) / 2;
        using (var graphics = Graphics.FromImage (result))
        {
            graphics.DrawImageUnscaled (top, cell, offset);
            graphics.DrawImageUnscaled (center, cell, cell);
            graphics.DrawImageUnscaled (bottom, cell, 2 * cell - offset);
            graphics.DrawImageUnscaled (left, offset, cell);
            graphics.DrawImageUnscaled (right, 2 * cell - offset, cell);
        }

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="innerBorder"></param>
    /// <param name="outerBorder"></param>
    /// <param name="painting"></param>
    /// <returns></returns>
    public static Bitmap GetFiveBackground
        (
            Bitmap mask,
            Color innerBorder,
            Color outerBorder,
            IPaintingService painting
        )
    {
        // TODO: calculate points using functions.
        using (var input = GetLayerImage (innerBorder, mask.Width, painting))
        {
            using (var graphics = Graphics.FromImage (input))
            {
                var pen = painting.GetPen (outerBorder);
                graphics.DrawLines (pen, new[]
                {
                    new Point (36, 25), new Point (36, 0),
                    new Point (75, 0), new Point (75, 25)
                });
                graphics.DrawLines (pen, new[]
                {
                    new Point (86, 36), new Point (111, 36),
                    new Point (111, 75), new Point (86, 75)
                });
                graphics.DrawLines (pen, new[]
                {
                    new Point (75, 86), new Point (75, 111),
                    new Point (36, 111), new Point (36, 86)
                });
                graphics.DrawLines (pen, new[]
                {
                    new Point (25, 75), new Point (0, 75),
                    new Point (0, 36), new Point (25, 36)
                });
                var pen2 = painting.GetPen (outerBorder, 2);
                graphics.DrawLine (pen2, new Point (36, 25), new Point (25, 36));
                graphics.DrawLine (pen2, new Point (75, 25), new Point (86, 36));
                graphics.DrawLine (pen2, new Point (86, 75), new Point (75, 86));
                graphics.DrawLine (pen2, new Point (36, 86), new Point (25, 75));
            }

            return MaskImages (input, mask);
        }
    }
}
