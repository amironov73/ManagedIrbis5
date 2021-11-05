// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Linq;
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

        private Field _GetField()
        {
            var result = new Field (200, "Значение")
            {
                { 'a', "Заглавие" },
                { 'e', "подзаголовочные" },
                { 'f', "об ответственности" }
            };

            return result;
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
        [Description ("Добавление логического значения")]
        public void Field_Add_4()
        {
            var field = new Field();
            Assert.AreSame (field, field.Add ('a', true));
            Assert.AreSame (field, field.Add ('b', false));
            Assert.AreEqual (2, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ("1", field.Subfields[0].Value);
            Assert.AreEqual ("0", field.Subfields[1].Value);
        }

        [TestMethod]
        [Description ("Добавление объекта произвольного типа")]
        public void Field_Add_5()
        {
            var field = new Field();
            var obj = new MyClass() { Text = "Какой-то текст" };
            Assert.AreSame (field, field.Add ('a', obj));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("Какой-то текст", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Добавление объекта произвольного типа")]
        public void Field_Add_6()
        {
            const string someText = "Some text";
            var field = new Field();
            var obj = new MyClass() { Text = someText };
            Assert.AreSame (field, field.Add ('\0', obj));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual (someText, field.Subfields[0].Value);
            Assert.AreEqual (someText, field.Value);
        }

        [TestMethod]
        [Description ("Добавление подполя при условии, что оно не пустое")]
        public void Field_AddNonEmpty_1()
        {
            var field = new Field()
                .AddNonEmpty ('a', (object?) null)
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
        [Description ("Добавление подполя при условии, что оно не равно 0")]
        public void Field_AddNonEmpty_6()
        {
            int? first = 0;
            int? second = 1;
            var field = new Field()
                .AddNonEmpty ('a', first)
                .AddNonEmpty ('b', second);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('b', field.Subfields[0].Code);
            Assert.AreEqual ("1", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Добавление подполя с флагом")]
        public void Field_AddNonEmpty_7()
        {
            const string someText = "Some text";
            MyClass? value = null;
            var field = new Field();
            Assert.AreSame (field, field.AddNonEmpty ('a', false, value));
            Assert.AreEqual (0, field.Subfields.Count);

            Assert.AreSame (field, field.AddNonEmpty ('a', true, value));
            Assert.AreEqual (0, field.Subfields.Count);

            value = new MyClass { Text = someText };
            Assert.AreSame (field, field.AddNonEmpty ('a', false, value));
            Assert.AreEqual (0, field.Subfields.Count);

            Assert.AreSame (field, field.AddNonEmpty ('a', true, value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual (someText, field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Добавление подполя с флагом: значение до первого разделителя")]
        public void Field_AddNonEmpty_8()
        {
            const string someText = "Some text";
            var value = new MyClass { Text = someText };
            var field = new Field();
            Assert.AreSame (field, field.AddNonEmpty ('\0', true, value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual (someText, field.Subfields[0].Value);
            Assert.AreEqual (someText, field.Value);
        }

        [TestMethod]
        [Description ("Добавление подполя: значение до первого разделителя")]
        public void Field_AddNonEmpty_9()
        {
            const string someText = "Some text";
            var value = new MyClass { Text = someText };
            var field = new Field();
            Assert.AreSame (field, field.AddNonEmpty ('\0',  value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual (someText, field.Subfields[0].Value);
            Assert.AreEqual (someText, field.Value);
        }

        [TestMethod]
        [Description ("Добавление нескольких подполей: пустой массив")]
        public void Field_AddRange_1()
        {
            var field = new Field();
            Assert.AreSame (field, field.AddRange (Array.Empty<SubField>()));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Добавление нескольких подполей: массив из одного элемента")]
        public void Field_AddRange_2()
        {
            var field = new Field();
            var sf1 = new SubField ('a', "SubA");
            Assert.AreSame (field, field.AddRange(Sequence.FromItem (sf1)));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual (sf1.Code, field.Subfields[0].Code);
            Assert.AreEqual (sf1.Value, field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Добавление нескольких подполей: массив из двух элементов")]
        public void Field_AddRange_3()
        {
            var field = new Field();
            var sf1 = new SubField ('a', "SubA");
            var sf2 = new SubField ('b', "SubB");
            Assert.AreSame (field, field.AddRange(Sequence.FromItems (sf1, sf2)));
            Assert.AreEqual (2, field.Subfields.Count);
            Assert.AreEqual (sf1.Code, field.Subfields[0].Code);
            Assert.AreEqual (sf1.Value, field.Subfields[0].Value);
            Assert.AreEqual (sf2.Code, field.Subfields[1].Code);
            Assert.AreEqual (sf2.Value, field.Subfields[1].Value);
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
        [Description ("Очистка подполей")]
        public void Field_Clear_1()
        {
            var field = new Field (100, "Value");
            Assert.AreSame (field, field.Clear());
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Клонирование: пустое поле")]
        public void Field_Clone_1()
        {
            var original = new Field();
            var clone = original.Clone();
            Assert.AreNotSame (original, clone);
            Assert.AreEqual (original.Tag, clone.Tag);
            Assert.AreEqual (original.Subfields.Count, clone.Subfields.Count);
        }

        [TestMethod]
        [Description ("Клонирование: только значение до первого разделителя")]
        public void Field_Clone_2()
        {
            var original = new Field (100, "Value");
            var clone = original.Clone();
            Assert.AreNotSame (original, clone);
            Assert.AreEqual (original.Tag, clone.Tag);
            Assert.AreEqual (original.Subfields.Count, clone.Subfields.Count);
            Assert.AreEqual(original.Subfields[0].Code, clone.Subfields[0].Code);
            Assert.AreEqual(original.Subfields[0].Value, clone.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Клонирование: только подполя")]
        public void Field_Clone_3()
        {
            var original = new Field (100, 'a', "SubFieldA", 'b', "SubFieldB");
            var clone = original.Clone();
            Assert.AreNotSame (original, clone);
            Assert.AreEqual (original.Tag, clone.Tag);
            Assert.AreEqual (original.Subfields.Count, clone.Subfields.Count);
            Assert.AreEqual(original.Subfields[0].Code, clone.Subfields[0].Code);
            Assert.AreEqual(original.Subfields[0].Value, clone.Subfields[0].Value);
            Assert.AreEqual(original.Subfields[1].Code, clone.Subfields[1].Code);
            Assert.AreEqual(original.Subfields[1].Value, clone.Subfields[1].Value);
        }

        [TestMethod]
        [Description ("Создание подполя для значения до первого разделителя: пустое поле")]
        public void Field_CreateValueSubField_1()
        {
            var field = new Field();
            var sub1 = field.CreateValueSubField();
            Assert.IsNotNull (sub1);
            var sub2 = field.CreateValueSubField();
            Assert.AreSame (sub1, sub2);
        }

        [TestMethod]
        [Description ("Создание подполя для значения до первого разделителя: непустое поле")]
        public void Field_CreateValueSubField_2()
        {
            var field = new Field()
            {
                new ('a', "SubFieldA")
            };
            var sub1 = field.CreateValueSubField();
            Assert.IsNotNull (sub1);
            var sub2 = field.CreateValueSubField();
            Assert.AreSame (sub1, sub2);
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

        [TestMethod]
        [Description ("Поиск или добавление подполя: пустое поле")]
        public void Field_GetOrAddSubField_1()
        {
            var field = new Field();
            var subfield = field.GetOrAddSubField ('a');
            Assert.IsNotNull (subfield);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreSame (field, subfield.Field);
            Assert.AreEqual ('a', subfield.Code);
        }

        [TestMethod]
        [Description ("Поиск или добавление подполя: подполе уже существует")]
        public void Field_GetOrAddSubField_2()
        {
            var field = new Field();
            var subfield1 = new SubField ('a');
            field.Add (subfield1);
            var subfield2 = field.GetOrAddSubField ('a');
            Assert.IsNotNull (subfield2);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreSame (field, subfield2.Field);
            Assert.AreSame (subfield1, subfield2);
            Assert.AreEqual ('a', subfield2.Code);
        }

        [TestMethod]
        [Description ("Поиск или добавление подполя: значение до первого разделителя")]
        public void Field_GetOrAddSubField_3()
        {
            var field = new Field();
            var subfield = field.GetOrAddSubField ('\0');
            Assert.IsNotNull (subfield);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreSame (field, subfield.Field);
            Assert.AreEqual ('\0', subfield.Code);
        }

        [TestMethod]
        [Description ("Поиск или добавление подполя: значение до первого разделителя")]
        public void Field_GetOrAddSubField_4()
        {
            var field = new Field();
            var subfield1 = new SubField ('\0');
            field.Add (subfield1);
            var subfield2 = field.GetOrAddSubField ('\0');
            Assert.IsNotNull (subfield2);
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreSame (field, subfield2.Field);
            Assert.AreEqual ('\0', subfield2.Code);
        }

        [TestMethod]
        [Description ("Поиск или добавление подполя: подполя есть, но не те")]
        public void Field_GetOrAddSubField_5()
        {
            var field = new Field();
            field.Add ('a');
            var subfield = field.GetOrAddSubField ('b');
            Assert.IsNotNull (subfield);
            Assert.AreEqual (2, field.Subfields.Count);
            Assert.AreSame (field, subfield.Field);
            Assert.AreEqual ('b', subfield.Code);
        }

        [TestMethod]
        [Description ("Поиск или добавление подполя: звездочка")]
        public void Field_GetOrAddSubField_6()
        {
            var field = new Field
            {
                { 'a', "SubA" }
            };
            var found = field.GetOrAddSubField ('*');
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreSame (field.Subfields[0], found);
        }

        [TestMethod]
        [Description ("Поиск или добавление подполя: звездочка")]
        public void Field_GetOrAddSubField_7()
        {
            var field = new Field();
            var found = field.GetOrAddSubField ('*');
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('\0', found.Code);
        }

        [TestMethod]
        [Description ("Получение массива подполей: пустое поле")]
        public void Field_GetSubFields_1()
        {
            var field = new Field();
            var found = field.GetSubFields ('a');
            Assert.AreEqual (0, found.Length);
        }

        [TestMethod]
        [Description ("Получение массива подполей: есть одно подходящее подполе")]
        public void Field_GetSubFields_2()
        {
            var field = new Field
            {
                { 'a', "SubA" },
                { 'b', "SubB" },
            };
            var found = field.GetSubFields ('a');
            Assert.AreEqual (1, found.Length);
            Assert.AreSame (field.Subfields[0], found[0]);
        }

        [TestMethod]
        [Description ("Получение массива подполей: есть два подходящих подполя")]
        public void Field_GetSubFields_3()
        {
            var field = new Field
            {
                { 'a', "SubA1" },
                { 'b', "SubB" },
                { 'a', "SubA2" },
            };
            var found = field.GetSubFields ('a');
            Assert.AreEqual (2, found.Length);
            Assert.AreSame (field.Subfields[0], found[0]);
            Assert.AreSame (field.Subfields[2], found[1]);
        }

        [TestMethod]
        [Description ("Получение массива подполей: нет подходящих полей")]
        public void Field_GetSubFields_4()
        {
            var field = new Field
            {
                { 'a', "SubA1" },
                { 'b', "SubB" },
            };
            var found = field.GetSubFields ('z');
            Assert.AreEqual (0, found.Length);
        }

        [TestMethod]
        [Description ("Получение массива подполей: звездочка")]
        public void Field_GetSubFields_5()
        {
            var field = new Field
            {
                { 'a', "SubA1" },
                { 'b', "SubB" },
            };
            var found = field.GetSubFields ('*');
            Assert.AreEqual (1, found.Length);
            Assert.AreSame (field.Subfields[0], found[0]);
        }

        [TestMethod]
        [Description ("Получение массива подполей: звездочка")]
        public void Field_GetSubFields_6()
        {
            var field = new Field { Value = "Value" };
            var found = field.GetSubFields ('*');
            Assert.AreEqual (1, found.Length);
            Assert.AreSame (field.Subfields[0], found[0]);
        }

        [TestMethod]
        [Description ("Поиск подподя с указанным кодом: пустое поле")]
        public void Field_GetSubField_1()
        {
            var field = new Field();
            var found = field.GetSubField ('a');
            Assert.IsNull (found);

            found = field.GetSubField ('a', 1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Поиск подполя с указанным кодом: есть одно вхождение")]
        public void Field_GetSubField_2()
        {
            var field = new Field
            {
                { 'a', "SubA" },
                { 'b', "SubB" }
            };
            var found = field.GetSubField ('a');
            Assert.AreSame (field.Subfields[0], found);

            found = field.GetSubField ('a', 1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Поиск подполя с указанным кодом: есть два вхождения")]
        public void Field_GetSubField_3()
        {
            var field = new Field
            {
                { 'a', "SubA1" },
                { 'b', "SubB" },
                { 'a', "SubA2" },
            };
            var found = field.GetSubField ('a');
            Assert.AreSame (field.Subfields[0], found);

            found = field.GetSubField ('a', 1);
            Assert.AreSame (field.Subfields[2], found);

            found = field.GetSubField ('a', 2);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Поиск подполя с указанным кодом: нет ни одного вхождения нужного подполя")]
        public void Field_GetSubField_4()
        {
            var field = new Field
            {
                { 'a', "SubA" },
                { 'b', "SubB" },
            };
            var found = field.GetSubField ('c');
            Assert.IsNull (found);

            found = field.GetSubField ('c', 1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Поиск подполя с указанным кодом: отрицательный индекс")]
        public void Field_GetSubField_5()
        {
            var field = new Field()
            {
                { 'a', "SubA1" },
                { 'b', "SubB" },
                { 'a', "SubA2" },
                { 'c', "SubC" },
                { 'a', "SubA3" },
            };
            var found = field.GetSubField ('a', -1);
            Assert.AreSame (field.Subfields[4], found);

            found = field.GetSubField ('a', -2);
            Assert.AreSame (field.Subfields[2], found);

            found = field.GetSubField ('a', -3);
            Assert.AreSame (field.Subfields[0], found);

            found = field.GetSubField ('a', -4);
            Assert.IsNull (found);

            found = field.GetSubField ('b', -1);
            Assert.AreSame (field.Subfields[1], found);

            found = field.GetSubField ('b', -2);
            Assert.IsNull (found);

            found = field.GetSubField ('c', -1);
            Assert.AreSame (field.Subfields[3], found);

            found = field.GetSubField ('c', -2);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Поиск подполя с указанным кодом: значение до первого разделителя")]
        public void Field_GetSubField_6()
        {
            var field = new Field();
            var found = field.GetSubField ('\0');
            Assert.IsNull (found);

            field.Value = "Value";
            found = field.GetSubField ('\0');
            Assert.IsNotNull (found);
            Assert.AreSame (field.Subfields[0], found);
            Assert.AreEqual ("Value", found.Value);

            found = field.GetSubField ('\0', 1);
            Assert.IsNull (found);

            found = field.GetSubField ('\0', -1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Поиск подполя с указанным кодом: звездочка")]
        public void Field_GetSubField_7()
        {
            var field = new Field();
            var found = field.GetSubField ('*');
            Assert.IsNull (found);

            field.Value = "Value";
            found = field.GetSubField ('*');
            Assert.IsNotNull (found);
            Assert.AreSame (field.Subfields[0], found);
            Assert.AreEqual ("Value", found.Value);

            found = field.GetSubField ('*', 1);
            Assert.IsNull (found);

            found = field.GetSubField ('*', -1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Поиск подполя с указанным кодом: звездочка")]
        public void Field_GetSubField_8()
        {
            var field = new Field { Value = "^aValue" };
            var found = field.GetSubField ('*');
            Assert.IsNotNull (found);
            Assert.AreSame (field.Subfields[0], found);
            Assert.AreEqual ("Value", found.Value);

            found = field.GetSubField ('*', 1);
            Assert.IsNull (found);

            found = field.GetSubField ('*', -1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Значение подполя с указанным кодом: пустое поле")]
        public void Field_GetSubFieldValue_1()
        {
            var field = new Field();
            var found = field.GetSubFieldValue ('a');
            Assert.IsNull (found);

            found = field.GetSubFieldValue ('a', 1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Значение подполя с указанным кодом: есть одно вхождение")]
        public void Field_GetSubFieldValue_2()
        {
            var field = new Field
            {
                { 'a', "SubA" },
                { 'b', "SubB" }
            };
            var found = field.GetSubFieldValue ('a');
            Assert.AreSame (field.Subfields[0].Value, found);

            found = field.GetSubFieldValue ('a', 1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Значение подполя с указанным кодом: есть два вхождения")]
        public void Field_GetSubFieldValue_3()
        {
            var field = new Field
            {
                { 'a', "SubA1" },
                { 'b', "SubB" },
                { 'a', "SubA2" },
            };
            var found = field.GetSubFieldValue ('a');
            Assert.AreSame (field.Subfields[0].Value, found);

            found = field.GetSubFieldValue ('a', 1);
            Assert.AreSame (field.Subfields[2].Value, found);

            found = field.GetSubFieldValue ('a', 2);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Значение подполя с указанным кодом: нет ни одного вхождения нужного подполя")]
        public void Field_GetSubFieldValue_4()
        {
            var field = new Field
            {
                { 'a', "SubA" },
                { 'b', "SubB" },
            };
            var found = field.GetSubFieldValue ('c');
            Assert.IsNull (found);

            found = field.GetSubFieldValue ('c', 1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Значение подполя с указанным кодом: отрицательный индекс")]
        public void Field_GetSubFieldValue_5()
        {
            var field = new Field()
            {
                { 'a', "SubA1" },
                { 'b', "SubB" },
                { 'a', "SubA2" },
                { 'c', "SubC" },
                { 'a', "SubA3" },
            };
            var found = field.GetSubFieldValue ('a', -1);
            Assert.AreSame (field.Subfields[4].Value, found);

            found = field.GetSubFieldValue ('a', -2);
            Assert.AreSame (field.Subfields[2].Value, found);

            found = field.GetSubFieldValue ('a', -3);
            Assert.AreSame (field.Subfields[0].Value, found);

            found = field.GetSubFieldValue ('a', -4);
            Assert.IsNull (found);

            found = field.GetSubFieldValue ('b', -1);
            Assert.AreSame (field.Subfields[1].Value, found);

            found = field.GetSubFieldValue ('b', -2);
            Assert.IsNull (found);

            found = field.GetSubFieldValue ('c', -1);
            Assert.AreSame (field.Subfields[3].Value, found);

            found = field.GetSubFieldValue ('c', -2);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Значение подполя с указанным кодом: значение до первого разделителя")]
        public void Field_GetSubFieldValue_6()
        {
            var field = new Field();
            var found = field.GetSubFieldValue ('\0');
            Assert.IsNull (found);

            field.Value = "Value";
            found = field.GetSubFieldValue ('\0');
            Assert.AreSame (field.Subfields[0].Value, found);
            Assert.AreEqual ("Value", found);

            found = field.GetSubFieldValue ('\0', 1);
            Assert.IsNull (found);

            found = field.GetSubFieldValue ('\0', -1);
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Звездочка: пустое поле")]
        public void Field_GetValueOrFirstSubField_1()
        {
            var field = new Field();
            var value = field.GetValueOrFirstSubField();
            Assert.IsNull (value);
        }

        [TestMethod]
        [Description ("Звездочка: есть значение до первого разделителя")]
        public void Field_GetValueOrFirstSubField_2()
        {
            var field = new Field { Value = "Value" };
            var value = field.GetValueOrFirstSubField();
            Assert.AreSame (field.Value, value);
        }

        [TestMethod]
        [Description ("Звездочка: есть подполя")]
        public void Field_GetValueOrFirstSubField_3()
        {
            var field = new Field
            {
                { 'a', "SubA" },
                { 'b', "SubB" },
            };
            var value = field.GetValueOrFirstSubField();
            Assert.AreSame (field.Subfields[0].Value, value);
        }

        [TestMethod]
        [Description ("Поиск первого подполя с указанным кодом: пустое поле")]
        public void Field_GetFirstSubField_1()
        {
            var field = new Field();
            var found = field.GetFirstSubField ('a');
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Поиск первого подполя с указанным кодом: только значение до первого разделителя")]
        public void Field_GetFirstSubField_2()
        {
            var field = new Field() { Value = "Value"};
            var found = field.GetFirstSubField ('a');
            Assert.IsNull (found);

            found = field.GetFirstSubField ('\0');
            Assert.IsNotNull (found);
            Assert.AreEqual ('\0', found.Code);
            Assert.AreEqual ("Value", found.Value);
        }

        [TestMethod]
        [Description ("Поиск первого подполя с указанным кодом: только подполя")]
        public void Field_GetFirstSubField_3()
        {
            var field = new Field()
            {
                new ('b', "SubFieldB"),
                new ('a', "SubFieldA1"),
                new ('a', "SubFieldA2"),
            };
            var found = field.GetFirstSubField ('z');
            Assert.IsNull (found);

            found = field.GetFirstSubField ('\0');
            Assert.IsNull (found);

            found = field.GetFirstSubField ('a');
            Assert.IsNotNull (found);
            Assert.AreEqual ('a', found.Code);
            Assert.AreEqual ("SubFieldA1", found.Value);
        }

        [TestMethod]
        [Description ("Поиск первого подполя с указанным кодом: звездочка")]
        public void Field_GetFirstSubField_4()
        {
            var field = new Field()
            {
                new ('b', "SubFieldB"),
                new ('a', "SubFieldA1"),
                new ('a', "SubFieldA2"),
            };
            var found = field.GetFirstSubField ('*');
            Assert.AreSame (field.Subfields[0], found);
        }

        [TestMethod]
        [Description ("Поиск первого подполя с указанным кодом: звездочка")]
        public void Field_GetFirstSubField_5()
        {
            var field = new Field();
            var found = field.GetFirstSubField ('*');
            Assert.IsNull (found);
        }

        [TestMethod]
        [Description ("Перечисление подполей: пустое поле")]
        public void Field_EnumerateSubFields_1()
        {
            var field = new Field();
            var array = field.EnumerateSubFields ('a').ToArray();
            Assert.AreEqual (0, array.Length);
        }

        [TestMethod]
        [Description ("Перечисление подполей: только значение до первого разделителя")]
        public void Field_EnumerateSubFields_2()
        {
            var field = new Field() { Value = "Value" };
            var found = field.EnumerateSubFields ('a').ToArray();
            Assert.AreEqual (0, found.Length);

            found = field.EnumerateSubFields ('\0').ToArray();
            Assert.AreEqual (1, found.Length);
            Assert.AreEqual ('\0', found[0].Code);
            Assert.AreEqual ("Value", found[0].Value);
        }

        [TestMethod]
        [Description ("Перечисление подполей: только подполя")]
        public void Field_EnumerateSubFields_3()
        {
            var field = new Field()
            {
                new ('a', "SubFieldA1"),
                new ('b', "SubFieldB"),
                new ('a', "SubFieldA2")
            };
            var found = field.EnumerateSubFields ('z').ToArray();
            Assert.AreEqual (0, found.Length);

            found = field.EnumerateSubFields ('\0').ToArray();
            Assert.AreEqual (0, found.Length);

            found = field.EnumerateSubFields ('a').ToArray();
            Assert.AreEqual (2, found.Length);
            Assert.AreEqual ('a', found[0].Code);
            Assert.AreEqual ('a', found[1].Code);
            Assert.AreEqual ("SubFieldA1", found[0].Value);
            Assert.AreEqual ("SubFieldA2", found[1].Value);
        }

        [TestMethod]
        [Description ("Перечисление подполей: звездочка")]
        public void Field_EnumerateSubFields_4()
        {
            var field = new Field()
            {
                new ('a', "SubFieldA1"),
                new ('b', "SubFieldB"),
                new ('a', "SubFieldA2")
            };
            var found = field.EnumerateSubFields ('*').ToArray();
            Assert.AreEqual (1, found.Length);
            Assert.AreSame (field.Subfields[0], found[0]);
        }

        [TestMethod]
        [Description ("Получение подполя для значения до первого разделителя: пустое поле")]
        public void Field_GetValueSubField_1()
        {
            var field = new Field();
            var sub = field.GetValueSubField();
            Assert.IsNull (sub);
        }

        [TestMethod]
        [Description ("Получение подполя для значения до первого разделителя: непустое поле, но значения нет")]
        public void Field_GetValueSubField_2()
        {
            var field = new Field
            {
                new ('a', "SubFieldA")
            };
            var sub = field.GetValueSubField();
            Assert.IsNull (sub);
        }

        [TestMethod]
        [Description ("Получение подполя для значения до первого разделителя: непустое поле, значение есть")]
        public void Field_GetValueSubField_3()
        {
            var field = new Field { Value = "Value" };
            var sub = field.GetValueSubField();
            Assert.IsNotNull (sub);
        }

        [TestMethod]
        [Description ("Удаление подполя с указанным кодом: пустое поле")]
        public void Field_RemoveSubField_1()
        {
            var field = new Field();
            Assert.AreSame (field, field.RemoveSubField ('a'));
            Assert.AreSame (field, field.RemoveSubField ('\0'));
        }

        [TestMethod]
        [Description ("Удаление подполя с указанным кодом: только значение до первого разделителя")]
        public void Field_RemoveSubField_2()
        {
            var field = new Field { Value = "Value" };
            Assert.AreSame (field, field.RemoveSubField ('a'));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreSame (field, field.RemoveSubField ('\0'));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Удаление подполя с указанным кодом: только значения")]
        public void Field_RemoveSubField_3()
        {
            var field = new Field
            {
                { 'a', "SubA1" },
                { 'b', "SubB" },
                { 'a', "SubA2" }
            };
            Assert.AreSame (field, field.RemoveSubField ('z'));
            Assert.AreEqual (3, field.Subfields.Count);
            Assert.AreSame (field, field.RemoveSubField ('a'));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreSame (field, field.RemoveSubField ('\0'));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('b', field.Subfields[0].Code);
            Assert.AreSame (field, field.RemoveSubField ('b'));
            Assert.AreEqual (0, field.Subfields.Count);
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

        [TestMethod]
        [ExpectedException (typeof (ReadOnlyException))]
        [Description ("Нельзя изменять значение до первого разделителя в ReadOnly-полях")]
        public void Field_ReadOnly_1()
        {
            var field = _GetField().AsReadOnly();
            Assert.IsTrue (field.ReadOnly);
            field.Value = "New value";
        }

        [TestMethod]
        [ExpectedException (typeof (ReadOnlyException))]
        [Description ("Нельзя добавлять подполя в ReadOnly-полях")]
        public void Field_ReadOnly_2()
        {
            var field = _GetField().AsReadOnly();
            Assert.IsTrue (field.ReadOnly);
            field.Add ('a', "Value");
        }

        [TestMethod]
        [ExpectedException (typeof (ReadOnlyException))]
        [Description ("Нельзя изменять значение подполей в ReadOnly-полях")]
        public void Field_ReadOnly_3()
        {
            var field = _GetField().AsReadOnly();
            Assert.IsTrue (field.ReadOnly);
            var subField = field.Subfields[0];
            subField.Value = "New value";
        }

        [TestMethod]
        [Description ("Можно изменять метку в ReadOnly-полях")]
        public void Field_ReadOnly_4()
        {
            var field = _GetField().AsReadOnly();
            Assert.IsTrue (field.ReadOnly);
            field.Tag = 1000;
            Assert.AreEqual (1000, field.Tag);
        }

        [TestMethod]
        [Description ("Свойство Record: должно присваиваться автоматически")]
        public void Field_Record_1()
        {
            var record = new Record();
            var field = new Field();
            Assert.IsNull (field.Record);
            record.Add (field);
            Assert.AreSame (record, field.Record);
        }

        [TestMethod]
        [Description ("Свойство Value: присваивание пустой строки")]
        public void Field_Value_1()
        {
            var field = _GetField();
            Assert.AreNotEqual (0, field.Subfields.Count);
            field.Value = string.Empty;
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Свойство Value: присваивание пустой строки")]
        public void Field_Value_2()
        {
            var field = _GetField();
            Assert.AreNotEqual (0, field.Subfields.Count);
            field.Value = null;
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Свойство Value: присваивание строки без разделителей")]
        public void Field_Value_3()
        {
            const string simpleValue = "Привет, мир!";
            var field = _GetField();
            Assert.AreNotEqual (0, field.Subfields.Count);
            field.Value = simpleValue;
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual (simpleValue, field.Value);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual (simpleValue, field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Свойство Value: присваивание строки с разделителями")]
        public void Field_Value_4()
        {
            var field = _GetField();
            Assert.AreNotEqual (0, field.Subfields.Count);
            field.Value = "^aSubA^bSubB";
            Assert.AreEqual (2, field.Subfields.Count);
            Assert.IsNull (field.Value);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("SubA", field.Subfields[0].Value);
            Assert.AreEqual ('b', field.Subfields[1].Code);
            Assert.AreEqual ("SubB", field.Subfields[1].Value);
        }

        [TestMethod]
        [Description ("Установка значения подполя: DateTime")]
        public void Field_SetSubFieldValue_1()
        {
            var field = new Field();
            Assert.AreSame(field, field.SetSubFieldValue ('a', new DateTime (2021, 11, 5)));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("20211105", field.Subfields[0].Value);

            Assert.AreSame(field, field.SetSubFieldValue ('a', new DateTime (2021, 11, 6)));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("20211106", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Установка значения подполя: DateTime?")]
        public void Field_SetSubFieldValue_2()
        {
            var field = new Field();
            DateTime? date = new DateTime (2021, 11, 5);
            Assert.AreSame(field, field.SetSubFieldValue ('a', date));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("20211105", field.Subfields[0].Value);

            date = null;
            Assert.AreSame(field, field.SetSubFieldValue ('a', date));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Установка значения подполя: int?")]
        public void Field_SetSubFieldValue_3()
        {
            var field = new Field();
            int? value = 123;
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("123", field.Subfields[0].Value);

            value = null;
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Установка значения подполя: long?")]
        public void Field_SetSubFieldValue_4()
        {
            var field = new Field();
            long? value = 123L;
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("123", field.Subfields[0].Value);

            value = null;
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Установка значения подполя: object?")]
        public void Field_SetSubFieldValue_5()
        {
            var field = new Field();
            object? value = new MyClass() { Text = "text" };
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("text", field.Subfields[0].Value);

            value = null;
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Установка значения подполя: bool и object?")]
        public void Field_SetSubFieldValue_6()
        {
            var field = new Field();
            object? value = new MyClass() { Text = "text" };
            Assert.AreSame(field, field.SetSubFieldValue ('a', true, value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("text", field.Subfields[0].Value);

            value = null;
            Assert.AreSame(field, field.SetSubFieldValue ('a', true, value));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Установка значения подполя: bool и object?")]
        public void Field_SetSubFieldValue_7()
        {
            var field = new Field();
            object? value = 123;
            Assert.AreSame(field, field.SetSubFieldValue ('a', true, value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("123", field.Subfields[0].Value);

            value = null;
            Assert.AreSame(field, field.SetSubFieldValue ('a', true, value));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Установка значения подполя: bool и object?")]
        public void Field_SetSubFieldValue_8()
        {
            var field = new Field();
            object? value = 123;
            Assert.AreSame(field, field.SetSubFieldValue ('\0', true, value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual ("123", field.Subfields[0].Value);

            value = null;
            Assert.AreSame(field, field.SetSubFieldValue ('\0', true, value));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Установка значения подполя: int")]
        public void Field_SetSubFieldValue_9()
        {
            var field = new Field();
            var value = 123;
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("123", field.Subfields[0].Value);

            value = 321;
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("321", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Установка значения подполя: long")]
        public void Field_SetSubFieldValue_10()
        {
            var field = new Field();
            var value = 123L;
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("123", field.Subfields[0].Value);

            value = 321L;
            Assert.AreSame(field, field.SetSubFieldValue ('a', value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual ("321", field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Установка значения подполя: звездочка")]
        public void Field_SetSubFieldValue_11()
        {
            var field = new Field();
            const string value = "Value";
            Assert.AreSame(field, field.SetSubFieldValue ('*', value));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual (value, field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Установка значения подполя: звездочка")]
        public void Field_SetSubFieldValue_12()
        {
            var field = new Field
            {
                { 'a', "OldValue" }
            };

            const string newValue = "NewValue";
            Assert.AreSame(field, field.SetSubFieldValue ('*', newValue));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('a', field.Subfields[0].Code);
            Assert.AreEqual (newValue, field.Subfields[0].Value);
        }

        [TestMethod]
        [Description ("Установка значения подполя: значение до первого разделителя")]
        public void Field_SetSubFieldValue_13()
        {
            var field = new Field();
            const string newValue = "NewValue";
            Assert.AreSame(field, field.SetSubFieldValue ('\0', newValue));
            Assert.AreEqual (1, field.Subfields.Count);
            Assert.AreEqual ('\0', field.Subfields[0].Code);
            Assert.AreEqual (newValue, field.Subfields[0].Value);
            Assert.AreEqual (newValue, field.Value);
        }

        [TestMethod]
        [Description ("Установка значения подполя: значение до первого разделителя")]
        public void Field_SetSubFieldValue_14()
        {
            var field = new Field() { Value = "OldValue" };
            Assert.AreSame(field, field.SetSubFieldValue ('\0', string.Empty));
            Assert.AreEqual (0, field.Subfields.Count);
        }

        [TestMethod]
        [Description ("Верификация поля: пустое поле")]
        public void Field_Verify_1()
        {
            var field = new Field();
            Assert.IsFalse (field.Verify (false));
        }

        [TestMethod]
        [Description ("Верификация поля: отрицательная метка")]
        public void Field_Verify_2()
        {
            var field = new Field (-100);
            Assert.IsFalse (field.Verify (false));
        }

        [TestMethod]
        [Description ("Верификация поля: нет подполей")]
        public void Field_Verify_3()
        {
            var field = new Field (100);
            Assert.IsFalse (field.Verify (false));
        }

        [TestMethod]
        [Description ("Верификация поля: пустые подполя")]
        public void Field_Verify_4()
        {
            var field = new Field (100)
            {
                {'a'},
                {'b'}
            };

            // Будем считать пустые подполя валидными
            Assert.IsTrue (field.Verify (false));
        }

        [TestMethod]
        [Description ("Верификация поля: два подполя со значением до первого разделителя")]
        public void Field_Verify_5()
        {
            var field = new Field (100)
            {
                {'\0', "First value"},
                {'\0', "Second value"}
            };

            Assert.IsFalse (field.Verify (false));
        }
    }
}
