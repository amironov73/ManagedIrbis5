// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Subfields
{
    [TestClass]
    public class SubFieldTest
    {
        [TestMethod]
        [Description("Конструктор по умолчанию")]
        public void SubField_Constructor_1()
        {
            var subField = new SubField();
            Assert.AreEqual(SubField.NoCode, subField.Code);
            Assert.IsTrue(subField.Value.IsEmpty);
            Assert.AreEqual(string.Empty, subField.ToString());
            Assert.IsTrue(subField.RepresentsValue);
            Assert.IsFalse(subField.Modified);
            Assert.IsNull(subField.Field);
            Assert.IsFalse(subField.ReadOnly);
            Assert.IsTrue(subField.Verify(false));
        }

        [TestMethod]
        [Description("Конструктор с ReadOnlyMemory")]
        public void SubField_Constructor_2()
        {
            const string value = "The value";
            var subField = new SubField ('A', value.AsMemory());
            Assert.AreEqual('A', subField.Code);
            Assert.AreEqual(value, subField.Value.ToString());
            Assert.AreEqual("^aThe value", subField.ToString());
            Assert.IsFalse(subField.RepresentsValue);
            Assert.IsFalse(subField.Modified);
            Assert.IsNull(subField.Field);
            Assert.IsFalse(subField.ReadOnly);
            Assert.IsTrue(subField.Verify(false));
        }

        [TestMethod]
        [Description("Конструктор со строкой")]
        public void SubField_Constructor_3()
        {
            const string value = "The value";
            var subField = new SubField ('A', value);
            Assert.AreEqual('A', subField.Code);
            Assert.AreEqual(value, subField.Value.ToString());
            Assert.AreEqual("^aThe value", subField.ToString());
            Assert.IsFalse(subField.RepresentsValue);
            Assert.IsFalse(subField.Modified);
            Assert.IsNull(subField.Field);
            Assert.IsFalse(subField.ReadOnly);
            Assert.IsTrue(subField.Verify(false));
        }

        [TestMethod]
        [Description("Клонирование пустого подполя")]
        public void SubField_Clone_1()
        {
            var subField = new SubField ();
            var clone = subField.Clone();
            Assert.AreEqual(subField.Code, clone.Code);
            Assert.AreEqual(subField.Value, clone.Value);
            Assert.AreEqual(string.Empty, clone.ToString());
            Assert.IsTrue(clone.RepresentsValue);
            Assert.IsFalse(clone.Modified);
            Assert.IsNull(clone.Field);
            Assert.AreEqual(subField.ReadOnly, clone.ReadOnly);
        }

        [TestMethod]
        [Description("Клонирование непустого подполя")]
        public void SubField_Clone_2()
        {
            const string value = "The value";
            var subField = new SubField ('A', value.AsMemory());
            var clone = subField.Clone();
            Assert.AreEqual(subField.Code, clone.Code);
            Assert.AreEqual(subField.Value, clone.Value);
            Assert.AreEqual("^aThe value", clone.ToString());
            Assert.IsFalse(clone.RepresentsValue);
            Assert.IsFalse(clone.Modified);
            Assert.IsNull(clone.Field);
            Assert.AreEqual(subField.ReadOnly, clone.ReadOnly);
        }

        [TestMethod]
        [Description("Сравнение пустых полей (в расчет идут только коды)")]
        public void SubField_Compare_1()
        {
            var left = new SubField('a');
            var right = new SubField('b');
            Assert.IsTrue(SubField.Compare(left, right) < 0);

            left.Code = 'c';
            Assert.IsTrue(SubField.Compare(left, right) > 0);

            left.Code = right.Code;
            Assert.IsTrue(SubField.Compare(left, right) == 0);

            left.Code = SubField.NoCode;
            Assert.IsTrue(SubField.Compare(left, right) < 0);
        }

        [TestMethod]
        [Description("Сравнение непустых полей")]
        public void SubField_Compare_2()
        {
            var left = new SubField('a', "The value");
            var right = new SubField('b', "The value");
            Assert.IsTrue(SubField.Compare(left, right) < 0);

            left = new SubField('a', "Title1");
            right = new SubField('a', "Title2");
            Assert.IsTrue(SubField.Compare(left, right) < 0);

            left = new SubField('a', "Title");
            right = new SubField('a', "Title");
            Assert.IsTrue(SubField.Compare(left, right) == 0);
        }

        [TestMethod]
        [Description("Декодирование пустой строки")]
        public void SubField_Decode_1()
        {
            var subField = new SubField();
            subField.Decode(string.Empty.AsMemory());
            Assert.AreEqual(SubField.NoCode, subField.Code);
            Assert.IsTrue(subField.Value.IsEmpty);
        }

        [TestMethod]
        [Description("Декодирование строки из одного символа")]
        public void SubField_Decode_2()
        {
            var subField = new SubField();
            subField.Decode("A".AsMemory());
            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual(0, subField.Value.Length);
        }

        [TestMethod]
        [Description("Декодирование строки из нескольких символов")]
        public void SubField_Decode_3()
        {
            var subField = new SubField();
            subField.Decode("AValue".AsMemory());
            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SubField_Decode_4()
        {
            var subField = new SubField();
            subField.Decode("Wrong^value".AsMemory());
        }

        [Ignore]
        [TestMethod]
        public void SubField_Field_1()
        {
            SubField subField = new SubField('a', "Title");
            Assert.IsNull(subField.Field);

            var field = new Field(200);
            field.Subfields.Add(subField);
            Assert.AreEqual(field, subField.Field);
        }

        private void _TestSerialization
            (
                params SubField[] subFields
            )
        {
            SubField[] array1 = subFields;
            byte[] bytes = array1.SaveToMemory();

            SubField[] array2 = bytes
                    .RestoreArrayFromMemory<SubField>();

            Assert.AreEqual(array1.Length, array2.Length);
            for (int i = 0; i < array1.Length; i++)
            {
                Assert.AreEqual
                    (
                        0,
                        SubField.Compare(array1[i], array2[i])
                    );
            }
        }

        [Ignore]
        [TestMethod]
        public void SubField_Serialization_1()
        {
            _TestSerialization(new SubField[0]);
            _TestSerialization(new SubField());
            _TestSerialization(new SubField(), new SubField());
            _TestSerialization(new SubField { Code = 'a' }, new SubField { Code = 'b' });
            _TestSerialization(new SubField ('a', "Hello"),
                new SubField ('b', "World") );
        }

        /*
        [TestMethod]
        public void SubField_SetValue_1()
        {
            SubField subField = new SubField('a')
            {
                Value = "Right Value"
            };
            Assert.AreEqual("Right Value", subField.Value);
        }

        [TestMethod]
        public void SubField_SetValue_2()
        {
            bool saveFlag = SubField.TrimValue;
            SubField.TrimValue = false;
            SubField subField = new SubField('a')
            {
                Value = "  Right Value  "
            };
            Assert.AreEqual("  Right Value  ", subField.Value);
            SubField.TrimValue = saveFlag;
        }

        [TestMethod]
        public void SubField_SetValue_3()
        {
            bool saveFlag = SubField.TrimValue;
            SubField.TrimValue = true;
            SubField subField = new SubField('a')
            {
                Value = "  Right Value  "
            };
            Assert.AreEqual("Right Value", subField.Value);
            SubField.TrimValue = saveFlag;
        }

        [TestMethod]
        public void SubField_SetValue_4()
        {
            SubField subField = new SubField('a')
            {
                Value = "Right\nValue"
            };
            Assert.AreEqual("Right Value", subField.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        public void SubField_SetValue_Exception_1()
        {
            bool save = SubFieldValue.ThrowOnVerify;
            SubFieldValue.ThrowOnVerify = true;
            try
            {
                SubField subField = new SubField('a')
                {
                    Value = "Wrong^Value"
                };
                Assert.IsNull(subField.Value);
            }
            finally
            {
                SubFieldValue.ThrowOnVerify = save;
            }
        }

        [TestMethod]
        public void SubField_ToJObject_1()
        {
            SubField subField = new SubField('a', "Value");

            JObject jObject = subField.ToJObject();
            Assert.AreEqual("a", jObject["code"].ToString());
            Assert.AreEqual("Value", jObject["value"].ToString());
        }

        [TestMethod]
        public void SubField_ToJson_1()
        {
            SubField subField = new SubField('a', "Value");

            string actual = subField.ToJson();
            const string expected = @"{'code':'a','value':'Value'}";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SubField_FromJObject_1()
        {
            JObject jObject = new JObject
                (
                    new JProperty("code", "a"),
                    new JProperty("value", "Value")
                );

            SubField subField = SubFieldUtility.FromJObject(jObject);
            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        public void SubField_FromJson_1()
        {
            const string text = @"{"
+@"  ""code"": ""a"","
+@"  ""value"": ""Value"""
+@"}";

            SubField subField = SubFieldUtility.FromJson(text);

            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        public void SubField_ToXml_1()
        {
            SubField subField = new SubField('a', "Value");
            string actual = XmlUtility.SerializeShort(subField);
            const string expected = "<subfield code=\"a\" value=\"Value\" />";
            Assert.AreEqual(expected, actual);
        }


        */

        [TestMethod]
        public void SubField_Verify_1()
        {
            var subField = new SubField();
            Assert.IsTrue(subField.Verify(false));

            subField = new SubField { Code = 'a' };
            Assert.IsTrue(subField.Verify(false));

            subField = new SubField ('a', "Title");
            Assert.IsTrue(subField.Verify(false));
        }


        /*

        [TestMethod]
        public void SubField_SetModified_1()
        {
            SubField subField = new SubField('a', "Title1");
            Assert.IsFalse(subField.Modified);
            subField.Value = "Title2";
            Assert.IsTrue(subField.Modified);
            subField.NotModified();
            Assert.IsFalse(subField.Modified);
        }

        [TestMethod]
        public void SubField_SetModified_2()
        {
            RecordField field = new RecordField("200");
            Assert.IsFalse(field.Modified);
            SubField subField = new SubField('a', "Title1");
            Assert.IsFalse(subField.Modified);
            field.SubFields.Add(subField);
            field.NotModified();
            subField.Value = "Title2";
            Assert.IsTrue(subField.Modified);
            Assert.IsTrue(field.Modified);
            subField.NotModified();
            Assert.IsFalse(subField.Modified);
        }

        [TestMethod]
        public void SubField_UserData_1()
        {
            SubField subField = new SubField();
            Assert.IsNull(subField.UserData);

            subField.UserData = "User data";
            Assert.AreEqual("User data", subField.UserData);
        }
        */

        [TestMethod]
        public void SubField_ToString_1()
        {
            var subField = new SubField();
            Assert.AreEqual(string.Empty, subField.ToString());

            subField = new SubField { Code = 'a' };
            Assert.AreEqual("^a", subField.ToString());

            subField = new SubField { Code = 'A' };
            Assert.AreEqual("^a", subField.ToString());

            subField = new SubField ('a', "Title");
            Assert.AreEqual("^aTitle", subField.ToString());
        }
    }
}
