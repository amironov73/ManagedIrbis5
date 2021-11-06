// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

using System;

using AM;
using AM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Fields
{
    [TestClass]
    public class FieldCollectionTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void FieldCollection_Constructor_1()
        {
            var collection = new FieldCollection();
            Assert.IsNull (collection.Record);
            Assert.AreEqual (0, collection.Count);
            Assert.IsFalse (collection.ReadOnly);
        }

        [TestMethod]
        [Description ("Добавление одного элемента в конец списка")]
        public void FieldCollection_Add_1()
        {
            var collection = new FieldCollection();
            var field1001 = new Field (100);
            var field1002 = new Field (100);
            var field2001 = new Field (200);
            var field2002 = new Field (200);
            collection.Add (field1001);
            collection.Add (field1002);
            collection.Add (field2001);
            collection.Add (field2002);
            Assert.IsNull (collection.Record);
            Assert.AreEqual (4, collection.Count);
            Assert.IsFalse (collection.ReadOnly);
            Assert.AreEqual (1, field1001.Repeat);
            Assert.AreEqual (2, field1002.Repeat);
            Assert.AreEqual (1, field2001.Repeat);
            Assert.AreEqual (2, field1002.Repeat);
        }

        [TestMethod]
        [Description ("Увеличение емкости списка")]
        public void FieldCollection_AddCapacity_1()
        {
            var collection = new FieldCollection();
            collection.AddCapacity (100);
            Assert.IsNull (collection.Record);
            Assert.AreEqual (0, collection.Count);
            Assert.IsFalse (collection.ReadOnly);
        }

        [TestMethod]
        [Description ("Добавление нескольких полей в конец списка")]
        public void FieldCollection_AddRange_1()
        {
            var collection = new FieldCollection();
            collection.AddRange (new Field[] { new (100), new (200), new (300) });
            Assert.IsNull (collection.Record);
            Assert.AreEqual (3, collection.Count);
            Assert.IsFalse (collection.ReadOnly);
        }

        [TestMethod]
        [Description ("Применение значения поля: поле добавляется")]
        public void FieldCollection_ApplyFieldValue_1()
        {
            const string value = "Value";
            var collection = new FieldCollection();
            Assert.AreSame (collection, collection.ApplyFieldValue (100, value));
            Assert.AreEqual (1, collection.Count);
            Assert.AreEqual (100, collection[0].Tag);
            Assert.AreEqual (value, collection[0].Value);
        }

        [TestMethod]
        [Description ("Применение значения поля: поле изменяется")]
        public void FieldCollection_ApplyFieldValue_2()
        {
            const string newValue = "New Value";
            var collection = new FieldCollection
            {
                new (100, "Value100"),
                new (200, "Value200"),
                new (300, "Value300")
            };
            Assert.AreSame (collection, collection.ApplyFieldValue (100, newValue));
            Assert.AreEqual (3, collection.Count);
            Assert.AreEqual (100, collection[0].Tag);
            Assert.AreEqual (newValue, collection[0].Value);
        }

        [TestMethod]
        [Description ("Применение значения поля: поле удаляется")]
        public void FieldCollection_ApplyFieldValue_3()
        {
            var collection = new FieldCollection
            {
                new (100, "Value100"),
                new (200, "Value200"),
                new (300, "Value300")
            };
            Assert.AreSame (collection, collection.ApplyFieldValue (100, null));
            Assert.AreEqual (2, collection.Count);
            Assert.AreEqual (200, collection[0].Tag);
            Assert.AreEqual ("Value200", collection[0].Value);
        }

        [TestMethod]
        [Description ("Получение версии списка только для чтения")]
        public void FieldCollection_AsReadOnly_1()
        {
            var originalCollection = new FieldCollection();
            originalCollection.Add (new Field (100));
            originalCollection.Add (new Field (200));
            originalCollection.Add (new Field (300));
            var readOnlyCollection = originalCollection.AsReadOnly();
            Assert.IsNull (readOnlyCollection.Record);
            Assert.IsTrue (readOnlyCollection.ReadOnly);
            Assert.AreEqual (originalCollection.Count, readOnlyCollection.Count);
        }

        [TestMethod]
        [Description ("Приостановка перенумерации повторений полей")]
        public void FieldCollection_BeginUpdate_1()
        {
            var collection = new FieldCollection();
            collection.BeginUpdate();
            var field1001 = new Field (100);
            var field1002 = new Field (100);
            var field2001 = new Field (200);
            var field2002 = new Field (200);
            collection.Add (field1001);
            collection.Add (field1002);
            collection.Add (field2001);
            collection.Add (field2002);
            Assert.AreEqual (0, field1001.Repeat);
            Assert.AreEqual (0, field1002.Repeat);
            Assert.AreEqual (0, field2001.Repeat);
            Assert.AreEqual (0, field1002.Repeat);

            collection.EndUpdate();
            Assert.AreEqual (1, field1001.Repeat);
            Assert.AreEqual (2, field1002.Repeat);
            Assert.AreEqual (1, field2001.Repeat);
            Assert.AreEqual (2, field1002.Repeat);
        }

        [TestMethod]
        [Description ("Очистка списка")]
        public void FieldCollection_ClearItems_1()
        {
            var collection = new FieldCollection ();
            var field1001 = new Field (100);
            var field1002 = new Field (100);
            var field2001 = new Field (200);
            var field2002 = new Field (200);
            collection.AddRange (new [] {field1001, field1002, field2001, field2002});
            collection.Clear();
            Assert.AreEqual (0, collection.Count);
        }

        private void _TestSerialization
            (
                FieldCollection first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<FieldCollection>();

            Assert.IsNotNull (second);
            Assert.AreEqual (first.Count, second.Count);
            for (var i = 0; i < first.Count; i++)
            {
                Assert.AreSame (first[i].Record, second[i].Record);
                Assert.AreEqual (first[i].Tag, second[i].Tag);
                Assert.AreEqual (first[i].Repeat, second[i].Repeat);
                Assert.AreEqual (first[i].Value, second[i].Value);
            }
        }

        [TestMethod]
        [Description ("Проверка ручной сериализации")]
        public void FieldCollection_Serialization_1()
        {
            var collection = new FieldCollection();
            _TestSerialization (collection);

            var field1001 = new Field (100);
            var field1002 = new Field (100);
            var field2001 = new Field (200);
            var field2002 = new Field (200);
            collection.AddRange (new [] {field1001, field1002, field2001, field2002});
            _TestSerialization (collection);
        }

        [TestMethod]
        [Description ("Присвоение элемента коллекции")]
        public void FieldCollection_SetItem_1()
        {
            var collection = new FieldCollection();
            var field1001 = new Field (100);
            var field1002 = new Field (100);
            var field2001 = new Field (200);
            var field2002 = new Field (200);
            collection.AddRange (new [] {field1001, field1002, field2001, field2002});
            var newField = new Field (1000);
            collection[1] = newField;
            Assert.AreEqual (4, collection.Count);
            Assert.AreSame (newField, collection[1]);
        }

        [TestMethod]
        [Description ("Присвоение элемента коллекции: read-only")]
        [ExpectedException (typeof (ReadOnlyException))]
        public void FieldCollection_SetItem_2()
        {
            var collection = new FieldCollection();
            var field1001 = new Field (100);
            var field1002 = new Field (100);
            var field2001 = new Field (200);
            var field2002 = new Field (200);
            collection.AddRange (new [] {field1001, field1002, field2001, field2002});
            var readOnly = collection.AsReadOnly();
            var newField = new Field (1000);
            readOnly[1] = newField;
        }

        [TestMethod]
        [Description ("Присвоение элемента коллекции: null")]
        [ExpectedException (typeof (ArgumentNullException))]
        public void FieldCollection_SetItem_3()
        {
            var collection = new FieldCollection();
            var field1001 = new Field (100);
            var field1002 = new Field (100);
            var field2001 = new Field (200);
            var field2002 = new Field (200);
            collection.AddRange (new [] {field1001, field1002, field2001, field2002});
            collection.Add (null!);
        }

        [TestMethod]
        [Description ("Отслеживание статуса измененной записи")]
        public void FieldCollection_SetModified_1()
        {
            var record = new Record();
            var collection = record.Fields;
            Assert.IsFalse (record.Modified);
            collection.Add (new Field(100));
            Assert.IsTrue (record.Modified);
        }

        [TestMethod]
        [Description ("Отслеживание статуса измененной записи")]
        public void FieldCollection_SetModified_2()
        {
            var record = new Record();
            var collection = record.Fields;
            Assert.IsFalse (record.Modified);
            collection.Add (new Field(100));
            record.NotModified();
            Assert.IsFalse (record.Modified);
            collection[0] = new Field (200);
            Assert.IsTrue (record.Modified);
        }

        [TestMethod]
        [Description ("Отслеживание статуса измененной записи")]
        public void FieldCollection_SetModified_3()
        {
            var record = new Record();
            var collection = record.Fields;
            Assert.IsFalse (record.Modified);
            collection.Add (new Field(100));
            record.NotModified();
            Assert.IsFalse (record.Modified);
            collection.Clear();
            Assert.IsTrue (record.Modified);
        }

        [TestMethod]
        [Description ("Отслеживание статуса измененной записи")]
        public void FieldCollection_SetModified_4()
        {
            var record = new Record();
            var collection = record.Fields;
            Assert.IsFalse (record.Modified);
            collection.Add (new Field(100));
            record.NotModified();
            Assert.IsFalse (record.Modified);
            collection.RemoveAt (0);
            Assert.IsTrue (record.Modified);
        }

        [TestMethod]
        [Description ("Получение текстового представления коллекции")]
        public void FieldCollection_ToString_1()
        {
            // ReSharper disable CollectionNeverUpdated.Local
            var collection = new FieldCollection();
            // ReSharper restore CollectionNeverUpdated.Local
            Assert.AreEqual (string.Empty, collection.ToString());
        }

        [TestMethod]
        [Description ("Получение текстового представления коллекции")]
        public void FieldCollection_ToString_2()
        {
            var collection = new FieldCollection();
            collection.Add (new Field(100, "Value100"));
            Assert.AreEqual
                (
                    "100#Value100",
                    collection.ToString()
                );
        }

        [TestMethod]
        [Description ("Получение текстового представления коллекции")]
        public void FieldCollection_ToString_3()
        {
            var newLine = Environment.NewLine;
            var collection = new FieldCollection();
            collection.Add (new Field(100, "Value100"));
            collection.Add (new Field(200, "Value200"));
            Assert.AreEqual
                (
                    $"100#Value100{newLine}200#Value200",
                    collection.ToString()
                );
        }
    }
}
