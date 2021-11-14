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
    public sealed class LanguageInfoTest
        : Common.CommonUnitTest
    {
        private LanguageInfo _GetLanguageInfo() => new ()
        {
            CatalogingLanguage = "rus",
            CatalogingRules = "RCR",
            MainTitleLanguage = "rus",
            OriginalLanguage = "eng"
        };

        private Field _GetField() => new (LanguageInfo.Tag, "^arus^kRCR^zrus^oeng");

        private void _Compare
            (
                LanguageInfo first,
                LanguageInfo second
            )
        {
            Assert.AreEqual (first.CatalogingLanguage, second.CatalogingLanguage);
            Assert.AreEqual (first.CatalogingRules, second.CatalogingRules);
            Assert.AreEqual (first.CharacterSet, second.CharacterSet);
            Assert.AreEqual (first.TitleCharacterSet, second.TitleCharacterSet);
            Assert.AreEqual (first.IntermediateTranslationLanguage, second.IntermediateTranslationLanguage);
            Assert.AreEqual (first.OriginalLanguage, second.OriginalLanguage);
            Assert.AreEqual (first.TocLanguage, second.TocLanguage);
            Assert.AreEqual (first.TitlePageLanguage, second.TitlePageLanguage);
            Assert.AreEqual (first.MainTitleLanguage, second.MainTitleLanguage);
            Assert.AreEqual (first.AccompanyingMaterialLanguage, second.AccompanyingMaterialLanguage);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void LanguageInfo_Constructor_1()
        {
            var contentType = new LanguageInfo();
            Assert.IsNull (contentType.CatalogingLanguage);
            Assert.IsNull (contentType.CatalogingRules);
            Assert.IsNull (contentType.CharacterSet);
            Assert.IsNull (contentType.TitleCharacterSet);
            Assert.IsNull (contentType.IntermediateTranslationLanguage);
            Assert.IsNull (contentType.OriginalLanguage);
            Assert.IsNull (contentType.TocLanguage);
            Assert.IsNull (contentType.TitlePageLanguage);
            Assert.IsNull (contentType.MainTitleLanguage);
            Assert.IsNull (contentType.AccompanyingMaterialLanguage);
            Assert.IsNull (contentType.UnknownSubFields);
            Assert.IsNull (contentType.Field);
            Assert.IsNull (contentType.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к полю библиографической записи")]
        public void LanguageInfo_ApplyTo_1()
        {
            var actual = new Field()
                .Add ('a', "o")
                .Add ('k', "03")
                .Add ('o', "111")
                .Add ('z', "91");
            var codes = _GetLanguageInfo();
            codes.ApplyToField (actual);
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void LanguageInfo_ParseField_1()
        {
            var field = _GetField();
            var actual = LanguageInfo.ParseField (field);
            var expected = _GetLanguageInfo();
            _Compare (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void LanguageInfo_ParseRecord_1()
        {
            var record = new Record();
            var field = _GetField();
            record.Add (field);
            var actual = LanguageInfo.ParseRecord (record);
            var expected = _GetLanguageInfo();
            Assert.IsNotNull (actual);
            Assert.AreEqual (1, actual.Length);
            _Compare (expected, actual[0]);
        }

        [TestMethod]
        [Description ("Преобразование в поле библиографической записи")]
        public void LanguageInfo_ToField_1()
        {
            var contentType = _GetLanguageInfo();
            var actual = contentType.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                LanguageInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<LanguageInfo>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void LanguageInfo_Serialization_1()
        {
            var contentType = new LanguageInfo();
            _TestSerialization (contentType);

            contentType = _GetLanguageInfo();
            contentType.Field = new Field();
            contentType.UserData = "User data";
            _TestSerialization (contentType);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void LanguageInfo_Verification_1()
        {
            var contentType = new LanguageInfo();
            Assert.IsFalse (contentType.Verify (false));

            contentType = _GetLanguageInfo();
            Assert.IsTrue (contentType.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void LanguageInfo_ToXml_1()
        {
            var contentType = new LanguageInfo();
            Assert.AreEqual ("<language-info />", XmlUtility.SerializeShort (contentType));

            contentType = _GetLanguageInfo();
            Assert.AreEqual ("<language-info><catalogingLanguage>rus</catalogingLanguage><cataloguingRules>RCR</cataloguingRules><originalLanguage>eng</originalLanguage><mainTitleLanguage>rus</mainTitleLanguage></language-info>",
                XmlUtility.SerializeShort (contentType));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void LanguageInfo_ToJson_1()
        {
            var contentType = new LanguageInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (contentType));

            contentType = _GetLanguageInfo();
            Assert.AreEqual ("{\"catalogingLanguage\":\"rus\",\"cataloguingRules\":\"RCR\",\"originalLanguage\":\"eng\",\"mainTitleLanguage\":\"rus\"}",
                JsonUtility.SerializeShort (contentType));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void LanguageInfo_ToString_1()
        {
            var contentType = new LanguageInfo();
            Assert.AreEqual ("(null)",
                contentType.ToString());

            contentType = _GetLanguageInfo();
            Assert.AreEqual ("rus",
                contentType.ToString());
        }
    }
}
