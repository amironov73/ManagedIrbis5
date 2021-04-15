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
    public class Code128Test
        : DrawingTests.CommonUnitTest
    {
        [TestMethod]
        public void Code128_DrawBarcode_1()
        {
            using var image = new Bitmap(500, 100);
            using var graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Lime);

            var barcode = new Code128();
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
            image.Save("Code128.bmp");
        }

        [Ignore]
        [TestMethod]
        public void Code128_Encode_1()
        {
            var expected = "101100101001011011010110100101101101001011011010101011010011010100110101011001";
            var actual = Code128.Encode("2128506");
            ShowDifference(expected, actual);
            Assert.AreEqual(expected, actual);
        }
    }
}
