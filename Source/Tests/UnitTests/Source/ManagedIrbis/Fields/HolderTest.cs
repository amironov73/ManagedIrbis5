// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class HolderTest
        : Common.CommonUnitTest
    {
        private Holder _GetHolder() => new ()
        {
            Organization = "ГПНТБ России",
            Address = "123298, Москва, 3-я Хорошевская ул., д.17",
            Communication = "gpntb@gpntb.ru",
            Sigla = "10010033"
        };

        private Field _GetField() => new Field (Holder.Tag)
            .Add ('a', "ГПНТБ России")
            .Add ('b', "123298, Москва, 3-я Хорошевская ул., д.17")
            .Add ('d', "gpntb@gpntb.ru")
            .Add ('s', "10010033");

        private void _Compare
            (
                Holder first,
                Holder second
            )
        {
            Assert.AreEqual (first.Organization, second.Organization);
            Assert.AreEqual (first.Address, second.Address);
            Assert.AreEqual (first.Communication, second.Communication);
            Assert.AreEqual (first.Sigla, second.Sigla);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Holder_Constructor_1()
        {
            var holder = new Holder();
            Assert.IsNull (holder.Organization);
            Assert.IsNull (holder.Address);
            Assert.IsNull (holder.Communication);
            Assert.IsNull (holder.Sigla);
            Assert.IsNull (holder.UnknownSubFields);
            Assert.IsNull (holder.Field);
            Assert.IsNull (holder.UserData);
        }

        private void _TestSerialization
            (
                Holder first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<Holder>();
            Assert.IsNotNull (second);
            _Compare (first, second);
            Assert.AreSame (first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void Holder_Serialization_1()
        {
            var holder = new Holder();
            _TestSerialization (holder);

            holder.UserData = "User data";
            _TestSerialization (holder);

            holder = _GetHolder();
            _TestSerialization (holder);
        }

        [TestMethod]
        [Description ("Разбор поля записи")]
        public void Holder_ParseField_1()
        {
            var expected = _GetHolder();
            var field = _GetField();
            var actual = Holder.ParseField (field);
            Assert.AreSame (field, actual.Field);
            _Compare (expected, actual);
            Assert.IsNotNull (actual.UnknownSubFields);
            Assert.AreEqual (0, actual.UnknownSubFields!.Length);
            Assert.IsNull (actual.UserData);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void Holder_ParseRecord_1()
        {
            var record = new Record();
            var field = _GetField();
            record.Fields.Add (field);
            var holders = Holder.ParseRecord (record);
            Assert.AreEqual (1, holders.Length);
            Assert.AreSame (field, holders[0].Field);

            Assert.IsNotNull (holders[0].UnknownSubFields);
            Assert.AreEqual (0, holders[0].UnknownSubFields!.Length);
            Assert.IsNull (holders[0].UserData);
        }

        [TestMethod]
        [Description ("Создание поля записи по информации о держателе")]
        public void Holder_ToField_1()
        {
            var expected = _GetField();
            var holder = _GetHolder();
            var actual = holder.ToField();
            Assert.AreEqual (Holder.Tag, actual.Tag);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Применение информации к полю записи")]
        public void Holder_ApplyToField_1()
        {
            var expected = _GetField();
            var actual = new Field (Holder.Tag)
                .Add ('a', "???")
                .Add ('b', "???");
            var holder = _GetHolder();
            holder.ApplyToField (actual);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Верификация данных о держателе документа")]
        public void Holder_Verify_1()
        {
            var holder = new Holder();
            Assert.IsFalse (holder.Verify (false));

            holder = _GetHolder();
            Assert.IsTrue (holder.Verify (false));
        }

        [TestMethod]
        [Description ("Получение XML-представления")]
        public void Holder_ToXml_1()
        {
            var holder = new Holder();
            Assert.AreEqual ("<holder />", XmlUtility.SerializeShort (holder));

            holder = _GetHolder();
            Assert.AreEqual ("<holder><organization>ГПНТБ России</organization><address>123298, Москва, 3-я Хорошевская ул., д.17</address><communication>gpntb@gpntb.ru</communication><sigla>10010033</sigla></holder>",
                XmlUtility.SerializeShort (holder));
        }

        [TestMethod]
        [Description ("Получение JSON-представления")]
        public void Holder_ToJson_1()
        {
            var holder = new Holder();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (holder));

            holder = _GetHolder();
            Assert.AreEqual ("{\"organization\":\"\\u0413\\u041F\\u041D\\u0422\\u0411 \\u0420\\u043E\\u0441\\u0441\\u0438\\u0438\",\"address\":\"123298, \\u041C\\u043E\\u0441\\u043A\\u0432\\u0430, 3-\\u044F \\u0425\\u043E\\u0440\\u043E\\u0448\\u0435\\u0432\\u0441\\u043A\\u0430\\u044F \\u0443\\u043B., \\u0434.17\",\"communication\":\"gpntb@gpntb.ru\",\"sigla\":\"10010033\"}",
                JsonUtility.SerializeShort (holder));
        }

        [TestMethod]
        [Description ("Получение текстового представления")]
        public void Holder_ToString_1()
        {
            var holder = new Holder();
            Assert.AreEqual
                (
                    "(null)",
                    holder.ToString().DosToUnix()
                );

            holder = _GetHolder();
            Assert.AreEqual
                (
                    "ГПНТБ России, 123298, Москва, 3-я Хорошевская ул., д.17, gpntb@gpntb.ru, 10010033",
                    holder.ToString().DosToUnix()
                );
        }
    }
}
