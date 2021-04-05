// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftF2Test
    {
        private void _Execute
            (
                PftFmt node,
                string expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private PftFmt _GetNode()
        {
            return new PftFmt
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F2")
                }
            };
        }

        [TestMethod]
        public void PftF2_Construction_1()
        {
            var node = new PftFmt();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Number);
            Assert.IsNotNull(node.Format);
        }

        [TestMethod]
        public void PftF2_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Fmt, 1, 1, "f2");
            var node = new PftFmt(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNull(node.Number);
            Assert.IsNotNull(node.Format);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftF2_Clone_1()
        {
            var first = new PftFmt();
            var second = (PftFmt) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF2_Clone_2()
        {
            var first = _GetNode();
            var second = (PftFmt) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                PftFmt node
            )
        {
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftF2_Compile_1()
        {
            var node = _GetNode();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftF2_Compile_2()
        {
            var node = new PftFmt();
            _TestCompile(node);
        }

        [TestMethod]
        public void PftF2_Execute_1()
        {
            var node = new PftFmt();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftF2_Execute_2()
        {
            var node = _GetNode();
            _Execute(node, "3.14");

            node.Format.Clear();
            _Execute(node, "3.141592653589793");
        }

        [TestMethod]
        public void PftF2_Execute_3()
        {
            var node = new PftFmt
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F"),
                    new PftUnconditionalLiteral("2")
                }
            };
            _Execute(node, "3.14");
        }

        [TestMethod]
        public void PftF2_Execute_4()
        {
            var node = new PftFmt
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F"),
                    new PftComma(),
                    new PftUnconditionalLiteral("2")
                }
            };
            _Execute(node, "3.14");
        }

        [TestMethod]
        public void PftF2_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Fmt", info.Name);
        }

        private void _TestSerialization
            (
                PftFmt first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftFmt) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftF2_Serialization_1()
        {
            var node = new PftFmt();
            _TestSerialization(node);

            node = new PftFmt();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftF2_ToString_1()
        {
            var node = new PftFmt();
            Assert.AreEqual("fmt(,)", node.ToString());
        }

        [TestMethod]
        public void PftF2_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("fmt(3.141592653589793,'F2')", node.ToString());

            node.Format.Clear();
            Assert.AreEqual("fmt(3.141592653589793,)", node.ToString());
        }

        [TestMethod]
        public void PftF2_ToString_3()
        {
            var node = new PftFmt
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F"),
                    new PftUnconditionalLiteral("2")
                }
            };
            Assert.AreEqual("fmt(3.141592653589793,'F' '2')", node.ToString());
        }

        [TestMethod]
        public void PftF2_ToString_4()
        {
            var node = new PftFmt
            {
                Number = new PftNumericLiteral(Math.PI),
                Format =
                {
                    new PftUnconditionalLiteral("F"),
                    new PftComma(),
                    new PftUnconditionalLiteral("2")
                }
            };
            Assert.AreEqual("fmt(3.141592653589793,'F' , '2')", node.ToString());
        }
    }
}

