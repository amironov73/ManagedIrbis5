// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Drawing.QRCoding;

#nullable enable

namespace UnitTests.AM.Drawing.QRCoding;

[TestClass]
public sealed class QRCodingTest
    : DrawingTests.CommonUnitTest
{
    [TestMethod]
    public void QRCoding_GetGraphic_1()
    {
        var generator = new QRCodeGenerator();
        var message = "Hello, 21-st century! This is QR-Code";
        var data = generator.CreateQrCode (message, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode (data);
        using var image = qrCode.GetGraphic (6);
        image.Save ("qrcode-message.png");
    }

    [TestMethod]
    public void QRCoding_CreateQrCode_1()
    {
        var generator = new QRCodeGenerator();
        var url = new Url ("https://mironov.online");
        var message = url.ToString();
        var data = generator.CreateQrCode (message, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode (data);
        using var image = qrCode.GetGraphic (6);
        image.Save ("qrcode-url.png");
    }
}
