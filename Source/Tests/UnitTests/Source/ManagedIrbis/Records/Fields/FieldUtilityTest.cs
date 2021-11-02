// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Runtime;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Fields
{
    [TestClass]
    public sealed class FieldUtilityTest
    {
        [TestMethod]
        [Description ("Все подполя: пустой массив")]
        public void FieldUtility_AllFields_1()
        {
            CollectionAssert.AreEqual
                (
                    Array.Empty<SubField>(),
                    Array.Empty<Field>().AllSubFields()
                );
        }

        [TestMethod]
        [Description ("Все подполя: массив из единственного поля")]
        public void FieldUtility_AllFields_2()
        {
            var field = new Field
                (
                    100,
                    'a', "SubFieldA",
                    'b', "SubFieldB"
                );
            Assert.AreEqual
                (
                    field.Subfields.Count,
                    new [] { field }.AllSubFields().Length
                );
        }

        [TestMethod]
        [Description ("Все подполя: массив из двух полей")]
        public void FieldUtility_AllFields_3()
        {
            var field1 = new Field
                (
                    100,
                    'a', "SubFieldA",
                    'b', "SubFieldB"
                );
            var field2 = new Field
                (
                    200,
                    'c', "SubFieldC",
                    'd', "SubFieldD"
                );
            Assert.AreEqual
                (
                    field1.Subfields.Count + field2.Subfields.Count,
                    new [] { field1, field2 }.AllSubFields().Length
                );
        }

    }
}
