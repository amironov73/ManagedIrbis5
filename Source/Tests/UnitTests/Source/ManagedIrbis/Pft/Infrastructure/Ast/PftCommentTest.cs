// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

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
    public class PftCommentTest
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
        public void PftComment_Construction_1()
        {
            var node = new PftComment();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftComment_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Comment, 1, 1, "/*");
            var node = new PftComment(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftComment_Construction_3()
        {
            var text = "text";
            var node = new PftComment(text);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreSame(text, node.Text);
        }

        [TestMethod]
        public void PftComment_Compile_1()
        {
            var node = new PftComment("text");
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftComment_Execute_1()
        {
            var program = new PftProgram();
            program.Children.Add(new PftUnconditionalLiteral("Hello, "));
            var node = new PftComment("text");
            program.Children.Add(new PftUnconditionalLiteral("world!"));
            program.Children.Add(node);
            _Execute(program, "Hello, world!");
        }

        [TestMethod]
        public void PftComment_Optimize_1()
        {
            var comment = new PftComment("text");
            Assert.IsNull(comment.Optimize());
        }

        [TestMethod]
        public void PftComment_PrettyPrint_1()
        {
            var program = new PftNode();
            program.Children.Add(new PftUnconditionalLiteral("Hello,"));
            program.Children.Add(new PftComment("comment"));
            program.Children.Add(new PftUnconditionalLiteral("world"));
            var printer = new PftPrettyPrinter();
            program.PrettyPrint(printer);
            Assert.AreEqual("'Hello,'/* comment\n'world'", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftComment_ToString_1()
        {
            var node = new PftComment("Hello");
            Assert.AreEqual("/* Hello\n", node.ToString().DosToUnix());
        }
    }
}

