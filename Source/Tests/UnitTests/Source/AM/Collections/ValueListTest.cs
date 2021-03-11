using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#nullable enable

// ReSharper disable CheckNamespace
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo

namespace UnitTests.AM.Collections
{
    [TestClass]
    public unsafe class ValueListTest
    {
        private static T[] Get<T>(ReadOnlySpan<char> span)
        {
            T[] result = new T[span.Length];
            for (var i = 0; i < span.Length; i++)
            {
                result[i] = (T) Convert.ChangeType(span[i], typeof(T));
            }

            return result;
        }

        private static bool Same<T>
            (
                Span<T> one,
                Span<T> two
            )
            where T: IEquatable<T>
        {
            if (one.Length != two.Length)
            {
                return false;
            }

            for (var i = 0; i < one.Length; i++)
            {
                if (!one[i].Equals(two[i]))
                {
                    return false;
                }

            }

            return true;
        }

        [TestMethod]
        public void ValueList_Constructor_1()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            Assert.AreEqual(0, list.Length);
            Assert.AreEqual(bufferLength, list.Capacity);
            Assert.AreEqual(0, list.ToArray().Length);
        }

        [TestMethod]
        public void ValueList_Append_1()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            list.Append('H');
            Assert.AreEqual(1, list.Length);
            list.Append('e');
            Assert.AreEqual(2, list.Length);
            list.Append('l');
            Assert.AreEqual(3, list.Length);
            list.Append('l');
            Assert.AreEqual(4, list.Length);
            list.Append('o');
            Assert.AreEqual(5, list.Length);
            Assert.IsTrue(Same<int>
                (
                    Get<int> ("Hello"),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_Append_2()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            list.Append(Get<int>("Hello, "));
            Assert.AreEqual(7, list.Length);
            list.Append(Get<int>("world!"));
            Assert.AreEqual(13, list.Length);
            Assert.IsTrue(Same<int>
                (
                    Get<int> ("Hello, world!"),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_Append_3()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            list.Append(Get<int>("Hello, "), Get<int>("world!"));
            Assert.AreEqual(13, list.Length);
            Assert.IsTrue(Same<int>
                (
                    Get<int> ("Hello, world!"),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_Append_4()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);
            var longString = new string('x', 80);

            list.Append(Get<int>(longString));
            Assert.AreEqual(longString.Length, list.Length);
            Assert.IsTrue(Same<int>
                (
                    Get<int> (longString),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_Append_5()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);
            var longString = new string('x', 80);

            list.Append(Get<int>(longString), Get<int>(longString));
            Assert.AreEqual(longString.Length * 2, list.Length);
            Assert.IsTrue(Same<int>
                (
                    Get<int> (longString + longString),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_Append_6()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);
            var longString = new string('x', 80);

            list.Append(Get<int>(longString));
            list.Append(Get<int>(longString));
            Assert.AreEqual(longString.Length * 2, list.Length);
            Assert.IsTrue(Same<int>
                (
                    Get<int> (longString + longString),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_Append_7()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);
            var longString = new string('x', 80);

            list.Append(Get<int>(longString), Get<int>(longString),
                Get<int>(longString));
            Assert.AreEqual(longString.Length * 3, list.Length);
            Assert.IsTrue(Same<int>
                (
                    Get<int> (longString + longString + longString),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_Append_8()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            for (var i = 0; i < 100; i++)
            {
                list.Append('x');
            }

            Assert.AreEqual(100, list.Length);
            Assert.IsTrue(Same<int>
                (
                    Get<int> (new string('x', 100)),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_Length_1()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            buffer.Fill('x');
            list.Length = 5;
            Assert.AreEqual(5, list.Length);
            Assert.IsTrue(Same<int>
                (
                    Get<int> (new string('x', 5)),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_RawBuffer_1()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            buffer.Fill('x');
            Assert.AreEqual('x', list.RawBuffer[0]);
        }

        [TestMethod]
        public void ValueList_Indexer_1()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            buffer.Fill('x');
            Assert.AreEqual('x', list[1]);
            list[1] = 'X';
            Assert.AreEqual('X', list[1]);
            list.Length = 2;
            Assert.IsTrue(Same<int>
                (
                    Get<int> ("xX"),
                    list.ToArray()
                ));
        }

        [TestMethod]
        public void ValueList_EnsureCapacity_1()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            list.EnsureCapacity(100);
            Assert.IsTrue(100 <= list.Capacity);
            list.Dispose();
        }

        [TestMethod]
        public void ValueList_AsSpan_1()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            list.Append(Get<int>("Hello, world"));
            var span = list.AsSpan(7);
            Assert.IsTrue(Same<int>
                (
                    Get<int> ("world"),
                    span.ToArray()
                ));
            list.Dispose();
        }

        [TestMethod]
        public void ValueList_AsSpan_2()
        {
            const int bufferLength = 16;
            Span<int> buffer = stackalloc int[bufferLength];
            var list = new ValueList<int>(buffer);

            list.Append(Get<int>("Hello, world"));
            var span = list.AsSpan(0, 5);
            Assert.IsTrue(Same<int>
                (
                    Get<int> ("Hello"),
                    span.ToArray()
                ));
            list.Dispose();
        }
    }
}
