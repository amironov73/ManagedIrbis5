// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Json;
using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class TitleInfoTest
        : CommonFieldsTest
    {
        [TestMethod]
        public void TitleInfo_Constructor_1()
        {
            var title = new TitleInfo();
            Assert.IsNull(title.VolumeNumber);
            Assert.IsNull(title.Title);
            Assert.IsNull(title.Specific);
            Assert.IsNull(title.General);
            Assert.IsNull(title.Subtitle);
            Assert.IsNull(title.FirstResponsibility);
            Assert.IsNull(title.OtherResponsibility);
            Assert.IsNull(title.UserData);
        }

        [TestMethod]
        public void TitleInfo_Constructor_2()
        {
            const string expected = "Сказка о рыбаке и рыбке";
            var title = new TitleInfo(expected);
            Assert.IsNull(title.VolumeNumber);
            Assert.AreEqual(expected, title.Title);
            Assert.IsNull(title.Specific);
            Assert.IsNull(title.General);
            Assert.IsNull(title.Subtitle);
            Assert.IsNull(title.FirstResponsibility);
            Assert.IsNull(title.OtherResponsibility);
            Assert.IsNull(title.UserData);
        }

        [TestMethod]
        public void TitleInfo_Constructor_3()
        {
            const string expected1 = "Т. 2";
            const string expected2 = "Письма";
            var title = new TitleInfo
                (
                    expected1,
                    expected2
                );
            Assert.AreEqual(expected1, title.VolumeNumber);
            Assert.AreEqual(expected2, title.Title);
            Assert.IsNull(title.Specific);
            Assert.IsNull(title.General);
            Assert.IsNull(title.Subtitle);
            Assert.IsNull(title.FirstResponsibility);
            Assert.IsNull(title.OtherResponsibility);
            Assert.IsNull(title.UserData);
        }

        private Record _GetRecord()
        {
            var result = new Record();
            result.Fields.Add
                (
                    Parse
                        (
                            200,
                            "^AПикассо сегодня"
                            + "^E[сборник статей]"
                            + "^FА. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев"
                            + "^GРос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
                        )
                );

            return result;
        }

        [TestMethod]
        public void TitleInfo_Parse_1()
        {
            var record = _GetRecord();
            var actual = TitleInfo.Parse(record);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("Пикассо сегодня", actual[0].Title);
            Assert.AreEqual("[сборник статей]", actual[0].Subtitle);
            Assert.AreEqual("А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев", actual[0].FirstResponsibility);
            Assert.AreEqual("Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина", actual[0].OtherResponsibility);
        }

        [TestMethod]
        public void TitleInfo_ToField_1()
        {
            var title = new TitleInfo
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };

            var actual = title.ToField200();
            Assert.AreEqual("Пикассо сегодня", actual.GetFirstSubFieldValue('a').ToString().EmptyToNull());
            Assert.AreEqual("[сборник статей]", actual.GetFirstSubFieldValue('e').ToString().EmptyToNull());
            Assert.AreEqual("А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев", actual.GetFirstSubFieldValue('f').ToString().EmptyToNull());
            Assert.AreEqual("Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина", actual.GetFirstSubFieldValue('g').ToString().EmptyToNull());
        }

        [TestMethod]
        public void TitleInfo_ToString_1()
        {
            var title = new TitleInfo
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };

            const string expected = "Title: Пикассо сегодня, Subtitle: [сборник статей]";
            var actual = title.ToString();
            Assert.AreEqual(expected, actual);
        }

        private void _TestSerialization
            (
                TitleInfo first
            )
        {
            var bytes = first.SaveToMemory();

            var second = bytes.RestoreObjectFromMemory<TitleInfo>()
                .ThrowIfNull();
            Assert.AreEqual(first.VolumeNumber, second.VolumeNumber);
            Assert.AreEqual(first.Title, second.Title);
            Assert.AreEqual(first.Specific, second.Specific);
            Assert.AreEqual(first.General, second.General);
            Assert.AreEqual(first.Subtitle, second.Subtitle);
            Assert.AreEqual(first.FirstResponsibility, second.FirstResponsibility);
            Assert.AreEqual(first.OtherResponsibility, second.OtherResponsibility);
        }

        [TestMethod]
        public void TitleInfo_Serialization_1()
        {
            var title = new TitleInfo();
            _TestSerialization(title);

            title = new TitleInfo("Сказка о рыбаке и рыбке");
            _TestSerialization(title);

            title = new TitleInfo
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };
            _TestSerialization(title);
        }

        [Ignore]
        [TestMethod]
        public void TitleInfo_ToJson_1()
        {
            var title = new TitleInfo
            {
                Title = "Пикассо сегодня",
                Subtitle = "[сборник статей]",
                FirstResponsibility = "А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев",
                OtherResponsibility = "Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина"
            };

            const string expected = "{\"title\":\"Пикассо сегодня\",\"subtitle\":\"[сборник статей]\",\"first\":\"А. А. Бабин, Т. В. Балашова ; отв. ред. М. А. Бусев\",\"other\":\"Рос. акад. художеств, Гос. музей изобр. искусств им. А. С. Пушкина\"}";
            var actual = JsonUtility.SerializeShort(title);
            Assert.AreEqual(expected, actual);
        }
    }
}
