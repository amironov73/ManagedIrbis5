// ReSharper disable CheckNamespace
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

#nullable enable

namespace UnitTests.AM.IO
{
    // Алиас для краткости
    using Chunk = ReadOnlyMemory<byte>;

    [TestClass]
    public class MemoryReaderUtilityTest
    {
        private static Chunk[] GetSingleChunk() => new Chunk[]
        {
            new byte[] { 2, 12, 85, 0, 6 }
        };

        private static Chunk[] GetMultipleChunks() => new Chunk[]
        {
            new byte[] { 2, 12, 85, 0, 6 },
            new byte[] { 3, 14, 15, 9, 26, 5 }
        };

        [TestMethod]
        [Description("Перечисление пустого читателя")]
        public void MemoryReaderUtility_Enumerate_1()
        {
            var reader = new MemoryReader();
            var array = reader.Enumerate().ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        [Description("Перечисление единственного чанка")]
        public void MemoryReaderUtility_Enumerate_2()
        {
            var reader = new MemoryReader(GetSingleChunk());
            var array = reader.Enumerate().ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.AreEqual(5, array[0].Length);
        }

        [TestMethod]
        [Description("Перечисление нескольких чанков")]
        public void MemoryReaderUtility_Enumerate_3()
        {
            var reader = new MemoryReader(GetMultipleChunks());
            var array = reader.Enumerate().ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(5, array[0].Length);
            Assert.AreEqual(6, array[1].Length);
        }

        [TestMethod]
        [Description("Чтение непрерывного блока из пустого читателя")]
        public void MemoryReaderUtility_ReadContinuous_1()
        {
            var reader = new MemoryReader();
            var chunk = reader.ReadContinuous();
            Assert.IsTrue(chunk.IsEmpty);
        }

        [TestMethod]
        [Description("Чтение непрерывного блока из одного чанка")]
        public void MemoryReaderUtility_ReadContinuous_2()
        {
            var reader = new MemoryReader(GetSingleChunk());
            var chunk = reader.ReadContinuous(4);
            Assert.AreEqual(4, chunk.Length);
            Assert.IsFalse(reader.IsEof);
            Assert.AreEqual(1, reader.Available);
            Assert.AreEqual(2, chunk.Span[0]);
        }

        [TestMethod]
        [Description("Чтение непрерывного блока из одного чанка")]
        public void MemoryReaderUtility_ReadContinuous_3()
        {
            var reader = new MemoryReader(GetSingleChunk());
            var chunk = reader.ReadContinuous();
            Assert.AreEqual(5, chunk.Length);
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(0, reader.Available);
            Assert.AreEqual(2, chunk.Span[0]);
        }

        [TestMethod]
        [Description("Чтение непрерывного блока из нескольких чанков")]
        public void MemoryReaderUtility_ReadContinuous_4()
        {
            var reader = new MemoryReader(GetMultipleChunks());
            var chunk = reader.ReadContinuous(4);
            Assert.IsFalse(chunk.IsEmpty);
            Assert.IsFalse(reader.IsEof);
            Assert.AreEqual(4, chunk.Length);
            Assert.AreEqual(7, reader.Available);
            Assert.AreEqual(2, chunk.Span[0]);

            chunk = reader.ReadContinuous(4);
            Assert.IsFalse(chunk.IsEmpty);
            Assert.IsFalse(reader.IsEof);
            Assert.AreEqual(4, chunk.Length);
            Assert.AreEqual(3, reader.Available);
            Assert.AreEqual(6, chunk.Span[0]);

            chunk = reader.ReadContinuous(3);
            Assert.IsFalse(chunk.IsEmpty);
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(3, chunk.Length);
            Assert.AreEqual(0, reader.Available);
            Assert.AreEqual(9, chunk.Span[0]);

            chunk = reader.ReadContinuous(4);
            Assert.IsTrue(chunk.IsEmpty);
            Assert.IsTrue(reader.IsEof);
        }

        [TestMethod]
        [Description("Чтение непрерывного блока из нескольких чанков")]
        public void MemoryReaderUtility_ReadContinuous_5()
        {
            var reader = new MemoryReader(GetMultipleChunks());
            var chunk = reader.ReadContinuous(9);
            Assert.IsFalse(chunk.IsEmpty);
            Assert.IsFalse(reader.IsEof);
            Assert.AreEqual(9, chunk.Length);
            Assert.AreEqual(2, reader.Available);
            Assert.AreEqual(2, chunk.Span[0]);
            Assert.AreEqual(9, chunk.Span[8]);

            chunk = reader.ReadContinuous(4);
            Assert.IsFalse(chunk.IsEmpty);
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(2, chunk.Length);
            Assert.AreEqual(0, reader.Available);
            Assert.AreEqual(26, chunk.Span[0]);
            Assert.AreEqual(5, chunk.Span[1]);

            chunk = reader.ReadContinuous(3);
            Assert.IsTrue(chunk.IsEmpty);
            Assert.IsTrue(reader.IsEof);
        }
    }
}
