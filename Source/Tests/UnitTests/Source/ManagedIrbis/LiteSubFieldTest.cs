using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

#nullable enable

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class LiteSubFieldTest
    {
        [TestMethod]
        public void LiteSubField_Constructor_1()
        {
            var subField = new LiteSubField();
            Assert.AreEqual(LiteSubField.NoCode, subField.Code);
            Assert.AreEqual(null, subField.Value);
            Assert.AreEqual(string.Empty, subField.ToString());

            var value = IrbisEncoding.Utf8.GetBytes("The value");
            subField = new LiteSubField { Code = (byte)'A', Value = value };
            Assert.AreEqual((byte)'A', subField.Code);
            Assert.AreEqual(value, subField.Value);
            Assert.AreEqual("^AThe value", subField.ToString());

            var clone = subField.Clone();
            Assert.AreEqual(subField.Code, clone.Code);
            Assert.AreEqual(subField.Value, clone.Value);
            Assert.AreEqual("^AThe value", clone.ToString());
        }

        [TestMethod]
        public void LiteSubField_Decode_1()
        {
            var subField = new LiteSubField();
            subField.Decode(ReadOnlyMemory<byte>.Empty);
            Assert.AreEqual(LiteSubField.NoCode, subField.Code);
            Assert.IsTrue(subField.Value.IsEmpty);
        }

        [TestMethod]
        public void LiteSubField_Decode_2()
        {
            var subField = new LiteSubField();
            var text = IrbisEncoding.Utf8.GetBytes("A");
            subField.Decode(text);
            Assert.AreEqual((byte)'A', subField.Code);
            Assert.AreEqual(0, subField.Value.Length);
        }

        [TestMethod]
        public void LiteSubField_Decode_3()
        {
            var subField = new LiteSubField();
            var text = IrbisEncoding.Utf8.GetBytes("AValue");
            subField.Decode(text);
            Assert.AreEqual((byte)'A', subField.Code);
            Assert.AreEqual(5, subField.Value.Length);
            Assert.AreEqual((byte)'V', subField.Value.Span[0]);
            Assert.AreEqual((byte)'a', subField.Value.Span[1]);
            Assert.AreEqual((byte)'l', subField.Value.Span[2]);
            Assert.AreEqual((byte)'u', subField.Value.Span[3]);
            Assert.AreEqual((byte)'e', subField.Value.Span[4]);
        }

        [TestMethod]
        public void LiteSubField_Verify_1()
        {
            var subField = new LiteSubField();
            Assert.IsFalse(subField.Verify(false));

            subField = new LiteSubField { Code = (byte)'a' };
            Assert.IsTrue(subField.Verify(false));

            var value = IrbisEncoding.Utf8.GetBytes("Title");
            subField = new LiteSubField { Code = (byte)'a', Value = value };
            Assert.IsTrue(subField.Verify(false));
        }

        [TestMethod]
        public void LiteSubField_Verify_2()
        {
            var value = IrbisEncoding.Utf8.GetBytes("Hello^World");
            var subField = new LiteSubField { Code = (byte)'a', Value = value };
            Assert.IsFalse(subField.Verify(false));
        }

        [TestMethod]
        public void LiteSubField_ToString_1()
        {
            var subField = new LiteSubField();
            Assert.AreEqual(string.Empty, subField.ToString());

            subField = new LiteSubField { Code = (byte)'a' };
            Assert.AreEqual("^a", subField.ToString());

            subField = new LiteSubField { Code = (byte)'A' };
            Assert.AreEqual("^A", subField.ToString());

            var value = IrbisEncoding.Utf8.GetBytes("Title");
            subField = new LiteSubField { Code = (byte)'a', Value = value };
            Assert.AreEqual("^aTitle", subField.ToString());
        }

    }
}
