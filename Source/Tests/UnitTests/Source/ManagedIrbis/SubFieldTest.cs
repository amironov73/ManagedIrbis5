﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

#nullable enable

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class SubFieldTest
    {
        [TestMethod]
        public void SubField_Constructor_1()
        {
            var subField = new SubField();
            Assert.AreEqual(SubField.NoCode, subField.Code);
            Assert.AreEqual(null, subField.Value);
            Assert.AreEqual(string.Empty, subField.ToString());

            subField = new SubField { Code = 'A', Value = "The value" };
            Assert.AreEqual('A', subField.Code);
            Assert.AreEqual("The value", subField.Value);
            Assert.AreEqual("^aThe value", subField.ToString());

            var clone = subField.Clone();
            Assert.AreEqual(subField.Code, clone.Code);
            Assert.AreEqual(subField.Value, clone.Value);
            Assert.AreEqual("^aThe value", clone.ToString());
        }

        [TestMethod]
        public void SubField_Decode_1()
        {
            var subField = new SubField();
            subField.Decode(string.Empty);
            Assert.AreEqual(SubField.NoCode, subField.Code);
            Assert.IsNull(subField.Value);
        }

        [TestMethod]
        public void SubField_Decode_2()
        {
            var subField = new SubField();
            subField.Decode("A");
            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual(0, subField.Value?.Length ?? -1);
        }

        [TestMethod]
        public void SubField_Decode_3()
        {
            var subField = new SubField();
            subField.Decode("AValue");
            Assert.AreEqual('a', subField.Code);
            Assert.AreEqual("Value", subField.Value);
        }

        /*
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

        [TestMethod]
        public void SubField_Serialization_1()
        {
            _TestSerialization(new SubField[0]);
            _TestSerialization(new SubField());
            _TestSerialization(new SubField(), new SubField());
            _TestSerialization(new SubField('a'), new SubField('b'));
            _TestSerialization(new SubField('a', "Hello"),
                new SubField('b', "World"));
        }

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
        [ExpectedException(typeof(ReadOnlyException))]
        public void SubField_ReadOnly_1()
        {
            SubField subField = new SubField('a', "Value", true, null);
            Assert.AreEqual("Value", subField.Value);
            subField.Value = "New value";
            Assert.AreEqual("Value", subField.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ReadOnlyException))]
        public void SubField_AsReadOnly_1()
        {
            SubField subField = new SubField('a', "Value")
                .AsReadOnly();
            Assert.AreEqual("Value", subField.Value);
            subField.Value = "New value";
            Assert.AreEqual("Value", subField.Value);
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

        [TestMethod]
        public void SubField_Field_1()
        {
            SubField subField = new SubField('a', "Title");
            Assert.IsNull(subField.Field);

            RecordField field = new RecordField("200");
            field.SubFields.Add(subField);
            Assert.AreEqual(field, subField.Field);
        }

        [TestMethod]
        public void SubField_Path_1()
        {
            SubField subField = new SubField();
            Assert.AreEqual(string.Empty, subField.Path);

            subField = new SubField('a', "Title");
            Assert.AreEqual("^a", subField.Path);

            RecordField field = new RecordField("200");
            field.SubFields.Add(subField);
            Assert.AreEqual("200/0^a", subField.Path);
        }
        */

        [TestMethod]
        public void SubField_Verify_1()
        {
            var subField = new SubField();
            Assert.IsFalse(subField.Verify(false));

            subField = new SubField { Code = 'a' };
            Assert.IsTrue(subField.Verify(false));

            subField = new SubField {Code = 'a', Value = "Title" };
            Assert.IsTrue(subField.Verify(false));
        }

        [TestMethod]
        public void SubField_Verify_2()
        {
            var subField = new SubField { Code = 'a', Value = "Hello^World" };
            Assert.IsFalse(subField.Verify(false));
        }

        /*
        [TestMethod]
        public void SubField_Compare_1()
        {
            SubField subField1 = new SubField('a');
            SubField subField2 = new SubField('b');
            Assert.IsTrue
                (
                    SubField.Compare(subField1, subField2) < 0
                );

            subField1 = new SubField('a', "Title1");
            subField2 = new SubField('a', "Title2");
            Assert.IsTrue
                (
                    SubField.Compare(subField1, subField2) < 0
                );

            subField1 = new SubField('a', "Title");
            subField2 = new SubField('a', "Title");
            Assert.IsTrue
                (
                    SubField.Compare(subField1, subField2) == 0
                );
        }

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

            subField = new SubField { Code = 'a', Value = "Title" };
            Assert.AreEqual("^aTitle", subField.ToString());
        }
    }
}
