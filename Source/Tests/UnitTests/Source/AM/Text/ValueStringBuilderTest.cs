using System;


using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#nullable enable

// ReSharper disable CheckNamespace
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo

namespace UnitTests.AM.Text
{
    [TestClass]
    public unsafe class ValueStringBuilderTest
    {
        [TestMethod]
        public void ValueStringBuilder_Constructor_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            Assert.AreEqual(0, builder.Length);
            Assert.AreEqual(bufferLength, builder.Capacity);
            Assert.AreEqual(string.Empty, builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_Append_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            builder.Append('H');
            Assert.AreEqual(1, builder.Length);
            builder.Append('e');
            Assert.AreEqual(2, builder.Length);
            builder.Append('l');
            Assert.AreEqual(3, builder.Length);
            builder.Append('l');
            Assert.AreEqual(4, builder.Length);
            builder.Append('o');
            Assert.AreEqual(5, builder.Length);
            Assert.AreEqual("Hello", builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_Append_2()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            builder.Append("Hello, ");
            Assert.AreEqual(7, builder.Length);
            builder.Append("world!");
            Assert.AreEqual(13, builder.Length);
            Assert.AreEqual("Hello, world!", builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_Append_3()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            builder.Append("Hello, ", "world!");
            Assert.AreEqual(13, builder.Length);
            Assert.AreEqual("Hello, world!", builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_Append_4()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);
            var longString = new string('x', 80);

            builder.Append(longString);
            Assert.AreEqual(longString.Length, builder.Length);
            Assert.AreEqual(longString, builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_Append_5()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);
            var longString = new string('x', 80);

            builder.Append(longString, longString);
            Assert.AreEqual(longString.Length * 2, builder.Length);
            Assert.AreEqual(longString + longString, builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_Append_6()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);
            var longString = new string('x', 80);

            builder.Append(longString);
            builder.Append(longString);
            Assert.AreEqual(longString.Length * 2, builder.Length);
            Assert.AreEqual(longString + longString, builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_Append_7()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);
            var longString = new string('x', 80);

            builder.Append(longString, longString, longString);
            Assert.AreEqual(longString.Length * 3, builder.Length);
            Assert.AreEqual(longString + longString + longString, builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_Append_8()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            for (var i = 0; i < 100; i++)
            {
                builder.Append('x');
            }

            Assert.AreEqual(100, builder.Length);
            Assert.AreEqual(new string('x', 100), builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_Length_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            buffer.Fill('x');
            builder.Length = 5;
            Assert.AreEqual(5, builder.Length);
            Assert.AreEqual("xxxxx", builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_RawCharacters_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            buffer.Fill('x');
            Assert.AreEqual('x', builder.RawCharacters[0]);
        }

        [TestMethod]
        public void ValueStringBuilder_Indexer_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            buffer.Fill('x');
            Assert.AreEqual('x', builder[1]);
            builder[1] = 'X';
            Assert.AreEqual('X', builder[1]);
            builder.Length = 2;
            Assert.AreEqual("xX", builder.ToString());
        }

        [TestMethod]
        public void ValueStringBuilder_EnsureCapacity_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            builder.EnsureCapacity(100);
            Assert.IsTrue(100 <= builder.Capacity);
            builder.Dispose();
        }

        [TestMethod]
        public void ValueStringBuilder_AsSpan_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            builder.Append("Hello, world");
            var span = builder.AsSpan(7);
            Assert.AreEqual("world", span.ToString());
            builder.Dispose();
        }

        [TestMethod]
        public void ValueStringBuilder_AsSpan_2()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder(buffer);

            builder.Append("Hello, world");
            var span = builder.AsSpan(0, 5);
            Assert.AreEqual("Hello", span.ToString());
            builder.Dispose();
        }

    }
}
