// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
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
    public class CollectiveInfoTest
    {
        private CollectiveInfo _GetCollective() => new ()
        {
            Title = "Иркутский государственный университет",
            Country = "RU",
            Abbreviation = "ИГУ",
            City1 = "Иркутск",
            Department = "Биолого-почвенный факультет",
            Gost = "М-во образования и науки Рос. Федерации, ФГБОУ ВПО \"Иркут. гос. ун-т\", Биол.-почв. фак."
        };

        // ^AИркутский государственный университет^SRU^RИГУ^CИркутск
        // ^BБиолого-почвенный факультет
        // ^7М-во образования и науки Рос. Федерации, ФГБОУ ВПО "Иркут. гос. ун-т", Биол.-почв. фак.
        private Field _GetField() => new Field (711)
            .Add ('a', "Иркутский государственный университет")
            .Add ('s', "RU")
            .Add ('r', "ИГУ")
            .Add ('c', "Иркутск")
            .Add ('b', "Биолого-почвенный факультет")
            .Add ('7', "М-во образования и науки Рос. Федерации, ФГБОУ ВПО \"Иркут. гос. ун-т\", Биол.-почв. фак.");

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void CollectiveInfo_Construction_1()
        {
            var collective = new CollectiveInfo();
            Assert.IsNull (collective.Title);
            Assert.IsNull (collective.Country);
            Assert.IsNull (collective.Abbreviation);
            Assert.IsNull (collective.Number);
            Assert.IsNull (collective.Date);
            Assert.IsNull (collective.City1);
            Assert.IsNull (collective.Department);
            Assert.IsFalse (collective.Characteristic);
            Assert.IsNull (collective.Gost);
            Assert.IsNull (collective.UnknownSubFields);
            Assert.IsNull (collective.Field);
            Assert.IsNull (collective.UserData);
        }

        [TestMethod]
        [Description ("Конструктор с наименованием коллектива")]
        public void CollectiveInfo_Construction_2()
        {
            var collective = new CollectiveInfo ("Иркутский государственный университет");
            Assert.AreEqual ("Иркутский государственный университет", collective.Title);
            Assert.IsNull (collective.Country);
            Assert.IsNull (collective.Abbreviation);
            Assert.IsNull (collective.Number);
            Assert.IsNull (collective.Date);
            Assert.IsNull (collective.City1);
            Assert.IsNull (collective.Department);
            Assert.IsFalse (collective.Characteristic);
            Assert.IsNull (collective.Gost);
            Assert.IsNull (collective.UnknownSubFields);
            Assert.IsNull (collective.Field);
            Assert.IsNull (collective.UserData);
        }

        private void _TestSerialization
            (
                CollectiveInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<CollectiveInfo>();
            Assert.IsNotNull (second);
            Assert.AreEqual (first.Title, second.Title);
            Assert.AreEqual (first.Country, second.Country);
            Assert.AreEqual (first.Abbreviation, second.Abbreviation);
            Assert.AreEqual (first.Number, second.Number);
            Assert.AreEqual (first.Date, second.Date);
            Assert.AreEqual (first.City1, second.City1);
            Assert.AreEqual (first.Department, second.Department);
            Assert.AreEqual (first.Characteristic, second.Characteristic);
            Assert.AreEqual (first.Gost, second.Gost);
            Assert.AreSame (first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void CollectiveInfo_Serialization_1()
        {
            var collective = new CollectiveInfo();
            _TestSerialization (collective);

            collective.Field = new Field();
            collective.UserData = "User data";
            _TestSerialization (collective);

            collective = _GetCollective();
            _TestSerialization (collective);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void CollectiveInfo_ParseField_1()
        {
            var field = _GetField();
            var collective = CollectiveInfo.Parse (field);
            Assert.AreSame (field, collective.Field);
            Assert.AreEqual (field.GetFirstSubFieldValue ('a'), collective.Title);
            Assert.AreEqual (field.GetFirstSubFieldValue ('s'), collective.Country);
            Assert.AreEqual (field.GetFirstSubFieldValue ('r'), collective.Abbreviation);
            Assert.AreEqual (field.GetFirstSubFieldValue ('n'), collective.Number);
            Assert.AreEqual (field.GetFirstSubFieldValue ('d'), collective.Date);
            Assert.AreEqual (field.GetFirstSubFieldValue ('c'), collective.City1);
            Assert.AreEqual (field.GetFirstSubFieldValue ('b'), collective.Department);
            Assert.IsFalse (collective.Characteristic);
            Assert.AreEqual (field.GetFirstSubFieldValue ('7'), collective.Gost);
            Assert.IsNotNull (collective.UnknownSubFields);
            Assert.AreEqual (0, collective.UnknownSubFields!.Length);
            Assert.IsNull (collective.UserData);
        }

        [TestMethod]
        [Description ("Ращбор библиографической записи")]
        public void CollectiveInfo_ParseRecord_1()
        {
            var record = new Record();
            var field = _GetField();
            record.Fields.Add (field);
            var collective = CollectiveInfo.ParseRecord (record, CollectiveInfo.KnownTags);
            Assert.AreSame (field, collective[0].Field);
            Assert.AreEqual (field.GetFirstSubFieldValue ('a'), collective[0].Title);
            Assert.AreEqual (field.GetFirstSubFieldValue ('s'), collective[0].Country);
            Assert.AreEqual (field.GetFirstSubFieldValue ('r'), collective[0].Abbreviation);
            Assert.AreEqual (field.GetFirstSubFieldValue ('n'), collective[0].Number);
            Assert.AreEqual (field.GetFirstSubFieldValue ('d'), collective[0].Date);
            Assert.AreEqual (field.GetFirstSubFieldValue ('c'), collective[0].City1);
            Assert.AreEqual (field.GetFirstSubFieldValue ('b'), collective[0].Department);
            Assert.IsFalse (collective[0].Characteristic);
            Assert.AreEqual (field.GetFirstSubFieldValue ('7'), collective[0].Gost);
            Assert.IsNotNull (collective[0].UnknownSubFields);
            Assert.AreEqual (0, collective[0].UnknownSubFields!.Length);
            Assert.IsNull (collective[0].UserData);
        }

        [TestMethod]
        [Description ("Преобразование в поле библиографической записи")]
        public void CollectiveInfo_ToField_1()
        {
            var collective = _GetCollective();
            var field = collective.ToField (711);
            Assert.AreEqual (6, field.Subfields.Count);
            Assert.AreEqual (711, field.Tag);
            Assert.AreEqual (collective.Title, field.GetFirstSubFieldValue ('a'));
            Assert.AreEqual (collective.Country, field.GetFirstSubFieldValue ('s'));
            Assert.AreEqual (collective.Abbreviation, field.GetFirstSubFieldValue ('r'));
            Assert.AreEqual (collective.Number, field.GetFirstSubFieldValue ('n'));
            Assert.AreEqual (collective.Date, field.GetFirstSubFieldValue ('d'));
            Assert.AreEqual (collective.City1, field.GetFirstSubFieldValue ('c'));
            Assert.AreEqual (collective.Department, field.GetFirstSubFieldValue ('b'));
            Assert.AreEqual (collective.Gost, field.GetFirstSubFieldValue ('7'));
            Assert.AreEqual (collective.Characteristic, field.HaveSubField ('x'));
        }

        [TestMethod]
        [Description ("Применение данных к полю библиографической записи")]
        public void CollectiveInfo_ApplyToField_1()
        {
            var field = new Field (711)
                .Add ('a', "???")
                .Add ('b', "???");
            var collective = _GetCollective();
            collective.ApplyToField (field);
            Assert.AreEqual (6, field.Subfields.Count);
            Assert.AreEqual (711, field.Tag);
            Assert.AreEqual (collective.Title, field.GetFirstSubFieldValue ('a'));
            Assert.AreEqual (collective.Country, field.GetFirstSubFieldValue ('s'));
            Assert.AreEqual (collective.Abbreviation, field.GetFirstSubFieldValue ('r'));
            Assert.AreEqual (collective.Number, field.GetFirstSubFieldValue ('n'));
            Assert.AreEqual (collective.Date, field.GetFirstSubFieldValue ('d'));
            Assert.AreEqual (collective.City1, field.GetFirstSubFieldValue ('c'));
            Assert.AreEqual (collective.Department, field.GetFirstSubFieldValue ('b'));
            Assert.AreEqual (collective.Gost, field.GetFirstSubFieldValue ('7'));
            Assert.AreEqual (collective.Characteristic, field.HaveSubField ('x'));
        }

        [TestMethod]
        [Description ("Верификация")]
        public void CollectiveInfo_Verify_1()
        {
            var collective = new CollectiveInfo();
            Assert.IsFalse (collective.Verify (false));

            collective = _GetCollective();
            Assert.IsTrue (collective.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void CollectiveInfo_ToXml_1()
        {
            var collective = new CollectiveInfo();
            Assert.AreEqual ("<collective />", XmlUtility.SerializeShort (collective));

            collective = _GetCollective();
            Assert.AreEqual (
                "<collective><title>Иркутский государственный университет</title><country>RU</country><abbreviation>ИГУ</abbreviation><city>Иркутск</city><department>Биолого-почвенный факультет</department><gost>М-во образования и науки Рос. Федерации, ФГБОУ ВПО \"Иркут. гос. ун-т\", Биол.-почв. фак.</gost></collective>",
                XmlUtility.SerializeShort (collective));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void CollectiveInfo_ToJson_1()
        {
            var collective = new CollectiveInfo();
            Assert.AreEqual ("{\"characteristic\":false}", JsonUtility.SerializeShort (collective));

            collective = _GetCollective();
            Assert.AreEqual (
                "{\"title\":\"\\u0418\\u0440\\u043A\\u0443\\u0442\\u0441\\u043A\\u0438\\u0439 \\u0433\\u043E\\u0441\\u0443\\u0434\\u0430\\u0440\\u0441\\u0442\\u0432\\u0435\\u043D\\u043D\\u044B\\u0439 \\u0443\\u043D\\u0438\\u0432\\u0435\\u0440\\u0441\\u0438\\u0442\\u0435\\u0442\",\"country\":\"RU\",\"abbreviation\":\"\\u0418\\u0413\\u0423\",\"city\":\"\\u0418\\u0440\\u043A\\u0443\\u0442\\u0441\\u043A\",\"department\":\"\\u0411\\u0438\\u043E\\u043B\\u043E\\u0433\\u043E-\\u043F\\u043E\\u0447\\u0432\\u0435\\u043D\\u043D\\u044B\\u0439 \\u0444\\u0430\\u043A\\u0443\\u043B\\u044C\\u0442\\u0435\\u0442\",\"characteristic\":false,\"gost\":\"\\u041C-\\u0432\\u043E \\u043E\\u0431\\u0440\\u0430\\u0437\\u043E\\u0432\\u0430\\u043D\\u0438\\u044F \\u0438 \\u043D\\u0430\\u0443\\u043A\\u0438 \\u0420\\u043E\\u0441. \\u0424\\u0435\\u0434\\u0435\\u0440\\u0430\\u0446\\u0438\\u0438, \\u0424\\u0413\\u0411\\u041E\\u0423 \\u0412\\u041F\\u041E \\u0022\\u0418\\u0440\\u043A\\u0443\\u0442. \\u0433\\u043E\\u0441. \\u0443\\u043D-\\u0442\\u0022, \\u0411\\u0438\\u043E\\u043B.-\\u043F\\u043E\\u0447\\u0432. \\u0444\\u0430\\u043A.\"}",
                JsonUtility.SerializeShort (collective));
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void CollectiveInfo_ToString_1()
        {
            var collective = new CollectiveInfo();
            Assert.AreEqual ("(null)", collective.ToString());

            collective = _GetCollective();
            Assert.AreEqual ("Иркутский государственный университет", collective.ToString());
        }
    }
}
