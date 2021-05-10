// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Subfields
{
    [TestClass]
    public class SubFieldCodeTest
    {
        [TestMethod]
        [Description("Валидные коды подполя")]
        public void SubFieldCode_IsValidCode_1()
        {
            Assert.IsTrue(SubFieldCode.IsValidCode('C'));
            Assert.IsTrue(SubFieldCode.IsValidCode('c'));
        }

        [TestMethod]
        [Description("Невалидные коды подполя")]
        public void SubFieldCode_IsValidCode_2()
        {
            Assert.IsFalse(SubFieldCode.IsValidCode('\0'));
            Assert.IsFalse(SubFieldCode.IsValidCode('\u042F'));
        }

        private void _TestNormalize
            (
                char source,
                char expected
            )
        {
            char actual = SubFieldCode.Normalize(source);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [Description("Нормализация кодов подполя")]
        public void SubFieldCode_Normalize_1()
        {
            _TestNormalize('\0', '\0');
            _TestNormalize('0', '0');
            _TestNormalize('C', 'c');
        }

        [TestMethod]
        [Description("Верификация: валидные коды подполей")]
        public void SubFieldCode_Verify_1()
        {
            Assert.IsTrue(SubFieldCode.Verify('c'));
            Assert.IsTrue(SubFieldCode.Verify('1'));
        }

        [TestMethod]
        [Description("Верификация: невалидные коды подполей")]
        public void SubFieldCode_Verify_2()
        {
            Assert.IsFalse(SubFieldCode.Verify('\0'));
            Assert.IsFalse(SubFieldCode.Verify('\t'));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [Description("Исключения при верификации кодов подполей")]
        public void SubFieldCode_Verify_3()
        {
            Assert.IsFalse(SubFieldCode.Verify('^', true));
        }

    }
}
