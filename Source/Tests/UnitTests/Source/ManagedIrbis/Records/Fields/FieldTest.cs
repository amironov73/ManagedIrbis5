// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Fields
{
    [TestClass]
    public sealed class FieldTest
    {
        class MyClass
        {
            public string? Text { get; set; }

            public override string ToString()
            {
                return Text.ToVisibleString();
            }
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void Field_Constructor_1()
        {
            var field = new Field();
            Assert.AreEqual (0, field.Tag);
            Assert.IsTrue (field.Value.IsEmpty());
            Assert.AreEqual (0, field.Subfields.Count);
            Assert.IsTrue (field.IsEmpty);
            Assert.AreEqual (0, field.Repeat);
            Assert.IsNull (field.Record);
            Assert.IsFalse (field.ReadOnly);
        }

        [TestMethod]
        [Description ("Конструктор с меткой и значением поля")]
        public void Field_Constructor_2()
        {
            var field = new Field(100, "Значение");
            Assert.AreEqual (100, field.Tag);
            Assert.AreEqual ("Значение", field.Value);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual ("Значение", field.Subfields[0].Value);
            Assert.IsFalse (field.IsEmpty);
            Assert.AreEqual (0, field.Repeat);
            Assert.IsNull (field.Record);
            Assert.IsFalse (field.ReadOnly);
        }

        [TestMethod]
        [Description ("Конструктор с одним подполем")]
        public void Field_Constructor_3()
        {
            var field = new Field
                (
                    100,
                    new SubField ('a', "Подполе A")
                );
            Assert.AreEqual (100, field.Tag);
            Assert.IsNull (field.Value);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.IsFalse (field.IsEmpty);
            Assert.AreEqual (0, field.Repeat);
            Assert.IsNull (field.Record);
            Assert.IsFalse (field.ReadOnly);
        }

        [TestMethod]
        [Description ("Конструктор с двумя подполями")]
        public void Field_Constructor_4()
        {
            var field = new Field
                (
                    100,
                    new SubField ('a', "Подполе A"),
                    new SubField ('b', "Подполе B")
                );
            Assert.AreEqual (100, field.Tag);
            Assert.IsNull (field.Value);
            Assert.AreEqual (2, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ("Подполе B", field.Subfields[1].Value);
            Assert.IsFalse (field.IsEmpty);
            Assert.AreEqual (0, field.Repeat);
            Assert.IsNull (field.Record);
            Assert.IsFalse (field.ReadOnly);
        }

        [TestMethod]
        [Description ("Конструктор с тремя подполями")]
        public void Field_Constructor_5()
        {
            var field = new Field
                (
                    100,
                    new SubField ('a', "Подполе A"),
                    new SubField ('b', "Подполе B"),
                    new SubField ('c', "Подполе C")
                );
            Assert.AreEqual (100, field.Tag);
            Assert.IsNull (field.Value);
            Assert.AreEqual (3, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ("Подполе B", field.Subfields[1].Value);
            Assert.AreEqual ('c', field.Subfields[2].Code);
            Assert.AreEqual ("Подполе C", field.Subfields[2].Value);
            Assert.IsFalse (field.IsEmpty);
            Assert.AreEqual (0, field.Repeat);
            Assert.IsNull (field.Record);
            Assert.IsFalse (field.ReadOnly);
        }

        [TestMethod]
        [Description ("Конструктор с одним распакованным подполем")]
        public void Field_Constructor_6()
        {
            var field = new Field
                (
                    100,
                    'a', "Подполе A"
                );
            Assert.AreEqual (100, field.Tag);
            Assert.IsNull (field.Value);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.IsFalse (field.IsEmpty);
            Assert.AreEqual (0, field.Repeat);
            Assert.IsNull (field.Record);
            Assert.IsFalse (field.ReadOnly);
        }

        [TestMethod]
        [Description ("Конструктор с двумя распакованными подполями")]
        public void Field_Constructor_7()
        {
            var field = new Field
                (
                    100,
                    'a', "Подполе A",
                    'b', "Подполе B"
                );
            Assert.AreEqual (100, field.Tag);
            Assert.IsNull (field.Value);
            Assert.AreEqual (2, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ("Подполе B", field.Subfields[1].Value);
            Assert.IsFalse (field.IsEmpty);
            Assert.AreEqual (0, field.Repeat);
            Assert.IsNull (field.Record);
            Assert.IsFalse (field.ReadOnly);
        }

        [TestMethod]
        [Description ("Конструктор с тремя распакованными подполями")]
        public void Field_Constructor_8()
        {
            var field = new Field
                (
                    100,
                    'a', "Подполе A",
                    'b', "Подполе B",
                    'c', "Подполе C"
                );
            Assert.AreEqual (100, field.Tag);
            Assert.IsNull (field.Value);
            Assert.AreEqual (3, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ("Подполе B", field.Subfields[1].Value);
            Assert.AreEqual ('c', field.Subfields[2].Code);
            Assert.AreEqual ("Подполе C", field.Subfields[2].Value);
            Assert.IsFalse (field.IsEmpty);
            Assert.AreEqual (0, field.Repeat);
            Assert.IsNull (field.Record);
            Assert.IsFalse (field.ReadOnly);
        }

        [TestMethod]
        [Description ("Конструктор с массивом подполей")]
        public void Field_Constructor_9()
        {
            var field = new Field
                (
                    100,
                    new SubField ('a', "Подполе A"),
                    new SubField ('b', "Подполе B"),
                    new SubField ('c', "Подполе C"),
                    new SubField ('d', "Подполе D")
                );
            Assert.AreEqual (100, field.Tag);
            Assert.IsNull (field.Value);
            Assert.AreEqual (4, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ('c', field.Subfields[2].Code);
            Assert.AreEqual ('d', field.Subfields[3].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.AreEqual ("Подполе B", field.Subfields[1].Value);
            Assert.AreEqual ("Подполе C", field.Subfields[2].Value);
            Assert.AreEqual ("Подполе D", field.Subfields[3].Value);
            Assert.IsFalse (field.IsEmpty);
            Assert.AreEqual (0, field.Repeat);
            Assert.IsNull (field.Record);
            Assert.IsFalse (field.ReadOnly);
        }

        [TestMethod]
        [Description ("Простое добавление подполя: собственно подполе")]
        public void Field_Add_1()
        {
            var field = new Field();
            Assert.AreSame (field, field.Add (new SubField('a', "Value")));
            Assert.AreEqual (1, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Простое добавление подполя: код и значение подполя")]
        public void Field_Add_2()
        {
            var field = new Field();
            Assert.AreSame (field, field.Add ('a', "Value".AsMemory()));
            Assert.AreEqual (1, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Простое добавление подполя: код и значение подполя")]
        public void Field_Add_3()
        {
            var field = new Field();
            Assert.AreSame (field, field.Add ('a', "Value"));
            Assert.AreEqual (1, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Добавление объекта произвольного типа")]
        public void Field_Add_4()
        {
            var field = new Field();
            var obj = new MyClass() { Text = "Какой-то текст" };
            Assert.AreSame (field, field.Add ('a', obj));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Какой-то текст", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Добавление подполя при условии, что оно не пустое")]
        public void Field_AddNonEmpty_1()
        {
            var field = new Field()
                .AddNonEmpty ('a', null)
                .AddNonEmpty ('b', new MyClass { Text = "SubfieldB" })
                .AddNonEmpty ('c', string.Empty);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('b', field.Subfields[0].Code);
            Assert.AreEqual ("SubfieldB", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Добавление подполя при условии, что оно не равно 0")]
        public void Field_AddNonEmpty_2()
        {
            var field = new Field()
                .AddNonEmpty ('a', 0)
                .AddNonEmpty ('b', 1);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('b', field.Subfields[0].Code);
            Assert.AreEqual ("1", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Добавление подполя при условии, что оно не равно 0")]
        public void Field_AddNonEmpty_3()
        {
            var field = new Field()
                .AddNonEmpty ('a', 0L)
                .AddNonEmpty ('b', 1L);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('b', field.Subfields[0].Code);
            Assert.AreEqual ("1", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Добавление подполя при условии, что оно не пустое")]
        public void Field_AddNonEmpty_4()
        {
            DateTime? first = null;
            DateTime? second = new DateTime (2017, 9, 30);
            var field = new Field()
                .AddNonEmpty ('a', first)
                .AddNonEmpty ('b', second);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('b', field.Subfields[0].Code);
            Assert.AreEqual ("20170930", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Добавление подполя при условии, что оно не пустое")]
        public void Field_AddNonEmpty_5()
        {
            var first = DateTime.MinValue;
            var second = new DateTime (2017, 9, 30);
            var field = new Field()
                .AddNonEmpty ('a', first)
                .AddNonEmpty ('b', second);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('b', field.Subfields[0].Code);
            Assert.AreEqual ("20170930", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Присваивание полей: пустое поле")]
        public void Field_AssignFrom_1()
        {
            var source = new Field();
            var target = new Field { { 'z', "Подполе Z" } };
            target.AssignFrom (source);
            Assert.AreEqual (source.Subfields.Count, target.Subfields.Count);
        }

        [TestMethod]
        [Description ("Присваивание полей: поле со значением до разделителя")]
        public void Field_AssignFrom_2()
        {
            var source = new Field() { Value = "Значение" };
            var target = new Field { { 'z', "Подполе Z" } };
            target.AssignFrom (source);
            Assert.AreEqual (source.Subfields.Count, target.Subfields.Count);
            Assert.AreEqual (source.Value, target.Value);
            Assert.AreEqual (source.Subfields[0].Code, target.Subfields[0].Code);
            Assert.AreEqual (source.Subfields[0].Value, target.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Присваивание полей: поле с подполями")]
        public void Field_AssignFrom_3()
        {
            var source = new Field()
            {
                new ('a', "Подполе A"),
                new ('b', "Подполе B"),
            };
            var target = new Field { { 'z', "Подполе Z" } };
            target.AssignFrom (source);
            Assert.AreEqual (source.Subfields.Count, target.Subfields.Count);
            Assert.AreEqual (source.Value, target.Value);
            Assert.AreEqual (source.Subfields[0].Code, target.Subfields[0].Code);
            Assert.AreEqual (source.Subfields[0].Value, target.Subfields[0].Value);
            Assert.AreEqual (source.Subfields[1].Code, target.Subfields[1].Code);
            Assert.AreEqual (source.Subfields[1].Value, target.Subfields[1].Value);
        }

        [TestMethod]
        [Description ("Присваивание полей: поле со значением и с подполями")]
        public void Field_AssignFrom_4()
        {
            var source = new Field() { Value = "Значение" };
            source.Add ('a', "Подполе A");
            source.Add ('b', "Подполе B");
            var target = new Field { { 'z', "Подполе Z" } };
            target.AssignFrom (source);
            Assert.AreEqual (source.Subfields.Count, target.Subfields.Count);
            Assert.AreEqual (source.Value, target.Value);
            Assert.AreEqual (source.Subfields[0].Code, target.Subfields[0].Code);
            Assert.AreEqual (source.Subfields[0].Value, target.Subfields[0].Value);
            Assert.AreEqual (source.Subfields[1].Code, target.Subfields[1].Code);
            Assert.AreEqual (source.Subfields[1].Value, target.Subfields[1].Value);
            Assert.AreEqual (source.Subfields[2].Code, target.Subfields[2].Code);
            Assert.AreEqual (source.Subfields[2].Value, target.Subfields[2].Value);
            Assert.AreEqual (source.Value, target.Value);
        }

        [TestMethod]
        [Description ("Декодирование поля: пустое поле")]
        public void Field_Decode_1()
        {
            var field = new Field();
            field.Decode ("100#");
            Assert.AreEqual (100, field.Tag);
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Декодирование поля: неверно сформированное поле")]
        [ExpectedException (typeof (ArgumentException))]
        public void Field_Decode_2()
        {
            var field = new Field();
            field.Decode ("Bad field");
        }

        [TestMethod]
        [Description ("Декодирование поля: неверно сформированное поле")]
        [ExpectedException (typeof (ArgumentException))]
        public void Field_Decode_3()
        {
            var field = new Field();
            field.Decode ("#Bad field");
        }

        [TestMethod]
        [Description ("Декодирование поля: неверно сформированное поле")]
        [ExpectedException (typeof (FormatException))]
        public void Field_Decode_4()
        {
            var field = new Field();
            field.Decode ("A#Bad field");
        }

        [TestMethod]
        [Description ("Декодирование тела поля: пустое тело")]
        public void Field_DecodeBody_1()
        {
            var field = new Field();
            field.DecodeBody ("");
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Декодирование тела поля: только значение до разделителя")]
        public void Field_DecodeBody_2()
        {
            var field = new Field();
            field.DecodeBody ("Значение");
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ("Значение", field.Value);
        }

        [TestMethod]
        [Description ("Декодирование тела поля: только значение до разделителя")]
        public void Field_DecodeBody_3()
        {
            var field = new Field();
            field.DecodeBody ("Значение^");
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ("Значение", field.Value);
        }

        [TestMethod]
        [Description ("Декодирование тела поля: только одно подполе")]
        public void Field_DecodeBody_4()
        {
            var field = new Field();
            field.DecodeBody ("^aПодполе A");
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Декодирование тела поля: только одно подполе")]
        public void Field_DecodeBody_5()
        {
            var field = new Field();
            field.DecodeBody ("^aПодполе A^");
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Декодирование тела поля: два подполя")]
        public void Field_DecodeBody_6()
        {
            var field = new Field();
            field.DecodeBody ("^aПодполе A^bПодполе B");
            Assert.AreEqual (2, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ("Подполе B", field.Subfields[1].Value);
        }

        [TestMethod]
        [Description ("Декодирование тела поля: игнорируем лишнюю крышку")]
        public void Field_DecodeBody_7()
        {
            var field = new Field();
            field.DecodeBody ("^aПодполе A^^bПодполе B");
            Assert.AreEqual (2, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ("Подполе B", field.Subfields[1].Value);
        }

        [TestMethod]
        [Description ("Декодирование тела поля: значение и два подполя")]
        public void Field_DecodeBody_8()
        {
            var field = new Field();
            field.DecodeBody ("Значение^aПодполе A^bПодполе B");
            Assert.AreEqual (3, field.Subfields.Count);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual ("Значение", field.Subfields[0].Value);
            Assert.AreEqual ('a', field.Subfields[1].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[1].Value);
            Assert.AreEqual ('b', field.Subfields[2].Code);
            Assert.AreEqual ("Подполе B", field.Subfields[2].Value);
            Assert.AreEqual ("Значение", field.Value);
        }

        private void _TestSerialization
            (
                Field field1
            )
        {
            var bytes = field1.SaveToMemory();
            var field2 = bytes.RestoreObjectFromMemory<Field>();

            Assert.AreEqual
                (
                    0,
                    Field.Compare
                        (
                            field1,
                            field2!
                        )
                );
        }

        [TestMethod]
        [Description ("Тест сериализации")]
        public void RecordField_Serialization_1()
        {
            _TestSerialization
                (
                    new Field()
                );
            _TestSerialization
                (
                    new Field (100)
                );
            _TestSerialization
                (
                    new Field (199, "Hello")
                );
            _TestSerialization
                (
                    new Field
                        (
                            200,
                            new SubField ('a', "Hello")
                        )
                );
        }


        [TestMethod]
        [Description ("Преобразование поля в строку: пустое поле")]
        public void Field_ToString_1()
        {
            var field = new Field();
            Assert.AreEqual ("0#", field.ToString());
        }

        [TestMethod]
        [Description ("Преобразование поля в строку: поле со значением до разделителя")]
        public void Field_ToString_2()
        {
            var field = new Field (100, "Значение");
            Assert.AreEqual ("100#Значение", field.ToString());
        }

        [TestMethod]
        [Description ("Преобразование поля в строку: поле с подполями")]
        public void Field_ToString_3()
        {
            var field = new Field
                (
                    100,
                    new SubField('a', "Подполе A"),
                    new SubField('b', "Подполе B")
                );
            Assert.AreEqual ("100#^aПодполе A^bПодполе B", field.ToString());
        }

        [TestMethod]
        [Description ("Преобразование поля в строку: поле с подполями и со значением до разделителя")]
        public void Field_ToString_4()
        {
            var field = new Field (100, "Значение")
            {
                { 'a', "Подполе A" },
                { 'b', "Подполе B" }
            };
            Assert.AreEqual ("100#Значение^aПодполе A^bПодполе B", field.ToString());
        }

        [TestMethod]
        [Description ("Текстовое представление: пустое поле")]
        public void Field_ToText_1()
        {
            var field = new Field();
            Assert.AreEqual (string.Empty, field.ToText());
        }

        [TestMethod]
        [Description ("Текстовое представление: только значение до разделителя")]
        public void Field_ToText_2()
        {
            var field = new Field() { Value = "Значение" };
            Assert.AreEqual ("Значение", field.ToText());
        }

        [TestMethod]
        [Description ("Текстовое представление: два подполя")]
        public void Field_ToText_3()
        {
            var field = new Field()
            {
                new ('a', "Подполе A"),
                new ('b', "Подполе B")
            };
            Assert.AreEqual ("^aПодполе A^bПодполе B", field.ToText());
        }

        [TestMethod]
        [Description ("Текстовое представление: два подполя")]
        public void Field_ToText_4()
        {
            var field = new Field { Value = "Значение" };
            field.Add ('a', "Подполе A");
            field.Add ('b', "Подполе B");
            Assert.AreEqual ("Значение^aПодполе A^bПодполе B", field.ToText());
        }

        [TestMethod]
        [Description ("Сравнение полей: разные метки полей")]
        public void Field_Compare_1()
        {
            var first = new Field (100, "Value1");
            var second = new Field (200, "Value2");
            Assert.IsTrue (Field.Compare (first, second) < 0);
        }

        [TestMethod]
        [Description ("Сравнение полей: одинаковые метки, но разные значения до разделителя")]
        public void Field_Compare_2()
        {
            var first = new Field (100, "Value1");
            var second = new Field (100, "Value2");
            Assert.IsTrue (Field.Compare (first, second) < 0);
        }

        [TestMethod]
        [Description ("Сравнение полей: одинаковые метки, одинаковые значения, но на одно подполе меньше")]
        public void Field_Compare_3()
        {
            var first = new Field (100, "Value1");
            var second = new Field (100, "Value1")
                .Add ('a', "SubfieldA");
            Assert.IsTrue (Field.Compare (first, second) < 0);
        }

        [TestMethod]
        [Description ("Сравнение полей: одинаковые метки, одинаковые значения, но значение подполя меньше")]
        public void RecordField_Compare_4()
        {
            var first = new Field (100, "Value1")
                .Add ('a', "SubfieldA1");
            var second = new Field (100, "Value1")
                .Add ('a', "SubfieldA2");
            Assert.IsTrue (Field.Compare (first, second) < 0);
        }

        [TestMethod]
        [Description ("Сравнение полей: равенство")]
        public void Field_Compare_5()
        {
            var first = new Field (100, "Value1")
                .Add ('a', "SubfieldA");
            var second = new Field (100, "Value1")
                .Add ('a', "SubfieldA");
            Assert.IsTrue (Field.Compare (first, second) == 0);
        }

        [TestMethod]
        [Description ("Создание поля с подполями: пустое поле")]
        public void Field_WithSubFields_1()
        {
            var field = Field.WithSubFields (100);
            Assert.AreEqual (100, field.Tag);
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Создание поля с подполями: только значение до разделителя")]
        public void Field_WithSubFields_3()
        {
            var field = Field.WithSubFields (100, "\0", "Значение");
            Assert.AreEqual (100, field.Tag);
            Assert.AreEqual ("Значение", field.Value);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual ("Значение", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Создание поля с подполями: пара подполей")]
        public void Field_WithSubFields_2()
        {
            var field = Field.WithSubFields (100, "a", "Подполе A", "b", "Подполе B");
            Assert.AreEqual (100, field.Tag);
            Assert.IsNull (field.Value);
            Assert.AreEqual (2, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Подполе A", field.Subfields[0].Value);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ("Подполе B", field.Subfields[1].Value);
        }
    }
}
