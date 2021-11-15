// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class TitleInfoTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void TitleInfo_Constructor_1()
        {
            var title = new TitleInfo();
            Assert.IsNull (title.VolumeNumber);
            Assert.IsNull (title.Title);
            Assert.IsNull (title.Specific);
            Assert.IsNull (title.General);
            Assert.IsNull (title.Subtitle);
            Assert.IsNull (title.FirstResponsibility);
            Assert.IsNull (title.OtherResponsibility);
            Assert.IsNull (title.UserData);
        }

        [TestMethod]
        [Description ("Конструктор с заголовком")]
        public void TitleInfo_Constructor_2()
        {
            const string expected = "Сказка о рыбаке и рыбке";
            var title = new TitleInfo (expected);
            Assert.IsNull (title.VolumeNumber);
            Assert.AreEqual (expected, title.Title);
            Assert.IsNull (title.Specific);
            Assert.IsNull (title.General);
            Assert.IsNull (title.Subtitle);
            Assert.IsNull (title.FirstResponsibility);
            Assert.IsNull (title.OtherResponsibility);
            Assert.IsNull (title.UserData);
        }

        [TestMethod]
        [Description ("Конструктор с частью и заголовком")]
        public void TitleInfo_Constructor_3()
        {
            const string expected1 = "Т. 2";
            const string expected2 = "Письма";
            var title = new TitleInfo
                (
                    expected1,
                    expected2
                );
            Assert.AreEqual (expected1, title.VolumeNumber);
            Assert.AreEqual (expected2, title.Title);
            Assert.IsNull (title.Specific);
            Assert.IsNull (title.General);
            Assert.IsNull (title.Subtitle);
            Assert.IsNull (title.FirstResponsibility);
            Assert.IsNull (title.OtherResponsibility);
            Assert.IsNull (title.UserData);
        }

        private Field _GetField200()
        {
            return new Field (200)
            {
                { 'v', "Т. 2" },
                { 'a', "Пикассо сегодня" },
                { 'e', "[сборник статей]" },
                { 'f', "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев" },
                { 'g', "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина" },
            };
        }

        private Field _GetField330()
        {
            return new Field (330)
            {
                { 'c', "Пикассо сегодня" },
                { 'e', "[сборник статей]" },
                { 'g', "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев" },
            };
        }

        private TitleInfo _GetTitleInfo()
        {
            return new TitleInfo()
            {
                VolumeNumber = "Т. 2",
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };
        }

        private Record _GetRecord()
        {
            return new Record().Add (_GetField200());
        }

        [TestMethod]
        [Description ("Разбор поля 200")]
        public void TitleInfo_ParseField200_1()
        {
            var expected = _GetTitleInfo();
            var field = _GetField200();
            var actual = TitleInfo.ParseField200 (field);
            _Compare (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля 330")]
        public void TitleInfo_ParseField330_1()
        {
            var expected = new TitleInfo()
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
            };
            var field = _GetField330();
            var actual = TitleInfo.ParseField330 (field);
            _Compare (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void TitleInfo_ParseRecord_1()
        {
            var record = _GetRecord();
            var actual = TitleInfo.ParseRecord (record);
            Assert.AreEqual (1, actual.Length);
            Assert.AreEqual ("Пикассо сегодня", actual[0].Title);
            Assert.AreEqual ("[сборник статей]", actual[0].Subtitle);
            Assert.AreEqual ("А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев", actual[0].FirstResponsibility);
            Assert.AreEqual ("Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина",
                actual[0].OtherResponsibility);
        }

        [TestMethod]
        [Description ("Преобразование данных в поле библиографической записи")]
        public void TitleInfo_ToField200_1()
        {
            var expected = _GetField200();
            var title = _GetTitleInfo();
            var actual = title.ToField200();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Преобразование данных в поле библиографической записи")]
        public void TitleInfo_ToField330_1()
        {
            var expected = _GetField330();
            var title = _GetTitleInfo();
            var actual = title.ToField330(330);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void TitleInfo_ToString_1()
        {
            var title = new TitleInfo();
            var actual = title.ToString();
            Assert.AreEqual ("(null)", actual);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void TitleInfo_ToString_2()
        {
            var title = new TitleInfo
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };

            const string expected = "Пикассо сегодня : [сборник статей] / А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев ; Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина";
            var actual = title.ToString();
            Assert.AreEqual (expected, actual);
        }

        private void _Compare
            (
                TitleInfo first,
                TitleInfo second
            )
        {
            Assert.AreEqual (first.VolumeNumber, second.VolumeNumber);
            Assert.AreEqual (first.Title, second.Title);
            Assert.AreEqual (first.Specific, second.Specific);
            Assert.AreEqual (first.General, second.General);
            Assert.AreEqual (first.Subtitle, second.Subtitle);
            Assert.AreEqual (first.FirstResponsibility, second.FirstResponsibility);
            Assert.AreEqual (first.OtherResponsibility, second.OtherResponsibility);
        }

        private void _TestSerialization
            (
                TitleInfo first
            )
        {
            var bytes = first.SaveToMemory();

            var second = bytes.RestoreObjectFromMemory<TitleInfo>()
                .ThrowIfNull();
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void TitleInfo_Serialization_1()
        {
            var title = new TitleInfo();
            _TestSerialization (title);

            title = new TitleInfo ("Сказка о рыбаке и рыбке");
            _TestSerialization (title);

            title = _GetTitleInfo();
            title.UserData = new object();
            _TestSerialization (title);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void TitleInfo_Verify_1()
        {
            var info = new TitleInfo();
            Assert.IsFalse (info.Verify (false));

            info = _GetTitleInfo();
            Assert.IsTrue (info.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void TitleInfoInfo_ToXml_1()
        {
            var info = new TitleInfo();
            Assert.AreEqual ("<title />", XmlUtility.SerializeShort (info));

            info = _GetTitleInfo();
            Assert.AreEqual ("<title volume=\"Т. 2\" title=\"Пикассо сегодня\" subtitle=\"[сборник статей]\" first=\"А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев\" other=\"Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина\" />",
                XmlUtility.SerializeShort (info));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void TitleInfo_ToJson_1()
        {
            var title = _GetTitleInfo();

            const string expected = "{\"volume\":\"\\u0422. 2\",\"title\":\"\\u041F\\u0438\\u043A\\u0430\\u0441\\u0441\\u043E \\u0441\\u0435\\u0433\\u043E\\u0434\\u043D\\u044F\",\"subtitle\":\"[\\u0441\\u0431\\u043E\\u0440\\u043D\\u0438\\u043A \\u0441\\u0442\\u0430\\u0442\\u0435\\u0439]\",\"first\":\"\\u0410. \\u0410. \\u0411\\u0430\\u0431\\u0438\\u043D, \\u0422. \\u0412. \\u0411\\u0430\\u043B\\u0430\\u0448\\u043E\\u0432\\u0430 ; \\u043E\\u0442\\u0432. \\u0440\\u0435\\u0434. \\u041C. \\u0410. \\u0411\\u0443\\u0441\\u0435\\u0432\",\"other\":\"\\u0420\\u043E\\u0441. \\u0430\\u043A\\u0430\\u0434. \\u0445\\u0443\\u0434\\u043E\\u0436\\u0435\\u0441\\u0442\\u0432, \\u0413\\u043E\\u0441. \\u043C\\u0443\\u0437\\u0435\\u0439 \\u0438\\u0437\\u043E\\u0431\\u0440. \\u0438\\u0441\\u043A\\u0443\\u0441\\u0441\\u0442\\u0432 \\u0438\\u043C. \\u0410. \\u0421. \\u041F\\u0443\\u0448\\u043A\\u0438\\u043D\\u0430\"}";
            var actual = JsonUtility.SerializeShort (title);
            Assert.AreEqual (expected, actual);
        }
    }
}
