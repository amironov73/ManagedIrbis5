// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class GlobalCounterTest
    {
        private Record _GetRecord()
        {
            var result = new Record();
            result.Fields.Add(new Field(1, "01"));
            result.Fields.Add(new Field(2, "СЧ000011/1"));
            result.Fields.Add(new Field(3, "СЧ******/1"));

            return result;
        }

        private GlobalCounter _GetCounter()
        {
            return new()
            {
                Index = "01",
                Value = "СЧ000011/1",
                Template = "СЧ******/1"
            };
        }

        private void _Compare
            (
                GlobalCounter first,
                GlobalCounter second
            )
        {
            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Value, second.Value);
            Assert.AreEqual(first.Template, second.Template);
        }

        [TestMethod]
        public void GlobalCounter_Construction_1()
        {
            var counter = new GlobalCounter();
            Assert.IsNull(counter.Index);
            Assert.IsNull(counter.Value);
            Assert.IsNull(counter.Template);
            Assert.IsNull(counter.Record);
            Assert.IsNull(counter.UserData);
        }

        [TestMethod]
        public void GlobalCounter_Parse_1()
        {
            var record = _GetRecord();
            var counter = GlobalCounter.Parse(record);
            Assert.AreSame(record, counter.Record);
            Assert.AreEqual(record.FM(1).ToString(), counter.Index);
            Assert.AreEqual(record.FM(2).ToString(), counter.Value);
            Assert.AreEqual(record.FM(3).ToString(), counter.Template);
            Assert.IsNull(counter.UserData);
        }

        [TestMethod]
        public void GlobalCounter_ToRecord_1()
        {
            var counter = _GetCounter();
            var record = counter.ToRecord();
            Assert.AreEqual(counter.Index, record.FM(1).ToString());
            Assert.AreEqual(counter.Value, record.FM(2).ToString());
            Assert.AreEqual(counter.Template, record.FM(3).ToString());
        }

        [TestMethod]
        public void GlobalCounter_ApplyTo_1()
        {
            var counter = _GetCounter();
            var record = new Record();
            record.Fields.Add(new Field(1, "Hello"));
            record.Fields.Add(new Field(2, "333"));
            record.Fields.Add(new Field(3, "???"));
            counter.ApplyTo(record);
            Assert.AreEqual(counter.Index, record.FM(1).ToString());
            Assert.AreEqual(counter.Value, record.FM(2).ToString());
            Assert.AreEqual(counter.Template, record.FM(3).ToString());
        }

        [TestMethod]
        public void GlobalCounter_NumericValue_1()
        {
            var counter = new GlobalCounter();
            Assert.AreEqual(0, counter.NumericValue);

            counter.NumericValue = 123;
            Assert.AreEqual("123", counter.Value);
        }

        [TestMethod]
        public void GlobalCounter_NumericValue_2()
        {
            var counter = _GetCounter();
            Assert.AreEqual(11, counter.NumericValue);

            counter.NumericValue = 123;
            Assert.AreEqual("СЧ000123/1", counter.Value);
        }

        [TestMethod]
        public void GlobalCounter_Increment_1()
        {
            var counter = new GlobalCounter();
            Assert.AreSame(counter, counter.Increment(1));
            Assert.AreEqual("1", counter.Value);
            Assert.AreEqual(1, counter.NumericValue);

            counter = _GetCounter();
            Assert.AreSame(counter, counter.Increment(1));
            Assert.AreEqual("СЧ000012/1", counter.Value);
            Assert.AreEqual(12, counter.NumericValue);
        }

        [TestMethod]
        public void GlobalCounter_ToXml_1()
        {
            var counter = new GlobalCounter();
            Assert.AreEqual("<counter />", XmlUtility.SerializeShort(counter));

            counter = _GetCounter();
            Assert.AreEqual("<counter index=\"01\" value=\"СЧ000011/1\" template=\"СЧ******/1\" />", XmlUtility.SerializeShort(counter));
        }

        [TestMethod]
        public void GlobalCounter_ToJson_1()
        {
            var counter = new GlobalCounter();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(counter));

            counter = _GetCounter();
            Assert.AreEqual("{\"index\":\"01\",\"value\":\"\\u0421\\u0427000011/1\",\"template\":\"\\u0421\\u0427******/1\"}", JsonUtility.SerializeShort(counter));
        }

        private void _TestSerialization
            (
                GlobalCounter first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<GlobalCounter>();
            Assert.IsNotNull(second);
            _Compare(first, second!);
            Assert.IsNull(second!.Record);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void GlobalCounter_Serialization_1()
        {
            var counter = new GlobalCounter();
            _TestSerialization(counter);

            counter = _GetCounter();
            counter.UserData = "User data";
            _TestSerialization(counter);
        }

        [TestMethod]
        public void GlobalCounter_Verify_1()
        {
            var counter = new GlobalCounter();
            Assert.IsFalse(counter.Verify(false));

            counter = _GetCounter();
            Assert.IsTrue(counter.Verify(false));
        }

        [TestMethod]
        public void GlobalCounter_ToString_1()
        {
            var counter = new GlobalCounter();
            Assert.AreEqual("(null):(null)", counter.ToString());

            counter = _GetCounter();
            Assert.AreEqual("01:СЧ000011/1", counter.ToString());
        }
    }
}
