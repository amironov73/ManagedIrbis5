// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Systematization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Systematization
{
    [TestClass]
    public sealed class BbkExceptionTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void BbkException_Construction_1()
        {
            var bbk = new BbkException();
            Assert.IsNull (bbk.InnerException);
        }

        [TestMethod]
        [Description ("Конструктор с сообщением")]
        public void BbkException_Construction_2()
        {
            const string message = "Some message";
            var bbk = new BbkException (message);
            Assert.AreEqual (message, bbk.Message);
            Assert.IsNull (bbk.InnerException);
        }

        [TestMethod]
        [Description ("Конструктор с сообщением и вложенным исключением")]
        public void BbkException_Construction_3()
        {
            const string message = "Some message";
            var inner = new Exception();
            var bbk = new BbkException (message, inner);
            Assert.AreEqual (message, bbk.Message);
            Assert.AreSame (inner, bbk.InnerException);
        }
    }
}
