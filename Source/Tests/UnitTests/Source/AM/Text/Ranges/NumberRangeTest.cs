// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using AM.Text;
using AM.Text.Ranges;

#nullable enable

namespace UnitTests.AM.Text.Ranges
{
    [TestClass]
    public class NumberRangeTest
    {
        [TestMethod]
        public void NumberRange_Constructor_1()
        {
            var range = new NumberRange("10", "15");
            Assert.AreEqual("10-15", range.ToString());
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void NumberRange_Parse_1()
        {
            var range = NumberRange.Parse("10-15");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void NumberRange_Parse_2()
        {
            var range = NumberRange.Parse(" 10-15");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void NumberRange_Parse_3()
        {
            var range = NumberRange.Parse("10-15 ");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void NumberRange_Parse_4()
        {
            var range = NumberRange.Parse("10 - 15");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        public void NumberRange_Parse_5()
        {
            var range = NumberRange.Parse(" 10 - 15 ");
            Assert.AreEqual(range.Start, new NumberText("10"));
            Assert.AreEqual(range.Stop, new NumberText("15"));
            Assert.AreEqual(true, range.Contains("12"));
            Assert.AreEqual(false, range.Contains("18"));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberRange_Parse_6()
        {
            NumberRange.Parse("10-15-");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberRange_Parse_7()
        {
            NumberRange.Parse("-10-15");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberRange_Parse_8()
        {
            NumberRange.Parse("10--15");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberRange_Parse_9()
        {
            NumberRange.Parse("-");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberRange_Parse_10()
        {
            NumberRange.Parse(";");
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void NumberRange_Parse_11()
        {
            NumberRange.Parse(";-");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        [Description("Пустая строка не допустима")]
        public void NumberRange_Parse_12()
        {
            NumberRange.Parse(string.Empty);
        }

        [TestMethod]
        public void NumberRange_Equals_1()
        {
            var left = new NumberRange("10", "20");
            var right = new NumberRange("10", "20");

            Assert.IsTrue(left.Equals(right));

            right.Stop = "200";

            Assert.IsFalse(left.Equals(right));
        }

        private void _TestSerialization
            (
                string text
            )
        {
            var first = NumberRange.Parse(text);
            var bytes = first.SaveToMemory();
            var second = bytes
                .RestoreObjectFromMemory<NumberRange>();

            Assert.IsTrue(first.Equals(second));
        }

        [TestMethod]
        public void NumberRange_Serialization_1()
        {
            _TestSerialization("1");
            _TestSerialization("a1");
            _TestSerialization("a1b");
            _TestSerialization("a1b2");
        }

        [TestMethod]
        public void NumberRange_Verify_1()
        {
            var range = new NumberRange();
            Assert.IsFalse(range.Verify(false));

            range.Start = "10";
            Assert.IsFalse(range.Verify(false));

            range.Stop = "20";
            Assert.IsTrue(range.Verify(false));

            range.Stop = "2";
            Assert.IsFalse(range.Verify(false));
        }

        [TestMethod]
        public void NumberRange_Enumerate_1()
        {
            var range = NumberRange.Parse("10-15");
            var array = range.ToArray();
            Assert.AreEqual(6, array.Length);
            Assert.IsTrue(array[0] == "10");
            Assert.IsTrue(array[1] == "11");
            Assert.IsTrue(array[2] == "12");
            Assert.IsTrue(array[3] == "13");
            Assert.IsTrue(array[4] == "14");
            Assert.IsTrue(array[5] == "15");
        }

        [TestMethod]
        public void NumberRange_Enumerate_2()
        {
            var range = NumberRange.Parse("20");
            var array = range.ToArray();
            Assert.AreEqual(1, array.Length);
            Assert.IsTrue(array[0] == "20");
        }
    }
}
