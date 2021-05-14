// ReSharper disable CheckNamespace
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

#nullable enable

namespace UnitTests.AM.IO
{
    // Алиас для краткости
    using Chunk = ReadOnlyMemory<byte>;

    [TestClass]
    public class MemoryReaderTest
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
        [Description("Свежесозданный пустой читатель")]
        public void MemoryReader_Construction_1()
        {
            var reader = new MemoryReader();
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(0, reader.Available);
            Assert.AreEqual(0, reader.TotalRead);
        }

        [TestMethod]
        [Description("Свежесозданный читатель с одним чанком")]
        public void MemoryReader_Construction_2()
        {
            var reader = new MemoryReader(GetSingleChunk());
            Assert.IsFalse(reader.IsEof);
            Assert.AreEqual(5, reader.Available);
            Assert.AreEqual(0, reader.TotalRead);
        }

        [TestMethod]
        [Description("Свежесозданный читатель с несколькими чанками")]
        public void MemoryReader_Construction_3()
        {
            var reader = new MemoryReader(GetMultipleChunks());
            Assert.IsFalse(reader.IsEof);
            Assert.AreEqual(11, reader.Available);
            Assert.AreEqual(0, reader.TotalRead);
        }

        [TestMethod]
        [Description("Добавление чанка")]
        public void MemoryReader_Add_1()
        {
            var reader = new MemoryReader();
            reader.Add(new byte[] { 2, 12, 85, 0, 6 });
            Assert.IsFalse(reader.IsEof);
            Assert.AreEqual(5, reader.Available);
            Assert.AreEqual(0, reader.TotalRead);
        }

        [TestMethod]
        [Description("Добавление чанка -- игнорирование пустых чанков")]
        public void MemoryReader_Add_2()
        {
            var reader = new MemoryReader();
            reader.Add(Chunk.Empty);
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(0, reader.Available);
            Assert.AreEqual(0, reader.TotalRead);
        }

        [TestMethod]
        [Description("Чтение пустого ридера")]
        public void MemoryReader_ReadByte_1()
        {
            var reader = new MemoryReader();
            Assert.AreEqual(-1, reader.ReadByte());
            Assert.AreEqual(-1, reader.ReadByte());
            Assert.AreEqual(0, reader.TotalRead);
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(0, reader.Available);
        }

        [TestMethod]
        [Description("Чтение одного чанка")]
        public void MemoryReader_ReadByte_2()
        {
            var reader = new MemoryReader(GetSingleChunk());
            Assert.AreEqual(2, reader.ReadByte());
            Assert.AreEqual(12, reader.ReadByte());
            Assert.AreEqual(85, reader.ReadByte());
            Assert.AreEqual(0, reader.ReadByte());
            Assert.AreEqual(6, reader.ReadByte());
            Assert.AreEqual(-1, reader.ReadByte());
            Assert.AreEqual(-1, reader.ReadByte());
            Assert.AreEqual(5, reader.TotalRead);
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(0, reader.Available);
        }

        [TestMethod]
        [Description("Чтение нескольких чанков")]
        public void MemoryReader_ReadByte_3()
        {
            var reader = new MemoryReader(GetMultipleChunks());
            Assert.AreEqual(2, reader.ReadByte());
            Assert.AreEqual(12, reader.ReadByte());
            Assert.AreEqual(85, reader.ReadByte());
            Assert.AreEqual(0, reader.ReadByte());
            Assert.AreEqual(6, reader.ReadByte());
            Assert.AreEqual(3, reader.ReadByte());
            Assert.AreEqual(14, reader.ReadByte());
            Assert.AreEqual(15, reader.ReadByte());
            Assert.AreEqual(9, reader.ReadByte());
            Assert.AreEqual(26, reader.ReadByte());
            Assert.AreEqual(5, reader.ReadByte());
            Assert.AreEqual(-1, reader.ReadByte());
            Assert.AreEqual(-1, reader.ReadByte());
            Assert.AreEqual(11, reader.TotalRead);
            Assert.AreEqual(0, reader.Available);
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(0, reader.Available);
        }

