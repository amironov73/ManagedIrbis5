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
    public sealed class HeadingInfoTest
        : Common.CommonUnitTest
    {
        private HeadingInfo _GetHeading() => new ()
        {
            Title = "Русская литература",
            Subtitle1 = "Проза",
            GeographicalSubtitle1 = "Санкт-Петербург",
            ChronologicalSubtitle = "19 в.",
            Aspect = "Сборники"
        };

        private Field _GetField() => new Field (HeadingInfo.Tag)
            .Add ('a', "Русская литература")
            .Add ('b', "Проза")
            .Add ('g', "Санкт-Петербург")
            .Add ('h', "19 в.")
            .Add ('9', "Сборники");

        private void _Compare
            (
                HeadingInfo first,
                HeadingInfo second
            )
        {
            Assert.AreEqual (first.Title, second.Title);
            Assert.AreEqual (first.Subtitle1, second.Subtitle1);
            Assert.AreEqual (first.Subtitle2, second.Subtitle2);
            Assert.AreEqual (first.Subtitle3, second.Subtitle3);
            Assert.AreEqual (first.GeographicalSubtitle1, second.GeographicalSubtitle1);
            Assert.AreEqual (first.GeographicalSubtitle2, second.GeographicalSubtitle2);
            Assert.AreEqual (first.GeographicalSubtitle3, second.GeographicalSubtitle3);
            Assert.AreEqual (first.ChronologicalSubtitle, second.ChronologicalSubtitle);
            Assert.AreEqual (first.Aspect, second.Aspect);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void HeadingInfo_Construction_1()
        {
            var heading = new HeadingInfo();
            Assert.IsNull (heading.Title);
            Assert.IsNull (heading.Subtitle1);
            Assert.IsNull (heading.Subtitle2);
            Assert.IsNull (heading.Subtitle3);
            Assert.IsNull (heading.GeographicalSubtitle1);
            Assert.IsNull (heading.GeographicalSubtitle2);
            Assert.IsNull (heading.GeographicalSubtitle3);
            Assert.IsNull (heading.ChronologicalSubtitle);
            Assert.IsNull (heading.Aspect);
            Assert.IsNull (heading.UnknownSubFields);
            Assert.IsNull (heading.Field);
            Assert.IsNull (heading.UserData);
        }

        private void _TestSerialization
            (
                HeadingInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<HeadingInfo>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.AreSame (first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void HeadingInfo_Serialization_1()
        {
            var heading = new HeadingInfo();
            _TestSerialization (heading);

            heading.UserData = "User data";
            _TestSerialization (heading);

            heading = _GetHeading();
            _TestSerialization (heading);
        }

        [TestMethod]
        [Description ("Разбор указанного поля библиографической записи")]
        public void HeadingInfo_ParseField_1()
        {
            var expected = _GetHeading();
            var field = _GetField();
            var actual = HeadingInfo.ParseField (field);
            Assert.AreSame (field, actual.Field);
            _Compare (expected, actual);
            Assert.IsNotNull (actual.UnknownSubFields);
            Assert.AreEqual (0, actual.UnknownSubFields!.Length);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void HeadingInfo_ParseRecord_1()
        {
            var expected = _GetHeading();
            var record = new Record().Add (_GetField());
            var actual = HeadingInfo.ParseRecord (record);
            Assert.AreEqual (1, actual.Length);
            _Compare (expected, actual[0]);
            Assert.IsNotNull (actual[0].UnknownSubFields);
            Assert.AreEqual (0, actual[0].UnknownSubFields!.Length);
            Assert.IsNull (actual[0].UserData);
        }

        [TestMethod]
        [Description ("Преобразование данных в поле библиографической записи")]
        public void HeadingInfo_ToField_1()
        {
            var expected = _GetField();
            var heading = _GetHeading();
            var actual = heading.ToField();
            Assert.AreEqual (expected.Tag, actual.Tag);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю библиографической записи")]
        public void HeadingInfo_ApplyToField_1()
        {
            var expected = _GetField();
            var actual = new Field (HeadingInfo.Tag)
                .Add ('a', "???")
                .Add ('b', "???");
            _GetHeading().ApplyToField (actual);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void HeadingInfo_Verify_1()
        {
            var heading = new HeadingInfo();
            Assert.IsFalse (heading.Verify (false));

            heading = _GetHeading();
            Assert.IsTrue (heading.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void HeadingInfo_ToXml_1()
        {
            var heading = new HeadingInfo();
            Assert.AreEqual ("<heading />", XmlUtility.SerializeShort (heading));

            heading = _GetHeading();
            Assert.AreEqual ("<heading><title>Русская литература</title><subtitle1>Проза</subtitle1><geoSubtitle1>Санкт-Петербург</geoSubtitle1><chronoSubtitle>19 в.</chronoSubtitle><aspect>Сборники</aspect></heading>",
                XmlUtility.SerializeShort (heading));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void HeadingInfo_ToJson_1()
        {
            var heading = new HeadingInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (heading));

            heading = _GetHeading();
            Assert.AreEqual ("{\"title\":\"\\u0420\\u0443\\u0441\\u0441\\u043A\\u0430\\u044F \\u043B\\u0438\\u0442\\u0435\\u0440\\u0430\\u0442\\u0443\\u0440\\u0430\",\"subtitle1\":\"\\u041F\\u0440\\u043E\\u0437\\u0430\",\"geoSubtitle1\":\"\\u0421\\u0430\\u043D\\u043A\\u0442-\\u041F\\u0435\\u0442\\u0435\\u0440\\u0431\\u0443\\u0440\\u0433\",\"chronoSubtitle\":\"19 \\u0432.\",\"aspect\":\"\\u0421\\u0431\\u043E\\u0440\\u043D\\u0438\\u043A\\u0438\"}",
                JsonUtility.SerializeShort (heading));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void HeadingInfo_ToString_1()
        {
            var heading = new HeadingInfo();
            Assert.AreEqual
                (
                    "(null)",
                    heading.ToString().DosToUnix()
                );

            heading = _GetHeading();
            Assert.AreEqual
                (
                    "Русская литература -- Проза -- Санкт-Петербург -- 19 в. -- Сборники",
                    heading.ToString().DosToUnix()
                );
        }
    }
}
