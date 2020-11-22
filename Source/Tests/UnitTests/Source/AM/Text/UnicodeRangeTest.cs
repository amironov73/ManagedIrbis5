using System;

//using AM.Json;
using AM.Runtime;
using AM.Text;
//using AM.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

// ReSharper disable CheckNamespace
// ReSharper disable ObjectCreationAsStatement

#nullable enable

namespace UnitTests.AM.Text
{
    [TestClass]
    public class UnicodeRangeTest
    {
        private UnicodeRange _GetRange()
        {
            return new UnicodeRange("Lower Latin", 'a', 'z');
        }

        [TestMethod]
        public void UnicodeRange_Construction_1()
        {
            var range = new UnicodeRange();
            Assert.IsNull(range.Name);
            Assert.AreEqual('\0', range.From);
            Assert.AreEqual('\0', range.To);
        }

        [TestMethod]
        public void UnicodeRange_Construction_2()
        {
            var name = "Lower Latin";
            var range = new UnicodeRange(name, 'a', 'z');
            Assert.AreEqual(name, range.Name);
            Assert.AreEqual('a', range.From);
            Assert.AreEqual('z', range.To);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnicodeRange_Construction_3()
        {
            var name = "Lower Latin";
            new UnicodeRange(name, 'z', 'a');
        }

        [TestMethod]
        public void UnicodeRange_Properties_1()
        {
            Assert.IsNotNull(UnicodeRange.BasicLatin);
            Assert.IsNotNull(UnicodeRange.ControlCharacters);
            Assert.IsNotNull(UnicodeRange.Cyrillic);
            Assert.IsNotNull(UnicodeRange.CyrillicSupplement);
            Assert.IsNotNull(UnicodeRange.Latin1Supplement);
            Assert.IsNotNull(UnicodeRange.LatinExtended);
            Assert.IsNotNull(UnicodeRange.Russian);
        }

        private void _TestSerialization
            (
                UnicodeRange first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<UnicodeRange>();
            Assert.AreEqual(first.Name, second?.Name);
            Assert.AreEqual(first.From, second?.From);
            Assert.AreEqual(first.To, second?.To);
        }

        [TestMethod]
        public void UnicodeRange_Serialization_1()
        {
            var range = new UnicodeRange();
            _TestSerialization(range);

            range = _GetRange();
            _TestSerialization(range);
        }

        /*
        [TestMethod]
        public void UnicodeRange_ToJson_1()
        {
            UnicodeRange range = new UnicodeRange();
            Assert.AreEqual("{\'from\':\"\\u0000\",\'to\':\"\\u0000\"}", JsonUtility.SerializeShort(range));

            range = _GetRange();
            Assert.AreEqual("{\'name\':\'Lower Latin\',\'from\':\"a\",\'to\':\"z\"}", JsonUtility.SerializeShort(range));
        }

        [TestMethod]
        public void UnicodeRange_ToXml_1()
        {
            UnicodeRange range = new UnicodeRange();
            Assert.AreEqual("<range from=\"0\" to=\"0\" />", XmlUtility.SerializeShort(range));

            range = _GetRange();
            Assert.AreEqual("<range name=\"Lower Latin\" from=\"97\" to=\"122\" />", XmlUtility.SerializeShort(range));
        }
        */

        [TestMethod]
        public void UnicodeRange_Verify_1()
        {
            var range = new UnicodeRange();
            Assert.IsFalse(range.Verify(false));

            range = _GetRange();
            Assert.IsTrue(range.Verify(false));
        }

        [TestMethod]
        public void UnicodeRange_ToString_1()
        {
            var range = _GetRange();
            Assert.AreEqual("Lower Latin: 97-122", range.ToString());
        }
    }
}
