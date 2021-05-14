// ReSharper disable CheckNamespace
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod

using System;
using System.Buffers;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.IO;

#nullable enable

namespace UnitTests.AM.IO
{
    [TestClass]
    public class ArrayPoolWriterTest
    {
        [TestMethod]
        [Description("Свежесозданный буфер")]
        public void ArrayPoolWriter_Construction_1()
        {
            using var writer = new ArrayPoolWriter();
            Assert.AreEqual(ArrayPoolWriter.DefaultChunkSize, writer.ChunkSize);
            Assert.AreEqual(0, writer.TotalWritten);
            Assert.AreEqual("0", writer.ToString());

            var array = writer.ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        [Description("Свежесозданный буфер")]
        public void ArrayPoolWriter_Construction_2()
        {
            const int chunkSize = 1024;
            var pool = ArrayPool<byte>.Create();
            using var writer = new ArrayPoolWriter(pool, chunkSize);
            Assert.AreEqual("0", writer.ToString());
            Assert.AreSame(pool, writer.Pool);
            Assert.AreEqual(chunkSize, writer.ChunkSize);
            Assert.AreEqual(0, writer.TotalWritten);

            var array = writer.ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        [Description("Неверный размер чанка")]
        [ExpectedException(typeof(ArgumentException))]
        public void ArrayPoolWriter_Construction_3()
        {
            using var writer = new ArrayPoolWriter(ArrayPool<byte>.Shared, 0);
            Assert.IsNotNull(writer);
        }

        [TestMethod]
        [Description("Запись в буфер пустого чанка")]
        public void ArrayPoolWriter_Write_1()
        {
            using var writer = new ArrayPoolWriter();
            writer.Write(ReadOnlySpan<byte>.Empty);
            Assert.AreEqual(0, writer.TotalWritten);
            Assert.AreEqual("0", writer.ToString());

            var array = writer.ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        [Description("Запись в буфер непустого чанка")]
        public void ArrayPoolWrite_Write_2()
        {
            using var writer = new ArrayPoolWriter();
            var data = new byte[] { 2, 12, 85, 0, 6 }.AsSpan();
            writer.Write(data);
            Assert.AreEqual(data.Length, writer.TotalWritten);
            Assert.AreEqual(data.Length.ToInvariantString(), writer.ToString());

            var array = writer.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(data.Length, array[0].Length);
            Assert.AreEqual(data[0], array[0].Span[0]);
        }

        [TestMethod]
        [Description("Запись в буфер большого чанка")]
        public void ArrayPoolWrite_Write_3()
        {
            using var writer = new ArrayPoolWriter(ArrayPool<byte>.Shared, 32);
            var data = new byte[1023].AsSpan();
            data.Fill(123);
            writer.Write(data);
            Assert.AreEqual(data.Length, writer.TotalWritten);
            Assert.AreEqual(data.Length.ToInvariantString(), writer.ToString());

            var array = writer.ToArray();
            Assert.AreEqual(32, array.Length);
            Assert.AreEqual(writer.ChunkSize, array[0].Length);
            Assert.AreEqual(31, array[^1].Length);
            Assert.AreEqual(data[0], array[0].Span[0]);
            Assert.AreEqual(data[^1], array[^1].Span[^1]);
        }

        [TestMethod]
        [Description("Побайтовая запись")]
        public void ArrayPoolWrite_Write_4()
        {
            using var writer = new ArrayPoolWriter();
            writer.Write(123);
            writer.Write(234);
            writer.Write(34);
            Assert.AreEqual(3, writer.TotalWritten);
            Assert.AreEqual("3", writer.ToString());

            var array = writer.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(3, array[0].Length);
            Assert.AreEqual(123, array[0].Span[0]);
            Assert.AreEqual(234, array[0].Span[1]);
            Assert.AreEqual(34, array[0].Span[2]);
        }

        [TestMethod]
        [Description("Перечисление пустого буфера")]
        public void ArrayPoolWrite_Enumerate_1()
        {
            using var writer = new ArrayPoolWriter();
            using var enumerator = writer.GetEnumerator();
            Assert.IsTrue(enumerator.Current.IsEmpty);
        }
    }
}
