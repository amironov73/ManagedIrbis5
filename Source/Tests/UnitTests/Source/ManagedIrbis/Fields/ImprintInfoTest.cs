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
    public sealed class ImprintInfoTest
        : Common.CommonUnitTest
    {
        private ImprintInfo _GetImprint()
        {
            return new ()
            {
                Publisher = "Центрполиграф",
                City1 = "Москва",
                Year = "2012"
            };
        }

        private Field _GetField()
        {
            return new Field (ImprintInfo.Tag)
                .Add ('d', "2012")
                .Add ('a', "Москва")
                .Add ('c', "Центрполиграф");
        }

        private void _Compare
            (
                ImprintInfo first,
                ImprintInfo second
            )
        {
            Assert.AreEqual (first.Publisher, second.Publisher);
            Assert.AreEqual (first.PrintedPublisher, second.PrintedPublisher);
            Assert.AreEqual (first.City1, second.City1);
            Assert.AreEqual (first.City2, second.City2);
            Assert.AreEqual (first.City3, second.City3);
            Assert.AreEqual (first.Year, second.Year);
            Assert.AreEqual (first.Place, second.Place);
            Assert.AreEqual (first.PrintingHouse, second.PrintingHouse);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ImprintInfo_Construction_1()
        {
            var imprint = new ImprintInfo();
            Assert.IsNull (imprint.Publisher);
            Assert.IsNull (imprint.PrintedPublisher);
            Assert.IsNull (imprint.City1);
            Assert.IsNull (imprint.City2);
            Assert.IsNull (imprint.City3);
            Assert.IsNull (imprint.Year);
            Assert.IsNull (imprint.Place);
            Assert.IsNull (imprint.PrintingHouse);
            Assert.IsNull (imprint.UnknownSubFields);
            Assert.IsNull (imprint.Field);
            Assert.IsNull (imprint.UserData);
        }

        [TestMethod]
        [Description ("Конструктор с городом, издательством и годом")]
        public void ImprintInfo_Construction_2()
        {
            var imprint = new ImprintInfo ("Москва", "Центрполиграф", "2012");
            Assert.AreEqual ("Центрполиграф", imprint.Publisher);
            Assert.IsNull (imprint.PrintedPublisher);
            Assert.AreEqual ("Москва", imprint.City1);
            Assert.IsNull (imprint.City2);
            Assert.IsNull (imprint.City3);
            Assert.AreEqual ("2012", imprint.Year);
            Assert.IsNull (imprint.Place);
            Assert.IsNull (imprint.PrintingHouse);
            Assert.IsNull (imprint.UnknownSubFields);
            Assert.IsNull (imprint.Field);
            Assert.IsNull (imprint.UserData);
        }

        private void _TestSerialization
            (
                ImprintInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<ImprintInfo>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.AreSame (first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void ImprintInfo_Serialization_1()
        {
            var imprint = new ImprintInfo();
            _TestSerialization (imprint);

            imprint.Field = new Field();
            imprint.UserData = "User data";
            _TestSerialization (imprint);

            imprint = _GetImprint();
            _TestSerialization (imprint);
        }

        [TestMethod]
        [Description ("Разбор указанного поля библиографической записи")]
        public void ImprintInfo_ParseField_1()
        {
            var expected = _GetImprint();
            var field = _GetField();
            var actual = ImprintInfo.ParseField (field);
            Assert.AreSame (field, actual.Field);
            _Compare (expected, actual);
            Assert.IsNotNull (actual.UnknownSubFields);
            Assert.AreEqual (0, actual.UnknownSubFields!.Length);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void ImprintInfo_ParseRecord_1()
        {
            var expected = _GetImprint();
            var record = new Record();
            var field = _GetField();
            record.Fields.Add (field);
            var actual = ImprintInfo.ParseRecord (record);
            Assert.AreEqual (1, actual.Length);
            Assert.AreSame (field, actual[0].Field);
            _Compare (expected, actual[0]);
            Assert.IsNotNull (actual[0].UnknownSubFields);
            Assert.AreEqual (0, actual[0].UnknownSubFields!.Length);
        }

        [TestMethod]
        [Description ("Преобразование данных в поле библиографической записи")]
        public void ImprintInfo_ToField_1()
        {
            var expected = _GetField();
            var imprint = _GetImprint();
            var actual = imprint.ToField();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю библиографической записи")]
        public void ImprintInfo_ApplyTo_1()
        {
            var expected = _GetField();
            var actual = new Field (ImprintInfo.Tag)
                .Add ('a', "???")
                .Add ('c', "???");
            var imprint = _GetImprint();
            imprint.ApplyTo (actual);
            Assert.AreEqual (ImprintInfo.Tag, actual.Tag);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void ImprintInfo_Verify_1()
        {
            var imprint = new ImprintInfo();
            Assert.IsFalse (imprint.Verify (false));

            imprint = _GetImprint();
            Assert.IsTrue (imprint.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void ImprintInfo_ToXml_1()
        {
            var imprint = new ImprintInfo();
            Assert.AreEqual ("<imprint />", XmlUtility.SerializeShort (imprint));

            imprint = _GetImprint();
            Assert.AreEqual ("<imprint publisher=\"Центрполиграф\" city1=\"Москва\" year=\"2012\" />",
                XmlUtility.SerializeShort (imprint));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void ImprintInfo_ToJson_1()
        {
            var imprint = new ImprintInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (imprint));

            imprint = _GetImprint();
            Assert.AreEqual (
                "{\"publisher\":\"\\u0426\\u0435\\u043D\\u0442\\u0440\\u043F\\u043E\\u043B\\u0438\\u0433\\u0440\\u0430\\u0444\",\"city1\":\"\\u041C\\u043E\\u0441\\u043A\\u0432\\u0430\",\"year\":\"2012\"}",
                JsonUtility.SerializeShort (imprint));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void ImprintInfo_ToString_1()
        {
            var imprint = new ImprintInfo();
            Assert.AreEqual ("(null): (null), (null)", imprint.ToString());

            imprint = _GetImprint();
            Assert.AreEqual ("Москва: Центрполиграф, 2012", imprint.ToString());
        }
    }
}
