// ReSharper disable CheckNamespace
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable ReplaceSliceWithRangeIndexer

using System;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

#nullable enable

namespace UnitTests.AM
{
    [TestClass]
    public class FastNumberTest
    {
        [TestMethod]
        public void FastNumber_Int32ToString_1()
        {
            Assert.AreEqual("0", FastNumber.Int32ToString(0));
            Assert.AreEqual("1", FastNumber.Int32ToString(1));
            Assert.AreEqual("12", FastNumber.Int32ToString(12));
            Assert.AreEqual("123", FastNumber.Int32ToString(123));
            Assert.AreEqual("1234", FastNumber.Int32ToString(1234));
            Assert.AreEqual("12345", FastNumber.Int32ToString(12345));
            Assert.AreEqual("123456", FastNumber.Int32ToString(123456));
            Assert.AreEqual("1234567", FastNumber.Int32ToString(1234567));
            Assert.AreEqual("12345678", FastNumber.Int32ToString(12345678));
            Assert.AreEqual("123456789", FastNumber.Int32ToString(123456789));
            Assert.AreEqual("1234567890", FastNumber.Int32ToString(1234567890));

            Assert.AreEqual("-1", FastNumber.Int32ToString(-1));
            Assert.AreEqual("-12", FastNumber.Int32ToString(-12));
            Assert.AreEqual("-123", FastNumber.Int32ToString(-123));
            Assert.AreEqual("-1234", FastNumber.Int32ToString(-1234));
            Assert.AreEqual("-12345", FastNumber.Int32ToString(-12345));
            Assert.AreEqual("-123456", FastNumber.Int32ToString(-123456));
            Assert.AreEqual("-1234567", FastNumber.Int32ToString(-1234567));
            Assert.AreEqual("-12345678", FastNumber.Int32ToString(-12345678));
            Assert.AreEqual("-123456789", FastNumber.Int32ToString(-123456789));
            Assert.AreEqual("-1234567890", FastNumber.Int32ToString(-1234567890));
        }

        [TestMethod]
        public void FastNumber_Int32ToChars_1()
        {
            var buffer = new [] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')' };
            var span = new Span<char>(buffer);
            var length = FastNumber.Int32ToChars(123456, span);
            Assert.AreEqual(6, length);
            Assert.AreEqual("123456", span.Slice(0, length).ToString());

            length = FastNumber.Int32ToChars(0, span);
            Assert.AreEqual(1, length);
            Assert.AreEqual("0", span.Slice(0, length).ToString());

            length = FastNumber.Int32ToChars(-123456, span);
            Assert.AreEqual(7, length);
            Assert.AreEqual("-123456", span.Slice(0, length).ToString());
        }

        [TestMethod]
        public unsafe void FastNumber_Int32ToChars_2()
        {
            var buffer = new [] { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')' };
            var span = new Span<char>(buffer);
            fixed (char* pointer = &buffer[0])
            {
                var length = (int) (FastNumber.Int32ToChars(123456, pointer) - pointer);
                Assert.AreEqual(6, length);
                Assert.AreEqual("123456", span.Slice(0, length).ToString());

                length = (int) (FastNumber.Int32ToChars(0, pointer) - pointer);
                Assert.AreEqual(1, length);
                Assert.AreEqual("0", span.Slice(0, length).ToString());

                length = (int ) (FastNumber.Int32ToChars(-123456, pointer) - pointer);
                Assert.AreEqual(7, length);
                Assert.AreEqual("-123456", span.Slice(0, length).ToString());
            }
        }

        [TestMethod]
        public void FastNumber_Int32ToBytes_1()
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

