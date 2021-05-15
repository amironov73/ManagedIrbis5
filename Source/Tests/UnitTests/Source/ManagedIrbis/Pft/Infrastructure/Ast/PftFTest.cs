// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM.Text;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftFTest
    {
        private void _Execute
            (
                PftF node,
                string expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private PftF _GetNode()
        {
            return new PftF
            {
                Argument1 = new PftNumericLiteral(Math.PI),
                Argument2 = new PftNumericLiteral(10),
                Argument3 = new PftNumericLiteral(8)
            };
        }

        [TestMethod]
        public void PftF_Construction_1()
        {
            var node = new PftF();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftF_Construction_2()
        {
            var token = new PftToken(PftTokenKind.F, 1, 1, "f");
            var node = new PftF(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftF_Clone_1()
        {
            var first = new PftF();
            var second = (PftF) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF_Clone_2()
        {
            var first = _GetNode();
            var second = (PftF) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF_Compile_1()
        {
            var node = _GetNode();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftF_Compile_2()
        {
            var node = new PftF();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftF_Compile_3()
        {
            var node = _GetNode();
            node.Argument3 = null;
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftF_Compile_4()
        {
            var node = _GetNode();
            node.Argument3 = null;
            node.Argument2 = null;
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftF_Execute_1()
        {
            var node = new PftF();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftF_Execute_2()
        {
            var node = _GetNode();
            _Execute(node, "3.14159265");

            node.Argument3 = new PftNumericLiteral(3);
            _Execute(node, "     3.142");

            node.Argument3 = new PftNumericLiteral(0);
            _Execute(node, "         3");

            node.Argument2 = new PftNumericLiteral(3);
            _Execute(node, "  3");

            node.Argument2 = new PftNumericLiteral(0);
            _Execute(node, "3");

            node.Argument1 = new PftNumericLiteral(123.45);
            _Execute(node, "123");
        }

        [TestMethod]
        public void PftF_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("F", info.Name);
        }

        private void _TestSerialization
            (
                PftF first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftF) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF_Serialization_1()
        {
            var node = new PftF();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftF_ToString_1()
        {
            var node = new PftF();
            Assert.AreEqual("f()", node.ToString());
        }

        [TestMethod]
        public void PftF_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("f(3.141592653589793,10,8)", node.ToString());
        }

        [TestMethod]
        public void PftF_ToString_3()
        {
            var node = _GetNode();
            node.Argument3 = null;
            Assert.AreEqual("f(3.141592653589793,10)", node.ToString());
        }
    }
}
