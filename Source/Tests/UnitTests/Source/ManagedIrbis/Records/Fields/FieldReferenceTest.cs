// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Runtime;

using ManagedIrbis;
using ManagedIrbis.Pft;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Fields
{
    [TestClass]
    public class FieldReferenceTest
    {
        [TestMethod]
        public void FieldReference_Constructor_1()
        {
            var reference = new FieldReference();
            Assert.AreEqual('\0', reference.Command);
            Assert.IsNull(reference.Embedded);
            Assert.AreEqual(0, reference.Indent);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);
            Assert.AreEqual(FieldReference.NoCode, reference.SubField);
            Assert.AreEqual(0, reference.Tag);
            Assert.IsNull(reference.TagSpecification);
            Assert.IsNull(reference.SubFieldSpecification);
        }

        [TestMethod]
        public void FieldReference_Constructor_2()
        {
            var reference = new FieldReference(200);
            Assert.AreEqual('\0', reference.Command);
            Assert.IsNull(reference.Embedded);
            Assert.AreEqual(0, reference.Indent);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);
            Assert.AreEqual(FieldReference.NoCode, reference.SubField);
            Assert.AreEqual(200, reference.Tag);
            Assert.IsNull(reference.TagSpecification);
            Assert.IsNull(reference.SubFieldSpecification);
        }

        [TestMethod]
        public void FieldReference_Constructor_3()
        {
            var reference = new FieldReference(200, 'a');
            Assert.AreEqual('\0', reference.Command);
            Assert.IsNull(reference.Embedded);
            Assert.AreEqual(0, reference.Indent);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);
            Assert.AreEqual('a', reference.SubField);
            Assert.AreEqual(200, reference.Tag);
            Assert.IsNull(reference.TagSpecification);
            Assert.IsNull(reference.SubFieldSpecification);
        }

        private void _TestSerialization
            (
                FieldReference first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<FieldReference>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Command, second!.Command);
            Assert.AreEqual(first.Embedded, second.Embedded);
            Assert.AreEqual(first.Indent, second.Indent);
            Assert.AreEqual(first.Offset, second.Offset);
            Assert.AreEqual(first.Length, second.Length);
            Assert.AreEqual(first.SubField, second.SubField);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.AreEqual(first.TagSpecification, second.TagSpecification);
            Assert.AreEqual(first.FieldRepeat, second.FieldRepeat);
            Assert.AreEqual(first.SubFieldRepeat, second.SubFieldRepeat);
            Assert.AreEqual(first.SubFieldSpecification, second.SubFieldSpecification);
        }

        [TestMethod]
        public void FieldReference_Serialization_1()
        {
            var reference = new FieldReference();
            _TestSerialization(reference);

            reference.Tag = 200;
            reference.SubField = 'a';
            _TestSerialization(reference);
        }

        private Record _GetRecord()
        {
            var result = new Record();

            var field = new Field { Tag = 700 };
            field.Add('a', "Иванов");
            field.Add('b', "И. И.");
            result.Fields.Add(field);

            field = new Field { Tag = 701 };
            field.Add('a', "Петров");
            field.Add('b', "П. П.");
            result.Fields.Add(field);

            field = new Field { Tag = 200 };
            field.Add('a', "Заглавие");
            field.Add('e', "подзаголовочное");
            field.Add('f', "И. И. Иванов, П. П. Петров");
            result.Fields.Add(field);

            field = new Field ( 300, "Первое примечание" );
            result.Fields.Add(field);
            field = new Field (300,  "Второе примечание" );
            result.Fields.Add(field);
            field = new Field ( 300, "Третье примечание" );
            result.Fields.Add(field);

            return result;
        }

        [TestMethod]
        public void FieldReference_Format_1()
        {
            var record = _GetRecord();
            var reference = new FieldReference(200, 'a');

            var actual = reference.Format(record);
            Assert.AreEqual
                (
                    record.FM
                        (
                            reference.Tag,
                            reference.SubField
                        ),
                    actual
                );
        }

        [TestMethod]
        public void FieldReference_GetValues_1()
        {
            var record = _GetRecord();
            var reference = new FieldReference(300);
            var values = reference.GetValues(record);
            Assert.AreEqual(3, values.Length);
        }

        [TestMethod]
        public void FieldReference_GetValues_2()
        {
            var record = _GetRecord();
            var reference = new FieldReference();
            var values = reference.GetValues(record);
            Assert.AreEqual(0, values.Length);
        }

        [TestMethod]
        public void FieldReference_GetValues_3()
        {
            var record = _GetRecord();
            var reference = new FieldReference(200, 'a');
            var values = reference.GetValues(record);
            Assert.AreEqual(1, values.Length);
        }

        [TestMethod]
        public void FieldReference_GetUniqueValues_1()
        {
            var record = _GetRecord();
            var reference = new FieldReference(300);
            var values = reference.GetUniqueValues(record);
            Assert.AreEqual(3, values.Length);
        }

        [TestMethod]
        public void FieldReference_GetUniqueValuesIgnoreCase_1()
        {
            var record = _GetRecord();
            var reference = new FieldReference(300);
            var values = reference.GetUniqueValuesIgnoreCase(record);
            Assert.AreEqual(3, values.Length);
        }

        [TestMethod]
        public void FieldReference_Parse_1()
        {
            var reference = FieldReference.Parse("v200");
            Assert.AreEqual('v', reference.Command);
            Assert.AreEqual(200, reference.Tag);
            Assert.AreEqual(FieldReference.NoCode, reference.SubField);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);

            reference = FieldReference.Parse("v200^a");
            Assert.AreEqual('v', reference.Command);
            Assert.AreEqual(200, reference.Tag);
            Assert.AreEqual('a', reference.SubField);
            Assert.AreEqual(0, reference.Offset);
            Assert.AreEqual(0, reference.Length);

            reference = FieldReference.Parse("v200^a*5.7");
            Assert.AreEqual('v', reference.Command);
            Assert.AreEqual(200, reference.Tag);
            Assert.AreEqual('a', reference.SubField);
            Assert.AreEqual(5, reference.Offset);
            Assert.AreEqual(7, reference.Length);
        }

        private void _TestParse
            (
                string expected
            )
        {
            var reference = FieldReference.Parse(expected);
            Assert.AreEqual(200, reference.Tag);
        }

        [TestMethod]
        public void FieldReference_Parse_2()
        {
            _TestParse("v200");
            _TestParse("v200^a");
            _TestParse("v200[1]");
            _TestParse("v200*5");
            _TestParse("v200.2");
        }

        [TestMethod]
        public void FieldReference_Parse_3()
        {
            var reference = FieldReference.Parse("q1");
            Assert.IsFalse(reference.Verify(false));
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void FieldReference_Parse_4()
        {
            var reference = FieldReference.Parse("v1[");
            Assert.IsTrue(reference.Verify(false));
        }

        [TestMethod]
        public void FieldReference_Verify_1()
        {
            var reference = new FieldReference();
            Assert.IsFalse(reference.Verify(false));

            reference = new FieldReference(200, 'a');
            Assert.IsTrue(reference.Verify(false));
        }
    }
}
