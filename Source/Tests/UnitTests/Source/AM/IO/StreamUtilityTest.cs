using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

#nullable enable

// ReSharper disable CheckNamespace
// ReSharper disable InvokeAsExtensionMethod

namespace UnitTests.AM.IO
{
    [TestClass]
    public class StreamUtilityTest
    {
        [TestMethod]
        public void StreamUtility_AppendTo_1()
        {
            byte[] sourceArray = { 1, 2, 3, 4 };
            var sourceStream = new MemoryStream(sourceArray);
            var destinationStream = new MemoryStream();
            StreamUtility.AppendTo(sourceStream, destinationStream, -1);
            var destinationArray = destinationStream.ToArray();
            Assert.AreEqual(4, destinationArray.Length);
            Assert.AreEqual(1, destinationArray[0]);
            Assert.AreEqual(2, destinationArray[1]);
            Assert.AreEqual(3, destinationArray[2]);
            Assert.AreEqual(4, destinationArray[3]);
        }

        private int _Compare
            (
                byte[] firstArray,
                byte[] secondArray
            )
        {
            var firstStream = new MemoryStream(firstArray);
            var secondStream = new MemoryStream(secondArray);
            var result = StreamUtility.CompareTo(firstStream, secondStream);

            return result;
        }

        [TestMethod]
        public void StreamUtility_CompareTo_1()
        {
            byte[] firstArray = { 1, 2, 3, 4 };
            byte[] secondArray = { 1, 2, 3, 4 };
            Assert.IsTrue(_Compare(firstArray, secondArray) == 0);

            secondArray = new byte[] { 1, 2, 3 };
            Assert.IsTrue(_Compare(firstArray, secondArray) > 0);

            secondArray = new byte[] { 1, 2, 3, 4, 5 };
            Assert.IsTrue(_Compare(firstArray, secondArray) < 0);

            secondArray = new byte[] { 1, 2, 3, 5 };
            Assert.IsTrue(_Compare(firstArray, secondArray) < 0);
        }

