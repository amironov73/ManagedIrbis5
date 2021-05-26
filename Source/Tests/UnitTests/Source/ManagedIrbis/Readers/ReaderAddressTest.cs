// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Readers;

#nullable enable

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class ReaderAddressTest
    {
        private ReaderAddress _GetAddress()
        {
            return new ReaderAddress
            {
                Postcode = "125075",
                Country = "Российская Федерация",
                City = "Москва",
                Street = "ул. Лесная",
                Building = "д. 5",
                Entrance = "подъезд 4",
                Apartment = "кв. 176"
            };
        }

        private static Field Parse(int tag, string value)
        {
            var result = new Field { Tag = tag };
            result.DecodeBody(value);

            return result;
        }

        private Field _GetField()
        {
            return Parse(ReaderAddress.Tag, "^A125075^BРоссийская Федерация^CМосква^Dул. Лесная^Eд. 5^Gподъезд 4^Hкв. 176");
        }

        private Record _GetRecord()
        {
            var result = new Record();
            result.Fields.Add(_GetField());

            return result;
        }

        private void _Compare
            (
                ReaderAddress first,
                ReaderAddress second
            )
        {
            Assert.AreEqual(first.Postcode, second.Postcode);
            Assert.AreEqual(first.Country, second.Country);
            Assert.AreEqual(first.City, second.City);
            Assert.AreEqual(first.Street, second.Street);
            Assert.AreEqual(first.Building, second.Building);
            Assert.AreEqual(first.Entrance, second.Entrance);
            Assert.AreEqual(first.Apartment, second.Apartment);
            Assert.AreEqual(first.AdditionalData, second.AdditionalData);
            if (!ReferenceEquals(first.UnknownSubFields, null))
            {
                Assert.IsNotNull(second.UnknownSubFields);
                Assert.AreEqual
                    (
                        first.UnknownSubFields.Length,
                        second.UnknownSubFields!.Length
                    );
                for (var i = 0; i < first.UnknownSubFields.Length; i++)
                {
                    Assert.AreEqual(first.UnknownSubFields[i].Code, second.UnknownSubFields[i].Code);
                    Assert.AreEqual(first.UnknownSubFields[i].Value, second.UnknownSubFields[i].Value);
                }
            }
        }

        [TestMethod]
        public void ReaderAddress_Construction_1()
        {
            var address = new ReaderAddress();
            Assert.IsNull(address.Postcode);
            Assert.IsNull(address.Country);
            Assert.IsNull(address.City);
            Assert.IsNull(address.Street);
            Assert.IsNull(address.Building);
            Assert.IsNull(address.Entrance);
            Assert.IsNull(address.Apartment);
            Assert.IsNull(address.AdditionalData);
            Assert.IsNull(address.Field);
            Assert.IsNull(address.UnknownSubFields);
            Assert.IsNull(address.UserData);
        }

        [TestMethod]
        public void ReaderAddress_Parse_1()
        {
            Field? field = null;
            var address = ReaderAddress.Parse(field);
            Assert.IsNull(address);

            address = ReaderAddress.Parse(_GetField());
            Assert.IsNotNull(address);
            _Compare(_GetAddress(), address!);
        }

        [TestMethod]
        public void ReaderAddress_Parse_2()
        {
            var record = _GetRecord();
            var address = ReaderAddress.Parse(record);
            Assert.IsNotNull(address);
            _Compare(_GetAddress(), address!);
        }

        [TestMethod]
        public void ReaderAddress_Parse_3()
        {
            var record = _GetRecord();
            var address = ReaderAddress.Parse(record);
            Assert.IsNotNull(address);
            _Compare(_GetAddress(), address!);
        }

        [TestMethod]
        public void ReaderAddress_ToField_1()
        {
            var address = _GetAddress();
            var field = address.ToField();
            Assert.AreEqual(7, field.Subfields.Count);
            Assert.AreEqual(address.Postcode, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(address.Country, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(address.City, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(address.Street, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(address.Building, field.GetFirstSubFieldValue('e'));
            Assert.AreEqual(address.Entrance, field.GetFirstSubFieldValue('g'));
            Assert.AreEqual(address.Apartment, field.GetFirstSubFieldValue('h'));
        }

        [TestMethod]
        public void ReaderAddress_ApplyToField_1()
        {
            Field field = new Field { Tag = ReaderAddress.Tag }
                .Add('c', "Иркутск");
            var address = _GetAddress();
            address.ApplyToField(field);
            Assert.AreEqual(7, field.Subfields.Count);
            Assert.AreEqual(address.Postcode, field.GetFirstSubFieldValue('a'));
            Assert.AreEqual(address.Country, field.GetFirstSubFieldValue('b'));
            Assert.AreEqual(address.City, field.GetFirstSubFieldValue('c'));
            Assert.AreEqual(address.Street, field.GetFirstSubFieldValue('d'));
            Assert.AreEqual(address.Building, field.GetFirstSubFieldValue('e'));
            Assert.AreEqual(address.Entrance, field.GetFirstSubFieldValue('g'));
            Assert.AreEqual(address.Apartment, field.GetFirstSubFieldValue('h'));
        }

        private void _TestSerialization
            (
                ReaderAddress first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<ReaderAddress>();

            Assert.IsNotNull(second);
            _Compare(first, second!);
        }

        [TestMethod]
        public void ReaderAddress_Serialization_1()
        {
            var address = new ReaderAddress();
            _TestSerialization(address);

            address = _GetAddress();
            address.Field = new Field();
            address.UserData = "User data";
            _TestSerialization(address);
        }

        [TestMethod]
        public void ReaderAddress_Vefify_1()
        {
            var address = new ReaderAddress();
            Assert.IsFalse(address.Verify(false));

            address = _GetAddress();
            Assert.IsTrue(address.Verify(false));
        }

        [TestMethod]
        public void ReaderAddress_ToXml_1()
        {
            var address = new ReaderAddress();
            Assert.AreEqual("<address />", XmlUtility.SerializeShort(address));

            address = _GetAddress();
            Assert.AreEqual("<address postcode=\"125075\" country=\"Российская Федерация\" city=\"Москва\" street=\"ул. Лесная\" building=\"д. 5\" entrance=\"подъезд 4\" apartment=\"кв. 176\" />", XmlUtility.SerializeShort(address));
        }

        [Ignore]
        [TestMethod]
        public void ReaderAddress_ToJson_1()
        {
            var address = new ReaderAddress();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(address));

            address = _GetAddress();
            Assert.AreEqual("{'postcode':'125075','country':'Российская Федерация','city':'Москва','street':'ул. Лесная','building':'д. 5','entrance':'подъезд 4','apartment':'кв. 176'}", JsonUtility.SerializeShort(address));
        }

        [TestMethod]
        public void ReaderAddress_ToString_1()
        {
            var address = new ReaderAddress();
            Assert.AreEqual("", address.ToString());

            address = _GetAddress();
            Assert.AreEqual("125075, Российская Федерация, Москва, ул. Лесная, д. 5, подъезд 4, кв. 176", address.ToString());
        }
    }
}
