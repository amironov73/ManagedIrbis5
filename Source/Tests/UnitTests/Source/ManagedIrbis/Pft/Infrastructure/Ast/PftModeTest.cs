// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftModeTest
    {
        private void _Execute
            (
                PftMode node,
                PftFieldOutputMode mode,
                bool upper
            )
        {
            var context = new PftContext(null);
            node.Execute(context);
            Assert.AreEqual(mode, context.FieldOutputMode);
            Assert.AreEqual(upper, context.UpperMode);
        }

        [TestMethod]
        public void PftMode_Construction_1()
        {
            var node = new PftMode();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(PftFieldOutputMode.PreviewMode, node.OutputMode);
            Assert.IsFalse(node.UpperMode);
        }

        [TestMethod]
        public void PftMode_Construction_2()
        {
            var token = new PftToken(PftTokenKind.Mpl, 1, 1, "mpl");
            var node = new PftMode(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(PftFieldOutputMode.PreviewMode, node.OutputMode);
            Assert.IsFalse(node.UpperMode);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftMode_Construction_3()
        {
            var token = new PftToken(PftTokenKind.Mpl, 1, 1, "@");
            Assert.IsNotNull(new PftMode(token));
        }

        [TestMethod]
        public void PftMode_Construction_4()
        {
            var node = new PftMode("mdu");
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.RequiresConnection);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.AreEqual(PftFieldOutputMode.DataMode, node.OutputMode);
            Assert.IsTrue(node.UpperMode);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftMode_Construction_5()
        {
            Assert.IsNotNull(new PftMode("@"));
        }

        private void _TestCompile
            (
                PftMode node
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
        public void PftMode_Compile_1()
        {
            var node = new PftMode();
            _TestCompile(node);
        }

        [TestMethod]
        public void PftMode_Execute_1()
        {
            var node = new PftMode();
            _Execute(node, PftFieldOutputMode.PreviewMode, false);

            node = new PftMode("mhu");
            _Execute(node, PftFieldOutputMode.HeaderMode, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftMode_ParseText_1()
        {
            var mode = new PftMode();
            mode.ParseText("@");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftMode_ParseText_2()
        {
            var mode = new PftMode();
            mode.ParseText("mqq");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PftMode_ParseText_3()
        {
            var mode = new PftMode();
            mode.ParseText("mpq");
        }

        [TestMethod]
        public void PftMode_ParseText_4()
        {
            var mode = new PftMode();
            mode.ParseText("MHU");
            Assert.AreEqual(PftFieldOutputMode.HeaderMode, mode.OutputMode);
            Assert.IsTrue(mode.UpperMode);

            mode.ParseText("MDL");
            Assert.AreEqual(PftFieldOutputMode.DataMode, mode.OutputMode);
            Assert.IsFalse(mode.UpperMode);
        }

        private void _TestSerialization
            (
                PftMode first
            )
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            PftSerializer.Serialize(writer, first);

            var bytes = stream.ToArray();
            stream = new MemoryStream(bytes);
            var reader = new BinaryReader(stream);
            var second = (PftMode) PftSerializer.Deserialize(reader);
            PftSerializationUtility.CompareNodes(first, second);
        }

        [TestMethod]
        public void PftMode_Serialization_1()
        {
            var node = new PftMode();
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftMode_ToString_1()
        {
            var node = new PftMode();
            Assert.AreEqual("mpl", node.ToString());

            node = new PftMode
            {
                OutputMode = PftFieldOutputMode.HeaderMode,
                UpperMode = true
            };
            Assert.AreEqual("mhu", node.ToString());

            node = new PftMode
            {
                OutputMode = PftFieldOutputMode.DataMode,
                UpperMode = false
            };
            Assert.AreEqual("mdl", node.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void PftMode_ToString_2()
        {
            var node = new PftMode
            {
                OutputMode = (PftFieldOutputMode)'q',
                UpperMode = true
            };
            Assert.IsNotNull(node.ToString());
        }
    }
}
