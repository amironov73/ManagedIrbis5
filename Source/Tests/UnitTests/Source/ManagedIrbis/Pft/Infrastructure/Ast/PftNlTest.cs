// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftNlTest
    {
        private void _Execute
            (
                PftNl node,
                string expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftNl_Construction_1()
        {
            var node = new PftNl();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftNl_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Nl, 1, 1, "nl");
            var node = new PftNl(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftNl_Compile_1()
        {
            var compiler = new PftCompiler();
            var program = new PftProgram();
            var node = new PftNl();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftNl_Execute_1()
        {
            var node = new PftNl();
            _Execute(node, "\n");
        }

        [TestMethod]
        public void PftNl_PrettyPrint_1()
        {
            var printer = new PftPrettyPrinter();
            var node = new PftNl();
            node.PrettyPrint(printer);
            Assert.AreEqual(" nl ", printer.ToString());
        }

        [TestMethod]
        public void PftNl_ToString_1()
        {
            var node = new PftNl();
            Assert.AreEqual("nl", node.ToString());
        }
    }
}
