// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using AM.Text;

using ManagedIrbis.Lng;

#nullable enable

namespace UnitTests.ManagedIrbis.Lng
{
    [TestClass]
    public sealed class LngFileTest
        : Common.CommonUnitTest
    {
        private string _GetFileName() => Path.Combine (TestDataPath, "uk.lng");

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void LngFile_Constructor_1()
        {
            var lng = new LngFile();
            Assert.IsNull (lng.Name);
            Assert.AreEqual ("(null)", lng.ToString());
        }

        [TestMethod]
        [Description ("Добавление пары оригинал-перевод")]
        public void LngFile_Add_1()
        {
            const string original = "Алфавит";
            const string expected = "Абетка";

            var lng = new LngFile() { Name = "Тестовый" };
            Assert.IsFalse (lng.Verify (false));

            Assert.AreSame (lng, lng.Add (original, expected));
            Assert.IsTrue (lng.Verify (false));

            var actual = lng.GetTranslation (original);
            Assert.AreEqual (expected, actual);

            lng.Clear();
            Assert.IsFalse (lng.Verify (false));
            actual = lng.GetTranslation (original);
            Assert.AreEqual (original, actual);

        }

        [TestMethod]
        [Description ("Чтение из локального файла")]
        public void LngFile_ReadLocalFile_1()
        {
            var fileName = _GetFileName();
            var lng = LngFile.ReadLocalFile (fileName);
            Assert.AreEqual (fileName, lng.Name);
            Assert.IsTrue (lng.Verify (false));
            Assert.AreEqual (fileName, lng.ToString());

            const string expected = "Абетка";
            var actual = lng.GetTranslation ("Алфавит");
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Запись в локальный файл")]
        public void LngFile_WriteLocalFile_1()
        {
            var fileName = Path.GetTempFileName();
            var lng = new LngFile();
            lng.Add ("Алфавит", "Абетка");
            lng.WriteLocalFile (fileName);
            Assert.IsTrue (File.Exists (fileName));

            const string expected = "Алфавит\nАбетка\n";
            var actual = File.ReadAllText (fileName).DosToUnix();
            Assert.AreEqual (expected, actual);
        }
        private void _TestSerialization
            (
                LngFile first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<LngFile>();
            Assert.IsNotNull (second);
            Assert.AreEqual (first.Name, second.Name);
            Assert.AreEqual (first.GetTranslation ("Алфавит"),
                second.GetTranslation ("Алфавит"));
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void LngFile_Serialization_1()
        {
            var lng = new LngFile();
            _TestSerialization (lng);

            lng.Add ("Алфавит", "Абетка");
            _TestSerialization (lng);
        }

    }
}
