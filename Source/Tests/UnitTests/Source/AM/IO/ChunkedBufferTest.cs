﻿using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

// ReSharper disable CheckNamespace

namespace UnitTests.AM.IO
{
    [TestClass]
    public class ChunkedBufferTest
    {
        [TestMethod]
        public void ChunkedBuffer_Construction_1()
        {
            var chunkSize = 4096;
            var buffer = new ChunkedBuffer(chunkSize);
            Assert.AreEqual(chunkSize, buffer.ChunkSize);
            Assert.AreEqual(0, buffer.Length);
            Assert.IsTrue(buffer.Eof);
        }

        [TestMethod]
        public void ChunkedBuffer_Construction_2()
        {
            var buffer = new ChunkedBuffer();
            Assert.AreEqual(ChunkedBuffer.DefaultChunkSize, buffer.ChunkSize);
            Assert.AreEqual(0, buffer.Length);
            Assert.IsTrue(buffer.Eof);
        }

        [TestMethod]
        public void ChunkedBuffer_CopyFrom_1()
        {
            var memory = new MemoryStream();
            for (var i = 0; i < 20; i++)
            {
                for (var j = 0; j < 256; j++)
                {
                    memory.WriteByte((byte) j);
                }
            }

            memory.Position = 0;
            var buffer = new ChunkedBuffer();
            buffer.CopyFrom(memory, 1024);
            Assert.AreEqual(20 * 256, buffer.Length);
            var original = memory.ToArray();
            var copy = buffer.ToBigArray();
            CollectionAssert.AreEqual(original, copy);
        }

        [TestMethod]
        public void ChunkedBuffer_Read_1()
        {
            var memory = new MemoryStream();
            for (var i = 0; i < 20; i++)
            {
                for (var j = 0; j < 256; j++)
                {
                    memory.WriteByte((byte) j);
                }
            }

            memory.Position = 0;
            var buffer = new ChunkedBuffer();
            buffer.CopyFrom(memory, 1024);
            buffer.Rewind();
            for (var i = 0; i < 20; i++)
            {
                Assert.IsFalse(buffer.Eof);
                var bytes = new byte[256];
                Assert.AreEqual(256, buffer.Read(bytes));
                for (var j = 0; j < 256; j++)
                {
                    Assert.AreEqual(j, bytes[j]);
                }
            }
            Assert.IsTrue(buffer.Eof);
        }

