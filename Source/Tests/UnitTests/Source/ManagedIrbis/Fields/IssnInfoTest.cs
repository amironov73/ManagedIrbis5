// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class IssnInfoTest
    {
        private IssnInfo _GetIssn()
        {
            return new ()
            {
                Issn = "0378-5955"
            };
        }

        private Field _GetField()
        {
            return new Field (IssnInfo.Tag)
                .Add ('a', "0378-5955");
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void IssnInfo_Construction_1()
        {
            var isbn = new IssnInfo();
            Assert.IsNull (isbn.Issn);
            Assert.IsNull (isbn.UnknownSubFields);
            Assert.IsNull (isbn.Field);
            Assert.IsNull (isbn.UserData);
        }

        private void _TestSerialization
            (
                IssnInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<IssnInfo>();
            Assert.IsNotNull (second);
            Assert.AreEqual (first.Issn, second.Issn);
            Assert.AreSame (first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void IssnInfo_Serialization_1()
        {
            var issn = new IssnInfo();
            _TestSerialization (issn);

            issn.Field = new Field();
            issn.UserData = "User data";
            _TestSerialization (issn);

            issn = _GetIssn();
            _TestSerialization (issn);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void IsbnInfo_ParseField_1()
        {
            var field = _GetField();
            var issn = IssnInfo.ParseField (field);
            Assert.AreSame (field, issn.Field);
            Assert.AreEqual (field.GetFirstSubFieldValue ('a'), issn.Issn);
            Assert.IsNotNull (issn.UnknownSubFields);
            Assert.AreEqual (0, issn.UnknownSubFields!.Length);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void IssnInfo_ParseRecord_1()
        {
            var record = new Record();
            var field = _GetField();
            record.Fields.Add (field);
            var issn = IssnInfo.ParseRecord (record);
            Assert.AreEqual (1, issn.Length);
            Assert.AreSame (field, issn[0].Field);
            Assert.AreEqual (field.GetFirstSubFieldValue ('a'), issn[0].Issn);
            Assert.IsNotNull (issn[0].UnknownSubFields);
            Assert.AreEqual (0, issn[0].UnknownSubFields!.Length);
        }

        [TestMethod]
        [Description ("Сощдание поля записи по информации о ISSN")]
        public void IssnInfo_ToField_1()
        {
            var issn = _GetIssn();
            var field = issn.ToField();
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual (issn.Issn, field.GetFirstSubFieldValue ('a'));
        }

        [TestMethod]
        [Description ("Применение информации к полю записи")]
        public void IsbnInfo_ApplyToField_1()
        {
            var field = new Field (IssnInfo.Tag)
                .Add ('a', "???");
            var issn = _GetIssn();
            issn.ApplyToField (field);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual (issn.Issn, field.GetFirstSubFieldValue ('a'));
        }

        [TestMethod]
        [Description ("Верификация")]
        public void Issn_Verify_1()
        {
            var issn = new IssnInfo();
            Assert.IsFalse (issn.Verify (false));

            issn = _GetIssn();
            Assert.IsTrue (issn.Verify (false));
        }

        [TestMethod]
        [Description ("Получение XML-представления")]
        public void IssnInfo_ToXml_1()
        {
            var issn = new IssnInfo();
            Assert.AreEqual ("<issn />", XmlUtility.SerializeShort (issn));

            issn = _GetIssn();
            Assert.AreEqual ("<issn>0378-5955</issn>", XmlUtility.SerializeShort (issn));
        }

        [TestMethod]
        [Description ("Получение JSON-представления")]
        public void IsbnInfo_ToJson_1()
        {
            var issn = new IssnInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (issn));

            issn = _GetIssn();
            Assert.AreEqual ("{\"issn\":\"0378-5955\"}", JsonUtility.SerializeShort (issn));
        }

        [TestMethod]
        [Description ("Получение текстового представления")]
        public void IssnInfo_ToString_1()
        {
            var issn = new IssnInfo();
            Assert.AreEqual ("(null)", issn.ToString().DosToUnix());

            issn = _GetIssn();
            Assert.AreEqual ("0378-5955", issn.ToString().DosToUnix());
        }
    }
}
