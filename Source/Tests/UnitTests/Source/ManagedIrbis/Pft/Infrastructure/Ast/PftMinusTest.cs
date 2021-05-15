// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftMinusTest
    {
        private void _Execute
            (
                PftMinus node,
                 double expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = node.Value;
            Assert.AreEqual(expected, actual, 1E-6);
        }

        private PftMinus _GetNode()
        {
            return new PftMinus
            {
                Children =
                {
                    new PftNumericExpression
                    {
                        LeftOperand = new PftNumericLiteral(1),
                        Operation = "+",
                        RightOperand = new PftNumericExpression
                        {
                            LeftOperand = new PftNumericLiteral(2),
                            Operation = "*",
                            RightOperand = new PftNumericLiteral(3)
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void PftMinus_Construction_1()
        {
            var node = new PftMinus();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0.0, node.Value);
        }

        [TestMethod]
        public void PftMinus_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Minus, 1, 1, "-");
            var node = new PftMinus(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
            Assert.AreEqual(0.0, node.Value);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftMinus_Compile_1()
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
        public void PftNumericExpression_Compile_2()
        {
            var node = new PftMinus();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftMinus_Execute_1()
        {
            var node = new PftMinus();
            _Execute(node, 0);
        }

        [TestMethod]
        public void PftMinus_Execute_2()
        {
            var node = _GetNode();
            _Execute(node, -7);
        }

        [TestMethod]
        public void PftMinus_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("-(1 + 2 * 3)", printer.ToString());
        }

        [TestMethod]
        public void PftMinus_ToString_1()
        {
            var node = new PftMinus();
            Assert.AreEqual("-()", node.ToString());
        }

        [TestMethod]
        public void PftMinus_ToString_2()
        {
            var node = _GetNode();
            Assert.AreEqual("-(1+2*3)", node.ToString());
        }
    }
}
