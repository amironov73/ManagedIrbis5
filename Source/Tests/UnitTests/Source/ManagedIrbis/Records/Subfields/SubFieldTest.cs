// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.Text.Json;
using AM;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

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
            Assert.IsTrue(subField.Value.IsEmpty());
            Assert.AreEqual(string.Empty, subField.ToString());
            Assert.IsTrue(subField.RepresentsValue);
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
            Assert.AreEqual(value, subField.Value);
            Assert.AreEqual("^aThe value", subField.ToString());
            Assert.IsFalse(subField.RepresentsValue);
            Assert.IsNull(subField.Field);
            Assert.IsFalse(subField.ReadOnly);
            Assert.IsTrue(subField.Verify(false));
        }

        [TestMethod]
        [Description("Неверное значение подполя (ReadOnlyMemory)")]
        [ExpectedException(typeof(ArgumentException))]
        public void SubField_Constructor_3()
        {
            var subField = new SubField('a', "Wrong^Value".AsMemory());
            Assert.IsNotNull(subField);
        }

        [TestMethod]
        [Description("Неверное значение подполя (String)")]
        [ExpectedException(typeof(VerificationException))]
        public void SubField_Constructor_4()
        {
            var subField = new SubField('a', "Wrong^Value");
            Assert.IsNotNull(subField);
        }

        [TestMethod]
        [Description("Конструктор со string")]
        public void SubField_Constructor_5()
        {
            const string value = "The value";
            var subField = new SubField ('A', value);
            Assert.AreEqual('A', subField.Code);
            Assert.AreEqual(value, subField.Value);
            Assert.AreEqual("^aThe value", subField.ToString());
            Assert.IsFalse(subField.RepresentsValue);
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
            subField.Decode(string.Empty);
            Assert.AreEqual(SubField.NoCode, subField.Code);
            Assert.IsTrue(subField.Value.IsEmpty());
        }

        [TestMethod]
        [Description("Декодирование строки из одного символа")]
        public void SubField_Decode_2()
        {
            var subField = new SubField();
            subField.Decode("A");
            Assert.AreEqual('a', subField.Code);
            Assert.IsNull(subField.Value);
        }

        [TestMethod]
        [Description("Декодирование строки из нескольких символов")]
        public void SubField_Decode_3()
        {
            var subField = new SubField();
            subField.Decode("AValue");
            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(VerificationException))]
        [Description("Декодирование строки, содержащей недопустимый символ")]
        public void SubField_Decode_4()
        {
            var subField = new SubField();
            subField.Decode("Wrong^value");
        }

        [Ignore]
        [TestMethod]
        [Description("Поле, которому принадлежит данное подполе")]
        public void SubField_Field_1()
        {
            var subField = new SubField('a', "Title");
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
            var array1 = subFields;
            var bytes = array1.SaveToMemory();

            var array2 = bytes
                    .RestoreArrayFromMemory<SubField>();

            Assert.AreEqual(array1.Length, array2.Length);
            for (var i = 0; i < array1.Length; i++)
            {
                Assert.AreEqual
                    (
                        0,
                        SubField.Compare(array1[i], array2[i])
                    );
            }
        }

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

        */

        [TestMethod]
        public void SubField_ToJson_1()
        {
            var subField = new SubField('a', "Value");
            var options = new JsonSerializerOptions()
            {
                Converters = { new SubFieldJsonConverter() }
            };
            var actual = JsonSerializer.Serialize(subField, options);
            const string expected = "{\"code\":\"a\",\"value\":\"Value\"}";
            Assert.AreEqual(expected, actual);
        }

        /*

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

        */

        [TestMethod]
        public void SubField_FromJson_1()
        {
            const string text = "{\"code\":\"a\",\"value\":\"Value\"}";
            var options = new JsonSerializerOptions()
            {
                Converters = { new SubFieldJsonConverter() }
            };

            var subField = JsonSerializer.Deserialize<SubField>(text, options);
            Assert.IsNotNull(subField);
            Assert.AreEqual('a', subField!.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        [Description("Проверяем наш метод WriteXml")]
        public void SubField_ToXml_1()
        {
            var subField = new SubField('a', "Value");
            var actual = XmlUtility.SerializeShort(subField);
            const string expected = "<subfield code=\"a\" value=\"Value\" />";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [Description("Проверяем наш метод ReadXml")]
        public void SubField_FromXml_1()
        {
            const string text = "<subfield code=\"a\" value=\"Value\" />";
            var subField = XmlUtility.DeserializeString<SubField>(text);
            Assert.IsNotNull(subField);
            Assert.AreEqual('a', subField!.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        [Description("Верификация подполя")]
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
        [Description("Преобразование в строку")]
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
