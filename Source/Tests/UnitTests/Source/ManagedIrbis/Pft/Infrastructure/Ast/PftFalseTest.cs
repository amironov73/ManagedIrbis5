// ReSharper disable CheckNamespace
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
    public class PftFalseTest
    {
        [TestMethod]
        public void PftFalse_Construction_1()
        {
            var node = new PftFalse();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
        }

        [TestMethod]
        public void PftFalse_Construction_2()
        {
            var token = new PftToken(PftTokenKind.False, 1, 1, "False");
            var node = new PftFalse(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFalse_Compile_1()
        {
            var compiler = new PftCompiler();
            var program = new PftProgram();
            program.Children.Add(new PftFalse());
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftFalse_Execute_1()
        {
            var context = new PftContext(null);
            var node = new PftFalse();
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.IsFalse(node.Value);
            Assert.AreEqual("", actual);
        }

        [TestMethod]
        public void PftFalse_PrettyPrint_1()
        {
            var printer = new PftPrettyPrinter();
            var program = new PftProgram();
            program.Children.Add(new PftFalse());
            program.PrettyPrint(printer);
            Assert.AreEqual("false ", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftFalse_ToString_1()
        {
            var node = new PftFalse();
            Assert.AreEqual("false", node.ToString());
        }

        [TestMethod]
        public void PftFalse_Value_1()
        {
            var node = new PftFalse();
            Assert.IsFalse(node.Value);
        }

        [TestMethod]
        public void PftFalse_Value_2()
        {
            var node = new PftFalse
            {
                Value = true
            };
            Assert.IsFalse(node.Value);
        }
    }
}
