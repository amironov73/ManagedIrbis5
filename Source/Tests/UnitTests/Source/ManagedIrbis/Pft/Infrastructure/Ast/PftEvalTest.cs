// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftEvalTest
    {
        private void _Execute
            (
                PftEval node,
                string expected
            )
        {
            var context = new PftContext(null);
            var record = new Record();
            record.Fields.Add(new Field { Tag = 100, Value = "field100" });
            context.Record = record;
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftEval_Construction_1()
        {
            var node = new PftEval();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftEval_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Eval, 1, 1, "eval");
            var node = new PftEval(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftEval_Execute_1()
        {
            var node = new PftEval();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftEval_Execute_2()
        {
            var node = new PftEval(new PftUnconditionalLiteral("v100"));
            _Execute(node, "field100");
        }

        [TestMethod]
        public void PftEval_PrettyPrint_1()
        {
            var node = new PftEval();
            node.Children.Add(new PftUnconditionalLiteral("v100"));
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("eval('v100')", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftEval_ToString_1()
        {
            var node = new PftEval();
            Assert.AreEqual("eval()", node.ToString());
        }

        [TestMethod]
        public void PftEval_ToString_2()
        {
            var node = new PftEval();
            node.Children.Add(new PftUnconditionalLiteral("v100"));
            Assert.AreEqual("eval('v100')", node.ToString());
        }
    }
}
