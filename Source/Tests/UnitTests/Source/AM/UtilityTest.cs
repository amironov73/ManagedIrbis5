// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

namespace UnitTests.AM
{
    [TestClass]
    public class UtilityTest
    {
        [TestMethod]
        public void Utility_ThrowIfNull_1()
        {
            string text = "Hello";
            Assert.AreSame(text, text.ThrowIfNull());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Utility_ThrowIfNull_2()
        {
            string text = null;

            // ReSharper disable ExpressionIsAlwaysNull
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            text.ThrowIfNull();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore ExpressionIsAlwaysNull
        }

        [TestMethod]
        public void Utility_ToVisibleString_1()
        {
            string text = "Hello";
            Assert.AreEqual("Hello", text.ToVisibleString());

            text = null;

            // ReSharper disable ExpressionIsAlwaysNull
            Assert.AreEqual("(null)", text.ToVisibleString());
            // ReSharper restore ExpressionIsAlwaysNull
        }
    }
}
