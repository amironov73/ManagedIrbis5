// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

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
    public class PftGraveAccentTest
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
        public void PftGraveAccent_Construction_1()
        {
            var node = new PftGraveAccent();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsNull(node.Text);
        }

        [TestMethod]
        public void PftGraveAccent_Construction_2()
        {
            var text = "text";
            var node = new PftGraveAccent(text);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreSame(text, node.Text);
        }

        [TestMethod]
        public void PftGraveAccent_Construction_3()
        {
            var text = "text";
            var token = new PftToken(PftTokenKind.GraveAccent, 1, 1, text);
            var node = new PftGraveAccent(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreSame(text, node.Text);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftGraveAccent_Construction_4()
        {
            string? text = null;
            var token = new PftToken(PftTokenKind.GraveAccent, 1, 1, text);
            Assert.IsNull(new PftGraveAccent(token));
        }

        [TestMethod]
        public void PftGraveAccent_Compile_1()
        {
            var node = new PftGraveAccent("text");
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftGraveAccent_Execute_1()
        {
            var text = "text";
            var node = new PftGraveAccent(text);
            _Execute(node, text);
        }

        [TestMethod]
        public void PftGraveAccent_Execute_2()
        {
            var program = new PftProgram();
            program.Children.Add(new PftMode("mpu"));
            var text = "text";
            var node = new PftGraveAccent(text);
            program.Children.Add(node);
            _Execute(program, "TEXT");
        }

        [TestMethod]
        public void PftGraveAccent_Optimize_1()
        {
            var expected = new PftGraveAccent("text");
            var actual = expected.Optimize();
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void PftGraveAccent_PrettyPrint_1()
        {
            var node = new PftGraveAccent("text");
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("`text`", printer.ToString());
        }

        [TestMethod]
        public void PftGraveAccent_ToString_1()
        {
            var node = new PftGraveAccent("text");
            Assert.AreEqual("`text`", node.ToString());
        }
    }
}
