// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftMfnTest
    {
        private void _Execute
            (
                Record? record,
                PftMfn node,
                string expected
            )
        {
            var context = new PftContext(null)
            {
                Record = record
            };
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftMfn_Construction_1()
        {
            var node = new PftMfn();
            Assert.AreEqual(PftMfn.DefaultWidth, node.Width);
        }

        [TestMethod]
        public void PftMfn_Construction_2()
        {
            const string text = "mfn(5)";
            var token = new PftToken(PftTokenKind.Mfn, 1, 1, text);
            var node = new PftMfn(token);
            Assert.AreEqual(5, node.Width);
            Assert.AreEqual(text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftMfn_Construction_2a_Exception()
        {
            const string text = "mfn(-5)";
            var token = new PftToken(PftTokenKind.Mfn, 1, 1, text);
            var node = new PftMfn(token);
            Assert.AreEqual(5, node.Width);
            Assert.AreEqual(text, node.Text);
        }

        [TestMethod]
        public void PftMfn_Construction_3()
        {
            const int width = 7;
            var node = new PftMfn(width);
            Assert.AreEqual(width, node.Width);
        }

        private void _TestSerialization
            (
                PftMfn first
            )
        {
            var program = new PftProgram();
            program.Children.Add(first);
            var bytes = PftSerializer.ToMemory(program);

            program = (PftProgram) PftSerializer.FromMemory(bytes);
            var second = (PftMfn) program.Children[0];

            Assert.AreEqual(first.Width, second.Width);
        }

        [TestMethod]
        public void PftMfn_Serialization_1()
        {
            var node = new PftMfn();
            _TestSerialization(node);

            node = new PftMfn(5);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftMfn_Execute_1()
        {
            const int mfn = 111;

            var node = new PftMfn();
            _Execute(null, node, "");

            var record = new Record { Mfn = mfn };
            _Execute(record, node, "0000000111");

            node = new PftMfn(5);
            _Execute(record, node, "00111");

            node = new PftMfn(1);
            _Execute(record, node, "111");
        }

        [TestMethod]
        public void PftMfn_PrettyPrint_1()
        {
            var printer = new PftPrettyPrinter();
            var node = new PftMfn();
            node.PrettyPrint(printer);
            Assert.AreEqual("mfn", printer.ToString());

            printer = new PftPrettyPrinter();
            node = new PftMfn(5);
            node.PrettyPrint(printer);
            Assert.AreEqual("mfn(5)", printer.ToString());
        }
    }
}
