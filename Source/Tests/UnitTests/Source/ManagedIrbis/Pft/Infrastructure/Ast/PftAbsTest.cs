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
    public class PftAbsTest
        : Common.CommonUnitTest
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
        [Description("Конструктор по умолчанию")]
        public void PftAbs_Construction_1()
        {
            var node = new PftAbs();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        [Description("Конструктор с токеном")]
        public void PftAbs_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Abs, 1, 1, "abs");
            var node = new PftAbs(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [Description("Успешная компиляция ноды")]
        public void PftAbs_Compile_1()
        {
            var node = new PftAbs();
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
        [Description("Компиляция ноды с ошибкой синтаксиса")]
        public void PftAbs_Compile_2()
        {
            var node = new PftAbs();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftAbs_Execute_1()
        {
            var program = new PftProgram();
            var node = new PftAbs();
            PftNumeric number = new PftNumericLiteral(123.45);
            node.Children.Add(number);
            var format = new PftF
            {
                Argument1 = node,
                Argument2 = new PftNumericLiteral(9),
                Argument3 = new PftNumericLiteral(5)
            };
            program.Children.Add(format);
            _Execute(program, "123.45000");

            number.Value = -123.45;
            _Execute(program, "123.45000");

            number.Value = 0.0;
            _Execute(program, "  0.00000");
        }

        [TestMethod]
        public void PftAbs_PrettyPrint_1()
        {
            var node = new PftAbs();
            node.Children.Add(new PftNumericLiteral(123.45));
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("abs(123.45)", printer.ToString());
        }

        [TestMethod]
        public void PftAbs_ToString_1()
        {
            var node = new PftAbs();
            Assert.AreEqual("abs()", node.ToString());
        }

        [TestMethod]
        public void PftAbs_ToString_2()
        {
            var node = new PftAbs();
            node.Children.Add(new PftNumericLiteral(123.45));
            Assert.AreEqual("abs(123.45)", node.ToString());
        }
    }
}
