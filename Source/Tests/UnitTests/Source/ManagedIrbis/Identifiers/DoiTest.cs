// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace

using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Identifiers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable
namespace UnitTests.ManagedIrbis.Identifiers
{
    [TestClass]
    public sealed class DoiTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Doi_Construction_1()
        {
            var doi = new Doi();
            Assert.IsNull (doi.Prefix);
            Assert.IsNull (doi.Suffix);
            Assert.IsNull (doi.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста: самый простой случай")]
        public void Doi_ParseText_1()
        {
            var doi = new Doi();
            doi.ParseText ("10.1000/182");
            Assert.AreEqual ("10.1000", doi.Prefix);
            Assert.AreEqual ("182", doi.Suffix);
            Assert.IsNull (doi.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста: убирает пробелы")]
        public void Doi_ParseText_2()
        {
            var doi = new Doi();
            doi.ParseText (" 10.1000 / 182 ");
            Assert.AreEqual ("10.1000", doi.Prefix);
            Assert.AreEqual ("182", doi.Suffix);
            Assert.IsNull (doi.UserData);
        }

        [TestMethod]
        [Description ("Разбор текста: пустой фрагмент")]
        [ExpectedException (typeof (ArgumentOutOfRangeException))]
        public void Doi_ParseText_3()
        {
            var doi = new Doi();
            doi.ParseText (ReadOnlySpan<char>.Empty);
        }

        [TestMethod]
        [Description ("Разбор текста: без слэша")]
        [ExpectedException (typeof (ArgumentException))]
        public void Doi_ParseText_4()
        {
            var doi = new Doi();
            doi.ParseText ("10.1000.182");
        }

        [TestMethod]
        [Description ("Разбор текста: пустой префикс")]
        [ExpectedException (typeof (ArgumentException))]
        public void Doi_ParseText_5()
        {
            var doi = new Doi();
            doi.ParseText (" /182");
        }

        [TestMethod]
        [Description ("Разбор текста: пустой суффикс")]
        [ExpectedException (typeof (ArgumentException))]
        public void Doi_ParseText_6()
        {
            var doi = new Doi();
            doi.ParseText (" 10.1000/ ");
        }

        [TestMethod]
        [Description ("Разбор URL: самый простой случай")]
        public void Doi_ParseUrl_1()
        {
            var doi = new Doi();
            doi.ParseUrl ("https://doi.org/10.1000/182");
            Assert.AreEqual ("10.1000", doi.Prefix);
            Assert.AreEqual ("182", doi.Suffix);
        }

        [TestMethod]
        [Description ("Разбор URL: без протокола")]
        [ExpectedException (typeof (FormatException))]
        public void Doi_ParseUrl_2()
        {
            var doi = new Doi();
            doi.ParseUrl ("10.1000/182");
        }

        [TestMethod]
        [Description ("Разбор URL: без начального слэша")]
        [ExpectedException (typeof (ArgumentException))]
        public void Doi_ParseUrl_3()
        {
            var doi = new Doi();
            doi.ParseUrl ("https://doi.org?10.1000/182");
        }

        private Doi _GetDoi()
        {
            return new Doi() { Prefix = "10.1000", Suffix = "182" };
        }

        private void _Compare
            (
                Doi first,
                Doi second
            )
        {
            Assert.AreEqual (first.Prefix, second.Prefix);
            Assert.AreEqual (first.Suffix, second.Suffix);
        }

        private void _TestSerialization
            (
                Doi first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Doi>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Doi_Serialization_1()
        {
            var doi = new Doi();
            _TestSerialization (doi);

            doi = _GetDoi();
            doi.UserData = "User data";
            _TestSerialization (doi);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void Doi_Verify_1()
        {
            var doi = new Doi();
            Assert.IsFalse (doi.Verify (false));

            doi = _GetDoi();
            Assert.IsTrue (doi.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void Doi_ToXml_1()
        {
            var doi = new Doi();
            Assert.AreEqual ("<doi />", XmlUtility.SerializeShort (doi));

            doi = _GetDoi();
            Assert.AreEqual ("<doi prefix=\"10.1000\" suffix=\"182\" />",
                XmlUtility.SerializeShort (doi));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void Doi_ToJson_1()
        {
            var doi = new Doi();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (doi));

            doi = _GetDoi();
            var expected = "{\"prefix\":\"10.1000\",\"suffix\":\"182\"}";
            var actual = JsonUtility.SerializeShort (doi);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void Doi_ToString_1()
        {
            var doi = new Doi();
            Assert.AreEqual ("(null)/(null)", doi.ToString());

            doi = _GetDoi();
            Assert.AreEqual ("10.1000/182", doi.ToString());
        }

        [TestMethod]
        [Description ("Преобразование в URI")]
        public void Doi_ToUri_1()
        {
            var doi = _GetDoi();
            Assert.AreEqual ("https://doi.org/10.1000/182", doi.ToUri());
        }
    }
}