        [TestMethod]
        public void StreamUtility_ReadBoolean_1()
        {
            byte[] buffer = { 0, 1, 2 };
            var stream = new MemoryStream(buffer);
            Assert.IsFalse(StreamUtility.ReadBoolean(stream));
            Assert.IsTrue(StreamUtility.ReadBoolean(stream));
            Assert.IsTrue(StreamUtility.ReadBoolean(stream));
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void StreamUtility_ReadBoolean_2()
        {
            var buffer = new byte[0];
            var stream = new MemoryStream(buffer);
            StreamUtility.ReadBoolean(stream);
        }

        [TestMethod]
        public void StreamUtility_ReadBytes_1()
        {
            byte[] buffer = { 1, 2, 3, 4 };
            var stream = new MemoryStream(buffer);
            var read = StreamUtility.ReadBytes(stream, 2)!;
            Assert.IsNotNull(read);
            Assert.AreEqual(2, read.Length);
            Assert.AreEqual(1, read[0]);
            Assert.AreEqual(2, read[1]);
        }

        [TestMethod]
        public void StreamUtility_ReadBytes_2()
        {
            byte[] buffer = { 1, 2, 3, 4 };
            var stream = new MemoryStream(buffer);
            var read = StreamUtility.ReadBytes(stream, 6)!;
            Assert.IsNotNull(read);
            Assert.AreEqual(4, read.Length);
            Assert.AreEqual(1, read[0]);
            Assert.AreEqual(2, read[1]);
            Assert.AreEqual(3, read[2]);
            Assert.AreEqual(4, read[3]);
        }

        [TestMethod]
        public void StreamUtility_ReadBytes_3()
        {
            var buffer = new byte[0];
            var stream = new MemoryStream(buffer);
            var read = StreamUtility.ReadBytes(stream, 6);
            Assert.IsNull(read);
        }

        [TestMethod]
        public void StreamUtility_ReadDateTime_1()
        {
            var stream = new MemoryStream();
            var expected = new DateTime(2017, 12, 4, 12, 29, 0);
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadDateTime(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadDecimal_1()
        {
            var stream = new MemoryStream();
            var expected = 123.45m;
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadDecimal(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadDouble_1()
        {
            var stream1 = new MemoryStream();
            var expected = 123.45;
            StreamUtility.Write(stream1, expected);
            var buffer = stream1.ToArray();
            var stream2 = new MemoryStream(buffer);
            var actual = StreamUtility.ReadDouble(stream2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt16_1()
        {
            var stream = new MemoryStream();
            short expected = 123;
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadInt16(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt32_1()
        {
            var stream = new MemoryStream();
            var expected = 123;
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadInt32(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt64_1()
        {
            var stream1 = new MemoryStream();
            long expected = 123;
            StreamUtility.Write(stream1, expected);
            var buffer = stream1.ToArray();
            var stream2 = new MemoryStream(buffer);
            var actual = StreamUtility.ReadInt64(stream2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt16_1()
        {
            var stream = new MemoryStream();
            ushort expected = 123;
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadUInt16(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt32_1()
        {
            var stream = new MemoryStream();
            uint expected = 123;
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadUInt32(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt64_1()
        {
            var stream = new MemoryStream();
            ulong expected = 123;
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadUInt64(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt16Array_1()
        {
            var stream = new MemoryStream();
            short[] expected = { 123, 234, 456 };
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadInt16Array(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt16Array_1()
        {
            var stream = new MemoryStream();
            ushort[] expected = { 123, 234, 456 };
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadUInt16Array(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt32Array_1()
        {
            var stream = new MemoryStream();
            int[] expected = { 123, 234, 456 };
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadInt32Array(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadUInt32Array_1()
        {
            var stream = new MemoryStream();
            uint[] expected = { 123, 234, 456 };
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadUInt32Array(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadSingle_1()
        {
            var stream = new MemoryStream();
            var expected = 123.45f;
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadSingle(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadString_1()
        {
            var stream = new MemoryStream();
            var expected = "Hello, world!";
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadString(stream);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadString_2()
        {
            var stream = new MemoryStream();
            var expected = "Hello, world!";
            var encoding = Encoding.ASCII;
            StreamUtility.Write(stream, expected, encoding);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadString(stream, encoding);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadStringArray_1()
        {
            var stream = new MemoryStream();
            string[] expected = {"Hello", "world!"};
            StreamUtility.Write(stream, expected);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadStringArray(stream);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadStringArray_2()
        {
            var stream = new MemoryStream();
            string[] expected = {"Hello", "world!"};
            var encoding = Encoding.ASCII;
            StreamUtility.Write(stream, expected, encoding);
            var buffer = stream.ToArray();
            stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadStringArray(stream, encoding);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StreamUtility_HostToNetwork16_1()
        {
            byte[] array = {1, 2};
            StreamUtility.HostToNetwork16(array, 0);
            Assert.AreEqual(2, array[0]);
            Assert.AreEqual(1, array[1]);
        }

        [TestMethod]
        public unsafe void StreamUtility_HostToNetwork16_2()
        {
            byte[] array = {1, 2};
            fixed (byte* ptr = &array[0])
            {
                StreamUtility.HostToNetwork16(ptr);
                Assert.AreEqual(2, array[0]);
                Assert.AreEqual(1, array[1]);
            }
        }

        [TestMethod]
        public void StreamUtility_HostToNetwork32_1()
        {
            byte[] array = {1, 2, 3, 4};
            StreamUtility.HostToNetwork32(array, 0);
            Assert.AreEqual(4, array[0]);
            Assert.AreEqual(3, array[1]);
            Assert.AreEqual(2, array[2]);
            Assert.AreEqual(1, array[3]);
        }

        [TestMethod]
        public unsafe void StreamUtility_HostToNetwork32_2()
        {
            byte[] array = {1, 2, 3, 4};
            fixed (byte* ptr = &array[0])
            {
                StreamUtility.HostToNetwork32(ptr);
                Assert.AreEqual(4, array[0]);
                Assert.AreEqual(3, array[1]);
                Assert.AreEqual(2, array[2]);
                Assert.AreEqual(1, array[3]);
            }
        }

        [TestMethod]
        public void StreamUtility_HostToNetwork64_1()
        {
            byte[] array = {1, 2, 3, 4, 5, 6, 7, 8};
            StreamUtility.HostToNetwork64(array, 0);
            Assert.AreEqual(4, array[0]);
            Assert.AreEqual(3, array[1]);
            Assert.AreEqual(2, array[2]);
            Assert.AreEqual(1, array[3]);
            Assert.AreEqual(8, array[4]);
            Assert.AreEqual(7, array[5]);
            Assert.AreEqual(6, array[6]);
            Assert.AreEqual(5, array[7]);
        }

        [TestMethod]
        public unsafe void StreamUtility_HostToNetwork64_2()
        {
            byte[] array = {1, 2, 3, 4, 5, 6, 7, 8};
            fixed (byte* ptr = &array[0])
            {
                StreamUtility.HostToNetwork64(ptr);
                Assert.AreEqual(4, array[0]);
                Assert.AreEqual(3, array[1]);
                Assert.AreEqual(2, array[2]);
                Assert.AreEqual(1, array[3]);
                Assert.AreEqual(8, array[4]);
                Assert.AreEqual(7, array[5]);
                Assert.AreEqual(6, array[6]);
                Assert.AreEqual(5, array[7]);
            }
        }

        [TestMethod]
        public void StreamUtility_NetworkToHost16_1()
        {
            byte[] array = {1, 2};
            StreamUtility.NetworkToHost16(array, 0);
            Assert.AreEqual(2, array[0]);
            Assert.AreEqual(1, array[1]);
        }

        [TestMethod]
        public unsafe void StreamUtility_NetworkToHost16_2()
        {
            byte[] array = {1, 2};
            fixed (byte* ptr = &array[0])
            {
                StreamUtility.NetworkToHost16(ptr);
                Assert.AreEqual(2, array[0]);
                Assert.AreEqual(1, array[1]);
            }
        }

        [TestMethod]
        public void StreamUtility_NetworkToHost32_1()
        {
            byte[] array = {1, 2, 3, 4};
            StreamUtility.NetworkToHost32(array, 0);
            Assert.AreEqual(4, array[0]);
            Assert.AreEqual(3, array[1]);
            Assert.AreEqual(2, array[2]);
            Assert.AreEqual(1, array[3]);
        }

        [TestMethod]
        public unsafe void StreamUtility_NetworkToHost32_2()
        {
            byte[] array = {1, 2, 3, 4};
            fixed (byte* ptr = &array[0])
            {
                StreamUtility.NetworkToHost32(ptr);
                Assert.AreEqual(4, array[0]);
                Assert.AreEqual(3, array[1]);
                Assert.AreEqual(2, array[2]);
                Assert.AreEqual(1, array[3]);
            }
        }

        [TestMethod]
        public void StreamUtility_NetworkToHost64_1()
        {
            byte[] array = {1, 2, 3, 4, 5, 6, 7, 8};
            StreamUtility.NetworkToHost64(array, 0);
            Assert.AreEqual(4, array[0]);
            Assert.AreEqual(3, array[1]);
            Assert.AreEqual(2, array[2]);
            Assert.AreEqual(1, array[3]);
            Assert.AreEqual(8, array[4]);
            Assert.AreEqual(7, array[5]);
            Assert.AreEqual(6, array[6]);
            Assert.AreEqual(5, array[7]);
        }

        [TestMethod]
        public unsafe void StreamUtility_NetworkToHost64_2()
        {
            byte[] array = {1, 2, 3, 4, 5, 6, 7, 8};
            fixed (byte* ptr = &array[0])
            {
                StreamUtility.NetworkToHost64(ptr);
                Assert.AreEqual(4, array[0]);
                Assert.AreEqual(3, array[1]);
                Assert.AreEqual(2, array[2]);
                Assert.AreEqual(1, array[3]);
                Assert.AreEqual(8, array[4]);
                Assert.AreEqual(7, array[5]);
                Assert.AreEqual(6, array[6]);
                Assert.AreEqual(5, array[7]);
            }
        }

        [TestMethod]
        public void StreamUtility_ReadInt16Network_1()
        {
            byte[] array = {1, 2};
            var stream = new MemoryStream(array);
            var actual = StreamUtility.ReadInt16Network(stream);
            Assert.AreEqual(0x0102, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt32Network_1()
        {
            byte[] array = {1, 2, 3, 4};
            var stream = new MemoryStream(array);
            var actual = StreamUtility.ReadInt32Network(stream);
            Assert.AreEqual(0x01020304, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt64Network_1()
        {
            byte[] array = {1, 2, 3, 4, 5, 6, 7, 8};
            var stream = new MemoryStream(array);
            var actual = StreamUtility.ReadInt64Network(stream);
            Assert.AreEqual(0x0506070801020304L, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt16Host_1()
        {
            byte[] array = {1, 2};
            var stream = new MemoryStream(array);
            var actual = StreamUtility.ReadInt16Host(stream);
            Assert.AreEqual(0x0201, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt32Host_1()
        {
            byte[] array = {1, 2, 3, 4};
            var stream = new MemoryStream(array);
            var actual = StreamUtility.ReadInt32Host(stream);
            Assert.AreEqual(0x04030201, actual);
        }

        [TestMethod]
        public void StreamUtility_ReadInt64Host_1()
        {
            byte[] array = {1, 2, 3, 4, 5, 6, 7, 8};
            var stream = new MemoryStream(array);
            var actual = StreamUtility.ReadInt64Host(stream);
            Assert.AreEqual(0x0807060504030201L, actual);
        }

        [TestMethod]
        public void StreamUtility_WriteInt16Network_1()
        {
            var stream = new MemoryStream();
            StreamUtility.WriteInt16Network(stream, 0x0102);
            var array = stream.ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
        }

        [TestMethod]
        public void StreamUtility_WriteInt32Network_1()
        {
            var stream = new MemoryStream();
            StreamUtility.WriteInt32Network(stream, 0x01020304);
            var array = stream.ToArray();
            Assert.AreEqual(4, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
        }

        [TestMethod]
        public void StreamUtility_WriteInt64Network_1()
        {
            var stream = new MemoryStream();
            StreamUtility.WriteInt64Network(stream, 0x0102030405060708L);
            var array = stream.ToArray();
            Assert.AreEqual(8, array.Length);
            Assert.AreEqual(5, array[0]);
            Assert.AreEqual(6, array[1]);
            Assert.AreEqual(7, array[2]);
            Assert.AreEqual(8, array[3]);
            Assert.AreEqual(1, array[4]);
            Assert.AreEqual(2, array[5]);
            Assert.AreEqual(3, array[6]);
            Assert.AreEqual(4, array[7]);
        }

        [TestMethod]
        public void StreamUtility_WriteUInt16Network_1()
        {
            var stream = new MemoryStream();
            StreamUtility.WriteUInt16Network(stream, 0x0102);
            var array = stream.ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
        }

        [TestMethod]
        public void StreamUtility_WriteUInt32Network_1()
        {
            var stream = new MemoryStream();
            StreamUtility.WriteUInt32Network(stream, 0x01020304);
            var array = stream.ToArray();
            Assert.AreEqual(4, array.Length);
            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(2, array[1]);
            Assert.AreEqual(3, array[2]);
            Assert.AreEqual(4, array[3]);
        }

        [TestMethod]
        public void StreamUtility_WriteUInt64Network_1()
        {
            var stream = new MemoryStream();
            StreamUtility.WriteUInt64Network(stream, 0x0102030405060708L);
            var array = stream.ToArray();
            Assert.AreEqual(8, array.Length);
            Assert.AreEqual(5, array[0]);
            Assert.AreEqual(6, array[1]);
            Assert.AreEqual(7, array[2]);
            Assert.AreEqual(8, array[3]);
            Assert.AreEqual(1, array[4]);
            Assert.AreEqual(2, array[5]);
            Assert.AreEqual(3, array[6]);
            Assert.AreEqual(4, array[7]);
        }

        [TestMethod]
        public void StreamUtility_WriteBoolean_1()
        {
            var stream = new MemoryStream();
            StreamUtility.Write(stream, false);
            StreamUtility.Write(stream, true);
            var array = stream.ToArray();
            Assert.AreEqual(2, array.Length);
            Assert.AreEqual(0, array[0]);
            Assert.AreEqual(1, array[1]);
        }

        [TestMethod]
        public void StreamUtility_ReadExact_1()
        {
            byte[] buffer = {1, 2, 3, 4, 5};
            var stream = new MemoryStream(buffer);
            var actual = StreamUtility.ReadExact(stream, 4);
            Assert.AreEqual(4, actual.Length);
            Assert.AreEqual(1, actual[0]);
            Assert.AreEqual(2, actual[1]);
            Assert.AreEqual(3, actual[2]);
            Assert.AreEqual(4, actual[3]);
            Assert.AreEqual(4L, stream.Position);
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void StreamUtility_ReadExact_2()
        {
            byte[] buffer = {1, 2};
            var stream = new MemoryStream(buffer);
            StreamUtility.ReadExact(stream, 4);
        }

        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void StreamUtility_ReadExact_3()
        {
            var bytes = new byte[4];
            var span = new Span<byte>(bytes);
            var stream = new MemoryStream();
            StreamUtility.ReadExact(stream, span);
        }

        [TestMethod]
        public void StreamUtility_ReadToEnd_1()
        {
            byte[] buffer = {1, 2, 3, 4, 5, 6, 7, 8};
            var stream = new MemoryStream(buffer);
            StreamUtility.ReadExact(stream, 4);
            var actual = StreamUtility.ReadToEnd(stream);
            Assert.AreEqual(4, actual.Length);
            Assert.AreEqual(5, actual[0]);
            Assert.AreEqual(6, actual[1]);
            Assert.AreEqual(7, actual[2]);
            Assert.AreEqual(8, actual[3]);
        }
    }
}
