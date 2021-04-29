// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftBangTest
    {
        private void _Execute
            (
                PftBang node
            )
        {
            var flag = false;
            var context = new PftContext(null);
            var mock = new Mock<PftDebugger>(context);
            var debugger = mock.Object;

            mock.Setup(d => d.Activate(It.IsAny<PftDebugEventArgs>()))
                .Callback(() => { flag = true; });

            context.Debugger = debugger;
            node.Execute(context);
            Assert.IsTrue(flag);
        }

        [TestMethod]
        public void PftBang_Construction_1()
        {
            var node = new PftBang();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftBang_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Bang, 1, 1, "!");
            var node = new PftBang(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftBang_Compile_1()
        {
            var node = new PftBang();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftBang_Execute_1()
        {
            var node = new PftBang();
            _Execute(node);
        }

        [TestMethod]
        public void PftBang_PrettyPrint_1()
        {
            var node = new PftBang();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("! ", printer.ToString());
        }

        [TestMethod]
        public void PftBang_ToString_1()
        {
            var node = new PftBang();
            Assert.AreEqual("!", node.ToString());
        }
    }
}
