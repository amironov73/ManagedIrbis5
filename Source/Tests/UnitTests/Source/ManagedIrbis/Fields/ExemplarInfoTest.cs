// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class ExemplarInfoTest
        : Common.CommonUnitTest
    {
        private Field _GetField()
        {
            return new Field (910, "^a5^b1700001^c20120806^dФ403^hE00401004A40BC3D^u2012/72^y72^fМОЭ^s20160921^!Ф403");
        }

        private Record _GetRecord()
        {
            return new Record()
                .Add (910, "^a5^b1700001^c20120806^dФ403^hE00401004A40BC3D^u2012/72^y72^fМОЭ^s20160921^!Ф403")
                .Add (910, "a0^b1700002^dФ303^hE00401004DDC4B6F^u2012/72^y72^c20120828^!Ф303^s20160321")
                .Add (910, "^a0^b1700003^dФКХ^hE00401004C7C89B2^u2012/72^y72^c20120828");
        }

        private ExemplarInfo _GetExemplar1()
        {
            return new ()
            {
                Status = "5",
                Number = "1700001",
                Date = "20120806",
                Place = "Ф403",
                Barcode = "E00401004A40BC3D",
                KsuNumber1 = "2012/72",
                ActNumber1 = "72",
                Channel = "МОЭ",
                CheckedDate = "20160921",
                RealPlace = "Ф403"
            };
        }

        private ExemplarInfo _GetExemplar2()
        {
            return new ()
            {
                Status = "0",
                Number = "1700002",
                Date = "20120806",
                Place = "Ф303",
                Barcode = "E00401004DDC4B6F",
                KsuNumber1 = "2012/72",
                ActNumber1 = "72",
                Channel = "МОЭ",
                CheckedDate = "20160321",
                RealPlace = "Ф303"
            };
        }

        private ExemplarInfo _GetExemplar3()
        {
            return new ()
            {
                Status = "U",
                Date = "20120806",
                Place = "Ф302",
                KsuNumber1 = "2012/72",
                ActNumber1 = "72",
                Channel = "МОЭ",
                Amount = "33",
                OnHand = "11"
            };
        }

        private void _Compare
            (
                ExemplarInfo first,
                ExemplarInfo second
            )
        {
            Assert.AreEqual (first.Status, second.Status);
            Assert.AreEqual (first.Number, second.Number);
            Assert.AreEqual (first.Date, second.Date);
            Assert.AreEqual (first.Place, second.Place);
            Assert.AreEqual (first.Collection, second.Collection);
            Assert.AreEqual (first.ShelfIndex, second.ShelfIndex);
            Assert.AreEqual (first.Price, second.Price);
            Assert.AreEqual (first.Barcode, second.Barcode);
            Assert.AreEqual (first.Amount, second.Amount);
            Assert.AreEqual (first.Purpose, second.Purpose);
            Assert.AreEqual (first.Coefficient, second.Coefficient);
            Assert.AreEqual (first.OffBalance, second.OffBalance);
            Assert.AreEqual (first.KsuNumber1, second.KsuNumber1);
            Assert.AreEqual (first.ActNumber1, second.ActNumber1);
            Assert.AreEqual (first.Channel, second.Channel);
            Assert.AreEqual (first.OnHand, second.OnHand);
            Assert.AreEqual (first.ActNumber2, second.ActNumber2);
            Assert.AreEqual (first.WriteOff, second.WriteOff);
            Assert.AreEqual (first.Completion, second.Completion);
            Assert.AreEqual (first.ActNumber3, second.ActNumber3);
            Assert.AreEqual (first.Moving, second.Moving);
            Assert.AreEqual (first.NewPlace, second.NewPlace);
            Assert.AreEqual (first.CheckedDate, second.CheckedDate);
            Assert.AreEqual (first.CheckedAmount, second.CheckedAmount);
            Assert.AreEqual (first.RealPlace, second.RealPlace);
            Assert.AreEqual (first.BindingIndex, second.BindingIndex);
            Assert.AreEqual (first.BindingNumber, second.BindingNumber);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ExemplarInfo_Construction_1()
        {
            var exemplar = new ExemplarInfo();
            Assert.AreEqual (0, exemplar.Id);
            Assert.IsNull (exemplar.Status);
            Assert.IsNull (exemplar.Number);
            Assert.IsNull (exemplar.Date);
            Assert.IsNull (exemplar.Place);
            Assert.IsNull (exemplar.Collection);
            Assert.IsNull (exemplar.ShelfIndex);
            Assert.IsNull (exemplar.Price);
            Assert.IsNull (exemplar.Barcode);
            Assert.IsNull (exemplar.Amount);
            Assert.IsNull (exemplar.Purpose);
            Assert.IsNull (exemplar.Coefficient);
            Assert.IsNull (exemplar.OffBalance);
            Assert.IsNull (exemplar.KsuNumber1);
            Assert.IsNull (exemplar.ActNumber1);
            Assert.IsNull (exemplar.Channel);
            Assert.IsNull (exemplar.OnHand);
            Assert.IsNull (exemplar.ActNumber2);
            Assert.IsNull (exemplar.WriteOff);
            Assert.IsNull (exemplar.Completion);
            Assert.IsNull (exemplar.ActNumber3);
            Assert.IsNull (exemplar.Moving);
            Assert.IsNull (exemplar.NewPlace);
            Assert.IsNull (exemplar.CheckedDate);
            Assert.IsNull (exemplar.CheckedAmount);
            Assert.IsNull (exemplar.RealPlace);
            Assert.IsNull (exemplar.BindingIndex);
            Assert.IsNull (exemplar.BindingNumber);
            Assert.IsNull (exemplar.Year);
            Assert.IsNull (exemplar.OtherSubFields);
            Assert.AreEqual (0, exemplar.Mfn);
            Assert.IsNull (exemplar.Description);
            Assert.IsNull (exemplar.Issue);
            Assert.AreEqual (0, exemplar.SequentialNumber);
            Assert.IsNull (exemplar.OrderingData);
            Assert.IsFalse (exemplar.Marked);
            Assert.IsNull (exemplar.Record);
            Assert.IsNull (exemplar.Field);
            Assert.IsNull (exemplar.UserData);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void InventoryInfo_Construction_2()
        {
            var exemplar = new ExemplarInfo ()
            {
                Status = "5",
                Number = "1700001",
                Date = "20120806",
                Place = "Ф403",
                Barcode = "E00401004A40BC3D",
                KsuNumber1 = "2012/72",
                ActNumber1 = "72",
                Channel = "МОЭ",
                CheckedDate = "20160921",
                RealPlace = "Ф403",
                SequentialNumber = 123,
                Marked = true,
                Record = new Record(),
                Field = new Field(),
                UserData = "User data"
            };
            Assert.AreEqual ("5", exemplar.Status);
            Assert.AreEqual ("1700001", exemplar.Number);
            Assert.AreEqual ("20120806", exemplar.Date);
            Assert.AreEqual ("Ф403", exemplar.Place);
            Assert.IsNull (exemplar.Collection);
            Assert.IsNull (exemplar.ShelfIndex);
            Assert.IsNull (exemplar.Price);
            Assert.AreEqual ("E00401004A40BC3D", exemplar.Barcode);
            Assert.IsNull (exemplar.Amount);
            Assert.IsNull (exemplar.Purpose);
            Assert.IsNull (exemplar.Coefficient);
            Assert.IsNull (exemplar.OffBalance);
            Assert.AreEqual ("2012/72", exemplar.KsuNumber1);
            Assert.AreEqual ("72", exemplar.ActNumber1);
            Assert.AreEqual ("МОЭ", exemplar.Channel);
            Assert.IsNull (exemplar.OnHand);
            Assert.IsNull (exemplar.ActNumber2);
            Assert.IsNull (exemplar.WriteOff);
            Assert.IsNull (exemplar.Completion);
            Assert.IsNull (exemplar.ActNumber3);
            Assert.IsNull (exemplar.Moving);
            Assert.IsNull (exemplar.NewPlace);
            Assert.AreEqual ("20160921", exemplar.CheckedDate);
            Assert.IsNull (exemplar.CheckedAmount);
            Assert.AreEqual ("Ф403", exemplar.RealPlace);
            Assert.IsNull (exemplar.BindingIndex);
            Assert.IsNull (exemplar.BindingNumber);
            Assert.IsNull (exemplar.Year);
            Assert.IsNull (exemplar.OtherSubFields);
            Assert.AreEqual (0, exemplar.Mfn);
            Assert.IsNull (exemplar.Description);
            Assert.IsNull (exemplar.Issue);
            Assert.AreEqual (123, exemplar.SequentialNumber);
            Assert.IsNull (exemplar.OrderingData);
            Assert.IsTrue (exemplar.Marked);
            Assert.IsNotNull (exemplar.Record);
            Assert.IsNotNull (exemplar.Field);
            Assert.AreEqual ("User data", exemplar.UserData);
        }

        [TestMethod]
        [Description ("Применение данных к полю библиографической записи")]
        public void ExemplarInfo_ApplyToField_1()
        {
            var exemplar = _GetExemplar1();
            var expected = _GetField();
            var actual = new Field (ExemplarInfo.ExemplarTag);
            exemplar.ApplyToField (actual);
            CompareFields (expected, actual);
        }

        [TestMethod]
        [Description ("Вычисление количества свободных экземпляров")]
        public void ExemplarInfo_GetFreeCount_1()
        {
            var exemplar = _GetExemplar1();
            Assert.AreEqual (0, exemplar.GetFreeCount());

            exemplar = _GetExemplar2();
            Assert.AreEqual (1, exemplar.GetFreeCount());
        }

        [TestMethod]
        [Description ("Вычисление количества свободных экземпляров")]
        public void ExemplarInfo_GetFreeCount_2()
        {
            var exemplar = _GetExemplar3();
            Assert.AreEqual (22, exemplar.GetFreeCount());
        }

        [TestMethod]
        [Description ("Вычисление общего количества экземпляров")]
        public void ExemplarInfo_GetTotalCount_1()
        {
            var exemplar = _GetExemplar1();
            Assert.AreEqual (1, exemplar.GetTotalCount());

            exemplar = _GetExemplar2();
            Assert.AreEqual (1, exemplar.GetTotalCount());
        }

        [TestMethod]
        [Description ("Вычисление общего количества экземпляров")]
        public void ExemplarInfo_GetTotalCount_2()
        {
            var exemplar = _GetExemplar3();
            Assert.AreEqual (33, exemplar.GetTotalCount());

            exemplar.Status = "6"; // списанный экземпляр
            Assert.AreEqual (0, exemplar.GetTotalCount());
        }

        [TestMethod]
        [Description ("Сравнение инвентарных номеров экземпляров")]
        public void ExemplarInfo_CompareNumbers_1()
        {
            var first = _GetExemplar1();
            var second = _GetExemplar2();

            Assert.IsTrue (ExemplarInfo.CompareNumbers (first, second) < 0);
            Assert.IsTrue (ExemplarInfo.CompareNumbers (second, first) > 0);
            Assert.IsTrue (ExemplarInfo.CompareNumbers (first, first) == 0);
            Assert.IsTrue (ExemplarInfo.CompareNumbers (second, second) == 0);
        }

        [TestMethod]
        [Description ("Сравнение инвентарных номеров экземпляров")]
        public void ExemplarInfo_CompareNumbers_2()
        {
            var first = _GetExemplar1();
            var second = _GetExemplar3();

            Assert.IsTrue (ExemplarInfo.CompareNumbers (first, second) > 0);
            Assert.IsTrue (ExemplarInfo.CompareNumbers (second, first) < 0);
            Assert.IsTrue (ExemplarInfo.CompareNumbers (first, first) == 0);
            Assert.IsTrue (ExemplarInfo.CompareNumbers (second, second) == 0);
        }

        [TestMethod]
        [Description ("Разбор поля библиографической записи")]
        public void ExemplarInfo_ParseField_1()
        {
            var field = _GetField();
            var expected = _GetExemplar1();
            var actual = ExemplarInfo.ParseField (field);
            _Compare (expected, actual);
        }

        [TestMethod]
        [Description ("Разбор библиографической записи")]
        public void ExemplarInfo_ParseRecord_1()
        {
            var record = _GetRecord();
            var exemplars = ExemplarInfo.ParseRecord (record);
            Assert.AreEqual (record.Fields.Count, exemplars.Length);
        }

        [TestMethod]
        [Description ("Преобразование данных в поле библиографической записи")]
        public void ExemplarInfo_ToField_1()
        {
            var exemplar = _GetExemplar1();
            var expected = _GetField();
            var actual = exemplar.ToField();
            CompareFields (expected, actual);
        }

        private void _TestSerialization
            (
                ExemplarInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<ExemplarInfo>();
            Assert.IsNotNull (second);
            _Compare (first, second);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void ExemplarInfo_Serialization_1()
        {
            var exemplar = new ExemplarInfo();
            _TestSerialization (exemplar);

            exemplar = _GetExemplar1();
            _TestSerialization (exemplar);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void ExemplarInfo_Verification_1()
        {
            var exemplar = new ExemplarInfo();
            Assert.IsFalse (exemplar.Verify (false));

            exemplar = _GetExemplar1();
            Assert.IsTrue (exemplar.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void ExemplarInfo_ToXml_1()
        {
            var exemplar = new ExemplarInfo();
            Assert.AreEqual
                (
                    "<exemplar mfn=\"0\" marked=\"false\" />",
                    XmlUtility.SerializeShort (exemplar)
                );

            exemplar = _GetExemplar1();
            Assert.AreEqual
                (
                    "<exemplar status=\"5\" number=\"1700001\" date=\"20120806\" place=\"Ф403\" barcode=\"E00401004A40BC3D\" ksu-number1=\"2012/72\" act-number1=\"72\" channel=\"МОЭ\" checked-date=\"20160921\" real-place=\"Ф403\" mfn=\"0\" marked=\"false\" />",
                    XmlUtility.SerializeShort (exemplar)
                );
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void ExemplarInfo_ToJson_1()
        {
            var exemplar = new ExemplarInfo();
            Assert.AreEqual
                (
                    "{\"mfn\":0,\"marked\":false}",
                    JsonUtility.SerializeShort (exemplar)
                );

            exemplar = _GetExemplar1();
            Assert.AreEqual
                (
                    "{\"status\":\"5\",\"number\":\"1700001\",\"date\":\"20120806\",\"place\":\"\\u0424403\",\"barcode\":\"E00401004A40BC3D\",\"ksu-number1\":\"2012/72\",\"act-number1\":\"72\",\"channel\":\"\\u041C\\u041E\\u042D\",\"checked-date\":\"20160921\",\"real-place\":\"\\u0424403\",\"mfn\":0,\"marked\":false}",
                    JsonUtility.SerializeShort (exemplar)
                );
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void ExemplarInfo_ToString_1()
        {
            var exemplar = new ExemplarInfo();
            Assert.AreEqual
                (
                    "(null) ((null)) [(null)]",
                    exemplar.ToString()
                );

            exemplar = _GetExemplar1();
            Assert.AreEqual
                (
                    "1700001 (Ф403) [5]",
                    exemplar.ToString()
                );

            exemplar.BindingNumber = "П123";
            Assert.AreEqual
                (
                    "1700001 (Ф403) [5] <binding П123>",
                    exemplar.ToString()
                );
        }
    }
}
