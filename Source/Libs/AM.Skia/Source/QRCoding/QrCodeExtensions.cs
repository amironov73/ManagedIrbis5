// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* QrCodeExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using SkiaSharp;

#endregion

#nullable enable

namespace AM.Skia.QrCoding;

/// <summary>
///
/// </summary>
public static class QrCodeExtensions
{
    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="data"></param>
    /// <param name="width"></param>
    /// <param name="hight"></param>
    public static void Render
        (
            this SKCanvas canvas,
            QRCodeData data,
            int width,
            int hight
        )
    {
        canvas.Clear (SKColors.Transparent);

        using var renderer = new QRCodeRenderer();
        var area = SKRect.Create (0, 0, width, hight);
        renderer.Render (canvas, area, data, null, null);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="data"></param>
    /// <param name="width"></param>
    /// <param name="hight"></param>
    /// <param name="clearColor"></param>
    /// <param name="codeColor"></param>
    public static void Render
        (
            this SKCanvas canvas,
            QRCodeData data,
            int width,
            int hight,
            SKColor clearColor,
            SKColor codeColor
        )
    {
        canvas.Clear (clearColor);

        using var renderer = new QRCodeRenderer();
        var area = SKRect.Create (0, 0, width, hight);
        renderer.Render (canvas, area, data, codeColor, null);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="data"></param>
    /// <param name="width"></param>
    /// <param name="hight"></param>
    /// <param name="clearColor"></param>
    /// <param name="codeColor"></param>
    /// <param name="backgroundColor"></param>
    public static void Render
        (
            this SKCanvas canvas,
            QRCodeData data,
            int width,
            int hight,
            SKColor clearColor,
            SKColor codeColor,
            SKColor backgroundColor
        )
    {
        canvas.Clear (clearColor);

        using var renderer = new QRCodeRenderer();
        var area = SKRect.Create (0, 0, width, hight);
        renderer.Render (canvas, area, data, codeColor, backgroundColor);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="data"></param>
    /// <param name="width"></param>
    /// <param name="hight"></param>
    /// <param name="clearColor"></param>
    /// <param name="codeColor"></param>
    /// <param name="iconData"></param>
    public static void Render
        (
            this SKCanvas canvas,
            QRCodeData data,
            int width,
            int hight,
            SKColor clearColor,
            SKColor codeColor,
            IconData iconData
        )
    {
        canvas.Clear (clearColor);

        using var renderer = new QRCodeRenderer();
        var area = SKRect.Create (0, 0, width, hight);
        renderer.Render (canvas, area, data, codeColor, null, iconData);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="data"></param>
    /// <param name="width"></param>
    /// <param name="hight"></param>
    /// <param name="clearColor"></param>
    /// <param name="codeColor"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="iconData"></param>
    public static void Render
        (
            this SKCanvas canvas,
            QRCodeData data,
            int width,
            int hight,
            SKColor clearColor,
            SKColor codeColor,
            SKColor backgroundColor,
            IconData iconData
        )
    {
        canvas.Clear (clearColor);

        using var renderer = new QRCodeRenderer();
        var area = SKRect.Create (0, 0, width, hight);
        renderer.Render (canvas, area, data, codeColor, backgroundColor, iconData);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="data"></param>
    /// <param name="area"></param>
    /// <param name="clearColor"></param>
    /// <param name="codeColor"></param>
    public static void Render
        (
            this SKCanvas canvas,
            QRCodeData data,
            SKRect area,
            SKColor clearColor,
            SKColor codeColor
        )
    {
        canvas.Clear (clearColor);

        using var renderer = new QRCodeRenderer();
        renderer.Render (canvas, area, data, codeColor, null);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="data"></param>
    /// <param name="area"></param>
    /// <param name="clearColor"></param>
    /// <param name="codeColor"></param>
    /// <param name="backgroundColor"></param>
    public static void Render
        (
            this SKCanvas canvas,
            QRCodeData data,
            SKRect area,
            SKColor clearColor,
            SKColor codeColor,
            SKColor backgroundColor
        )
    {
        canvas.Clear (clearColor);

        using var renderer = new QRCodeRenderer();
        renderer.Render (canvas, area, data, codeColor, backgroundColor);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="data"></param>
    /// <param name="area"></param>
    /// <param name="clearColor"></param>
    /// <param name="codeColor"></param>
    /// <param name="iconData"></param>
    public static void Render
        (
            this SKCanvas canvas,
            QRCodeData data,
            SKRect area,
            SKColor clearColor,
            SKColor codeColor,
            IconData iconData
        )
    {
        canvas.Clear (clearColor);

        using var renderer = new QRCodeRenderer();
        renderer.Render (canvas, area, data, codeColor, null, iconData);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="data"></param>
    /// <param name="area"></param>
    /// <param name="clearColor"></param>
    /// <param name="codeColor"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="iconData"></param>
    public static void Render
        (
            this SKCanvas canvas,
            QRCodeData data,
            SKRect area,
            SKColor clearColor,
            SKColor codeColor,
            SKColor backgroundColor,
            IconData iconData
        )
    {
        canvas.Clear (clearColor);

        using var renderer = new QRCodeRenderer();
        renderer.Render (canvas, area, data, codeColor, backgroundColor, iconData);
    }

    #endregion
}
