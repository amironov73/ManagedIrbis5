﻿// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Quality;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Quality
{
    [TestClass]
    public class FieldDefectTest
    {
        private static FieldDefect _GetDefect()
        {
            var result = new FieldDefect
            {
                Field = 200,
                Message = "Отсутствует поле 200",
                Damage = 100,
                UserData = "User data"
            };

            return result;
        }

        private void _TestSerialization
            (
                FieldDefect first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<FieldDefect>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Damage, second!.Damage);
            Assert.AreEqual(first.Field, second.Field);
            Assert.AreEqual(first.Repeat, second.Repeat);
            Assert.AreEqual(first.Message, second.Message);
            Assert.AreEqual(first.Subfield, second.Subfield);
            Assert.AreEqual(first.Value, second.Value);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void FieldDefect_Serialization_1()
        {
            var defect = new FieldDefect();
            _TestSerialization(defect);

            defect = _GetDefect();
            _TestSerialization(defect);
        }

        [TestMethod]
        public void FieldDefect_ToXml_1()
        {
            var defect = new FieldDefect();
            var actual = XmlUtility.SerializeShort(defect);
            Assert.AreEqual("<defect field=\"0\" />", actual);

            defect = _GetDefect();
            actual = XmlUtility.SerializeShort(defect);
            Assert.AreEqual("<defect field=\"200\" message=\"Отсутствует поле 200\" damage=\"100\" />", actual);
        }

        [TestMethod]
        public void FieldDefect_ToJson_1()
        {
            var defect = new FieldDefect();
            var actual = JsonUtility.SerializeShort(defect);
            Assert.AreEqual("{\"field\":0}", actual);

            //defect = _GetDefect();
            //actual = JsonUtility.SerializeShort(defect);
            //Assert.AreEqual("{\"field\":200,\"message\":\"Отсутствует поле 200\",\"damage\":100}", actual);
        }

        [TestMethod]
        public void FieldDefect_Verify_1()
        {
            var defect = new FieldDefect();
            Assert.IsFalse(defect.Verify(false));

            defect = _GetDefect();
            Assert.IsTrue(defect.Verify(false));
        }

        [TestMethod]
        public void FieldDefect_ToString_1()
        {
            var defect = new FieldDefect();
            Assert.AreEqual("Field: 0, Value: (null), Message: (null)", defect.ToString());

            defect = _GetDefect();
            Assert.AreEqual("Field: 200, Value: (null), Message: Отсутствует поле 200", defect.ToString());
        }
    }
}