        [TestMethod]
        [Description("Чтение пустого ридера")]
        public void MemoryReader_ReadBlock_1()
        {
            var reader = new MemoryReader();
            var chunk = reader.ReadBlock(0);
            Assert.IsTrue(chunk.IsEmpty);

            chunk = reader.ReadBlock(10);
            Assert.IsTrue(chunk.IsEmpty);
        }

        [TestMethod]
        [Description("Чтение одного чанка")]
        public void MemoryReader_ReadBlock_2()
        {
            var reader = new MemoryReader(GetSingleChunk());
            var chunk = reader.ReadBlock(0);
            Assert.IsTrue(chunk.IsEmpty);

            chunk = reader.ReadBlock(10);
            Assert.IsFalse(chunk.IsEmpty);
            Assert.AreEqual(5, chunk.Length);
            Assert.AreEqual(2, chunk.Span[0]);
            Assert.AreEqual(12, chunk.Span[1]);
            Assert.AreEqual(85, chunk.Span[2]);
            Assert.AreEqual(0, chunk.Span[3]);
            Assert.AreEqual(6, chunk.Span[4]);
            Assert.AreEqual(5, reader.TotalRead);
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(0, reader.Available);

            chunk = reader.ReadBlock(10);
            Assert.IsTrue(chunk.IsEmpty);
        }

        [TestMethod]
        [Description("Чтение нескольких чанков")]
        public void MemoryReader_ReadBlock_3()
        {
            var reader = new MemoryReader(GetMultipleChunks());
            var chunk = reader.ReadBlock(0);
            Assert.IsTrue(chunk.IsEmpty);

            chunk = reader.ReadBlock(10);
            Assert.IsFalse(chunk.IsEmpty);
            Assert.AreEqual(5, chunk.Length);
            Assert.AreEqual(2, chunk.Span[0]);
            Assert.AreEqual(12, chunk.Span[1]);
            Assert.AreEqual(85, chunk.Span[2]);
            Assert.AreEqual(0, chunk.Span[3]);
            Assert.AreEqual(6, chunk.Span[4]);
            Assert.AreEqual(5, reader.TotalRead);
            Assert.IsFalse(reader.IsEof);
            Assert.AreEqual(6, reader.Available);

            chunk = reader.ReadBlock(5);
            Assert.IsFalse(chunk.IsEmpty);
            Assert.AreEqual(5, chunk.Length);
            Assert.AreEqual(3, chunk.Span[0]);
            Assert.AreEqual(14, chunk.Span[1]);
            Assert.AreEqual(15, chunk.Span[2]);
            Assert.AreEqual(9, chunk.Span[3]);
            Assert.AreEqual(26, chunk.Span[4]);
            Assert.AreEqual(10, reader.TotalRead);
            Assert.IsFalse(reader.IsEof);
            Assert.AreEqual(1, reader.Available);

            chunk = reader.ReadBlock(10);
            Assert.IsFalse(chunk.IsEmpty);
            Assert.AreEqual(1, chunk.Length);
            Assert.AreEqual(5, chunk.Span[0]);
            Assert.IsTrue(reader.IsEof);
            Assert.AreEqual(0, reader.Available);

            chunk = reader.ReadBlock(10);
            Assert.IsTrue(chunk.IsEmpty);
            Assert.IsTrue(reader.IsEof);
        }

        [TestMethod]
        [Description("Чтение вплоть до указанного символа из пустого ридера")]
        public void MemoryReader_ReadTo_1()
        {
            var reader = new MemoryReader();
            var unit = reader.ReadTo(10);
            Assert.IsTrue(unit.Chunk.IsEmpty);
            Assert.IsFalse(unit.Found);
        }

