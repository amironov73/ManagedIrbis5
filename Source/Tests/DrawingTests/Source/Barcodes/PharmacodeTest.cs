// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

/* PharmacodeTest.cs --
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
public class PharmacodeTest
    : DrawingTests.CommonUnitTest
{
    [TestMethod]
    public void Pharmacode_DrawBarcode_1()
    {
        using var image = new Bitmap (500, 100);
        using var graphics = Graphics.FromImage (image);
        graphics.Clear (Color.Lime);

        var barcode = new Pharmacode();
        var data = new BarcodeData
        {
            Message = "123456"
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
        image.Save ("Pharmacode.bmp");
    }

    [TestMethod]
    public void Pharmacode_Encode_1()
    {
        var barcode = new Pharmacode();
        var expected = "1110011100111001001001001110010010011100100100100100100111";
        var data = new BarcodeData
        {
            Message = "123456"
        };
        var actual = barcode.Encode (data);
        ShowDifference (expected, actual);
        Assert.AreEqual (expected, actual);
    }
}
