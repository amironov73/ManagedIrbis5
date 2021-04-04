// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Ast
{
    [TestClass]
    public class PftCTest
    {
        [TestMethod]
        public void PftC_Construction_1()
        {
            var node = new PftC();
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(0, node.NewPosition);
        }

        [TestMethod]
        public void PftC_Construction_2()
        {
            var token = new PftToken(PftTokenKind.C, 1, 1, "5");
            var node = new PftC(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
            Assert.AreEqual(5, node.NewPosition);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSyntaxException))]
        public void PftC_Construction_2a_Exception()
        {
            var token = new PftToken(PftTokenKind.C, 1, 1, "q");
            var node = new PftC(token);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(token.Column, node.Column);
            Assert.AreEqual(token.Line, node.LineNumber);
            Assert.AreEqual(token.Text, node.Text);
            Assert.AreEqual(5, node.NewPosition);
        }

        [TestMethod]
        public void PftC_Construction_3()
        {
            var node = new PftC(5);
            Assert.IsTrue(node.ConstantExpression);
            Assert.IsFalse(node.ExtendedSyntax);
            Assert.IsFalse(node.RequiresConnection);
            Assert.AreEqual(5, node.NewPosition);
        }

        [TestMethod]
        public void PftC_Compile_1()
        {
            var compiler = new PftCompiler();
            var program = new PftProgram();
            program.Children.Add(new PftC(5));
            compiler.CompileProgram(program);
        }

        [TestMethod]
        public void PftC_Execute_1()
        {
            var context = new PftContext(null);
            var node = new PftC(5);
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual("    ", actual);
        }

        [TestMethod]
        public void PftC_Execute_2()
        {
            var record = new Record();
            record.Fields.Add(new Field { Tag = 1, Value = "Hello" });
            var context = new PftContext(null)
            {
                Record = record
            };
            var v = new PftV("v1");
            var node = new PftC(5);
            v.LeftHand.Add(node);
            v.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual("    Hello", actual);
        }

        [TestMethod]
        public void PftC_Execute_3()
        {
            var context = new PftContext(null);
            context.Write(null, "=====");
            var node = new PftC(3);
            node.Execute(context);
            var actual = context.Text.DosToUnix();
            Assert.AreEqual("=====\n  ", actual);
        }

        [TestMethod]
        public void PftC_PrettyPrint_1()
        {
            var printer = new PftPrettyPrinter();
            var program = new PftProgram();
            program.Children.Add(new PftC(5));
            program.PrettyPrint(printer);
            Assert.AreEqual("c5 ", printer.ToString().DosToUnix());
        }

        private void _TestSerialization
            (
                PftC first
            )
        {
            var ethalon = new PftProgram();
            ethalon.Children.Add(first);
            var bytes = PftSerializer.ToMemory(ethalon);

            var deserialized = (PftProgram)PftSerializer.FromMemory(bytes);
            var second = (PftC)deserialized.Children[0];

            Assert.AreEqual(first.NewPosition, second.NewPosition);

            PftSerializationUtility.VerifyDeserializedProgram
                (
                    ethalon,
                    deserialized
                );

            try
            {
                second.NewPosition = 6;
                PftSerializationUtility.VerifyDeserializedProgram
                    (
                        ethalon,
                        deserialized
                    );
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void PftC_Serialization_1()
        {
            var node = new PftC();
            _TestSerialization(node);

            node = new PftC(5);
            _TestSerialization(node);
        }

        [TestMethod]
        public void PftC_NewPosition_1()
        {
            var node = new PftC(5);
            Assert.AreEqual(5, node.NewPosition);

            node.NewPosition = 6;
            Assert.AreEqual(6, node.NewPosition);
        }

        [TestMethod]
        public void PftTrue_ToString_1()
        {
            var node = new PftC(5);
            Assert.AreEqual("c5", node.ToString());
        }
    }
}