        [TestMethod]
        [Description("Чтение вплоть до указанного символа из одного чанка")]
        public void MemoryReader_ReadTo_2()
        {
            var reader = new MemoryReader(GetSingleChunk());
            var unit = reader.ReadTo(10);
            Assert.AreEqual(5, unit.Chunk.Length);
            Assert.IsFalse(unit.Found);
            Assert.AreEqual(2, unit.Chunk.Span[0]);
            Assert.AreEqual(0, reader.Available);
            Assert.AreEqual(5, reader.TotalRead);
            Assert.IsTrue(reader.IsEof);
        }

        [TestMethod]
        [Description("Чтение вплоть до указанного символа из одного чанка")]
        public void MemoryReader_ReadTo_3()
        {
            var reader = new MemoryReader(GetSingleChunk());
            var unit = reader.ReadTo(0);
            Assert.AreEqual(3, unit.Chunk.Length);
            Assert.IsTrue(unit.Found);
            Assert.AreEqual(2, unit.Chunk.Span[0]);
            Assert.AreEqual(1, reader.Available);
            Assert.AreEqual(4, reader.TotalRead);
            Assert.IsFalse(reader.IsEof);

            unit = reader.ReadTo(0);
            Assert.AreEqual(1, unit.Chunk.Length);
            Assert.IsFalse(unit.Found);
            Assert.AreEqual(6, unit.Chunk.Span[0]);
            Assert.AreEqual(0, reader.Available);
            Assert.AreEqual(5, reader.TotalRead);
            Assert.IsTrue(reader.IsEof);
        }

        [TestMethod]
        [Description("Чтение вплоть до указанного символа из нескольких чанков")]
        public void MemoryReader_ReadTo_4()
        {
            var reader = new MemoryReader(GetMultipleChunks());
            var unit = reader.ReadTo(10);
            Assert.AreEqual(5, unit.Chunk.Length);
            Assert.IsFalse(unit.Found);
            Assert.AreEqual(2, unit.Chunk.Span[0]);
            Assert.AreEqual(6, reader.Available);
            Assert.AreEqual(5, reader.TotalRead);
            Assert.IsFalse(reader.IsEof);

            unit = reader.ReadTo(10);
            Assert.AreEqual(6, unit.Chunk.Length);
            Assert.IsFalse(unit.Found);
            Assert.AreEqual(3, unit.Chunk.Span[0]);
            Assert.AreEqual(0, reader.Available);
            Assert.AreEqual(11, reader.TotalRead);
            Assert.IsTrue(reader.IsEof);
        }

        [TestMethod]
        [Description("Чтение вплоть до указанного символа из нескольких чанков")]
        public void MemoryReader_ReadTo_5()
        {
            var reader = new MemoryReader(GetMultipleChunks());
            var unit = reader.ReadTo(0);
            Assert.AreEqual(3, unit.Chunk.Length);
            Assert.IsTrue(unit.Found);
            Assert.AreEqual(2, unit.Chunk.Span[0]);
            Assert.AreEqual(7, reader.Available);
            Assert.AreEqual(4, reader.TotalRead);
            Assert.IsFalse(reader.IsEof);

            unit = reader.ReadTo(0);
            Assert.AreEqual(1, unit.Chunk.Length);
            Assert.IsFalse(unit.Found);
            Assert.AreEqual(6, unit.Chunk.Span[0]);
            Assert.AreEqual(6, reader.Available);
            Assert.AreEqual(5, reader.TotalRead);
            Assert.IsFalse(reader.IsEof);

            unit = reader.ReadTo(0);
            Assert.AreEqual(6, unit.Chunk.Length);
            Assert.IsFalse(unit.Found);
            Assert.AreEqual(3, unit.Chunk.Span[0]);
            Assert.AreEqual(0, reader.Available);
            Assert.AreEqual(11, reader.TotalRead);
            Assert.IsTrue(reader.IsEof);
        }
    }
}
