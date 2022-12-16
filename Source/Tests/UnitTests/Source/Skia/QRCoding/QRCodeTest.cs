// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

#region Using directives

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Skia.QrCoding;

using SkiaSharp;

#endregion

#nullable enable

namespace UnitTests.AM.Skia.QRCoding;

[TestClass]
public sealed class QRCodeTest
{
    [TestMethod]
    public void QRCode_Generate_1()
    {
        const string content = "Hello, world!";
        const string outputFile = "qrcode1.png";
        File.Delete (outputFile);

        var qrCode = new QrCode (content, new Vector2Slim (256, 256), SKEncodedImageFormat.Png);
        using var output = new FileStream (outputFile, FileMode.CreateNew);
        qrCode.GenerateImage (output);
    }
}
