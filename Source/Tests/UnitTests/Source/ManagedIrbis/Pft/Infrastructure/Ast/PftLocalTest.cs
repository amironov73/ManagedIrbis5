// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Text;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftLocalTest
    {
        private void _Execute
            (
                PftLocal node,
                string expected
            )
        {
            var context = new PftContext(null);
            context.Variables.SetVariable("x", "OldX");
            context.Variables.SetVariable("y", "OldY");
            context.Variables.SetVariable("z", "OldZ");
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        private PftLocal _GetNode()
        {
            return new PftLocal
            {
                Names = { "x", "y" },
                Children =
                {
                    new PftAssignment
                    {
                        Name = "x",
                        Children =
                        {
                            new PftUnconditionalLiteral("NewX")
                        }
                    },
                    new PftAssignment
                    {
                        Name = "y",
                        Children =
                        {
                            new PftUnconditionalLiteral("NewY")
                        }
                    },
                    new PftVariableReference("x"),
                    new PftSlash(),
                    new PftVariableReference("y"),
                    new PftSlash(),
                    new PftVariableReference("z")
                }
            };
        }

        [TestMethod]
        public void PftLocal_Construction_1()
        {
            var node = new PftLocal();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNotNull(node.Names);
            Assert.AreEqual(0, node.Names.Count);
        }

        [TestMethod]
        public void PftLocal_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Local, 1, 1, "local");
            var node = new PftLocal(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNotNull(node.Names);
            Assert.AreEqual(0, node.Names.Count);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        public void PftLocal_Clone_1()
        {
            var first = new PftLocal();
            var second = (PftLocal)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLocal_Clone_2()
        {
            var first = _GetNode();
            var second = (PftLocal)first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLocal_Execute_1()
        {
            var node = new PftLocal();
            _Execute(node, "");
        }

        [TestMethod]
        public void PftLocal_Execute_2()
        {
            var node = _GetNode();
            _Execute(node, "NewX\nNewY\nOldZ");
        }

        [TestMethod]
        public void PftLocal_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.AreSame(node, info.Node);
            Assert.AreEqual("Local", info.Name);
        }

        [TestMethod]
        public void PftLocal_PrettyPrint_1()
        {
            var node = new PftLocal();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("\nlocal \ndo\nend\n", printer.ToString().DosToUnix());
        }

        [TestMethod]
        public void PftLocal_PrettyPrint_2()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual
                (
                    "\nlocal $x, $y\ndo\n  $x='NewX'; $y='NewY';$x / $y / $z\nend\n",
                    printer.ToString().DosToUnix()
                );
        }

        private void _TestSerialization
            (
                PftLocal first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftLocal)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftLocal_Serialization_1()
        {
            var node = new PftLocal();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftLocal_ToString_1()
        {
            var node = new PftLocal();
            Assert.AreEqual("local  do  end", node.ToString());

            node = _GetNode();
            Assert.AreEqual("local $x, $y do $x='NewX'; $y='NewY'; $x / $y / $z end", node.ToString());
        }
    }
}
