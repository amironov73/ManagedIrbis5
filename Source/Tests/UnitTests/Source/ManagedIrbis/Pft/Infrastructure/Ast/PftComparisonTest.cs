// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
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
    public class PftComparisonTest
    {
        private void _Execute
            (
                PftComparison node,
                bool expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        private PftComparison _GetTextNode()
        {
            return new PftComparison
            {
                LeftOperand = new PftUnconditionalLiteral("Hello"),
                Operation = ":",
                RightOperand = new PftUnconditionalLiteral("ll")
            };
        }

        private PftComparison _GetNumericNode()
        {
            return new PftComparison
            {
                LeftOperand = new PftNumericLiteral(123.45),
                Operation = "<",
                RightOperand = new PftNumericLiteral(0.0)
            };
        }

        [TestMethod]
        public void PftComparison_Construction_1()
        {
            var node = new PftComparison();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftBlank_Construction_2()
        {
            var token = new PftToken(PftTokenKind.If, 1, 1, "if");
            var node = new PftComparison(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftComparison_Construction_3()
        {
            var node = new PftComparison()
            {
                Operation = "~"
            };
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftComparison_Clone_1()
        {
            var first = new PftComparison();
            var second = (PftComparison) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftComparison_Clone_2()
        {
            var first = _GetTextNode();
            var second = (PftComparison) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftComparison_Clone_3()
        {
            var first = _GetTextNode();
            var second = (PftComparison) first.Clone();
            second.Operation = "!";
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftComparison_Compile_1()
        {
            var node = _GetTextNode();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftComparison_Compile_2()
        {
            var node = _GetNumericNode();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftComparison_Compile_3()
        {
            var node = _GetTextNode();
            node.Operation = null;
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftComparison_Execute_1()
        {
            var node = _GetTextNode();
            node.LeftOperand = null;
            _Execute(node, false);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftComparison_Execute_2()
        {
            var node = _GetTextNode();
            node.RightOperand = null;
            _Execute(node, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftComparison_Execute_3()
        {
            var node = _GetTextNode();
            node.Operation = null;
            _Execute(node, false);
        }

        [TestMethod]
        public void PftComparison_Execute_4()
        {
            var node = _GetTextNode();

            // 'Hello' : 'll'
            node.Operation = ":";
            _Execute(node, true);

            // 'Hello' :: 'll'
            node.Operation = "::";
            _Execute(node, true);

            // 'Hello' = 'll'
            node.Operation = "=";
            _Execute(node, false);

            // 'Hello' != 'll'
            node.Operation = "!=";
            _Execute(node, true);

            // 'Hello' <> 'll'
            node.Operation = "<>";
            _Execute(node, true);

            // 'Hello' < 'll'
            node.Operation = "<";
            _Execute(node, true);

            // 'Hello' <= 'll'
            node.Operation = "<=";
            _Execute(node, true);

            // 'Hello' > 'll'
            node.Operation = ">";
            _Execute(node, false);

            // 'Hello' >= 'll'
            node.Operation = ">=";
            _Execute(node, false);

            // 'Hello' == 'HELLO'
            node.LeftOperand = new PftUnconditionalLiteral("Hello");
            node.RightOperand = new PftUnconditionalLiteral("HELLO");
            node.Operation = "==";
            _Execute(node, false);

            // 'Hello' == 'HELLO'
            node.Operation = "!==";
            _Execute(node, true);

            // 'Hello' ~ '^He'
            node.LeftOperand = new PftUnconditionalLiteral("Hello");
            node.RightOperand = new PftUnconditionalLiteral("^He");
            node.Operation = "~";
            _Execute(node, true);

            // 'Hello' ~~ '^He'
            node.Operation = "~~";
            _Execute(node, true);

            // 'Hello' !~ '^He'
            node.Operation = "!~";
            _Execute(node, false);

            // 'Hello' !~~ '^He'
            node.Operation = "!~~";
            _Execute(node, false);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftComparison_Execute_4a()
        {
            var node = _GetTextNode();
            node.Operation = "@@@";
            _Execute(node, false);
        }

        [TestMethod]
        public void PftComparison_Execute_5()
        {
            var node = _GetNumericNode();

            // 123.45 < 0
            node.Operation = "<";
            _Execute(node, false);

            // 123.45 <= 0
            node.Operation = "<=";
            _Execute(node, false);

            // 123.45 = 0
            node.Operation = "=";
            _Execute(node, false);

            // 123.45 != 0
            node.Operation = "!=";
            _Execute(node, true);

            // 123.45 <> 0
            node.Operation = "<>";
            _Execute(node, true);

            // 123.45 > 0
            node.Operation = ">";
            _Execute(node, true);

            // 123.45 >= 0
            node.Operation = ">=";
            _Execute(node, true);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftComparison_Execute_5a()
        {
            var node = _GetNumericNode();
            node.Operation = "@@@";
            _Execute(node, false);
        }

        [TestMethod]
        public void PftComparison_GetNodeInfo_1()
        {
            var node = _GetTextNode();
            var info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("Comparison", info.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftComparison_Optimize_1()
        {
            var node = new PftComparison();
            node.Optimize();
        }

        [TestMethod]
        public void PftComparison_Optimize_2()
        {
            var node = _GetTextNode();
            Assert.AreSame(node, node.Optimize());
        }

        [TestMethod]
        public void PftComparison_PrettyPrint_1()
        {
            var node = _GetTextNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("'Hello':'ll'", printer.ToString());
        }

        [TestMethod]
        public void PftComparison_PrettyPrint_2()
        {
            var node = _GetNumericNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("123.45<0", printer.ToString());
        }

        private void _TestSerialization
            (
                PftComparison first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftComparison) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftComparison_Serialization_1()
        {
            var node = new PftComparison();
            _TestSerialization(node);

            node = _GetTextNode();
            _TestSerialization(node);

            node = _GetNumericNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftComparison_ToString_1()
        {
            var node = new PftComparison();
            Assert.AreEqual("", node.ToString());
        }

        [TestMethod]
        public void PftComparison_ToString_2()
        {
            var node = _GetTextNode();
            Assert.AreEqual("'Hello':'ll'", node.ToString());
        }

        [TestMethod]
        public void PftComparison_ToString_3()
        {
            var node = _GetNumericNode();
            Assert.AreEqual("123.45<0", node.ToString());
        }
    }
}
