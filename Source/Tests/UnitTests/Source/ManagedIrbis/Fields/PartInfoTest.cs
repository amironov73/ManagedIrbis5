// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class PartInfoTest
    {
        private PartInfo _GetPart() => new ()
            {
                SecondLevelNumber = "Ч. 2",
                SecondLevelTitle = "Отрочество"
            };

        private Field _GetField() => new Field(PartInfo.Tag)
                .Add('h', "Ч. 2")
                .Add('i', "Отрочество");

        [TestMethod]
        public void PartInfo_Construction_1()
        {
            var part = new PartInfo();
            Assert.IsNull(part.SecondLevelNumber);
            Assert.IsNull(part.SecondLevelTitle);
            Assert.IsNull(part.ThirdLevelNumber);
            Assert.IsNull(part.ThirdLevelTitle);
            Assert.IsNull(part.Role);
            Assert.IsNull(part.UnknownSubFields);
            Assert.IsNull(part.Field);
            Assert.IsNull(part.UserData);
        }

        private void _TestSerialization
            (
                PartInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<PartInfo>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.SecondLevelNumber, second.SecondLevelNumber);
            Assert.AreEqual(first.SecondLevelTitle, second.SecondLevelTitle);
            Assert.AreEqual(first.ThirdLevelNumber, second.ThirdLevelNumber);
            Assert.AreEqual(first.ThirdLevelTitle, second.ThirdLevelTitle);
            Assert.AreEqual(first.Role, second.Role);
            Assert.AreSame(first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull(second.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void PartInfo_Serialization_1()
        {
            var part = new PartInfo();
            _TestSerialization(part);

            part.Field = new Field();
            part.UserData = "User data";
            _TestSerialization(part);

            part = _GetPart();
            _TestSerialization(part);
        }

        [TestMethod]
        public void PartInfo_ParseField_1()
        {
            var field = _GetField();
            var part = PartInfo.ParseField(field);
            Assert.AreSame(field, part.Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('h'), part.SecondLevelNumber);
            Assert.AreEqual(field.GetFirstSubFieldValue('i'), part.SecondLevelTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('k'), part.ThirdLevelNumber);
            Assert.AreEqual(field.GetFirstSubFieldValue('l'), part.ThirdLevelTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('u'), part.Role);
            Assert.IsNotNull(part.UnknownSubFields);
            Assert.AreEqual(0, part.UnknownSubFields.Length);
            Assert.IsNull(part.UserData);
        }

        [TestMethod]
        public void PartInfo_ParseRecord_1()
        {
            var record = new Record();
            var field = _GetField();
            record.Fields.Add(field);
            var part = PartInfo.ParseRecord(record);
            Assert.AreEqual(1, part.Length);
            Assert.AreSame(field, part[0].Field);
            Assert.AreEqual(field.GetFirstSubFieldValue('h'), part[0].SecondLevelNumber);
            Assert.AreEqual(field.GetFirstSubFieldValue('i'), part[0].SecondLevelTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('k'), part[0].ThirdLevelNumber);
            Assert.AreEqual(field.GetFirstSubFieldValue('l'), part[0].ThirdLevelTitle);
            Assert.AreEqual(field.GetFirstSubFieldValue('u'), part[0].Role);
            Assert.IsNotNull(part[0].UnknownSubFields);
            Assert.AreEqual(0, part[0].UnknownSubFields!.Length);
            Assert.IsNull(part[0].UserData);
        }

        [TestMethod]
        public void PartInfo_ToField_1()
        {
            var part = _GetPart();
            var field = part.ToField();
            Assert.AreEqual(PartInfo.Tag, field.Tag);
            Assert.AreEqual(2, field.Subfields.Count);
            Assert.AreEqual(part.SecondLevelNumber, field.GetFirstSubFieldValue('h'));
            Assert.AreEqual(part.SecondLevelTitle, field.GetFirstSubFieldValue('i'));
            Assert.AreEqual(part.ThirdLevelNumber, field.GetFirstSubFieldValue('k'));
            Assert.AreEqual(part.ThirdLevelTitle, field.GetFirstSubFieldValue('l'));
            Assert.AreEqual(part.Role, field.GetFirstSubFieldValue('u'));
        }

        [TestMethod]
        public void PartInfo_ApplyToField_1()
        {
            var field = new Field(PartInfo.Tag)
                .Add('h', "???")
                .Add('i', "???");
            var part = _GetPart();
            part.ApplyToField(field);
            Assert.AreEqual(PartInfo.Tag, field.Tag);
            Assert.AreEqual(2, field.Subfields.Count);
            Assert.AreEqual(part.SecondLevelNumber, field.GetFirstSubFieldValue('h'));
            Assert.AreEqual(part.SecondLevelTitle, field.GetFirstSubFieldValue('i'));
            Assert.AreEqual(part.ThirdLevelNumber, field.GetFirstSubFieldValue('k'));
            Assert.AreEqual(part.ThirdLevelTitle, field.GetFirstSubFieldValue('l'));
            Assert.AreEqual(part.Role, field.GetFirstSubFieldValue('u'));
        }

        [TestMethod]
        public void PartInfo_Verify_1()
        {
            var part = new PartInfo();
            Assert.IsFalse(part.Verify(false));

            part = _GetPart();
            Assert.IsTrue(part.Verify(false));
        }

        [TestMethod]
        public void PartInfo_ToXml_1()
        {
            var part = new PartInfo();
            Assert.AreEqual("<part />", XmlUtility.SerializeShort(part));

            part = _GetPart();
            Assert.AreEqual("<part><secondLevelNumber>Ч. 2</secondLevelNumber><secondLevelTitle>Отрочество</secondLevelTitle></part>", XmlUtility.SerializeShort(part));
        }

        [TestMethod]
        public void PartInfo_ToJson_1()
        {
            var part = new PartInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(part));

            part = _GetPart();
            Assert.AreEqual("{\"secondLevelNumber\":\"\\u0427. 2\",\"secondLevelTitle\":\"\\u041E\\u0442\\u0440\\u043E\\u0447\\u0435\\u0441\\u0442\\u0432\\u043E\"}", JsonUtility.SerializeShort(part));
        }

        [Ignore]
        [TestMethod]
        public void PartInfo_ToString_1()
        {
            var part = new PartInfo();
            Assert.AreEqual("(empty)", part.ToString());

            part = _GetPart();
            Assert.AreEqual("Ч. 2 -- Отрочество", part.ToString());
        }
    }
}
