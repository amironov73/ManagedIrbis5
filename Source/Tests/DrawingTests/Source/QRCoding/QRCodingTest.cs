// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

/* QRCodingTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Drawing.QRCoding;

#endregion

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
