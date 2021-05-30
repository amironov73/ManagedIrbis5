// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class CodesInfoTest
        : Common.CommonUnitTest
    {
        private CodesInfo _GetCodes()
        {
            return new ()
            {
                DocumentType = "a",
                DocumentKind = "05",
                DocumentCharacter1 = "454",
                DocumentCharacter2 = "19",
                PurposeCode1 = "m",
                PurposeCode2 = "e"
            };
        }
        private Field _GetField()
        {
            return new (CodesInfo.Tag, "^Ta^B05^C454^219^Xm^Ye");
        }

        private void _Compare
            (
                CodesInfo first,
                CodesInfo second
            )
        {
            Assert.AreEqual(first.DocumentType, second.DocumentType);
            Assert.AreEqual(first.DocumentKind, second.DocumentKind);
            Assert.AreEqual(first.DocumentCharacter1, second.DocumentCharacter1);
            Assert.AreEqual(first.DocumentCharacter2, second.DocumentCharacter2);
            Assert.AreEqual(first.DocumentCharacter3, second.DocumentCharacter3);
            Assert.AreEqual(first.DocumentCharacter4, second.DocumentCharacter4);
            Assert.AreEqual(first.DocumentCharacter5, second.DocumentCharacter5);
            Assert.AreEqual(first.DocumentCharacter6, second.DocumentCharacter6);
            Assert.AreEqual(first.PurposeCode1, second.PurposeCode1);
            Assert.AreEqual(first.PurposeCode2, second.PurposeCode2);
            Assert.AreEqual(first.PurposeCode3, second.PurposeCode3);
            Assert.AreEqual(first.AgeRestrictions, second.AgeRestrictions);
        }

        [TestMethod]
        public void CodesInfo_Constructor_1()
        {
            var codes = new CodesInfo();
            Assert.IsNull(codes.DocumentType);
            Assert.IsNull(codes.DocumentKind);
            Assert.IsNull(codes.DocumentCharacter1);
            Assert.IsNull(codes.DocumentCharacter2);
            Assert.IsNull(codes.DocumentCharacter3);
            Assert.IsNull(codes.DocumentCharacter4);
            Assert.IsNull(codes.DocumentCharacter5);
            Assert.IsNull(codes.DocumentCharacter6);
            Assert.IsNull(codes.PurposeCode1);
            Assert.IsNull(codes.PurposeCode2);
            Assert.IsNull(codes.PurposeCode3);
            Assert.IsNull(codes.AgeRestrictions);
        }

        [TestMethod]
        public void CodesInfo_Parse_1()
        {
            var field = _GetField();
            var actual = CodesInfo.Parse(field);
            var expected = _GetCodes();
            _Compare(expected, actual);
        }

        [TestMethod]
        public void CodesInfo_ToField_1()
        {
            var codes = _GetCodes();
            var actual = codes.ToField();
            var expected = _GetField();
            CompareFields(expected, actual);
        }

        [TestMethod]
        public void CodesInfo_ApplyToField_1()
        {
            var actual = new Field()
                .Add('t', "o")
                .Add('b', "03")
                .Add('c', "111")
                .Add('2', "91")
                .Add('x', "l")
                .Add('y', "a");
            var codes = _GetCodes();
            codes.ApplyToField(actual);
            var expected = _GetField();
            CompareFields(expected, actual);
        }

        private void _TestSerialization
            (
                CodesInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<CodesInfo>();
            Assert.IsNotNull(second);
            _Compare(first, second!);
            Assert.IsNull(second!.Field);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void CodesInfo_Serialization_1()
        {
            var codes = new CodesInfo();
            _TestSerialization(codes);

            codes = _GetCodes();
            codes.Field = new Field();
            codes.UserData = "User data";
            _TestSerialization(codes);
        }

        [TestMethod]
        public void CodesInfo_Verify_1()
        {
            var codes = new CodesInfo();
            Assert.IsFalse(codes.Verify(false));

            codes = _GetCodes();
            Assert.IsTrue(codes.Verify(false));
        }

        [TestMethod]
        public void CodesInfo_ToXml_1()
        {
            var codes = new CodesInfo();
            Assert.AreEqual("<codes />", XmlUtility.SerializeShort(codes));

            codes = _GetCodes();
            Assert.AreEqual("<codes type=\"a\" kind=\"05\" character1=\"454\" character2=\"19\" purpose1=\"m\" purpose2=\"e\" />", XmlUtility.SerializeShort(codes));
        }

        [TestMethod]
        public void CodesInfo_ToJson_1()
        {
            var codes = new CodesInfo();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(codes));

            codes = _GetCodes();
            Assert.AreEqual("{\"type\":\"a\",\"kind\":\"05\",\"character1\":\"454\",\"character2\":\"19\",\"purpose1\":\"m\",\"purpose2\":\"e\"}", JsonUtility.SerializeShort(codes));
        }

        [TestMethod]
        public void CodesInfo_ToString_1()
        {
            var codes = new CodesInfo();
            Assert.AreEqual("DocumentType: (null), DocumentKind: (null), DocumentCharacter1: (null)", codes.ToString());

            codes = _GetCodes();
            Assert.AreEqual("DocumentType: a, DocumentKind: 05, DocumentCharacter1: 454", codes.ToString());
        }
    }
}