        [TestMethod]
        public void ChunkedBuffer_Read_2()
        {
            var buffer = new ChunkedBuffer();
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            var bytes = new byte[3];
            Assert.AreEqual(0, buffer.Read(bytes, 0, -1));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 0));
            Assert.AreEqual(3, buffer.Read(bytes, 0, 3));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 3));
        }

        [TestMethod]
        public void ChunkedBuffer_Read_3()
        {
            var buffer = new ChunkedBuffer();
            var bytes = new byte[3];
            Assert.AreEqual(0, buffer.Read(bytes, 0, -1));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 0));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 3));
            Assert.AreEqual(0, buffer.Read(bytes, 0, 3));
        }

        [TestMethod]
        public void ChunkedBuffer_Peek_1()
        {
            var buffer = new ChunkedBuffer();
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.ReadByte());
            Assert.AreEqual(2, buffer.Peek());
            Assert.AreEqual(2, buffer.Peek());
            Assert.AreEqual(2, buffer.ReadByte());
            Assert.AreEqual(3, buffer.Peek());
            Assert.AreEqual(3, buffer.Peek());
            Assert.AreEqual(3, buffer.ReadByte());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_Peek_2()
        {
            var memory = new MemoryStream();
            for (var i = 0; i < 16; i++)
            {
                for (var j = 0; j < 256; j++)
                {
                    memory.WriteByte((byte) j);
                }
            }

            memory.Position = 0;
            var buffer = new ChunkedBuffer();
            buffer.CopyFrom(memory, 1024);
            buffer.Rewind();
            Assert.AreEqual(0, buffer.Peek());
            Assert.AreEqual(0, buffer.Peek());
            Assert.AreEqual(0, buffer.ReadByte());
            var size = ChunkedBuffer.DefaultChunkSize - 1;
            var bytes = new byte[size];
            Assert.AreEqual(size, buffer.Read(bytes));
            Assert.AreEqual(0, buffer.Peek());
            Assert.AreEqual(0, buffer.Peek());
            Assert.AreEqual(0, buffer.ReadByte());
            Assert.AreEqual(size, buffer.Read(bytes));
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_Peek_3()
        {
            var buffer = new ChunkedBuffer(2);
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            buffer.WriteByte(4);
            buffer.WriteByte(5);
            buffer.WriteByte(6);
            buffer.Rewind();
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.ReadByte());
            Assert.AreEqual(2, buffer.Peek());
            Assert.AreEqual(2, buffer.Peek());
            Assert.AreEqual(2, buffer.ReadByte());
            Assert.AreEqual(3, buffer.Peek());
            Assert.AreEqual(3, buffer.Peek());
            Assert.AreEqual(3, buffer.ReadByte());
            Assert.AreEqual(4, buffer.Peek());
            Assert.AreEqual(4, buffer.Peek());
            Assert.AreEqual(4, buffer.ReadByte());
            Assert.AreEqual(5, buffer.Peek());
            Assert.AreEqual(5, buffer.Peek());
            Assert.AreEqual(5, buffer.ReadByte());
            Assert.AreEqual(6, buffer.Peek());
            Assert.AreEqual(6, buffer.Peek());
            Assert.AreEqual(6, buffer.ReadByte());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_ReadByte_1()
        {
            var buffer = new ChunkedBuffer(2);
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            buffer.WriteByte(4);
            buffer.WriteByte(5);
            buffer.WriteByte(6);
            buffer.Rewind();
            Assert.AreEqual(1, buffer.ReadByte());
            Assert.AreEqual(2, buffer.ReadByte());
            Assert.AreEqual(3, buffer.ReadByte());
            Assert.AreEqual(4, buffer.ReadByte());
            Assert.AreEqual(5, buffer.ReadByte());
            Assert.AreEqual(6, buffer.ReadByte());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_ReadByte_2()
        {
            var buffer = new ChunkedBuffer();
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_ReadByte_3()
        {
            var buffer = new ChunkedBuffer();
            buffer.WriteByte(1);
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.Peek());
            Assert.AreEqual(1, buffer.ReadByte());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.Peek());
            Assert.AreEqual(-1, buffer.ReadByte());
        }

        [TestMethod]
        public void ChunkedBuffer_ToArrays_1()
        {
            var buffer = new ChunkedBuffer();
            Assert.AreEqual(0, buffer.Length);
            var arrays = buffer.ToArrays(0);
            Assert.AreEqual(0, arrays.Length);
            arrays = buffer.ToArrays(1);
            Assert.AreEqual(1, arrays.Length);
            Assert.AreEqual(0, arrays[0].Length);
            arrays = buffer.ToArrays(2);
            Assert.AreEqual(2, arrays.Length);
            Assert.AreEqual(0, arrays[0].Length);
            Assert.AreEqual(0, arrays[1].Length);
        }

        [TestMethod]
        public void ChunkedBuffer_ToArrays_2()
        {
            var buffer = new ChunkedBuffer(2);
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            buffer.WriteByte(4);
            buffer.WriteByte(5);
            Assert.AreEqual(5, buffer.Length);
            var arrays = buffer.ToArrays(0);
            Assert.AreEqual(3, arrays.Length);
            Assert.AreEqual(2, arrays[0].Length);
            Assert.AreEqual(1, arrays[0][0]);
            Assert.AreEqual(2, arrays[0][1]);
            Assert.AreEqual(2, arrays[1].Length);
            Assert.AreEqual(3, arrays[1][0]);
            Assert.AreEqual(4, arrays[1][1]);
            Assert.AreEqual(1, arrays[2].Length);
            Assert.AreEqual(5, arrays[2][0]);
            arrays = buffer.ToArrays(1);
            Assert.AreEqual(4, arrays.Length);
            Assert.AreEqual(0, arrays[0].Length);
            Assert.AreEqual(2, arrays[1].Length);
            Assert.AreEqual(2, arrays[2].Length);
            Assert.AreEqual(1, arrays[3].Length);
        }

        [TestMethod]
        public void ChunkedBuffer_ToBigArray_1()
        {
            var buffer = new ChunkedBuffer();
            Assert.AreEqual(0, buffer.Length);
            var array = buffer.ToBigArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void ChunkedBuffer_ToBigArray_2()
        {
            var buffer = new ChunkedBuffer(2);
            buffer.WriteByte(1);
            buffer.WriteByte(2);
            buffer.WriteByte(3);
            buffer.WriteByte(4);
            buffer.WriteByte(5);
            Assert.AreEqual(5, buffer.Length);
            var array = buffer.ToBigArray();
            Assert.AreEqual(5, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
            Assert.AreEqual(5, array[4]);
        }

        [TestMethod]
        public void ChunkedBuffer_Write_1()
        {
            var buffer = new ChunkedBuffer(2);
            Assert.AreEqual(0, buffer.Length);
            var array = new byte[0];
            buffer.Write(array);
            Assert.AreEqual(0, buffer.Length);
            buffer.Write(array, 0, -1);
            Assert.AreEqual(0, buffer.Length);
            buffer.Write(array, 0, 0);
        }

        [TestMethod]
        public void ChunkedBuffer_Write_2()
        {
            var buffer = new ChunkedBuffer(2);
            Assert.AreEqual(0, buffer.Length);
            byte[] array = { 1, 2, 3, 4, 5 };
            buffer.Write(array);
            Assert.AreEqual(5, buffer.Length);
        }

        [TestMethod]
        public void ChunkedBuffer_Write_3()
        {
            var buffer = new ChunkedBuffer(2);
            Assert.AreEqual(0, buffer.Length);
            buffer.Write("Hello", Encoding.ASCII);
            Assert.AreEqual(5, buffer.Length);
            buffer.Rewind();
            var array = buffer.ToBigArray();
            Assert.AreEqual(5, array.Length);
            Assert.AreEqual(72, array[0]);
            Assert.AreEqual(101, array[1]);
            Assert.AreEqual(108, array[2]);
            Assert.AreEqual(108, array[3]);
            Assert.AreEqual(111, array[4]);
        }

        [TestMethod]
        public void ChunkedBuffer_Write_4()
        {
            var buffer = new ChunkedBuffer(2);
            Assert.AreEqual(0, buffer.Length);
            buffer.Write(string.Empty, Encoding.ASCII);
            Assert.AreEqual(0, buffer.Length);
            buffer.Rewind();
            var array = buffer.ToBigArray();
            Assert.AreEqual(0, array.Length);
        }

        [TestMethod]
        public void ChunkedBuffer_ReadLine_1()
        {
            var buffer = new ChunkedBuffer(2);
            var line = buffer.ReadLine(Encoding.ASCII);
            Assert.IsNull(line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.IsNull(line);
        }

        [TestMethod]
        public void ChunkedBuffer_ReadLine_2()
        {
            var buffer = new ChunkedBuffer(2);
            var expected = "Hello";
            buffer.Write(expected, Encoding.ASCII);
            var actual = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual(expected, actual);
            actual = buffer.ReadLine(Encoding.ASCII);
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void ChunkedBuffer_ReadLine_3()
        {
            var buffer = new ChunkedBuffer(2);
            buffer.Write("Hello\nworld", Encoding.ASCII);
            var line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual("Hello", line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual("world", line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.IsNull(line);
        }

        [TestMethod]
        public void ChunkedBuffer_ReadLine_4()
        {
            var buffer = new ChunkedBuffer(2);
            buffer.Write("Hello\r\nworld", Encoding.ASCII);
            var line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual("Hello", line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual("world", line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.IsNull(line);
        }

        [TestMethod]
        public void ChunkedBuffer_ReadLine_5()
        {
            var buffer = new ChunkedBuffer(2);
            buffer.Write("Hello\rworld", Encoding.ASCII);
            var line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual("Hello", line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual("world", line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.IsNull(line);
        }

        [TestMethod]
        public void ChunkedBuffer_ReadLine_6()
        {
            var buffer = new ChunkedBuffer(2);
            buffer.Write("Hello\rworld\n", Encoding.ASCII);
            var line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual("Hello", line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual("world", line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.IsNull(line);
        }

        [TestMethod]
        public void ChunkedBuffer_ReadLine_7()
        {
            var buffer = new ChunkedBuffer(2);
            buffer.Write("\n\r\n\r", Encoding.ASCII);
            var line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual(string.Empty, line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual(string.Empty, line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.AreEqual(string.Empty, line);
            line = buffer.ReadLine(Encoding.ASCII);
            Assert.IsNull(line);
        }
    }
}
