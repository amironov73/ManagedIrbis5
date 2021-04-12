// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftFloorTest
    {
        private void _Execute
            (
                PftNode node,
                string expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFloor_Construction_1()
        {
            var node = new PftFloor();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftFloor_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Floor, 1, 1, "floor");
            var node = new PftFloor(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftFloor_Compile_1()
        {
            var node = new PftFloor();
            node.Children.Add(new PftNumericLiteral(123.45));
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftFloor_Compile_2()
        {
            var node = new PftFloor();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftFloor_Execute_1()
        {
            var program = new PftProgram();
            var node = new PftFloor();
            PftNumeric number = new PftNumericLiteral(123.45);
            node.Children.Add(number);
            var format = new PftF
            {
                Argument1 = node,
                Argument2 = new PftNumericLiteral(10),
                Argument3 = new PftNumericLiteral(5)
            };
            program.Children.Add(format);
            _Execute(program, " 123.00000");

            number.Value = -123.45;
            _Execute(program, "-124.00000");

            number.Value = 0.0;
            _Execute(program, "   0.00000");
        }

        [TestMethod]
        public void PftFloor_PrettyPrint_1()
        {
            var node = new PftFloor();
            node.Children.Add(new PftNumericLiteral(123.45));
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("floor(123.45)", printer.ToString());
        }

        [TestMethod]
        public void PftFloor_ToString_1()
        {
            var node = new PftFloor();
            Assert.AreEqual("floor()", node.ToString());
        }

        [TestMethod]
        public void PftFloor_ToString_2()
        {
            var node = new PftFloor();
            node.Children.Add(new PftNumericLiteral(123.45));
            Assert.AreEqual("floor(123.45)", node.ToString());
        }
    }
}
