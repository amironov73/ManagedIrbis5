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
    public class PftEatTest
    {
        private PftProgram _GetProgram()
        {
            var result = new PftProgram();
            result.Children.Add(new PftUnconditionalLiteral("Hello, "));
            var eat = new PftEat();
            result.Children.Add(eat);
            eat.Children.Add(new PftUnconditionalLiteral("new "));
            result.Children.Add(new PftUnconditionalLiteral("world!"));

            return result;
        }

        private void _Execute
            (
                PftProgram program,
                string expected
            )
        {
            var context = new PftContext(null);
            program.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftEat_Construction_1()
        {
            var node = new PftEat();
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftEat_Construction_2()
        {
            var token = new PftToken(PftTokenKind.EatOpen, 1, 1, ",");
            var node = new PftEat(token);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftEat_Compile_1()
        {
            var compiler = new PftCompiler();
            var program = _GetProgram();
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftEat_Execute_1()
        {
            var program = _GetProgram();
            _Execute(program, "Hello, world!");
        }

        [TestMethod]
        public void PftEat_PrettyPrint_1()
        {
            var printer = new PftPrettyPrinter();
            var program = _GetProgram();
            program.PrettyPrint(printer);
            Assert.AreEqual("'Hello, '[[['new ']]] 'world!'", printer.ToString().DosToUnix());
        }
    }
}
