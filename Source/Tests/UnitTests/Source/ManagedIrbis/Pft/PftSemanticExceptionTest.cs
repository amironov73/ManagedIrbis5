// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Pft;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftSemanticExceptionTest
    {
        [TestMethod]
        public void PftSemanticException_Construction_1()
        {
            var exception = new PftSemanticException();
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void PftSemanticException_Construction_2()
        {
            const string message = "Message";
            var exception = new PftSemanticException(message);
            Assert.AreEqual(message, exception.Message);
        }

        [TestMethod]
        public void PftSemanticException_Construction_3()
        {
            const string message = "Message";
            var innerException = new Exception();
            var exception = new PftSemanticException(message, innerException);
            Assert.AreEqual(message, exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
        }
    }
}
