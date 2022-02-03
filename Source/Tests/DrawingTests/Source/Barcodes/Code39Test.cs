// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

using System.Drawing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Drawing.Barcodes;

#nullable enable

namespace UnitTests.AM.Drawing.Barcodes;

[TestClass]
public class Code39Test
    : DrawingTests.CommonUnitTest
{
    [TestMethod]
    public void Code39_DrawBarcode_1()
    {
        using var image = new Bitmap (500, 100);
        using var graphics = Graphics.FromImage (image);
        graphics.Clear (Color.Lime);

        var barcode = new Code39();
        var data = new BarcodeData
        {
            Message = "2128506"
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
        image.Save ("Code39.bmp");
    }

    [TestMethod]
    public void Code39_Encode_1()
    {
        var expected =
            "10010110110101011001010110110100101011010110010101101101001011010110100110101010100110110101011001101010100101101101";
        var data = new BarcodeData() { Message = "2128506" };
        var actual = new Code39().Encode (data);
        ShowDifference (expected, actual);
        Assert.AreEqual (expected, actual);
    }
}
