// ReSharper disable CheckNamespace
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;
using AM.IO;
using AM.Runtime;

namespace UnitTests.AM.IO
{
    [TestClass]
    public class BinaryReaderUtilityTest
    {
        private class Dummy
            : IHandmadeSerializable
        {
            public int Value { get; set; }

            public void RestoreFromStream(BinaryReader reader)
            {
                Value = reader.ReadInt32();
            }

            public void SaveToStream(BinaryWriter writer)
            {
                writer.Write(Value);
            }
        }

        // ==========================================================

        [TestMethod]
        public void BinaryReaderUtility_ReadCollectionT_1()
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);

            NonNullCollection<Dummy> expected = new NonNullCollection<Dummy>
            {
                new Dummy { Value = 123 },
                new Dummy { Value = 456 },
                new Dummy { Value = 789 }
            };
            BinaryWriterUtility.Write(writer, expected);

            byte[] bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);

            NonNullCollection<Dummy> actual = new NonNullCollection<Dummy>();
            BinaryReaderUtility.ReadCollection(reader, actual);
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[0].Value, actual[0].Value);
            Assert.AreEqual(expected[1].Value, actual[1].Value);
            Assert.AreEqual(expected[2].Value, actual[2].Value);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void BinaryReaderUtility_ReadPackedInt32_1()
        {
            byte[] bytes = {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF};
            MemoryStream stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            BinaryReaderUtility.ReadPackedInt32(reader);
        }

        [TestMethod]
        public void BinaryReaderUtility_ReadString_1()
        {
            byte[] bytes = { 72, 101, 108, 108, 111 };
            MemoryStream stream = new MemoryStream(bytes);
            BinaryReader reader = new BinaryReader(stream);
            string actual = BinaryReaderUtility.ReadString(reader, 5);
            Assert.AreEqual("Hello", actual);
        }
    }
}
