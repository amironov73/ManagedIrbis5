// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.Diagnostics.CodeAnalysis;
using System.IO;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft;
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
    public class PftFunctionCallTest
    {
        private PftFunctionCall _GetNode()
        {
            var result = new PftFunctionCall("replace");
            result.Arguments.Add(new PftUnconditionalLiteral("Happy New Year!"));
            result.Arguments.Add(new PftUnconditionalLiteral("New Year"));
            result.Arguments.Add(new PftUnconditionalLiteral("Birthday"));

            return result;
        }

        private void _Execute
            (
                string name,
                string expected,
                params PftNode[] nodes
            )
        {
            var program = new PftProgram();
            var call = new PftFunctionCall(name);
            call.Arguments.AddRange(nodes);
            program.Children.Add(call);

            var context = new PftContext(null)
            {
                Record = new Record()
            };
            program.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFunctionCall_Construction_1()
        {
            var node = new PftFunctionCall();
            Assert.IsNull(node.Name);
            Assert.IsNotNull(node.Arguments);
            Assert.AreEqual(0, node.Arguments.Count);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
        }

        [TestMethod]
        public void PftFunctionCall_Construction_2()
        {
            var name = "name";
            var token = new PftToken(PftTokenKind.Identifier, 1, 1, name);
            var node = new PftFunctionCall(token);
            Assert.AreEqual(name, node.Name);
            Assert.IsNotNull(node.Arguments);
            Assert.AreEqual(0, node.Arguments.Count);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
        }

        [TestMethod]
        public void PftFunctionCall_Construction_3()
        {
            var name = "name";
            var node = new PftFunctionCall(name);
            Assert.AreEqual(name, node.Name);
            Assert.IsNotNull(node.Arguments);
            Assert.AreEqual(0, node.Arguments.Count);
            Assert.IsTrue(node.ExtendedSyntax);
            Assert.IsNotNull(node.Children);
            Assert.AreEqual(0, node.Children.Count);
        }

        [TestMethod]
        public void PftFunctionCall_Clone_1()
        {
            var first = new PftFunctionCall();
            var second = (PftFunctionCall) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFunctionCall_Clone_2()
        {
            var first = _GetNode();
            var second = (PftFunctionCall) first.Clone();
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSerializationException))]
        public void PftFunctionCall_Clone_3()
        {
            var first = _GetNode();
            var second = (PftFunctionCall) first.Clone();
            second.Name = "qqq";
            PftSerializationUtility.CompareNodes(first, second);
        }

        private void _TestCompile
            (
                PftFunctionCall node
            )
        {
            var provider = new NullProvider();
            var compiler = new PftCompiler();
            compiler.SetProvider(provider);
            var program = new PftProgram();
            program.Children.Add(node);
            compiler.CompileProgram(program);
        }

        [TestMethod]
        [ExpectedException(typeof(PftCompilerException))]
        public void PftFunctionCall_Compile_1()
        {
            var node = new PftFunctionCall();
            _TestCompile(node);
        }

        [TestMethod]
        public void PftFunctionCall_Compile_2()
        {
            var node = _GetNode();
            _TestCompile(node);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftFunctionCall_Execute_1()
        {
            var node = new PftFunctionCall();
            var context = new PftContext(null);
            node.Execute(context);
        }

        [TestMethod]
        public void PftFunctionCall_Execute_bold_1()
        {
            _Execute
                (
                    "bold",
                    ""
                );
            _Execute
                (
                    "bold",
                    "<b>hello</b>",
                    new PftUnconditionalLiteral("hello")
                );
            _Execute
                (
                    "bold",
                    "<b>1</b>",
                    new PftNumericLiteral(1.0)
                );
        }

        [TestMethod]
        public void PftFunctionCall_Execute_chr_1()
        {
            _Execute
                (
                    "chr",
                    ""
                );
            _Execute
                (
                    "chr",
                    "0",
                    new PftNumericLiteral(48)
                );
            _Execute
                (
                    "chr",
                    "0",
                    new PftUnconditionalLiteral("48")
                );
        }

        [TestMethod]
        public void PftFunctionCall_Execute_insert_1()
        {
            _Execute
                (
                    "insert",
                    ""
                );
            _Execute
                (
                    "insert",
                    "Happy New Year!",
                    new PftUnconditionalLiteral("Happy Year!"),
                    new PftNumericLiteral(5),
                    new PftUnconditionalLiteral(" New")
                );
        }

        [TestMethod]
        public void PftFunctionCall_Execute_italic_1()
        {
            _Execute
                (
                    "italic",
                    ""
                );
            _Execute
                (
                    "italic",
                    "<i>hello</i>",
                    new PftUnconditionalLiteral("hello")
                );
            _Execute
                (
                    "italic",
                    "<i>1</i>",
                    new PftNumericLiteral(1.0)
                );
        }

        [TestMethod]
        public void PftFunctionCall_Execute_len_1()
        {
            _Execute
                (
                    "len",
                    "0"
                );
            _Execute
                (
                    "len",
                    "5",
                    new PftUnconditionalLiteral("Hello")
                );
            _Execute
                (
                    "len",
                    "2",
                    new PftNumericLiteral(10)
                );
        }

        [TestMethod]
        public void PftFunctionCall_Execute_padLeft_1()
        {
            _Execute
                (
                    "padLeft",
                    ""
                );
            _Execute
                (
                    "padLeft",
                    "=========================Hello",
                    new PftUnconditionalLiteral("Hello"),
                    new PftNumericLiteral(30),
                    new PftUnconditionalLiteral("=")
                );
            _Execute
                (
                    "padLeft",
                    "                         Hello",
                    new PftUnconditionalLiteral("Hello"),
                    new PftNumericLiteral(30)
                );
            _Execute
                (
                    "padLeft",
                    "============================10",
                    new PftNumericLiteral(10),
                    new PftNumericLiteral(30),
                    new PftUnconditionalLiteral("=")
                );
            _Execute
                (
                    "padLeft",
                    "                            10",
                    new PftNumericLiteral(10),
                    new PftNumericLiteral(30)
                );
        }

        [TestMethod]
        public void PftFunctionCall_Execute_padRight_1()
        {
            _Execute
                (
                    "padRight",
                    ""
                );
            _Execute
                (
                    "padRight",
                    "Hello=========================",
                    new PftUnconditionalLiteral("Hello"),
                    new PftNumericLiteral(30),
                    new PftUnconditionalLiteral("=")
                );
            _Execute
                (
                    "padRight",
                    "Hello                         ",
                    new PftUnconditionalLiteral("Hello"),
                    new PftNumericLiteral(30)
                );
            _Execute
                (
                    "padRight",
                    "10============================",
                    new PftNumericLiteral(10),
                    new PftNumericLiteral(30),
                    new PftUnconditionalLiteral("=")
                );
            _Execute
                (
                    "padRight",
                    "10                            ",
                    new PftNumericLiteral(10),
                    new PftNumericLiteral(30)
                );
        }

        [TestMethod]
        public void PftFunctionCall_Execute_remove_1()
        {
            _Execute
                (
                    "remove",
                    ""
                );
            _Execute
                (
                    "remove",
                    "Happy New Year!",
                    new PftUnconditionalLiteral("Happy Twice New Year!"),
                    new PftNumericLiteral(5),
                    new PftNumericLiteral(6)
                );
        }

        [TestMethod]
        public void PftFunctionCall_Execute_replace_1()
        {
            _Execute
                (
                    "replace",
                    ""
                );
            _Execute
                (
                    "replace",
                    "Happy Birthday!",
                    new PftUnconditionalLiteral("Happy New Year!"),
                    new PftUnconditionalLiteral("New Year"),
                    new PftUnconditionalLiteral("Birthday")
                );
        }

        [TestMethod]
        public void PftFunctionCall_Execute_size_1()
        {
            _Execute
                (
                    "size",
                    "0"
                );
            _Execute
                (
                    "size",
                    "1",
                    new PftUnconditionalLiteral("Happy New Year!")
                );
        }

        [TestMethod]
        public void PftFunctionCall_GetNodeInfo_1()
        {
            var node = _GetNode();
            var info = node.GetNodeInfo();
            Assert.IsNotNull(info);
            Assert.AreEqual("FunctionCall", info.Name);
        }

        [TestMethod]
        public void PftFunctionCall_PrettyPrint_1()
        {
            var node = _GetNode();
            var printer = new PftPrettyPrinter();
            node.PrettyPrint(printer);
            Assert.AreEqual("replace('Happy New Year!', 'New Year', 'Birthday')", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                [NotNull] PftFunctionCall first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftFunctionCall) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftFunctionCall_Serialization_1()
        {
            var node = new PftFunctionCall();
            _TestSerialization(node);

            node = _GetNode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftFunctionCall_ToString_1()
        {
            var node = new PftFunctionCall();
            Assert.AreEqual("()", node.ToString());

            node = _GetNode();
            Assert.AreEqual("replace('Happy New Year!','New Year','Birthday')", node.ToString());
        }
    }
}
