// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

/* Ean8Test.cs --
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
public class Ean8Test
    : DrawingTests.CommonUnitTest
{
    [TestMethod]
    public void Ean8_DrawBarcode_1()
    {
        using var image = new Bitmap (500, 100);
        using var graphics = Graphics.FromImage (image);
        graphics.Clear (Color.Lime);

        var barcode = new Ean8();
        var data = new BarcodeData
        {
            Message = "46009333"
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
        image.Save ("Ean8.bmp");
    }

    [TestMethod]
    public void Ean8_Encode_1()
    {
        var barcode = new Ean8();
        var expected = "1010100011010111100011010001101010101110100100001010000101000010101";
        var data = new BarcodeData
        {
            Message = "46009333"
        };
        var actual = barcode.Encode (data);
        ShowDifference (expected, actual);
        Assert.AreEqual (expected, actual);
    }
}
