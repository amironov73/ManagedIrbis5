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
    [Ignore]
    [TestMethod]
    public void QRCode_Generate_1()
    {
        // TODO разобраться, как победить на Linux
        // System.DllNotFoundException: Unable to load shared library 'libSkiaSharp' or one of its dependencies

        const string content = "Hello, world!";
        const string fileName = "qrcode1.png";

        var qrCode = new QrCode (content, new Vector2Slim (256, 256), SKEncodedImageFormat.Png);
        using var output = new FileStream (fileName, FileMode.CreateNew);
        qrCode.GenerateImage (output);
    }
}
