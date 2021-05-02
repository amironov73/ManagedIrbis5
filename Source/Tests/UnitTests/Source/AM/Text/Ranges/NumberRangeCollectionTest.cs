﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Ranges;

#nullable enable

namespace UnitTests.AM.Text.Ranges
{
    [TestClass]
    public class NumberRangeCollectionTest
    {
        [TestMethod]
        public void NumberRangeCollection_Parse_1()
        {
            var collection = NumberRangeCollection.Parse("10-15");
            Assert.AreEqual(1, collection.Count);

            collection = NumberRangeCollection.Parse("10;15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10;15-20;30");
            Assert.AreEqual(3, collection.Count);

            collection = NumberRangeCollection.Parse("10; ;15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10   15-20,;30");
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        public void NumberRangeCollection_Parse_2()
        {
            var collection = NumberRangeCollection.Parse("10 - 15");
            Assert.AreEqual(1, collection.Count);

            collection = NumberRangeCollection.Parse("10 ;15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10;15 - 20;30");
            Assert.AreEqual(3, collection.Count);

            collection = NumberRangeCollection.Parse("10; ;15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10   15 - 20,;30");
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        public void NumberRangeCollection_Parse_3()
        {
            var collection = NumberRangeCollection.Parse("10 15");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10 - 15 20");
            Assert.AreEqual(2, collection.Count);

            collection = NumberRangeCollection.Parse("10 15 - 20 30");
            Assert.AreEqual(3, collection.Count);

            collection = NumberRangeCollection.Parse("10 15 30");
            Assert.AreEqual(3, collection.Count);

            collection = NumberRangeCollection.Parse("10   15 - 20 30-40");
            Assert.AreEqual(3, collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberRangeCollection_Parse_4()
        {
            NumberRangeCollection.Parse("-10");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberRangeCollection_Parse_5()
        {
            NumberRangeCollection.Parse("10-15;-");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberRangeCollection_Parse_6()
        {
            NumberRangeCollection.Parse(";;-,");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        [Description("Пустая строка не допустима")]
        public void NumberRangeCollection_Parse_7()
        {
            NumberRangeCollection.Parse(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        [Description("null недопустим так же, как и пустая строка")]
        public void NumberRangeCollection_Parse_8()
        {
            NumberRangeCollection.Parse(null!);
        }

        [TestMethod]
        public void NumberRangeCollection_Enumerate_1()
        {
            var collection = NumberRangeCollection.Parse("1-10");
            var array = collection.ToArray();
            Assert.AreEqual(10, array.Length);
        }

        [TestMethod]
        public void NumberRangeCollection_Enumerate_2()
        {
            var collection = NumberRangeCollection.Parse("1-10,15-20");
            var array = collection.ToArray();
            Assert.AreEqual(16, array.Length);
            Assert.IsTrue(array[0] == "1");
            Assert.IsTrue(array[1] == "2");
            Assert.IsTrue(array[2] == "3");
            Assert.IsTrue(array[3] == "4");
            Assert.IsTrue(array[4] == "5");
            Assert.IsTrue(array[5] == "6");
            Assert.IsTrue(array[6] == "7");
            Assert.IsTrue(array[7] == "8");
            Assert.IsTrue(array[8] == "9");
            Assert.IsTrue(array[9] == "10");
            Assert.IsTrue(array[10] == "15");
            Assert.IsTrue(array[11] == "16");
            Assert.IsTrue(array[12] == "17");
            Assert.IsTrue(array[13] == "18");
            Assert.IsTrue(array[14] == "19");
            Assert.IsTrue(array[15] == "20");
        }

        [TestMethod]
        public void NumberRangeCollection_Enumerate_3()
        {
            var collection = NumberRangeCollection.Parse("1-10,15-20,22");
            var array = collection.ToArray();
            Assert.AreEqual(17, array.Length);
            Assert.IsTrue(array[0] == "1");
            Assert.IsTrue(array[1] == "2");
            Assert.IsTrue(array[2] == "3");
            Assert.IsTrue(array[3] == "4");
            Assert.IsTrue(array[4] == "5");
            Assert.IsTrue(array[5] == "6");
            Assert.IsTrue(array[6] == "7");
            Assert.IsTrue(array[7] == "8");
            Assert.IsTrue(array[8] == "9");
            Assert.IsTrue(array[9] == "10");
            Assert.IsTrue(array[10] == "15");
            Assert.IsTrue(array[11] == "16");
            Assert.IsTrue(array[12] == "17");
            Assert.IsTrue(array[13] == "18");
            Assert.IsTrue(array[14] == "19");
            Assert.IsTrue(array[15] == "20");
            Assert.IsTrue(array[16] == "22");
        }

        [TestMethod]
        public void NumberRangeCollection_Enumerate_4()
        {
            var collection = NumberRangeCollection.Parse("1-3,5,3");
            var array = collection.ToArray();
            Assert.AreEqual(5, array.Length);
            Assert.IsTrue(array[0] == "1");
            Assert.IsTrue(array[1] == "2");
            Assert.IsTrue(array[2] == "3");
            Assert.IsTrue(array[3] == "5");
            Assert.IsTrue(array[4] == "3");
        }
    }
}
