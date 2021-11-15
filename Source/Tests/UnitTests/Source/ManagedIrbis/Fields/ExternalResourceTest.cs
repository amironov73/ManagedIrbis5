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
    public sealed class ExternalResourceTest
        : Common.CommonUnitTest
    {
        private Field _GetField() => new (ExternalResource.Tag, "^AПример PDF-файла.PDF^TПример внешнего объекта в виде PDF-файла - с постраничным просмотром^N14");

        private ExternalResource _GetExternalResource() => new ()
        {
            FileName = "Пример PDF-файла.PDF",
            Description = "Пример внешнего объекта в виде PDF-файла - с постраничным просмотром",
            FileCount = "14"
        };

        private void _Compare
            (
                ExternalResource first,
                ExternalResource second
            )
        {
            Assert.AreEqual (first.FileName, second.FileName);
            Assert.AreEqual (first.Url, second.Url);
            Assert.AreEqual (first.Description, second.Description);
            Assert.AreEqual (first.FileCount, second.FileCount);
            Assert.AreEqual (first.NameTemplate, second.NameTemplate);
            Assert.AreEqual (first.FileType, second.FileType);
            Assert.AreEqual (first.Textbook, second.Textbook);
            Assert.AreEqual (first.AccessLevel, second.AccessLevel);
            Assert.AreEqual (first.LanOnly, second.LanOnly);
            Assert.AreEqual (first.InputDate, second.InputDate);
            Assert.AreEqual (first.FileSize, second.FileSize);
            Assert.AreEqual (first.Number, second.Number);
            Assert.AreEqual (first.LastCheck, second.LastCheck);
            Assert.AreEqual (first.ImageSize, second.ImageSize);
            Assert.AreEqual (first.Issn, second.Issn);
            Assert.AreEqual (first.Form, second.Form);
            Assert.AreEqual (first.Provider, second.Provider);
            Assert.AreEqual (first.Price, second.Price);
            Assert.AreEqual (first.Index, second.Index);
            Assert.AreEqual (first.Remarks, second.Remarks);
            Assert.AreEqual (first.System, second.System);
            Assert.AreEqual (first.Rules, second.Rules);
            Assert.AreEqual (first.AccessMode, second.AccessMode);
            Assert.AreEqual (first.Rsu, second.Rsu);
            Assert.AreEqual (first.AccessExpirationDate, second.AccessExpirationDate);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ExternalResource_Construction_1()
        {
            var info = new ExternalResource();
            Assert.IsNull (info.FileName);
            Assert.IsNull (info.Url);
            Assert.IsNull (info.Description);
            Assert.IsNull (info.NameTemplate);
            Assert.IsNull (info.FileType);
            Assert.IsNull (info.FileCount);
            Assert.IsNull (info.Textbook);
            Assert.IsNull (info.AccessLevel);
            Assert.IsNull (info.LanOnly);
            Assert.IsNull (info.InputDate);
            Assert.IsNull (info.FileSize);
            Assert.IsNull (info.Number);
            Assert.IsNull (info.LastCheck);
            Assert.IsNull (info.ImageSize);
            Assert.IsNull (info.Issn);
            Assert.IsNull (info.Form);
            Assert.IsNull (info.Provider);
            Assert.IsNull (info.Price);
            Assert.IsNull (info.Index);
            Assert.IsNull (info.Remarks);
            Assert.IsNull (info.System);
            Assert.IsNull (info.Rules);
            Assert.IsNull (info.AccessMode);
            Assert.IsNull (info.Rsu);
            Assert.IsNull (info.AccessExpirationDate);
            Assert.IsNull (info.Field);
            Assert.IsNull (info.UnknownSubFields);
            Assert.IsNull (info.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к указанному полю записи")]
        public void ExternalResource_ApplyTo_1()
        {
            var expected = _GetField();
            var actual = new Field();
            var info = _GetExternalResource();
            info.ApplyTo (actual);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void ExternalResource_ParseField_1()
        {
            var field = _GetField();
            var actual = ExternalResource.ParseField (field);
            var expected = _GetExternalResource();
            _Compare (expected, actual);
            Assert.AreSame (field, actual.Field);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Разбор записи")]
        public void ExternalResource_ParseRecord_1()
        {
            var record = new Record();
            var field = _GetField();
            record.Add (field);
            var actual = ExternalResource.ParseRecord (record);
            var expected = _GetExternalResource();
            Assert.IsNotNull (actual);
            Assert.AreEqual (1, actual.Length);
            _Compare (expected, actual[0]);
            Assert.AreSame (field, actual[0].Field);
            Assert.IsNull (actual[0].UserData);
        }

        private void _TestSerialization
            (
                ExternalResource first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<ExternalResource>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void ExternalResource_Serialization_1()
        {
            var info = new ExternalResource();
            _TestSerialization (info);

            info = _GetExternalResource();
            info.Field = new Field();
            info.UserData = "User data";
            _TestSerialization (info);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void ExternalResource_ToField_1()
        {
            var info = _GetExternalResource();
            var expected = _GetField();
            var actual = info.ToField();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Преобразование в поле записи")]
        public void ExternalResource_ToField46_1()
        {
            var info = _GetExternalResource();
            var expected = _GetField();
            var actual = info.ToField();
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Верифцикация")]
        public void ExternalResource_Verify_1()
        {
            var info = new ExternalResource();
            Assert.IsFalse (info.Verify (false));

            info = _GetExternalResource();
            Assert.IsTrue (info.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void ExternalResource_ToXml_1()
        {
            var info = new ExternalResource();
            Assert.AreEqual ("<external />", XmlUtility.SerializeShort (info));

            info = _GetExternalResource();
            Assert.AreEqual ("<external filename=\"Пример PDF-файла.PDF\" description=\"Пример внешнего объекта в виде PDF-файла - с постраничным просмотром\" fileCount=\"14\" />",
                XmlUtility.SerializeShort (info));
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void ExternalResource_ToJson_1()
        {
            var info = new ExternalResource();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (info));

            info = _GetExternalResource();
            var expected = "{\"filename\":\"\\u041F\\u0440\\u0438\\u043C\\u0435\\u0440 PDF-\\u0444\\u0430\\u0439\\u043B\\u0430.PDF\",\"description\":\"\\u041F\\u0440\\u0438\\u043C\\u0435\\u0440 \\u0432\\u043D\\u0435\\u0448\\u043D\\u0435\\u0433\\u043E \\u043E\\u0431\\u044A\\u0435\\u043A\\u0442\\u0430 \\u0432 \\u0432\\u0438\\u0434\\u0435 PDF-\\u0444\\u0430\\u0439\\u043B\\u0430 - \\u0441 \\u043F\\u043E\\u0441\\u0442\\u0440\\u0430\\u043D\\u0438\\u0447\\u043D\\u044B\\u043C \\u043F\\u0440\\u043E\\u0441\\u043C\\u043E\\u0442\\u0440\\u043E\\u043C\",\"fileCount\":\"14\"}";
            var actual = JsonUtility.SerializeShort (info);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void ExternalResource_ToString_1()
        {
            var info = new ExternalResource();
            Assert.AreEqual ("FileName: (null), Url: (null), Description: (null)", info.ToString());

            info = _GetExternalResource();
            Assert.AreEqual ("FileName: Пример PDF-файла.PDF, Url: (null), Description: Пример внешнего объекта в виде PDF-файла - с постраничным просмотром", info.ToString());
        }

    }
}
