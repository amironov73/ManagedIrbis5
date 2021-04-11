// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

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
        public void RecordField_Constructor_1()
        {
            Field field = new Field();
            Assert.AreEqual(0, field.Tag);
            Assert.IsTrue(field.Value.IsEmpty);
            Assert.AreEqual(0,field.Subfields.Count);
        }

        [TestMethod]
        public void RecordField_Add_1()
        {
            Field field = new Field();
            Assert.AreSame(field, field.Add('a', "Value"));
            Assert.AreEqual(1, field.Subfields.Count);
        }

        [TestMethod]
        public void RecordField_AddNonEmpty_1()
        {
            Field field = new Field()
                .AddNonEmpty('a', null)
                .AddNonEmpty('b', new MyClass{Text = "SubfieldB"});
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("SubfieldB", field.Subfields[0].Value.ToString());
        }

        [TestMethod]
        public void RecordField_AddNonEmpty_2()
        {
            Field field = new Field()
                .AddNonEmpty('a', 0)
                .AddNonEmpty('b', 1);
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("1", field.Subfields[0].Value.ToString());
        }

        [TestMethod]
        public void RecordField_AddNonEmpty_3()
        {
            Field field = new Field()
                .AddNonEmpty('a', 0L)
                .AddNonEmpty('b', 1L);
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("1", field.Subfields[0].Value.ToString());
        }

        [TestMethod]
        public void RecordField_AddNonEmpty_4()
        {
            DateTime? first = null;
            DateTime? second = new DateTime(2017, 9, 30);
            Field field = new Field()
                .AddNonEmpty('a', first)
                .AddNonEmpty('b', second);
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("20170930", field.Subfields[0].Value.ToString());
        }

        [TestMethod]
        public void RecordField_AddNonEmptySubField_5()
        {
            DateTime first = DateTime.MinValue;
            DateTime second = new DateTime(2017, 9, 30);
            Field field = new Field()
                .AddNonEmpty('a', first)
                .AddNonEmpty('b', second);
            Assert.AreEqual(1, field.Subfields.Count);
            Assert.AreEqual('b', field.Subfields[0].Code);
            Assert.AreEqual("20170930", field.Subfields[0].Value.ToString());
        }
    }
}
