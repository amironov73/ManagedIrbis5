// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM;
using AM.Text;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftCodeBlockTest
    {
        private void _Execute
            (
                PftCodeBlock node,
                string expected
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftCodeBlock_Construction_1()
        {
            var node = new PftCodeBlock();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
        }

        [TestMethod]
        public void PftCodeBlock_Construction_2()
        {
            var token = new PftToken(PftTokenKind.TripleCurly, 1, 1, "{{{");
            var node = new PftCodeBlock(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftCodeBlock_Construction_3()
        {
            var token = new PftToken(PftTokenKind.TripleCurly, 1, 1, "");
            var block = new PftCodeBlock(token);
            Assert.IsNotNull(block);
        }

        [TestMethod]
        public void PftCodeBlock_Execute_1()
        {
            var node = new PftCodeBlock();
            _Execute(node, "");
        }

        [Ignore]
        [TestMethod]
        public void PftCodeBlock_Execute_2()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!Utility.DetectAppVeyor())
            {
                var node = new PftCodeBlock
                {
                    Text = "context.Write(null, \"Hello\");"
                };
                _Execute(node, "Hello");
            }
        }

        [Ignore]
        [TestMethod]
        public void PftCodeBlock_Execute_3()
        {
            // TODO придумать что-нибудь для AppVeyor
            if (!Utility.DetectAppVeyor())
            {
                var node = new PftCodeBlock
                {
                    Text = "context.Write(null, \"Hello\")"
                };
                var context = new PftContext(null);
                node.Execute(context);
                var actual = context.Text.DosToUnix();
                Assert.IsNotNull(actual);
                Assert.IsTrue(actual!.Contains(";"));
            }
        }

        [TestMethod]
        public void PftCodeBlock_PrettyPrint_1()
        {
            var node = new PftCodeBlock
            {
                Text = "context.Write(null, \"Hello\");"
            };
            node.Children.Add(new PftUnconditionalLiteral("Hello"));
            node.Children.Add(new PftUnconditionalLiteral("world"));
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\n{{{context.Write(null, \"Hello\");}}}\n", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftCodeBlock_ToString_1()
        {
            var node = new PftCodeBlock();
            Assert.AreEqual("{{{}}}", node.ToString());
        }

        [TestMethod]
        public void PftCodeBlock_ToString_2()
        {
            var node = new PftCodeBlock
            {
                Text = "context.Write(null, \"Hello\");"
            };
            Assert.AreEqual("{{{context.Write(null, \"Hello\");}}}", node.ToString());
        }
    }
}
