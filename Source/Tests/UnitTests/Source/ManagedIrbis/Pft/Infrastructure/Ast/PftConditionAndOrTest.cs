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
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftConditionAndOrTest
    {
        private void _Execute
            (
                PftConditionAndOr node,
                bool expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        private PftConditionAndOr _GetNode()
        {
            var result = new PftConditionAndOr
            {
                LeftOperand = new PftTrue(),
                RightOperand = new PftFalse(),
                Operation = "or"
            };

            return result;
        }

        [TestMethod]
        public void PftConditionAndOr_Construction_1()
        {
            var node = new PftConditionAndOr();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNull(node.LeftOperand);
            Assert.IsNull(node.RightOperand);
            Assert.IsNull(node.Operation);
        }

        [TestMethod]
        public void PftConditionAndOr_Construction_2()
        {
            var token = new PftToken(PftTokenKind.And, 1, 1, "and");
            var node = new PftConditionAndOr(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
            Assert.IsNull(node.LeftOperand);
            Assert.IsNull(node.RightOperand);
            Assert.IsNull(node.Operation);
        }

        [TestMethod]
        public void PftConditionAndOr_Clone_1()
        {
            var first = new PftConditionAndOr();
            var second = (PftConditionAndOr) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionAndOr_Clone_2()
        {
            var first = _GetNode();
            var second = (PftConditionAndOr) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftConditionAndOr_Clone_3()
        {
            var first = _GetNode();
            var second = (PftConditionAndOr) first.Clone();
            second.Operation = "@@@";
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                PftConditionAndOr node
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
        public void PftConditionAndOr_Compile_1()
        {
            var node = _GetNode();
            node.Operation = "or";
            _TestCompile(node);

            node = _GetNode();
            node.Operation = "and";
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftConditionAndOr_Compile_2()
        {
            var node = _GetNode();
            node.Operation = "@@@";
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftConditionAndOr_Compile_3()
        {
            var node = new PftConditionAndOr { Operation = "@@@" };
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionAndOr_Execute_1()
        {
            var node = new PftConditionAndOr();
            _Execute(node, true);
        }

        [TestMethod]
        public void PftConditionAndOr_Execute_2()
        {
            var node = _GetNode();
            node.Operation = "or";
            _Execute(node, true);

            node.Operation = "and";
            _Execute(node, false);
        }

        [TestMethod]
        public void PftConditionAndOr_Execute_3()
        {
            var node = _GetNode();
            node.RightOperand = null;
            _Execute(node, true);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionAndOr_Execute_4()
        {
            var node = _GetNode();
            node.Operation = "@@@";
            _Execute(node, true);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftConditionAndOr_Execute_5()
        {
            var node = _GetNode();
            node.Operation = null;
            _Execute(node, true);
        }

        [TestMethod]
        public void PftConditionAndOr_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("ConditionAndOr", info.Name);
        }

        [TestMethod]
        public void PftConditionAndOr_Optimize_1()
        {
            var node = _GetNode();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftConditionAndOr_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("true or false ", printer.ToString());
        }

        private void _TestSerialization
            (
                PftConditionAndOr first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftConditionAndOr) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftConditionAndOr_Serialization_1()
        {
            var node = new PftConditionAndOr();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftConditionAndOr_ToString_1()
        {
            var node = new PftConditionAndOr();
            Assert.AreEqual("  ", node.ToString());

            node = _GetNode();
            node.Operation = "or";
            Assert.AreEqual("true or false", node.ToString());

            node.Operation = "and";
            Assert.AreEqual("true and false", node.ToString());
        }
    }
}
