// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.IO;

using AM.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.AM.IO;

[TestClass]
public sealed class ValueBufferTest
    : Common.CommonUnitTest
{
    private Stream _GetLongStream()
    {
        var path = Path.Combine(TestDataPath, "bouni.ws");
        var result = File.OpenRead(path);

        return result;
    }

    private Stream _GetShortStream()
    {
        var path = Path.Combine(TestDataPath, "included.json");
        var result = File.OpenRead(path);

        return result;
    }

    [TestMethod]
    [Description("Чтение в буфер, выделенный на стеке")]
    public unsafe void ValueBuffer_Read_1()
    {
        const int initialSize = 16;
        byte *initialBytes = stackalloc byte [initialSize];
        var buffer = new ValueBuffer (initialBytes, initialSize);

        using var stream = _GetLongStream();
        var read = buffer.Read (stream, 10);
        Assert.AreEqual (10, read);
        Assert.IsTrue (buffer.Data.Length >= 10);

        read = buffer.Read (stream, 20);
        Assert.AreEqual (20, read);
        Assert.IsTrue (buffer.Data.Length >= 20);
    }

    [TestMethod]
    [Description("Чтение в автоматически выделенный буфер")]
    public void ValueBuffer_Read_2()
    {
        var buffer = new ValueBuffer ();

        using var stream = _GetLongStream();
        var read = buffer.Read (stream, 10);
        Assert.AreEqual (10, read);
        Assert.IsTrue (buffer.Data.Length >= 10);

        read = buffer.Read (stream, 20);
        Assert.AreEqual (20, read);
        Assert.IsTrue (buffer.Data.Length >= 20);
    }

    [TestMethod]
    [Description("Чтение в буфер, выделенный в куче")]
    public void ValueBuffer_Read_3()
    {
        var bytes = new byte[8];
        var memory = new System.Memory<byte>(bytes);
        var buffer = new ValueBuffer (memory);

        using var stream = _GetLongStream();
        var read = buffer.Read (stream, 10);
        Assert.AreEqual (10, read);
        Assert.IsTrue (buffer.Data.Length >= 10);

        read = buffer.Read (stream, 20);
        Assert.AreEqual (20, read);
        Assert.IsTrue (buffer.Data.Length >= 20);
    }

    [TestMethod]
    [Description("Чтение строк в буфер, выделенный на стеке")]
    public unsafe void ValueBuffer_ReadLine_1()
    {
        const int initialSize = 16;
        byte *initialBytes = stackalloc byte [initialSize];
        var buffer = new ValueBuffer (initialBytes, initialSize);
        var lineLength = new []
        {
            1, 4, 11, 11, 13, 6, 6, 1, 2, 2, 2, 1, 1,
            3, 40, 1, 7, 1, 7, 5, 0, 0, 0, 3, 11, 1
        };

        using var stream = _GetLongStream();
        foreach (var expected in lineLength)
        {
            var read = buffer.ReadLine (stream);
            Assert.AreEqual (expected, read);
            Assert.AreEqual (expected, buffer.Data.Length);
        }
    }

    [TestMethod]
    [Description("Чтение строк в автоматически выделенный буфер")]
    public void ValueBuffer_ReadLine_2()
    {
        var buffer = new ValueBuffer ();
        var lineLength = new []
        {
            1, 4, 11, 11, 13, 6, 6, 1, 2, 2, 2, 1, 1,
            3, 40, 1, 7, 1, 7, 5, 0, 0, 0, 3, 11, 1
        };

        using var stream = _GetLongStream();
        foreach (var expected in lineLength)
        {
            var read = buffer.ReadLine (stream);
            Assert.AreEqual (expected, read);
            Assert.AreEqual (expected, buffer.Data.Length);
        }
    }

    [TestMethod]
    [Description("Чтение строк из короткого файла")]
    public void ValueBuffer_ReadLine_3()
    {
        var buffer = new ValueBuffer ();
        var lineLength = new []
        {
            1, 13, 12, 1, -1
        };

        using var stream = _GetShortStream();
        foreach (var expectedRead in lineLength)
        {
            var read = buffer.ReadLine (stream);
            Assert.AreEqual (expectedRead, read);
            var expectedLength = expectedRead;
            if (expectedLength < 0)
            {
                expectedLength = 0;
            }
            Assert.AreEqual (expectedLength, buffer.Data.Length);
        }
    }
}
