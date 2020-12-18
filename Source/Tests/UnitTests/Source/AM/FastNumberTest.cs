using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

// ReSharper disable CheckNamespace
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach

#nullable enable

namespace UnitTests.AM
{
    [TestClass]
    public class FastNumberTest
    {
        [TestMethod]
        public void Int32ToChars_1()
        {
            var buffer = new [] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')' };
            var span = new Span<char>(buffer);
            var length = FastNumber.Int32ToChars(123456, span);
            Assert.AreEqual(6, length);
            Assert.AreEqual("123456", span.Slice(0, length).ToString());

            length = FastNumber.Int32ToChars(0, span);
            Assert.AreEqual(1, length);
            Assert.AreEqual("0", span.Slice(0, length).ToString());
        }

        [TestMethod]
        public void Int32ToBytes_1()
        {
            var buffer = new [] { (byte)'!', (byte)'@', (byte)'#',
                (byte)'$', (byte)'%', (byte)'^', (byte)'&', (byte)'*',
                (byte)'(', (byte)')' };
            var span = new Span<byte>(buffer);
            var length = FastNumber.Int32ToBytes(123456, span);
            Assert.AreEqual(6, length);
            Assert.AreEqual((byte) '1', buffer[0]);
            Assert.AreEqual((byte) '2', buffer[1]);
            Assert.AreEqual((byte) '3', buffer[2]);
            Assert.AreEqual((byte) '4', buffer[3]);
            Assert.AreEqual((byte) '5', buffer[4]);
            Assert.AreEqual((byte) '6', buffer[5]);

            length = FastNumber.Int32ToBytes(0, span);
            Assert.AreEqual(1, length);
            Assert.AreEqual((byte)'0', buffer[0]);
        }
    }
}