            length = FastNumber.Int32ToBytes(-123456, span);
            Assert.AreEqual(7, length);
            Assert.AreEqual((byte) '-', buffer[0]);
            Assert.AreEqual((byte) '1', buffer[1]);
            Assert.AreEqual((byte) '2', buffer[2]);
            Assert.AreEqual((byte) '3', buffer[3]);
            Assert.AreEqual((byte) '4', buffer[4]);
            Assert.AreEqual((byte) '5', buffer[5]);
            Assert.AreEqual((byte) '6', buffer[6]);
        }

        [TestMethod]
        public unsafe void FastNumber_Int32ToBytes_2()
        {
            var buffer = new [] { (byte)'!', (byte)'@', (byte)'#',
                (byte)'$', (byte)'%', (byte)'^', (byte)'&', (byte)'*',
                (byte)'(', (byte)')' };
            fixed (byte* pointer = &buffer[0])
            {
                var length = (int) (FastNumber.Int32ToBytes(123456, pointer) - pointer);
                Assert.AreEqual(6, length);
                Assert.AreEqual((byte) '1', buffer[0]);
                Assert.AreEqual((byte) '2', buffer[1]);
                Assert.AreEqual((byte) '3', buffer[2]);
                Assert.AreEqual((byte) '4', buffer[3]);
                Assert.AreEqual((byte) '5', buffer[4]);
                Assert.AreEqual((byte) '6', buffer[5]);

                length = (int) (FastNumber.Int32ToBytes(0, pointer) - pointer);
                Assert.AreEqual(1, length);
                Assert.AreEqual((byte) '0', buffer[0]);

                length = (int) (FastNumber.Int32ToBytes(-123456, pointer) - pointer);
                Assert.AreEqual(7, length);
                Assert.AreEqual((byte) '-', buffer[0]);
                Assert.AreEqual((byte) '1', buffer[1]);
                Assert.AreEqual((byte) '2', buffer[2]);
                Assert.AreEqual((byte) '3', buffer[3]);
                Assert.AreEqual((byte) '4', buffer[4]);
                Assert.AreEqual((byte) '5', buffer[5]);
                Assert.AreEqual((byte) '6', buffer[6]);
            }
        }

        [TestMethod]
        public void FastNumber_Int64ToString_1()
        {
            Assert.AreEqual("0", FastNumber.Int64ToString(0));
            Assert.AreEqual("1", FastNumber.Int64ToString(1));
            Assert.AreEqual("12", FastNumber.Int64ToString(12));
            Assert.AreEqual("123", FastNumber.Int64ToString(123));
            Assert.AreEqual("1234", FastNumber.Int64ToString(1234));
            Assert.AreEqual("12345", FastNumber.Int64ToString(12345));
            Assert.AreEqual("123456", FastNumber.Int64ToString(123456));
            Assert.AreEqual("1234567", FastNumber.Int64ToString(1234567));
            Assert.AreEqual("12345678", FastNumber.Int64ToString(12345678));
            Assert.AreEqual("123456789", FastNumber.Int64ToString(123456789));
            Assert.AreEqual("1234567890", FastNumber.Int64ToString(1234567890));

            Assert.AreEqual("-1", FastNumber.Int64ToString(-1));
            Assert.AreEqual("-12", FastNumber.Int64ToString(-12));
            Assert.AreEqual("-123", FastNumber.Int64ToString(-123));
            Assert.AreEqual("-1234", FastNumber.Int64ToString(-1234));
            Assert.AreEqual("-12345", FastNumber.Int64ToString(-12345));
            Assert.AreEqual("-123456", FastNumber.Int64ToString(-123456));
            Assert.AreEqual("-1234567", FastNumber.Int64ToString(-1234567));
            Assert.AreEqual("-12345678", FastNumber.Int64ToString(-12345678));
            Assert.AreEqual("-123456789", FastNumber.Int64ToString(-123456789));
            Assert.AreEqual("-1234567890", FastNumber.Int64ToString(-1234567890));
        }

        [TestMethod]
        public void FastNumber_ParseInt32_1()
        {
            Assert.AreEqual(0, FastNumber.ParseInt32(string.Empty.AsMemory()));

            Assert.AreEqual(0, FastNumber.ParseInt32("-".AsMemory()));
            Assert.AreEqual(0, FastNumber.ParseInt32("--".AsMemory()));
            Assert.AreEqual(0, FastNumber.ParseInt32("---".AsMemory()));

            Assert.AreEqual(0, FastNumber.ParseInt32("0".AsMemory()));
            Assert.AreEqual(0, FastNumber.ParseInt32("-0".AsMemory()));
            Assert.AreEqual(0, FastNumber.ParseInt32("--0".AsMemory()));

            Assert.AreEqual(1, FastNumber.ParseInt32("1".AsMemory()));
            Assert.AreEqual(-1, FastNumber.ParseInt32("-1".AsMemory()));
            Assert.AreEqual(1, FastNumber.ParseInt32("--1".AsMemory()));

            Assert.AreEqual(123, FastNumber.ParseInt32("123".AsMemory()));
            Assert.AreEqual(-123, FastNumber.ParseInt32("-123".AsMemory()));
            Assert.AreEqual(123, FastNumber.ParseInt32("--123".AsMemory()));
        }

        //=====================================================================

        private static void TestParse_2(int expected, string text)
        {
            var bytes = Encoding.ASCII.GetBytes(text);
            var memory = bytes.AsMemory();
            var actual = FastNumber.ParseInt32(memory);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void FastNumber_ParseInt32_2()
        {
            TestParse_2(0, string.Empty);

            TestParse_2(0, "-");
            TestParse_2(0, "--");
            TestParse_2(0, "---");

            TestParse_2(0, "0");
            TestParse_2(0, "-0");
            TestParse_2(0, "--0");

            TestParse_2(1, "1");
            TestParse_2(-1, "-1");
            TestParse_2(1, "--1");

            TestParse_2(123, "123");
            TestParse_2(-123, "-123");
            TestParse_2(123, "--123");
        }

        //=====================================================================

        private static unsafe void TestParse_3(int expected, string text)
        {
            var bytes = Encoding.ASCII.GetBytes(text + "\0");
            fixed (byte* ptr = bytes)
            {
                var actual = FastNumber.ParseInt32(ptr);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public unsafe void FastNumber_ParseInt32_3()
        {
            Assert.AreEqual(0, FastNumber.ParseInt32((byte*) null));

            TestParse_3(0, string.Empty);

            TestParse_3(0, "-");
            TestParse_3(0, "--");
            TestParse_3(0, "---");

            TestParse_3(0, "0");
            TestParse_3(0, "-0");
            TestParse_3(0, "--0");

            TestParse_3(1, "1");
            TestParse_3(-1, "-1");
            TestParse_3(1, "--1");

            TestParse_3(123, "123");
            TestParse_3(-123, "-123");
            TestParse_3(123, "--123");
        }

    }
}
