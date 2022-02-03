// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using System.Drawing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Drawing.Barcodes;

#nullable enable

namespace UnitTests.AM.Drawing.Barcodes;

[TestClass]
public class CodabarTest
    : DrawingTests.CommonUnitTest
{
    [TestMethod]
    public void Codabar_DrawBarcode_1()
    {
        using var image = new Bitmap (600, 100);
        using var graphics = Graphics.FromImage (image);
        graphics.Clear (Color.Lime);

        var barcode = new Codabar();
        var data = new BarcodeData
        {
            Message = "A31117013206375A"
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
        image.Save ("Codabar.bmp");
    }

    [TestMethod]
    public void Codabar_Encode_1()
    {
        var barcode = new Codabar();
        var expected =
            "10110010010110010101010101100101010110010101011001010010110101010100110101011001011001010101010010110101010011010010101101100101010100101101011010100101011001001";
        var data = new BarcodeData
        {
            Message = "A31117013206375A"
        };
        var actual = barcode.Encode (data);
        ShowDifference (expected, actual);
        Assert.AreEqual (expected, actual);
    }
}
