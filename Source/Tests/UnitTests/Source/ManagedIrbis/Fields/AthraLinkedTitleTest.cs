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
    public class AthraLinkedTitleTest
        : Common.CommonUnitTest
    {
        private Field _GetField() => new Field (AthraLinkedTitle.Tag)
            .Add ('a', "Иванов")
            .Add ('b', "И. И.")
            .Add ('g', "Иван Иванович")
            .Add ('f', "1991-")
            .Add ('3', "84/И 20-425445");

        private AthraLinkedTitle _GetAthraLinkedTitle() => new ()
        {
            Surname = "Иванов",
            Initials = "И. И.",
            Extension = "Иван Иванович",
            Dates = "1991-",
            RelatedRecord = "84/И 20-425445"
        };

        private void _Compare
            (
                AthraLinkedTitle first,
                AthraLinkedTitle second
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
            Assert.AreEqual (first.Graphics, second.Graphics);
            Assert.AreEqual (first.Language, second.Language);
            Assert.AreEqual (first.Mark, second.Mark);
            Assert.AreEqual (first.RelationCode, second.RelationCode);
            Assert.AreEqual (first.RelatedRecord, second.RelatedRecord);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthraLinkedTitle_Construction_1()
        {
            var athra = new AthraLinkedTitle();
            Assert.IsNull (athra.Surname);
            Assert.IsNull (athra.Initials);
            Assert.IsNull (athra.Extension);
            Assert.IsNull (athra.Role);
            Assert.IsNull (athra.IntegralPart);
            Assert.IsNull (athra.IdentifyingSigns);
            Assert.IsNull (athra.RomanNumerals);
            Assert.IsNull (athra.Dates);
            Assert.IsNull (athra.Graphics);
            Assert.IsNull (athra.Language);
            Assert.IsNull (athra.RelatedRecord);
            Assert.IsNull (athra.Mark);
            Assert.IsNull (athra.RelationCode);
            Assert.IsNull (athra.Field);
            Assert.IsNull (athra.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void AthraLinkedTitle_ApplyTo_1()
        {
            var expected = _GetField();
            var actual = new Field();
            var athra = _GetAthraLinkedTitle();
            Assert.AreSame (actual, athra.ApplyTo (actual));
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void AthraLinkedTitle_ParseField_1()
        {
            var field = _GetField();
            var actual = AthraLinkedTitle.ParseField (field);
            var expected = _GetAthraLinkedTitle();
            _Compare (expected, actual);
            Assert.AreSame (field, actual.Field);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void Atrha_ToField_1()
        {
            var athra = _GetAthraLinkedTitle();
            var actual = athra.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                AthraLinkedTitle first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<AthraLinkedTitle>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void AthraLinkedTitle_Serialization_1()
        {
            var athra = new AthraLinkedTitle();
            _TestSerialization (athra);

            athra = _GetAthraLinkedTitle();
            athra.Field = new Field();
            athra.UserData = "User data";
            _TestSerialization (athra);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void AthraLinkedTitle_Verify_1()
        {
            var athra = new AthraLinkedTitle();
            Assert.IsFalse (athra.Verify (false));

            athra = _GetAthraLinkedTitle();
            Assert.IsTrue (athra.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void AthraLinkedTitle_ToXml_1()
        {
            var athra = new AthraLinkedTitle();
            Assert.AreEqual ("<related />", XmlUtility.SerializeShort (athra));

            athra = _GetAthraLinkedTitle();
            Assert.AreEqual ("<related><surname>Иванов</surname><initials>И. И.</initials><extension>Иван Иванович</extension><dates>1991-</dates><relatedRecord>84/И 20-425445</relatedRecord></related>",
                XmlUtility.SerializeShort (athra));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void AthraLinkedTitle_ToJson_1()
        {
            var athra = new AthraLinkedTitle();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (athra));

            athra = _GetAthraLinkedTitle();
            var expected = "{\"surname\":\"\\u0418\\u0432\\u0430\\u043D\\u043E\\u0432\",\"initials\":\"\\u0418. \\u0418.\",\"extension\":\"\\u0418\\u0432\\u0430\\u043D \\u0418\\u0432\\u0430\\u043D\\u043E\\u0432\\u0438\\u0447\",\"dates\":\"1991-\",\"relatedRecord\":\"84/\\u0418 20-425445\"}";
            var actual = JsonUtility.SerializeShort (athra);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void AthraLinkedTitle_ToString_1()
        {
            var athra = new AthraLinkedTitle();
            Assert.AreEqual ("(null)", athra.ToString());

            athra = _GetAthraLinkedTitle();
            Assert.AreEqual ("Иванов", athra.ToString());
        }

    }
}
