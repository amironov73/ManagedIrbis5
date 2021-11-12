// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class CommonInfoTest
        : Common.CommonUnitTest
    {
        private Field _GetField461() =>
            new (461, "^CУправление банком^GНаука^DМ.; СПб.^H1990^P2-е изд.^FЗ. М. Акулова [и др.]");

        private Field _GetField46() => new (46, "^AДеньги, кредит, финансирование");

        private Record _GetRecord() => new ()
            {
                _GetField461(),
                _GetField46()
            };

        private CommonInfo _GetCommonInfo() => new ()
            {
                Title = "Управление банком",
                City = "М.; СПб.",
                BeginningYear = "1990",
                Edition = "2-е изд.",
                Publisher = "Наука",
                SeriesTitle = "Деньги, кредит, финансирование",
                Responsibility = "З. М. Акулова [и др.]"
            };

        private void _Compare
            (
                CommonInfo first,
                CommonInfo second
            )
        {
            Assert.AreEqual (first.Title, second.Title);
            Assert.AreEqual (first.Specific, second.Specific);
            Assert.AreEqual (first.General, second.General);
            Assert.AreEqual (first.Subtitle, second.Subtitle);
            Assert.AreEqual (first.Responsibility, second.Responsibility);
            Assert.AreEqual (first.Publisher, second.Publisher);
            Assert.AreEqual (first.City, second.City);
            Assert.AreEqual (first.BeginningYear, second.BeginningYear);
            Assert.AreEqual (first.EndingYear, second.EndingYear);
            Assert.AreEqual (first.Isbn, second.Isbn);
            Assert.AreEqual (first.Issn, second.Issn);
            Assert.AreEqual (first.Edition, second.Edition);
            Assert.AreEqual (first.Translation, second.Translation);
            Assert.AreEqual (first.FirstAuthor, second.FirstAuthor);
            Assert.AreEqual (first.Collective, second.Collective);
            Assert.AreEqual (first.TitleVariant, second.TitleVariant);
            Assert.AreEqual (first.SecondLevelNumber, second.SecondLevelNumber);
            Assert.AreEqual (first.SecondLevelTitle, second.SecondLevelTitle);
            Assert.AreEqual (first.ThirdLevelNumber, second.ThirdLevelNumber);
            Assert.AreEqual (first.ThirdLevelTitle, second.ThirdLevelTitle);
            Assert.AreEqual (first.ParallelTitle, second.ParallelTitle);
            Assert.AreEqual (first.SeriesTitle, second.SeriesTitle);
            Assert.AreEqual (first.PreviousTitle, second.PreviousTitle);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void CommonInfo_Construction_1()
        {
            var info = new CommonInfo();
            Assert.IsNull (info.Title);
            Assert.IsNull (info.Specific);
            Assert.IsNull (info.General);
            Assert.IsNull (info.Subtitle);
            Assert.IsNull (info.Responsibility);
            Assert.IsNull (info.Publisher);
            Assert.IsNull (info.City);
            Assert.IsNull (info.BeginningYear);
            Assert.IsNull (info.EndingYear);
            Assert.IsNull (info.Isbn);
            Assert.IsNull (info.Issn);
            Assert.IsNull (info.Edition);
            Assert.IsNull (info.Translation);
            Assert.IsNull (info.FirstAuthor);
            Assert.IsNull (info.Collective);
            Assert.IsNull (info.Field461);
            Assert.IsNull (info.TitleVariant);
            Assert.IsNull (info.SecondLevelNumber);
            Assert.IsNull (info.SecondLevelTitle);
            Assert.IsNull (info.ThirdLevelNumber);
            Assert.IsNull (info.ThirdLevelTitle);
            Assert.IsNull (info.ParallelTitle);
            Assert.IsNull (info.SeriesTitle);
            Assert.IsNull (info.PreviousTitle);
            Assert.IsNull (info.Field46);
            Assert.IsNull (info.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void CommonInfo_ApplyTo_1()
        {
            var expected461 = _GetField461();
            var expected46 = _GetField46();
            var actual461 = new Field();
            var actual46 = new Field();
            var info = _GetCommonInfo();
            info.ApplyTo (actual461, actual46);
            CompareFields (expected461, actual461);
            CompareFields (expected46, actual46);
        }

        [TestMethod]
        [Description ("Применение данных к библиографической записи")]
        public void CommonInfo_ApplyTo_2()
        {
            var expected = _GetRecord();
            var actual = new Record();
            var info = _GetCommonInfo();
            info.ApplyTo (actual);
            CompareFields (expected.GetField (CommonInfo.MainTag)!, actual.GetField ( (CommonInfo.MainTag))!);
            CompareFields (expected.GetField (CommonInfo.AdditionalTag)!, actual.GetField ( (CommonInfo.AdditionalTag))!);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void CommonInfo_ParseRecord_1()
        {
            var record = _GetRecord();
            var array = CommonInfo.ParseRecord (record);
            Assert.IsNotNull (array);
            Assert.AreEqual (1, array.Length);
            var info = array[0];
            Assert.AreEqual ("Управление банком", info.Title);
            Assert.IsNull (info.Specific);
            Assert.IsNull (info.General);
            Assert.IsNull (info.Subtitle);
            Assert.AreEqual ("З. М. Акулова [и др.]", info.Responsibility);
            Assert.AreEqual ("Наука", info.Publisher);
            Assert.AreEqual ("М.; СПб.", info.City);
            Assert.AreEqual ("1990", info.BeginningYear);
            Assert.AreEqual ("2-е изд.", info.Edition);
            Assert.IsNull (info.EndingYear);
            Assert.IsNull (info.Isbn);
            Assert.IsNull (info.Issn);
            Assert.IsNull (info.Translation);
            Assert.IsNull (info.FirstAuthor);
            Assert.IsNull (info.Collective);
            Assert.IsNull (info.TitleVariant);
            Assert.IsNull (info.SecondLevelNumber);
            Assert.IsNull (info.SecondLevelTitle);
            Assert.IsNull (info.ThirdLevelNumber);
            Assert.IsNull (info.ThirdLevelTitle);
            Assert.IsNull (info.ParallelTitle);
            Assert.AreEqual ("Деньги, кредит, финансирование", info.SeriesTitle);
            Assert.IsNull (info.PreviousTitle);
            Assert.IsNotNull (info.Field461);
            Assert.IsNotNull (info.Field46);
        }

        private void _TestSerialization
            (
                CommonInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<CommonInfo>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field46);
            Assert.IsNull (second.Field461);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void CommonInfo_Serialization_1()
        {
            var info = new CommonInfo();
            _TestSerialization (info);

            info = _GetCommonInfo();
            info.Field46 = new Field();
            info.Field461 = new Field();
            info.UserData = "User data";
            _TestSerialization (info);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void CommonInfo_ToField461_1()
        {
            var info = _GetCommonInfo();
            var expected = _GetField461();
            var actual = info.ToField461();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void CommonInfo_ToField46_1()
        {
            var info = _GetCommonInfo();
            var expected = _GetField46();
            var actual = info.ToField46();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void CommonInfo_Verify_1()
        {
            var info = new CommonInfo();
            Assert.IsFalse (info.Verify (false));

            info = _GetCommonInfo();
            Assert.IsTrue (info.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void CommonInfo_ToXml_1()
        {
            var info = new CommonInfo();
            Assert.AreEqual ("<common />", XmlUtility.SerializeShort (info));

            info = _GetCommonInfo();
            Assert.AreEqual ("<common><title>Управление банком</title><responsibility>З. М. Акулова [и др.]</responsibility><publisher>Наука</publisher><city>М.; СПб.</city><beginningYear>1990</beginningYear><edition>2-е изд.</edition><seriesTitle>Деньги, кредит, финансирование</seriesTitle></common>",
                XmlUtility.SerializeShort (info));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void CommonInfo_ToJson_1()
        {
            var info = new CommonInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (info));

            info = _GetCommonInfo();
            var expected = "{\"title\":\"\\u0423\\u043F\\u0440\\u0430\\u0432\\u043B\\u0435\\u043D\\u0438\\u0435 \\u0431\\u0430\\u043D\\u043A\\u043E\\u043C\",\"responsibility\":\"\\u0417. \\u041C. \\u0410\\u043A\\u0443\\u043B\\u043E\\u0432\\u0430 [\\u0438 \\u0434\\u0440.]\",\"publisher\":\"\\u041D\\u0430\\u0443\\u043A\\u0430\",\"city\":\"\\u041C.; \\u0421\\u041F\\u0431.\",\"beginningYear\":\"1990\",\"edition\":\"2-\\u0435 \\u0438\\u0437\\u0434.\",\"seriesTitle\":\"\\u0414\\u0435\\u043D\\u044C\\u0433\\u0438, \\u043A\\u0440\\u0435\\u0434\\u0438\\u0442, \\u0444\\u0438\\u043D\\u0430\\u043D\\u0441\\u0438\\u0440\\u043E\\u0432\\u0430\\u043D\\u0438\\u0435\"}";
            var actual = JsonUtility.SerializeShort (info);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void CommonInfo_ToString_1()
        {
            var info = new CommonInfo();
            Assert.AreEqual ("(null)", info.ToString());

            info = _GetCommonInfo();
            Assert.AreEqual ("Управление банком", info.ToString());
        }

    }
}
