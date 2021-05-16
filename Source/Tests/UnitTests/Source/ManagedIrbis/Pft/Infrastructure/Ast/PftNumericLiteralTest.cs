// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftNumericLiteralTest
    {
        private void _Execute
            (
                PftNumericLiteral node,
                string expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = node.Value.ToInvariantString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftNumericLiteral_Construction_1()
        {
            var node = new PftNumericLiteral();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(0.0, node.Value);
        }

        [TestMethod]
        public void PftNumericLiteral_Construction_2()
        {
            var value = 123.45;
            var node = new PftNumericLiteral(value);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(value, node.Value);
        }

        [TestMethod]
        public void PftNumericLiteral_Construction_3()
        {
            var token = new PftToken(PftTokenKind.Number, 1, 1, "123.45");
            var node = new PftNumericLiteral(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(123.45, node.Value);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void PftNumericLiteral_Construction_4()
        {
            var token = new PftToken(PftTokenKind.Number, 1, 1, "123#45");
            Assert.IsNotNull(new PftNumericLiteral(token));
        }

        [TestMethod]
        public void PftNumericLiteral_Construction_5()
        {
            var token = new PftToken(PftTokenKind.Number, 1, 1, "123,45");
            var node = new PftNumericLiteral(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(12345, node.Value);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftNumericLiteral_CompareNode_1()
        {
            var left = new PftNumericLiteral(123.45);
            var right = new PftNumericLiteral(124.35);
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftNumericLiteral_Compile_1()
        {
            var node = new PftNumericLiteral(123.45);
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftNumericLiteral_Clone_1()
        {
            var left = new PftNumericLiteral(123.45);
            var right = (PftNumericLiteral)left.Clone();
            PftSerializationUtility.CompareNodes(left, right);
        }

        [TestMethod]
        public void PftNumericLiteral_Execute_1()
        {
            var node = new PftNumericLiteral();
            _Execute(node, "0");
        }

        [TestMethod]
        public void PftNumericLiteral_Execute_2()
        {
            var node = new PftNumericLiteral(123.45);
            _Execute(node, "123.45");
        }

        [TestMethod]
        public void PftNumericLiteral_Execute_3()
        {
            var node = new PftNumericLiteral(-123.45);
            _Execute(node, "-123.45");
        }

        [TestMethod]
        public void PftNumericLiteral_PrettyPrint_1()
        {
            var node = new PftNumericLiteral(123.45);
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("123.45", printer.ToString());
        }

        private void _TestSerialization
            (
                PftNumericLiteral first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftNumericLiteral) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftNumericLiteral_Serialization_1()
        {
            var node = new PftNumericLiteral();
            _TestSerialization(node);

            node = new PftNumericLiteral(123.45);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftNumericLiteral_ToString_1()
        {
            var node = new PftNumericLiteral();
            Assert.AreEqual("0", node.ToString());

            node = new PftNumericLiteral(123.45);
            Assert.AreEqual("123.45", node.ToString());
        }
    }
}
