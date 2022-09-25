// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

/* UpcATest.cs --
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
public class UpcATest
    : DrawingTests.CommonUnitTest
{
    [TestMethod]
    public void UpcA_DrawBarcode_1()
    {
        using var image = new Bitmap (500, 100);
        using var graphics = Graphics.FromImage (image);
        graphics.Clear (Color.Lime);

        var barcode = new UpcA();
        var data = new BarcodeData
        {
            Message = "03600029145"
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
        image.Save ("UpcA.bmp");
    }

    [TestMethod]
    public void UpcA_Encode_1()
    {
        var barcode = new UpcA();
        var expected =
            "10100011010111101010111100011010001101000110101010110110011101001100110101110010011101101100101";
        var data = new BarcodeData
        {
            Message = "03600029145"
        };
        var actual = barcode.Encode (data);
        ShowDifference (expected, actual);
        Assert.AreEqual (expected, actual);
    }
}
