// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
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
    public sealed class AthraTitleTest
        : Common.CommonUnitTest
    {
        private Field _GetField() => new Field (AthraTitle.Tag)
            .Add ('a', "Иванов")
            .Add ('b', "И. И.")
            .Add ('g', "Иван Иванович")
            .Add ('f', "1991-");

        private AthraTitle _GetAthraTitle() => new ()
        {
            Surname = "Иванов",
            Initials = "И. И.",
            Extension = "Иван Иванович",
            Dates = "1991-"
        };

        private void _Compare
            (
                AthraTitle first,
                AthraTitle second
            )
        {
            Assert.AreEqual (first.Surname, second.Surname);
            Assert.AreEqual (first.Initials, second.Initials);
            Assert.AreEqual (first.Extension, second.Extension);
            Assert.AreEqual (first.Role, second.Role);
            Assert.AreEqual (first.IntegralPart, second.IntegralPart);
            Assert.AreEqual (first.IdentifyingSigns, second.IdentifyingSigns);
            Assert.AreEqual (first.RomanNumerals, second.RomanNumerals);
            Assert.AreEqual (first.Dates, second.Dates);
            Assert.AreEqual (first.CorrectionNeeded, second.CorrectionNeeded);
            Assert.AreEqual (first.Graphics, second.Graphics);
            Assert.AreEqual (first.Language, second.Language);
            Assert.AreEqual (first.Mark, second.Mark);
            Assert.AreEqual (first.RelationCode, second.RelationCode);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthraTitle_Construction_1()
        {
            var athra = new AthraTitle();
            Assert.IsNull (athra.Surname);
            Assert.IsNull (athra.Initials);
            Assert.IsNull (athra.Extension);
            Assert.IsNull (athra.Role);
            Assert.IsNull (athra.IntegralPart);
            Assert.IsNull (athra.IdentifyingSigns);
            Assert.IsNull (athra.RomanNumerals);
            Assert.IsNull (athra.Dates);
            Assert.IsNull (athra.CorrectionNeeded);
            Assert.IsNull (athra.Graphics);
            Assert.IsNull (athra.Language);
            Assert.IsNull (athra.Mark);
            Assert.IsNull (athra.RelationCode);
            Assert.IsNull (athra.UnknownSubFields);
            Assert.IsNull (athra.Field);
            Assert.IsNull (athra.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void AthraTitle_ApplyTo_1()
        {
            var expected = _GetField();
            var actual = new Field();
            var athra = _GetAthraTitle();
            Assert.AreSame (actual, athra.ApplyTo (actual));
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void AthraTitle_ParseField_1()
        {
            var field = _GetField();
            var actual = AthraTitle.ParseField (field);
            var expected = _GetAthraTitle();
            _Compare (expected, actual);
            Assert.AreSame (field, actual.Field);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void AthraTitle_ToField_1()
        {
            var athra = _GetAthraTitle();
            var actual = athra.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                AthraTitle first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<AthraTitle>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void AthraTitle_Serialization_1()
        {
            var athra = new AthraTitle();
            _TestSerialization (athra);

            athra = _GetAthraTitle();
            athra.Field = new Field();
            athra.UserData = "User data";
            _TestSerialization (athra);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void AthraTitle_Verify_1()
        {
            var athra = new AthraTitle();
            Assert.IsFalse (athra.Verify (false));

            athra = _GetAthraTitle();
            Assert.IsTrue (athra.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void AthraTitle_ToXml_1()
        {
            var athra = new AthraTitle();
            Assert.AreEqual ("<title />", XmlUtility.SerializeShort (athra));

            athra = _GetAthraTitle();
            Assert.AreEqual ("<title><surname>Иванов</surname><initials>И. И.</initials><extension>Иван Иванович</extension><dates>1991-</dates></title>",
                XmlUtility.SerializeShort (athra));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void AthraTitle_ToJson_1()
        {
            var athra = new AthraTitle();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (athra));

            athra = _GetAthraTitle();
            var expected = "{\"surname\":\"\\u0418\\u0432\\u0430\\u043D\\u043E\\u0432\",\"initials\":\"\\u0418. \\u0418.\",\"extension\":\"\\u0418\\u0432\\u0430\\u043D \\u0418\\u0432\\u0430\\u043D\\u043E\\u0432\\u0438\\u0447\",\"dates\":\"1991-\"}";
            var actual = JsonUtility.SerializeShort (athra);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void AthraTitle_ToString_1()
        {
            var athra = new AthraTitle();
            Assert.AreEqual ("(null)", athra.ToString());

            athra = _GetAthraTitle();
            Assert.AreEqual ("Иванов", athra.ToString());
        }
    }
}
