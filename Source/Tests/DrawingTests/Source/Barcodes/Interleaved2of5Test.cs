// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

/* Interleaved2of5Test.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Drawing.Barcodes;

#endregion

#nullable enable

namespace UnitTests.AM.Drawing.Barcodes;

[TestClass]
public class Interleaved2of5Test
{
    [TestMethod]
    public void Interleaved2of5_DrawBarcode_1()
    {
        using var image = new Bitmap (300, 100);
        using var graphics = Graphics.FromImage (image);
        graphics.Clear (Color.Lime);

        var barcode = new Interleaved2of5();
        var data = new BarcodeData
        {
            Message = "02128506"
        };
        var bounds = new RectangleF (0, 0, image.Width, image.Height);
        bounds.Inflate (-5, -5);
        var context = new BarcodeContext
        {
            Graphics = graphics,
            Data = data,
            Bounds = bounds
        };
        barcode.DrawBarcode (context);

        graphics.Dispose();
        image.Save ("Interleaved2of5.bmp");
    }
}
