// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Text;

using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftLTest
        : Common.CommonUnitTest
    {
        private void _Execute
            (
                PftL node,
                double expected
            )
        {
            using (var provider = GetProvider())
            {
                var context = new PftContext(null);
                context.SetProvider(provider);
                node.Execute(context);
            }

            var actual = node.Value;
            Assert.AreEqual(expected, actual);
        }

        private PftL _GetNode()
        {
            return new PftL
            {
                Children =
                {
                    new PftUnconditionalLiteral("K=ATLAS")
                }
            };
        }

        [TestMethod]
        public void PftL_Construction_1()
        {
            var node = new PftL();
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(0.0, node.Value);
        }

        [TestMethod]
        public void PftL_Construction_2()
        {
            var token = new PftToken(PftTokenKind.L, 1, 1, "l");
            var node = new PftL(token);
            Assert.IsFalse(node.ConstantExpression);
            Assert.IsTrue(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(0.0, node.Value);
        }

        [TestMethod]
        public void PftL_Compile_1()
        {
            var node = _GetNode();
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftL_Execute_1()
        {
            var node = new PftL();
            _Execute(node, 0);
        }

        [Ignore]
        [TestMethod]
        public void PftL_Execute_2()
        {
            var node = _GetNode();
            _Execute(node, 27);
        }

        [TestMethod]
        public void PftL_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("l('K=ATLAS')", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftL first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftL)PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftRef_Serialization_1()
        {
            var node = new PftL();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftL_ToString_1()
        {
            var node = new PftL();
            Assert.AreEqual("l()", node.ToString());

            node = _GetNode();
            Assert.AreEqual("l('K=ATLAS')", node.ToString());
        }
    }
}
