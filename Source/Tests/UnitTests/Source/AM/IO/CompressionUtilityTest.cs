// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

#endregion

#nullable enable

namespace UnitTests.AM.IO;

[TestClass]
public sealed class CompressionUtilityTest
{
    private void _TestCompression
        (
            byte[] first
        )
    {
        var zipped = CompressionUtility.Compress (first);
        var second = CompressionUtility.Decompress (zipped);

        Assert.AreEqual (first.Length, second.Length);

        for (var i = 0; i < first.Length; i++)
        {
            Assert.AreEqual (first[i], second[i]);
        }
    }

    [TestMethod]
    public void CompressionUtility_Compress_Decompress()
    {
        var bytes = Array.Empty<byte>();
        _TestCompression (bytes);

        bytes = new byte[] { 0 };
        _TestCompression (bytes);

        bytes = new byte[] { 0, 1, 2, 3 };
        _TestCompression (bytes);
    }
}
