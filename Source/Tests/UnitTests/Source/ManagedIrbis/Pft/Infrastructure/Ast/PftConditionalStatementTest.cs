// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Text;

using ManagedIrbis.Pft;
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
    public class PftConditionalStatementTest
    {
        private void _Execute
            (
                PftConditionalStatement node,
                string expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private PftConditionalStatement _GetNode()
        {
            return new PftConditionalStatement
            {
                Condition = new PftTrue(),
                ThenBranch =
                {
                    new PftUnconditionalLiteral("then")
                },
                ElseBranch =
                {
                    new PftUnconditionalLiteral("else")
                }
            };
        }

        [TestMethod]
        public void PftConditionalStatement_Construction_1()
        {
            var node = new PftConditionalStatement();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Condition);
            Assert.IsNotNull(node.ThenBranch);
            Assert.IsNotNull(node.ElseBranch);
        }

        [TestMethod]
        public void PftConditionalStatement_Construction_2()
        {
            var token = new PftToken(PftTokenKind.If, 1, 1, "if");
            var node = new PftConditionalStatement(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsTrue(node.ComplexExpression);
            Assert.IsNull(node.Condition);
            Assert.IsNotNull(node.ThenBranch);
            Assert.IsNotNull(node.ElseBranch);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftConditionalStatement_Clone_1()
        {
            var first = new PftConditionalStatement();
            var second = (PftConditionalStatement) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionalStatement_Clone_2()
        {
            var first = _GetNode();
            var second = (PftConditionalStatement) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                PftConditionalStatement node
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
        public void PftConditionalStatement_Compile_1()
        {
            var node = _GetNode();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftConditionalStatement_Compile_2()
        {
            var node = new PftConditionalStatement();
            _TestCompile(node);
        }

        [TestMethod]
        public void PftConditionalStatement_Execute_1()
        {
            var node = _GetNode();
            _Execute(node, "then");
        }

        [TestMethod]
        public void PftConditionalStatement_Execute_2()
        {
            var node = _GetNode();
            node.ThenBranch.Clear();
            node.ElseBranch.Clear();
            _Execute(node, "");
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionalStatement_Execute_3()
        {
            var node = new PftConditionalStatement();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftConditionalStatement_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("ConditionalStatement", info.Name);
        }

        [TestMethod]
        public void PftConditionalStatement_Optimize_1()
        {
            var node = new PftConditionalStatement();
            Assert.IsNull(node.Optimize());
        }

        [TestMethod]
        public void PftConditionalStatement_Optimize_2()
        {
            var node = _GetNode();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionalStatement_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nif true \nthen \'then\'\nelse \'else\'\nfi,\n", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftConditionalStatement_PrettyPrint_2()
        {
            var node = _GetNode();
            node.ThenBranch.Add(_GetNode());
            node.ElseBranch.Add(_GetNode());
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nif true \nthen\n  \'then\'\n  if true \n  then \'then\'\n  else \'else\'\n  fi,\nelse\n  \'else\'\n  if true \n  then \'then\'\n  else \'else\'\n  fi,\nfi,\n", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftConditionalStatement first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftConditionalStatement) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionalStatement_Serialization_1()
        {
            var node = new PftConditionalStatement();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftConditionalStatement_ToString_1()
        {
            var node = new PftConditionalStatement();
            Assert.AreEqual("if  then fi", node.ToString());

            node = _GetNode();
            Assert.AreEqual("if true then 'then' else 'else' fi", node.ToString());

            node.ElseBranch.Clear();
            Assert.AreEqual("if true then 'then' fi", node.ToString());
        }
    }
}
