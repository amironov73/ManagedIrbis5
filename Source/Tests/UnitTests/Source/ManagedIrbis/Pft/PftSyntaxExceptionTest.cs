// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using AM.Text;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftSyntaxExceptionTest
    {
        [TestMethod]
        public void PftSyntaxException_Construction_1()
        {
            var exception = new PftSyntaxException();
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_2()
        {
            var token = new PftToken
                (
                    PftTokenKind.UnconditionalLiteral,
                    1,
                    1,
                    "Hello"
                );
            var exception = new PftSyntaxException(token);
            Assert.AreEqual("Unexpected token: UnconditionalLiteral (1,1): Hello", exception.Message);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_3()
        {
            PftToken[] tokens =
            {
                new (PftTokenKind.UnconditionalLiteral, 1, 1, "Hello"),
                new (PftTokenKind.V, 2, 1, "v200"),
                new (PftTokenKind.Slash, 3, 1, "/")
            };
            var tokenList = new PftTokenList(tokens);
            var exception = new PftSyntaxException(tokenList);
            Assert.AreEqual
                (
                    "Unexpected end of file:UnconditionalLiteral (1,1): Hello V (2,1): v200 Slash (3,1): /",
                    exception.Message
                );
        }

        [TestMethod]
        public void PftSyntaxException_Construction_4()
        {
            PftToken[] tokens =
            {
                new (PftTokenKind.UnconditionalLiteral, 1, 1, "Hello"),
                new (PftTokenKind.V, 2, 1, "v200"),
                new (PftTokenKind.Slash, 3, 1, "/")
            };
            var tokenList = new PftTokenList(tokens);
            var innerException = new Exception();
            var exception = new PftSyntaxException(tokenList, innerException);
            Assert.AreEqual
                (
                    "Unexpected end of file: UnconditionalLiteral (1,1): Hello V (2,1): v200 Slash (3,1): /",
                    exception.Message
                );
            Assert.AreSame
                (
                    innerException,
                    exception.InnerException
                );
        }

        [TestMethod]
        public void PftSyntaxException_Construction_5()
        {
            const string message = "Message";
            var innerException = new Exception();
            var exception = new PftSyntaxException(message, innerException);
            Assert.AreEqual(message, exception.Message);
            Assert.AreSame(innerException, exception.InnerException);
        }

        [TestMethod]
        public void PftSyntaxException_Construction_6()
        {
            var token = new PftToken
                (
                    PftTokenKind.UnconditionalLiteral,
                    1,
                    1,
                    "Hello"
                );
            var innerException = new Exception();
            var exception = new PftSyntaxException(token, innerException);
            Assert.AreEqual
                (
                    "Unexpected token: UnconditionalLiteral (1,1): Hello",
                    exception.Message
                );
            Assert.AreSame
                (
                    innerException,
                    exception.InnerException
                );
        }

        [TestMethod]
        public void PftSyntaxException_Construction_7()
        {
            var navigator = new TextNavigator("Hello\r\nWorld");
            navigator.ReadLine();
            var exception = new PftSyntaxException(navigator);
            Assert.AreEqual
                (
                    "Syntax error at: Line=2, Column=1",
                    exception.Message
                );
        }

        [TestMethod]
        public void PftSyntaxException_Construction_8()
        {
            var token = new PftToken
                (
                    PftTokenKind.UnconditionalLiteral,
                    1,
                    1,
                    "Hello"
                );
            var literal = new PftUnconditionalLiteral(token);
            var exception = new PftSyntaxException(literal);
            Assert.AreEqual
                (
                    "Syntax error at: 'Hello'",
                    exception.Message
                );
        }
    }
}
