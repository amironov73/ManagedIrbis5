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
    public sealed class ContentTypeTest
        : Common.CommonUnitTest
    {
        private ContentType _GetContentType() => new ()
        {
            ContentKind = "i", // текст
            DegreeOfApplicability = "4", // полностью
            DimensionSpecification = "2", // двумерный
            SensorySpecification = "e" // визуальный
        };

        private Field _GetField() => new (ContentType.Tag, "^ai^b4^e2^fe");

        private void _Compare
            (
                ContentType first,
                ContentType second
            )
        {
            Assert.AreEqual (first.ContentKind, second.ContentKind);
            Assert.AreEqual (first.DegreeOfApplicability, second.DegreeOfApplicability);
            Assert.AreEqual (first.TypeSpecification, second.TypeSpecification);
            Assert.AreEqual (first.MovementSpecification, second.MovementSpecification);
            Assert.AreEqual (first.DimensionSpecification, second.DimensionSpecification);
            Assert.AreEqual (first.SensorySpecification, second.SensorySpecification);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ContentType_Constructor_1()
        {
            var contentType = new ContentType();
            Assert.IsNull (contentType.ContentKind);
            Assert.IsNull (contentType.DegreeOfApplicability);
            Assert.IsNull (contentType.TypeSpecification);
            Assert.IsNull (contentType.MovementSpecification);
            Assert.IsNull (contentType.DimensionSpecification);
            Assert.IsNull (contentType.SensorySpecification);
            Assert.IsNull (contentType.UnknownSubFields);
            Assert.IsNull (contentType.Field);
            Assert.IsNull (contentType.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к полю библиографической записи")]
        public void ContentType_ApplyTo_1()
        {
            var actual = new Field()
                .Add ('a', "o")
                .Add ('b', "03")
                .Add ('e', "111")
                .Add ('f', "91");
            var codes = _GetContentType();
            codes.ApplyToField (actual);
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void ContentType_ParseField_1()
        {
            var field = _GetField();
            var actual = ContentType.ParseField (field);
            var expected = _GetContentType();
            _Compare (expected, actual);
        }

        [TestMethod]
        [Description ("Преобразование в поле библиографической записи")]
        public void ContentType_ToField_1()
        {
            var codes = _GetContentType();
            var actual = codes.ToField();
            var expected = _GetField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                ContentType first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<ContentType>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void ContentType_Serialization_1()
        {
            var contentType = new ContentType();
            _TestSerialization (contentType);

            contentType = _GetContentType();
            contentType.Field = new Field();
            contentType.UserData = "User data";
            _TestSerialization (contentType);

        }

        [TestMethod]
        [Description ("Верификация")]
        public void ContentType_Verification_1()
        {
            var contentType = new ContentType();
            Assert.IsFalse (contentType.Verify (false));

            contentType = _GetContentType();
            Assert.IsTrue (contentType.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void ContentType_ToXml_1()
        {
            var contentType = new ContentType();
            Assert.AreEqual ("<content-type />", XmlUtility.SerializeShort (contentType));

            contentType = _GetContentType();
            Assert.AreEqual ("<content-type content-kind=\"i\" degree-of-applicability=\"4\" dimension-specification=\"2\" sensory-specification=\"e\" />",
                XmlUtility.SerializeShort (contentType));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void ContentType_ToJson_1()
        {
            var contentType = new ContentType();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (contentType));

            contentType = _GetContentType();
            Assert.AreEqual ("{\"contentKind\":\"i\",\"degreeOfApplicability\":\"4\",\"dimensionSpecification\":\"2\",\"sensorySpecification\":\"e\"}",
                JsonUtility.SerializeShort (contentType));
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void ContentType_ToString_1()
        {
            var contentType = new ContentType();
            Assert.AreEqual ("ContentKind: (null), DegreeOfApplicability: (null), TypeSpecification: (null), MovementSpecification: (null), DimensionSpecification: (null), SensorySpecification: (null)",
                contentType.ToString());

            contentType = _GetContentType();
            Assert.AreEqual ("ContentKind: i, DegreeOfApplicability: 4, TypeSpecification: (null), MovementSpecification: (null), DimensionSpecification: 2, SensorySpecification: e",
                contentType.ToString());
        }

    }
}
