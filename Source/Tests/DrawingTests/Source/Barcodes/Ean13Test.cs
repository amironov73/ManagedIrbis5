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
                Message = "4600051000057"
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
            var expected = "10101011110100111000110100011010111001011001101010111001011100101110010111001010011101000100101";
            var data = new BarcodeData
            {
                Message = "4600051000057"
            };
            var actual = barcode.Encode(data);
            ShowDifference(expected, actual);
            Assert.AreEqual(expected, actual);
        }
    }
}
