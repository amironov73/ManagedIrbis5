// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* BackgroundImageDrawHandler.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Drawing.HtmlRenderer.Adapters;
using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Core.Dom;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Core.Handlers;

/// <summary>
/// Contains all the paint code to paint different background images.
/// </summary>
internal static class BackgroundImageDrawHandler
{
    /// <summary>
    /// Draw the background image of the given box in the given rectangle.<br/>
    /// Handle background-repeat and background-position values.
    /// </summary>
    /// <param name="graphics">the device to draw into</param>
    /// <param name="box">the box to draw its background image</param>
    /// <param name="imageLoadHandler">the handler that loads image to draw</param>
    /// <param name="rectangle">the rectangle to draw image in</param>
    public static void DrawBackgroundImage
        (
            RGraphics graphics,
            CssBox box,
            ImageLoadHandler imageLoadHandler,
            RRect rectangle
        )
    {
        // image size depends if specific rectangle given in image loader
        var imgSize = new RSize
            (
                imageLoadHandler.Rectangle == RRect.Empty
                    ? imageLoadHandler.Image!.Width
                    : imageLoadHandler.Rectangle.Width,
                imageLoadHandler.Rectangle == RRect.Empty
                    ? imageLoadHandler.Image!.Height
                    : imageLoadHandler.Rectangle.Height
            );

        // get the location by BackgroundPosition value
        var location = GetLocation (box.BackgroundPosition, rectangle, imgSize);

        var srcRect = imageLoadHandler.Rectangle == RRect.Empty
            ? new RRect (0, 0, imgSize.Width, imgSize.Height)
            : new RRect
                (
                    imageLoadHandler.Rectangle.Left,
                    imageLoadHandler.Rectangle.Top,
                    imgSize.Width,
                    imgSize.Height
                );

        // initial image destination rectangle
        var destRect = new RRect (location, imgSize);

        // need to clip so repeated image will be cut on rectangle
        var lRectangle = rectangle;
        lRectangle.Intersect (graphics.GetClip());
        graphics.PushClip (lRectangle);

        switch (box.BackgroundRepeat)
        {
            case "no-repeat":
                graphics.DrawImage (imageLoadHandler.Image!, destRect, srcRect);
                break;

            case "repeat-x":
                DrawRepeatX (graphics, imageLoadHandler, rectangle, srcRect, destRect, imgSize);
                break;

            case "repeat-y":
                DrawRepeatY (graphics, imageLoadHandler, rectangle, srcRect, destRect, imgSize);
                break;

            default:
                DrawRepeat (graphics, imageLoadHandler, rectangle, srcRect, destRect, imgSize);
                break;
        }

        graphics.PopClip();
    }


    #region Private methods

    /// <summary>
    /// Get top-left location to start drawing the image at depending on background-position value.
    /// </summary>
    /// <param name="backgroundPosition">the background-position value</param>
    /// <param name="rectangle">the rectangle to position image in</param>
    /// <param name="imgSize">the size of the image</param>
    /// <returns>the top-left location</returns>
    private static RPoint GetLocation
        (
            string backgroundPosition,
            RRect rectangle,
            RSize imgSize
        )
    {
        var left = rectangle.Left;
        if (backgroundPosition.IndexOf ("left", StringComparison.OrdinalIgnoreCase) > -1)
        {
            left = (rectangle.Left + .5f);
        }
        else if (backgroundPosition.IndexOf ("right", StringComparison.OrdinalIgnoreCase) > -1)
        {
            left = rectangle.Right - imgSize.Width;
        }
        else if (backgroundPosition.IndexOf ("0", StringComparison.OrdinalIgnoreCase) < 0)
        {
            left = (rectangle.Left + (rectangle.Width - imgSize.Width) / 2 + .5f);
        }

        var top = rectangle.Top;
        if (backgroundPosition.IndexOf ("top", StringComparison.OrdinalIgnoreCase) > -1)
        {
            top = rectangle.Top;
        }
        else if (backgroundPosition.IndexOf ("bottom", StringComparison.OrdinalIgnoreCase) > -1)
        {
            top = rectangle.Bottom - imgSize.Height;
        }
        else if (backgroundPosition.IndexOf ("0", StringComparison.OrdinalIgnoreCase) < 0)
        {
            top = (rectangle.Top + (rectangle.Height - imgSize.Height) / 2 + .5f);
        }

        return new RPoint (left, top);
    }

    /// <summary>
    /// Draw the background image at the required location repeating it over the X axis.<br/>
    /// Adjust location to left if starting location doesn't include all the range (adjusted to center or right).
    /// </summary>
    private static void DrawRepeatX
        (
            RGraphics graphics,
            ImageLoadHandler imageLoadHandler,
            RRect rectangle,
            RRect srcRect,
            RRect destRect, RSize imgSize
        )
    {
        while (destRect.X > rectangle.X)
        {
            destRect.X -= imgSize.Width;
        }

        using (var brush = graphics.GetTextureBrush (imageLoadHandler.Image!, srcRect, destRect.Location))
        {
            graphics.DrawRectangle (brush, rectangle.X, destRect.Y, rectangle.Width, srcRect.Height);
        }
    }

    /// <summary>
    /// Draw the background image at the required location repeating it over the Y axis.<br/>
    /// Adjust location to top if starting location doesn't include all the range (adjusted to center or bottom).
    /// </summary>
    private static void DrawRepeatY
        (
            RGraphics graphics,
            ImageLoadHandler imageLoadHandler,
            RRect rectangle,
            RRect srcRect,
            RRect destRect,
            RSize imgSize
        )
    {
        while (destRect.Y > rectangle.Y)
        {
            destRect.Y -= imgSize.Height;
        }

        using (var brush = graphics.GetTextureBrush (imageLoadHandler.Image!, srcRect, destRect.Location))
        {
            graphics.DrawRectangle (brush, destRect.X, rectangle.Y, srcRect.Width, rectangle.Height);
        }
    }

    /// <summary>
    /// Draw the background image at the required location repeating it over the X and Y axis.<br/>
    /// Adjust location to left-top if starting location doesn't include all the range (adjusted to center or bottom/right).
    /// </summary>
    private static void DrawRepeat
        (
            RGraphics graphics,
            ImageLoadHandler imageLoadHandler,
            RRect rectangle,
            RRect srcRect,
            RRect destRect,
            RSize imgSize
        )
    {
        while (destRect.X > rectangle.X)
        {
            destRect.X -= imgSize.Width;
        }

        while (destRect.Y > rectangle.Y)
        {
            destRect.Y -= imgSize.Height;
        }

        using (var brush = graphics.GetTextureBrush (imageLoadHandler.Image!, srcRect, destRect.Location))
        {
            graphics.DrawRectangle (brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }
    }

    #endregion
}
