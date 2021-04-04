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
    public class PftConditionParenthesisTest
    {
        private void _Execute
            (
                PftConditionParenthesis node,
                bool expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        private PftConditionParenthesis _GetNode()
        {
            var result = new PftConditionParenthesis
            {
                InnerCondition = new PftTrue()
            };

            return result;
        }

        [TestMethod]
        public void PftConditionParenthesis_Construction_1()
        {
            var node = new PftConditionParenthesis();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
        }

        [TestMethod]
        public void PftConditionParenthesis_Construction_2()
        {
            var token = new PftToken(PftTokenKind.LeftParenthesis, 1, 1, "(");
            var node = new PftConditionParenthesis(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.InnerCondition);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftConditionParenthesis_Clone_1()
        {
            var first = new PftConditionParenthesis();
            var second = (PftConditionParenthesis) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionParenthesis_Clone_2()
        {
            var first = _GetNode();
            var second = (PftConditionParenthesis) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                PftConditionParenthesis node
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
        public void PftConditionParenthesis_Compile_1()
        {
            var node = _GetNode();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftConditionParenthesis_Compile_2()
        {
            var node = new PftConditionParenthesis();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionParenthesis_Execute_1()
        {
            var node = new PftConditionParenthesis();
            _Execute(node, false);
        }

        [TestMethod]
        public void PftConditionParenthesis_Execute_2()
        {
            var node = _GetNode();
            _Execute(node, true);
        }

        [TestMethod]
        public void PftConditionParenthesis_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("ConditionParenthesis", info.Name);
        }

        [TestMethod]
        public void PftConditionParenthesis_Optimize_1()
        {
            var node = _GetNode();
            Assert.AreNotSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionParenthesis_Optimize_2()
        {
            var node = new PftConditionParenthesis();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionParenthesis_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("(true)", printer.ToString());
        }

        private void _TestSerialization
            (
                PftConditionParenthesis first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftConditionParenthesis) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionParenthesis_Serialization_1()
        {
            var node = new PftConditionParenthesis();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftConditionParenthesis_ToString_1()
        {
            var node = new PftConditionParenthesis();
            Assert.AreEqual("()", node.ToString());

            node = _GetNode();
            Assert.AreEqual("(true)", node.ToString());
        }
    }
}
