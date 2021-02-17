﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class SubFieldCodeTest
    {
        [TestMethod]
        public void TestSubFieldCode_IsValidCode()
        {
            Assert.IsTrue(SubFieldCode.IsValidCode('C'));
            Assert.IsTrue(SubFieldCode.IsValidCode('c'));
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
        public void TestSubFieldCode_Normalize()
        {
            _TestNormalize('\0', '\0');
            _TestNormalize('0', '0');
            _TestNormalize('C', 'c');
        }

        [TestMethod]
        public void TestSubFieldCode_Verify()
        {
            Assert.IsTrue(SubFieldCode.Verify('c'));
            Assert.IsFalse(SubFieldCode.Verify('\0'));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSubFieldCode_Verify_Exception()
        {
            Assert.IsFalse(SubFieldCode.Verify('^', true));
        }

    }
}
