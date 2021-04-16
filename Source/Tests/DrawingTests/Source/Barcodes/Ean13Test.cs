// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

using System.Drawing;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Drawing.Barcodes;

#nullable enable

namespace UnitTests.AM.Drawing.Barcodes
{
    [TestClass]
    public class Ean13Test
        : DrawingTests.CommonUnitTest
    {
        [TestMethod]
        public void Ean13_DrawBarcode_1()
        {
            using var image = new Bitmap(500, 100);
            using var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Lime);

            var barcode = new Ean13();
            var data = new BarcodeData
            {
                Message = "2128506"
            };
            var bounds = new RectangleF(0, 0, image.Width, image.Height);
            bounds.Inflate(-5, - 5);
            var context = new BarcodeContext
            {
                Graphics = graphics,
                Data = data,
                Bounds = bounds
            };
            barcode.DrawBarcode(context);

            graphics.Dispose();
            image.Save("Ean13.bmp");
        }

        [TestMethod]
        public void Ean13_Encode_1()
        {
            var barcode = new Ean13();
            var expected = "";
            var actual = barcode.Encode("2128506");
            ShowDifference(expected, actual);
            Assert.AreEqual(expected, actual);
        }
    }
}
