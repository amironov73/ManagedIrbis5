// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftConditionNotTest
    {
        private void _Execute
            (
                PftConditionNot node,
                bool expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        private PftConditionNot _GetNode()
        {
            var result = new PftConditionNot
            {
                InnerCondition = new PftTrue()
            };

            return result;
        }

        [TestMethod]
        public void PftConditionNot_Construction_1()
        {
            var node = new PftConditionNot();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftConditionNot_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Not, 1, 1, "not");
            var node = new PftConditionNot(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftConditionNot_Clone_1()
        {
            var first = new PftConditionNot();
            var second = (PftConditionNot) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionNot_Clone_2()
        {
            var first = _GetNode();
            var second = (PftConditionNot) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                PftConditionNot node
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
        public void PftConditionNot_Compile_1()
        {
            var node = _GetNode();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftConditionNot_Compile_2()
        {
            var node = new PftConditionNot();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionNot_Execute_1()
        {
            var node = new PftConditionNot();
            _Execute(node, false);
        }

        [TestMethod]
        public void PftConditionNot_Execute_2()
        {
            var node = _GetNode();
            _Execute(node, false);
        }

        [TestMethod]
        public void PftConditionNot_Optimize_1()
        {
            var node = _GetNode();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionNot_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("not true ", printer.ToString());
        }

        private void _TestSerialization
            (
                PftConditionNot first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftConditionNot) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionNot_Serialization_1()
        {
            var node = new PftConditionNot();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftConditionNot_ToString_1()
        {
            var node = new PftConditionNot();
            Assert.AreEqual(" not ", node.ToString());

            node = _GetNode();
            Assert.AreEqual(" not true", node.ToString());
        }
    }
}
