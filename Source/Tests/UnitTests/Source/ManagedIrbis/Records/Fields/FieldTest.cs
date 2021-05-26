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
        [Description("Создание пустого поля")]
        public void Field_Constructor_1()
        {
            var field = new Field();
            Assert.AreEqual(0, field.Tag);
            Assert.IsTrue(field.Value.IsEmpty());
            Assert.AreEqual(0,field.Subfields.Count);
        }

        [TestMethod]
        public void Field_Add_1()
        {
            var field = new Field();
            Assert.AreSame(field, field.Add('a', "Value"));
            Assert.AreEqual(1, field.Subfields.Count);
        }

        [TestMethod]
        public void Field_AddNonEmpty_1()
        {
            var field = new Field()
                .AddNonEmpty('a', null)
                .AddNonEmpty('b', new MyClass{Text = "SubfieldB"});
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("SubfieldB", field.Subfields[0].Value);
        }

        [TestMethod]
        public void Field_AddNonEmpty_2()
        {
            var field = new Field()
                .AddNonEmpty('a', 0)
                .AddNonEmpty('b', 1);
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("1", field.Subfields[0].Value);
        }

        [TestMethod]
        public void Field_AddNonEmpty_3()
        {
            var field = new Field()
                .AddNonEmpty('a', 0L)
                .AddNonEmpty('b', 1L);
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("1", field.Subfields[0].Value);
        }

        [TestMethod]
        public void Field_AddNonEmpty_4()
        {
            DateTime? first = null;
            DateTime? second = new DateTime(2017, 9, 30);
            var field = new Field()
                .AddNonEmpty('a', first)
                .AddNonEmpty('b', second);
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("20170930", field.Subfields[0].Value);
        }

        [TestMethod]
        public void Field_AddNonEmpty_5()
        {
            var first = DateTime.MinValue;
            var second = new DateTime(2017, 9, 30);
            var field = new Field()
                .AddNonEmpty('a', first)
                .AddNonEmpty('b', second);
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("20170930", field.Subfields[0].Value);
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
        public void RecordField_Serialization_1()
        {
            _TestSerialization
                (
                    new Field()
                );
            _TestSerialization
                (
                    new Field(100)
                );
            _TestSerialization
                (
                    new Field(199, "Hello")
                );
            _TestSerialization
                (
                    new Field
                        (
                            200,
                            new SubField('a', "Hello")
                        )
                );
        }

    }
}
