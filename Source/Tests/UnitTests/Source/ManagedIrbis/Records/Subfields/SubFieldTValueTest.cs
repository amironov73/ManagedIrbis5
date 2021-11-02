// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Subfields
{
    [TestClass]
    public class SubFieldTValueTest
    {
        [TestMethod]
        [Description ("Валидные значения подполя")]
        public void SubFieldValue_IsValidValue_1()
        {
            Assert.IsTrue (SubFieldValue.IsValidValue (null));
            Assert.IsTrue (SubFieldValue.IsValidValue (ReadOnlySpan<char>.Empty));
            Assert.IsTrue (SubFieldValue.IsValidValue ("A"));
            Assert.IsTrue (SubFieldValue.IsValidValue (" "));
        }

        [TestMethod]
        [Description ("Невалидные значения подполя")]
        public void SubFieldValue_IsValidValue_2()
        {
            Assert.IsFalse (SubFieldValue.IsValidValue ("^"));
            Assert.IsFalse (SubFieldValue.IsValidValue ("A^B"));
        }

        private void _TestNormalize
            (
                string? expected
            )
        {
            var actual = SubFieldValue.Normalize (expected);

            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Нормализация значения подполя")]
        public void SubFieldValue_Normalize_1()
        {
            _TestNormalize (null);
            _TestNormalize (string.Empty);
            _TestNormalize ("A");
            _TestNormalize (" ");
        }

        [TestMethod]
        [Description ("Верификация: валидные значения подполя")]
        public void SubFieldValue_Verify_1()
        {
            Assert.IsTrue (SubFieldValue.Verify (null));
            Assert.IsTrue (SubFieldValue.Verify (ReadOnlySpan<char>.Empty));
            Assert.IsTrue (SubFieldValue.Verify ("A"));
            Assert.IsTrue (SubFieldValue.Verify (" "));
        }

        [TestMethod]
        [Description ("Верификация: невалидные значения подполя")]
        public void SubFieldValue_Verify_2()
        {
            Assert.IsFalse (SubFieldValue.Verify ("^"));
            Assert.IsFalse (SubFieldValue.Verify ("A^B"));
        }

        [TestMethod]
        [ExpectedException (typeof (VerificationException))]
        [Description ("Исключения при верификации значения подполя")]
        public void SubFieldValue_ThrowOnVerify_1()
        {
            var save = SubFieldValue.ThrowOnVerify;
            SubFieldValue.ThrowOnVerify = true;
            try
            {
                SubFieldValue.Verify ("^");
            }
            finally
            {
                SubFieldValue.ThrowOnVerify = save;
            }
        }
    }
}
